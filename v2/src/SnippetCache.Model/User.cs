using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SnippetCache.Model
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        public string Blog { get; set; }
        public string Twitter { get; set; }
        public string ImageSource { get; set; }

        public virtual ICollection<Snippet> Snippets { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } 
        public virtual ICollection<Notification> Notifications { get; set; } 
    }
}
