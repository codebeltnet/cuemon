using System.Linq;
using Cuemon.AspNetCore.Configuration;
using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Configuration;
public class ServiceCollectionExtensionsTest : Test
{
    public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void AddAssemblyCacheBusting_ShouldAddToServiceCollection_HavingLifetimeOfSingleton()
    {
        var sut1 = new ServiceCollection().AddAssemblyCacheBusting();
        var sut2 = sut1.Single();

        Assert.True(sut2.Lifetime == ServiceLifetime.Singleton, "sut2.Lifetime == ServiceLifetime.Singleton");
        Assert.True(sut2.ImplementationType == typeof(AssemblyCacheBusting));
    }

    [Fact]
    public void AddDynamicCacheBusting_ShouldAddToServiceCollection_HavingLifetimeOfSingleton()
    {
        var sut1 = new ServiceCollection().AddDynamicCacheBusting();
        var sut2 = sut1.Single();

        Assert.True(sut2.Lifetime == ServiceLifetime.Singleton, "sut2.Lifetime == ServiceLifetime.Singleton");
        Assert.True(sut2.ImplementationType == typeof(DynamicCacheBusting));
    }
}
