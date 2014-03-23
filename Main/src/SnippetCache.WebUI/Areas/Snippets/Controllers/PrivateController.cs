using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Web;
using System.Web.Mvc;
//using SnippetCache.Data.DTO;
using SnippetCache.Utils.Logging;
using SnippetCache.WebUI.Areas.Snippets.Models;
using SnippetCache.WebUI.Infrastructure.AccountData;
using SnippetCache.WebUI.ManagerService;

namespace SnippetCache.WebUI.Areas.Snippets.Controllers
{

    [Authorize]
    public class PrivateController : Controller
    {
        private readonly ISnippetCacheManagerService _managerService;
        private const int MAX_SNIPPETS_PER_PAGE = 6;
        private readonly IEnumerable<LanguageDTO> _displayLanguages;

        public PrivateController()
        {
            if (_managerService == null)
                _managerService = new SnippetCacheManagerServiceClient();
            _displayLanguages = _managerService.GetLanguagesForDisplay();
        }

        public PrivateController(ISnippetCacheManagerService service)
        {
            if (service != null)
                _managerService = service;
        }

        internal IList<string> UploadedFiles
        {
            get
            {
                if (Session["UploadedFiles"] == null || ((IList<string>)Session["UploadedFiles"]).Count <= 0)
                {
                    Session["UploadedFiles"] = new List<string>();
                }

                return (List<string>)Session["UploadedFiles"];
            }
            set { Session["UploadedFiles"] = value; }
        }

        internal CreateSnippetModel CurrentSnippet
        {
            get
            {
                if (Session["CurrentSnippet"] == null)
                    Session["CurrentSnippet"] = new CreateSnippetModel(_displayLanguages);
                return (CreateSnippetModel)Session["CurrentSnippet"];
            }
            set
            {
                if (Session["CurrentSnippet"] == null)
                    Session["CurrentSnippet"] = value;
            }
        }

        [ActionName("Create")]
        [Authorize(Roles = "RegisteredUser, Administrator")]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CreateNewPrivateSnippet()
        {
            return View(new CreateSnippetModelPrivate(_displayLanguages));
        }

        [ActionName("Create")]
        [Authorize(Roles = "RegisteredUser, Administrator")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateNewPrivateSnippet(CreateSnippetModelPrivate inModel)
        {
            UserDTO user = GetCurrentUserData();
            var model = new CreateSnippetModel(_displayLanguages);

            if (ModelState.IsValid)
            {
                try
                {
                    var newSnippet = new SnippetDTO
                    {
                        Guid = Guid.NewGuid(),
                        Name = inModel.Name,
                        Description = inModel.Description,
                        Data = Encoding.UTF8.GetBytes(inModel.Data),
                        IsPublic = false,
                        Language_Id = inModel.LanguageId,
                        LastModified = DateTime.UtcNow,
                        User_Id = user.Id,
                        User_FormsAuthId = user.FormsAuthId
                    };

                    var files = new List<FileDTO>();
                    if (CurrentSnippet.UploadModel.Files.Any())
                    {
                        for (int i = 0; i < CurrentSnippet.UploadModel.Files.Count(); i++)
                        {
                            NamedFileWithDescription file =
                                ((List<NamedFileWithDescription>)CurrentSnippet.UploadModel.Files)[i];
                            var temp = new FileDTO
                            {
                                Name = file.Name,
                                Description = file.Description,
                                Data = file.File.Data,
                                LastModified = DateTime.UtcNow
                            };
                            files.Add(temp);
                        }
                    }

                    // The new Guid and snippet id should be returned to us
                    newSnippet = _managerService.CreateSnippet(
                        newSnippet,
                        new LanguageDTO { Id = inModel.LanguageId },
                        user,
                        files.ToArray(),
                        CurrentSnippet.Hyperlinks.ToArray());


                    var detailsModel = new SnippetDetailsModel
                    {
                        UserId = user.Id,
                        UserGuid = user.FormsAuthId,
                        UserName = user.LoginName,
                        UserAvatar = user.AvatarImage,
                        PreviewData = Encoding.UTF8.GetString(newSnippet.PreviewData),
                        Data = Encoding.UTF8.GetString(newSnippet.Data),
                        Name = newSnippet.Name,
                        Description = newSnippet.Description,
                        IsPublic = inModel.IsPublic,
                        SnippetId = newSnippet.Guid,
                        SnippetUrl = newSnippet.Guid.ToString(),
                        Languages = _displayLanguages,
                        Files =
                            CurrentSnippet.UploadModel.Files ??
                            new List<NamedFileWithDescription>(),
                        Hyperlinks =
                            CurrentSnippet.Hyperlinks ?? new List<HyperlinkDTO>(),
                        LanguageId = inModel.LanguageId
                    };
                    UploadedFiles = null;
                    CurrentSnippet.Hyperlinks = null;
                    TempData["Message"] = "Saved Snippet Id: " + newSnippet.Guid + " Successfully";

                    return PartialView("_SnippetDetailsTablePartial", detailsModel);
                }
                catch (Exception ex)
                {
                    Logger.LogError("CreateNewSnippet action method failed", ex);
                    if (inModel.Languages == null) inModel.Languages = _displayLanguages;
                    if (inModel.Hyperlinks == null) inModel.Hyperlinks = CurrentSnippet.Hyperlinks;
                    if (inModel.UploadModel == null) inModel.UploadModel = CurrentSnippet.UploadModel;
                    TempData["Error"] = "Unknown error occurred.";
                    return View(inModel);
                }
            }
            else
            {
                inModel.Languages = _displayLanguages;
                TempData["Error"] = "Please fix any errors to continue";
                return View(inModel);
            }
        }

        [ActionName("All")]
        [Authorize(Roles = "RegisteredUser, Administrator")]
        public ActionResult Index(int page = 1)
        {
            var viewModel = new List<SnippetDetailsModel>();
            IEnumerable<SnippetDTO> snippetList;
            var user = User.Identity as CustomIdentity;

            try
            {
                if (user != null)
                {
                    snippetList = _managerService.GetPrivateSnippets(user.UserId, user.UserGuid);

                    if (snippetList != null && snippetList.Any())
                    {
                        viewModel.AddRange(from snippetDTO in snippetList
                                           let currentUser =
                                               _managerService.UserDetails(snippetDTO.User_Id,
                                                                           snippetDTO.User_FormsAuthId)
                                           select new SnippetDetailsModel
                                                      {
                                                          Name = snippetDTO.Name,
                                                          Description = snippetDTO.Description,
                                                          SnippetUrl = snippetDTO.Guid.ToString(),
                                                          PreviewData =
                                                              Encoding.UTF8.GetString(snippetDTO.PreviewData),
                                                          UserName =
                                                              (user != null && currentUser.Id > 0)
                                                                  ? currentUser.LoginName
                                                                  : "Anonymous",
                                                          UserAvatar =
                                                              (user != null && currentUser.Id > 0)
                                                                  ? currentUser.AvatarImage
                                                                  : null,
                                                          UserId = currentUser.Id,
                                                          UserGuid = currentUser.FormsAuthId,
                                                          IsPublic = snippetDTO.IsPublic
                                                      });
                    }
                }
                else
                {
                    // Make an empty list so as not to crash the view
                    snippetList = new List<SnippetDTO>();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("IndexError", ex);
                ViewBag.Message = ex.Message;
                return View(viewModel);
            }

            return View(viewModel);
        }

        [ActionName("Snippet")]
        [Authorize(Roles = "RegisteredUser, Administrator")]
        public ActionResult SingleSnippet(string guid)
        {
            var viewModel = new SnippetDetailsModel();
            SnippetDTO snippet = _managerService.GetSnippet(guid);
            try
            {
                if (snippet != null)
                {
                    UserDTO user = _managerService.UserDetails(snippet.User_Id, snippet.User_FormsAuthId);
                    if (user == null)
                        throw new SecurityException("Unable to retrieve user information for this private snippet.");

                    viewModel = new SnippetDetailsModel
                                    {
                                        Name = snippet.Name,
                                        Description = snippet.Description,
                                        SnippetUrl = snippet.Guid.ToString(),
                                        PreviewData =
                                            Encoding.UTF8.GetString(snippet.PreviewData),
                                        UserName =
                                            (user != null && snippet.Id > 0)
                                                ? user.LoginName
                                                : "Anonymous",
                                        UserAvatar =
                                            (user != null && snippet.Id > 0)
                                                ? user.AvatarImage
                                                : null,
                                        UserId = snippet.Id,
                                        UserGuid = user.FormsAuthId,
                                        IsPublic = snippet.IsPublic
                                    };
                }
            }
            catch (SecurityException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("IndexError", ex);
                ViewBag.Message = ex.Message;
                return View(viewModel);
            }

            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Administrator, RegisteredUser")]
        public EmptyResult UploadPrivateFiles()
        {
            string sDirectory =
                Request.RequestContext.HttpContext.Server.MapPath(
                    Request.RequestContext.HttpContext.Request["TempFileUploads"]);
            if (!Directory.Exists(sDirectory))
            {
                Directory.CreateDirectory(sDirectory);
            }
            HttpRequestBase request = Request.RequestContext.HttpContext.Request;
            string requestType = request.Headers["Wijmo-RequestType"];
            if (!String.IsNullOrEmpty(requestType) && requestType == "XMLHttpRequest")
            {
                string fileName = request.Headers["Wijmo-FileName"];
                using (var fs = new FileStream(sDirectory + "\\" + fileName, FileMode.Create))
                {
                    Stream inputStream = Request.RequestContext.HttpContext.Request.InputStream;
                    var bytes = new byte[(int)inputStream.Length];
                    inputStream.Read(bytes, 0, (int)inputStream.Length);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                }

                UploadedFiles.Add(fileName);

                using (var fs = new FileStream(sDirectory + "\\" + fileName, FileMode.Open))
                using (var fr = new BinaryReader(fs))
                {
                    ((List<NamedFileWithDescription>)CurrentSnippet.UploadModel.Files).Add(
                        new NamedFileWithDescription
                        {
                            Name = fileName,
                            Description = "",
                            File = new FileDTO
                            {
                                Name = fileName,
                                Description = "",
                                Data = fr.ReadBytes((int)fs.Length)
                            }
                        });
                }


                var returnString = new StringBuilder();
                foreach (string file in UploadedFiles)
                {
                    returnString.Append(file);
                    returnString.Append(";");
                }

                Request.RequestContext.HttpContext.Response.Write(returnString.ToString());
            }
            else
            {
                HttpFileCollectionBase oFiles = Request.RequestContext.HttpContext.Request.Files;
                if (oFiles != null && oFiles.Count > 0)
                {
                    for (int i = 0; i < oFiles.Count; i++)
                    {
                        HttpPostedFileBase httpPostedFileBase = oFiles[i];
                        if (httpPostedFileBase != null)
                        {
                            string fileName = httpPostedFileBase.FileName;
                            int idx = fileName.LastIndexOf("\\", StringComparison.Ordinal);
                            if (idx > -1)
                            {
                                fileName = fileName.Substring(idx);
                            }
                            httpPostedFileBase.SaveAs(sDirectory + fileName);
                        }

                        HttpPostedFileBase postedFileBase = oFiles[i];
                        if (postedFileBase != null) UploadedFiles.Add(postedFileBase.FileName);
                    }

                    var returnString = new StringBuilder();
                    foreach (string file in UploadedFiles)
                    {
                        returnString.AppendLine(file);
                    }
                    Request.RequestContext.HttpContext.Response.Write(returnString.ToString());
                }
            }

            return new EmptyResult();
        }

        [Authorize(Roles = "Administrator, RegisteredUser")]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult AddHyperlink(string hyperlink)
        {
            if (Request.InputStream != null)
            {
                byte[] hyperLinkRawData = Encoding.UTF8.GetBytes(hyperlink);
                string encodedString = Encoding.UTF8.GetString(hyperLinkRawData);
                if (!string.IsNullOrEmpty(encodedString) && !encodedString.Equals("http://"))
                {
                    ((List<HyperlinkDTO>)CurrentSnippet.Hyperlinks).Add(
                        new HyperlinkDTO
                        {
                            Uri = encodedString,
                            LastModified = DateTime.UtcNow
                        });
                }
            }

            return Json(new { result = hyperlink }, JsonRequestBehavior.AllowGet);
        }

        private UserDTO GetCurrentUserData()
        {
            UserDTO user;

            if (User != null && User.Identity.IsAuthenticated)
            {
                var identity = ((CustomIdentity)User.Identity);
                user = _managerService.UserDetails(identity.UserId, identity.UserGuid);
            }
            else
            {
                // The user is not authenticated, so we're creating an anonymous
                // snippet. 
                // Cheap hack:: User credentials hard coded in Global.asax. 
                // Note: If a new database is created or the current database has
                // it's existing users deleted, a new "Anonymous" or default user
                // will need to be created and the user information updated in Global.asax.
                user = _managerService.UserDetails(MvcApplication.AnonymousUserId, MvcApplication.AnonymousUserGuid);
            }

            return user;
        }
    }
}