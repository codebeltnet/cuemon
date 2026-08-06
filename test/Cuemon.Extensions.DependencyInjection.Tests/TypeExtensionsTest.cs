using System;
using Cuemon.Extensions.DependencyInjection.Assets;
using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.Extensions.DependencyInjection;
public class TypeExtensionsTest : Test
{
    public TypeExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void TryGetDependencyInjectionMarker_ShouldReturnMarkerTypeForGeneric()
    {
        Assert.False(typeof(DefaultService).TryGetDependencyInjectionMarker(out _));
        Assert.True(typeof(DefaultService<>).TryGetDependencyInjectionMarker(out _));
    }

    [Fact]
    public void TryGetDependencyInjectionMarker_ShouldReturnIndividualImplementations()
    {
        var sut1 = new ServiceCollection();
        sut1.AddSingleton<IService, DefaultService>();
        sut1.AddSingleton<IService<long>, DefaultService<long>>();
        sut1.AddSingleton<IService<int>, DefaultService<int>>();
        sut1.AddSingleton<IService<Guid>, DefaultService<Guid>>();
        var sut2 = sut1.BuildServiceProvider();

        Assert.IsType<DefaultService>(sut2.GetRequiredService<IService>());
        Assert.IsType<DefaultService<int>>(sut2.GetRequiredService<IService<int>>());
        Assert.IsType<DefaultService<long>>(sut2.GetRequiredService<IService<long>>());
        Assert.IsType<DefaultService<Guid>>(sut2.GetRequiredService<IService<Guid>>());
        Assert.Equal("DefaultService", sut2.GetRequiredService<IService>().ServiceType);
        Assert.Equal("DefaultService:Int32", sut2.GetRequiredService<IService<int>>().ServiceType);
        Assert.Equal("DefaultService:Int64", sut2.GetRequiredService<IService<long>>().ServiceType);
        Assert.Equal("DefaultService:Guid", sut2.GetRequiredService<IService<Guid>>().ServiceType);
    }
}
