using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SnippetCache.Data;

namespace SnippetCache.Web.Controllers
{
    public class HomeController : Controller
    {
        private SnippetCache.Data.SnippetCacheDbContext context = new SnippetCacheDbContext();
        public ActionResult Index()
        {
            var language = context.Languages.Where(x => x.Name.Equals("C#"));
            return View();
        }
    }
}
