using System;
using System.IO;
using System.Security.Authentication;
using System.Web.Mvc;
using System.Web.Security;
using Ninject;
using SnippetCache.Data.DTO;
using SnippetCache.Utils.Logging;
using SnippetCache.Utils.Security;
using SnippetCache.Utils.Validation;
using SnippetCache.WebUI.Infrastructure.AccountData;
using SnippetCache.WebUI.ManagerService;
using SnippetCache.WebUI.Models;
using SnippetCache.WebUI.SnippetCacheDataService;

using NotificationDTO = SnippetCache.WebUI.ManagerService.NotificationDTO;

namespace SnippetCache.WebUI.Controllers
{
    public class AccountController : Controller
    {
        [Inject] private readonly IAccountRepository _accountRepository;
        private readonly IAuthProvider _authProvider;

        private readonly ISnippetCacheManagerService _managerService;

        public AccountController(IAuthProvider authProvider, IAccountRepository accountRepository)
        {
            Guard.ArgNotNull(authProvider, "authProvider");
            Guard.ArgNotNull(accountRepository, "accountRepository");

            _authProvider = authProvider;
            _accountRepository = accountRepository;
            if (_managerService == null)
                _managerService = new SnippetCacheManagerServiceClient();
        }

        public AccountController(IAuthProvider authProvider, IAccountRepository accountRepository,
                                 ISnippetCacheManagerService managerService)
        {
            Guard.ArgNotNull(authProvider, "authProvider");
            Guard.ArgNotNull(accountRepository, "accountRepository");
            Guard.ArgNotNull(managerService, "managerService");

            _authProvider = authProvider;
            _accountRepository = accountRepository;
            _managerService = managerService;
        }

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (_authProvider.Authenticate(model.UserName, model.Password))
                {
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("All", "Public", new {area = "Snippets"});
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null,
                                      out createStatus);


                if (createStatus == MembershipCreateStatus.Success)
                {
                    // Put into default Role "RegisteredUser". "Administrators" are flagged manually 
                    // in the database.
                    _accountRepository.AssignUserToRole(model.UserName, "RegisteredUser");

                    // Add user data to the custom model database
                    var snippetData = new SnippetCacheDataServiceClient();
                    var aspnetData = new AccountRepository(snippetData);
                    const string defaultImagePath = "~/Content/images/default_user_avatar.png";

                    {
                        using (
                            var avatarStream =
                                new FileStream(System.Web.HttpContext.Current.Server.MapPath(defaultImagePath),
                                               FileMode.Open))
                        {
                            var buffer = new byte[avatarStream.Length];
                            avatarStream.Read(buffer, 0, (int) avatarStream.Length);
                            var request = new CreateNewUserRequest
                                              {
                                                  LoginName = model.UserName,
                                                  Email = model.Email,
                                                  FormsAuthId = aspnetData.GetUserGuid(model.UserName),
                                                  //AvatarImage = Encoding.UTF8.GetBytes(Url.Content("~/Content/images/default_user_avatar.png"))
                                                  AvatarImage = buffer
                                              };
                            CreateNewUserResponse response = snippetData.CreateNewUser(request);
                            if (!response.Success)
                            {
                                const string message = "Failure creating new user.";
                                var e = new Exception(response.FailureInformation);
                                Logger.LogError(message, e);
                                throw new AuthenticationException(message, e);
                            }

                            // Create welcome message for user's inbox
                            _managerService.CreateNewUserNotification(new NotificationDTO
                                                                          {
                                                                              NotificationType_Id = 1,
                                                                              User_FormsAuthId = response.FormsAuthId,
                                                                              User_Id = response.Id,
                                                                              Text = "Welcome to SnippetCache!",
                                                                              DateCreated = DateTime.UtcNow
                                                                          });
                        }

                        FormsAuthentication.SetAuthCookie(model.UserName, false);
                    }
                    ;

                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("Error", "Please correct any issues and try again.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return
                        "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return
                        "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return
                        "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return
                        "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        #endregion
    }
}