using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using SnippetCache.Data.DTO;
using SnippetCache.WebUI.ManagerService;

namespace SnippetCache.WebUI.Areas.Snippets.Models
{
    [Serializable]
    public class CreateSnippetModelPrivate
    {
        public CreateSnippetModelPrivate()
        {
        }

        public CreateSnippetModelPrivate(IEnumerable<LanguageDTO> languages)
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

        public IEnumerable<LanguageDTO> Languages { get; set; }

        public IEnumerable<HyperlinkDTO> Hyperlinks { get; set; }

        public UploadPublicFileModel UploadModel { get; set; }
    }
}
