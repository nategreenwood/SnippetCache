using System;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using SnippetCache.Utils.Logging;
using SnippetCache.WebUI.Infrastructure;
using SnippetCache.WebUI.Infrastructure.AccountData;

namespace SnippetCache.WebUI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static string AnonoymousUserName
        {
            get { return "Anonymous"; }
        }

        public static int AnonymousUserId
        {
            get { return 4; }
        }

        public static Guid AnonymousUserGuid
        {
            get { return Guid.Parse("DBE62781-B3EA-4743-97B1-E53E67FCC562"); }
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
#if PROFILE
            filters.Add(new ProfileAttribute());
#endif
#if LOGALL
            filters.Add(new LogAllAttribute());
#endif
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                null,
                new[] { "SnippetCache.WebUI.Controllers","SnippetCache.WebUI.Areas.Snippets.Controllers" });
        }

        protected void Application_Start()
        {
            Logger.LogInfo("Application starting", LogType.General);

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            // Let Ninject handle creating new controller instances so DI can occur.
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
        }

        protected void Application_AuthenticateRequest()
        {
            IPrincipal user = HttpContext.Current.User;
            IAccountRepository accountRepository = new AccountRepository();
            if (user != null && user.Identity.IsAuthenticated && user.Identity.AuthenticationType.Equals("Forms"))
            {
                var formsIdentity = user.Identity as FormsIdentity;
                if (formsIdentity != null)
                {
                    var customIdentity = new CustomIdentity(formsIdentity.Ticket);
                    var customPrincipal = new CustomPrincipal(customIdentity, accountRepository);

                    HttpContext.Current.User = customPrincipal;
                    Thread.CurrentPrincipal = customPrincipal;
                }
            }
        }

        protected void Application_Error()
        {
            const string errorText = "Unknown application error occurred";
            Logger.LogError(errorText, new Exception(errorText));
        }

        public override string GetVaryByCustomString(HttpContext context, string arg)
        {
            if (arg.ToLower() == "sessionid")
            {
                HttpCookie cookie = context.Request.Cookies["ASP.NET_SessionID"];
                if (cookie != null)
                    return cookie.Value;
            }
            return base.GetVaryByCustomString(context, arg);
        }
    }
}