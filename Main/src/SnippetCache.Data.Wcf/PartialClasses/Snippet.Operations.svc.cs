using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnippetCache.Data.DTO;
using SnippetCache.Data.EF4.Model;
using SnippetCache.Data.Wcf.Requests;
using SnippetCache.Data.Wcf.Responses;
using SnippetCache.Utils.Logging;
using SnippetCache.Utils.Validation;

namespace SnippetCache.Data.Wcf
{
    public partial class SnippetCacheDataService
    {
        #region ISnippetCacheDataService Members

        public GetPagedSnippetListResponse GetPagedSnippetList(GetPagedSnippetListRequest request)
        {
            var response = new GetPagedSnippetListResponse();

            try
            {
                int skipCount = 0;
                int takeCount = request.SnippetsPerPage;

                if (request.CurrentPage == 0) request.CurrentPage = 1;
                if (request.CurrentPage == 1)
                    takeCount = request.SnippetsPerPage;
                else
                {
                    skipCount = (request.CurrentPage - 1)*request.SnippetsPerPage;
                    takeCount = request.SnippetsPerPage;
                }

                List<Snippet> snippets =
                    _unitOfWork.SnippetRepository.Get(
                        d => d.Id > 0
                             && ((request.IncludePrivate) ? d.IsPublic == (true | false) : d.IsPublic)
                             &&
                             (!(request.IncludePrivate) || (d.User_Id == request.UserId && d.Guid == request.UserGuid)))
                        .OrderByDescending(d => d.LastModified)
                        .Skip(skipCount).Take(takeCount).ToList();

                response.Snippets = new List<SnippetDTO>();
                foreach (Snippet snippet in snippets)
                {
                    var temp = new SnippetDTO
                                   {
                                       Id = snippet.Id,
                                       Data = snippet.Data,
                                       Description = snippet.Description,
                                       Guid = snippet.Guid,
                                       IsPublic = snippet.IsPublic,
                                       Name = snippet.Name,
                                       Language_Id = snippet.Language_Id,
                                       PreviewData = snippet.PreviewData,
                                       LastModified = snippet.LastModified,
                                       User_FormsAuthId = snippet.User_FormsAuthId,
                                       User_Id = snippet.User_Id
                                   };
                    ((List<SnippetDTO>) response.Snippets).Add(temp);
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetPagedSnippetList method failed", ex);
            }

            return response;
        }

        public GetSnippetByIdResponse GetSnippetById(GetSnippetByIdRequest request)
        {
            var response = new GetSnippetByIdResponse();

            try
            {
                Snippet snippet = _unitOfWork.SnippetRepository.GetById(request.Id);
                response.Snippet = new SnippetDTO
                                       {
                                           Id = snippet.Id,
                                           Guid = snippet.Guid,
                                           Name = snippet.Name,
                                           Description = snippet.Description,
                                           PreviewData = snippet.PreviewData,
                                           Data = snippet.PreviewData,
                                           LastModified = snippet.LastModified,
                                           IsPublic = snippet.IsPublic,
                                           Language_Id = snippet.Language_Id,
                                           User_Id = snippet.User_Id,
                                           User_FormsAuthId = snippet.User_FormsAuthId
                                       };
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("CreateSnippet Method Failed", ex);
            }
            return response;
        }

        public GetSnippetByGuidResponse GetSnippetByGuid(GetSnippetByGuidRequest request)
        {
            var response = new GetSnippetByGuidResponse();

            try
            {
                Snippet returnValue = _unitOfWork.SnippetRepository.Get(d => d.Guid == request.Guid).First();
                if (returnValue != null)
                {
                    var dto = new SnippetDTO
                                  {
                                      Id = returnValue.Id,
                                      Guid = returnValue.Guid,
                                      Name = returnValue.Name,
                                      Description = returnValue.Description,
                                      PreviewData = returnValue.PreviewData,
                                      Data = returnValue.Data,
                                      IsPublic = returnValue.IsPublic,
                                      LastModified = returnValue.LastModified,
                                      Language_Id = returnValue.Language_Id,
                                      User_FormsAuthId = returnValue.User_FormsAuthId,
                                      User_Id = returnValue.User_Id
                                  };
                    response.Snippet = dto;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetSnippetByGuid Method Failed", ex);
            }

            return response;
        }

        public GetSnippetsByUserIdResponse GetSnippetsByUserId(GetSnippetsByUserIdRequest request)
        {
            var response = new GetSnippetsByUserIdResponse();
            var returnList = new List<SnippetDTO>();

            try
            {
                Guard.ArgNotNull(_unitOfWork.SnippetRepository, "SnippetRepository");
                Snippet[] snippets =
                    _unitOfWork.SnippetRepository.Get(
                        d => d.User_Id == request.UserId && d.User_FormsAuthId == request.User_FormsAuthId).ToArray();

                Guard.ArgNotNull(snippets, "snippets");
                if (snippets.Any())
                {
                    returnList.AddRange(snippets.Select(snippet => new SnippetDTO
                                                                       {
                                                                           Id = snippet.Id,
                                                                           Guid = snippet.Guid,
                                                                           Name = snippet.Name,
                                                                           Description = snippet.Description,
                                                                           PreviewData = snippet.PreviewData,
                                                                           Data = snippet.PreviewData,
                                                                           LastModified = snippet.LastModified,
                                                                           IsPublic = snippet.IsPublic,
                                                                           Language_Id = snippet.Language_Id,
                                                                           User_Id = snippet.User_Id,
                                                                           User_FormsAuthId = snippet.User_FormsAuthId
                                                                       }));
                    response.Snippets = returnList;
                }

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

        public GetSnippetsByTitleResponse GetSnippetsByTitle(GetSnippetsByTitleRequest request)
        {
            var response = new GetSnippetsByTitleResponse();
            var returnList = new List<SnippetDTO>();

            try
            {
                Guard.ArgNotNull(_unitOfWork.SnippetRepository, "SnippetRepository");
                Snippet[] snippets =
                    _unitOfWork.SnippetRepository.Get(d => d.Name.Contains(request.TitleText) && d.IsPublic).ToArray();
                Guard.ArgNotNull(snippets, "snippets");
                if (snippets.Any())
                {
                    returnList.AddRange(snippets.Select(snippet => new SnippetDTO
                                                                       {
                                                                           Id = snippet.Id,
                                                                           Guid = snippet.Guid,
                                                                           Name = snippet.Name,
                                                                           Description = snippet.Description,
                                                                           PreviewData = snippet.PreviewData,
                                                                           Data = snippet.PreviewData,
                                                                           LastModified = snippet.LastModified,
                                                                           IsPublic = snippet.IsPublic,
                                                                           Language_Id = snippet.Language_Id,
                                                                           User_Id = snippet.User_Id,
                                                                           User_FormsAuthId = snippet.User_FormsAuthId
                                                                       }));

                    response.Snippets = returnList.AsEnumerable();
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetSnippetsByTitle Method Failed", ex);
            }

            return response;
        }

        public GetSnippetsByDescriptionResponse GetSnippetsByDescription(GetSnippetsByDescriptionRequest request)
        {
            var response = new GetSnippetsByDescriptionResponse();
            var returnList = new List<SnippetDTO>();

            try
            {
                Guard.ArgNotNull(_unitOfWork.SnippetRepository, "SnippetRepository");
                Snippet[] snippets = _unitOfWork.SnippetRepository.Get(
                    d => d.Description.Contains(request.DescriptionText) && d.IsPublic).ToArray();

                Guard.ArgNotNull(snippets, "snippets");
                if (snippets.Any())
                {
                    returnList.AddRange(snippets.Select(snippet => new SnippetDTO
                                                                       {
                                                                           Id = snippet.Id,
                                                                           Guid = snippet.Guid,
                                                                           Name = snippet.Name,
                                                                           Description = snippet.Description,
                                                                           PreviewData = snippet.PreviewData,
                                                                           Data = snippet.PreviewData,
                                                                           LastModified = snippet.LastModified,
                                                                           IsPublic = snippet.IsPublic,
                                                                           Language_Id = snippet.Language_Id,
                                                                           User_Id = snippet.User_Id,
                                                                           User_FormsAuthId = snippet.User_FormsAuthId
                                                                       }));

                    response.Snippets = returnList.AsEnumerable();
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetSnippetsByDescription Method Failed", ex);
            }

            return response;
        }

        public GetSnippetsByDataResponse GetSnippetsByData(GetSnippetsByDataRequest request)
        {
            var response = new GetSnippetsByDataResponse();
            var returnList = new List<SnippetDTO>();

            try
            {
                Guard.ArgNotNull(_unitOfWork.SnippetRepository, "SnippetRepository");

                byte[] searchTextBytes = Encoding.UTF8.GetBytes(request.DataString);

                // Horrible hack to get working, need to find a more efficient way
                var snippetsData = (from d in _unitOfWork.SnippetRepository.Get(d => d.Id > 0 && d.IsPublic)
                                    select new {d.Id, d.Data}).AsEnumerable();
                List<int> matchingIds = (
                                            from binaryData in snippetsData
                                            where
                                                Encoding.UTF8.GetString(binaryData.Data).Contains(
                                                    Encoding.UTF8.GetString(searchTextBytes))
                                            select binaryData.Id).ToList();

                List<Snippet> snippets =
                    matchingIds.Select(matchingId => _unitOfWork.SnippetRepository.GetById(matchingId)).ToList();

                Guard.ArgNotNull(snippets, "snippets");
                if (snippets.Any())
                {
                    returnList.AddRange(snippets.Select(snippet => new SnippetDTO
                                                                       {
                                                                           Id = snippet.Id,
                                                                           Guid = snippet.Guid,
                                                                           Name = snippet.Name,
                                                                           Description = snippet.Description,
                                                                           PreviewData = snippet.PreviewData,
                                                                           Data = snippet.PreviewData,
                                                                           LastModified = snippet.LastModified,
                                                                           IsPublic = snippet.IsPublic,
                                                                           Language_Id = snippet.Language_Id,
                                                                           User_Id = snippet.User_Id,
                                                                           User_FormsAuthId = snippet.User_FormsAuthId
                                                                       }));

                    response.Snippets = returnList.AsEnumerable();
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetSnippetsByData Method Failed", ex);
            }

            return response;
        }

        public CreateSnippetResponse CreateSnippet(CreateSnippetRequest request)
        {
            var response = new CreateSnippetResponse();
            var newSnippet = new Snippet
                                 {
                                     Id = request.Id,
                                     Guid = request.Guid,
                                     Name = request.Name,
                                     Description = request.Description,
                                     PreviewData = request.PreviewData,
                                     Data = request.Data,
                                     LastModified = request.LastModified,
                                     IsPublic = request.IsPublic,
                                     Language_Id = request.Language_Id,
                                     User_Id = request.User_Id,
                                     User_FormsAuthId = request.User_FormsAuthId
                                 };
            try
            {
                _unitOfWork.SnippetRepository.Insert(newSnippet);
                _unitOfWork.Save();

                response.SnippetId = newSnippet.Id;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("CreateSnippet Method Failed", ex);
            }
            return response;
        }

        public UpdateSnippetResponse UpdateSnippet(UpdateSnippetRequest request)
        {
            var response = new UpdateSnippetResponse();

            try
            {
                _unitOfWork.SnippetRepository.Update(new Snippet
                                                         {
                                                             Id = request.Id,
                                                             Guid = request.Guid,
                                                             Name = request.Name,
                                                             Description = request.Description,
                                                             PreviewData = request.PreviewData,
                                                             Data = request.PreviewData,
                                                             LastModified = request.LastModified,
                                                             IsPublic = request.IsPublic,
                                                             Language_Id = request.Language_Id,
                                                             User_Id = request.User_Id,
                                                             User_FormsAuthId = request.User_FormsAuthId
                                                         });

                _unitOfWork.Save();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("UpdateSnippet Method Failed", ex);
            }
            return response;
        }

        public DeleteSnippetResponse DeleteSnippet(DeleteSnippetRequest request)
        {
            var response = new DeleteSnippetResponse();

            try
            {
                _unitOfWork.SnippetRepository.Delete(request.Id);
                _unitOfWork.Save();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("CreateSnippet Method Failed", ex);
            }
            return response;
        }

        public int GetTotalSnippetCount(bool isPrivate)
        {
            return _unitOfWork.SnippetRepository.Get(d => (d.Id > 0) && (d.IsPublic == !isPrivate)).Count();
        }

        #endregion
    }
}