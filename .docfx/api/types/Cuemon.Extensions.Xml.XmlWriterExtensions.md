---
uid: Cuemon.Extensions.Xml.XmlWriterExtensions
example:
- *content
---

`XmlWriterExtensions` provides extension methods for `XmlWriter` including object serialization, qualified element names, conditional wrapping, and root element generation. This example creates an `XmlWriter` targeting a `StringWriter` with indented, declaration-free settings, then calls `WriteObject` to serialize an `InvalidOperationException`, `WriteStartElement` with an `XmlQualifiedEntity("Cuemon")` for a standalone element, `WriteEncapsulatingElementWhenNotNull` to conditionally wrap an exception in a `"MyWrappedElement"`, and `WriteXmlRootElement` to produce a `Root` element with a `"cuemon"` namespace URI. Console output displays four separate XML results showing the serialized exception, the standalone element, the wrapped exception, and the namespace-qualified root element.

```csharp
using System;
using System.IO;
using System.Xml;
using Cuemon.Extensions.Xml;
using Cuemon.Xml.Serialization;

namespace DocExamples;

public static class XmlWriterExtensionsExample
{
    public static void Demonstrate()
    {
        Console.WriteLine(WriteException());
        Console.WriteLine(WriteStandaloneElement());
        Console.WriteLine(WriteWrappedException());
        Console.WriteLine(WriteRootElement());
    }

    private static string WriteException()
    {
        var output = new StringWriter();
        using var writer = XmlWriter.Create(output, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true });
        writer.WriteObject(new InvalidOperationException());
        writer.Flush();
        return output.ToString();
    }

    private static string WriteStandaloneElement()
    {
        var output = new StringWriter();
        using var writer = XmlWriter.Create(output, new XmlWriterSettings { OmitXmlDeclaration = true });
        writer.WriteStartElement(new XmlQualifiedEntity("Cuemon"));
        writer.WriteEndElement();
        writer.Flush();
        return output.ToString();
    }

    private static string WriteWrappedException()
    {
        var output = new StringWriter();
        using var writer = XmlWriter.Create(output, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true });
        writer.WriteEncapsulatingElementWhenNotNull(new InvalidOperationException(), new XmlQualifiedEntity("MyWrappedElement"), (nestedWriter, exception) =>
        {
            nestedWriter.WriteObject(exception);
        });
        writer.Flush();
        return output.ToString();
    }

    private static string WriteRootElement()
    {
        var output = new StringWriter();
        using var writer = XmlWriter.Create(output, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true });
        writer.WriteXmlRootElement(new InvalidOperationException(), (nestedWriter, exception, rootEntity) =>
        {
            nestedWriter.WriteObject(exception);
        }, new XmlQualifiedEntity("Root", "cuemon"));
        writer.Flush();
        return output.ToString();
    }
}
```
