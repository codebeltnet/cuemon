---
uid: Cuemon.Extensions.Text.Json.Utf8JsonWriterExtensions
example:
- *content
---

`Utf8JsonWriterExtensions` provides the `WriteObject` extension method for serializing .NET objects directly into a `Utf8JsonWriter` stream. This example creates a `Person` instance with `Name`, `Age`, and `City` properties, configures `JsonSerializerOptions` with `CamelCase` naming and indented output, and calls `WriteObject` on a `Utf8JsonWriter` backed by a `MemoryStream`. Key steps include flushing the writer and reading the stream content as a UTF-8 string. Console output displays the formatted JSON with camelCase property names, such as `"name": "John Doe"`.

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
