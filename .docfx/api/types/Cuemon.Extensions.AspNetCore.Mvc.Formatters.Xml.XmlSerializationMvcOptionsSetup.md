---
uid: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.XmlSerializationMvcOptionsSetup
example:
- *content
---

The following example demonstrates how <see cref="XmlSerializationMvcOptionsSetup" /> adds XML serialization formatters to <see cref="Microsoft.AspNetCore.Mvc.MvcOptions" />.

```csharp
using System;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MyApp.Examples
{
    public static class XmlSerializationMvcOptionsSetupExample
    {
        public static void Demonstrate()
        {
            var formatterOptions = Options.Create(new XmlFormatterOptions());
            var setup = new XmlSerializationMvcOptionsSetup(formatterOptions);
            var mvcOptions = new MvcOptions();

            setup.Configure(mvcOptions);

            Console.WriteLine(mvcOptions.InputFormatters.Count);
            Console.WriteLine(mvcOptions.OutputFormatters.Count);
        }
    }
}
```
