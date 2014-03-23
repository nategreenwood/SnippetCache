using System.Web.Mvc;

namespace SnippetCache.WebUI.Areas.Files
{
    public class FilesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Files"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Files_default",
                "Files/{controller}/{action}/{id}",
                new {action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}