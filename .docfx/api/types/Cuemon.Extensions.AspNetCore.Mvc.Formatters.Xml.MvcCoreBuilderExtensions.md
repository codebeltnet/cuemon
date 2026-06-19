---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.MvcCoreBuilderExtensions
example:
- *content
---

The following example demonstrates how to add XML serialization formatters to an MVC core builder using the <xref cref="Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.MvcCoreBuilderExtensions"/> extension methods.

```csharp
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.Examples;

public class MvcCoreBuilderExtensionsExample
{
    public void ConfigureMvc(IMvcCoreBuilder builder)
    {
        // Invoke the AddXmlFormatters extension method
        MvcCoreBuilderExtensions.AddXmlFormatters(builder, options =>
        {
            options.SynchronizeWithXmlConvert = true;
        });

        // Invoke the AddXmlFormattersOptions extension method
        MvcCoreBuilderExtensions.AddXmlFormattersOptions(builder, options =>
        {
            options.SynchronizeWithXmlConvert = true;
        });

}
}

```
