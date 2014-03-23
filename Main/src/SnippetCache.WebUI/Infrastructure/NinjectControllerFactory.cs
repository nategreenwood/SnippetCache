using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using SnippetCache.Utils.Security;
using SnippetCache.WebUI.Infrastructure.AccountData;

//using SnippetCache.WebUI.Infrastructure.AccountData;

namespace SnippetCache.WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _ninjectKernel;

        public NinjectControllerFactory()
        {
            _ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController) _ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            _ninjectKernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
            _ninjectKernel.Bind<IAccountRepository>().To<AccountRepository>();
        }
    }
}