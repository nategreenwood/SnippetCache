using System;
using System.Collections.Generic;
using System.Linq;
using SnippetCache.Data.DTO;
using SnippetCache.Engines.DataService;
using SnippetCache.Utils.Logging;
using SnippetCache.Utils.Validation;

namespace SnippetCache.Engines
{
    public class SnippetOperationsEngine : IEngine
    {
        private readonly ISnippetCacheDataService _dataService;

        public SnippetOperationsEngine()
        {
        }

        public SnippetOperationsEngine(ISnippetCacheDataService dataService)
        {
            _dataService = dataService;
        }

        #region IEngine Members

        public string EngineTypeName
        {
            get { return "SnippetOperationsEngine"; }
        }

        #endregion

        public SnippetDTO GetSnippetByGuid(Guid guid)
        {
            GetSnippetByGuidResponse result = _dataService.GetSnippetByGuid(new GetSnippetByGuidRequest
                                                                                {
                                                                                    Guid = guid
                                                                                });

            return result.Snippet;
        }

        public IEnumerable<LanguageDTO> GetAllLanguages()
        {
            GetAllLanguagesResponse response = _dataService.GetAllLanguages(new GetAllLanguagesRequest());
            var returnList = new List<LanguageDTO>();

            if (response.Success)
            {
                returnList.AddRange(response.Languages.Select(
                    language => new LanguageDTO
                                    {
                                        Id = language.Id,
                                        Name = language.Name
                                    }));
            }

            return returnList;
        }

        public int CreateNewSnippet(SnippetDTO snippet)
        {
            Guard.ArgNotNull(snippet, "snippet");
            int newSnippetId = -1;
            try
            {
                CreateSnippetResponse result = _dataService.CreateSnippet(
                    new CreateSnippetRequest
                        {
                            IsPublic = snippet.IsPublic,
                            Name = snippet.Name,
                            Description = snippet.Description,
                            PreviewData = snippet.PreviewData,
                            Data = snippet.Data,
                            LastModified = snippet.LastModified,
                            Language_Id = snippet.Language_Id,
                            User_Id = snippet.User_Id,
                            User_FormsAuthId = snippet.User_FormsAuthId,
                            Id = -1,
                            Guid = snippet.Guid
                        });

                if (result.Success)
                    newSnippetId = result.SnippetId;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error creating new snippet", ex);
            }

            return newSnippetId;
        }

        public int CreateNewHyperlink(HyperlinkDTO hyperlink)
        {
            Guard.ArgNotNull(hyperlink, "hyperlink");
            var result = new CreateHyperlinkResponse();
            int newHyperlinkId = -1;
            try
            {
                result = _dataService.CreateHyperlink(new CreateHyperlinkRequest
                                                          {
                                                              Uri = hyperlink.Uri,
                                                              Description = hyperlink.Description,
                                                              LastModified = hyperlink.LastModified,
                                                              SnippetId = hyperlink.Snippet_Id
                                                          });
            }
            catch (Exception ex)
            {
                Logger.LogError("Error creating new hyperlink", ex);
            }

            return result.HyperlinkId;
        }

        public int CreateNewFile(FileDTO file)
        {
            Guard.ArgNotNull(file, "file");
            var result = new CreateFileResponse();
            int newFileId = -1;
            try
            {
                result = _dataService.CreateFile(new CreateFileRequest
                                                     {
                                                         Name = file.Name,
                                                         Description = file.Description,
                                                         Data = file.Data,
                                                         LastModified = file.LastModified,
                                                         SnippetId = file.Snippet_Id
                                                     });
            }
            catch (Exception ex)
            {
                Logger.LogError("Error creating new file", ex);
            }

            return result.FileId;
        }

        public int GetTotalSnippetCount(bool isPrivate)
        {
            return _dataService.GetTotalSnippetCount(isPrivate);
        }

        public IEnumerable<HyperlinkDTO> GetHyperlinksForSnippet(Guid snippetGuid, int userId, Guid userGuid)
        {
            var response = new GetHyperlinksBySnippetIdResponse();

            response =
                _dataService.GetHyperlinksBySnippetId(new GetHyperlinksBySnippetIdRequest
                                                          {
                                                              SnippetId = snippetGuid,
                                                              UserId = userId,
                                                              UserGuid = userGuid
                                                          });
            return response.Hyperlinks;
        }

        public IEnumerable<FileDTO> GetFilesForSnippets(Guid snippetGuid, int userId, Guid userGuid)
        {
            var response = new GetFilesBySnippetIdResponse();
            response =
                _dataService.GetFilesBySnippetId(new GetFilesBySnippetIdRequest
                                                     {SnippetId = snippetGuid, UserId = userId, UserGuid = userGuid});

            return response.Files;
        }
    }
}