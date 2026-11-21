---
uid: Cuemon.IO.BufferWriterOptions
example:
- *content
---

The following example demonstrates how to configure `BufferWriterOptions` and use it with an `IBufferWriter<byte>` to produce a string.

```csharp
using System;
using System.Buffers;
using System.Text;
using Cuemon.IO;
using Cuemon.Text;

namespace MyApp.Examples;

public class BufferWriterOptionsExample
{
    public static void Main()
    {
        var options = new BufferWriterOptions
        {
            BufferSize = 1024,
            Encoding = Encoding.UTF8,
            Preamble = PreambleSequence.Remove
        };

        var writer = new ArrayBufferWriter<byte>(options.BufferSize);
        byte[] data = Encoding.UTF8.GetBytes("Hello, BufferWriterOptions!");
        writer.Write(data);

        string result = Encoding.UTF8.GetString(writer.WrittenSpan);
        Console.WriteLine(result);

        // Output:
        // Hello, BufferWriterOptions!

}
}

```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="99c89ff0aaf28ac8c13df795"><a href="~/run-actions/99c89ff0aaf28ac8c13df795/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/99c89ff0aaf28ac8c13df795/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
