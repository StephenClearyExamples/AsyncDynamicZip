using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private static HttpClient Client { get; } = new HttpClient();

        // http://localhost:24425/api/file
        [HttpGet]
        public async Task<FileStreamResult> Get()
        {
            var stream = await Client.GetStreamAsync("https://raw.githubusercontent.com/StephenClearyExamples/AsyncPushStreamContent/master/README.md");

            return new FileStreamResult(stream, new MediaTypeHeaderValue("text/plain"))
            {
                FileDownloadName = "README.md"
            };
        }
    }
}
