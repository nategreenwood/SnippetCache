using System;
using System.Web.Mvc;
using SnippetCache.WebUI.ManagerService;
using SnippetCache.WebUI.SnippetCacheDataService;

namespace SnippetCache.WebUI.Areas.Files.Controllers
{
    public class FileOperationsController : Controller
    {
        private readonly ISnippetCacheDataService _dataService;
        private readonly ISnippetCacheManagerService _managerService;

        public FileOperationsController()
        {
            if (_managerService == null)
                _managerService = new SnippetCacheManagerServiceClient();
            if (_dataService == null)
                _dataService = new SnippetCacheDataServiceClient();
        }

        public FileOperationsController(ISnippetCacheManagerService managerService, ISnippetCacheDataService dataService)
        {
            _managerService = managerService;
            _dataService = dataService;
        }

        public FileContentResult GetFile(int userId, Guid userGuid, int fileId)
        {
            GetFileByIdResponse result = _dataService.GetFileById(new GetFileByIdRequest
                                                                      {
                                                                          FileId = fileId
                                                                      });

            return new FileContentResult(result.File.Data, "text");
        }
    }
}