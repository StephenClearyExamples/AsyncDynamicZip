using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ionic.Zip;
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
        public FileCallbackResult Get()
        {
            var filenamesAndUrls = new Dictionary<string, string>
            {
                { "README.md", "https://raw.githubusercontent.com/StephenClearyExamples/AsyncDynamicZip/master/README.md" },
                { ".gitignore", "https://raw.githubusercontent.com/StephenClearyExamples/AsyncDynamicZip/master/.gitignore" },
            };

            return new FileCallbackResult(new MediaTypeHeaderValue("application/octet-stream"), async (outputStream, _) =>
            {
                using (var zipStream = new ZipOutputStream(outputStream))
                {
                    foreach (var kvp in filenamesAndUrls)
                    {
                        zipStream.PutNextEntry(kvp.Key);
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
