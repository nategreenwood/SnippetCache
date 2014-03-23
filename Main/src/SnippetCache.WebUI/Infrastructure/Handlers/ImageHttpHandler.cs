using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnippetCache.WebUI.Infrastructure.Handlers
{
    public class ImageHttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/png";

        }
    }
}