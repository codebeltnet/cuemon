---
uid: Cuemon.Extensions.Hosting.Environments
example:
- *content
---

The following example shows how to configure a .NET generic host to use the `Environments.LocalDevelopment` environment. The host is built and run with this environment setting.

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
