using System.Web.Mvc;

namespace SnippetCache.WebUI.Areas.Messages
{
    public class MessagesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Messages"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Messages_default",
                "Messages/{controller}/{action}/{id}",
                new {action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}