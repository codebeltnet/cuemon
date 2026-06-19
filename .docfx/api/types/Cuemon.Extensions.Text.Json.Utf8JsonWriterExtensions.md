---
uid: Cuemon.Extensions.Text.Json.Utf8JsonWriterExtensions
example:
- *content
---

The following example demonstrates writing a dynamic object to JSON using the <xref:Cuemon.Extensions.Text.Json.Utf8JsonWriterExtensions.WriteObject(System.Text.Json.Utf8JsonWriter,System.Object,System.Text.Json.JsonSerializerOptions)> extension method.

```csharp
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Cuemon.Extensions.Text.Json;

namespace MyApp.Examples;

public class Utf8JsonWriterExtensionsExample
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string City { get; set; }

    public static void Main()
    {
        var person = new Person
        {
            Name = "John Doe",
            Age = 42,
            City = "Copenhagen"
        };

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });

        // Write the object directly to the Utf8JsonWriter
        writer.WriteObject(person, options);
        writer.Flush();

        string json = Encoding.UTF8.GetString(stream.ToArray());
        Console.WriteLine(json);
        // Output:
        // {
        //   "name": "John Doe",
        //   "age": 42,
        //   "city": "Copenhagen"
        // }

}}
}

```
