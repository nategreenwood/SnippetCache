using System;
using System.Collections.Generic;
using SnippetCache.Data;
using SnippetCache.Data.DTO;
using SnippetCache.Engines.DataService;

namespace SnippetCache.Engines
{
    public class FileOperationsEngine : IEngine
    {
        private readonly ISnippetCacheDataService _dataService;

        public FileOperationsEngine()
        {
            if (_dataService == null)
                _dataService = new SnippetCacheDataServiceClient();
        }

        public FileOperationsEngine(ISnippetCacheDataService dataService)
        {
            _dataService = dataService;
        }

        #region IEngine Members

        public string EngineTypeName
        {
            get { return "FileOperationsEngine"; }
        }

        #endregion

        public FileDTO GetFile(int id)
        {
            GetFileByIdResponse result = _dataService.GetFileById(new GetFileByIdRequest {FileId = id});
            var returnVal = new FileDTO();

            if (result.Success)
            {
                returnVal.Id = result.File.Id;
                returnVal.Name = result.File.Name;
                returnVal.Description = result.File.Description;
                returnVal.Data = result.File.Data;
                returnVal.LastModified = result.File.LastModified;
                returnVal.Snippet_Id = result.File.Snippet_Id;
            }
            return returnVal;
        }

        public IEnumerable<FileDTO> GetFiles(int userId, Guid userGuid, int snippetId)
        {
            GetFilesByUserIdResponse result =
                _dataService.GetFilesByUserId(new GetFilesByUserIdRequest
                                                  {UserId = userId, UserGuid = userGuid, SnippetId = snippetId});
            IList<FileDTO> returnValue = new List<FileDTO>();
            if (result.Success)
            {
                foreach (FileDTO file in result.Files)
                {
                    var temp = new FileDTO
                                   {
                                       Id = file.Id,
                                       Name = file.Name,
                                       Description = file.Description,
                                       Data = file.Data,
                                       Snippet_Id = file.Snippet_Id,
                                       LastModified = file.LastModified
                                   };
                    returnValue.Add(temp);
                }
            }

            return returnValue;
        }

        public void AddFile(IFile file, int userId, Guid userGuid, Guid snippetId)
        {
            CreateFileResponse result = _dataService.CreateFile(new CreateFileRequest
                                                                    {
                                                                        Name = file.Name,
                                                                        Description = file.Description,
                                                                        Data = file.Data,
                                                                        SnippetId = file.Snippet_Id,
                                                                        LastModified = file.LastModified
                                                                    });
        }

        public void AddFiles(IEnumerable<IFile> files, int userId, Guid userGuid, Guid snippetId)
        {
        }
    }
}