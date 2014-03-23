using System;
using System.Collections.Generic;
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
        #region NotificationType Operations

        public GetAllNotificationTypesResponse GetAllNotificationTypes(GetAllNotificationTypesRequest request)
        {
            var response = new GetAllNotificationTypesResponse();

            try
            {
                Guard.ArgNotNull(request, "request");

                NotificationType[] results = _unitOfWork.NotificationTypeRepository.Get(d => d.Id > 0).ToArray();
                Guard.ArgNotNull(results, "results");

                var quickInfoResults = new List<NotificationTypeQuickInfo>(results.Count());
                quickInfoResults.AddRange(results.Select(
                    notificationType => new NotificationTypeQuickInfo
                                            {
                                                Id = notificationType.Id,
                                                Name = notificationType.Name,
                                                Description = notificationType.Description
                                            }));
                response.Success = true;
                response.NotificationTypes = quickInfoResults.AsEnumerable();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetAllNotificationTypes Method Failed", ex);
            }

            return response;
        }

        public GetNotificationTypesByIdResponse GetNotificationTypeById(GetNotificationTypesByIdRequest request)
        {
            var response = new GetNotificationTypesByIdResponse();
            try
            {
                Guard.ArgNotNull(_unitOfWork.NotificationTypeRepository, "_notificationType");
                NotificationType notificationType = _unitOfWork.NotificationTypeRepository.GetById(request.Id);

                Guard.ArgNotNull(notificationType, "notificationType");

                response.Id = notificationType.Id;
                response.Name = notificationType.Name;
                response.Description = notificationType.Description;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetNotificationTypeById Method Failed", ex);
            }

            return response;
        }

        public GetNotificationTypesByNameResponse GetNotificationTypeByName(GetNotificationTypesByNameRequest request)
        {
            var response = new GetNotificationTypesByNameResponse();

            try
            {
                Guard.ArgNotNull(request, "request");
                NotificationType notificationType =
                    _unitOfWork.NotificationTypeRepository.Get(d => d.Name == request.Name).FirstOrDefault();

                Guard.ArgNotNull(notificationType, "user");

                response.Id = notificationType.Id;
                response.Name = notificationType.Name;
                response.Description = notificationType.Description;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetNotificationTypeByName Method Failed", ex);
            }

            return response;
        }

        // Create
        public CreateNewNotificationTypesResponse CreateNewNotificationType(CreateNewNotificationTypesRequest request)
        {
            var response = new CreateNewNotificationTypesResponse();
            try
            {
                Guard.ArgNotNull(_unitOfWork.NotificationTypeRepository, "_notificationType");

                var notificationType = new NotificationType
                                           {
                                               Name = request.Name,
                                               Description = request.Description
                                           };
                _unitOfWork.NotificationTypeRepository.Insert(notificationType);
                _unitOfWork.Save();

                int newId = notificationType.Id;
                if (newId > 0)
                {
                    response.Success = true;
                    response.Id = newId;
                    response.Name = notificationType.Name;
                    response.Description = notificationType.Description;
                    Logger.LogInfo("Successfully created NotificationType Id: " + newId.ToString(), LogType.General);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("CreateNewNotificationType Method Failed", ex);
            }

            return response;
        }

        // Update
        public UpdateNotificationTypesResponse UpdateNotificationType(UpdateNotificationTypesRequest request)
        {
            var response = new UpdateNotificationTypesResponse();
            try
            {
                Guard.ArgNotNull(request, "request");
                NotificationType notificationType = _unitOfWork.NotificationTypeRepository.GetById(request.Id);

                Guard.ArgNotNull(notificationType, "notificationType");
                notificationType.Name = request.Name;
                notificationType.Description = request.Description;

                _unitOfWork.NotificationTypeRepository.Update(notificationType);
                _unitOfWork.Save();

                response.Success = true;
                Logger.LogInfo("Successfully updated NotificationType Id: " + request.Id.ToString(), LogType.General);


                if (!response.Success)
                {
                    string errorString = "Unknown failure updating NotificationType Id: " + request.Id.ToString();
                    response.FailureInformation = errorString;
                    Logger.LogWarning(errorString, null);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("UpdateNotificationType method failed", ex);
            }

            return response;
        }

        // Delete
        public DeleteNotificationTypesResponse DeleteNotificationType(DeleteNotificationTypesRequest request)
        {
            var response = new DeleteNotificationTypesResponse();
            try
            {
                Guard.ArgNotNull(request, "request");
                NotificationType notificationType = _unitOfWork.NotificationTypeRepository.GetById(request.Id);

                Guard.ArgNotNull(notificationType, "notificationType");

                _unitOfWork.NotificationTypeRepository.Delete(notificationType);
                _unitOfWork.Save();

                response.Success = true;
                Logger.LogInfo("Successfully deleted NotificationType Id: " + request.Id.ToString(), LogType.General);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("DeleteNotificationType method failed", ex);
            }

            return response;
        }

        #endregion
    }
}