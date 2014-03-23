using System;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using SnippetCache.WebUI.Models;

namespace SnippetCache.WebUI.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            var result = new StringBuilder();
            if (pagingInfo.TotalPages <= 1)
                return MvcHtmlString.Empty;

            for (int i = pagingInfo.CurrentPage; i <= (5 + pagingInfo.CurrentPage); i++)
            {
                int startIndex = (i < 5) ? 1 : (pagingInfo.CurrentPage - 5) + 2;
                int endIndex = (i < 5)
                                   ? pagingInfo.TotalPages
                                   : (pagingInfo.CurrentPage + 2) <= pagingInfo.TotalPages
                                         ? (pagingInfo.CurrentPage + 2)
                                         : pagingInfo.TotalPages;
                bool frontArrows = (i - 5 >= 0);
                bool endArrows = (pagingInfo.TotalPages - (i + 2)) > 0;

                if (frontArrows)
                    result.Append("... ");
                for (int j = startIndex; j <= endIndex; j++)
                {
                    var tag = new TagBuilder("a");
                    tag.MergeAttribute("href", pageUrl(j));
                    tag.InnerHtml = j.ToString(CultureInfo.InvariantCulture);
                    if (j == pagingInfo.CurrentPage)
                    {
                        tag.AddCssClass("selected-page-link");
                        tag.Attributes.Add("style", "color:blue;");
                    }
                    result.Append(tag.ToString());

                    if (i != pagingInfo.TotalPages)
                        result.Append(" ");
                }
                if (endArrows)
                    result.Append("...");
                break;
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}