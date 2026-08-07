using System;
using System.Text.Json;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Text.Json.Converters;
public class StringEnumConverterTest : Test
{
    public StringEnumConverterTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CreateConverter_ShouldResolveOrFallback_DependingOnRuntimeSupport()
    {
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = null
        };
        options.Converters.Add(new StringEnumConverter());

        var exception = Record.Exception(() => JsonSerializer.Serialize(DayOfWeek.Friday, options));

        if (exception == null)
        {
            var json = JsonSerializer.Serialize(DayOfWeek.Friday, options);
            Assert.Equal("\"Friday\"", json);
            return;
        }

        var notSupported = Assert.IsType<NotSupportedException>(exception);
        Assert.Equal("Unable to locate internal members required by this method.", notSupported.Message);
    }
}
