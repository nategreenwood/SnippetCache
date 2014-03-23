using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetCache.Model
{
    public class Rating
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Value is required")]
        public string Value { get; set; }
        ICollection<Snippet> Snippets { get; set; } 
    }
}
