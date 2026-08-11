using System;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Razor.TagHelpers;
public class AppImageTagHelperTest : Test
{
    public AppImageTagHelperTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task Page_RenderImageTagForAppRole()
    {
        var body = await TagHelperTestFactory.GetBodyAsync("/AppImageTagHelper");

        TestOutput.WriteLine(body);

        Assert.Equal(@"<img class=""hero-logo-image"" src=""//static.cuemon.net/cuemon-logo.svg"" alt=""Cuemon for .NET"">", body, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public async Task Page_RenderImageTagForAppRole_WithCacheBusting()
    {
        var body = await TagHelperTestFactory.GetBodyAsync("/AppImageTagHelper", useCacheBusting: true);

        TestOutput.WriteLine(body);

        Assert.Equal(@"<img class=""hero-logo-image"" src=""//static.cuemon.net/cuemon-logo.svg?v=00000000000000000000000000000000"" alt=""Cuemon for .NET"">", body, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public async Task Page_RenderImageTagForAppRole_UsingCurrentRequestOrigin()
    {
        var body = await TagHelperTestFactory.GetBodyAsync("/AppImageTagHelper?path=/images/logo.svg", o =>
        {
            o.BaseUrlMode = TagHelperBaseUrlMode.Automatic;
            o.BaseUrl = null;
        }, baseAddress: new Uri("https://localhost:7241"));

        TestOutput.WriteLine(body);

        Assert.Equal(@"<img class=""hero-logo-image"" src=""https://localhost:7241/images/logo.svg"" alt=""Cuemon for .NET"">", body, ignoreLineEndingDifferences: true);
    }
}
