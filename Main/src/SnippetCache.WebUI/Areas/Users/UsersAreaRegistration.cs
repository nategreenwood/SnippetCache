using System.Web.Mvc;

namespace SnippetCache.WebUI.Areas.Users
{
    public class UsersAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Users"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "",
                "Users/{controller}/{action}/{id}",
                new {action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}