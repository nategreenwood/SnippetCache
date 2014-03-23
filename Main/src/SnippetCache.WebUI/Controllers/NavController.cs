using System.Web.Mvc;
using SnippetCache.Data;
using SnippetCache.Data.EF4.Model;

namespace SnippetCache.WebUI.Controllers
{
    public class NavController : Controller
    {
        //private readonly IRepository<Language> _repo;

        public NavController(IRepository<Language> repo)
        {
            //_repo = repo;
        }

        public PartialViewResult Menu(string language = null)
        {
            ViewBag.SelectedLanguage = language;

            //  IEnumerable<Language> languages = _repo.Entities.Distinct().OrderBy(d => d.Name);

            //return PartialView(languages);
            return null;
        }
    }
}