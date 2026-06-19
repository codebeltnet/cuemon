---
uid: Cuemon.Extensions.AspNetCore.Data.Integrity.ChecksumBuilderExtensions
example:
- *content
---

The following example demonstrates how to create an `EntityTagHeaderValue` from a `ChecksumBuilder` instance.

```csharp
using Cuemon.Data.Integrity;
using Cuemon.Extensions.AspNetCore.Data.Integrity;
using Microsoft.Net.Http.Headers;

namespace Examples;

public class EntityTagExample
{
    public EntityTagHeaderValue CreateEntityTag(ChecksumBuilder builder)
    {
        return builder.ToEntityTagHeaderValue();
    }

    public EntityTagHeaderValue CreateWeakEntityTag(ChecksumBuilder builder)
    {
        return builder.ToEntityTagHeaderValue(isWeak: true);
    }
}
```
