using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetCache.Model
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [StringLength(1024, ErrorMessage = "Description is too long.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }
        public Snippet Snippet { get; set; }
    }
}
