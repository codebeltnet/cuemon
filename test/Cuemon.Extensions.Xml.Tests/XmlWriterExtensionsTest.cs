using System;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Cuemon.Xml;
using Cuemon.Xml.Serialization;
using Cuemon.Xml.Serialization.Formatters;
using Xunit;

namespace Cuemon.Extensions.Xml;
public class XmlWriterExtensionsTest : Test
{
    public XmlWriterExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void WriteObject_ShouldSerializeObjectToXml_Generic()
    {
        var sut1 = new InvalidOperationException();
        var sut2 = XmlStreamFactory.CreateStream(writer =>
        {
            writer.WriteObject(sut1);
        }, o => o.Indent = true);
        var sut3 = sut2.ToEncodedString(o => o.LeaveOpen = true);
        var sut4 = new XmlFormatter().Deserialize<InvalidOperationException>(sut2);

        TestOutput.WriteLine(sut3);

        Assert.Equal(sut1.Message, sut4.Message);
        Assert.Equal(sut1.ToString(), sut4.ToString());
        Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<InvalidOperationException namespace=""System"">
  <Message>Operation is not valid due to the current state of the object.</Message>
</InvalidOperationException>".ReplaceLineEndings(), sut3);
    }

    [Fact]
    public void WriteObject_ShouldSerializeObjectToXml()
    {
        var sut1 = new InvalidOperationException();
        var sut2 = XmlStreamFactory.CreateStream(writer =>
        {
            writer.WriteObject(sut1, sut1.GetType());
        }, o => o.Indent = true);
        var sut3 = sut2.ToEncodedString(o => o.LeaveOpen = true);
        var sut4 = new XmlFormatter().Deserialize(sut2, sut1.GetType()) as InvalidOperationException;

        TestOutput.WriteLine(sut3);

        Assert.Equal(sut1.Message, sut4.Message);
        Assert.Equal(sut1.ToString(), sut4.ToString());
        Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<InvalidOperationException namespace=""System"">
  <Message>Operation is not valid due to the current state of the object.</Message>
</InvalidOperationException>".ReplaceLineEndings(), sut3);
    }

    [Fact]
    public void WriteStartElement_ShouldWriteStartElement_Cuemon()
    {
        var sut1 = XmlStreamFactory.CreateStream(writer =>
        {
            writer.WriteStartElement(new XmlQualifiedEntity("Cuemon"));
            writer.WriteEndElement();
        }, o => o.Indent = true);
        var sut2 = sut1.ToEncodedString(o => o.LeaveOpen = true);

        TestOutput.WriteLine(sut2);

        Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Cuemon />".ReplaceLineEndings(), sut2);
    }

    [Fact]
    public void WriteEncapsulatingElementWhenNotNull_ShouldWriteNonNullElements()
    {
        var sut1 = XmlStreamFactory.CreateStream(writer =>
        {
            writer.WriteEncapsulatingElementWhenNotNull(new InvalidOperationException(), new XmlQualifiedEntity("MyWrappedElement"), (conditionalWriter, exception) =>
            {
                conditionalWriter.WriteObject(exception);
                conditionalWriter.WriteEncapsulatingElementWhenNotNull(new ArgumentNullException(), null, (innerConditionalWriter, exception) =>
                {
                    innerConditionalWriter.WriteObject(exception);
                });
            });
        }, o => o.Indent = true);
        var sut2 = sut1.ToEncodedString(o => o.LeaveOpen = true);

        TestOutput.WriteLine(sut2);

        Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<MyWrappedElement>
  <InvalidOperationException namespace=""System"">
    <Message>Operation is not valid due to the current state of the object.</Message>
  </InvalidOperationException>
  <ArgumentNullException namespace=""System"">
    <Message>Value cannot be null.</Message>
  </ArgumentNullException>
</MyWrappedElement>".ReplaceLineEndings(), sut2);
    }

    [Fact]
    public void WriteXmlRootElement_ShouldWriteXmlRootElement()
    {
        var sut1 = XmlStreamFactory.CreateStream(writer =>
        {
            writer.WriteXmlRootElement(new InvalidOperationException(), (treeWriter, exception, rootEntity) =>
            {
                treeWriter.WriteObject(exception);
            }, new XmlQualifiedEntity("Root", "cuemon"));
        }, o => o.Indent = true);
        var sut2 = sut1.ToEncodedString(o => o.LeaveOpen = true);

        TestOutput.WriteLine(sut2);

        Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Root xmlns=""cuemon"">
  <InvalidOperationException namespace=""System"">
    <Message>Operation is not valid due to the current state of the object.</Message>
  </InvalidOperationException>
</Root>".ReplaceLineEndings(), sut2);
    }
}
