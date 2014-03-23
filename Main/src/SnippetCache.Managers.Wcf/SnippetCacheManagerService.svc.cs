using System;
using System.Collections.Generic;
using System.Linq;
using SnippetCache.Data.DTO;
using SnippetCache.Data.EF4.Model;
using SnippetCache.Engines;
using SnippetCache.Engines.DataService;

namespace SnippetCache.Managers.Wcf
{
    public class SnippetCacheManagerService : ISnippetCacheManagerService
    {
        private ISnippetCacheDataService _dataService;
        private NotificationManager _notificationManager;
        private SearchEngine _searchEngine;
        private SnippetManager _snippetManager;
        private SnippetOperationsEngine _snippetOperationsEngine;
        private UserOperationsEngine _userOpsEngine;
        private UserValidationEngine _userValidationEngine;

        public SnippetCacheManagerService()
        {
            if (_dataService == null)
            {
                _dataService = new SnippetCacheDataServiceClient();
            }
        }

        public SnippetCacheManagerService(ISnippetCacheDataService dataService)
        {
            _dataService = dataService;
        }

        internal ISnippetCacheDataService DataService
        {
            get { return _dataService ?? (_dataService = new SnippetCacheDataServiceClient()); }
        }

        internal SearchEngine SearchEngine
        {
            get { return _searchEngine ?? (_searchEngine = new SearchEngine(DataService)); }
        }

        internal UserOperationsEngine UserOperationsEngine
        {
            get { return _userOpsEngine ?? (_userOpsEngine = new UserOperationsEngine(DataService)); }
        }

        internal UserValidationEngine UserValidationEngine
        {
            get { return _userValidationEngine ?? (_userValidationEngine = new UserValidationEngine(DataService)); }
        }

        internal SnippetOperationsEngine SnippetOperationsEngine
        {
            get { return _snippetOperationsEngine ?? (_snippetOperationsEngine = new SnippetOperationsEngine(DataService)); }
        }

        internal SnippetManager SnippetManager
        {
            get
            {
                return _snippetManager ??
                       (_snippetManager =
                        new SnippetManager(SearchEngine, UserOperationsEngine, UserValidationEngine,
                                           SnippetOperationsEngine));
            }
        }

        internal NotificationManager NotificationManager
        {
            get { return _notificationManager ?? (_notificationManager = new NotificationManager(DataService)); }
        }

        #region ISnippetCacheManagerService Members

        /// <summary>
        /// Returns all public snippets and private snippets that match the input text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="userId"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public IEnumerable<SnippetDTO> FindSnippetsByUser(string text, int userId, Guid userGuid)
        {
            var mgr = new SnippetManager(SearchEngine, UserOperationsEngine, UserValidationEngine,
                                         SnippetOperationsEngine);

            IEnumerable<SnippetDTO> result = mgr.PerformSearch(text, userId, userGuid).ToSnippetDTO();

            return result.GroupBy(i => i.Id, (key, group) => group.First()).ToArray();
        }

        public IEnumerable<SnippetDTO> FindSnippetsByUserPaged(string text, int userId, Guid userGuid,
                                                               int snippetsPerPage, int currentPage = 1)
        {
            var mgr = new SnippetManager(SearchEngine, UserOperationsEngine, UserValidationEngine,
                                         SnippetOperationsEngine);

            IEnumerable<SnippetDTO> result = mgr.GetPagedSnippetList(snippetsPerPage, currentPage, userId, userGuid);

            return result.GroupBy(i => i.Id, (key, group) => group.First()).ToArray();
        }

        /// <summary>
        /// Returns only public snippets that match the input text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IEnumerable<SnippetDTO> FindSnippets(string text)
        {
            SnippetManager mgr = SnippetManager;
            IEnumerable<SnippetDTO> result = mgr.PerformSearch(text, -1, Guid.Empty);

            return result.GroupBy(i => i.Id, (key, group) => group.First()).ToArray();
        }

        public IEnumerable<SnippetDTO> FindSnippetsPaged(string text, int itemsPerPage, int currentPage)
        {
            SnippetManager mgr = SnippetManager;
            IEnumerable<SnippetDTO> result = mgr.PerformSearch(text, -1, Guid.Empty, itemsPerPage, currentPage);

            return
                result.OrderByDescending(d => d.LastModified).GroupBy(i => i.Id, (key, group) => group.First()).ToArray();
        }

        public IEnumerable<SnippetDTO> GetSnippets()
        {
            const int resultMax = 500;
            IEnumerable<SnippetDTO> result = SnippetManager.GetPagedSnippetList(resultMax, 1);

            return result.GroupBy(i => i.Id, (key, group) => group.First()).ToArray();
        }

        public IEnumerable<SnippetDTO> GetAllPublicSnippets(int snippetsPerPage, int currentPage)
        {
            IEnumerable<SnippetDTO> result = SnippetManager.GetPagedSnippetList(snippetsPerPage, currentPage);

            return result;
        }

        public SnippetDTO GetSnippet(string guid)
        {
            SnippetDTO result = SnippetManager.GetSnippetByGuid(guid);

            return result;
        }

        public IEnumerable<SnippetDTO> GetSnippetsByPage(int snippetsPerPage, int currentPage)
        {
            return
                SnippetManager.GetPagedSnippetList(snippetsPerPage, currentPage).GroupBy(i => i.Id,
                                                                                         (key, group) => group.First()).
                    ToArray();
        }

        public IEnumerable<SnippetDTO> GetPrivateSnippets(int userId, Guid userGuid)
        {
            return
                SnippetManager.GetPrivateSnippets(userId, userGuid).GroupBy(i => i.Id, (key, group) => group.First()).
                    ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public UserDTO UserDetails(int userId, Guid userGuid)
        {
            UserDTO result = SnippetManager.UserDetails(userId, userGuid);

            return result;
        }

        public bool UpdateUserDetails(UserDTO userDto)
        {
            var userResult = new User
                                 {
                                     Id = userDto.Id,
                                     FormsAuthId = userDto.FormsAuthId,
                                     LoginName = userDto.LoginName,
                                     AvatarImage = userDto.AvatarImage,
                                     Email = userDto.Email
                                 };

            return UserOperationsEngine.UpdateUserDetails(userResult);
        }

        public IEnumerable<NotificationDTO> UnreadUserNotification(int userId, Guid userGuid)
        {
            IEnumerable<NotificationDTO> result = NotificationManager.UnreadUserNotification(userId, userGuid);

            return result;
        }

        public void CreateNewUserNotification(NotificationDTO notification)
        {
            NotificationManager.AddUserNotification(notification);
        }

        public void RemoveUserNotification(NotificationDTO notification)
        {
            NotificationManager.DeleteUserNotification(notification);
        }

        public IEnumerable<LanguageDTO> GetLanguagesForDisplay()
        {
            IEnumerable<LanguageDTO> languages = SnippetManager.GetAllLanguages();

            return languages;
        }

        public SnippetDTO CreateSnippet(SnippetDTO snippet, LanguageDTO language, UserDTO user,
                                        IEnumerable<FileDTO> files,
                                        IEnumerable<HyperlinkDTO> links)
        {
            SnippetDTO newSnippet = SnippetManager.CreateSnippet(snippet, language, user, files, links);

            return newSnippet;
        }

        public int GetTotalSnippetCount(bool isPrivate)
        {
            return SnippetManager.CurrentSnippetCount(isPrivate);
        }

        public IEnumerable<HyperlinkDTO> GetHyperlinksForSnippet(Guid snippetGuid, int userId, Guid userGuid)
        {
            IEnumerable<HyperlinkDTO> hyperlinks = SnippetManager.GetHyperlinksForSnippet(snippetGuid, userId, userGuid);

            return hyperlinks;
        }

        public IEnumerable<FileDTO> GetFilesForSnippet(Guid snippetGuid, int userId, Guid userGuid)
        {
            IEnumerable<FileDTO> files = SnippetManager.GetFilesForSnippet(snippetGuid, userId, userGuid);

            return files;
        }

        #endregion
    }
}