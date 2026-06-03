using System;
using System.Linq;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Http.Throttling;
using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Http.Throttling
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddThrottlingCache_ShouldThrowArgumentNullException_WhenServicesIsNull()
        {
            Assert.Throws<ArgumentNullException>("services", () => ServiceCollectionExtensions.AddThrottlingCache<MemoryThrottlingCache>(null));
        }

        [Fact]
        public void AddMemoryThrottlingCache_ShouldRegisterIThrottlingCacheAsSingleton()
        {
            var sut = new ServiceCollection();

            sut.AddMemoryThrottlingCache();

            var descriptor = sut.FirstOrDefault(sd => sd.ServiceType == typeof(IThrottlingCache));

            Assert.NotNull(descriptor);
            Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
        }

        [Fact]
        public void AddThrottlingCache_ShouldRegisterIThrottlingCacheAsSingleton()
        {
            var sut = new ServiceCollection();

            sut.AddThrottlingCache<MemoryThrottlingCache>();

            var descriptor = sut.FirstOrDefault(sd => sd.ServiceType == typeof(IThrottlingCache));

            Assert.NotNull(descriptor);
            Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
        }

        [Fact]
        public void AddThrottlingSentinelOptions_ShouldThrowArgumentNullException_WhenServicesIsNull()
        {
            Assert.Throws<ArgumentNullException>("services", () => ServiceCollectionExtensions.AddThrottlingSentinelOptions(null));
        }

        [Fact]
        public void AddThrottlingSentinelOptions_ShouldRegisterThrottlingSentinelOptions_WithDefaultValues()
        {
            var sut = new ServiceCollection();

            sut.AddThrottlingSentinelOptions();

            var count = sut.Count(sd => sd.ServiceType == typeof(IConfigureOptions<ThrottlingSentinelOptions>));

            TestOutput.WriteLine($"IConfigureOptions<ThrottlingSentinelOptions> registrations: {count}");

            Assert.True(count >= 1);
        }

        [Fact]
        public void AddThrottlingSentinelOptions_ShouldRegisterThrottlingSentinelOptions_WithCustomValues()
        {
            var sut = new ServiceCollection();

            sut.AddThrottlingSentinelOptions(o =>
            {
                o.TooManyRequestsMessage = "Slow down!";
                o.UseRetryAfterHeader = true;
            });

            var count = sut.Count(sd => sd.ServiceType == typeof(IConfigureOptions<ThrottlingSentinelOptions>));

            Assert.True(count >= 1);
        }

        [Fact]
        public void AddMemoryThrottlingCache_ShouldResolveMemoryThrottlingCache()
        {
            var services = new ServiceCollection();

            services.AddMemoryThrottlingCache();

            var cache = services.BuildServiceProvider().GetRequiredService<IThrottlingCache>();

            Assert.IsType<MemoryThrottlingCache>(cache);
        }

        [Fact]
        public void AddThrottlingCache_ShouldResolveRegisteredCache()
        {
            var services = new ServiceCollection();

            services.AddThrottlingCache<MemoryThrottlingCache>();

            var cache = services.BuildServiceProvider().GetRequiredService<IThrottlingCache>();

            Assert.IsType<MemoryThrottlingCache>(cache);
        }

        [Fact]
        public void AddThrottlingSentinelOptions_ShouldResolveDefaultOptions()
        {
            var services = new ServiceCollection();

            services.AddOptions();
            services.AddThrottlingSentinelOptions();

            var options = services.BuildServiceProvider().GetRequiredService<IOptions<ThrottlingSentinelOptions>>().Value;

            Assert.Equal("RateLimit-Limit", options.RateLimitHeaderName);
            Assert.Equal("RateLimit-Remaining", options.RateLimitRemainingHeaderName);
            Assert.Equal("RateLimit-Reset", options.RateLimitResetHeaderName);
            Assert.Equal(RetryConditionScope.DeltaSeconds, options.RateLimitResetScope);
            Assert.Equal(RetryConditionScope.DeltaSeconds, options.RetryAfterScope);
            Assert.True(options.UseRetryAfterHeader);
            Assert.Equal("Throttling rate limit quota violation. Quota limit exceeded.", options.TooManyRequestsMessage);
            Assert.NotNull(options.ResponseHandler);
        }

        [Fact]
        public void AddThrottlingSentinelOptions_ShouldResolveConfiguredOptions()
        {
            var services = new ServiceCollection();
            Func<Microsoft.AspNetCore.Http.HttpContext, string> contextResolver = _ => "global";
            var quota = new ThrottleQuota(1, TimeSpan.FromMinutes(1));

            services.AddOptions();
            services.AddThrottlingSentinelOptions(o =>
            {
                o.ContextResolver = contextResolver;
                o.Quota = quota;
                o.RateLimitHeaderName = "X-Limit";
                o.RateLimitRemainingHeaderName = "X-Remaining";
                o.RateLimitResetHeaderName = "X-Reset";
                o.RateLimitResetScope = RetryConditionScope.HttpDate;
                o.RetryAfterScope = RetryConditionScope.HttpDate;
                o.TooManyRequestsMessage = "Slow down!";
                o.UseRetryAfterHeader = false;
            });

            var options = services.BuildServiceProvider().GetRequiredService<IOptions<ThrottlingSentinelOptions>>().Value;

            Assert.Same(contextResolver, options.ContextResolver);
            Assert.Same(quota, options.Quota);
            Assert.Equal("X-Limit", options.RateLimitHeaderName);
            Assert.Equal("X-Remaining", options.RateLimitRemainingHeaderName);
            Assert.Equal("X-Reset", options.RateLimitResetHeaderName);
            Assert.Equal(RetryConditionScope.HttpDate, options.RateLimitResetScope);
            Assert.Equal(RetryConditionScope.HttpDate, options.RetryAfterScope);
            Assert.Equal("Slow down!", options.TooManyRequestsMessage);
            Assert.False(options.UseRetryAfterHeader);
            Assert.NotNull(options.ResponseHandler);
        }
    }
}
