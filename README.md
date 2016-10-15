# Async Zip-on-the-Fly

Example of dynamically generating a zip file download from ASP.NET Core, with minimal threading and memory usage.

Specifically, with this code:
- No file is ever completely in memory at any time. As files are read, they are immediately compressed and sent to the browser.
- No threads are ever blocked waiting on reads or writes. The fully-asynchronous code permits threads to return to the thread pool unless they are actually executing code.

This example uses its own `FileCallbackResult` to stream output on demand, and includes a workaround for [a bug in the `ZipArchive` class](https://github.com/dotnet/corefx/issues/11497).