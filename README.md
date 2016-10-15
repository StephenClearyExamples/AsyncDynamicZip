# Async Zip-on-the-Fly

Example of dynamically generating a zip file download from ASP.NET, with minimal threading and memory usage.

Specifically, with this code:
- No file is ever completely in memory at any time. As files are read, they are immediately compressed and sent to the browser.
- No threads are ever blocked waiting on reads or writes. The fully-asynchronous code permits threads to return to the thread pool unless they are actually executing code.

There are four examples in this repository, each on its own branch:
- [core-ziparchive](https://github.com/StephenClearyExamples/AsyncDynamicZip/tree/core-ziparchive) - ASP.NET Core using the built-in `ZipArchive`
- [core-dotnetzip](https://github.com/StephenClearyExamples/AsyncDynamicZip/tree/core-dotnetzip) - ASP.NET Core using `DotNetZip` (which restricts this application to the full .NET Framework)
- [full-ziparchive](https://github.com/StephenClearyExamples/AsyncDynamicZip/tree/full-ziparchive) - ASP.NET WebAPI using the built-in `ZipArchive`
- [full-dotnetzip](https://github.com/StephenClearyExamples/AsyncDynamicZip/tree/full-dotnetzip) - ASP.NET WebAPI using `DotNetZip`

Both of the `ZipArchive` repositories include a workaround for a bug in that class.

Both of the ASP.NET Core repositories include their own `FileCallbackResult` to asynchronously stream output on demand.
