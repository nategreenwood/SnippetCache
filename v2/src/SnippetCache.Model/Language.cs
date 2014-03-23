using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetCache.Model
{
    public class Language
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Language name is required")]
        public string Name { get; set; }
        [StringLength(25, ErrorMessage = "Version text is too long")]
        public string Version { get; set; }
    }
}
