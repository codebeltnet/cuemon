using System;
using System.Collections.Generic;
using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Cuemon.Extensions.Hosting;

public class HostBuilderExtensionsTest : Test
{
    public HostBuilderExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ConfigureConfigurationSources_ShouldThrowArgumentNullException_WhenHostBuilderIsNull()
    {
        IHostBuilder hostBuilder = null;

        Assert.Throws<ArgumentNullException>(() => hostBuilder.ConfigureConfigurationSources((environment, sources) => { }));
    }

    [Fact]
    public void ConfigureConfigurationSources_ShouldThrowArgumentNullException_WhenDelegateIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Host.CreateDefaultBuilder().ConfigureConfigurationSources(null));
    }

    [Fact]
    public void ConfigureConfigurationSources_ShouldInvokeDelegateWithEnvironmentAndSources_WhenHostIsBuilt()
    {
        var delegateCalled = false;
        IHostEnvironment capturedEnvironment = null;
        IList<IConfigurationSource> capturedSources = null;

        var hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureConfigurationSources((environment, sources) =>
            {
                delegateCalled = true;
                capturedEnvironment = environment;
                capturedSources = sources;
            });

        using (var host = hostBuilder.Build())
        {
            Assert.True(delegateCalled);
            Assert.NotNull(capturedEnvironment);
            Assert.NotNull(capturedSources);
        }
    }

    [Fact]
    public void RemoveConfigurationSource_ShouldInvokePredicateForEachSource_WhenHostIsBuilt()
    {
        var predicateCalled = false;
        IConfigurationSource sourceToRemove = null;

        var hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Sentinel", "Present" }
                });
                sourceToRemove = builder.Sources[builder.Sources.Count - 1];
            })
            .RemoveConfigurationSource((environment, source) =>
            {
                predicateCalled = true;
                return ReferenceEquals(source, sourceToRemove);
            });

        using (var host = hostBuilder.Build())
        {
            var configuration = (IConfiguration)host.Services.GetService(typeof(IConfiguration));

            Assert.True(predicateCalled);
            Assert.NotNull(configuration);
            Assert.Null(configuration["Sentinel"]);
        }
    }
}
