---
uid: Cuemon.Extensions.AspNetCore.Xml.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to register minimal XML formatter options using `AddMinimalXmlOptions` in the ASP.NET Core service collection.

```csharp
using Cuemon.Extensions.AspNetCore.Xml;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.Examples;

public class XmlServiceCollectionExtensionsExample
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMinimalXmlOptions(options =>
        {
            options.SynchronizeWithXmlConvert = true;
        });

}
}

```
