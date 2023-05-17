using LibBusinessLogic.Interface;
using System.Net;
using WebAdvertisementApi.Controllers;

namespace WebAdvertisementApi.Middleware
{
    public class ResizeImageMiddleware
    {
        private IInfo _info;
        private readonly IWebHostEnvironment _environment;
        public ResizeImageMiddleware(RequestDelegate next, IInfo info, IWebHostEnvironment environment)
        {
            _environment = environment;
            _info = info;
            Next = next;
        }

        public RequestDelegate Next { get; }

        public async Task Invoke(HttpContext context)
        {

            Guid id = Guid.Parse(context.Request.Query["id"]);
            int height =  int.Parse(context.Request.Query["height"]);
            int width = int.Parse(context.Request.Query["width"]);

            var advertisement = await _info.InfoAdvertisement(id);
            if (advertisement == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            var imagePath = advertisement.ImageUrl;
            var imagePhysicalPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));
            if (!System.IO.File.Exists(imagePhysicalPath))
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            var ms = _info.ImageResize(imagePhysicalPath, width, height);
            context.Response.ContentType = "image/jpeg";
            await ms.CopyToAsync(context.Response.Body);
        }
    }
}
