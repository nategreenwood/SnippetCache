using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SnippetCache.WebUI.Infrastructure
{
    public class ImageResult : ActionResult
    {
        private readonly WebImage _image;

        public ImageResult(WebImage image)
        {
            _image = image;
        }


        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "image/jpeg";
            response.Write(_image);
        }
    }
}