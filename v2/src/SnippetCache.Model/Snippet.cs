using System;
using System.Collections.Generic;

namespace SnippetCache.Model
{
    public class Snippet : SnippetBrief
    {
        public string Description { get; set; }
        public bool IsFavorite { get; set; }
        public Language Language { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<File> Files { get; set; }
        public ICollection<Hyperlink> Hyperlinks { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
