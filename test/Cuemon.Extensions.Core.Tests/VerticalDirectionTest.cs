using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions;
public class VerticalDirectionTest : Test
{
    public VerticalDirectionTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void EnumValues_ShouldMatchExpectedDirections_WhenAccessed()
    {
        Assert.Equal(0, (int)VerticalDirection.Down);
        Assert.Equal(1, (int)VerticalDirection.Up);
    }
}
