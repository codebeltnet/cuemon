---
uid: Cuemon.Extensions.Hosting.Environments
example:
- *content
---

```csharp
using Cuemon.Extensions.Hosting;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Hosting;

public class EnvironmentsExample
{
    public void Demonstrate()
    {
        var builder = Host.CreateDefaultBuilder()
            .UseEnvironment(Environments.LocalDevelopment);

        using var host = builder.Build();
        host.Run();
    }
}
```
