using System;
using System.Collections.Generic;
using System.IO.Compression;
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
        public IActionResult Get()
        {
            var filenamesAndUrls = new Dictionary<string, string>
            {
                { "README.md", "https://raw.githubusercontent.com/StephenClearyExamples/AsyncPushStreamContent/master/README.md" },
                { ".gitignore", "https://raw.githubusercontent.com/StephenClearyExamples/AsyncPushStreamContent/master/.gitignore" },
            };

            return new FileCallbackResult(new MediaTypeHeaderValue("application/octet-stream"), async (outputStream, _) =>
            {
                using (var zipArchive = new ZipArchive(new WriteOnlyStreamWrapper(outputStream), ZipArchiveMode.Create))
                {
                    foreach (var kvp in filenamesAndUrls)
                    {
                        var zipEntry = zipArchive.CreateEntry(kvp.Key);
                        using (var zipStream = zipEntry.Open())
                        using (var stream = await Client.GetStreamAsync(kvp.Value))
                            await stream.CopyToAsync(zipStream);
                    }
                }
            })
            {
                FileDownloadName = "MyZipfile.zip"
            };
        }
    }
}
