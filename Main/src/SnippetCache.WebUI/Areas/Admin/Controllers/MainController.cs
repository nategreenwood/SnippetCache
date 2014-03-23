using System.Web.Mvc;

namespace SnippetCache.WebUI.Areas.Admin.Controllers
{
    public class MainController : Controller
    {
        //
        // GET: /Admin/Home/
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}