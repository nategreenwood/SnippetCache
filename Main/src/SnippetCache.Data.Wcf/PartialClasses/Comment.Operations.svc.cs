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
        #region Comment Operations

        public GetAllCommentsResponse GetAllComments(GetAllCommentsRequest request)
        {
            var response = new GetAllCommentsResponse();
            using (_unitOfWork)
                try
                {
                    Guard.ArgNotNull(request, "request");

                    Comment[] results = _unitOfWork.CommentRepository.Get(d => d.Id > 0).ToArray();
                    Guard.ArgNotNull(results, "results");

                    var quickInfoResults = new List<CommentQuickInfo>(results.Count());
                    quickInfoResults.AddRange(results.Select(
                        comment => new CommentQuickInfo
                                       {
                                           Id = comment.Id,
                                           Text = comment.Text,
                                           DataLastModified = comment.LastModified
                                       }));
                    response.Comments = quickInfoResults;
                    response.Success = true;
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.FailureInformation = ex.Message;
                    Logger.LogError("GetAllComments Method Failed", ex);
                }
            _unitOfWork.Dispose();
            return response;
        }

        public GetAllCommentsForSnippetResponse GetAllCommentsBySnippetId(GetAllCommentsForSnippetRequest request)
        {
            var response = new GetAllCommentsForSnippetResponse();

            try
            {
                Guard.ArgNotNull(request, "request");

                IEnumerable<Comment> results = _unitOfWork.CommentRepository.Get(d => d.Snippet_Id == request.SnippetId);
                Guard.ArgNotNull(results, "results");

                var quickInfoResults = new List<CommentQuickInfo>(results.Count());
                quickInfoResults.AddRange(results.Select(
                    comment => new CommentQuickInfo
                                   {
                                       Id = comment.Id,
                                       Text = comment.Text,
                                       DataLastModified = comment.LastModified
                                   }));
                response.Comments = quickInfoResults;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetAllCommentsBySnippetId Method Failed", ex);
            }

            return response;
        }

        public GetCommentByIdResponse GetCommentById(GetCommentByIdRequest request)
        {
            var response = new GetCommentByIdResponse();
            try
            {
                Guard.ArgNotNull(_unitOfWork.CommentRepository, "CommentRepository");
                Comment comment = _unitOfWork.CommentRepository.GetById(request.Id);

                Guard.ArgNotNull(comment, "comment");

                response.Id = comment.Id;
                response.Text = comment.Text;
                response.SnippetId = comment.Snippet_Id;
                response.UserId = comment.User_Id;
                response.UserFormsAuthId = comment.User_FormsAuthId;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetCommentById Method Failed", ex);
            }

            return response;
        }

        public CreateNewCommentResponse CreateNewComment(CreateNewCommentRequest request)
        {
            var response = new CreateNewCommentResponse();
            try
            {
                Guard.ArgNotNull(_unitOfWork.CommentRepository, "CommentRepository");

                var comment = new Comment
                                  {
                                      Id = 0,
                                      LastModified = request.DateLastModified,
                                      Snippet_Id = request.SnippetId,
                                      User_Id = request.UserId,
                                      User_FormsAuthId = request.UserFormsAuthId,
                                      Text = request.Text
                                  };

                _unitOfWork.CommentRepository.Insert(comment);
                _unitOfWork.Save();

                int newId = comment.Id;
                if (newId > 0)
                {
                    response.Success = true;
                    response.Id = newId;
                    response.SnippetId = request.SnippetId;
                    response.UserFormsAuthId = request.UserFormsAuthId;
                    response.UserId = request.UserId;
                    response.Text = request.Text;
                    response.DateLastModified = request.DateLastModified;

                    Logger.LogInfo("Successfully created Comment Id: " + newId.ToString(), LogType.General);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("CreateNewComment Method Failed", ex);
            }

            return response;
        }

        public UpdateCommentResponse UpdateComment(UpdateCommentRequest request)
        {
            var response = new UpdateCommentResponse();
            try
            {
                Guard.ArgNotNull(request, "request");
                Comment comment = _unitOfWork.CommentRepository.GetById(request.Id);

                Guard.ArgNotNull(comment, "comment");
                comment.Text = request.Text;

                _unitOfWork.CommentRepository.Update(comment);
                _unitOfWork.Save();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("UpdateLanguage method failed", ex);
            }

            return response;
        }

        public DeleteCommentResponse DeleteComment(DeleteCommentRequest request)
        {
            var response = new DeleteCommentResponse();
            try
            {
                Guard.ArgNotNull(request, "request");
                Comment comment = _unitOfWork.CommentRepository.GetById(request.Id);

                Guard.ArgNotNull(comment, "comment");

                _unitOfWork.CommentRepository.Delete(comment);
                _unitOfWork.Save();

                response.Success = true;
                Logger.LogInfo("Successfully deleted Comment Id: " + request.Id.ToString(), LogType.General);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("DeleteComment method failed", ex);
            }

            return response;
        }

        #endregion
    }
}