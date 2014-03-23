using System;
using SnippetCache.Engines.DataService;

namespace SnippetCache.Engines
{
    public class UserValidationEngine : IEngine
    {
        private readonly ISnippetCacheDataService _dataService;

        public UserValidationEngine(ISnippetCacheDataService dataService)
        {
            _dataService = dataService;
        }

        #region IEngine Members

        public string EngineTypeName
        {
            get { return "UserValidationEngine"; }
        }

        #endregion

        public bool CheckUserExists(int userId, Guid userGuid)
        {
            bool exists = false;

            GetUserByIdResponse response =
                _dataService.GetUserById(new GetUserByIdRequest {Id = userId, FormsAuthId = userGuid});

            return response.Success;
        }
    }
}