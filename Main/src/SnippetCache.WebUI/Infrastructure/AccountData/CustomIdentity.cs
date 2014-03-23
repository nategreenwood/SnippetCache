using System;
using System.Security.Principal;
using System.Web.Security;
using Ninject;

namespace SnippetCache.WebUI.Infrastructure.AccountData
{
    public class CustomIdentity : IIdentity
    {
        private readonly FormsAuthenticationTicket _ticket;

        public CustomIdentity(FormsAuthenticationTicket ticket)
        {
            _ticket = ticket;
            var kernel = new StandardKernel();
            AccountRepository = new AccountRepository();
            kernel.Inject(AccountRepository);
        }

        [Inject]
        public IAccountRepository AccountRepository { get; set; }

        public FormsAuthenticationTicket Ticket
        {
            get { return _ticket; }
        }

        public int UserId
        {
            get { return AccountRepository.GetUserId(_ticket.Name); }
        }

        public Guid UserGuid
        {
            get { return AccountRepository.GetUserGuid(_ticket.Name); }
        }

        #region IIdentity Members

        public string AuthenticationType
        {
            get { return "Custom"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get { return _ticket.Name; }
        }

        #endregion
    }
}