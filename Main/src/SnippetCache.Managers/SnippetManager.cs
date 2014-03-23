using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SnippetCache.Data.DTO;
using SnippetCache.Data.EF4.Model;
using SnippetCache.Engines;
using SnippetCache.Utils.Logging;
using SnippetCache.Utils.Validation;

namespace SnippetCache.Managers
{
    public class SnippetManager : ISnippetManager
    {
        private const int PREVIEW_DATA_LINE_COUNT = 5;
        private const int SNIPPETS_PER_PAGE = 6;
        private readonly SearchEngine _searchEngine;
        private readonly SnippetOperationsEngine _snippetOperationsEngine;
        private readonly UserOperationsEngine _userOperationsEngine;
        private readonly UserValidationEngine _userValidationEngine;

        public SnippetManager()
        {
        }

        public SnippetManager(
            SearchEngine searchEngine,
            UserOperationsEngine userOperationEngine,
            UserValidationEngine userValidationEngine,
            SnippetOperationsEngine snippetOperationsEngine)
        {
            _searchEngine = searchEngine;
            _userOperationsEngine = userOperationEngine;
            _userValidationEngine = userValidationEngine;
            _snippetOperationsEngine = snippetOperationsEngine;
        }

        #region ISnippetManager Members

        public IEnumerable<SnippetDTO> PerformSearch(string searchText, int userId, Guid userGuid)
        {
            bool userExists = false;
            var returnList = new List<SnippetDTO>();

            if (userId > 0 && userGuid != Guid.Empty)
            {
                userExists = _userValidationEngine.CheckUserExists(userId, userGuid);
            }

            if (userExists)
                returnList.AddRange(_searchEngine.FindPrivateSnippetsForUser(userId, userGuid));

            returnList.AddRange(_searchEngine.FindPublicSnippets(searchText));

            return returnList.AsEnumerable().OrderByDescending(d => d.LastModified);
        }

        public string ManagerTypeName
        {
            get { return "SnippetManager"; }
        }

        #endregion

        public SnippetDTO CreateSnippet(SnippetDTO snippet, LanguageDTO language, UserDTO user,
                                        IEnumerable<FileDTO> files,
                                        IEnumerable<HyperlinkDTO> links)
        {
            SnippetDTO returnDTO = snippet;

            if (snippet.Data == null || (language == null || language.Id <= 0)) return null;

            if (snippet.Language_Id > 0)
            {
                snippet.Language_Id = language.Id;
                snippet.PreviewData = GetPreviewData(snippet.Data);
                snippet.LastModified = DateTime.UtcNow;
                snippet.User_Id = user.Id;
                snippet.User_FormsAuthId = user.FormsAuthId;
            }

            int result = _snippetOperationsEngine.CreateNewSnippet(snippet);
            if (result > 0)
            {
                returnDTO.Id = snippet.Id;
                returnDTO.Guid = snippet.Guid;

                // Snippet creation was successult. Now we have a SnippetId and can insert
                // any hyperlinks or files

                // Hyperlinks first
                if (links != null && links.Any())
                {
                    foreach (HyperlinkDTO hyperlinkDTO in links)
                    {
                        // Update the hyperlink's Snippet Id since we successfully sved it
                        hyperlinkDTO.Snippet_Id = result;
                        int createHyperlinkResponse = _snippetOperationsEngine.CreateNewHyperlink(hyperlinkDTO);
                        if (createHyperlinkResponse <= 0)
                            Logger.LogInfo(
                                string.Format("Failed to create hyperlink uri {0} for snippet id {1}", hyperlinkDTO.Uri,
                                              hyperlinkDTO.Snippet_Id), LogType.Notify);
                    }
                }

                // Files now
                if (files != null && files.Any())
                {
                    foreach (FileDTO fileDTO in files)
                    {
                        // Update the SnippetId with newly created id
                        fileDTO.Snippet_Id = result;
                        int createFileResponse = _snippetOperationsEngine.CreateNewFile(fileDTO);
                        if (createFileResponse <= 0)
                            Logger.LogInfo(string.Format("Failed to create new file {0} for snippet id {1}", fileDTO.Id,
                                                         fileDTO.Snippet_Id), LogType.Notify);
                    }
                }
            }

            return returnDTO;
        }

        public IEnumerable<SnippetDTO> GetPagedSnippetList(int snippetsPerPage, int currentPage)
        {
            Guard.IntArgIsPositive(snippetsPerPage, "snippetsPerPage");
            Guard.IntArgIsPositive(currentPage, "currentPage");

            IEnumerable<SnippetDTO> returnList =
                _searchEngine.GetPagedSnippetList(snippetsPerPage, currentPage).AsEnumerable();
            return returnList;
        }

        public IEnumerable<SnippetDTO> GetPagedSnippetList(int snippetsPerPage, int currentPage, int userId,
                                                           Guid userGuid)
        {
            Guard.IntArgIsPositive(userId, "currentPage");
            Guard.GuidArgIsValue(userGuid, "userGuid");

            IEnumerable<SnippetDTO> returnList = _searchEngine.GetPagedSnippetList(snippetsPerPage, currentPage, userId,
                                                                                   userGuid);

            return returnList.OrderByDescending(d => d.LastModified);
        }

        public int CurrentSnippetCount(bool isPrivate)
        {
            return _snippetOperationsEngine.GetTotalSnippetCount(isPrivate);
        }

        public SnippetDTO GetSnippetByGuid(string guid)
        {
            SnippetDTO returnValue;
            Guid parsedGuid;
            if (Guid.TryParse(guid, out parsedGuid))
                returnValue = _snippetOperationsEngine.GetSnippetByGuid(parsedGuid);
            else
                returnValue = new SnippetDTO();

            return returnValue;
        }

        public IEnumerable<SnippetDTO> GetPrivateSnippets(int userId, Guid userGuid)
        {
            IEnumerable<SnippetDTO> returnList = _searchEngine.FindPrivateSnippetsForUser(userId, userGuid);

            return returnList.OrderByDescending(d => d.LastModified);
        }

        public IEnumerable<SnippetDTO> PerformSearch(string searchText, int userId, Guid userGuid, int currentPage = 1)
        {
            return PerformSearch(searchText, userId, userGuid, 0, currentPage);
        }

        public IEnumerable<SnippetDTO> PerformSearch(string searchText, int userId, Guid userGuid,
                                                     int snippetsPerPage = SNIPPETS_PER_PAGE, int currentPage = 1)
        {
            bool userExists = false;
            var returnList = new List<SnippetDTO>();
            if (userId > 0 && userGuid != Guid.Empty)
            {
                userExists = _userValidationEngine.CheckUserExists(userId, userGuid);
            }
            if (userExists)
                returnList.AddRange(
                    _searchEngine.FindPrivateSnippetsForUserPaged(userId, userGuid, snippetsPerPage,
                                                                  currentPage).OrderByDescending(d => d.LastModified).
                        Skip(snippetsPerPage*((currentPage - 1 >= 0) ? (currentPage - 1) : (currentPage))).Take(
                            snippetsPerPage));
            else
            {
                returnList.AddRange(
                    _searchEngine.FindPublicSnippets(searchText).OrderByDescending(d => d.LastModified).Skip(
                        snippetsPerPage*((currentPage - 1 >= 0) ? (currentPage - 1) : (currentPage))).Take(
                            snippetsPerPage));
            }
            return returnList.AsEnumerable();
        }

        public IEnumerable<HyperlinkDTO> GetHyperlinksForSnippet(Guid snippetGuid, int userId, Guid userGuid)
        {
            IEnumerable<HyperlinkDTO> response = _snippetOperationsEngine.GetHyperlinksForSnippet(snippetGuid, userId,
                                                                                                  userGuid);

            return response;
        }

        public IEnumerable<FileDTO> GetFilesForSnippet(Guid snippetGuid, int userId, Guid userGuid)
        {
            return _snippetOperationsEngine.GetFilesForSnippets(snippetGuid, userId, userGuid);
        }

        public UserDTO UserDetails(int userId, Guid userGuid)
        {
            UserDTO result = null;

            User response = _userOperationsEngine.GetUserDetails(userId, userGuid);
            if (response != null)
            {
                result = new UserDTO
                             {
                                 Id = response.Id,
                                 FormsAuthId = response.FormsAuthId,
                                 LoginName = response.LoginName,
                                 Email = response.Email,
                                 AvatarImage = response.AvatarImage
                             };
            }

            return result;
        }

        public IEnumerable<LanguageDTO> GetAllLanguages()
        {
            IEnumerable<LanguageDTO> languages;

            languages = _snippetOperationsEngine.GetAllLanguages().OrderBy(d => d.Name);

            return languages;
        }

        private byte[] GetPreviewData(byte[] fullData)
        {
            Guard.IntArgIsPositive(fullData.Length, "Snippet Data Raw");

            string data = Encoding.UTF8.GetString(fullData);
            string preview = string.Empty;

            Guard.ArgNotNullAndNotEmpty(data, "data");

            var sbPreview = new StringBuilder(preview);
            using (var sr = new StringReader(data))
                for (int i = 0; i < PREVIEW_DATA_LINE_COUNT; i++)
                {
                    sbPreview.AppendLine(sr.ReadLine());
                }

            return Encoding.UTF8.GetBytes(sbPreview.ToString());
        }
    }
}