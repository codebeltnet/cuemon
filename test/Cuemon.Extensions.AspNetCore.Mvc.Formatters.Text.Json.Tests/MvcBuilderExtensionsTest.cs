using Cuemon.Extensions.Text.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;

public class MvcBuilderExtensionsTest : Test
{
    public MvcBuilderExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void AddJsonFormattersOptions_ShouldConfigureOptions_WhenCalledOnIMvcBuilder()
    {
        var services = new ServiceCollection();
        var builder = services.AddControllers().AddJsonFormatters();

        var result = builder.AddJsonFormattersOptions(o => o.Settings.WriteIndented = true);

        Assert.Same(builder, result);

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<JsonFormatterOptions>>().Value;

        Assert.True(options.Settings.WriteIndented);
    }
}
