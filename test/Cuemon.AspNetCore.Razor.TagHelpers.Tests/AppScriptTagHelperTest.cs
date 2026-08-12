using System;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Razor.TagHelpers;
public class AppScriptTagHelperTest : Test
{
    public AppScriptTagHelperTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task Page_RenderScriptTagForAppRole()
    {
        var body = await TagHelperTestFactory.GetBodyAsync("/AppScriptTagHelper");

        TestOutput.WriteLine(body);

        Assert.Equal(@"<script type=""text/javascript"" src=""//static.cuemon.net/js/site.js""></script>", body, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public async Task Page_RenderScriptTagForAppRole_WithCacheBusting()
    {
        var body = await TagHelperTestFactory.GetBodyAsync("/AppScriptTagHelper", useCacheBusting: true);

        TestOutput.WriteLine(body);

        Assert.Equal(@"<script type=""text/javascript"" src=""//static.cuemon.net/js/site.js?v=00000000000000000000000000000000""></script>", body, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public async Task Page_RenderScriptTagForAppRole_UsingCurrentRequestOrigin()
    {
        var body = await TagHelperTestFactory.GetBodyAsync("/AppScriptTagHelper", o =>
        {
            o.BaseUrlMode = TagHelperBaseUrlMode.Automatic;
            o.BaseUrl = null;
        }, baseAddress: new Uri("https://localhost:7241"));

        TestOutput.WriteLine(body);

        Assert.Equal(@"<script type=""text/javascript"" src=""https://localhost:7241/js/site.js""></script>", body, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public async Task Page_RenderScriptTagForAppRole_UsingCurrentRequestOriginAndPathBase_WithCacheBusting()
    {
        var body = await TagHelperTestFactory.GetBodyAsync("/myapp/AppScriptTagHelper?path=~/js/site.js", o =>
        {
            o.BaseUrlMode = TagHelperBaseUrlMode.Automatic;
            o.BaseUrl = null;
        }, useCacheBusting: true, baseAddress: new Uri("https://example.com"), pathBase: "/myapp");

        TestOutput.WriteLine(body);

        Assert.Equal(@"<script type=""text/javascript"" src=""https://example.com/myapp/js/site.js?v=00000000000000000000000000000000""></script>", body, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public async Task Page_RenderScriptTagForAppRole_UsingLocalHttpExternalOriginWhenConfigured()
    {
        var body = await TagHelperTestFactory.GetBodyAsync("/AppScriptTagHelper?path=~/js/site.js", o =>
        {
            o.BaseUrlMode = TagHelperBaseUrlMode.Automatic;
            o.BaseUrl = "localhost:8080";
            o.Scheme = ProtocolUriScheme.Http;
        }, baseAddress: new Uri("https://localhost:7241"));

        TestOutput.WriteLine(body);

        Assert.Equal(@"<script type=""text/javascript"" src=""http://localhost:8080/js/site.js""></script>", body, ignoreLineEndingDifferences: true);
    }
}
