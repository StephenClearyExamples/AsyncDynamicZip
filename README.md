# Async Zip-on-the-Fly

Example of dynamically generating a zip file download from ASP.NET, with minimal threading and memory usage.

Specifically, with this code:
- No file is ever completely in memory at any time. As files are read, they are immediately compressed and sent to the browser.
- No threads are ever blocked waiting on reads or writes. The fully-asynchronous code permits threads to return to the thread pool unless they are actually executing code.

There are five examples in this repository, each on its own branch:
- [net6-ziparchive](https://github.com/StephenClearyExamples/AsyncDynamicZip/tree/net6-ziparchive) - ASP.NET (.NET 6.0) using the built-in `ZipArchive`
- [core-ziparchive](https://github.com/StephenClearyExamples/AsyncDynamicZip/tree/core-ziparchive) - ASP.NET (.NET Core 1.0) using the built-in `ZipArchive`
- [core-dotnetzip](https://github.com/StephenClearyExamples/AsyncDynamicZip/tree/core-dotnetzip) - ASP.NET (.NET Core 1.0) using `DotNetZip` (which restricts this application to the full .NET Framework)
- [full-ziparchive](https://github.com/StephenClearyExamples/AsyncDynamicZip/tree/full-ziparchive) - ASP.NET (.NET 4.6.1) using the built-in `ZipArchive`
- [full-dotnetzip](https://github.com/StephenClearyExamples/AsyncDynamicZip/tree/full-dotnetzip) - ASP.NET (.NET 4.6.1) using `DotNetZip`

The `core-ziparchive` and `full-ziparchive` branches include a workaround for a bug in the `ZipArchive` class on those platforms. The `net6-ziparchive` does not include that workaround since the bug has been fixed.

The `net6-ziparchive`, `core-ziparchive`, and `core-dotnetzip` branches include their own `FileCallbackResult` to asynchronously stream output on demand.
