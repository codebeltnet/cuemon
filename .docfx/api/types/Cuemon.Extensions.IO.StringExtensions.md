---
uid: Cuemon.Extensions.IO.StringExtensions
example:
- *content
---

`StringExtensions` in the `IO` namespace converts strings into `Stream` and `TextReader` instances for stream-based processing. This example starts with a JSON string `{"key":"value"}`, calls `ToStream` with a UTF-8 encoding configuration, `ToStreamAsync` for the asynchronous variant, and `ToTextReader` for direct text reader access. Key steps include verifying the stream has positive length, reading back the async stream's content with `ToEncodedStringAsync`, and reading the text reader content with `ReadToEnd`. Console output confirms the stream length is greater than zero, the async round-trip matches the original JSON, and the text reader reads the expected content.

```csharp
// Program.cs
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cuemon.Extensions.IO;

const string json = "{\"key\":\"value\"}";

using Stream stream = json.ToStream(options =>
{
    options.Encoding = Encoding.UTF8;
});

using Stream asyncStream = await json.ToStreamAsync();
using TextReader reader = json.ToTextReader();

Console.WriteLine(stream.Length > 0);
Console.WriteLine((await asyncStream.ToEncodedStringAsync()) == json);
Console.WriteLine(reader.ReadToEnd());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="196f08bdc120b1c7dd82ec5f"><a href="~/run-actions/196f08bdc120b1c7dd82ec5f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/196f08bdc120b1c7dd82ec5f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
