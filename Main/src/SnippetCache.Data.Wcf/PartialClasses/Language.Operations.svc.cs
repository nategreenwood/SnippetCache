using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SnippetCache.Data.EF4.Model;
using SnippetCache.Data.Wcf.Requests;
using SnippetCache.Data.Wcf.Responses;
using SnippetCache.Utils.Logging;
using SnippetCache.Utils.Validation;

namespace SnippetCache.Data.Wcf
{
    public partial class SnippetCacheDataService
    {
        #region Language Operations

        public GetAllLanguagesResponse GetAllLanguages(GetAllLanguagesRequest request)
        {
            var response = new GetAllLanguagesResponse();

            try
            {
                Guard.ArgNotNull(request, "request");
                IEnumerable<Language> languages = _unitOfWork.LanguageRepository.Get(d => d.Id > 0, null);
                Guard.ArgNotNull(languages, "languages");
                Language[] results = languages.ToArray();

                var quickInfoResults = new List<LanguageQuickInfo>(results.Count());
                quickInfoResults.AddRange(results.Select(
                    language => new LanguageQuickInfo
                                    {
                                        Id = language.Id,
                                        Name = language.Name,
                                        Description = language.Description
                                    }));
                response.Languages = quickInfoResults.AsEnumerable();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetAllLanguages Method Failed", ex);
            }

            return response;
        }

        public GetLanguageByIdResponse GetLanguageById(GetLanguageByIdRequest request)
        {
            var response = new GetLanguageByIdResponse();
            try
            {
                Guard.ArgNotNull(_unitOfWork.LanguageRepository, "LanguageRepository");
                Language language = _unitOfWork.LanguageRepository.GetById(request.Id);

                Guard.ArgNotNull(language, "language");

                response.Id = request.Id;
                response.Name = language.Name;
                response.Description = language.Description;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetLanguageById Method Failed", ex);
            }

            return response;
        }

        public GetLanguageByNameResponse GetLanguageByName(GetLanguageByNameRequest request)
        {
            var response = new GetLanguageByNameResponse();

            try
            {
                Guard.ArgNotNull(request, "request");
                Language language = _unitOfWork.LanguageRepository.Get(d => d.Name == request.Name).FirstOrDefault();

                Guard.ArgNotNull(language, "language");
                response.Success = true;
                response.Language = new LanguageQuickInfo
                                        {
                                            Id = language.Id,
                                            Name = language.Name,
                                            Description = language.Description
                                        };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("GetLanguageByName Method Failed", ex);
            }

            return response;
        }

        public CreateNewLanguageResponse CreateNewLanguage(CreateNewLanguageRequest request)
        {
            var response = new CreateNewLanguageResponse();
            try
            {
                Guard.ArgNotNull(_unitOfWork.LanguageRepository, "LanguageRepository");

                Language existingLanguage =
                    _unitOfWork.LanguageRepository.Get(d => d.Name.Equals(request.Name)).FirstOrDefault();
                if (existingLanguage != null && existingLanguage.Name == request.Name)
                    throw new DuplicateNameException("A language with the name " + request.Name + " already exists");

                var language = new Language
                                   {
                                       Id = 0,
                                       Name = request.Name,
                                       Description = request.Description
                                   };

                _unitOfWork.LanguageRepository.Insert(language);
                _unitOfWork.Save();

                Language firstOrDefault =
                    _unitOfWork.LanguageRepository.Get(d => d.Name.Equals(request.Name), null).FirstOrDefault();
                if (firstOrDefault != null)
                {
                    int newId = firstOrDefault.Id;
                    if (newId > 0)
                    {
                        response.Success = true;
                        response.Id = newId;
                        response.Name = request.Name;
                        response.Description = request.Description;
                        Logger.LogInfo("Successfully created Language Id: " + newId.ToString(), LogType.General);
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("CreateNewLanguage Method Failed", ex);
            }

            return response;
        }

        public UpdateLanguageResponse UpdateLanguage(UpdateLanguageRequest request)
        {
            var response = new UpdateLanguageResponse();
            try
            {
                Guard.ArgNotNull(request, "request");
                Language language = _unitOfWork.LanguageRepository.GetById(request.Id);

                Guard.ArgNotNull(language, "language");
                language.Name = request.Name;
                language.Description = request.Description;

                _unitOfWork.LanguageRepository.Update(language);
                _unitOfWork.Save();

                response.Success = true;
                Logger.LogInfo("Successfully updated Language Id: " + request.Id.ToString(), LogType.General);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("UpdateLanguage method failed", ex);
            }

            return response;
        }

        public DeleteLanguageResponse DeleteLanguage(DeleteLanguageRequest request)
        {
            var response = new DeleteLanguageResponse();
            try
            {
                Guard.ArgNotNull(request, "request");
                Language language = _unitOfWork.LanguageRepository.GetById(request.Id);

                Guard.ArgNotNull(language, "language");

                _unitOfWork.LanguageRepository.Delete(language);
                _unitOfWork.Save();

                response.Success = true;
                Logger.LogInfo("Successfully deleted Language Id: " + request.Id.ToString(), LogType.General);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.FailureInformation = ex.Message;
                Logger.LogError("DeleteLanguage method failed", ex);
            }

            return response;
        }

        #endregion
    }
}