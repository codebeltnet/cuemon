using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Runtime.Serialization;
using Cuemon.Extensions.Xml.Assets;
using Cuemon.Xml.Serialization;
using Xunit;

namespace Cuemon.Extensions.Xml
{
    public class CoverageTest : Test
    {
        public CoverageTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void XmlExtensions_ShouldCreateReadersFromBytesStreamsAndUris()
        {
            var xml = "<?xml version=\"1.0\"?><root><!--comment--><item>42</item></root>";
            var bytes = Encoding.UTF8.GetBytes(xml);
            var filePath = Path.Combine(Environment.CurrentDirectory, $"xml-{Guid.NewGuid():N}.xml");
            File.WriteAllText(filePath, xml, Encoding.UTF8);
            try
            {
                using var byteReader = bytes.ToXmlReader(o => o.IgnoreComments = true);
                using var streamReader = new MemoryStream(bytes).ToXmlReader();
                using var uriReader = new Uri(filePath).ToXmlReader();

                Assert.True(byteReader.MoveToFirstElement());
                Assert.True(streamReader.MoveToFirstElement());
                Assert.True(uriReader.MoveToFirstElement());
                Assert.Equal("root", byteReader.LocalName);
                Assert.Equal("root", streamReader.LocalName);
                Assert.Equal("root", uriReader.LocalName);
                Assert.Throws<ArgumentNullException>(() => ByteArrayExtensions.ToXmlReader(null));
                Assert.Throws<ArgumentNullException>(() => StreamExtensions.ToXmlReader(null));
                Assert.Throws<ArgumentNullException>(() => UriExtensions.ToXmlReader(null));
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        public void DateTimeAndHierarchyExtensions_ShouldExposeXmlMetadata()
        {
            var dateTime = new DateTime(2024, 6, 7, 8, 9, 10, DateTimeKind.Utc);
            var nodes = new HierarchySerializer(new HierarchyExample()).Nodes;
            var children = nodes.GetChildren().ToList();
            var idNode = children.Single(child => child.MemberReference?.Name == "Id");
            var animalsNode = children.Single(child => child.MemberReference?.Name == "Animals");
            var ownerNameNode = children.Single(child => child.MemberReference?.Name == "Owner").GetChildren().Single(child => child.MemberReference?.Name == "Name");
            var ignoredNode = new HierarchySerializer(new IgnoreExample()).Nodes.GetChildren().Single(child => child.MemberReference?.Name == "Hidden");
            var overrideEntity = new XmlQualifiedEntity("Override");
            var reordered = new[] { ownerNameNode, idNode }.OrderByXmlAttributes().ToList();

            Assert.Equal(System.Xml.XmlConvert.ToString(dateTime, XmlDateTimeSerializationMode.RoundtripKind), dateTime.ToString(XmlDateTimeSerializationMode.RoundtripKind));
            Assert.False(idNode.HasXmlIgnoreAttribute());
            Assert.True(ignoredNode.HasXmlIgnoreAttribute());
            Assert.True(animalsNode.IsNodeEnumerable());
            Assert.False(ownerNameNode.IsNodeEnumerable());
            Assert.Equal("Id", idNode.GetXmlQualifiedEntity().LocalName);
            Assert.Same(overrideEntity, idNode.GetXmlQualifiedEntity(overrideEntity));
            Assert.Equal("Id", reordered[0].MemberReference.Name);
            Assert.Throws<ArgumentNullException>(() => HierarchyExtensions.HasXmlIgnoreAttribute(null));
            Assert.Throws<ArgumentNullException>(() => HierarchyExtensions.IsNodeEnumerable(null));
            Assert.Throws<ArgumentNullException>(() => HierarchyExtensions.GetXmlQualifiedEntity(null, null));
            Assert.Throws<ArgumentNullException>(() => HierarchyExtensions.OrderByXmlAttributes<object>(null));
        }

        [Fact]
        public void XmlReaderAndCopyOptions_ShouldSupportMovingAndCustomCopying()
        {
            var copyOptions = new XmlCopyOptions();
            using var initialReader = XmlReader.Create(new StringReader("<?xml version=\"1.0\"?><root><!--comment--><item>42</item></root>"));
            using var emptyReader = XmlReader.Create(new StringReader("<!--comment-->"));
            using var copiedReader = XmlReader.Create(new StringReader("<root />"));
            using var stream = copiedReader.ToStream((writer, reader, options) =>
            {
                writer.WriteStartElement("copied");
                writer.WriteAttributeString("leaveOpen", options.LeaveOpen.ToString());
                writer.WriteEndElement();
                if (!options.LeaveOpen) { reader.Dispose(); }
            }, o =>
            {
                o.LeaveOpen = false;
                o.WriterSettings = settings => settings.Indent = true;
            });

            copyOptions.WriterSettings = settings => settings.Indent = true;

            Assert.Null(new XmlCopyOptions().WriterSettings);
            Assert.NotNull(copyOptions.WriterSettings);
            Assert.True(initialReader.MoveToFirstElement());
            Assert.Throws<XmlException>(() => emptyReader.MoveToFirstElement());
            Assert.Contains("copied", stream.ToEncodedString());
            Assert.Equal(ReadState.Closed, copiedReader.ReadState);
            Assert.Throws<ArgumentNullException>(() => XmlReaderExtensions.MoveToFirstElement(null));
            Assert.Throws<ArgumentNullException>(() => XmlReaderExtensions.ToStream(null, (Action<XmlWriter, XmlReader, DisposableOptions>)null));
        }

        public class IgnoreExample
        {
            [XmlIgnore]
            public string Hidden => "ignored";

            public string Visible => "shown";
        }
    }
}
