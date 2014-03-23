using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using Ninject;
using SnippetCache.Data;
using SnippetCache.Utils.Validation;
using SnippetCache.WebUI.SnippetCacheDataService;

namespace SnippetCache.WebUI.Infrastructure.AccountData
{
    public class AccountRepository : IAccountRepository
    {
        public AccountRepository()
        {
            if (SnippetCacheModelContext == null)
                SnippetCacheModelContext = new SnippetCacheDataServiceClient();
            if (AspNetAccountServices == null)
                AspNetAccountServices = new AccountServicesDataContext();
        }

        public AccountRepository(ISnippetCacheDataService dataService)
        {
            SnippetCacheModelContext = dataService;
            AspNetAccountServices = new AccountServicesDataContext();
        }

        [Inject]
        public AccountServicesDataContext AspNetAccountServices { get; set; }

        public ISnippetCacheDataService SnippetCacheModelContext { get; set; }

        #region IAccountRepository Members

        public int GetUserId(string username)
        {
            GetUserByNameResponse response =
                SnippetCacheModelContext.GetUserByName(new GetUserByNameRequest {LoginName = username});
            if (response.Success)
            {
                return response.Id;
            }
            else
            {
                throw new DataException("Could not retrieve user id from data store");
            }
        }

        public Guid GetUserGuid(string username)
        {
            aspnet_Membership firstOrDefault = AspNetAccountServices.aspnet_Memberships.FirstOrDefault(
                d => d.aspnet_User.LoweredUserName.Equals(username.ToLower()));
            if (firstOrDefault != null)
                return firstOrDefault.UserId;
            else
                return Guid.Empty;
        }

        public bool UserIsInRole(int userId, Guid userGuid, string role)
        {
            string userName = string.Empty;

            aspnet_User firstOrDefault =
                AspNetAccountServices.aspnet_Users.FirstOrDefault(d => d.UserId.Equals(userGuid));
            if (firstOrDefault != null)
            {
                userName = firstOrDefault.LoweredUserName;
            }

            ISingleResult<aspnet_UsersInRoles_GetRolesForUserResult> roles =
                AspNetAccountServices.aspnet_UsersInRoles_GetRolesForUser("/", userName);
            return roles != null && roles.Any(d => d.RoleName.Equals(role));
        }

        public void AssignUserToRole(string userName, string role)
        {
            Guard.ArgNotNull(role, "role");
            Guard.ArgNotNull(userName, "userName");

            aspnet_User newUser = AspNetAccountServices.aspnet_Users.FirstOrDefault(d => d.UserName.Equals(userName));
            aspnet_Role registeredUserRoleId =
                AspNetAccountServices.aspnet_Roles.FirstOrDefault(d => d.RoleName == "RegisteredUser");

            if (registeredUserRoleId != null)
                if (newUser != null)
                    AspNetAccountServices.aspnet_UsersInRoles.InsertOnSubmit(
                        new aspnet_UsersInRole
                            {
                                RoleId = registeredUserRoleId.RoleId,
                                UserId = newUser.UserId,
                                aspnet_Role = registeredUserRoleId,
                                aspnet_User = newUser
                            });

            AspNetAccountServices.SubmitChanges(ConflictMode.ContinueOnConflict);
        }

        #endregion

        #region Implementation of IRepository<IUser>

        public IEnumerable<IUser> Get(Expression<Func<IUser, bool>> filter = null,
                                      Func<IQueryable<IUser>, IOrderedQueryable<IUser>> orderBy = null,
                                      string includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public IUser GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(IUser e)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(IUser e)
        {
            throw new NotImplementedException();
        }

        public void Update(IUser e)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}