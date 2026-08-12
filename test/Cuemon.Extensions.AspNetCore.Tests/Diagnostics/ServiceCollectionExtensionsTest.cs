using System;
using System.Linq;
using Cuemon.AspNetCore.Diagnostics;
using Codebelt.Extensions.Xunit;
using Cuemon.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Diagnostics;
public class ServiceCollectionExtensionsTest : Test
{
    public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void AddServerTiming_ShouldAddToServiceCollection_HavingLifetimeOfScope()
    {
        var sut1 = new ServiceCollection().AddServerTiming();
        var sut2 = sut1.Single(sd => sd.ServiceType == typeof(IServerTiming));

        Assert.True(sut2.Lifetime == ServiceLifetime.Scoped);
        Assert.True(sut2.ImplementationType == typeof(ServerTiming));
    }

    [Fact]
    public void AddServerTiming_ShouldRegisterCustomImplementationAndConfiguredOptions()
    {
        var services = new ServiceCollection();

        services.AddOptions();
        services.AddServerTiming<FakeServerTiming>(o => o.TimeMeasureCompletedThreshold = TimeSpan.FromMilliseconds(42));

        var descriptor = Assert.Single(services.Where(sd => sd.ServiceType == typeof(IServerTiming)));
        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<ServerTimingOptions>>().Value;

        Assert.Equal(ServiceLifetime.Scoped, descriptor.Lifetime);
        Assert.Equal(typeof(FakeServerTiming), descriptor.ImplementationType);
        Assert.Equal(TimeSpan.FromMilliseconds(42), options.TimeMeasureCompletedThreshold);
    }

    [Fact]
    public void AddFaultDescriptorOptions_ShouldCopySensitivityDetailsToExceptionDescriptorOptions()
    {
        var services = new ServiceCollection();

        services.AddOptions();
        services.AddFaultDescriptorOptions(o =>
        {
            o.SensitivityDetails = FaultSensitivityDetails.Failure;
            o.RootHelpLink = new Uri("https://docs.cuemon.net/errors");
            o.UseBaseException = true;
        });

        var provider = services.BuildServiceProvider();
        var faultOptions = provider.GetRequiredService<IOptions<FaultDescriptorOptions>>().Value;
        var exceptionOptions = provider.GetRequiredService<IOptions<ExceptionDescriptorOptions>>().Value;

        Assert.Equal(FaultSensitivityDetails.Failure, faultOptions.SensitivityDetails);
        Assert.Equal(new Uri("https://docs.cuemon.net/errors"), faultOptions.RootHelpLink);
        Assert.True(faultOptions.UseBaseException);
        Assert.Equal(FaultSensitivityDetails.Failure, exceptionOptions.SensitivityDetails);
    }

    [Fact]
    public void AddExceptionDescriptorOptionsAndPostConfigureAll_ShouldApplyToAllRegisteredOptions()
    {
        var services = new ServiceCollection();

        services.AddOptions();
        services.AddExceptionDescriptorOptions(o => o.SensitivityDetails = FaultSensitivityDetails.None);
        services.AddFaultDescriptorOptions(o => o.SensitivityDetails = FaultSensitivityDetails.Evidence);
        services.PostConfigureAllExceptionDescriptorOptions(o => o.SensitivityDetails = FaultSensitivityDetails.All);

        var provider = services.BuildServiceProvider();
        var faultOptions = provider.GetRequiredService<IOptions<FaultDescriptorOptions>>().Value;
        var exceptionOptions = provider.GetRequiredService<IOptions<ExceptionDescriptorOptions>>().Value;

        Assert.Equal(FaultSensitivityDetails.All, faultOptions.SensitivityDetails);
        Assert.Equal(FaultSensitivityDetails.All, exceptionOptions.SensitivityDetails);
    }

    private sealed class FakeServerTiming : IServerTiming
    {
        private readonly ServerTiming _inner = new ServerTiming();

        public System.Collections.Generic.IEnumerable<ServerTimingMetric> Metrics => _inner.Metrics;

        public IServerTiming AddServerTiming(string name)
        {
            _inner.AddServerTiming(name);
            return this;
        }

        public IServerTiming AddServerTiming(string name, TimeSpan duration)
        {
            _inner.AddServerTiming(name, duration);
            return this;
        }

        public IServerTiming AddServerTiming(string name, TimeSpan duration, string description)
        {
            _inner.AddServerTiming(name, duration, description);
            return this;
        }
    }
}
