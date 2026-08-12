using System;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Razor.TagHelpers;
public class AppLinkTagHelperTest : Test
{
    public AppLinkTagHelperTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task Page_RenderLinkTagForAppRole()
    {
        var body = await TagHelperTestFactory.GetBodyAsync("/AppLinkTagHelper");

        TestOutput.WriteLine(body);

        Assert.Equal(@"<link rel=""icon"" href=""//static.cuemon.net/favicon.svg"" type=""image/svg+xml"">", body, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public async Task Page_RenderLinkTagForAppRole_WithCacheBusting()
    {
        var body = await TagHelperTestFactory.GetBodyAsync("/AppLinkTagHelper", useCacheBusting: true);

        TestOutput.WriteLine(body);

        Assert.Equal(@"<link rel=""icon"" href=""//static.cuemon.net/favicon.svg?v=00000000000000000000000000000000"" type=""image/svg+xml"">", body, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public async Task Page_RenderLinkTagForAppRole_WithoutConfiguredBaseUrl()
    {
        var body = await TagHelperTestFactory.GetBodyAsync("/AppLinkTagHelper?variant=stylesheet&path=/css/site.css", o => o.BaseUrl = null);

        TestOutput.WriteLine(body);

        Assert.Equal(@"<link rel=""stylesheet"" href=""/css/site.css"" type=""text/css"">", body, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public async Task Page_RenderLinkTagForAppRole_UsingCurrentRequestOrigin()
    {
        var body = await TagHelperTestFactory.GetBodyAsync("/AppLinkTagHelper?variant=stylesheet&path=css/site.css", o =>
        {
            o.BaseUrlMode = TagHelperBaseUrlMode.Automatic;
            o.BaseUrl = null;
            o.Scheme = ProtocolUriScheme.Http;
        }, baseAddress: new Uri("https://localhost:7241"));

        TestOutput.WriteLine(body);

        Assert.Equal(@"<link rel=""stylesheet"" href=""https://localhost:7241/css/site.css"" type=""text/css"">", body, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public async Task Page_RenderLinkTagForAppRole_UsingExplicitBaseUrlWhenAutomaticModeIsEnabled()
    {
        var body = await TagHelperTestFactory.GetBodyAsync("/AppLinkTagHelper?variant=stylesheet&path=/css/site.css", o =>
        {
            o.BaseUrlMode = TagHelperBaseUrlMode.Automatic;
            o.BaseUrl = "assets.example.com";
            o.Scheme = ProtocolUriScheme.Https;
        }, baseAddress: new Uri("http://localhost:7241"));

        TestOutput.WriteLine(body);

        Assert.Equal(@"<link rel=""stylesheet"" href=""https://assets.example.com/css/site.css"" type=""text/css"">", body, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public async Task Page_RenderLinkTagForAppRole_PreservesFontPreloadAttributes()
    {
        var body = await TagHelperTestFactory.GetBodyAsync("/AppLinkTagHelper?variant=preload");

        TestOutput.WriteLine(body);

        Assert.Contains(@"<link", body);
        Assert.Contains(@"rel=""preload""", body);
        Assert.Contains(@"href=""//static.cuemon.net/fonts/antonio-latin.woff2""", body);
        Assert.Contains(@"as=""font""", body);
        Assert.Contains(@"type=""font/woff2""", body);
        Assert.Contains("crossorigin", body);
    }

    [Fact]
    public async Task Page_RenderLinkTagForAppRole_PreservesMaskIconAttributes()
    {
        var body = await TagHelperTestFactory.GetBodyAsync("/AppLinkTagHelper?variant=mask-icon");

        TestOutput.WriteLine(body);

        Assert.Contains(@"<link", body);
        Assert.Contains(@"rel=""mask-icon""", body);
        Assert.Contains(@"href=""//static.cuemon.net/mask-icon.svg""", body);
        Assert.Contains(@"type=""image/svg+xml""", body);
        Assert.Contains(@"color=""#000000""", body);
    }
}
