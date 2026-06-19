---
uid: Cuemon.Xml.Serialization.XmlQualifiedEntity
example:
- *content
---

The following example demonstrates how to create XmlQualifiedEntity instances from local names, namespaces, prefixes, and XML serialization attributes to control element naming.

```csharp
using System;
using System.Xml.Serialization;
using Cuemon.Xml.Serialization;

namespace MyApp.Xml
{
    public class XmlQualifiedEntityExample
    {
        public void Demonstrate()
        {
            // Create an entity from local name only
            var entity1 = new XmlQualifiedEntity("Order");
            Console.WriteLine($"LocalName: {entity1.LocalName}"); // Order
            Console.WriteLine($"Namespace: {entity1.Namespace}"); // (null)
            Console.WriteLine($"Prefix: {entity1.Prefix}"); // (null)

            // Create an entity with local name and namespace
            var entity2 = new XmlQualifiedEntity("Order", "http://example.com/orders");
            Console.WriteLine($"LocalName: {entity2.LocalName}, Namespace: {entity2.Namespace}");

            // Create an entity with prefix, local name, and namespace
            var entity3 = new XmlQualifiedEntity("o", "Order", "http://example.com/orders");
            Console.WriteLine($"Prefix: {entity3.Prefix}, LocalName: {entity3.LocalName}, Namespace: {entity3.Namespace}");

            // Create from XmlRootAttribute
            var rootAttr = new XmlRootAttribute("PurchaseOrder");
            var fromRoot = new XmlQualifiedEntity(rootAttr);
            Console.WriteLine($"From XmlRoot: {fromRoot.LocalName} (Namespace: {fromRoot.Namespace})");
            Console.WriteLine($"HasXmlRootDecoration: {fromRoot.HasXmlRootDecoration}"); // True

            // Create from XmlElementAttribute
            var elemAttr = new XmlElementAttribute("LineItem");
            var fromElement = new XmlQualifiedEntity(elemAttr);
            Console.WriteLine($"From XmlElement: {fromElement.LocalName}");
            Console.WriteLine($"HasXmlElementDecoration: {fromElement.HasXmlElementDecoration}"); // True

            // Create from XmlAttributeAttribute
            var attr = new XmlAttributeAttribute("Quantity");
            var fromAttribute = new XmlQualifiedEntity(attr);
            Console.WriteLine($"From XmlAttribute: {fromAttribute.LocalName}");
            Console.WriteLine($"HasXmlAttributeDecoration: {fromAttribute.HasXmlAttributeDecoration}"); // True

            // Use with XmlSerializerOptions to set a custom root name
            var options = new XmlSerializerOptions
            {
                RootName = new XmlQualifiedEntity("CustomRoot", "http://example.com/schema")
            };
            Console.WriteLine($"Options root name: {options.RootName.LocalName}");

}}
}

```
