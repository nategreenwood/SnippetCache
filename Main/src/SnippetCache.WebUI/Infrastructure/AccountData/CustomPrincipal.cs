using System.Security.Principal;

namespace SnippetCache.WebUI.Infrastructure.AccountData
{
    public class CustomPrincipal : IPrincipal
    {
        private readonly IAccountRepository _accountRepository;
        private readonly CustomIdentity _identity;

        public CustomPrincipal()
        {
            if (_accountRepository == null)
                _accountRepository = new AccountRepository();
        }

        public CustomPrincipal(CustomIdentity identity, IAccountRepository accountRepository)
            : this()
        {
            _identity = identity;
            _accountRepository = accountRepository;
        }

        #region IPrincipal Members

        public IIdentity Identity
        {
            get { return _identity; }
        }

        public bool IsInRole(string role)
        {
            bool isInRole = false;
            if (!string.IsNullOrEmpty(role))
                isInRole = _accountRepository.UserIsInRole(_identity.UserId, _identity.UserGuid, role);

            return isInRole;
        }

        #endregion
    }
}