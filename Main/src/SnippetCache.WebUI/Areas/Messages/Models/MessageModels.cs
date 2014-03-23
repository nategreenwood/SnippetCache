using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SnippetCache.WebUI.Areas.Messages.Models
{
    public class UserMessagesModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public bool Remove { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }
    }
}