using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Runtime.Serialization;
public class HierarchySerializerTest : Test
{
    public HierarchySerializerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Constructor_ShouldCreateHierarchyNodes_WhenSerializingObjectGraph()
    {
        var sut = new HierarchySerializer(new SerializerRoot { Name = "alpha", Child = new SerializerChild { Count = 7 } });

        Assert.NotNull(sut.Nodes);
        Assert.Equal(typeof(SerializerRoot), sut.Nodes.InstanceType);
        Assert.True(sut.Nodes.HasChildren);
    }

    [Fact]
    public void ToString_ShouldDescribeHierarchy_WhenNodesHaveChildren()
    {
        var sut = new HierarchySerializer(new SerializerRoot { Name = "alpha", Child = new SerializerChild { Count = 7 } });
        var text = sut.ToString();

        Assert.Contains("SerializerRoot", text);
        Assert.Contains("SerializerRoot.String", text);
        Assert.Contains("SerializerRoot.SerializerChild", text);

        TestOutput.WriteLine(text);
    }

    private sealed class SerializerRoot
    {
        public string Name { get; set; }

        public SerializerChild Child { get; set; }
    }

    private sealed class SerializerChild
    {
        public int Count { get; set; }
    }
}
