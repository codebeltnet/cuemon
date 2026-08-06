using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon;
public class DisposableOptionsTest : Test
{
    public DisposableOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Ctor_ShouldSetLeaveOpenToFalse()
    {
        var sut = new DisposableOptions();

        Assert.False(sut.LeaveOpen);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void LeaveOpen_ShouldBeSettable(bool value)
    {
        var sut = new DisposableOptions
        {
            LeaveOpen = value
        };

        Assert.Equal(value, sut.LeaveOpen);
    }
}
