using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SnippetCache.Data.EF4.Model;
using SnippetCache.Data.Wcf.Requests;
using SnippetCache.Data.Wcf.Responses;
using SnippetCache.Utils.Logging;
using SnippetCache.Utils.Validation;

namespace SnippetCache.Data.Wcf
{
    public partial class SnippetCacheDataService
    {
        #region User Operations

        public GetAllUsersResponse GetAllUsers(GetAllUsersRequest request)
        {
            var response = new GetAllUsersResponse();

            try
            {
                Guard.ArgNotNull(request, "request");

                User[] results = _unitOfWork.UserRepository.Get(d => d.Id > 0).ToArray();
                Guard.ArgNotNull(results, "results");

                var quickInfoResults = new List<UserQuickInfo>(results.Count());
                quickInfoResults.AddRange(results.Select(
                    user => new UserQuickInfo
                                {
                                    Id = user.Id,
                                    FormsAuthId = user.FormsAuthId,
                                    Email = user.Email,
                                    LoginName = user.LoginName
                                }));
                response.Users = quickInfoResults.AsEnumerable();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetAllUsers Method Failed", ex);
            }

            return response;
        }

        public GetUserByIdResponse GetUserById(GetUserByIdRequest request)
        {
            var response = new GetUserByIdResponse();
            try
            {
                Guard.ArgNotNull(_unitOfWork.UserRepository, "UserRepository");
                User user = _unitOfWork.UserRepository.GetById(request.Id);

                Guard.ArgNotNull(user, "user");

                response.Id = request.Id;
                response.FormsAuthId = user.FormsAuthId;
                response.Email = user.Email;
                response.LoginName = user.LoginName;
                response.AvatarImage = user.AvatarImage;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetUserById Method Failed", ex);
            }

            return response;
        }

        public GetUserByNameResponse GetUserByName(GetUserByNameRequest request)
        {
            var response = new GetUserByNameResponse();

            try
            {
                Guard.ArgNotNull(request, "request");
                User user = _unitOfWork.UserRepository.Get(d => d.LoginName.Equals(request.LoginName)).FirstOrDefault();

                Guard.ArgNotNull(user, "user");

                response.Id = user.Id;
                response.FormsAuthId = user.FormsAuthId;
                response.LoginName = user.LoginName;
                response.Email = user.Email;
                response.AvatarImage = user.AvatarImage;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetUserByName Method Failed", ex);
            }

            return response;
        }

        // Create
        public CreateNewUserResponse CreateNewUser(CreateNewUserRequest request)
        {
            var response = new CreateNewUserResponse();
            try
            {
                Guard.ArgNotNull(_unitOfWork.UserRepository, "UserRepository");

                User orDefault =
                    _unitOfWork.UserRepository.Get(
                        d => d.LoginName == request.LoginName || d.FormsAuthId == request.FormsAuthId).FirstOrDefault();
                if (orDefault != null)
                {
                    int existingId =
                        orDefault.Id;
                    if (existingId > 0)
                        throw new DuplicateNameException("A user with the specified LoginName already exists", null);
                }

                var user = new User
                               {
                                   FormsAuthId = request.FormsAuthId,
                                   LoginName = request.LoginName,
                                   Email = request.Email,
                                   AvatarImage = request.AvatarImage
                               };

                _unitOfWork.UserRepository.Insert(user);
                _unitOfWork.Save();

                User firstOrDefault =
                    _unitOfWork.UserRepository.Get(d => d.LoginName == request.LoginName).FirstOrDefault();
                if (firstOrDefault != null)
                {
                    int newId = firstOrDefault.Id;

                    if (newId > 0)
                    {
                        response.Success = true;
                        response.Id = newId;
                        response.FormsAuthId = request.FormsAuthId;
                        response.LoginName = request.LoginName;
                        response.Email = request.Email;
                        Logger.LogInfo("Successfully created User Id: " + newId.ToString(), LogType.General);
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("CreateNewUser Method Failed", ex);
            }

            return response;
        }

        // Update
        public UpdateUserResponse UpdateUser(UpdateUserRequest request)
        {
            var response = new UpdateUserResponse();
            try
            {
                Guard.ArgNotNull(request, "request");
                User user = _unitOfWork.UserRepository.GetById(request.Id);

                Guard.ArgNotNull(user, "user");
                user.LoginName = request.LoginName;
                user.Email = request.Email;
                user.AvatarImage = request.AvatarImage;

                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();

                response.Success = true;
                Logger.LogInfo("Successfully updated User Id: " + request.Id.ToString(), LogType.General);


                if (!response.Success)
                {
                    string errorString = "Unknown failure updating User Id: " + request.Id.ToString();
                    response.FailureInformation = errorString;
                    Logger.LogWarning(errorString, null);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("UpdateUser method failed", ex);
            }

            return response;
        }

        // Delete
        public DeleteUserResponse DeleteUser(DeleteUserRequest request)
        {
            var response = new DeleteUserResponse();
            try
            {
                Guard.ArgNotNull(request, "request");
                User user = _unitOfWork.UserRepository.GetById(request.Id);

                Guard.ArgNotNull(user, "user");

                _unitOfWork.UserRepository.Delete(user);
                _unitOfWork.Save();

                response.Success = true;
                Logger.LogInfo("Successfully deleted User Id: " + request.Id.ToString(), LogType.General);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("DeleteUser method failed", ex);
            }

            return response;
        }

        #endregion
    }
}