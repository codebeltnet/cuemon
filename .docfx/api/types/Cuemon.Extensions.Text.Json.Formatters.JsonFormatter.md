---
uid: Cuemon.Extensions.Text.Json.Formatters.JsonFormatter
example:
- *content
---

The following example demonstrates how to use `JsonFormatter` to serialize and deserialize objects to and from JSON.

```csharp
using System;
using System.IO;
using Cuemon;
using Cuemon.IO;
using Cuemon.Extensions.Text.Json.Formatters;

namespace MyApp.Examples;

public record Product(int Id, string Name, decimal Price);

public class Example
{
    public void Run()
    {
        // Create a JsonFormatter with default settings
        var formatter = new JsonFormatter();

        // Serialize an object to a stream
        var product = new Product(1, "Wireless Mouse", 29.99m);
        using var jsonStream = formatter.Serialize(product, typeof(Product));

        // Read the JSON string (leave the stream open for reuse)
        Console.WriteLine("Serialized JSON:");
        Console.WriteLine(Decorator.Enclose(jsonStream).ToEncodedString(o => o.LeaveOpen = true));

        // Reset position and deserialize from the same stream
        jsonStream.Position = 0;
        var deserialized = (Product)formatter.Deserialize(jsonStream, typeof(Product));
        Console.WriteLine($"Deserialized: Id={deserialized.Id}, Name={deserialized.Name}, Price={deserialized.Price}");

        // Use static convenience methods
        using var json = JsonFormatter.SerializeObject(product);
        json.Position = 0;
        Console.WriteLine($"Static round-trip: {JsonFormatter.DeserializeObject<Product>(json).Name}");

    }
}

```
