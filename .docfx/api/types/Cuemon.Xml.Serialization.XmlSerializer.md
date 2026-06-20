---
uid: Cuemon.Xml.Serialization.XmlSerializer
example:
- *content
---

The following example demonstrates how to serialize and deserialize a simple object with <see cref="XmlSerializer" />.

```csharp
using System;
using Cuemon.Xml.Serialization;

namespace MyApp.Examples;

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

```
