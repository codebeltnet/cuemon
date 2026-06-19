---
uid: Cuemon.Extensions.AspNetCore.Xml.Formatters.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to register XML formatter options and an XML-based exception response formatter in the ASP.NET Core service collection.

```csharp
using Cuemon.Extensions.AspNetCore.Xml.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.Examples;

public class XmlFormattersServiceCollectionExtensionsExample
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddXmlFormatterOptions(options =>
        {
            options.SynchronizeWithXmlConvert = true;
        });

        services.AddXmlExceptionResponseFormatter();

}
}

```
