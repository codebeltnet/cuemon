---
uid: Cuemon.Extensions.Text.Json.Formatters.JsonFormatter
example:
- *content
---

This example creates a `JsonFormatter` with a `DefaultJsonTypeInfoResolver`, serializes a `Product` record to a stream, and reads the encoded JSON back. It then resets the stream position, deserializes the payload into a `Product`, and prints the round-tripped values plus the result of a static serialization round trip.

```csharp
// Program.cs
using System;
using System.IO;
using System.Text.Json.Serialization.Metadata;
using Cuemon;
using Cuemon.IO;
using Cuemon.Extensions.Text.Json.Formatters;
using MyApp.Examples;

new Example().Run();

namespace MyApp.Examples
{
    public record Product(int Id, string Name, decimal Price);
    public class Example
    {
        public void Run()
        {
            // Create a JsonFormatter with default settings
            var formatter = new JsonFormatter(o => o.Settings.TypeInfoResolver = new DefaultJsonTypeInfoResolver());

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
            using var json = JsonFormatter.SerializeObject(product, o => o.Settings.TypeInfoResolver = new DefaultJsonTypeInfoResolver());
            json.Position = 0;
            Console.WriteLine($"Static round-trip: {JsonFormatter.DeserializeObject<Product>(json, o => o.Settings.TypeInfoResolver = new DefaultJsonTypeInfoResolver()).Name}");

        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e76b7e41242ac6868f1f3008"><a href="~/run-actions/e76b7e41242ac6868f1f3008/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e76b7e41242ac6868f1f3008/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
