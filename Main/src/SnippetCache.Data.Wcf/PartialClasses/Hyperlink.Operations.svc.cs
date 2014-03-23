using System;
using System.Collections.Generic;
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

        public GetHyperlinksBySnippetIdResponse GetHyperlinksBySnippetId(GetHyperlinksBySnippetIdRequest request)
        {
            var response = new GetHyperlinksBySnippetIdResponse
                               {
                                   Hyperlinks = new List<HyperlinkDTO>()
                               };

            try
            {
                Guard.ArgNotNull(request, "request");

                IEnumerable<Hyperlink> result =
                    _unitOfWork.HyperLinkRepository.Get(d => d.Snippet.Guid == request.SnippetId);
                Guard.ArgNotNull(result, "results");

                foreach (Hyperlink hyperlink in result)
                {
                    var temp = new HyperlinkDTO
                                   {
                                       Id = hyperlink.Id,
                                       Description = hyperlink.Description,
                                       LastModified = hyperlink.LastModified,
                                       Snippet_Id = hyperlink.Snippet_Id,
                                       Uri = hyperlink.Uri
                                   };
                    ((IList<HyperlinkDTO>) response.Hyperlinks).Add(temp);
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetHyperlinksBySnippetId Method Failed", ex);
            }

            return response;
        }

        public GetHyperlinkByIdResponse GetHyperlinkById(GetHyperlinkByIdRequest request)
        {
            var response = new GetHyperlinkByIdResponse();

            try
            {
                Guard.ArgNotNull(request, "request");

                Hyperlink result = _unitOfWork.HyperLinkRepository.GetById(request.HyperlinkId);

                Guard.ArgNotNull(result, "result");

                var dto = new HyperlinkDTO
                              {
                                  Id = result.Id,
                                  Description = result.Description,
                                  LastModified = result.LastModified,
                                  Snippet_Id = result.Snippet_Id,
                                  Uri = result.Uri
                              };

                response.Success = true;
                response.Hyperlink = dto;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetHyperlinkById Method Failed", ex);
            }

            return response;
        }

        public GetHyperlinksByUserIdResponse GetHyperlinksByUserId(GetHyperlinksByUserIdRequest request)
        {
            var response = new GetHyperlinksByUserIdResponse();

            try
            {
                Guard.ArgNotNull(request, "request");

                IEnumerable<Hyperlink> result =
                    _unitOfWork.HyperLinkRepository.Get(d => d.Snippet.User_Id == request.UserId &&
                                                             d.Snippet.User_FormsAuthId == request.UserGuid);

                foreach (Hyperlink hyperlink in result)
                {
                    var temp = new HyperlinkDTO
                                   {
                                       Id = hyperlink.Id,
                                       Description = hyperlink.Description,
                                       LastModified = hyperlink.LastModified,
                                       Snippet_Id = hyperlink.Snippet_Id,
                                       Uri = hyperlink.Uri
                                   };
                    ((IList<HyperlinkDTO>) response.Hyperlinks).Add(temp);
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetHyperlinksByUserId Method Failed", ex);
            }

            return response;
        }

        public CreateHyperlinkResponse CreateHyperlink(CreateHyperlinkRequest request)
        {
            var response = new CreateHyperlinkResponse();

            try
            {
                Guard.ArgNotNull(request, "request");
                var hyperLink = new Hyperlink
                                    {
                                        Description = request.Description,
                                        LastModified = request.LastModified,
                                        Snippet_Id = request.SnippetId,
                                        Uri = request.Uri
                                    };
                _unitOfWork.HyperLinkRepository.Insert(hyperLink);
                _unitOfWork.Save();

                response.Success = true;
                response.HyperlinkId = hyperLink.Id;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("CreateHyperlink Method Failed", ex);
            }
            return response;
        }

        public UpdateHyperlinkResponse UpdateHyperlink(UpdateHyperlinkRequest request)
        {
            var response = new UpdateHyperlinkResponse();
            try
            {
                Guard.ArgNotNull(request, "request");

                var hyperlink = new Hyperlink
                                    {
                                        Id = request.HyperlinkId,
                                        Description = request.Description,
                                        Uri = request.Uri,
                                        LastModified = request.LastModified
                                    };
                _unitOfWork.HyperLinkRepository.Update(hyperlink);
                _unitOfWork.Save();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("UpdateHyperlink Method Failed", ex);
            }
            return response;
        }

        public DeleteHyperlinkResponse DeleteHyperlink(DeleteHyperlinkRequest request)
        {
            var response = new DeleteHyperlinkResponse();
            try
            {
                Guard.ArgNotNull(request, "request");

                _unitOfWork.HyperLinkRepository.Delete(request.HyperlinkId);
                _unitOfWork.Save();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("DeleteHyperlink Method Failed", ex);
            }

            return response;
        }

        #endregion
    }
}