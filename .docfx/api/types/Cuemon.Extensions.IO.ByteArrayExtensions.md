---
uid: Cuemon.Extensions.IO.ByteArrayExtensions
example:
- *content
---

`ByteArrayExtensions` provides extension methods for converting `byte[]` into seekable `MemoryStream` instances, with both synchronous and asynchronous overloads. This example encodes `"Hello, World!"` into a UTF-8 byte array, then calls `ToStream()` and `ToStreamAsync()` to create streams, verifying that the synchronous stream has `Length = 13` and `CanSeek = True`. The content is read back with a `StreamReader` to confirm round-trip fidelity, and the asynchronous overload is demonstrated with a `CancellationToken`. Console output confirms the stream properties and that re-read content matches the original string.

```csharp
using System.Threading;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cuemon.Extensions.IO;

namespace MyApp.Extensions.IO
{
    public class ByteArrayExtensionsExample
    {
        public async Task DemonstrateAsync()
        {
            byte[] data = Encoding.UTF8.GetBytes("Hello, World!");

            // Convert a byte array to a seekable Stream (synchronous)
            using Stream stream = data.ToStream();
            Console.WriteLine(stream.Length); // 13
            Console.WriteLine(stream.CanSeek); // True

            // Read the content back
            using var reader = new StreamReader(stream);
            string content = reader.ReadToEnd();
            Console.WriteLine(content); // "Hello, World!"

            // Convert a byte array to a Stream (asynchronous)
            using Stream asyncStream = await data.ToStreamAsync();

            // Verify the async stream content
            using var asyncReader = new StreamReader(asyncStream);
            string asyncContent = asyncReader.ReadToEnd();
            Console.WriteLine(asyncContent); // "Hello, World!"

            // Use cancellation with the async overload
            using var cts = new System.Threading.CancellationTokenSource();
            using Stream cancelStream = await data.ToStreamAsync(cts.Token);

}}
}

```
