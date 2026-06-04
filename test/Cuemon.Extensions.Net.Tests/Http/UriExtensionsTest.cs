using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Cuemon.Threading;
using Xunit;

namespace Cuemon.Extensions.Net.Http
{
    public class UriExtensionsTest : Test
    {
        public UriExtensionsTest(ITestOutputHelper output) : base(output)
        {
            UriExtensions.DefaultHttpClientFactory = new SlimHttpClientFactory(() => new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                MaxAutomaticRedirections = 10
            }, o => o.HandlerLifetime = TimeSpan.MinValue);
        }

        [Fact]
        public async Task HttpGetAsync_ShouldGetResponseFromUri()
        {
            // Test SlimHttpClientFactory robustness under parallel load using a reliable external server
            var uri = new Uri("https://free.mockerapi.com/200");
            var expected = 125;
            var atomicCount = 0;

            await ParallelFactory.ForAsync(0, expected, async (i, ct) =>
            {
                using (var response = await uri.HttpGetAsync(ct))
                {
                    Interlocked.Increment(ref atomicCount);
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                }
            });

            Assert.Equal(expected, atomicCount);
        }

        [Fact]
        public async Task HttpGetAsync_ShouldHandleHttpStatusCodes()
        {
            // Test that the extension method properly returns non-OK status codes under parallel load
            var uri = new Uri("https://free.mockerapi.com/404");
            var expected = 50;
            var atomicCount = 0;

            await ParallelFactory.ForAsync(0, expected, async (i, ct) =>
            {
                using (var response = await uri.HttpGetAsync(ct))
                {
                    Interlocked.Increment(ref atomicCount);
                    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                }
            });

            Assert.Equal(expected, atomicCount);
        }
    }
}
