using System;
using System.Linq;
#if NET9_0_OR_GREATER
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

    [Theory]
    [InlineData(ServiceLifetime.Singleton)]
    [InlineData(ServiceLifetime.Scoped)]
    [InlineData(ServiceLifetime.Transient)]
    public void Registration_ShouldResolveWithSpecifiedLifetime_UsingTypesAndFactories(ServiceLifetime lifetime)
    {
        Action<IServiceCollection>[] registrations =
        {
            s => s.Add<IService, DefaultService>(lifetime),
            s => s.TryAdd<IService, DefaultService>(lifetime),
            s => s.Add<IService, DefaultService>(_ => new DefaultService(), lifetime),
            s => s.TryAdd<IService, DefaultService>(_ => new DefaultService(), lifetime)
        };
        foreach (var register in registrations)
        {
            var services = new ServiceCollection();
            register(services);
            Assert.Equal(lifetime, Assert.Single(services).Lifetime);
            using (var provider = services.BuildServiceProvider())
            using (var firstScope = provider.CreateScope())
            using (var secondScope = provider.CreateScope())
            {
                var first = firstScope.ServiceProvider.GetRequiredService<IService>();
                var repeated = firstScope.ServiceProvider.GetRequiredService<IService>();
                var other = secondScope.ServiceProvider.GetRequiredService<IService>();
                Assert.IsType<DefaultService>(first);
                if (lifetime == ServiceLifetime.Transient) { Assert.NotSame(first, repeated); }
                else { Assert.Same(first, repeated); }
                if (lifetime == ServiceLifetime.Singleton) { Assert.Same(first, other); }
                else { Assert.NotSame(first, other); }
            }
        }
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void Forwarding_ShouldResolveSelectedInterfacesToSameInstance(bool tryAdd, bool factory)
    {
        var services = new ServiceCollection();
        Action<TypeForwardServiceOptions> setup = o => o.Lifetime = ServiceLifetime.Singleton;
        IServiceCollection result;
        if (tryAdd)
        {
            result = factory ? services.TryAdd<Foo>(_ => new Foo(), setup) : services.TryAdd<Foo>(setup);
        }
        else
        {
            result = factory ? services.Add<Foo>(_ => new Foo(), setup) : services.Add<Foo>(setup);
        }
        Assert.Same(services, result);
        Assert.Equal(3, services.Count);
        using (var provider = services.BuildServiceProvider())
        {
            Assert.Same(provider.GetRequiredService<Foo>(), provider.GetRequiredService<IFoo>());
            Assert.Same(provider.GetRequiredService<Foo>(), provider.GetRequiredService<IBar>());
        }
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void Forwarding_ShouldChooseMarkerInterface_WhenGenericAndNonGenericInterfacesShareName(bool tryAdd, bool factory)
    {
        var services = new ServiceCollection();
        Action<TypeForwardServiceOptions> setup = o => o.Lifetime = ServiceLifetime.Singleton;
        if (tryAdd)
        {
            if (factory) { services.TryAdd<DefaultService<Foo>>(_ => new DefaultService<Foo>(), setup); }
            else { services.TryAdd<DefaultService<Foo>>(setup); }
        }
        else
        {
            if (factory) { services.Add<DefaultService<Foo>>(_ => new DefaultService<Foo>(), setup); }
            else { services.Add<DefaultService<Foo>>(setup); }
        }
        using (var provider = services.BuildServiceProvider())
        {
            Assert.Same(provider.GetRequiredService<DefaultService<Foo>>(), provider.GetRequiredService<IService<Foo>>());
            Assert.Null(provider.GetService<IService>());
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void FactoryForwarding_ShouldHonorDisabledForwardingAndPredicate(bool tryAdd)
    {
        foreach (var disabled in new[] { false, true })
        {
            var services = new ServiceCollection();
            Action<TypeForwardServiceOptions> setup = o =>
            {
                o.UseNestedTypeForwarding = !disabled;
                o.NestedTypeSelector = _ => new[] { typeof(IFoo), typeof(IBar) };
                o.NestedTypePredicate = t => t == typeof(IBar);
                o.Lifetime = ServiceLifetime.Singleton;
            };
            var result = tryAdd ? services.TryAdd<Foo>(_ => new Foo(), setup) : services.Add<Foo>(_ => new Foo(), setup);
            Assert.Same(services, result);
            using (var provider = services.BuildServiceProvider())
            {
                Assert.Null(provider.GetService<IFoo>());
                if (disabled) { Assert.Null(provider.GetService<IBar>()); }
                else { Assert.Same(provider.GetRequiredService<Foo>(), provider.GetRequiredService<IBar>()); }
            }
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Registration_ShouldPreserveDuplicatesOnlyForAdd(bool factory)
    {
        var services = new ServiceCollection();
        var original = new DefaultService();
        services.AddSingleton<IService>(original);
        var descriptor = services.Single();
        var result = factory
            ? services.TryAdd<IService, DefaultService>(_ => new DefaultService(), ServiceLifetime.Transient)
            : services.TryAdd<IService, DefaultService>(ServiceLifetime.Transient);
        Assert.Same(services, result);
        Assert.Same(descriptor, Assert.Single(services));
        if (factory) { services.Add<IService, DefaultService>(_ => new DefaultService(), ServiceLifetime.Transient); }
        else { services.Add<IService, DefaultService>(ServiceLifetime.Transient); }
        using (var provider = services.BuildServiceProvider())
        {
            var resolved = provider.GetServices<IService>().ToArray();
            Assert.Equal(2, resolved.Length);
            Assert.Same(original, resolved[0]);
            Assert.NotSame(original, resolved[1]);
        }
    }

    [Fact]
    public void Registration_ShouldLeaveCollectionEmpty_WhenLifetimeIsUnknown()
    {
        var services = new ServiceCollection();
        var lifetime = (ServiceLifetime)int.MaxValue;
        Assert.Same(services, services.Add<IService, DefaultService>(lifetime));
        Assert.Same(services, services.TryAdd<IService, DefaultService>(lifetime));
        Assert.Same(services, services.Add<IService, DefaultService>(_ => new DefaultService(), lifetime));
        Assert.Same(services, services.TryAdd<IService, DefaultService>(_ => new DefaultService(), lifetime));
        Assert.Empty(services);
    }

    [Fact]
    public void TryConfigure_ShouldReturnNull_WhenServicesAreNull()
    {
        Assert.Null(ServiceCollectionExtensions.TryConfigure<FakeOptions>(null, o => o.Greeting = "Hello"));
    }

    [Fact]
    public void TryConfigure_ShouldIgnoreNullSetup_WhenOptionsAlreadyRegistered()
    {
        var services = new ServiceCollection();
        services.Configure<FakeOptions>(o => o.Greeting = "First");
        Assert.Same(services, services.TryConfigure<FakeOptions>(null));
        using (var provider = services.BuildServiceProvider())
        {
            Assert.Equal("First", provider.GetRequiredService<IOptions<FakeOptions>>().Value.Greeting);
        }
    }

    [Theory]
    [InlineData(false, false, "First")]
    [InlineData(false, true, "Second")]
    [InlineData(true, false, "First")]
    [InlineData(true, true, "First")]
    public void RegistrationWithOptions_ShouldHonorExistingConfiguration(bool tryAdd, bool factory, string expected)
    {
        var services = new ServiceCollection();
        services.Configure<FakeOptions>(o => o.Greeting = "First");
        IServiceCollection result;
        if (tryAdd)
        {
            result = factory
                ? services.TryAdd<IService, DefaultService, FakeOptions>(_ => new DefaultService(), ServiceLifetime.Singleton, o => o.Greeting = "Second")
                : services.TryAdd<IService, DefaultService, FakeOptions>(ServiceLifetime.Singleton, o => o.Greeting = "Second");
        }
        else
        {
            result = factory
                ? services.Add<IService, DefaultService, FakeOptions>(_ => new DefaultService(), ServiceLifetime.Singleton, o => o.Greeting = "Second")
                : services.Add<IService, DefaultService, FakeOptions>(ServiceLifetime.Singleton, o => o.Greeting = "Second");
        }
        Assert.Same(services, result);
        using (var provider = services.BuildServiceProvider())
        {
            Assert.IsType<DefaultService>(provider.GetRequiredService<IService>());
            Assert.Equal(expected, provider.GetRequiredService<IOptions<FakeOptions>>().Value.Greeting);
        }
    }

    [Fact]
    public void Registration_ShouldRejectNullServices_ForEveryOverload()
    {
        Func<IServiceCollection, IServiceCollection>[] registrations =
        {
            s => s.Add<IService, DefaultService>(ServiceLifetime.Singleton),
            s => s.Add<IService, DefaultService, FakeOptions>(ServiceLifetime.Singleton, _ => { }),
            s => s.Add(typeof(IService), typeof(DefaultService), ServiceLifetime.Singleton),
            s => s.Add<FakeOptions>(typeof(IService), typeof(DefaultService), ServiceLifetime.Singleton, _ => { }),
            s => s.Add<IService, DefaultService>(_ => new DefaultService(), ServiceLifetime.Singleton),
            s => s.Add<IService, DefaultService, FakeOptions>(_ => new DefaultService(), ServiceLifetime.Singleton, _ => { }),
            s => s.Add(typeof(IService), _ => new DefaultService(), ServiceLifetime.Singleton),
            s => s.Add<FakeOptions>(typeof(IService), _ => new DefaultService(), ServiceLifetime.Singleton, _ => { }),
            s => s.Add<Foo>(),
            s => s.Add<IService, DefaultService>(),
            s => s.Add(typeof(IService), typeof(DefaultService)),
            s => s.Add<Foo>(_ => new Foo()),
            s => s.Add<IService, DefaultService>(_ => new DefaultService()),
            s => s.Add(typeof(IService), _ => new DefaultService()),
            s => s.TryAdd<Foo>(),
            s => s.TryAdd<IService, DefaultService>(),
            s => s.TryAdd(typeof(IService), typeof(DefaultService)),
            s => s.TryAdd<Foo>(_ => new Foo()),
            s => s.TryAdd<IService, DefaultService>(_ => new DefaultService()),
            s => s.TryAdd(typeof(IService), _ => new DefaultService()),
            s => s.TryAdd<IService, DefaultService>(ServiceLifetime.Singleton),
            s => s.TryAdd<IService, DefaultService, FakeOptions>(ServiceLifetime.Singleton, _ => { }),
            s => s.TryAdd(typeof(IService), typeof(DefaultService), ServiceLifetime.Singleton),
            s => s.TryAdd<FakeOptions>(typeof(IService), typeof(DefaultService), ServiceLifetime.Singleton, _ => { }),
            s => s.TryAdd<IService, DefaultService>(_ => new DefaultService(), ServiceLifetime.Singleton),
            s => s.TryAdd<IService, DefaultService, FakeOptions>(_ => new DefaultService(), ServiceLifetime.Singleton, _ => { }),
            s => s.TryAdd(typeof(IService), _ => new DefaultService(), ServiceLifetime.Singleton),
            s => s.TryAdd<FakeOptions>(typeof(IService), _ => new DefaultService(), ServiceLifetime.Singleton, _ => { })
        };
        foreach (var register in registrations)
        {
            Assert.Equal("services", Assert.Throws<ArgumentNullException>(() => register(null)).ParamName);
        }
    }

    [Fact]
    public void Registration_ShouldRejectNullSetup_BeforeAddingService()
    {
        var services = new ServiceCollection();
        Func<IServiceCollection>[] registrations =
        {
            () => services.Add<IService, DefaultService, FakeOptions>(ServiceLifetime.Singleton, null),
            () => services.Add<IService, DefaultService, FakeOptions>(_ => new DefaultService(), ServiceLifetime.Singleton, null),
            () => services.TryAdd<IService, DefaultService, FakeOptions>(ServiceLifetime.Singleton, null),
            () => services.TryAdd<IService, DefaultService, FakeOptions>(_ => new DefaultService(), ServiceLifetime.Singleton, null)
        };
        foreach (var register in registrations)
        {
            Assert.Equal("setup", Assert.Throws<ArgumentNullException>(() => register()).ParamName);
            Assert.Empty(services);
        }
        Assert.Equal("configureOptions", Assert.Throws<ArgumentNullException>(() => services.TryConfigure<FakeOptions>(null)).ParamName);
        Assert.Empty(services);
    }

    [Fact]
    public void Forwarding_ShouldRegisterOnlyService_WhenNoNestedTypesMatch()
    {
        var services = new ServiceCollection();
        Assert.Same(services, services.Add<Foo>(o => o.NestedTypePredicate = _ => false));
        Assert.Equal(typeof(Foo), Assert.Single(services).ServiceType);
        using (var provider = services.BuildServiceProvider())
        {
            Assert.NotNull(provider.GetRequiredService<Foo>());
            Assert.Null(provider.GetService<IFoo>());
            Assert.Null(provider.GetService<IBar>());
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TryAddWithForwarding_ShouldPreserveExistingServiceAndInterface(bool factory)
    {
        var services = new ServiceCollection();
        var originalService = new Foo();
        var originalInterface = new Foo();
        services.AddSingleton(originalService);
        services.AddSingleton<IFoo>(originalInterface);
        var result = factory ? services.TryAdd<Foo>(_ => new Foo()) : services.TryAdd<Foo>();
        Assert.Same(services, result);
        Assert.Equal(3, services.Count);
        using (var provider = services.BuildServiceProvider())
        {
            Assert.Same(originalService, provider.GetRequiredService<Foo>());
            Assert.Same(originalInterface, provider.GetRequiredService<IFoo>());
            Assert.Same(originalService, provider.GetRequiredService<IBar>());
        }
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void Forwarding_ShouldLeavePrimaryRegistration_WhenSelectorOrPredicateIsNull(bool tryAdd, bool factory)
    {
        foreach (var nullSelector in new[] { false, true })
        {
            var services = new ServiceCollection();
            Action<TypeForwardServiceOptions> setup = o =>
            {
                if (nullSelector) { o.NestedTypeSelector = null; }
                else { o.NestedTypePredicate = null; }
            };
            // Characterize the current partial registration so the refactor can change it deliberately.
            Assert.Throws<NullReferenceException>(() =>
            {
                if (tryAdd)
                {
                    if (factory) { services.TryAdd<Foo>(_ => new Foo(), setup); }
                    else { services.TryAdd<Foo>(setup); }
                }
                else
                {
                    if (factory) { services.Add<Foo>(_ => new Foo(), setup); }
                    else { services.Add<Foo>(setup); }
                }
            });
            Assert.Equal(typeof(Foo), Assert.Single(services).ServiceType);
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Forwarding_ShouldChooseNonGenericInterface_WhenServiceHasNoMarker(bool tryAdd)
    {
        var services = new ServiceCollection();
        Action<TypeForwardServiceOptions> setup = o =>
        {
            o.Lifetime = ServiceLifetime.Singleton;
            o.NestedTypeSelector = _ => new[] { typeof(IService<Foo>), typeof(IService) };
        };
        if (tryAdd) { services.TryAdd<object, DefaultService<Foo>>(setup); }
        else { services.Add<object, DefaultService<Foo>>(setup); }
        using (var provider = services.BuildServiceProvider())
        {
            Assert.Same(provider.GetRequiredService<object>(), provider.GetRequiredService<IService>());
            Assert.Null(provider.GetService<IService<Foo>>());
        }
    }

    [Fact]
    public void PostConfigureAllOf_ShouldConfigureOnlyOptionsImplementingInterface()
    {
        var services = new ServiceCollection();
        services.Configure<InterfaceOptions>(o => o.Greeting = "Configured");
        services.Configure<FakeOptions>(o => o.Greeting = "Unrelated");
        services.PostConfigureAllOf<IOptionGreeting>(o => o.Greeting += " then post-configured");
        using (var provider = services.BuildServiceProvider())
        {
            Assert.Equal("Configured then post-configured", provider.GetRequiredService<IOptions<InterfaceOptions>>().Value.Greeting);
            Assert.Equal("Unrelated", provider.GetRequiredService<IOptions<FakeOptions>>().Value.Greeting);
        }
    }

    [Fact]
    public void Registration_ShouldRejectNullServiceAndImplementationTypes()
    {
        var services = new ServiceCollection();
        Assert.Equal("service", Assert.Throws<ArgumentNullException>(() => services.Add(null, typeof(DefaultService))).ParamName);
        Assert.Equal("implementation", Assert.Throws<ArgumentNullException>(() => services.Add(typeof(IService), (Type)null)).ParamName);
        Assert.Equal("service", Assert.Throws<ArgumentNullException>(() => services.TryAdd(null, typeof(DefaultService))).ParamName);
        Assert.Equal("implementation", Assert.Throws<ArgumentNullException>(() => services.TryAdd(typeof(IService), (Type)null)).ParamName);
        Assert.Empty(services);
    }

    [Fact]
    public void TryAdd_ShouldRejectNullFactory_BeforeAddingService()
    {
        var services = new ServiceCollection();
        Func<IServiceCollection>[] registrations =
        {
            () => services.Add<Foo>((Func<IServiceProvider, Foo>)null),
            () => services.TryAdd<Foo>((Func<IServiceProvider, Foo>)null),
            () => services.TryAdd<IService, DefaultService>((Func<IServiceProvider, DefaultService>)null, ServiceLifetime.Singleton),
            () => services.TryAdd<IService, DefaultService, FakeOptions>((Func<IServiceProvider, DefaultService>)null, ServiceLifetime.Singleton, _ => { }),
            () => services.TryAdd(typeof(IService), (Func<IServiceProvider, object>)null, ServiceLifetime.Singleton),
            () => services.TryAdd<FakeOptions>(typeof(IService), (Func<IServiceProvider, object>)null, ServiceLifetime.Singleton, _ => { })
        };
        foreach (var register in registrations)
        {
            Assert.Equal("implementationFactory", Assert.Throws<ArgumentNullException>(() => register()).ParamName);
            Assert.Empty(services);
        }
    }

    [Fact]
    public void PostConfigureAllOf_ShouldPreserveNamedOptionsAndRunAfterConfigure()
    {
        var services = new ServiceCollection();
        services.Configure<FakeServiceScopedOptions>("named", o => o.Greeting = "Configured");
        services.Configure<FakeOptions>(o => o.Greeting = "Default");
        Assert.Same(services, services.PostConfigureAllOf<FakeOptions>(o => o.Greeting += " then post-configured"));
        using (var provider = services.BuildServiceProvider())
        {
            var options = provider.GetRequiredService<IOptionsMonitor<FakeServiceScopedOptions>>();
            Assert.Equal("Configured then post-configured", options.Get("named").Greeting);
            Assert.Equal("", options.CurrentValue.Greeting);
            Assert.Equal("Default then post-configured", provider.GetRequiredService<IOptions<FakeOptions>>().Value.Greeting);
        }
    }

    [Fact]
    public void PostConfigureAllOf_ShouldSkipUnsupportedConfigureDescriptors()
    {
        var services = new ServiceCollection();
        services.AddOptions();
        services.AddSingleton<IConfigureOptions<FakeOptions>, NonGenericConfigureOptions>();
        services.AddSingleton<IConfigureOptions<FakeOptions>>(_ => new NonGenericConfigureOptions());
        services.AddSingleton<IConfigureOptions<FakeOptions>>(new NonGenericConfigureOptions());
        services.AddSingleton<IConfigureOptions<FakeOptions>>(new GenericConfigureOptions<FakeOptions>());
        Assert.Same(services, services.PostConfigureAllOf<FakeOptions>(o => o.Greeting = "Post"));
        Assert.DoesNotContain(services, d => d.ServiceType == typeof(IPostConfigureOptions<FakeOptions>));
        using (var provider = services.BuildServiceProvider())
        {
            Assert.Equal("Configured", provider.GetRequiredService<IOptions<FakeOptions>>().Value.Greeting);
        }
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

#if NET9_0_OR_GREATER

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


    private class NonGenericConfigureOptions : IConfigureOptions<FakeOptions>
    {
        public void Configure(FakeOptions options) { options.Greeting = "Configured"; }
    }

    public interface IOptionGreeting
    {
        string Greeting { get; set; }
    }

    public class InterfaceOptions : FakeOptions, IOptionGreeting
    {
    }

    private class GenericConfigureOptions<TOptions> : IConfigureOptions<TOptions> where TOptions : FakeOptions
    {
        public void Configure(TOptions options) { options.Greeting = "Configured"; }
    }
}
