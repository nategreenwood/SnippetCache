using System.Web.Mvc;

namespace SnippetCache.WebUI.Areas.Snippets
{
    public class SnippetsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Snippets"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(null, "Snippets/{controller}/{action}/{page}",
                             new {Controller = "Public", action = "Index", page = 1},
                             new {page = @"\d+"});

            context.MapRoute(
                null,
                "Snippets/{controller}/{action}/{id}",
                new {controller = "Public", action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}