using System;
using System.Linq;
using System.Xml;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Xml.Assets;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.Runtime.Serialization;
using Cuemon.Xml.Serialization.Formatters;
using Xunit;

namespace Cuemon.Extensions.Xml;
public class XmlReaderExtensionsTest : Test
{
    public XmlReaderExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ToStream_ShouldConvertXmlReaderToStream()
    {
        var sut1 = new XmlFormatter(o => o.Settings.Writer.Indent = true).Serialize(new HierarchyExample());
        var sut2 = XmlReader.Create(sut1);
        var sut3 = sut2.ToStream();

        TestOutput.WriteLine(sut3.ToEncodedString(o => o.LeaveOpen = true));

        Assert.Equal(sut1.Length, sut3.Length);
        Assert.Equal(sut1.ToEncodedString(), sut3.ToEncodedString());
    }

    [Fact]
    public void Chunk_ShouldSplitOneXmlReaderIntoThreeSmaller()
    {
        var sut1 = new XmlFormatter(o => o.Settings.Writer.Indent = true).Serialize(new HierarchyExample());
        var sut2 = XmlReader.Create(sut1);
        var sut3 = sut2.Chunk(1, o => o.Indent = true);

        sut3.First().MoveToFirstElement();
        sut3.Skip(1).First().MoveToFirstElement();
        sut3.Last().MoveToFirstElement();

        TestOutput.WriteLine(sut1.ToEncodedString(o => o.LeaveOpen = true));

        Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<HierarchyExample Id=""00000000-0000-0000-0000-000000000000"">
	<Animals>
		<Item>
			<Dog Output=""Vooooof"" />
		</Item>
		<Item>
			<Cat Output=""Mioauw"" />
		</Item>
		<Item>
			<Pig Output=""Oink"" />
		</Item>
	</Animals>
	<Owner Age=""42"">
		<Name>Gimlichael</Name>
		<Address>
			<City>Gilleleje</City>
			<PostalCode>3250</PostalCode>
		</Address>
	</Owner>
	<Cuemon Name=""Cuemon for .NET"" Tags=""* 42 Infinity"">
		<Version>
			<HasAlphanumericVersion>false</HasAlphanumericVersion>
			<Value>6.0.0</Value>
		</Version>
	</Cuemon>
</HierarchyExample>", sut1.ToEncodedString(), ignoreLineEndingDifferences: true);
        Assert.Equal(3, sut3.Count());
        Assert.Equal(@"<HierarchyExample>
  <Animals>
    <Item>
      <Dog Output=""Vooooof"" />
    </Item>
    <Item>
      <Cat Output=""Mioauw"" />
    </Item>
    <Item>
      <Pig Output=""Oink"" />
    </Item>
  </Animals>
</HierarchyExample>", sut3.First().ReadOuterXml(), ignoreLineEndingDifferences: true);
        Assert.Equal(@"<HierarchyExample>
  <Owner Age=""42"">
    <Name>Gimlichael</Name>
    <Address>
      <City>Gilleleje</City>
      <PostalCode>3250</PostalCode>
    </Address>
  </Owner>
</HierarchyExample>", sut3.Skip(1).First().ReadOuterXml(), ignoreLineEndingDifferences: true);
        Assert.Equal(@"<HierarchyExample>
  <Cuemon Name=""Cuemon for .NET"" Tags=""* 42 Infinity"">
    <Version>
      <HasAlphanumericVersion>false</HasAlphanumericVersion>
      <Value>6.0.0</Value>
    </Version>
  </Cuemon>
</HierarchyExample>", sut3.Last().ReadOuterXml(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void ToHierarchy_ShouldConvertReaderToHierarchy()
    {
        var sut1 = new HierarchySerializer(new HierarchyExample());
        var sut2 = new XmlFormatter(o => o.Settings.Writer.Indent = true).Serialize(new HierarchyExample());
        var sut3 = XmlReader.Create(sut2);
        var sut4 = sut3.ToHierarchy();
        var sut5 = sut4.GetChildren(); // namespace

        TestOutput.WriteLine(sut1.ToString());
        TestOutput.WriteLine(sut2.ToEncodedString());

        Assert.NotNull(sut3);
        Assert.True(sut4.HasChildren);
        Assert.Equal(sut1.Nodes.GetChildren().Count(), sut4.GetChildren().Count());
        Assert.Equal("HierarchyExample", sut4.Instance.Name);
        Assert.Collection(sut5, h =>
        {
            Assert.Equal("Id", h.Instance.Name);
            Assert.Equal(Guid.Empty, h.Instance.Value);
            Assert.False(h.HasChildren);
        }, h =>
        {
            Assert.Equal("Animals", h.Instance.Name);
            Assert.Equal(null, h.Instance.Value);
            Assert.True(h.HasChildren);
            Assert.Collection(h.GetChildren(), i =>
            {
                Assert.Equal("Item", i.Instance.Name);
                Assert.Equal(null, i.Instance.Value);
                Assert.Collection(i.GetChildren(), a =>
                {
                    Assert.Equal("Dog", a.Instance.Name);
                    Assert.Equal(null, a.Instance.Value);
                    Assert.True(a.HasChildren);
                    Assert.Collection(a.GetChildren(), d =>
                    {
                        Assert.Equal("Output", d.Instance.Name);
                        Assert.Equal("Vooooof", d.Instance.Value);
                        Assert.False(d.HasChildren);
                    });
                }, a =>
                {
                    Assert.Equal("Cat", a.Instance.Name);
                    Assert.Equal(null, a.Instance.Value);
                    Assert.True(a.HasChildren);
                    Assert.Collection(a.GetChildren(), d =>
                    {
                        Assert.Equal("Output", d.Instance.Name);
                        Assert.Equal("Mioauw", d.Instance.Value);
                        Assert.False(d.HasChildren);
                    });
                }, a =>
                {
                    Assert.Equal("Pig", a.Instance.Name);
                    Assert.Equal(null, a.Instance.Value);
                    Assert.True(a.HasChildren);
                    Assert.Collection(a.GetChildren(), d =>
                    {
                        Assert.Equal("Output", d.Instance.Name);
                        Assert.Equal("Oink", d.Instance.Value);
                        Assert.False(d.HasChildren);
                    });
                });
            }, i =>
            {
                Assert.Equal("Item", i.Instance.Name);
                Assert.Equal(null, i.Instance.Value);
            }, i =>
            {
                Assert.Equal("Item", i.Instance.Name);
                Assert.Equal(null, i.Instance.Value);
            });
        }, h =>
        {
            Assert.Equal("Owner", h.Instance.Name);
            Assert.Equal(null, h.Instance.Value);
            Assert.True(h.HasChildren);
            Assert.Collection(h.GetChildren(), o =>
            {
                Assert.Equal("Age", o.Instance.Name);
                Assert.Equal((byte)42, o.Instance.Value);
                Assert.False(o.HasChildren);
            }, o =>
            {
                Assert.Equal("Name", o.Instance.Name);
                Assert.Equal("Gimlichael", o.Instance.Value);
                Assert.False(o.HasChildren);
            }, o =>
            {
                Assert.Equal("Address", o.Instance.Name);
                Assert.Equal(null, o.Instance.Value);
                Assert.True(o.HasChildren);
                Assert.Collection(o.GetChildren(), a =>
                {
                    Assert.Equal("City", a.Instance.Name);
                    Assert.Equal("Gilleleje", a.Instance.Value);
                    Assert.False(a.HasChildren);
                }, a =>
                {
                    Assert.Equal("PostalCode", a.Instance.Name);
                    Assert.Equal(3250, a.Instance.Value);
                    Assert.False(a.HasChildren);
                });
            });
        }, h =>
        {
            Assert.Equal("Cuemon", h.Instance.Name);
            Assert.Equal(null, h.Instance.Value);
            Assert.True(h.HasChildren);
            Assert.Collection(h.GetChildren(), c =>
            {
                Assert.Equal("Name", c.Instance.Name);
                Assert.Equal("Cuemon for .NET", c.Instance.Value);
                Assert.False(c.HasChildren);
            }, c =>
            {
                Assert.Equal("Tags", c.Instance.Name);
                Assert.Equal("* 42 Infinity", c.Instance.Value);
                Assert.False(c.HasChildren);
            }, c =>
            {
                Assert.Equal("Version", c.Instance.Name);
                Assert.Equal(null, c.Instance.Value);
                Assert.True(c.HasChildren);
                Assert.Collection(c.GetChildren(), v =>
                {
                    Assert.Equal("HasAlphanumericVersion", v.Instance.Name);
                    Assert.Equal(false, v.Instance.Value);
                }, v =>
                {
                    Assert.Equal("Value", v.Instance.Name);
                    Assert.Equal("6.0.0", v.Instance.Value);
                });
            });
        });
    }
}
