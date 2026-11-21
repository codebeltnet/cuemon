---
uid: Cuemon.Xml.Serialization.XmlSerializer
example:
- *content
---

The following example demonstrates how to serialize and deserialize a simple object with <see cref="XmlSerializer" />. It configures an `XmlSerializerOptions` root name, writes an `Order` to a stream, then reads the stream back and prints the customer and total.

```csharp
// Program.cs
using System;
using Cuemon.Xml.Serialization;
using MyApp.Examples;

XmlSerializerExample.Demonstrate();

namespace MyApp.Examples
{
    public static class XmlSerializerExample
    {
        public static void Demonstrate()
        {
            var serializer = XmlSerializer.Create(new XmlSerializerOptions
            {
                RootName = new XmlQualifiedEntity("Order")
            });

            var order = new Order
            {
                Id = 1001,
                Customer = "John Doe",
                Total = 299.99m
            };

            using var stream = serializer.Serialize(order, typeof(Order));
            stream.Position = 0;

            var deserialized = serializer.Deserialize<Order>(stream);

            Console.WriteLine(deserialized.Customer);
            Console.WriteLine(deserialized.Total);
        }

        private sealed class Order
        {
            public int Id { get; set; }

            public string Customer { get; set; }

            public decimal Total { get; set; }
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="9e8a8d5a5789ce8a01744b6c"><a href="~/run-actions/9e8a8d5a5789ce8a01744b6c/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/9e8a8d5a5789ce8a01744b6c/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
