---
uid: Cuemon.Net.StringDecoratorExtensions
example:
- *content
---

`StringDecoratorExtensions` in the `Net` namespace provides URL-encoding and URL-decoding extension methods on `Decorator.Enclose` for strings with custom encoding support. This example encodes `"hello world & some <stuff>"` via `UrlEncode` producing `"hello+world+%26+some+%3cstuff%3e"`, decodes it back to the original via `UrlDecode`, and also demonstrates calling the static methods directly on `Cuemon.Net.StringDecoratorExtensions.UrlEncode`. Custom encoding with UTF-8 on `"a=b&c=d"` produces `"a%3db%26c%3dd"`. Console output confirms each encode and decode operation's round-trip behavior.

```csharp
// Program.cs
using System;
using Cuemon;
using System.Text;
using Cuemon.Net;
using Cuemon.Text;

// URL-encode a string with special characters
string raw = "hello world & some <stuff>";

// Invoke UrlEncode extension method
string encoded = Decorator.Enclose(raw).UrlEncode();
Console.WriteLine(encoded); // "hello+world+%26+some+%3cstuff%3e"

// Also invoke as static method via the Cuemon.Net.StringDecoratorExtensions type
string encodedStatic = Cuemon.Net.StringDecoratorExtensions.UrlEncode(Decorator.Enclose(raw));
Console.WriteLine(encodedStatic); // "hello+world+%26+some+%3cstuff%3e"

// URL-decode the encoded string back
string decoded = Decorator.Enclose(encoded).UrlDecode();
Console.WriteLine(decoded); // "hello world & some <stuff>"

// Invoke UrlDecode as static method via the Cuemon.Net.StringDecoratorExtensions type
string decodedStatic = Cuemon.Net.StringDecoratorExtensions.UrlDecode(Decorator.Enclose(encoded));
Console.WriteLine(decodedStatic); // "hello world & some <stuff>"

// Encode with a specific encoding
string encodedUtf8 = Decorator.Enclose("a=b&c=d").UrlEncode(o =>
{
    o.Encoding = Encoding.UTF8;
});
Console.WriteLine(encodedUtf8); // "a%3db%26c%3dd"
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="a29a9fd052fbb59215e84789"><a href="~/run-actions/a29a9fd052fbb59215e84789/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/a29a9fd052fbb59215e84789/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
