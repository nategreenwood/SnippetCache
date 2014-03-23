using System;
using System.Web.Helpers;
using System.Web.Mvc;
//using SnippetCache.Data.DTO;
using SnippetCache.Utils.Logging;
using SnippetCache.WebUI.ManagerService;

namespace SnippetCache.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISnippetCacheManagerService _managerService;

        public HomeController()
        {
            if (_managerService == null)
            {
                _managerService = new SnippetCacheManagerServiceClient();
            }
        }

        public HomeController(ISnippetCacheManagerService managerService)
        {
            _managerService = managerService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("All", "Public", new { area = "Snippets" });
        }

        [HttpPost]
        public EmptyResult UpdatePageTheme(string style)
        {
            if (!string.IsNullOrEmpty(style))
            {
                Session["theme"] = style;
            }

            return new EmptyResult();
        }

        public ActionResult GetUserThumbnailImage(int userId, Guid userGuid)
        {
            try
            {
                UserDTO user = _managerService.UserDetails(userId, userGuid);
                var img = new WebImage(user.AvatarImage);
                img.Resize(32, 32, true, true);

                return File(img.GetBytes("png"), "image/png");
            }
            catch (Exception ex)
            {
                Logger.LogError("GetUserImage: Exception while retrieving user avatar image data", ex);
                return null;
            }
        }
    }
}