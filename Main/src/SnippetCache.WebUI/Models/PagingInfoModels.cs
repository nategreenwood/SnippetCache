using System;

namespace SnippetCache.WebUI.Models
{
    public class PagingInfo
    {
        private const int DEFAULT_ITEMS_PER_PAGE = 7;

        public PagingInfo()
        {
            ItemsPerPage = DEFAULT_ITEMS_PER_PAGE;
            CurrentPage = 1;
        }

        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages
        {
            get { return (int) Math.Ceiling((decimal) TotalItems/ItemsPerPage); }
        }
    }
}