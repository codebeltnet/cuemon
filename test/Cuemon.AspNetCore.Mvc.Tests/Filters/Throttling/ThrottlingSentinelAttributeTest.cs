using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.AspNetCore.Mvc.Assets;
using Cuemon.Extensions;
using Cuemon.Extensions.AspNetCore.Http.Throttling;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.AspNetCore.Mvc.Filters.Throttling;
public class ThrottlingSentinelAttributeTest : WebHostTest<ManagedWebHostFixture>
{
    private readonly IServiceProvider _provider;

    public ThrottlingSentinelAttributeTest(ManagedWebHostFixture hostFixture, ITestOutputHelper output) : base(hostFixture, output)
    {
        _provider = hostFixture.Host.Services;
    }

    [Fact]
    public async Task Bearer_ShouldThrottleWhenQuotaIsExceeded()
    {
        var cache = _provider.GetRequiredService<IThrottlingCache>();
        var client = Host.GetTestClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", nameof(Bearer_ShouldThrottleWhenQuotaIsExceeded));

        var te = await Assert.ThrowsAsync<ThrottlingException>(async () =>
        {
            for (var i = 0; i < 15; i++)
            {
                var result = await client.GetAsync("/fake");
                Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
                Assert.Equal("Unit Test", await result.Content.ReadAsStringAsync());
            }
        });


        var ce = cache[nameof(Bearer_ShouldThrottleWhenQuotaIsExceeded)];

        Assert.InRange(ce.Total, te.RateLimit, 15);
        Assert.Equal(ce.Quota.RateLimit, te.RateLimit);
        Assert.Equal(ce.Quota.Window, TimeSpan.FromSeconds(5));
        Assert.Equal(StatusCodes.Status429TooManyRequests, te.StatusCode);
    }

    [Fact]
    public async Task Bearer_ShouldThrottleAndThenRehydrateAfterWindowHasPassed()
    {
        var cache = _provider.GetRequiredService<IThrottlingCache>();
        var client = Host.GetTestClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", nameof(Bearer_ShouldThrottleAndThenRehydrateAfterWindowHasPassed));

        var te = await Assert.ThrowsAsync<ThrottlingException>(async () =>
        {
            for (var i = 0; i < 15; i++)
            {
                var result = await client.GetAsync("/fake");
                Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
                Assert.Equal("Unit Test", await result.Content.ReadAsStringAsync());
            }
        });


        var ce = cache[nameof(Bearer_ShouldThrottleAndThenRehydrateAfterWindowHasPassed)];

        Assert.InRange(ce.Total, te.RateLimit, 15);
        Assert.Equal(ce.Quota.RateLimit, te.RateLimit);
        Assert.Equal(ce.Quota.Window, TimeSpan.FromSeconds(5));
        Assert.Equal(StatusCodes.Status429TooManyRequests, te.StatusCode);

        await Task.Delay(TimeSpan.FromSeconds(10));

        for (var i = 0; i < 5; i++)
        {
            var result = await client.GetAsync("/fake");
            Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
            Assert.Equal("Unit Test", await result.Content.ReadAsStringAsync());
        }

        Assert.Equal(5, ce.Total);
    }

    [Fact]
    public async Task Bearer_VerifyHeadersAreSetCorrectly()
    {
        using (var filter = WebHostTestFactory.Create(services =>
        {
            services.AddControllers(o => { o.Filters.Add<ExceptionFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly);
            services.AddMemoryThrottlingCache();
        }, app =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", nameof(Bearer_VerifyHeadersAreSetCorrectly));

            HttpResponseMessage result = null;
            for (var i = 0; i < 10; i++)
            {
                result = await client.GetAsync("/fake");
                Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
                Assert.Equal("Unit Test", await result.Content.ReadAsStringAsync());
            }

            result = await client.GetAsync("/fake");

            var retryAfter = result.Headers.RetryAfter.Delta.Value.TotalSeconds;
            var ratelimitReset = result.Headers.GetValues("RateLimit-Reset").Single().As<int>();

            Assert.Equal(StatusCodes.Status429TooManyRequests, (int)result.StatusCode);
            Assert.Equal("Throttling rate limit quota violation. Quota limit exceeded.", await result.Content.ReadAsStringAsync());
            Assert.Contains("Retry-After", result.Headers.Select(pair => pair.Key));
            Assert.Contains("RateLimit-Limit", result.Headers.Select(pair => pair.Key));
            Assert.Contains("RateLimit-Remaining", result.Headers.Select(pair => pair.Key));
            Assert.Contains("RateLimit-Reset", result.Headers.Select(pair => pair.Key));

            Assert.Equal(retryAfter, ratelimitReset);
        }
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
        services.AddFakeHttpContextAccessor(ServiceLifetime.Singleton);
        services.AddMemoryThrottlingCache();
    }

    public override void ConfigureApplication(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(routes =>
        {
            routes.MapControllers();
        });
    }
}
