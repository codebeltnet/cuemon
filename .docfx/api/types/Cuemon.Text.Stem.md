---
uid: Cuemon.Text.Stem
example:
- *content
---

The following example demonstrates how to use `Stem` to build a URL path by attaching prefixes and suffixes without duplication.

```csharp
using Cuemon.Text;

namespace MyApp.Examples;

public class StemExample
{
    public void Demonstrate()
    {
        var path = new Stem("api")
            .AttachPrefix("/")
            .AttachSuffix("/")
            .AttachSuffix("v1")
            .AttachSuffix("/")
            .AttachSuffix("users");

        var result = path.ToString();
        // result == "/api/v1/users"

}
}

```
