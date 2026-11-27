---
uid: Cuemon.Xml.Serialization.Formatters.XmlFormatter
example:
- *content
---

The following example demonstrates how to serialize an object with <see cref="XmlFormatter" />.

```csharp
// Program.cs
using System;
using System.IO;
using Cuemon.Xml.Serialization.Formatters;
using MyApp.Examples;

XmlFormatterExample.Demonstrate();

namespace MyApp.Examples
{
    public static class XmlFormatterExample
    {
        public static void Demonstrate()
        {
            var formatter = new XmlFormatter();
            var person = new Person { Name = "John Doe", Age = 42 };

            using var stream = formatter.Serialize(person, typeof(Person));
            stream.Position = 0;

            using var reader = new StreamReader(stream);
            var xml = reader.ReadToEnd();

            Console.WriteLine(xml.Contains("John Doe"));
            Console.WriteLine(xml.Contains("42"));
        }

        private sealed class Person
        {
            public string Name { get; set; }

            public int Age { get; set; }
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="8de013a0b7f38829493d2d48"><a href="~/run-actions/8de013a0b7f38829493d2d48/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/8de013a0b7f38829493d2d48/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
