using System;
using System.IO;
using System.Xml;
using Codebelt.Extensions.Xunit;
using Cuemon.Diagnostics;
using Xunit;

namespace Cuemon.Xml.Serialization.Converters;
public class FailureConverterTest : Test
{
    public FailureConverterTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CanRead_ShouldBeFalse()
    {
        var sut = new FailureConverter();
        Assert.False(sut.CanRead);
    }

    [Fact]
    public void CanWrite_ShouldBeTrue()
    {
        var sut = new FailureConverter();
        Assert.True(sut.CanWrite);
    }

    [Fact]
    public void ReadXml_ShouldThrowNotImplementedException()
    {
        var sut = new FailureConverter();
        using var reader = XmlReader.Create(new StringReader("<Failure/>"));
        Assert.Throws<NotImplementedException>(() => sut.ReadXml(typeof(Failure), reader));
    }

    [Fact]
    public void WriteXml_ShouldSerializeFailure_WithMessage()
    {
        Exception caught = null;
        try { throw new InvalidOperationException("Failure message"); } catch (Exception ex) { caught = ex; }

        var failure = new Failure(caught, FaultSensitivityDetails.None);
        var sut = new FailureConverter();

        using var ms = new MemoryStream();
        using var writer = XmlWriter.Create(ms, new XmlWriterSettings { OmitXmlDeclaration = true });
        sut.WriteXml(writer, failure, null);
        writer.Flush();
        ms.Position = 0;
        var xml = new StreamReader(ms).ReadToEnd();

        TestOutput.WriteLine(xml);
        Assert.Contains("<System.InvalidOperationException", xml);
        Assert.Contains("namespace=\"System\"", xml);
        Assert.Contains("<Message>Failure message</Message>", xml);
    }

    [Fact]
    public void WriteXml_ShouldSerializeFailure_WithStackTrace()
    {
        Exception caught = null;
        try { throw new InvalidOperationException("Stack message"); } catch (Exception ex) { caught = ex; }

        var failure = new Failure(caught, FaultSensitivityDetails.StackTrace);
        var sut = new FailureConverter();

        using var ms = new MemoryStream();
        using var writer = XmlWriter.Create(ms, new XmlWriterSettings { OmitXmlDeclaration = true });
        sut.WriteXml(writer, failure, null);
        writer.Flush();
        ms.Position = 0;
        var xml = new StreamReader(ms).ReadToEnd();

        TestOutput.WriteLine(xml);
        Assert.Contains("<Stack>", xml);
        Assert.Contains("<Frame>", xml);
    }

    [Fact]
    public void WriteXml_ShouldSerializeFailure_WithData()
    {
        Exception caught = null;
        try
        {
            var ex = new InvalidOperationException("Data message");
            ex.Data.Add("TestKey", "TestValue");
            throw ex;
        }
        catch (Exception ex) { caught = ex; }

        var failure = new Failure(caught, FaultSensitivityDetails.Data);
        var sut = new FailureConverter();

        using var ms = new MemoryStream();
        using var writer = XmlWriter.Create(ms, new XmlWriterSettings { OmitXmlDeclaration = true });
        sut.WriteXml(writer, failure, null);
        writer.Flush();
        ms.Position = 0;
        var xml = new StreamReader(ms).ReadToEnd();

        TestOutput.WriteLine(xml);
        Assert.Contains("<Data>", xml);
        Assert.Contains("<TestKey>TestValue</TestKey>", xml);
    }

    [Fact]
    public void WriteXml_ShouldSerializeFailure_WithInnerException()
    {
        var inner = new ArgumentNullException("param", "Inner");
        var outer = new InvalidOperationException("Outer", inner);
        var failure = new Failure(outer, FaultSensitivityDetails.None);
        var sut = new FailureConverter();

        using var ms = new MemoryStream();
        using var writer = XmlWriter.Create(ms, new XmlWriterSettings { OmitXmlDeclaration = true });
        sut.WriteXml(writer, failure, null);
        writer.Flush();
        ms.Position = 0;
        var xml = new StreamReader(ms).ReadToEnd();

        TestOutput.WriteLine(xml);
        Assert.Contains("<System.ArgumentNullException", xml);
    }

    [Fact]
    public void WriteXml_ShouldSerializeFailure_WithAggregateException()
    {
        var inner1 = new ArithmeticException("Arith");
        var inner2 = new AccessViolationException("AV");
        var agg = new AggregateException("Agg", inner1, inner2);
        var failure = new Failure(agg, FaultSensitivityDetails.None);
        var sut = new FailureConverter();

        using var ms = new MemoryStream();
        using var writer = XmlWriter.Create(ms, new XmlWriterSettings { OmitXmlDeclaration = true });
        sut.WriteXml(writer, failure, null);
        writer.Flush();
        ms.Position = 0;
        var xml = new StreamReader(ms).ReadToEnd();

        TestOutput.WriteLine(xml);
        Assert.Contains("<System.AggregateException", xml);
        Assert.Contains("<System.ArithmeticException", xml);
        Assert.Contains("<System.AccessViolationException", xml);
    }
}
