using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApplication.Controllers
{
    public class FileController : ApiController
    {
        private static HttpClient Client { get; } = new HttpClient();

        // http://localhost:12084/api/file
        public async Task<HttpResponseMessage> Get()
        {
            var stream = await Client.GetStreamAsync("https://raw.githubusercontent.com/StephenClearyExamples/AsyncPushStreamContent/master/README.md");

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(stream),
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "README.md" };
            return result;
        }
    }
}
