using Ionic.Zip;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
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

            return new FileCallbackResult(MediaTypeNames.Application.Octet, async (outputStream, _) =>
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
