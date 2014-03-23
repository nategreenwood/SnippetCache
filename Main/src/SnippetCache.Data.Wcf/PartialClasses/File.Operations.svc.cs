using System;
using System.Collections.Generic;
using System.Linq;
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
        #region File Operations

        public GetFilesBySnippetIdResponse GetFilesBySnippetId(GetFilesBySnippetIdRequest request)
        {
            var response = new GetFilesBySnippetIdResponse();

            try
            {
                Guard.ArgNotNull(request, "request");

                File[] results = _unitOfWork.FileRepository.Get(d => d.Snippet.Guid == request.SnippetId).ToArray();
                Guard.ArgNotNull(results, "results");

                var quickInfoResults = new List<FileDTO>(results.Count());
                quickInfoResults.AddRange(results.Select(
                    notification => new FileDTO
                                        {
                                            Id = notification.Id,
                                            Data = notification.Data,
                                            Name = notification.Name,
                                            Description = notification.Description,
                                            LastModified = notification.LastModified,
                                            Snippet_Id = notification.Snippet_Id
                                        }));
                response.Success = true;
                response.Files = quickInfoResults;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetFilesBySnippetId Method Failed", ex);
            }

            return response;
        }

        public GetFileByIdResponse GetFileById(GetFileByIdRequest request)
        {
            var response = new GetFileByIdResponse();

            try
            {
                Guard.ArgNotNull(request, "request");

                File results = _unitOfWork.FileRepository.GetById(request.FileId);
                Guard.ArgNotNull(results, "results");

                var returnDTO = new FileDTO
                                    {
                                        Id = results.Id,
                                        Name = results.Name,
                                        Description = results.Description,
                                        Data = results.Data,
                                        LastModified = results.LastModified,
                                        Snippet_Id = results.Snippet_Id
                                    };
                response.Success = true;
                response.File = returnDTO;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetFileById Method Failed", ex);
            }

            return response;
        }

        public GetFilesByUserIdResponse GetFilesByUserId(GetFilesByUserIdRequest request)
        {
            var response = new GetFilesByUserIdResponse();

            try
            {
                Guard.ArgNotNull(request, "request");

                IEnumerable<FileDTO> results =
                    _unitOfWork.FileRepository.Get(
                        d => d.Snippet.User_Id == request.UserId && d.Snippet.User_FormsAuthId == request.UserGuid
                             && d.Snippet_Id == request.SnippetId).Cast<FileDTO>();

                Guard.ArgNotNull(results, "results");

                response.Success = true;
                response.Files = results;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetFilesByUserId Method Failed", ex);
            }

            return response;
        }

        public CreateFileResponse CreateFile(CreateFileRequest request)
        {
            var response = new CreateFileResponse();

            try
            {
                Guard.ArgNotNull(request, "request");
                var f = new File
                            {
                                Name = request.Name,
                                Description = request.Description,
                                Data = request.Data,
                                LastModified = request.LastModified,
                                Snippet_Id = request.SnippetId
                            };
                _unitOfWork.FileRepository.Insert(f);
                _unitOfWork.Save();

                response.Success = true;
                response.FileId = f.Id;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("CreateFile Method Failed", ex);
            }

            return response;
        }

        public UpdateFileResponse UpdateFile(UpdateFileRequest request)
        {
            var response = new UpdateFileResponse();

            try
            {
                Guard.ArgNotNull(request, "request");
                var f = new File
                            {
                                Name = request.Name,
                                Description = request.Description,
                                Data = request.Data,
                                LastModified = request.LastModified,
                                Snippet_Id = request.SnippetId
                            };
                _unitOfWork.FileRepository.Update(f);
                _unitOfWork.Save();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("UpdateFile Method Failed", ex);
            }

            return response;
        }

        public DeleteFileResponse DeleteFile(DeleteFileRequest request)
        {
            var response = new DeleteFileResponse();

            try
            {
                Guard.ArgNotNull(request, "request");

                _unitOfWork.FileRepository.Delete(request.FileId);
                _unitOfWork.Save();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("DeleteFile Method Failed", ex);
            }

            return response;
        }

        #endregion
    }
}