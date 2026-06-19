---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.MvcBuilderExtensions
example:
- *content
---

The following example demonstrates how to add XML serialization formatters to an MVC builder.

```csharp
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.Examples
{
    public static class MvcBuilderExtensionsExample
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            var builder = services.AddControllers();

            builder.AddXmlFormatters(options =>
            {
                options.Settings.Writer.Indent = true;
            });

            builder.AddXmlFormattersOptions(options =>
            {
                options.Settings.Writer.Indent = false;
            });
        }
    }
}
```
