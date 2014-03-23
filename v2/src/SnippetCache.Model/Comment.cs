using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetCache.Model
{
    public class Comment
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Comment text is required")]
        public string Text { get; set; }
        public Snippet Snippet { get; set; }
    }
}
