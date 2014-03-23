using System;
using System.Collections.Generic;
using System.ServiceModel;
using SnippetCache.Data.DTO;

namespace SnippetCache.Managers.Wcf
{
    [ServiceContract]
    public interface ISnippetCacheManagerService
    {
        #region Snippets Manager Operations

        [OperationContract]
        SnippetDTO CreateSnippet(SnippetDTO snippet, LanguageDTO language, UserDTO user, IEnumerable<FileDTO> files,
                                 IEnumerable<HyperlinkDTO> links);

        [OperationContract]
        IEnumerable<SnippetDTO> FindSnippetsByUser(string text, int userId, Guid userGuid);

        [OperationContract]
        IEnumerable<SnippetDTO> FindSnippetsByUserPaged(string text, int userId, Guid userGuid, int snippetsPerPage,
                                                        int currentPage);

        [OperationContract]
        IEnumerable<SnippetDTO> FindSnippets(string text);

        [OperationContract]
        IEnumerable<SnippetDTO> FindSnippetsPaged(string text, int itemsPerPage, int currentPage);

        [OperationContract]
        IEnumerable<SnippetDTO> GetSnippets();

        [OperationContract]
        IEnumerable<SnippetDTO> GetAllPublicSnippets(int snippetsPerPage, int currentPage);

        [OperationContract]
        SnippetDTO GetSnippet(string guid);

        [OperationContract]
        IEnumerable<SnippetDTO> GetSnippetsByPage(int snippetsPerPage, int currentPage);

        [OperationContract]
        IEnumerable<SnippetDTO> GetPrivateSnippets(int userId, Guid userGuid);

        [OperationContract]
        IEnumerable<LanguageDTO> GetLanguagesForDisplay();

        [OperationContract]
        int GetTotalSnippetCount(bool isPrivate);

        [OperationContract]
        IEnumerable<HyperlinkDTO> GetHyperlinksForSnippet(Guid snippetId, int userId, Guid userGuid);

        [OperationContract]
        IEnumerable<FileDTO> GetFilesForSnippet(Guid snippetId, int userId, Guid userGuid);

        #endregion

        #region User Manager Operations

        [OperationContract]
        UserDTO UserDetails(int userId, Guid userGuid);

        [OperationContract]
        bool UpdateUserDetails(UserDTO userDto);

        #endregion

        #region Notification

        [OperationContract]
        IEnumerable<NotificationDTO> UnreadUserNotification(int userId, Guid userGuid);

        [OperationContract]
        void CreateNewUserNotification(NotificationDTO notification);

        [OperationContract]
        void RemoveUserNotification(NotificationDTO notification);

        #endregion
    }
}