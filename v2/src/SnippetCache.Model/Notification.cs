using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetCache.Model
{
    public class Notification
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Notification text is required")]
        [StringLength(1024, ErrorMessage = "Notification text is too long")]
        public string Text { get; set; }
        public NotificationType Type { get; set; }
        public User User { get; set; }
        public Snippet Snippet { get; set; }
    }
}
