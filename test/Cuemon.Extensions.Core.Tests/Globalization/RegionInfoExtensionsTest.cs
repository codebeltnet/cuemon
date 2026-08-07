using System;
using System.Globalization;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Globalization;
public class RegionInfoExtensionsTest : Test
{
    public RegionInfoExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GetCultures_ShouldReturnMatchingCultures_WhenRegionExists()
    {
        var cultures = new RegionInfo("US").GetCultures().ToList();

        Assert.NotEmpty(cultures);
        Assert.Contains(cultures, culture => culture.Name.Equals("en-US", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void GetCultures_ShouldThrowArgumentNullException_WhenRegionIsNull()
    {
        RegionInfo region = null;

        var exception = Assert.Throws<ArgumentNullException>(() => region.GetCultures().ToList());

        Assert.Equal("region", exception.ParamName);
    }
}
