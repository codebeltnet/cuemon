using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;
using HttpRequestOptions = Cuemon.Net.Http.HttpRequestOptions;

namespace Cuemon.Extensions.Net.Http
{
    public class CoverageTest : Test
    {
        public CoverageTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void HttpMethodExtensions_ShouldConvertMethods()
        {
            Assert.Equal(Cuemon.Net.Http.HttpMethods.Get, HttpMethod.Get.ToHttpMethod());
            Assert.Equal(Cuemon.Net.Http.HttpMethods.Post, HttpMethod.Post.ToHttpMethod());
            Assert.Equal(Cuemon.Net.Http.HttpMethods.Put, HttpMethod.Put.ToHttpMethod());
            Assert.Equal(Cuemon.Net.Http.HttpMethods.Delete, HttpMethod.Delete.ToHttpMethod());
            Assert.Equal(Cuemon.Net.Http.HttpMethods.Patch, new HttpMethod("PATCH").ToHttpMethod());
            Assert.Throws<ArgumentNullException>(() => HttpMethodExtensions.ToHttpMethod(null));
        }

        [Fact]
        public void SlimHttpClientFactory_ShouldReuseHandlersAndProtectInnerHandlerOnDispose()
        {
            var invocations = 0;
            var firstInner = new TrackingAwareHttpClientHandler();
            var secondInner = new TrackingAwareHttpClientHandler();
            var created = new Queue<TrackingAwareHttpClientHandler>(new[] { firstInner, secondInner });
            var sut = new SlimHttpClientFactory(() =>
            {
                invocations++;
                return created.Dequeue();
            }, o => o.HandlerLifetime = TimeSpan.Zero);
            var factory = (IHttpMessageHandlerFactory)sut;
            var options = new SlimHttpClientFactoryOptions() { HandlerLifetime = TimeSpan.Zero };

            var handlerA1 = factory.CreateHandler("alpha");
            var handlerA2 = factory.CreateHandler("alpha");
            var handlerB = factory.CreateHandler("beta");
            using var client = sut.CreateClient("alpha");

            Assert.Same(handlerA1, handlerA2);
            Assert.NotSame(handlerA1, handlerB);
            Assert.Equal(2, invocations);
            Assert.Equal(TimeSpan.FromSeconds(15), options.HandlerLifetime);
            handlerA1.Dispose();
            Assert.False(firstInner.WasDisposed);
            Assert.Throws<ArgumentNullException>(() => new SlimHttpClientFactory(null));
        }

        [Fact]
        public async Task SlimHttpClientFactory_ShouldExpireAndDisposeHandlers_WhenLifetimeElapses()
        {
            var invocations = 0;
            var firstInner = new TrackingAwareHttpClientHandler();
            var secondInner = new TrackingAwareHttpClientHandler();
            var created = new Queue<TrackingAwareHttpClientHandler>(new[] { firstInner, secondInner });
            var sut = new SlimHttpClientFactory(() =>
            {
                invocations++;
                return created.Dequeue();
            }, o => o.HandlerLifetime = TimeSpan.Zero);
            var factory = (IHttpMessageHandlerFactory)sut;

            var handler = factory.CreateHandler("expiring");
            handler = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            await Task.Delay(TimeSpan.FromSeconds(16));

            var replacement = factory.CreateHandler("expiring");
            replacement = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            await Task.Delay(TimeSpan.FromSeconds(16));

            Assert.True(firstInner.WasDisposed);
            Assert.Equal(2, invocations);
        }

        [Fact]
        public async Task UriExtensions_ShouldDelegateRequestsThroughConfiguredFactory()
        {
            var previousFactory = UriExtensions.DefaultHttpClientFactory;
            var recordingFactory = new RecordingHttpClientFactory();
            try
            {
                UriExtensions.DefaultHttpClientFactory = recordingFactory;
                UriExtensions.DefaultHttpClientFactory = null;

                var location = new Uri("https://example.com/endpoint");

                using (await location.HttpDeleteAsync()) { }
                using (await location.HttpGetAsync()) { }
                using (await location.HttpHeadAsync()) { }
                using (await location.HttpOptionsAsync()) { }
                using (await location.HttpPostAsync("text/plain", ToStream("alpha"))) { }
                using (await location.HttpPostAsync(MediaTypeHeaderValue.Parse("application/json"), ToStream("{}"))) { }
                using (await location.HttpPutAsync("text/plain", ToStream("beta"))) { }
                using (await location.HttpPutAsync(MediaTypeHeaderValue.Parse("application/json"), ToStream("{}"))) { }
                using (await location.HttpPatchAsync("text/plain", ToStream("gamma"))) { }
                using (await location.HttpPatchAsync(MediaTypeHeaderValue.Parse("application/json"), ToStream("{}"))) { }
                using (await location.HttpTraceAsync()) { }
                using (await location.HttpAsync(HttpMethod.Post, "application/xml", ToStream("<a />"))) { }
                using (await location.HttpAsync(HttpMethod.Put, MediaTypeHeaderValue.Parse("application/octet-stream"), ToStream("bin"))) { }
                using (await location.HttpAsync(o => o.Request.Method = HttpMethod.Get)) { }

                Assert.Equal(new[]
                {
                    HttpMethod.Delete.Method,
                    HttpMethod.Get.Method,
                    HttpMethod.Head.Method,
                    HttpMethod.Options.Method,
                    HttpMethod.Post.Method,
                    HttpMethod.Post.Method,
                    HttpMethod.Put.Method,
                    HttpMethod.Put.Method,
                    "PATCH",
                    "PATCH",
                    HttpMethod.Trace.Method,
                    HttpMethod.Post.Method,
                    HttpMethod.Put.Method,
                    HttpMethod.Get.Method
                }, recordingFactory.Handler.Requests.Select(r => r.Method.Method).ToArray());
                Assert.Equal("text/plain", recordingFactory.Handler.Requests[4].Content.Headers.ContentType.MediaType);
                Assert.Same(recordingFactory, UriExtensions.DefaultHttpClientFactory);
            }
            finally
            {
                UriExtensions.DefaultHttpClientFactory = previousFactory;
            }
        }

        private static MemoryStream ToStream(string value)
        {
            return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(value));
        }

        private sealed class TrackingAwareHttpClientHandler : HttpClientHandler
        {
            public bool WasDisposed { get; private set; }

            protected override void Dispose(bool disposing)
            {
                WasDisposed = true;
                base.Dispose(disposing);
            }
        }

        private sealed class RecordingHttpClientFactory : IHttpClientFactory
        {
            public RecordingHttpMessageHandler Handler { get; } = new RecordingHttpMessageHandler();

            public HttpClient CreateClient(string name)
            {
                return new HttpClient(Handler, false);
            }
        }

        private sealed class RecordingHttpMessageHandler : HttpMessageHandler
        {
            public List<HttpRequestMessage> Requests { get; } = new List<HttpRequestMessage>();

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Requests.Add(request);
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(Array.Empty<byte>())
                });
            }
        }
    }
}
