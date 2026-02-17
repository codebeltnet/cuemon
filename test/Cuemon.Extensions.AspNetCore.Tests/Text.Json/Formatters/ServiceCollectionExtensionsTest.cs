using System.Linq;
using Cuemon.Extensions.Text.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Text.Json.Formatters
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddJsonFormatterOptions_ShouldOnlyRegisterOnce_WhenCalledMultipleTimes()
        {
            var sut = new ServiceCollection();

            sut.AddJsonFormatterOptions();
            sut.AddJsonFormatterOptions();
            sut.AddJsonFormatterOptions();

            var configureOptionsCount = sut.Count(sd =>
                sd.ServiceType == typeof(IConfigureOptions<JsonFormatterOptions>));

            TestOutput.WriteLine($"IConfigureOptions<JsonFormatterOptions> registrations: {configureOptionsCount}");

            Assert.Equal(1, configureOptionsCount);
        }
    }
}
