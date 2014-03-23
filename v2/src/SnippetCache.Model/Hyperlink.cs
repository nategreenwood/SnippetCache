using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetCache.Model
{
    public class Hyperlink
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Uri is required")]
        public string Uri { get; set; }
        [StringLength(1024, ErrorMessage = "Description is too long.")]
        public string Description { get; set; }
        public Snippet Snippet { get; set; }
    }
}
