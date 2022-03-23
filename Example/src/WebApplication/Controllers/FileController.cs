using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private static HttpClient Client { get; } = new HttpClient();

        // http://localhost:5130/file
        [HttpGet]
        public IActionResult Get()
        {
            var filenamesAndUrls = new Dictionary<string, string>
            {
                { "README.md", "https://raw.githubusercontent.com/StephenClearyExamples/AsyncDynamicZip/master/README.md" },
                { ".gitignore", "https://raw.githubusercontent.com/StephenClearyExamples/AsyncDynamicZip/master/.gitignore" },
            };

            return new FileCallbackResult("application/octet-stream", async (outputStream, _) =>
            {
                using (var zipArchive = new ZipArchive(outputStream, ZipArchiveMode.Create))
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