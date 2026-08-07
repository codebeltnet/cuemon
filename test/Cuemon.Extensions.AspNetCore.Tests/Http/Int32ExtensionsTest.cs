using System.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Http;
public class Int32ExtensionsTest : Test
{
    public Int32ExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void IsInformationStatusCode_ShouldBeInRangeOf100To199()
    {
        var sut = Enumerable.Range(100, 100);

        Assert.Equal(100, sut.Min());
        Assert.Equal(199, sut.Max());
        Assert.All(sut, i => Assert.True(i.IsInformationStatusCode()));
    }

    [Fact]
    public void IsSuccessStatusCode_ShouldBeInRangeOf200To299()
    {
        var sut = Enumerable.Range(200, 100);

        Assert.Equal(200, sut.Min());
        Assert.Equal(299, sut.Max());
        Assert.All(sut, i => Assert.True(i.IsSuccessStatusCode()));
    }

    [Fact]
    public void IsRedirectionStatusCode_ShouldBeInRangeOf300To399()
    {
        var sut = Enumerable.Range(300, 100);

        Assert.Equal(300, sut.Min());
        Assert.Equal(399, sut.Max());
        Assert.All(sut, i => Assert.True(i.IsRedirectionStatusCode()));
    }

    [Fact]
    public void IsClientErrorStatusCode_ShouldBeInRangeOf400To499()
    {
        var sut = Enumerable.Range(400, 100);

        Assert.Equal(400, sut.Min());
        Assert.Equal(499, sut.Max());
        Assert.All(sut, i => Assert.True(i.IsClientErrorStatusCode()));
    }

    [Fact]
    public void IsServerErrorStatusCode_ShouldBeInRangeOf500To599()
    {
        var sut = Enumerable.Range(500, 100);

        Assert.Equal(500, sut.Min());
        Assert.Equal(599, sut.Max());
        Assert.All(sut, i => Assert.True(i.IsServerErrorStatusCode()));
    }

    [Fact]
    public void IsNotModifiedStatusCode_ShouldBe304()
    {
        var sut = 304;

        Assert.True(sut.IsNotModifiedStatusCode());
    }

}
