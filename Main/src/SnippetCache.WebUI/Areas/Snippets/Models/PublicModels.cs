using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
//using SnippetCache.Data.DTO;
using SnippetCache.WebUI.ManagerService;
using SnippetCache.WebUI.Models;

namespace SnippetCache.WebUI.Areas.Snippets.Models
{
    [Serializable]
    public class SnippetPreviewModel
    {
        public IEnumerable<LanguageDTO> Languages;
        public string Name { get; set; }

        public string Description { get; set; }

        public string SnippetUrl { get; set; }

        public string PreviewData { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool IsPublic { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int LanguageId { get; set; }

        public int UserId { get; set; }

        public Guid UserGuid { get; set; }

        public string UserName { get; set; }

        public byte[] UserAvatar { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }

    [Serializable]
    public class SnippetDetailsModel
    {
        public Guid SnippetId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SnippetUrl { get; set; }

        public string PreviewData { get; set; }

        [AllowHtml]
        public string Data { get; set; }

        public string UserName { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Guid UserGuid { get; set; }

        public byte[] UserAvatar { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool IsPublic { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int LanguageId { get; set; }

        public IEnumerable<LanguageDTO> Languages { get; set; }

        public IEnumerable<NamedFileWithDescription> Files { get; set; }

        public IEnumerable<HyperlinkDTO> Hyperlinks { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }

    [Serializable]
    public class CreateSnippetModel
    {
        public CreateSnippetModel()
        {
        }

        public CreateSnippetModel(IEnumerable<LanguageDTO> languages)
        {
            Languages = languages;
            Hyperlinks = new List<HyperlinkDTO>();
            UploadModel = new UploadPublicFileModel
                              {
                                  Files = new List<NamedFileWithDescription>()
                              };
        }

        [Required(ErrorMessage = "A title is required.")]
        [StringLength(64, ErrorMessage = "Title length too long.")]
        [Display(Name = "Title")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "Description length too long.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "A snippet requires at least 1 character.")]
        [StringLength(65536, ErrorMessage = "Snippet too long.")]
        [AllowHtml]
        public string Data { get; set; }

        [Display(Name = "Create as private?")]
        public bool IsPrivate { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool IsPublic { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int LanguageId { get; set; }

        [Display(Name = "Language")]
        public IEnumerable<LanguageDTO> Languages { get; set; }

        public IEnumerable<HyperlinkDTO> Hyperlinks { get; set; }

        public UploadPublicFileModel UploadModel { get; set; }
    }

    [Serializable]
    public class UploadPublicFileModel
    {
        public IEnumerable<NamedFileWithDescription> Files { get; set; }
    }

    [Serializable]
    public class NamedFileWithDescription
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public FileDTO File { get; set; }
    }

    [Serializable]
    public class SearchForSnippetModel
    {
        [StringLength(100, ErrorMessage = "Search term is too long.")]
        public string SearchString { get; set; }
    }
}