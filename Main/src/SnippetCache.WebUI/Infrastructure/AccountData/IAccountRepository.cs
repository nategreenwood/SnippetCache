using System;
using SnippetCache.Data;

namespace SnippetCache.WebUI.Infrastructure.AccountData
{
    public interface IAccountRepository : IRepository<IUser>
    {
        int GetUserId(string username);
        Guid GetUserGuid(string username);
        bool UserIsInRole(int userId, Guid userGuid, string role);
        void AssignUserToRole(string userName, string role);
    }
}