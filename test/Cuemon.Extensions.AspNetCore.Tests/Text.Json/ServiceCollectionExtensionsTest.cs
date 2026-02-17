using System.Linq;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.Text.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Text.Json
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddMinimalJsonOptions_ShouldRegisterMinimalJsonOptionsAsSingleton()
        {
            var sut = new ServiceCollection();

            sut.AddMinimalJsonOptions();

            var descriptor = sut.Single(sd =>
                sd.ServiceType == typeof(IConfigureOptions<JsonOptions>) &&
                sd.ImplementationType == typeof(MinimalJsonOptions));

            TestOutput.WriteLine($"Lifetime: {descriptor.Lifetime}");

            Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
        }

        [Fact]
        public void AddMinimalJsonOptions_ShouldRegisterMinimalJsonOptionsOnlyOnce_WhenCalledMultipleTimes()
        {
            var sut = new ServiceCollection();

            sut.AddMinimalJsonOptions();
            sut.AddMinimalJsonOptions();
            sut.AddMinimalJsonOptions();

            var count = sut.Count(sd =>
                sd.ServiceType == typeof(IConfigureOptions<JsonOptions>) &&
                sd.ImplementationType == typeof(MinimalJsonOptions));

            TestOutput.WriteLine($"MinimalJsonOptions registrations: {count}");

            Assert.Equal(1, count);
        }

        [Fact]
        public void AddMinimalJsonOptions_ShouldAlsoRegisterJsonExceptionResponseFormatter()
        {
            var sut = new ServiceCollection();
            sut.AddFaultDescriptorOptions();

            sut.AddMinimalJsonOptions();

            var hasFormatter = sut.Any(sd =>
                sd.ServiceType == typeof(HttpExceptionDescriptorResponseFormatter<JsonFormatterOptions>));

            Assert.True(hasFormatter);
        }

        [Fact]
        public void AddMinimalJsonOptions_ShouldRegisterJsonFormatterOptions()
        {
            var sut = new ServiceCollection();

            sut.AddMinimalJsonOptions();

            var count = sut.Count(sd =>
                sd.ServiceType == typeof(IConfigureOptions<JsonFormatterOptions>));

            TestOutput.WriteLine($"IConfigureOptions<JsonFormatterOptions> registrations: {count}");

            Assert.True(count >= 1);
        }
    }
}
