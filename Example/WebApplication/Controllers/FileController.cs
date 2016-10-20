using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace WebApplication.Controllers
{
    public class FileController : ApiController
    {
        private static HttpClient Client { get; } = new HttpClient();

        // http://localhost:12084/api/file
        public HttpResponseMessage Get()
        {
            var filenamesAndUrls = new Dictionary<string, string>
            {
                { "README.md", "https://raw.githubusercontent.com/StephenClearyExamples/AsyncDynamicZip/master/README.md" },
                { ".gitignore", "https://raw.githubusercontent.com/StephenClearyExamples/AsyncDynamicZip/master/.gitignore" },
            };

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new PushStreamContent(async (outputStream, httpContext, transportContext) =>
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
                }),
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "MyZipfile.zip" };
            return result;
        }
    }
}
