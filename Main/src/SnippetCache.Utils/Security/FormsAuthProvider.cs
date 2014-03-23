using System.Web.Security;

namespace SnippetCache.Utils.Security
{
    public class FormsAuthProvider : IAuthProvider
    {
        #region IAuthProvider Members

        public bool Authenticate(string username, string password)
        {
            bool result = Membership.ValidateUser(username, password);
            if (result)
                FormsAuthentication.SetAuthCookie(username, false);

            return result;
        }

        #endregion
    }
}