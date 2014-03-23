using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
//using SnippetCache.Data.DTO;
using SnippetCache.WebUI.Areas.Messages.Models;
using SnippetCache.WebUI.Infrastructure.AccountData;
using SnippetCache.WebUI.ManagerService;

namespace SnippetCache.WebUI.Areas.Messages.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly ISnippetCacheManagerService _managerService;

        public MessageController()
        {
            if (_managerService == null)
                _managerService = new SnippetCacheManagerServiceClient();
        }

        public MessageController(ISnippetCacheManagerService managerService)
        {
            _managerService = managerService;
        }

        [HttpGet]
        //[OutputCache(Duration = 180)]
        public ActionResult UserMessages(int userId, string userGuid)
        {
            var model = new List<UserMessagesModel>();

            if (User.Identity.IsAuthenticated)
            {
                NotificationDTO[] notifications = _managerService.UnreadUserNotification(userId, Guid.Parse(userGuid));

                var user = User.Identity as CustomIdentity;
                if (user == null)
                    return View(model);

                foreach (NotificationDTO notificationDTO in notifications)
                {
                    var temp = new UserMessagesModel
                                   {
                                       Id = notificationDTO.Id,
                                       Text = notificationDTO.Text,
                                       DateCreated = notificationDTO.DateCreated,
                                       //Remove = false
                                   };
                    model.Add(temp);
                }
            }

            return View(model);
        }

        [HttpPost]
        //[Authorize(Roles="Administrators, RegisteredUsers")]
        public ActionResult UserMessages(FormCollection model)
        {
            int userId = ((CustomIdentity) User.Identity).UserId;
            Guid userGuid = ((CustomIdentity) User.Identity).UserGuid;

            if (model != null && model.Keys.Count > 0)
            {
                foreach (string strMessageId in model.Keys)
                {
                    if (!string.IsNullOrEmpty(strMessageId))
                    {
                        _managerService.RemoveUserNotification(new NotificationDTO
                                                                   {
                                                                       Id = int.Parse(strMessageId)
                                                                   });
                    }
                }
            }

            TempData["Message"] = "User notifications have been removed";
            return RedirectToAction("UserMessages", new {area = "Messages", userId, userGuid = userGuid.ToString()});
        }

        [ChildActionOnly]
        [ActionName("UserMessageCount")]
        public ActionResult UnreadMessageCountMessage(int userId, string userGuid)
        {
            NotificationDTO[] messages = _managerService.UnreadUserNotification(userId, Guid.Parse(userGuid));

            return View((object) messages.Count().ToString(CultureInfo.InvariantCulture));
        }
    }
}