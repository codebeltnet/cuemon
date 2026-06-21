using Codebelt.Extensions.Xunit;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;

public class MvcBuilderExtensionsTest : Test
{
    public MvcBuilderExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void AddXmlFormattersOptions_ShouldConfigureOptions_WhenCalledOnIMvcBuilder()
    {
        var services = new ServiceCollection();
        var builder = services.AddControllers().AddXmlFormatters();

        var result = builder.AddXmlFormattersOptions(o => { });

        Assert.Same(builder, result);

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<XmlFormatterOptions>>().Value;

        Assert.NotNull(options);
        Assert.NotNull(options.Settings);
    }
}
