using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.Extensions.AspNetCore.Mvc.Assets;
using Cuemon.Extensions.AspNetCore.Mvc.Filters.Cacheable;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.Net.Http;
using Cuemon.Runtime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Mvc;
public class HttpDependencyTest : Test // non-ideal fit for now; covers HttpDependency from Cuemon.Net (due to ALM and ironing out Netstandard 2.0 from all ASP.NET)
{
    public HttpDependencyTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task StartAsync_ShouldReceiveTwoSignalsFromHttpWatcher()
    {
        using (var filter = WebHostTestFactory.Create(services =>
        {
            services.Configure<HttpCacheableOptions>(o =>
            {
                o.Filters.AddEntityTagHeader();
                o.Filters.AddLastModifiedHeader();
            });
            services.AddControllers(o => o.Filters.Add<HttpCacheableFilter>()).AddApplicationPart(typeof(FakeController).Assembly);
        }, app =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var ce = new CountdownEvent(2);

            var sut1 = new Uri("http://localhost/fake");
            var sut2 = new Lazy<HttpWatcher>(() => new HttpWatcher(sut1, o =>
            {
                o.Period = TimeSpan.FromSeconds(1);
                o.DueTime = TimeSpan.FromMilliseconds(500);
                o.ClientFactory = () => filter.Host.GetTestClient();
            }));
            var sut3 = new HttpDependency(sut2);
            var sut4 = DateTime.UtcNow;
            var sut5 = new List<DateTime>();
            var sut6 = new EventHandler<DependencyEventArgs>((s, e) =>
            {
                sut5.Add(e.UtcLastModified);
                ce.Signal();
            });

            sut3.DependencyChanged += sut6;

            await sut3.StartAsync();

            await Task.Delay(TimeSpan.FromSeconds(2));

            TestOutput.WriteLines(sut5);

            var signaled = ce.Wait(TimeSpan.FromSeconds(15));

            sut3.DependencyChanged -= sut6;

            Assert.True(signaled);
            Assert.True(sut2.IsValueCreated);
            Assert.True(sut3.HasChanged);
            Assert.NotNull(sut3.UtcLastModified);
            Assert.InRange(sut3.UtcLastModified.Value, sut4, sut4.AddSeconds(5));
            Assert.Equal(2, sut5.Count);
        }
    }

    [Fact]
    public async Task StartAsync_ShouldReceiveTwoSignalsFromHttpWatcher_UsingReadResponse()
    {
        using (var filter = WebHostTestFactory.Create(services =>
        {
            services.Configure<HttpCacheableOptions>(o =>
            {
                o.Filters.AddEntityTagHeader();
                o.Filters.AddLastModifiedHeader();
            });
            services.AddControllers(o => o.Filters.Add<HttpCacheableFilter>()).AddApplicationPart(typeof(FakeController).Assembly);
        }, app =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var ce = new CountdownEvent(2);

            var sut1 = new Uri("http://localhost/fake");
            var sut2 = new Lazy<HttpWatcher>(() => new HttpWatcher(sut1, o =>
            {
                o.Period = TimeSpan.FromSeconds(1);
                o.DueTime = TimeSpan.FromMilliseconds(500);
                o.ReadResponseBody = true;
                o.ClientFactory = () => filter.Host.GetTestClient();
            }));
            var sut3 = new HttpDependency(sut2);
            var sut4 = DateTime.UtcNow;
            var sut5 = new List<DateTime>();
            var sut6 = new EventHandler<DependencyEventArgs>((s, e) =>
            {
                sut5.Add(e.UtcLastModified);
                ce.Signal();
            });

            sut3.DependencyChanged += sut6;

            await sut3.StartAsync();

            await Task.Delay(TimeSpan.FromSeconds(3));

            TestOutput.WriteLines(sut5);

            var signaled = ce.Wait(TimeSpan.FromSeconds(15));

            sut3.DependencyChanged -= sut6;

            Assert.True(signaled);
            Assert.True(sut2.IsValueCreated);
            Assert.True(sut3.HasChanged);
            Assert.NotNull(sut3.UtcLastModified);
            Assert.InRange(sut3.UtcLastModified.Value, sut4, sut4.AddSeconds(5));
            Assert.Equal(2, sut5.Count);
        }
    }

    [Fact]
    public async Task StartAsync_ShouldReceiveOnlyOneSignalFromHttpWatcher()
    {
        using (var filter = WebHostTestFactory.Create(services =>
        {
            services.Configure<HttpCacheableOptions>(o =>
            {
                o.Filters.AddEntityTagHeader();
                o.Filters.AddLastModifiedHeader();
            });
            services.AddControllers(o => o.Filters.Add<HttpCacheableFilter>()).AddApplicationPart(typeof(FakeController).Assembly);
        }, app =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var are = new AutoResetEvent(false);

            var sut1 = new Uri("http://localhost/fake");
            var sut2 = new Lazy<HttpWatcher>(() => new HttpWatcher(sut1, o =>
            {
                o.Period = TimeSpan.FromSeconds(1);
                o.DueTime = TimeSpan.FromMilliseconds(500);
                o.ClientFactory = () => filter.Host.GetTestClient();
            }));
            var sut3 = new HttpDependency(sut2, true);
            var sut4 = DateTime.UtcNow;
            var sut5 = new List<DateTime>();
            var sut6 = new EventHandler<DependencyEventArgs>((s, e) =>
            {
                sut5.Add(e.UtcLastModified);
                are.Set();
            });

            sut3.DependencyChanged += sut6;

            await sut3.StartAsync();

            await Task.Delay(TimeSpan.FromSeconds(2));

            TestOutput.WriteLines(sut5);

            var signaled = are.WaitOne(TimeSpan.FromSeconds(15));

            sut3.DependencyChanged -= sut6;

            Assert.True(signaled);
            Assert.True(sut2.IsValueCreated);
            Assert.True(sut3.HasChanged);
            Assert.NotNull(sut3.UtcLastModified);
            Assert.InRange(sut3.UtcLastModified.Value, sut4, sut4.AddSeconds(5));
            Assert.Equal(1, sut5.Count);
        }
    }
}
