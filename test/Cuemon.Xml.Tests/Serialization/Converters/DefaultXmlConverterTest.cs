using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.IO;
using Xunit;

namespace Cuemon.Xml.Serialization.Converters;
public class DefaultXmlConverterTest : Test
{
    public DefaultXmlConverterTest(ITestOutputHelper output) : base(output)
    {
    }

    private static string SerializeWithDefault(object value, Type type, XmlQualifiedEntity rootName = null)
    {
        var converter = new DefaultXmlConverter(rootName, new List<XmlConverter>());
        var ms = new MemoryStream();
        var settings = new XmlWriterSettings { OmitXmlDeclaration = true };
        using (var writer = XmlWriter.Create(ms, settings))
        {
            converter.WriteXml(writer, value, null);
        }
        ms.Position = 0;
        return ms.ToEncodedString();
    }

    private static object DeserializeWithDefault(string xml, Type type, XmlQualifiedEntity rootName = null)
    {
        var converter = new DefaultXmlConverter(rootName, new List<XmlConverter>());
        using var reader = XmlReader.Create(new StringReader(xml));
        return converter.ReadXml(reader, type);
    }

    [Fact]
    public void CanConvert_ShouldAlwaysReturnTrue()
    {
        var sut = new DefaultXmlConverter(null, new List<XmlConverter>());
        Assert.True(sut.CanConvert(typeof(string)));
        Assert.True(sut.CanConvert(typeof(int)));
        Assert.True(sut.CanConvert(typeof(object)));
    }

    [Fact]
    public void WriteXml_ShouldSerializePrimitive_Int()
    {
        var xml = SerializeWithDefault(42, typeof(int), new XmlQualifiedEntity("Int32"));
        TestOutput.WriteLine(xml);
        Assert.Contains("<Int32>42</Int32>", xml);
    }

    [Fact]
    public void WriteXml_ShouldSerializePrimitive_Bool()
    {
        var xml = SerializeWithDefault(true, typeof(bool), new XmlQualifiedEntity("Boolean"));
        TestOutput.WriteLine(xml);
        Assert.Contains("<Boolean>true</Boolean>", xml);
    }

    [Fact]
    public void WriteXml_ShouldSerializeString()
    {
        var xml = SerializeWithDefault("Hello World", typeof(string), new XmlQualifiedEntity("String"));
        TestOutput.WriteLine(xml);
        Assert.Contains("Hello World", xml);
    }

    [Fact]
    public void WriteXml_ShouldWrapXmlStringInCData()
    {
        var xmlString = "<html><body>Test</body></html>";
        var xml = SerializeWithDefault(xmlString, typeof(string), new XmlQualifiedEntity("String"));
        TestOutput.WriteLine(xml);
        Assert.Contains("<![CDATA[", xml);
    }

    [Fact]
    public void WriteXml_ShouldSerializeComplexObject()
    {
        var obj = new SimpleModel { Name = "Test", Value = 99 };
        var xml = SerializeWithDefault(obj, typeof(SimpleModel));
        TestOutput.WriteLine(xml);
        Assert.Contains("<Name>Test</Name>", xml);
        Assert.Contains("<Value>99</Value>", xml);
    }

    [Fact]
    public void WriteXml_ShouldRespectXmlIgnoreAttribute()
    {
        var obj = new ModelWithIgnored { Name = "Visible", Ignored = "NotVisible" };
        var xml = SerializeWithDefault(obj, typeof(ModelWithIgnored));
        TestOutput.WriteLine(xml);
        Assert.Contains("<Name>Visible</Name>", xml);
        Assert.DoesNotContain("NotVisible", xml);
    }

    [Fact]
    public void WriteXml_ShouldRespectXmlAttributeAttribute()
    {
        var obj = new ModelWithXmlAttribute { Name = "AttrValue" };
        var xml = SerializeWithDefault(obj, typeof(ModelWithXmlAttribute));
        TestOutput.WriteLine(xml);
        Assert.Contains("Name=\"AttrValue\"", xml);
    }

    [Fact]
    public void WriteXml_ShouldSerializeIXmlSerializable()
    {
        var obj = new XmlSerializableModel("CustomValue");
        var xml = SerializeWithDefault(obj, typeof(XmlSerializableModel));
        TestOutput.WriteLine(xml);
        Assert.Contains("CustomValue", xml);
    }

    [Fact]
    public void WriteXml_ShouldSkipNullProperties()
    {
        var obj = new ModelWithOptional { Name = "Present", Optional = null };
        var xml = SerializeWithDefault(obj, typeof(ModelWithOptional));
        TestOutput.WriteLine(xml);
        Assert.Contains("Present", xml);
        Assert.DoesNotContain("<Optional", xml);
    }

    [Fact]
    public void ReadXml_ShouldDeserializePrimitive_Int()
    {
        var result = DeserializeWithDefault("<Int32>42</Int32>", typeof(int));
        Assert.Equal(42, result);
    }

    [Fact]
    public void ReadXml_ShouldDeserializePrimitive_Bool()
    {
        var result = DeserializeWithDefault("<Boolean>true</Boolean>", typeof(bool));
        Assert.Equal(true, result);
    }

    [Fact]
    public void ReadXml_ShouldDeserializeGuid()
    {
        var guid = Guid.NewGuid();
        var result = DeserializeWithDefault($"<Guid>{guid}</Guid>", typeof(Guid));
        Assert.Equal(guid, result);
    }

    [Fact]
    public void ReadXml_ShouldDeserializeDecimal()
    {
        var result = DeserializeWithDefault("<Decimal>3.14</Decimal>", typeof(decimal));
        Assert.Equal(3.14m, result);
    }

    [Fact]
    public void ReadXml_ShouldDeserializeString()
    {
        var result = DeserializeWithDefault("<String>Hello</String>", typeof(string));
        Assert.Equal("Hello", result);
    }

    [Fact]
    public void ReadXml_ThrowsOnNull_Reader()
    {
        var sut = new DefaultXmlConverter(null, new List<XmlConverter>());
        Assert.Throws<ArgumentNullException>(() => sut.ReadXml((XmlReader)null, typeof(int)));
    }

    [Fact]
    public void ReadXml_ThrowsOnNull_ObjectType()
    {
        var sut = new DefaultXmlConverter(null, new List<XmlConverter>());
        using var reader = XmlReader.Create(new StringReader("<x>1</x>"));
        Assert.Throws<ArgumentNullException>(() => sut.ReadXml(reader, null));
    }

    [Fact]
    public void ReadXml_ShouldDeserializeComplexObject_WithDefaultConstructor()
    {
        var xml = "<SimpleModel><Name>Parsed</Name><Value>7</Value></SimpleModel>";
        var result = (SimpleModel)DeserializeWithDefault(xml, typeof(SimpleModel));
        Assert.Equal("Parsed", result.Name);
        Assert.Equal(7, result.Value);
    }

    [Fact]
    public void ReadXml_ShouldDeserializeList()
    {
        var xml = "<List><Item>1</Item><Item>2</Item><Item>3</Item></List>";
        var result = DeserializeWithDefault(xml, typeof(List<int>));
        var list = Assert.IsType<List<int>>(result);
        Assert.Equal(new[] { 1, 2, 3 }, list);
    }

    [Fact]
    public void ReadXml_ShouldDeserializeEmptyDictionary()
    {
        var xml = "<Dictionary />";
        var result = DeserializeWithDefault(xml, typeof(Dictionary<string, string>));
        var dict = Assert.IsType<Dictionary<string, string>>(result);
        Assert.Empty(dict);
    }

    [Fact]
    public void ReadXml_ShouldDeserializeComplexList_ThrowsNotSupported()
    {
        var xml = "<List><Item><Sub>1</Sub></Item></List>";
        var sut = new DefaultXmlConverter(null, new List<XmlConverter>());
        using var reader = XmlReader.Create(new StringReader(xml));
        Assert.Throws<NotSupportedException>(() => sut.ReadXml(reader, typeof(List<SimpleModel>)));
    }

    [Fact]
    public void ReadXml_ShouldDeserializeComplexObject_ViaStaticFactory()
    {
        var xml = $"<ModelWithStaticFactory><Id>42</Id><Label>Test</Label></ModelWithStaticFactory>";
        var result = (ModelWithStaticFactory)DeserializeWithDefault(xml, typeof(ModelWithStaticFactory));
        Assert.Equal(42, result.Id);
        Assert.Equal("Test", result.Label);
    }

    [Fact]
    public void ReadXml_ShouldThrowSerializationException_WhenNoSuitableConstructor()
    {
        var xml = "<NoDefaultCtor><UnmatchedProp>42</UnmatchedProp></NoDefaultCtor>";
        var sut = new DefaultXmlConverter(null, new List<XmlConverter>());
        using var reader = XmlReader.Create(new StringReader(xml));
        Assert.Throws<System.Runtime.Serialization.SerializationException>(() => sut.ReadXml(reader, typeof(NoDefaultCtor)));
    }

    // ---- Test assets ----

    public class SimpleModel
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class ModelWithIgnored
    {
        public string Name { get; set; }
        [XmlIgnore]
        public string Ignored { get; set; }
    }

    public class ModelWithXmlAttribute
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
    }

    public class ModelWithList
    {
        public List<string> Items { get; set; }
    }

    public class ModelWithOptional
    {
        public string Name { get; set; }
        public string Optional { get; set; }
    }

    public class XmlSerializableModel : IXmlSerializable
    {
        private readonly string _value;

        public XmlSerializableModel(string value)
        {
            _value = value;
        }

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader) { }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(_value);
        }
    }

    public class ModelWithStaticFactory
    {
        private ModelWithStaticFactory(int identifier, string name)
        {
            Id = identifier;
            Label = name;
        }

        public int Id { get; }
        public string Label { get; }

        public static ModelWithStaticFactory Create(int id, string label) => new ModelWithStaticFactory(id, label);
    }

    public class NoDefaultCtor
    {
        public NoDefaultCtor(int required)
        {
            Required = required;
        }

        public int Required { get; }
    }
}
