using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.Runtime.Serialization;
using Cuemon.Extensions.Xml.Assets;
using Cuemon.Xml.Serialization;
using Xunit;

namespace Cuemon.Extensions.Xml
{
    public class HierarchyExtensionsTest : Test
    {
        public HierarchyExtensionsTest(ITestOutputHelper output) : base(output)
        {
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

        public class IgnoreExample
        {
            [XmlIgnore]
            public string Hidden => "ignored";

            public string Visible => "shown";
        }
    }
}
