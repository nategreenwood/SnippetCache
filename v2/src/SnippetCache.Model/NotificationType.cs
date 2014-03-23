using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetCache.Model
{
    public class NotificationType
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Notification Type is required")]
        public string Type { get; set; }
    }
}
