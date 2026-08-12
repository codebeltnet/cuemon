using Cuemon.Assets;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.Runtime.Serialization;
using Xunit;

namespace Cuemon.Runtime.Serialization;
public class HierarchySerializerTest : Test
{
    public HierarchySerializerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Ctor_ShouldMakeHierarchy()
    {
        var sut1 = new HierarchyExample();
        var sut2 = new HierarchySerializer(sut1);
    }
}
