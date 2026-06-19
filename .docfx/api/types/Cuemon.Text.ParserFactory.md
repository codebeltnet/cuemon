---
uid: Cuemon.Text.ParserFactory
example:
- *content
---

```csharp
using System;
using Cuemon.Text;

namespace Cuemon.Text;

public class ParserFactoryExample
{
    public void Demonstrate()
    {
        var guidParser = ParserFactory.FromGuid();
        var result = guidParser.Parse("6B29FC40-CA47-1067-B31D-00DD010662DA");
        Console.WriteLine($"Parsed GUID: {result}");

        var base64Parser = ParserFactory.FromBase64();
        var bytes = base64Parser.Parse("SGVsbG8gV29ybGQ=");
        Console.WriteLine($"Base64 decoded bytes: {bytes.Length}");

        var uriParser = ParserFactory.FromUri();
        var uri = uriParser.Parse("https://example.com/resource");
        Console.WriteLine($"Parsed URI: {uri}");
    }
}
```
