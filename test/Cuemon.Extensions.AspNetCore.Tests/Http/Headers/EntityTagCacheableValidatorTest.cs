using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Cuemon.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Http.Headers
{
    public class EntityTagCacheableValidatorTest : Test
    {
        public EntityTagCacheableValidatorTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task ProcessAsync_ShouldAddEntityTagHeader_WhenServerTimingIsUnavailable()
        {
            var sut = new EntityTagCacheableValidator();
            var context = new DefaultHttpContext();
            context.RequestServices = new ServiceCollection().BuildServiceProvider();
            var body = new MemoryStream(Encoding.UTF8.GetBytes("Hello world!"));

            await sut.ProcessAsync(context, body);

            Assert.True(context.Response.Headers.ContainsKey(HeaderNames.ETag));
            Assert.False(string.IsNullOrWhiteSpace(context.Response.Headers[HeaderNames.ETag]));
        }

        [Fact]
        public async Task ProcessAsync_ShouldRecordServerTimingMetric_WhenServerTimingIsAvailable()
        {
            var sut = new EntityTagCacheableValidator();
            var serverTiming = new ServerTiming();
            var context = new DefaultHttpContext();
            context.RequestServices = new ServiceCollection().AddSingleton<IServerTiming>(serverTiming).BuildServiceProvider();
            var body = new MemoryStream(Encoding.UTF8.GetBytes("Hello world!"));

            await sut.ProcessAsync(context, body);

            var metric = Assert.Single(serverTiming.Metrics);
            Assert.Equal("entity-tag", metric.Name);
            Assert.True(metric.Duration.HasValue);
            Assert.True(context.Response.Headers.ContainsKey(HeaderNames.ETag));
        }
    }
}
