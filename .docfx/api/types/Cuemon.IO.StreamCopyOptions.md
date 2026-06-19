---
uid: Cuemon.IO.StreamCopyOptions
example:
- *content
---

The following example demonstrates how to configure StreamCopyOptions to control buffer size and whether the source stream remains open after copying.

```csharp
using System;
using System.IO;
using System.Text;
using Cuemon;
using Cuemon.IO;

namespace MyApp.IO;

public class StreamCopyOptionsExample
{
    public void Demonstrate()
    {
        // Create and use StreamCopyOptions directly
        var customOptions = new StreamCopyOptions { BufferSize = 4096, LeaveOpen = true };
        Console.WriteLine($"Buffer size: {customOptions.BufferSize}, Leave open: {customOptions.LeaveOpen}");

        // Create a memory stream with some data
        var source = new MemoryStream(Encoding.UTF8.GetBytes("Hello, StreamCopyOptions!"));

        // Convert the stream to a byte array using custom StreamCopyOptions
        byte[] bytes = Decorator.Enclose(source).ToByteArray(setup =>
        {
            setup.BufferSize = 4096;
            setup.LeaveOpen = true;
        });

        Console.WriteLine($"Read {bytes.Length} bytes"); // 26
        Console.WriteLine(Encoding.UTF8.GetString(bytes)); // Hello, StreamCopyOptions!
        Console.WriteLine($"Source stream is still open: {source.CanRead}"); // True

        // Without LeaveOpen, the source stream is automatically disposed after reading
        var temp = new MemoryStream(Encoding.UTF8.GetBytes("Temporary data."));
        byte[] tempBytes = Decorator.Enclose(temp).ToByteArray(); // disposes 'temp'
        Console.WriteLine(Encoding.UTF8.GetString(tempBytes)); // Temporary data.
        Console.WriteLine($"Stream was disposed: {!temp.CanRead}"); // True

        // Using default options (BufferSize = 81920, LeaveOpen = false)
        var data = new MemoryStream(Encoding.UTF8.GetBytes("Default options."));
        byte[] result = Decorator.Enclose(data).ToByteArray();
        Console.WriteLine(Encoding.UTF8.GetString(result)); // Default options.

}
}

```
