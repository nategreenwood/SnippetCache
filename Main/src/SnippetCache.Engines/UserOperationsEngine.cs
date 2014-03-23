using System;
using SnippetCache.Data.EF4.Model;
using SnippetCache.Engines.DataService;

namespace SnippetCache.Engines
{
    public class UserOperationsEngine : IEngine
    {
        private readonly ISnippetCacheDataService _dataService;

        public UserOperationsEngine()
        {
        }

        public UserOperationsEngine(ISnippetCacheDataService dataService)
        {
            _dataService = dataService;
        }

        #region IEngine Members

        public string EngineTypeName
        {
            get { return "UserOperationsEngine"; }
        }

        #endregion

        public User GetUserDetails(int userId, Guid userGuid)
        {
            User user = null;

            GetUserByIdResponse response =
                _dataService.GetUserById(new GetUserByIdRequest {Id = userId, FormsAuthId = userGuid});
            if (response.Id > 0)
                user = new User
                           {
                               Id = response.Id,
                               FormsAuthId = response.FormsAuthId,
                               Email = response.Email,
                               LoginName = response.LoginName,
                               AvatarImage = response.AvatarImage
                           };
            return user;
        }

        public bool UpdateUserDetails(User user)
        {
            UpdateUserResponse result = _dataService.UpdateUser(new UpdateUserRequest
                                                                    {
                                                                        Id = user.Id,
                                                                        FormsAuthId = user.FormsAuthId,
                                                                        Email = user.Email,
                                                                        LoginName = user.LoginName,
                                                                        AvatarImage = user.AvatarImage,
                                                                    });
            return result.Success;
        }
    }
}