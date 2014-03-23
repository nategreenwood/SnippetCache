using System;
using System.Collections.Generic;
using System.Linq;
using SnippetCache.Data.DTO;
using SnippetCache.Engines.DataService;

namespace SnippetCache.Engines
{
    public class SearchEngine : IEngine
    {
        private readonly ISnippetCacheDataService _dataService;

        public SearchEngine()
        {
        }

        public SearchEngine(ISnippetCacheDataService dataService)
        {
            _dataService = dataService;
        }

        #region IEngine Members

        public string EngineTypeName
        {
            get { return "SearchEngine"; }
        }

        #endregion

        public IEnumerable<SnippetDTO> GetPagedSnippetList(int snippetsPerPage, int currentPage)
        {
            GetPagedSnippetListResponse returnList = _dataService.GetPagedSnippetList(
                new GetPagedSnippetListRequest {CurrentPage = currentPage, SnippetsPerPage = snippetsPerPage});


            return returnList.Snippets;
        }

        public IEnumerable<SnippetDTO> GetPagedSnippetList(int snippetsPerPage, int currentPage, int userId,
                                                           Guid userGuid)
        {
            GetPagedSnippetListResponse response = _dataService.GetPagedSnippetList(new GetPagedSnippetListRequest
                                                                                        {
                                                                                            SnippetsPerPage =
                                                                                                snippetsPerPage,
                                                                                            CurrentPage = currentPage,
                                                                                            UserId = userId,
                                                                                            UserGuid = userGuid
                                                                                        });
            return response.Snippets;
        }

        public IEnumerable<SnippetDTO> FindPublicSnippets(string queryText)
        {
            var returnList = new List<SnippetDTO>();

            GetSnippetsByTitleResponse titleTextMatches =
                _dataService.GetSnippetsByTitle(new GetSnippetsByTitleRequest {TitleText = queryText});
            GetSnippetsByDescriptionResponse descriptionTextMatches =
                _dataService.GetSnippetsByDescription(new GetSnippetsByDescriptionRequest {DescriptionText = queryText});
            GetSnippetsByDataResponse dataTextMatches =
                _dataService.GetSnippetsByData(new GetSnippetsByDataRequest {DataString = queryText});

            if (titleTextMatches.Snippets != null && titleTextMatches.Snippets.Any())
                returnList.AddRange(titleTextMatches.Snippets);

            if (descriptionTextMatches.Snippets != null && descriptionTextMatches.Snippets.Any())
                returnList.AddRange(descriptionTextMatches.Snippets);

            if (dataTextMatches.Snippets != null && dataTextMatches.Snippets.Any())
                returnList.AddRange(dataTextMatches.Snippets);

            return returnList;
        }

        public IEnumerable<SnippetDTO> FindPrivateSnippetsForUser(int userId, Guid userGuid)
        {
            var returnList = new List<SnippetDTO>();

            GetSnippetsByUserIdResponse userSnippets =
                _dataService.GetSnippetsByUserId(new GetSnippetsByUserIdRequest
                                                     {UserId = userId, User_FormsAuthId = userGuid});
            if (userSnippets.Snippets != null && userSnippets.Snippets.Any())
            {
                returnList.AddRange(userSnippets.Snippets.Where(d => d.IsPublic == false));
            }

            return returnList;
        }

        public IEnumerable<SnippetDTO> FindPrivateSnippetsForUserPaged(int userId, Guid userGuid, int snippetsPerPage,
                                                                       int currentPage)
        {
            var returnList = new List<SnippetDTO>();

            GetPagedSnippetListResponse userSnippets = _dataService.GetPagedSnippetList(new GetPagedSnippetListRequest
                                                                                            {
                                                                                                UserId = userId,
                                                                                                UserGuid = userGuid,
                                                                                                CurrentPage =
                                                                                                    currentPage,
                                                                                                SnippetsPerPage =
                                                                                                    snippetsPerPage
                                                                                            });
            return returnList;
        }
    }
}