using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SnippetCache.Data.DTO;
using SnippetCache.Utils.Logging;
using SnippetCache.WebUI.Areas.Snippets.Models;
using SnippetCache.WebUI.Infrastructure.AccountData;
using SnippetCache.WebUI.ManagerService;
using SnippetCache.WebUI.Models;

using FileDTO = SnippetCache.WebUI.ManagerService.FileDTO;
using HyperlinkDTO = SnippetCache.WebUI.ManagerService.HyperlinkDTO;
using LanguageDTO = SnippetCache.WebUI.ManagerService.LanguageDTO;
using NotificationDTO = SnippetCache.WebUI.ManagerService.NotificationDTO;
using SnippetDTO = SnippetCache.WebUI.ManagerService.SnippetDTO;
using UserDTO = SnippetCache.WebUI.ManagerService.UserDTO;

namespace SnippetCache.WebUI.Areas.Snippets.Controllers
{
    public class PublicController : Controller
    {
        private const int MAX_SNIPPETS_PER_PAGE = 6;
        private readonly IEnumerable<LanguageDTO> _displayLanguages;
        private readonly ISnippetCacheManagerService _managerService;

        public PublicController()
        {
            if (_managerService == null)
                _managerService = new SnippetCacheManagerServiceClient();
            _displayLanguages = _managerService.GetLanguagesForDisplay();
        }

        public PublicController(ISnippetCacheManagerService service)
        {
            if (service != null)
                _managerService = service;
        }

        internal IList<string> UploadedFiles
        {
            get
            {
                if (Session["UploadedFiles"] == null || ((IList<string>) Session["UploadedFiles"]).Count <= 0)
                {
                    Session["UploadedFiles"] = new List<string>();
                }

                return (List<string>) Session["UploadedFiles"];
            }
            set { Session["UploadedFiles"] = value; }
        }

        internal CreateSnippetModel CurrentSnippet
        {
            get
            {
                if (Session["CurrentSnippet"] == null)
                    Session["CurrentSnippet"] = new CreateSnippetModel(_displayLanguages);
                return (CreateSnippetModel) Session["CurrentSnippet"];
            }
            set
            {
                if (Session["CurrentSnippet"] == null)
                    Session["CurrentSnippet"] = value;
            }
        }

        [ActionName("All")]
        [OutputCache(Duration = 20, VaryByParam = "page")]
        public ActionResult Index(int page = 1)
        {
            var viewModel = new List<SnippetDetailsModel>();
            var pagingInfo = new PagingInfo {CurrentPage = page};

            UserDTO user = GetCurrentUserData(); //User.Identity as CustomIdentity;

            try
            {
                int currentCount = _managerService.GetTotalSnippetCount(false);
                pagingInfo.TotalItems = currentCount;
                if (user != null)
                {
                    IEnumerable<SnippetDTO> snippetList =
                        _managerService.GetAllPublicSnippets(MAX_SNIPPETS_PER_PAGE, page).OrderByDescending(
                            d => d.LastModified);

                    if (snippetList.Any())
                    {
                        viewModel.AddRange(
                            from snippetDTO in snippetList
                            let snippetsOwner =
                                _managerService.UserDetails(snippetDTO.User_Id,
                                                            snippetDTO.User_FormsAuthId)
                            select new SnippetDetailsModel
                                       {
                                           Name = snippetDTO.Name,
                                           Description = snippetDTO.Description,
                                           SnippetUrl = snippetDTO.Guid.ToString(),
                                           PreviewData =
                                               Encoding.UTF8.GetString(snippetDTO.PreviewData),
                                           UserName = snippetsOwner.LoginName,
                                           UserAvatar = snippetsOwner.AvatarImage,
                                           UserId = snippetsOwner.Id,
                                           UserGuid = snippetsOwner.FormsAuthId,
                                           IsPublic = snippetDTO.IsPublic,
                                           PagingInfo = pagingInfo
                                       });
                    }
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

        [HttpGet, ActionName("Create")]
        public ActionResult CreateNewSnippet()
        {
            var viewModel = new CreateSnippetModel(_displayLanguages)
                                {
                                    Data = string.Empty,
                                    Name = string.Empty,
                                    Description = string.Empty,
                                    IsPublic = true,
                                    Hyperlinks = new List<HyperlinkDTO>(),
                                    UploadModel = new UploadPublicFileModel
                                                      {
                                                          Files = new List<NamedFileWithDescription>()
                                                      },
                                    Languages = _displayLanguages,
                                    LanguageId = 1,
                                    IsPrivate = false
                                };

            return View(viewModel);
        }

        [ActionName("Create")]
        public ActionResult CreateNewSnippet(CreateSnippetModel inModel)
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
                                             IsPublic = true,
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
                                ((List<NamedFileWithDescription>) CurrentSnippet.UploadModel.Files)[i];
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
                        new LanguageDTO {Id = inModel.LanguageId},
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

        [ActionName("Snippet")]
        public ActionResult SingleSnippet(string guid)
        {
            var viewModel = new SnippetDetailsModel();
            var userId = -1;
            Guid userGuid;
            try
            {
                SnippetDTO snippet = _managerService.GetSnippet(guid);
                userId = snippet.User_Id;
                userGuid = snippet.User_FormsAuthId;

                if (snippet != null)
                {
                    UserDTO user = _managerService.UserDetails(snippet.User_Id, snippet.Guid);
                    if (user == null)
                    {
                        TempData["Error"] = "Error getting user information for snippet";
                        return View(viewModel);
                    }

                    viewModel = new SnippetDetailsModel
                                    {
                                        Data = Encoding.UTF8.GetString(snippet.Data),
                                        PreviewData = Encoding.UTF8.GetString(snippet.PreviewData),
                                        SnippetId = snippet.Guid,
                                        Name = snippet.Name,
                                        Description = snippet.Description,
                                        SnippetUrl = snippet.Guid.ToString(),
                                        UserId = snippet.User_Id,
                                        UserGuid = user.FormsAuthId,
                                        UserName = user.LoginName,
                                        UserAvatar = user.AvatarImage,
                                        Hyperlinks = new List<HyperlinkDTO>(),
                                        Files = new List<NamedFileWithDescription>()
                                    };
                    HyperlinkDTO[] hyperlinks = _managerService.GetHyperlinksForSnippet(snippet.Guid, user.Id,
                                                                                        user.FormsAuthId);
                    if (hyperlinks != null && hyperlinks.Any())
                        viewModel.Hyperlinks = hyperlinks;

                    FileDTO[] files = _managerService.GetFilesForSnippet(snippet.Guid, user.Id, user.FormsAuthId);
                    if (files != null && files.Any())
                    {
                        foreach (NamedFileWithDescription file in files.Select(fileDTO => new NamedFileWithDescription
                                                                                              {
                                                                                                  File = fileDTO,
                                                                                                  Name = fileDTO.Name,
                                                                                                  Description =
                                                                                                      fileDTO.
                                                                                                      Description
                                                                                              }))
                        {
                            ((List<NamedFileWithDescription>) viewModel.Files).Add(file);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Snippet exception", ex);
                TempData["Error"] = "Unknow error occurred.";
                return RedirectToAction("All");
            }

            // If the user has opted to receive notifications, add one now
            var newMessage = new NotificationDTO
                                 {
                                     User_Id = userId,
                                     User_FormsAuthId = userGuid,
                                     NotificationType_Id = 1,
                                     Text = "Your snipppet (Id: " + guid + ") has been viewed.",
                                     DateCreated = DateTime.UtcNow
                                 };
            _managerService.CreateNewUserNotification(newMessage);

            return View(viewModel);
        }

        public FileContentResult DisplayImage(byte[] bytes)
        {
            return File(Encoding.UTF8.GetBytes(Convert.ToBase64String(bytes)), "image/png");
        }

        public EmptyResult UploadPublicFiles()
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
                    var bytes = new byte[(int) inputStream.Length];
                    inputStream.Read(bytes, 0, (int) inputStream.Length);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                }

                UploadedFiles.Add(fileName);

                using (var fs = new FileStream(sDirectory + "\\" + fileName, FileMode.Open))
                using (var fr = new BinaryReader(fs))
                {
                    ((List<NamedFileWithDescription>) CurrentSnippet.UploadModel.Files).Add(
                        new NamedFileWithDescription
                            {
                                Name = fileName,
                                Description = "",
                                File = new FileDTO
                                           {
                                               Name = fileName,
                                               Description = "",
                                               Data = fr.ReadBytes((int) fs.Length)
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

        public ActionResult AddHyperlink(string hyperlink)
        {
            if (Request.InputStream != null)
            {
                byte[] hyperLinkRawData = Encoding.UTF8.GetBytes(hyperlink);
                string encodedString = Encoding.UTF8.GetString(hyperLinkRawData);
                if (!string.IsNullOrEmpty(encodedString) && !encodedString.Equals("http://"))
                {
                    ((List<HyperlinkDTO>) CurrentSnippet.Hyperlinks).Add(
                        new HyperlinkDTO
                            {
                                Uri = encodedString,
                                LastModified = DateTime.UtcNow
                            });
                }
            }

            return Json(new {result = hyperlink}, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs("POST", "GET")]
        [ActionName("Search")]
        public ActionResult PerformSearch(string searchText, int page = 1)
        {
            if (string.IsNullOrEmpty(searchText))
                return RedirectToAction("SearchResults", new {results = new List<SnippetDetailsModel>()});

            var results = new List<SnippetDetailsModel>();
            var pagingInfo = new PagingInfo {CurrentPage = page, ItemsPerPage = MAX_SNIPPETS_PER_PAGE};

            try
            {
                // Get search results
                SnippetDTO[] response = _managerService.FindSnippetsPaged(searchText ?? string.Empty,
                                                                          MAX_SNIPPETS_PER_PAGE, page);
                pagingInfo.TotalItems = _managerService.FindSnippets(searchText).Count();

                if (response.Any())
                {
                    results = ConvertDtoCollectionToModelCollection(response, pagingInfo).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("PerformSearch exception", ex);
                TempData["Error"] = "Unknow error occurred.";
                return RedirectToAction("All");
            }

            TempData["SearchResults"] = results;
            Session["SearchTerm"] = (string.IsNullOrEmpty(searchText) ? string.Empty : searchText);
            return RedirectToAction("SearchResults", new {results = results.ToArray()});
        }

        [HttpGet]
        public ActionResult SearchResults(int page = 1)
        {
            string searchText = string.Empty;
            var searchResults = new List<SnippetDetailsModel>();

            if (Session["SearchTerm"] != null && !string.IsNullOrEmpty(Session["SearchTerm"].ToString()))
            {
                searchText = Session["SearchTerm"].ToString();
            }

            if (TempData["SearchResults"] != null)
            {
                searchResults = ((List<SnippetDetailsModel>) TempData["SearchResults"]);
            }
            var viewModel = new List<SnippetDetailsModel>();
            var pagingInfo = new PagingInfo
                                 {
                                     ItemsPerPage = MAX_SNIPPETS_PER_PAGE,
                                     CurrentPage = page,
                                     TotalItems = searchResults != null ? searchResults.Count() : 0
                                 };

            if (searchResults != null)
            {
                viewModel = ((List<SnippetDetailsModel>) TempData["SearchResults"]);

                if (viewModel != null && viewModel.Any())
                {
                    viewModel = MergeUserAndSnippets(viewModel, pagingInfo);
                }
                else
                {
                    TempData["Message"] = "No results found";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchText))
                    viewModel = (List<SnippetDetailsModel>) ConvertDtoCollectionToModelCollection(
                        _managerService.FindSnippetsPaged((searchText == string.Empty) ? string.Empty : searchText,
                                                          pagingInfo.ItemsPerPage, page), pagingInfo);
            }

            TempData["SearchTerm"] = searchText;
            return View(viewModel);
        }

        private UserDTO GetCurrentUserData()
        {
            UserDTO user;

            if (User != null && User.Identity.IsAuthenticated)
            {
                var identity = ((CustomIdentity) User.Identity);
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

        private List<SnippetDetailsModel> MergeUserAndSnippets(List<SnippetDetailsModel> viewModel,
                                                               PagingInfo pagingInfo)
        {
            var newList = new List<SnippetDetailsModel>();
            if (viewModel != null && viewModel.Any())
            {
                newList.AddRange(from snippetDTO in viewModel
                                 let currentUser =
                                     _managerService.UserDetails(snippetDTO.UserId,
                                                                 snippetDTO.UserGuid)
                                 select new SnippetDetailsModel
                                            {
                                                SnippetId = snippetDTO.SnippetId,
                                                Name = snippetDTO.Name,
                                                Description = snippetDTO.Description,
                                                SnippetUrl = snippetDTO.SnippetId.ToString(),
                                                PreviewData = snippetDTO.PreviewData,
                                                UserName =
                                                    (currentUser != null && currentUser.Id > 0)
                                                        ? currentUser.LoginName
                                                        : MvcApplication.AnonoymousUserName,
                                                UserAvatar =
                                                    (currentUser != null && currentUser.Id > 0)
                                                        ? currentUser.AvatarImage
                                                        : null,
                                                UserId =
                                                    (currentUser.Id > 0)
                                                        ? currentUser.Id
                                                        : MvcApplication.AnonymousUserId,
                                                UserGuid =
                                                    (currentUser.FormsAuthId != Guid.Empty)
                                                        ? currentUser.FormsAuthId
                                                        : MvcApplication.AnonymousUserGuid,
                                                IsPublic = snippetDTO.IsPublic,
                                                PagingInfo = pagingInfo
                                            });
            }
            return newList;
        }

        private IEnumerable<SnippetDetailsModel> ConvertDtoCollectionToModelCollection(IEnumerable<SnippetDTO> response,
                                                                                       PagingInfo pagingInfo)
        {
            var results = new List<SnippetDetailsModel>();

            results.AddRange(
                from snippetDTO in response
                let currentUser =
                    _managerService.UserDetails(snippetDTO.User_Id,
                                                snippetDTO.User_FormsAuthId)
                select new SnippetDetailsModel
                           {
                               SnippetId = snippetDTO.Guid,
                               Name = snippetDTO.Name,
                               Description = snippetDTO.Description,
                               SnippetUrl = snippetDTO.Guid.ToString(),
                               PreviewData = Encoding.UTF8.GetString(snippetDTO.PreviewData),
                               UserName =
                                   (currentUser != null && currentUser.Id > 0)
                                       ? currentUser.LoginName
                                       : MvcApplication.AnonoymousUserName,
                               UserAvatar =
                                   (currentUser != null && currentUser.Id > 0)
                                       ? currentUser.AvatarImage
                                       : new byte[0],
                               UserId =
                                   (currentUser.Id > 0)
                                       ? currentUser.Id
                                       : MvcApplication.AnonymousUserId,
                               UserGuid =
                                   (currentUser.FormsAuthId != Guid.Empty)
                                       ? currentUser.FormsAuthId
                                       : MvcApplication.AnonymousUserGuid,
                               IsPublic = snippetDTO.IsPublic,
                               PagingInfo = pagingInfo
                           });

            return results;
        }
    }
}