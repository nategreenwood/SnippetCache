using System.ServiceModel;
using SnippetCache.Data.Wcf.Requests;
using SnippetCache.Data.Wcf.Responses;

namespace SnippetCache.Data.Wcf
{
    [ServiceContract]
    public interface ISnippetCacheDataService
    {
        #region Language Operations

        [OperationContract]
        GetAllLanguagesResponse GetAllLanguages(GetAllLanguagesRequest request);

        [OperationContract]
        GetLanguageByIdResponse GetLanguageById(GetLanguageByIdRequest request);

        [OperationContract]
        GetLanguageByNameResponse GetLanguageByName(GetLanguageByNameRequest request);

        // Create
        [OperationContract]
        CreateNewLanguageResponse CreateNewLanguage(CreateNewLanguageRequest request);

        // Update
        [OperationContract]
        UpdateLanguageResponse UpdateLanguage(UpdateLanguageRequest request);

        // Delete
        [OperationContract]
        DeleteLanguageResponse DeleteLanguage(DeleteLanguageRequest request);

        #endregion

        #region User Operations

        [OperationContract]
        GetAllUsersResponse GetAllUsers(GetAllUsersRequest request);

        [OperationContract]
        GetUserByIdResponse GetUserById(GetUserByIdRequest request);

        [OperationContract]
        GetUserByNameResponse GetUserByName(GetUserByNameRequest request);

        // Create
        [OperationContract]
        CreateNewUserResponse CreateNewUser(CreateNewUserRequest request);

        // Update
        [OperationContract]
        UpdateUserResponse UpdateUser(UpdateUserRequest request);

        // Delete
        [OperationContract]
        DeleteUserResponse DeleteUser(DeleteUserRequest request);

        #endregion

        #region NotificationType Operations

        [OperationContract]
        GetAllNotificationTypesResponse GetAllNotificationTypes(GetAllNotificationTypesRequest request);

        [OperationContract]
        GetNotificationTypesByIdResponse GetNotificationTypeById(GetNotificationTypesByIdRequest request);

        [OperationContract]
        GetNotificationTypesByNameResponse GetNotificationTypeByName(GetNotificationTypesByNameRequest request);

        // Create
        [OperationContract]
        CreateNewNotificationTypesResponse CreateNewNotificationType(CreateNewNotificationTypesRequest request);

        // Update
        [OperationContract]
        UpdateNotificationTypesResponse UpdateNotificationType(UpdateNotificationTypesRequest request);

        // Delete
        [OperationContract]
        DeleteNotificationTypesResponse DeleteNotificationType(DeleteNotificationTypesRequest request);

        #endregion

        #region Notification Operations

        [OperationContract]
        GetAllNotificationsResponse GetAllNotifications(GetAllNotificationsRequest request);

        [OperationContract]
        GetNotificationsByIdResponse GetNotificationById(GetNotificationsByIdRequest request);

        // Do not expose this method
        GetNotificationsByNameResponse GetNotificationByName(GetNotificationsByNameRequest request);

        [OperationContract]
        GetAllNotificationsForUserResponse GetNotificationsForUser(GetAllNotificationsForUserRequest request);

        // Create
        [OperationContract]
        CreateNewNotificationsResponse CreateNewNotification(CreateNewNotificationsRequest request);

        // Update
        [OperationContract]
        UpdateNotificationsResponse UpdateNotification(UpdateNotificationsRequest request);

        // Delete
        [OperationContract]
        DeleteNotificationsResponse DeleteNotification(DeleteNotificationsRequest request);

        #endregion

        #region Comment Operations

        [OperationContract]
        GetAllCommentsResponse GetAllComments(GetAllCommentsRequest request);

        [OperationContract]
        GetAllCommentsForSnippetResponse GetAllCommentsBySnippetId(GetAllCommentsForSnippetRequest request);

        [OperationContract]
        GetCommentByIdResponse GetCommentById(GetCommentByIdRequest request);

        [OperationContract]
        CreateNewCommentResponse CreateNewComment(CreateNewCommentRequest request);

        [OperationContract]
        UpdateCommentResponse UpdateComment(UpdateCommentRequest request);

        [OperationContract]
        DeleteCommentResponse DeleteComment(DeleteCommentRequest request);

        #endregion

        #region File Operations

        [OperationContract]
        GetFilesBySnippetIdResponse GetFilesBySnippetId(GetFilesBySnippetIdRequest request);

        [OperationContract]
        GetFileByIdResponse GetFileById(GetFileByIdRequest request);

        [OperationContract]
        GetFilesByUserIdResponse GetFilesByUserId(GetFilesByUserIdRequest request);

        [OperationContract]
        CreateFileResponse CreateFile(CreateFileRequest request);

        [OperationContract]
        UpdateFileResponse UpdateFile(UpdateFileRequest request);

        [OperationContract]
        DeleteFileResponse DeleteFile(DeleteFileRequest request);

        [OperationContract]
        int GetTotalSnippetCount(bool isPrivate);

        #endregion

        #region Hyperlink Operations

        [OperationContract]
        GetHyperlinksBySnippetIdResponse GetHyperlinksBySnippetId(GetHyperlinksBySnippetIdRequest request);

        [OperationContract]
        GetHyperlinkByIdResponse GetHyperlinkById(GetHyperlinkByIdRequest request);

        [OperationContract]
        GetHyperlinksByUserIdResponse GetHyperlinksByUserId(GetHyperlinksByUserIdRequest request);

        [OperationContract]
        CreateHyperlinkResponse CreateHyperlink(CreateHyperlinkRequest request);

        [OperationContract]
        UpdateHyperlinkResponse UpdateHyperlink(UpdateHyperlinkRequest request);

        [OperationContract]
        DeleteHyperlinkResponse DeleteHyperlink(DeleteHyperlinkRequest request);

        #endregion

        #region Snippet Operations

        [OperationContract]
        GetPagedSnippetListResponse GetPagedSnippetList(GetPagedSnippetListRequest request);

        [OperationContract]
        GetSnippetByIdResponse GetSnippetById(GetSnippetByIdRequest request);

        [OperationContract]
        GetSnippetsByUserIdResponse GetSnippetsByUserId(GetSnippetsByUserIdRequest request);

        [OperationContract]
        GetSnippetByGuidResponse GetSnippetByGuid(GetSnippetByGuidRequest request);

        [OperationContract]
        CreateSnippetResponse CreateSnippet(CreateSnippetRequest request);

        [OperationContract]
        UpdateSnippetResponse UpdateSnippet(UpdateSnippetRequest request);

        [OperationContract]
        DeleteSnippetResponse DeleteSnippet(DeleteSnippetRequest request);

        [OperationContract]
        GetSnippetsByTitleResponse GetSnippetsByTitle(GetSnippetsByTitleRequest request);

        [OperationContract]
        GetSnippetsByDescriptionResponse GetSnippetsByDescription(GetSnippetsByDescriptionRequest request);

        [OperationContract]
        GetSnippetsByDataResponse GetSnippetsByData(GetSnippetsByDataRequest request);

        #endregion
    }
}