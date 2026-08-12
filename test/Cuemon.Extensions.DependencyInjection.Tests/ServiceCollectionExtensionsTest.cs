using System;
using System.Linq;
#if NET10_0_OR_GREATER
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Xml.Serialization.Formatters;
#endif
using Cuemon.Diagnostics;
using Cuemon.Extensions.DependencyInjection.Assets;
using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.Extensions.DependencyInjection;
public class ServiceCollectionExtensionsTest : Test
{
    public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void AddWithSetup_ShouldAddServiceToServiceCollectionWithSpecifiedLifetime()
    {
        var sut1 = new ServiceCollection();
        var sut2 = sut1.Add<FakeService, FakeServiceScoped>(o => o.Lifetime = ServiceLifetime.Scoped)
            .Add<FakeService, FakeServiceSingleton>(o => o.Lifetime = ServiceLifetime.Singleton)
            .Add<FakeService, FakeServiceTransient>(o => o.Lifetime = ServiceLifetime.Transient);
        var sut3 = sut2.Single(sd => sd.ImplementationType == typeof(FakeServiceScoped));
        var sut4 = sut2.Single(sd => sd.ImplementationType == typeof(FakeServiceSingleton));
        var sut5 = sut2.Single(sd => sd.ImplementationType == typeof(FakeServiceTransient));

        Assert.Equal(3, sut1.Count);
        Assert.Equal(sut1, sut2);
        Assert.NotNull(sut3);
        Assert.NotNull(sut4);
        Assert.NotNull(sut5);
        Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
        Assert.Equal(ServiceLifetime.Singleton, sut4.Lifetime);
        Assert.Equal(ServiceLifetime.Transient, sut5.Lifetime);
    }

    [Fact]
    public void AddWithSetup_ShouldAddServiceToServiceCollectionUsingFactoryWithSpecifiedLifetime()
    {
        var sut1 = new ServiceCollection();
        var sut2 = sut1.Add<FakeService, FakeServiceScoped>(_ => new FakeServiceScoped(default), o => o.Lifetime = ServiceLifetime.Scoped)
            .Add<FakeService, FakeServiceSingleton>(_ => new FakeServiceSingleton(default), o => o.Lifetime = ServiceLifetime.Singleton)
            .Add<FakeService, FakeServiceTransient>(_ => new FakeServiceTransient(default), o => o.Lifetime = ServiceLifetime.Transient);
        var sut3 = sut2.Single(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceScoped));
        var sut4 = sut2.Single(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceSingleton));
        var sut5 = sut2.Single(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceTransient));

        Assert.Equal(3, sut1.Count);
        Assert.Equal(sut1, sut2);
        Assert.NotNull(sut3);
        Assert.NotNull(sut4);
        Assert.NotNull(sut5);
        Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
        Assert.Equal(ServiceLifetime.Singleton, sut4.Lifetime);
        Assert.Equal(ServiceLifetime.Transient, sut5.Lifetime);
    }

    [Fact]
    public void TryAddWithSetup_ShouldAddServiceToServiceCollectionWithSpecifiedLifetime()
    {
        var sut1 = new ServiceCollection();
        var sut2 = sut1.TryAdd<FakeService, FakeServiceScoped>(o => o.Lifetime = ServiceLifetime.Scoped)
            .TryAdd<FakeService, FakeServiceSingleton>(o => o.Lifetime = ServiceLifetime.Singleton)
            .TryAdd<FakeService, FakeServiceTransient>(o => o.Lifetime = ServiceLifetime.Transient);
        var sut3 = sut2.SingleOrDefault(sd => sd.ImplementationType == typeof(FakeServiceScoped));
        var sut4 = sut2.SingleOrDefault(sd => sd.ImplementationType == typeof(FakeServiceSingleton));
        var sut5 = sut2.SingleOrDefault(sd => sd.ImplementationType == typeof(FakeServiceTransient));

        Assert.Equal(1, sut1.Count);
        Assert.Equal(sut1, sut2);
        Assert.NotNull(sut3);
        Assert.Null(sut4);
        Assert.Null(sut5);
        Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
    }

    [Fact]
    public void TryAddWithSetup_ShouldAddServiceToServiceCollectionUsingFactoryWithSpecifiedLifetime()
    {
        var sut1 = new ServiceCollection();
        var sut2 = sut1.TryAdd<FakeService, FakeServiceScoped>(_ => new FakeServiceScoped(default), o => o.Lifetime = ServiceLifetime.Scoped)
            .TryAdd<FakeService, FakeServiceSingleton>(_ => new FakeServiceSingleton(default), o => o.Lifetime = ServiceLifetime.Singleton)
            .TryAdd<FakeService, FakeServiceTransient>(_ => new FakeServiceTransient(default), o => o.Lifetime = ServiceLifetime.Transient);
        var sut3 = sut2.SingleOrDefault(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceScoped));
        var sut4 = sut2.SingleOrDefault(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceSingleton));
        var sut5 = sut2.SingleOrDefault(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceTransient));

        Assert.Equal(1, sut1.Count);
        Assert.Equal(sut1, sut2);
        Assert.NotNull(sut3);
        Assert.Null(sut4);
        Assert.Null(sut5);
        Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
    }

    [Fact]
    public void AddWithSetup_ShouldAddServiceToServiceCollectionWithTypeForwarding()
    {
        var sut1 = new ServiceCollection()
            .Add<Foo>(o =>
            {
                o.Lifetime = ServiceLifetime.Scoped;
            });

        var provider = sut1.BuildServiceProvider();

        var sut2 = provider.GetRequiredService<Foo>();
        var sut3 = provider.GetRequiredService<IBar>();
        var sut4 = provider.GetRequiredService<IFoo>();

        Assert.Same(sut2, sut3);
        Assert.Same(sut2, sut4);

        Assert.Collection(sut1,
            sd => Assert.True(sd.ServiceType == typeof(Foo)),
            sd => Assert.True(sd.ServiceType == typeof(IFoo)),
            sd => Assert.True(sd.ServiceType == typeof(IBar)));
    }

    [Fact]
    public void AddWithSetup_ShouldAddServiceToServiceCollectionWithoutTypeForwarding()
    {
        var sut1 = new ServiceCollection()
            .Add<Foo>(o =>
            {
                o.UseNestedTypeForwarding = false;
                o.Lifetime = ServiceLifetime.Scoped;
            });

        var provider = sut1.BuildServiceProvider();

        var sut2 = provider.GetService<Foo>();
        var sut3 = provider.GetService<IBar>();
        var sut4 = provider.GetService<IFoo>();

        Assert.NotNull(sut2);
        Assert.Null(sut3);
        Assert.Null(sut4);

        Assert.Collection(sut1,
            sd => Assert.True(sd.ServiceType == typeof(Foo)));
    }

    [Fact]
    public void AddWithSetup_ShouldAddServiceToServiceCollectionWithTypeForwardingForMarker()
    {
        var sut1 = new ServiceCollection()
            .Add<DefaultService<FakeService>>(o =>
            {
                o.Lifetime = ServiceLifetime.Scoped;
            });

        var provider = sut1.BuildServiceProvider();

        var sut2 = provider.GetRequiredService<DefaultService<FakeService>>();
        var sut3 = provider.GetRequiredService<IService<FakeService>>();
        var sut4 = provider.GetRequiredService<IDependencyInjectionMarker<FakeService>>();

        Assert.Same(sut2, sut3);
        Assert.Same(sut2, sut4);

        Assert.Collection(sut1,
            sd => Assert.True(sd.ServiceType == typeof(DefaultService<FakeService>)),
            sd => Assert.True(sd.ServiceType == typeof(IService<FakeService>)),
            sd => Assert.True(sd.ServiceType == typeof(IDependencyInjectionMarker<FakeService>)));
    }

    [Fact]
    public void AddWithSetup_ShouldAddServiceToServiceCollectionWithoutTypeForwardingForMarker()
    {
        var sut1 = new ServiceCollection()
            .Add<DefaultService<FakeService>>(o =>
            {
                o.UseNestedTypeForwarding = false;
                o.Lifetime = ServiceLifetime.Scoped;
            });

        var provider = sut1.BuildServiceProvider();

        var sut2 = provider.GetService<DefaultService<FakeService>>();
        var sut3 = provider.GetService<IService<FakeService>>();
        var sut4 = provider.GetService<IDependencyInjectionMarker<FakeService>>();

        Assert.NotNull(sut2);
        Assert.Null(sut3);
        Assert.Null(sut4);

        Assert.Collection(sut1,
            sd => Assert.True(sd.ServiceType == typeof(DefaultService<FakeService>)));
    }

    [Fact]
    public void Add_ShouldAddServiceToServiceCollectionWithSpecifiedLifetime()
    {
        var sut1 = new ServiceCollection();
        var sut2 = sut1.Add<FakeService, FakeServiceScoped>(ServiceLifetime.Scoped)
            .Add<FakeService, FakeServiceSingleton>(ServiceLifetime.Singleton)
            .Add<FakeService, FakeServiceTransient>(ServiceLifetime.Transient);
        var sut3 = sut2.Single(sd => sd.ImplementationType == typeof(FakeServiceScoped));
        var sut4 = sut2.Single(sd => sd.ImplementationType == typeof(FakeServiceSingleton));
        var sut5 = sut2.Single(sd => sd.ImplementationType == typeof(FakeServiceTransient));

        Assert.Equal(3, sut1.Count);
        Assert.Equal(sut1, sut2);
        Assert.NotNull(sut3);
        Assert.NotNull(sut4);
        Assert.NotNull(sut5);
        Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
        Assert.Equal(ServiceLifetime.Singleton, sut4.Lifetime);
        Assert.Equal(ServiceLifetime.Transient, sut5.Lifetime);
    }

    [Fact]
    public void Add_ShouldAddServiceToServiceCollectionWithSpecifiedLifetimeAndOptions()
    {
        var sut1 = new ServiceCollection();
        var sut2 = sut1.Add<FakeService, FakeServiceScoped, FakeServiceScopedOptions>(ServiceLifetime.Scoped, o => o.Greeting = "Hejsa!")
            .Add<FakeService, FakeServiceSingleton, FakeServiceSingletonOptions>(ServiceLifetime.Singleton, o => o.Greeting = "Hello!")
            .Add<FakeService, FakeServiceTransient, FakeServiceTransientOptions>(ServiceLifetime.Transient, o => o.Greeting = "Aloha!");
        var sut3 = sut2.Single(sd => sd.ImplementationType == typeof(FakeServiceScoped));
        var sut4 = sut2.Single(sd => sd.ImplementationType == typeof(FakeServiceSingleton));
        var sut5 = sut2.Single(sd => sd.ImplementationType == typeof(FakeServiceTransient));
        var sut6 = sut2.BuildServiceProvider();
        var sut7 = sut6.GetServices<FakeService>().Single(fs => fs.GetType() == typeof(FakeServiceScoped));
        var sut8 = sut6.GetServices<FakeService>().Single(fs => fs.GetType() == typeof(FakeServiceSingleton));
        var sut9 = sut6.GetServices<FakeService>().Single(fs => fs.GetType() == typeof(FakeServiceTransient));

        Assert.Equal(11, sut1.Count);
        Assert.Equal(sut1, sut2);
        Assert.NotNull(sut3);
        Assert.NotNull(sut4);
        Assert.NotNull(sut5);
        Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
        Assert.Equal(ServiceLifetime.Singleton, sut4.Lifetime);
        Assert.Equal(ServiceLifetime.Transient, sut5.Lifetime);
        Assert.Equal("Hejsa!", sut7.Greeting);
        Assert.Equal("Hello!", sut8.Greeting);
        Assert.Equal("Aloha!", sut9.Greeting);
    }

    [Fact]
    public void Add_ShouldAddServiceToServiceCollectionUsingFactoryWithSpecifiedLifetime()
    {
        var sut1 = new ServiceCollection();
        var sut2 = sut1.Add<FakeService, FakeServiceScoped>(_ => new FakeServiceScoped(default), ServiceLifetime.Scoped)
            .Add<FakeService, FakeServiceSingleton>(_ => new FakeServiceSingleton(default), ServiceLifetime.Singleton)
            .Add<FakeService, FakeServiceTransient>(_ => new FakeServiceTransient(default), ServiceLifetime.Transient);
        var sut3 = sut2.Single(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceScoped));
        var sut4 = sut2.Single(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceSingleton));
        var sut5 = sut2.Single(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceTransient));

        Assert.Equal(3, sut1.Count);
        Assert.Equal(sut1, sut2);
        Assert.NotNull(sut3);
        Assert.NotNull(sut4);
        Assert.NotNull(sut5);
        Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
        Assert.Equal(ServiceLifetime.Singleton, sut4.Lifetime);
        Assert.Equal(ServiceLifetime.Transient, sut5.Lifetime);
    }

    [Fact]
    public void Add_ShouldAddServiceToServiceCollectionUsingFactoryWithSpecifiedLifetimeAndOptions()
    {
        var sut1 = new ServiceCollection();
        var sut2 = sut1.Add<FakeService, FakeServiceScoped, FakeServiceScopedOptions>(sp => new FakeServiceScoped(sp.GetRequiredService<IOptions<FakeServiceScopedOptions>>()), ServiceLifetime.Scoped, o => o.Greeting = "Hejsa!")
            .Add<FakeService, FakeServiceSingleton, FakeServiceSingletonOptions>(sp => new FakeServiceSingleton(sp.GetRequiredService<IOptions<FakeServiceSingletonOptions>>()), ServiceLifetime.Singleton, o => o.Greeting = "Hello!")
            .Add<FakeService, FakeServiceTransient, FakeServiceTransientOptions>(sp => new FakeServiceTransient(sp.GetRequiredService<IOptions<FakeServiceTransientOptions>>()), ServiceLifetime.Transient, o => o.Greeting = "Aloha!");
        var sut3 = sut2.Single(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceScoped));
        var sut4 = sut2.Single(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceSingleton));
        var sut5 = sut2.Single(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceTransient));
        var sut6 = sut2.BuildServiceProvider();
        var sut7 = sut6.GetServices<FakeService>().Single(fs => fs.GetType() == typeof(FakeServiceScoped));
        var sut8 = sut6.GetServices<FakeService>().Single(fs => fs.GetType() == typeof(FakeServiceSingleton));
        var sut9 = sut6.GetServices<FakeService>().Single(fs => fs.GetType() == typeof(FakeServiceTransient));

        Assert.Equal(11, sut1.Count);
        Assert.Equal(sut1, sut2);
        Assert.NotNull(sut3);
        Assert.NotNull(sut4);
        Assert.NotNull(sut5);
        Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
        Assert.Equal(ServiceLifetime.Singleton, sut4.Lifetime);
        Assert.Equal(ServiceLifetime.Transient, sut5.Lifetime);
        Assert.Equal("Hejsa!", sut7.Greeting);
        Assert.Equal("Hello!", sut8.Greeting);
        Assert.Equal("Aloha!", sut9.Greeting);
    }

    [Fact]
    public void TryAdd_ShouldAddServiceToServiceCollectionWithSpecifiedLifetime()
    {
        var sut1 = new ServiceCollection();
        var sut2 = sut1.TryAdd<FakeService, FakeServiceScoped>(ServiceLifetime.Scoped)
            .TryAdd<FakeService, FakeServiceSingleton>(ServiceLifetime.Singleton)
            .TryAdd<FakeService, FakeServiceTransient>(ServiceLifetime.Transient);
        var sut3 = sut2.SingleOrDefault(sd => sd.ImplementationType == typeof(FakeServiceScoped));
        var sut4 = sut2.SingleOrDefault(sd => sd.ImplementationType == typeof(FakeServiceSingleton));
        var sut5 = sut2.SingleOrDefault(sd => sd.ImplementationType == typeof(FakeServiceTransient));

        Assert.Equal(1, sut1.Count);
        Assert.Equal(sut1, sut2);
        Assert.NotNull(sut3);
        Assert.Null(sut4);
        Assert.Null(sut5);
        Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
    }

    [Fact]
    public void TryAdd_ShouldAddServiceToServiceCollectionWithSpecifiedLifetimeAndOptions()
    {
        var sut1 = new ServiceCollection();
        var sut2 = sut1.TryAdd<FakeService, FakeServiceScoped, FakeServiceScopedOptions>(ServiceLifetime.Scoped, o => o.Greeting = "Hejsa!")
            .TryAdd<FakeService, FakeServiceSingleton, FakeServiceSingletonOptions>(ServiceLifetime.Singleton, o => o.Greeting = "Hello!")
            .TryAdd<FakeService, FakeServiceTransient, FakeServiceTransientOptions>(ServiceLifetime.Transient, o => o.Greeting = "Aloha!");
        var sut3 = sut2.Single(sd => sd.ImplementationType == typeof(FakeServiceScoped));
        var sut4 = sut2.SingleOrDefault(sd => sd.ImplementationType == typeof(FakeServiceSingleton));
        var sut5 = sut2.SingleOrDefault(sd => sd.ImplementationType == typeof(FakeServiceTransient));
        var sut6 = sut2.BuildServiceProvider();
        var sut7 = sut6.GetServices<FakeService>().SingleOrDefault(fs => fs.GetType() == typeof(FakeServiceScoped));
        var sut8 = sut6.GetServices<FakeService>().SingleOrDefault(fs => fs.GetType() == typeof(FakeServiceSingleton));
        var sut9 = sut6.GetServices<FakeService>().SingleOrDefault(fs => fs.GetType() == typeof(FakeServiceTransient));

        Assert.Equal(9, sut1.Count);
        Assert.Equal(sut1, sut2);
        Assert.NotNull(sut3);
        Assert.Null(sut4);
        Assert.Null(sut5);
        Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
        Assert.Equal("Hejsa!", sut7.Greeting);
        Assert.Null(sut8);
        Assert.Null(sut9);
    }

    [Fact]
    public void TryAdd_ShouldAddServiceToServiceCollectionUsingFactoryWithSpecifiedLifetime()
    {
        var sut1 = new ServiceCollection();
        var sut2 = sut1.TryAdd<FakeService, FakeServiceScoped>(_ => new FakeServiceScoped(default), ServiceLifetime.Scoped)
            .TryAdd<FakeService, FakeServiceSingleton>(_ => new FakeServiceSingleton(default), ServiceLifetime.Singleton)
            .TryAdd<FakeService, FakeServiceTransient>(_ => new FakeServiceTransient(default), ServiceLifetime.Transient);
        var sut3 = sut2.SingleOrDefault(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceScoped));
        var sut4 = sut2.SingleOrDefault(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceSingleton));
        var sut5 = sut2.SingleOrDefault(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceTransient));

        Assert.Equal(1, sut1.Count);
        Assert.Equal(sut1, sut2);
        Assert.NotNull(sut3);
        Assert.Null(sut4);
        Assert.Null(sut5);
        Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
    }

    [Fact]
    public void TryAdd_ShouldAddServiceToServiceCollectionUsingFactoryWithSpecifiedLifetimeAndOptions()
    {
        var sut1 = new ServiceCollection();
        var sut2 = sut1.TryAdd<FakeService, FakeServiceScoped, FakeServiceScopedOptions>(sp => new FakeServiceScoped(sp.GetRequiredService<IOptions<FakeServiceScopedOptions>>()), ServiceLifetime.Scoped, o => o.Greeting = "Hejsa!")
            .TryAdd<FakeService, FakeServiceSingleton, FakeServiceSingletonOptions>(sp => new FakeServiceSingleton(sp.GetRequiredService<IOptions<FakeServiceSingletonOptions>>()), ServiceLifetime.Singleton, o => o.Greeting = "Hello!")
            .TryAdd<FakeService, FakeServiceTransient, FakeServiceTransientOptions>(sp => new FakeServiceTransient(sp.GetRequiredService<IOptions<FakeServiceTransientOptions>>()), ServiceLifetime.Transient, o => o.Greeting = "Aloha!");
        var sut3 = sut2.SingleOrDefault(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceScoped));
        var sut4 = sut2.SingleOrDefault(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceSingleton));
        var sut5 = sut2.SingleOrDefault(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceTransient));
        var sut6 = sut2.BuildServiceProvider();
        var sut7 = sut6.GetServices<FakeService>().SingleOrDefault(fs => fs.GetType() == typeof(FakeServiceScoped));
        var sut8 = sut6.GetServices<FakeService>().SingleOrDefault(fs => fs.GetType() == typeof(FakeServiceSingleton));
        var sut9 = sut6.GetServices<FakeService>().SingleOrDefault(fs => fs.GetType() == typeof(FakeServiceTransient));

        Assert.Equal(9, sut1.Count);
        Assert.Equal(sut1, sut2);
        Assert.NotNull(sut3);
        Assert.Null(sut4);
        Assert.Null(sut5);
        Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
        Assert.Equal("Hejsa!", sut7.Greeting);
        Assert.Null(sut8);
        Assert.Null(sut9);
    }

#if NET10_0_OR_GREATER

    [Fact]
    public void TryConfigure_ShouldAddConfigureOptions()
    {
        var services = new ServiceCollection()
            .TryConfigure<ExceptionDescriptorOptions>(o => o.SensitivityDetails = FaultSensitivityDetails.All);

        var serviceProvider = services.BuildServiceProvider();

        var exceptionDescriptorOptions = serviceProvider.GetRequiredService<IOptions<ExceptionDescriptorOptions>>().Value;

        Assert.Equal(FaultSensitivityDetails.All, exceptionDescriptorOptions.SensitivityDetails);
        Assert.Collection(services,
            sp => Assert.True(sp.ServiceType == typeof(IOptions<>), "sp.ServiceType == typeof(IOptions<>)"),
            sp => Assert.True(sp.ServiceType == typeof(IOptionsSnapshot<>), "sp.ServiceType == typeof(IOptionsSnapshot<>)"),
            sp => Assert.True(sp.ServiceType == typeof(IOptionsMonitor<>), "sp.ServiceType == typeof(IOptionsMonitor<>)"),
            sp => Assert.True(sp.ServiceType == typeof(IOptionsFactory<>), "sp.ServiceType == typeof(IOptionsFactory<>)"),
            sp => Assert.True(sp.ServiceType == typeof(IOptionsMonitorCache<>), "sp.ServiceType == typeof(IOptionsMonitorCache<>)"),
            sp => Assert.True(sp.ServiceType == typeof(IConfigureOptions<ExceptionDescriptorOptions>), "sp.ServiceType == typeof(IConfigureOptions<ExceptionDescriptorOptions>)"));
    }

    [Fact]
    public void TryConfigure_ShouldAddConfigureOptions_OnlyOnce()
    {
        var services = new ServiceCollection()
            .TryConfigure<ExceptionDescriptorOptions>(o => o.SensitivityDetails = FaultSensitivityDetails.All)
            .TryConfigure<ExceptionDescriptorOptions>(o => o.SensitivityDetails = FaultSensitivityDetails.None);

        var serviceProvider = services.BuildServiceProvider();

        var exceptionDescriptorOptions = serviceProvider.GetRequiredService<IOptions<ExceptionDescriptorOptions>>().Value;

        Assert.Equal(FaultSensitivityDetails.All, exceptionDescriptorOptions.SensitivityDetails);
        Assert.Collection(services,
            sp => Assert.True(sp.ServiceType == typeof(IOptions<>), "sp.ServiceType == typeof(IOptions<>)"),
            sp => Assert.True(sp.ServiceType == typeof(IOptionsSnapshot<>), "sp.ServiceType == typeof(IOptionsSnapshot<>)"),
            sp => Assert.True(sp.ServiceType == typeof(IOptionsMonitor<>), "sp.ServiceType == typeof(IOptionsMonitor<>)"),
            sp => Assert.True(sp.ServiceType == typeof(IOptionsFactory<>), "sp.ServiceType == typeof(IOptionsFactory<>)"),
            sp => Assert.True(sp.ServiceType == typeof(IOptionsMonitorCache<>), "sp.ServiceType == typeof(IOptionsMonitorCache<>)"),
            sp => Assert.True(sp.ServiceType == typeof(IConfigureOptions<ExceptionDescriptorOptions>), "sp.ServiceType == typeof(IConfigureOptions<ExceptionDescriptorOptions>)"));
    }

    [Fact]
    public void Configure_ShouldAddConfigureOptions_Twice()
    {
        var services = new ServiceCollection()
            .Configure<ExceptionDescriptorOptions>(o => o.SensitivityDetails = FaultSensitivityDetails.All)
            .Configure<ExceptionDescriptorOptions>(o => o.SensitivityDetails = FaultSensitivityDetails.None);

        var serviceProvider = services.BuildServiceProvider();

        var exceptionDescriptorOptions = serviceProvider.GetRequiredService<IOptions<ExceptionDescriptorOptions>>().Value;

        Assert.Equal(FaultSensitivityDetails.None, exceptionDescriptorOptions.SensitivityDetails);
        Assert.Collection(services,
            sp => Assert.True(sp.ServiceType == typeof(IOptions<>), "sp.ServiceType == typeof(IOptions<>)"),
            sp => Assert.True(sp.ServiceType == typeof(IOptionsSnapshot<>), "sp.ServiceType == typeof(IOptionsSnapshot<>)"),
            sp => Assert.True(sp.ServiceType == typeof(IOptionsMonitor<>), "sp.ServiceType == typeof(IOptionsMonitor<>)"),
            sp => Assert.True(sp.ServiceType == typeof(IOptionsFactory<>), "sp.ServiceType == typeof(IOptionsFactory<>)"),
            sp => Assert.True(sp.ServiceType == typeof(IOptionsMonitorCache<>), "sp.ServiceType == typeof(IOptionsMonitorCache<>)"),
            sp => Assert.True(sp.ServiceType == typeof(IConfigureOptions<ExceptionDescriptorOptions>), "sp.ServiceType == typeof(IConfigureOptions<ExceptionDescriptorOptions>)"),
            sp => Assert.True(sp.ServiceType == typeof(IConfigureOptions<ExceptionDescriptorOptions>), "sp.ServiceType == typeof(IConfigureOptions<ExceptionDescriptorOptions>)"));
    }

    [Fact]
    public void SynchronizeOptions_ShouldNotChangeAnything_ValuesAreAsConfigured()
    {
        var invocationCount = 0;
        var services = new ServiceCollection()
            .Configure<JsonFormatterOptions>(o =>
            {
                o.Settings.DefaultBufferSize = 4096;
                o.SensitivityDetails = FaultSensitivityDetails.Failure;
            })
            .Configure<XmlFormatterOptions>(o =>
            {
                o.Settings.Writer.Async = true;
                o.SensitivityDetails = FaultSensitivityDetails.Evidence;
            })
            .Configure<FaultDescriptorOptions>(o =>
            {
                o.RootHelpLink = new Uri("about:blank");
                o.SensitivityDetails = FaultSensitivityDetails.FailureWithStackTrace;
            })
            .Configure<MvcFaultDescriptorOptions>(o =>
            {
                o.MarkExceptionHandled = true;
                o.SensitivityDetails = FaultSensitivityDetails.FailureWithStackTraceAndData;
            })
            .Configure<ExceptionDescriptorOptions>(o => o.SensitivityDetails = FaultSensitivityDetails.None);

        services.PostConfigureAllOf<FakeOptions>(_ => { invocationCount++; });

        var serviceProvider = services.BuildServiceProvider();

        var jsonFormatterOptions = serviceProvider.GetRequiredService<IOptions<JsonFormatterOptions>>().Value;
        var xmlFormatterOptions = serviceProvider.GetRequiredService<IOptions<XmlFormatterOptions>>().Value;
        var faultDescriptorOptions = serviceProvider.GetRequiredService<IOptions<FaultDescriptorOptions>>().Value;
        var mvcFaultDescriptorOptions = serviceProvider.GetRequiredService<IOptions<MvcFaultDescriptorOptions>>().Value;
        var exceptionDescriptorOptions = serviceProvider.GetRequiredService<IOptions<ExceptionDescriptorOptions>>().Value;

        Assert.Equal(FaultSensitivityDetails.Failure, jsonFormatterOptions.SensitivityDetails);
        Assert.Equal(4096, jsonFormatterOptions.Settings.DefaultBufferSize);

        Assert.Equal(FaultSensitivityDetails.Evidence, xmlFormatterOptions.SensitivityDetails);
        Assert.True(xmlFormatterOptions.Settings.Writer.Async);

        Assert.Equal(FaultSensitivityDetails.FailureWithStackTrace, faultDescriptorOptions.SensitivityDetails);
        Assert.Equal(new Uri("about:blank"), faultDescriptorOptions.RootHelpLink);

        Assert.Equal(FaultSensitivityDetails.FailureWithStackTraceAndData, mvcFaultDescriptorOptions.SensitivityDetails);
        Assert.True(mvcFaultDescriptorOptions.MarkExceptionHandled);

        Assert.Equal(FaultSensitivityDetails.None, exceptionDescriptorOptions.SensitivityDetails);

        Assert.Equal(0, invocationCount);
    }

    [Fact]
    public void SynchronizeOptions_ShouldChangeAllWithAServiceTypeHavingIExceptionDescriptorOptions_RemainingValuesAreAsConfigured()
    {
        var invocationCount = 0;
        var services = new ServiceCollection()
            .Configure<JsonFormatterOptions>(o =>
            {
                o.Settings.DefaultBufferSize = 4096;
                o.SensitivityDetails = FaultSensitivityDetails.Failure;
            })
            .Configure<XmlFormatterOptions>(o =>
            {
                o.Settings.Writer.Async = true;
                o.SensitivityDetails = FaultSensitivityDetails.Evidence;
            })
            .Configure<FaultDescriptorOptions>(o =>
            {
                o.RootHelpLink = new Uri("about:blank");
                o.SensitivityDetails = FaultSensitivityDetails.FailureWithStackTrace;
            })
            .Configure<MvcFaultDescriptorOptions>(o =>
            {
                o.MarkExceptionHandled = true;
                o.SensitivityDetails = FaultSensitivityDetails.FailureWithStackTraceAndData;
            })
            .Configure<ExceptionDescriptorOptions>(o => o.SensitivityDetails = FaultSensitivityDetails.None);

        services.PostConfigureAllOf<IExceptionDescriptorOptions>(o =>
        {
            invocationCount++;
            o.SensitivityDetails = FaultSensitivityDetails.None;
        });

        var serviceProvider = services.BuildServiceProvider();

        var jsonFormatterOptions = serviceProvider.GetRequiredService<IOptions<JsonFormatterOptions>>().Value;
        var xmlFormatterOptions = serviceProvider.GetRequiredService<IOptions<XmlFormatterOptions>>().Value;
        var faultDescriptorOptions = serviceProvider.GetRequiredService<IOptions<FaultDescriptorOptions>>().Value;
        var mvcFaultDescriptorOptions = serviceProvider.GetRequiredService<IOptions<MvcFaultDescriptorOptions>>().Value;
        var exceptionDescriptorOptions = serviceProvider.GetRequiredService<IOptions<ExceptionDescriptorOptions>>().Value;

        Assert.Equal(FaultSensitivityDetails.None, jsonFormatterOptions.SensitivityDetails);
        Assert.Equal(4096, jsonFormatterOptions.Settings.DefaultBufferSize);

        Assert.Equal(FaultSensitivityDetails.None, xmlFormatterOptions.SensitivityDetails);
        Assert.True(xmlFormatterOptions.Settings.Writer.Async);

        Assert.Equal(FaultSensitivityDetails.None, faultDescriptorOptions.SensitivityDetails);
        Assert.Equal(new Uri("about:blank"), faultDescriptorOptions.RootHelpLink);

        Assert.Equal(FaultSensitivityDetails.None, mvcFaultDescriptorOptions.SensitivityDetails);
        Assert.True(mvcFaultDescriptorOptions.MarkExceptionHandled);

        Assert.Equal(FaultSensitivityDetails.None, exceptionDescriptorOptions.SensitivityDetails);

        Assert.Equal(5, invocationCount);
    }

    [Fact]
    public void SynchronizeOptions_ShouldChangeAllWithAServiceTypeHavingFaultDescriptorOptions_RemainingValuesAreAsConfigured()
    {
        var invocationCount = 0;
        var services = new ServiceCollection()
            .Configure<JsonFormatterOptions>(o =>
            {
                o.Settings.DefaultBufferSize = 4096;
                o.SensitivityDetails = FaultSensitivityDetails.Failure;
            })
            .Configure<XmlFormatterOptions>(o =>
            {
                o.Settings.Writer.Async = true;
                o.SensitivityDetails = FaultSensitivityDetails.Evidence;
            })
            .Configure<FaultDescriptorOptions>(o =>
            {
                o.RootHelpLink = new Uri("about:blank");
                o.SensitivityDetails = FaultSensitivityDetails.FailureWithStackTrace;
            })
            .Configure<MvcFaultDescriptorOptions>(o =>
            {
                o.MarkExceptionHandled = true;
                o.SensitivityDetails = FaultSensitivityDetails.FailureWithStackTraceAndData;
            })
            .Configure<ExceptionDescriptorOptions>(o => o.SensitivityDetails = FaultSensitivityDetails.None);

        services.PostConfigureAllOf<FaultDescriptorOptions>(o =>
        {
            invocationCount++;
            o.RootHelpLink = new Uri("about:not-so-blank");
            o.SensitivityDetails = FaultSensitivityDetails.Data;
        });

        var serviceProvider = services.BuildServiceProvider();

        var jsonFormatterOptions = serviceProvider.GetRequiredService<IOptions<JsonFormatterOptions>>().Value;
        var xmlFormatterOptions = serviceProvider.GetRequiredService<IOptions<XmlFormatterOptions>>().Value;
        var faultDescriptorOptions = serviceProvider.GetRequiredService<IOptions<FaultDescriptorOptions>>().Value;
        var mvcFaultDescriptorOptions = serviceProvider.GetRequiredService<IOptions<MvcFaultDescriptorOptions>>().Value;
        var exceptionDescriptorOptions = serviceProvider.GetRequiredService<IOptions<ExceptionDescriptorOptions>>().Value;

        Assert.Equal(FaultSensitivityDetails.Failure, jsonFormatterOptions.SensitivityDetails);
        Assert.Equal(4096, jsonFormatterOptions.Settings.DefaultBufferSize);

        Assert.Equal(FaultSensitivityDetails.Evidence, xmlFormatterOptions.SensitivityDetails);
        Assert.True(xmlFormatterOptions.Settings.Writer.Async);

        Assert.Equal(FaultSensitivityDetails.Data, faultDescriptorOptions.SensitivityDetails);
        Assert.Equal(new Uri("about:not-so-blank"), faultDescriptorOptions.RootHelpLink);

        Assert.Equal(FaultSensitivityDetails.Data, mvcFaultDescriptorOptions.SensitivityDetails);
        Assert.Equal(new Uri("about:not-so-blank"), mvcFaultDescriptorOptions.RootHelpLink);
        Assert.True(mvcFaultDescriptorOptions.MarkExceptionHandled);

        Assert.Equal(FaultSensitivityDetails.None, exceptionDescriptorOptions.SensitivityDetails);

        Assert.Equal(2, invocationCount);
    }

#endif

    [Fact]
    public void Add_ShouldOnlyRegisterOptionsOnce_WhenCalledMultipleTimesWithSameOptionsType()
    {
        var sut = new ServiceCollection();

        sut.Add<FakeOptions>(typeof(FakeService), typeof(FakeServiceScoped), ServiceLifetime.Scoped, (Action<FakeOptions>)(o => o.Greeting = "First"));
        sut.Add<FakeOptions>(typeof(FakeService), typeof(FakeServiceSingleton), ServiceLifetime.Singleton, (Action<FakeOptions>)(o => o.Greeting = "Second"));
        sut.Add<FakeOptions>(typeof(FakeService), typeof(FakeServiceTransient), ServiceLifetime.Transient, (Action<FakeOptions>)(o => o.Greeting = "Third"));

        var configureOptionsCount = sut.Count(sd =>
            sd.ServiceType == typeof(IConfigureOptions<FakeOptions>));

        TestOutput.WriteLine($"IConfigureOptions<FakeOptions> registrations: {configureOptionsCount}");

        Assert.Equal(1, configureOptionsCount);
    }

    [Fact]
    public void TryAdd_ShouldOnlyRegisterOptionsOnce_WhenCalledMultipleTimesWithSameOptionsType()
    {
        var sut = new ServiceCollection();

        sut.TryAdd<FakeOptions>(typeof(FakeService), typeof(FakeServiceScoped), ServiceLifetime.Scoped, (Action<FakeOptions>)(o => o.Greeting = "First"));
        sut.TryAdd<FakeOptions>(typeof(FakeService), typeof(FakeServiceSingleton), ServiceLifetime.Singleton, (Action<FakeOptions>)(o => o.Greeting = "Second"));
        sut.TryAdd<FakeOptions>(typeof(FakeService), typeof(FakeServiceTransient), ServiceLifetime.Transient, (Action<FakeOptions>)(o => o.Greeting = "Third"));

        var configureOptionsCount = sut.Count(sd =>
            sd.ServiceType == typeof(IConfigureOptions<FakeOptions>));

        TestOutput.WriteLine($"IConfigureOptions<FakeOptions> registrations: {configureOptionsCount}");

        Assert.Equal(1, configureOptionsCount);
    }

    [Fact]
    public void TryAdd_WithFactory_ShouldOnlyRegisterOptionsOnce_WhenCalledMultipleTimesWithSameOptionsType()
    {
        var sut = new ServiceCollection();

        sut.TryAdd<FakeOptions>(typeof(FakeService), _ => new FakeServiceScoped(default), ServiceLifetime.Scoped, (Action<FakeOptions>)(o => o.Greeting = "First"));
        sut.TryAdd<FakeOptions>(typeof(FakeService), _ => new FakeServiceSingleton(default), ServiceLifetime.Singleton, (Action<FakeOptions>)(o => o.Greeting = "Second"));
        sut.TryAdd<FakeOptions>(typeof(FakeService), _ => new FakeServiceTransient(default), ServiceLifetime.Transient, (Action<FakeOptions>)(o => o.Greeting = "Third"));

        var configureOptionsCount = sut.Count(sd =>
            sd.ServiceType == typeof(IConfigureOptions<FakeOptions>));

        TestOutput.WriteLine($"IConfigureOptions<FakeOptions> registrations: {configureOptionsCount}");

        Assert.Equal(1, configureOptionsCount);
    }

}
