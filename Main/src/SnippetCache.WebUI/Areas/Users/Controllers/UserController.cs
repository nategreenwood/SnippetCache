using System;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

using SnippetCache.Data.DTO;
using SnippetCache.Utils.Logging;
using SnippetCache.WebUI.Areas.Users.Models;
using SnippetCache.WebUI.Infrastructure.AccountData;
using SnippetCache.WebUI.ManagerService;
using System.Collections;
using System.Collections.Generic;

using NotificationDTO = SnippetCache.WebUI.ManagerService.NotificationDTO;
using UserDTO = SnippetCache.WebUI.ManagerService.UserDTO;

namespace SnippetCache.WebUI.Areas.Users.Controllers
{
    public class UserController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ISnippetCacheManagerService _managerService;
        private IDictionary<int, UserDTO> _userCache;

        public UserController()
        {
            if (_managerService == null)
                _managerService = new SnippetCacheManagerServiceClient();

            if (_accountRepository == null)
                _accountRepository = new AccountRepository();

            if(_userCache == null)
                _userCache = new Dictionary<int, UserDTO>();
        }

        public UserController(ISnippetCacheManagerService managerService, IAccountRepository accountRepository)
        {
            _managerService = managerService;
            _accountRepository = accountRepository;
        }

        [HttpGet, ActionName("Details")]
        public ActionResult Index(int userId, Guid userGuid)
        {
            var model = new UserModel.UserDetailsModel();
            UserDTO user = _managerService.UserDetails(userId, userGuid);

            if (user != null)
            {
                model.UserName = user.LoginName;
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, RegisteredUser")]
        public ActionResult Profile( /*int profileId, string profileGuid*/)
        {
            // Check to see if the user requesting the profile is the same user. 
            // If not, show them the details page so they can't edit it.

            int userId = ((CustomIdentity) User.Identity).UserId;
            Guid userGuid = ((CustomIdentity) User.Identity).UserGuid;

            //if (userId != profileId || !(Guid.Parse(profileGuid).Equals(userGuid)))
            //    RedirectToAction("Details", new {userId = profileId, userGuid = Guid.Parse(profileGuid)});

            UserDTO profileDetails = _managerService.UserDetails(userId, userGuid);
            var model = new UserModel.EditUserProfileModel
                            {
                                UserId = userId,
                                UserGuid = userGuid,
                                UserName = profileDetails.LoginName,
                                Email = profileDetails.Email,
                                AvatarImage = profileDetails.AvatarImage,
                                OriginalPassword = string.Empty,
                                NewPassword = string.Empty,
                                NewPasswordConfirmation = string.Empty
                            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, RegisteredUser")]
        public ActionResult Profile(UserModel.EditUserProfileModel model, HttpPostedFileBase image)
        {
            UserDTO details = _managerService.UserDetails(model.UserId, model.UserGuid);
            if (details != null)
            {
                details.Id = model.UserId;
                details.FormsAuthId = model.UserGuid;
                details.Email = model.Email;
                details.AvatarImage = (image != null) ? new byte[image.ContentLength] : details.AvatarImage;
                if (image != null) image.InputStream.Read(details.AvatarImage, 0, image.ContentLength);
            }

            try
            {
                bool result = _managerService.UpdateUserDetails(details);
                if (result)
                {
                    // Send notification
                    var notification = new NotificationDTO
                                           {
                                               NotificationType_Id = 1,
                                               User_FormsAuthId = model.UserGuid,
                                               User_Id = model.UserId,
                                               Text = "Your profile settings were updated",
                                               DateCreated = DateTime.UtcNow
                                           };
                    _managerService.CreateNewUserNotification(notification);

                    TempData["Message"] = "User settings saved";
                    return RedirectToAction("Profile", model);
                }
                else
                {
                    TempData["Error"] = "Error saving data";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error saving user profile data. User image too large.", ex);
                TempData["Error"] = "Error saving user profile data. User image too large.";
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, RegisteredUser")]
        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Any, VaryByParam = "userId")]
        public FileContentResult GetImage(int userId, Guid userGuid)
        {
            UserDTO user;
            if (!_userCache.ContainsKey(userId))
            {
                user = _managerService.UserDetails(userId, userGuid);
                if (user != null && user.AvatarImage != null)
                {
                    _userCache.Add(userId, user);
                    return new FileContentResult(user.AvatarImage, "image/png");
                }
            }
            else
            {
                return new FileContentResult(_userCache[userId].AvatarImage, "image/png");
            }

            return null;
        }
    }
}