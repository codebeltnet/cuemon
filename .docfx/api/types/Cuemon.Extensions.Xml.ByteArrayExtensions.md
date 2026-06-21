---
uid: Cuemon.Extensions.Xml.ByteArrayExtensions
example:
- *content
---

The following example demonstrates how to convert a byte array to an <xref:System.Xml.XmlReader> using the <xref:Cuemon.Extensions.Xml.ByteArrayExtensions> class.

```csharp
using System;
using System.Text;
using System.Xml;
using Cuemon.Extensions.Xml;

namespace MyApp.Examples;

public class ByteArrayExtensionsExample
{
    public void Demonstrate()
    {
        byte[] xmlData = Encoding.UTF8.GetBytes("<root><item>Value</item></root>");

        // Convert the byte array to an XmlReader
        using (XmlReader reader = xmlData.ToXmlReader())
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    Console.WriteLine(reader.Name);

}}}}
}

```
