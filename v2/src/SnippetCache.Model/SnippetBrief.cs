using System.ComponentModel.DataAnnotations;

namespace SnippetCache.Model
{
    public class SnippetBrief
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }
        public string PreviewData { get; set; }
        public string Tags { get; set; }
        public bool IsPublic { get; set; }
        public int LanguageId { get; set; }
        public int RatingId { get; set; }
        public int UserId { get; set; }
    }
}
