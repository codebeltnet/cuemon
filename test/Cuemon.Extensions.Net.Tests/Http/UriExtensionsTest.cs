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
        }

        [Fact]
        public async Task HttpGetAsync_ShouldGetResponseFromUri()
        {
            var factory = new StatusCodeHttpClientFactory(HttpStatusCode.OK);
            UriExtensions.DefaultHttpClientFactory = factory;
            var uri = new Uri("https://example.com/200");
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
            Assert.Equal(expected, factory.RequestCount);
        }

        [Fact]
        public async Task HttpGetAsync_ShouldHandleHttpStatusCodes()
        {
            var factory = new StatusCodeHttpClientFactory(HttpStatusCode.NotFound);
            UriExtensions.DefaultHttpClientFactory = factory;
            var uri = new Uri("https://example.com/404");
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
            Assert.Equal(expected, factory.RequestCount);
        }

        private sealed class StatusCodeHttpClientFactory : IHttpClientFactory
        {
            private int _requestCount;
            private readonly HttpStatusCode _statusCode;

            public StatusCodeHttpClientFactory(HttpStatusCode statusCode)
            {
                _statusCode = statusCode;
            }

            public int RequestCount => _requestCount;

            public HttpClient CreateClient(string name)
            {
                return new HttpClient(new StatusCodeHttpMessageHandler(this, _statusCode));
            }

            private void IncrementRequestCount()
            {
                Interlocked.Increment(ref _requestCount);
            }

            private sealed class StatusCodeHttpMessageHandler : HttpMessageHandler
            {
                private readonly StatusCodeHttpClientFactory _factory;
                private readonly HttpStatusCode _statusCode;

                public StatusCodeHttpMessageHandler(StatusCodeHttpClientFactory factory, HttpStatusCode statusCode)
                {
                    _factory = factory;
                    _statusCode = statusCode;
                }

                protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
                {
                    _factory.IncrementRequestCount();
                    return Task.FromResult(new HttpResponseMessage(_statusCode)
                    {
                        Content = new ByteArrayContent(Array.Empty<byte>()),
                        RequestMessage = request
                    });
                }
            }
        }
    }
}
