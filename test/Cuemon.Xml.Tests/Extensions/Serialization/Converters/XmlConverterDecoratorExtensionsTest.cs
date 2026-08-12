using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Codebelt.Extensions.Xunit;
using Cuemon.Diagnostics;
using Cuemon.Extensions.IO;
using Xunit;

namespace Cuemon.Xml.Serialization.Converters;
public class XmlConverterDecoratorExtensionsTest : Test
{
    public XmlConverterDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    private static string SerializeWithConverters(object value, Type type, Action<IDecorator<IList<XmlConverter>>> configure)
    {
        var converters = new List<XmlConverter>();
        configure(Decorator.Enclose(converters));
        var options = new XmlSerializerOptions();
        foreach (var c in converters) { options.Converters.Add(c); }
        var serializer = XmlSerializer.Create(options);
        var result = serializer.Serialize(value, type);
        return result.ToEncodedString();
    }

    [Fact]
    public void FirstOrDefaultReaderConverter_WithNullDecorator_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            XmlConverterDecoratorExtensions.FirstOrDefaultReaderConverter(null, typeof(string)));
    }

    [Fact]
    public void FirstOrDefaultWriterConverter_WithNullDecorator_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            XmlConverterDecoratorExtensions.FirstOrDefaultWriterConverter(null, typeof(string)));
    }

    [Fact]
    public void FirstOrDefaultReaderConverter_ShouldReturnNullWhenNoConverterFound()
    {
        var converters = new List<XmlConverter>();
        var result = Decorator.Enclose(converters).FirstOrDefaultReaderConverter(typeof(string));
        Assert.Null(result);
    }

    [Fact]
    public void FirstOrDefaultWriterConverter_ShouldReturnNullWhenNoConverterFound()
    {
        var converters = new List<XmlConverter>();
        var result = Decorator.Enclose(converters).FirstOrDefaultWriterConverter(typeof(string));
        Assert.Null(result);
    }

    [Fact]
    public void FirstOrDefaultReaderConverter_ShouldReturnMatchingConverter()
    {
        var converters = new List<XmlConverter>();
        Decorator.Enclose(converters).AddExceptionConverter(false, false);

        var result = Decorator.Enclose(converters).FirstOrDefaultReaderConverter(typeof(InvalidOperationException));

        Assert.NotNull(result);
        Assert.IsType<ExceptionConverter>(result);
    }

    [Fact]
    public void FirstOrDefaultWriterConverter_ShouldReturnMatchingConverter()
    {
        var converters = new List<XmlConverter>();
        Decorator.Enclose(converters).AddExceptionConverter(false, false);

        var result = Decorator.Enclose(converters).FirstOrDefaultWriterConverter(typeof(InvalidOperationException));

        Assert.NotNull(result);
        Assert.IsType<ExceptionConverter>(result);
    }

    [Fact]
    public void FirstOrDefaultReaderConverter_ShouldSkipWriteOnlyConverter()
    {
        var converters = new List<XmlConverter>();
        Decorator.Enclose(converters).AddFailureConverter();

        var result = Decorator.Enclose(converters).FirstOrDefaultReaderConverter(typeof(Failure));

        Assert.Null(result);
    }

    [Fact]
    public void AddXmlConverter_ShouldAddGenericConverter()
    {
        var converters = new List<XmlConverter>();
        Decorator.Enclose(converters).AddXmlConverter<string>(writer: (w, v, q) => { });

        Assert.Single(converters);
    }

    [Fact]
    public void InsertXmlConverter_ShouldInsertAtSpecifiedIndex()
    {
        var converters = new List<XmlConverter>();
        Decorator.Enclose(converters).AddExceptionConverter(false, false);
        Decorator.Enclose(converters).InsertXmlConverter<string>(0, writer: (w, v, q) => { });

        Assert.Equal(2, converters.Count);
        Assert.IsType<DynamicXmlConverterCore>(converters[0]);
        Assert.IsType<ExceptionConverter>(converters[1]);
    }

    [Fact]
    public void AddEnumerableConverter_ShouldSerializeEnumerable()
    {
        var xml = SerializeWithConverters(
            new[] { 1, 2, 3 }, typeof(int[]),
            d => d.AddEnumerableConverter());

        TestOutput.WriteLine(xml);
        Assert.Contains("<Item>1</Item>", xml);
        Assert.Contains("<Item>2</Item>", xml);
        Assert.Contains("<Item>3</Item>", xml);
    }

    [Fact]
    public void AddEnumerableConverter_ShouldSerializeDictionary()
    {
        var dict = new Dictionary<string, int> { { "A", 1 }, { "B", 2 } };
        var xml = SerializeWithConverters(dict, typeof(Dictionary<string, int>), d => d.AddEnumerableConverter());

        TestOutput.WriteLine(xml);
        Assert.Contains("name=\"A\"", xml);
        Assert.Contains("name=\"B\"", xml);
        Assert.Contains(">1<", xml);
        Assert.Contains(">2<", xml);
    }

    [Fact]
    public void AddEnumerableConverter_WithFlattenItems_ShouldUseDictionaryKeyAsElementName()
    {
        var dict = new Dictionary<string, int> { { "Population", 100 } };
        var options = new XmlSerializerOptions { RootName = new XmlQualifiedEntity("Stats") };
        Decorator.Enclose(options.Converters).AddEnumerableConverter(flattenItems: true);
        var serializer = XmlSerializer.Create(options);
        var result = serializer.Serialize(dict, typeof(Dictionary<string, int>));
        var xml = result.ToEncodedString();

        TestOutput.WriteLine(xml);
        Assert.DoesNotContain("name=", xml);
        Assert.Contains("<Population>100</Population>", xml);
    }

    [Fact]
    public void AddExceptionConverter_WithStackTrace_ShouldIncludeStack()
    {
        Exception caught = null;
        try { throw new InvalidOperationException("Stack test"); } catch (Exception ex) { caught = ex; }

        var xml = SerializeWithConverters(caught, typeof(InvalidOperationException),
            d => d.AddExceptionConverter(includeStackTrace: true, includeData: false));

        TestOutput.WriteLine(xml);
        Assert.Contains("<Stack>", xml);
    }

    [Fact]
    public void AddExceptionConverter_WithData_ShouldIncludeData()
    {
        var ex = new InvalidOperationException("Data test");
        ex.Data.Add("Key1", "Val1");

        var xml = SerializeWithConverters(ex, typeof(InvalidOperationException),
            d => d.AddExceptionConverter(includeStackTrace: false, includeData: true));

        TestOutput.WriteLine(xml);
        Assert.Contains("<Key1>Val1</Key1>", xml);
    }

    [Fact]
    public void AddFailureConverter_ShouldAddWriteOnlyConverter()
    {
        var converters = new List<XmlConverter>();
        Decorator.Enclose(converters).AddFailureConverter();

        Assert.Single(converters);
        Assert.IsType<FailureConverter>(converters[0]);
        Assert.False(converters[0].CanRead);
        Assert.True(converters[0].CanWrite);
    }

    [Fact]
    public void AddUriConverter_ShouldSerializeUri()
    {
        var xml = SerializeWithConverters(
            new Uri("https://example.com/"),
            typeof(Uri),
            d => d.AddUriConverter());

        TestOutput.WriteLine(xml);
        Assert.Contains("<Uri>https://example.com/</Uri>", xml);
    }

    [Fact]
    public void AddUriConverter_ShouldDeserializeUri()
    {
        var converters = new List<XmlConverter>();
        Decorator.Enclose(converters).AddUriConverter();
        var options = new XmlSerializerOptions();
        foreach (var c in converters) { options.Converters.Add(c); }
        var serializer = XmlSerializer.Create(options);
        var stream = serializer.Serialize(new Uri("https://example.com/"), typeof(Uri));
        stream.Position = 0;

        var result = serializer.Deserialize<Uri>(stream);

        Assert.Equal("https://example.com/", result.OriginalString);
    }

    [Fact]
    public void AddDateTimeConverter_ShouldSerializeDateTime()
    {
        var dt = new DateTime(2023, 6, 15, 0, 0, 0, DateTimeKind.Utc);
        var xml = SerializeWithConverters(dt, typeof(DateTime), d => d.AddDateTimeConverter());

        TestOutput.WriteLine(xml);
        Assert.Contains("<DateTime>2023-06-15T00:00:00.0000000Z</DateTime>", xml);
    }

    [Fact]
    public void AddDateTimeConverter_ShouldDeserializeDateTime()
    {
        var converters = new List<XmlConverter>();
        Decorator.Enclose(converters).AddDateTimeConverter();
        var options = new XmlSerializerOptions();
        foreach (var c in converters) { options.Converters.Add(c); }
        var serializer = XmlSerializer.Create(options);
        var dt = new DateTime(2023, 6, 15, 0, 0, 0, DateTimeKind.Utc);
        var stream = serializer.Serialize(dt, typeof(DateTime));
        stream.Position = 0;

        var result = serializer.Deserialize<DateTime>(stream);

        Assert.Equal(dt, result);
    }

    [Fact]
    public void AddTimeSpanConverter_ShouldSerializeTimeSpan()
    {
        var ts = new TimeSpan(1, 2, 3);
        var xml = SerializeWithConverters(ts, typeof(TimeSpan), d => d.AddTimeSpanConverter());

        TestOutput.WriteLine(xml);
        Assert.Contains("<TimeSpan>01:02:03</TimeSpan>", xml);
    }

    [Fact]
    public void AddTimeSpanConverter_ShouldDeserializeTimeSpan()
    {
        var converters = new List<XmlConverter>();
        Decorator.Enclose(converters).AddTimeSpanConverter();
        var options = new XmlSerializerOptions();
        foreach (var c in converters) { options.Converters.Add(c); }
        var serializer = XmlSerializer.Create(options);
        var ts = new TimeSpan(1, 2, 3);
        var stream = serializer.Serialize(ts, typeof(TimeSpan));
        stream.Position = 0;

        var result = serializer.Deserialize<TimeSpan>(stream);

        Assert.Equal(ts, result);
    }

    [Fact]
    public void AddStringConverter_ShouldSerializePlainString()
    {
        var xml = SerializeWithConverters("Hello World", typeof(string), d => d.AddStringConverter());

        TestOutput.WriteLine(xml);
        Assert.Contains("Hello World", xml);
    }

    [Fact]
    public void AddStringConverter_ShouldWrapXmlStringInCData()
    {
        var xmlContent = "<tag>value</tag>";
        var xml = SerializeWithConverters(xmlContent, typeof(string), d => d.AddStringConverter());

        TestOutput.WriteLine(xml);
        Assert.Contains("<![CDATA[", xml);
    }

    [Fact]
    public void AddStringConverter_ShouldReturnEarlyForWhitespaceString()
    {
        var converters = new List<XmlConverter>();
        Decorator.Enclose(converters).AddStringConverter();
        var options = new XmlSerializerOptions();
        foreach (var c in converters) { options.Converters.Add(c); }
        var serializer = XmlSerializer.Create(options);
        var result = serializer.Serialize("   ", typeof(string));
        var xml = result.ToEncodedString();

        TestOutput.WriteLine(xml);
        Assert.DoesNotContain("<String>", xml);
    }

    [Fact]
    public void AddExceptionDescriptorConverter_ShouldSerializeDescriptorWithError()
    {
        Exception caught = null;
        try { throw new InvalidOperationException("Descriptor error"); } catch (Exception ex) { caught = ex; }

        var descriptor = new ExceptionDescriptor(caught, "ERR001", "An error occurred.");
        var xml = SerializeWithConverters(
            descriptor,
            typeof(ExceptionDescriptor),
            d => d.AddExceptionDescriptorConverter(o => o.SensitivityDetails = FaultSensitivityDetails.None));

        TestOutput.WriteLine(xml);
        Assert.Contains("<ExceptionDescriptor>", xml);
        Assert.Contains("<Code>ERR001</Code>", xml);
        Assert.Contains("<Message>An error occurred.</Message>", xml);
        Assert.DoesNotContain("<Failure>", xml);
    }

    [Fact]
    public void AddExceptionDescriptorConverter_ShouldSerializeFailureWhenRequested()
    {
        Exception caught = null;
        try { throw new InvalidOperationException("With failure"); } catch (Exception ex) { caught = ex; }

        var descriptor = new ExceptionDescriptor(caught, "ERR002", "Failure included.");
        var xml = SerializeWithConverters(
            descriptor,
            typeof(ExceptionDescriptor),
            d => d.AddExceptionDescriptorConverter(o => o.SensitivityDetails = FaultSensitivityDetails.Failure));

        TestOutput.WriteLine(xml);
        Assert.Contains("<Failure>", xml);
        Assert.Contains("<InvalidOperationException", xml);
    }

    [Fact]
    public void AddExceptionDescriptorConverter_ShouldSerializeEvidenceWhenRequested()
    {
        Exception caught = null;
        try { throw new InvalidOperationException("With evidence"); } catch (Exception ex) { caught = ex; }

        var descriptor = new ExceptionDescriptor(caught, "ERR003", "Evidence included.");
        descriptor.AddEvidence("RequestId", "abc-123", v => v);

        var xml = SerializeWithConverters(
            descriptor,
            typeof(ExceptionDescriptor),
            d => d.AddExceptionDescriptorConverter(o => o.SensitivityDetails = FaultSensitivityDetails.Evidence));

        TestOutput.WriteLine(xml);
        Assert.Contains("<Evidence>", xml);
        Assert.Contains("<RequestId>abc-123</RequestId>", xml);
    }

    [Fact]
    public void AddXmlConverter_WithNullDecorator_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            XmlConverterDecoratorExtensions.AddXmlConverter<string>(null, writer: (w, v, q) => { }));
    }

    [Fact]
    public void InsertXmlConverter_WithNullDecorator_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            XmlConverterDecoratorExtensions.InsertXmlConverter<string>(null, 0, writer: (w, v, q) => { }));
    }

    [Fact]
    public void AddEnumerableConverter_WithNullDecorator_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            XmlConverterDecoratorExtensions.AddEnumerableConverter(null));
    }

    [Fact]
    public void AddExceptionConverter_WithNullDecorator_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            XmlConverterDecoratorExtensions.AddExceptionConverter(null, false, false));
    }

    [Fact]
    public void AddFailureConverter_WithNullDecorator_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            XmlConverterDecoratorExtensions.AddFailureConverter(null));
    }

    [Fact]
    public void AddUriConverter_WithNullDecorator_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            XmlConverterDecoratorExtensions.AddUriConverter(null));
    }

    [Fact]
    public void AddDateTimeConverter_WithNullDecorator_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            XmlConverterDecoratorExtensions.AddDateTimeConverter(null));
    }

    [Fact]
    public void AddTimeSpanConverter_WithNullDecorator_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            XmlConverterDecoratorExtensions.AddTimeSpanConverter(null));
    }

    [Fact]
    public void AddStringConverter_WithNullDecorator_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            XmlConverterDecoratorExtensions.AddStringConverter(null));
    }

    [Fact]
    public void AddExceptionDescriptorConverter_WithNullDecorator_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            XmlConverterDecoratorExtensions.AddExceptionDescriptorConverter(null, o => { }));
    }
}
