using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Ionic.Zip;

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
                { "README.md", "https://raw.githubusercontent.com/StephenClearyExamples/AsyncPushStreamContent/master/README.md" },
                { ".gitignore", "https://raw.githubusercontent.com/StephenClearyExamples/AsyncPushStreamContent/master/.gitignore" },
            };

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new PushStreamContent(async (outputStream, httpContext, transportContext) =>
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
                }),
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "MyZipfile.zip" };
            return result;
        }
    }
}
