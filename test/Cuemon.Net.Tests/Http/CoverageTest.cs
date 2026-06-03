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
using Cuemon.Runtime;
using Xunit;

namespace Cuemon.Net.Http
{
    public class CoverageTest : Test
    {
        public CoverageTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void HttpMethodConverterAndRequestOptions_ShouldResolveKnownValues()
        {
            Assert.Equal(HttpMethods.Get, HttpMethodConverter.ToHttpMethod(HttpMethod.Get));
            Assert.Equal(HttpMethods.Post, HttpMethodConverter.ToHttpMethod(HttpMethod.Post));
            Assert.Equal(HttpMethods.Put, HttpMethodConverter.ToHttpMethod(HttpMethod.Put));
            Assert.Equal(HttpMethods.Delete, HttpMethodConverter.ToHttpMethod(HttpMethod.Delete));
            Assert.Equal(HttpMethods.Head, HttpMethodConverter.ToHttpMethod(HttpMethod.Head));
            Assert.Equal(HttpMethods.Options, HttpMethodConverter.ToHttpMethod(HttpMethod.Options));
            Assert.Equal(HttpMethods.Trace, HttpMethodConverter.ToHttpMethod(HttpMethod.Trace));
            Assert.Equal(HttpMethods.Patch, HttpMethodConverter.ToHttpMethod(new HttpMethod("PATCH")));
            Assert.Equal(HttpMethods.Get, HttpMethodConverter.ToHttpMethod(new HttpMethod("CUSTOM")));
            Assert.Throws<ArgumentNullException>(() => HttpMethodConverter.ToHttpMethod(null));

            var options = new HttpRequestOptions();
            Assert.NotNull(options.Request);
            Assert.Equal(HttpCompletionOption.ResponseContentRead, options.CompletionOption);
            options.Request.Method = HttpMethod.Head;
            Assert.Equal(HttpCompletionOption.ResponseHeadersRead, options.CompletionOption);
            options.Request.Method = HttpMethod.Trace;
            Assert.Equal(HttpCompletionOption.ResponseHeadersRead, options.CompletionOption);
        }

        [Fact]
        public async Task HttpManager_ShouldSendRequestsAndValidateArguments()
        {
            var handler = new RecordingHttpMessageHandler();
            using var manager = new HttpManager(() => new HttpClient(handler, false));
            var location = new Uri("https://example.com/resource");

            using (await manager.HttpDeleteAsync(location)) { }
            using (await manager.HttpGetAsync(location)) { }
            using (await manager.HttpHeadAsync(location)) { }
            using (await manager.HttpOptionsAsync(location)) { }
            using (await manager.HttpTraceAsync(location)) { }
            using (await manager.HttpPostAsync(location, "text/plain", ToStream("alpha"))) { }
            using (await manager.HttpPostAsync(location, MediaTypeHeaderValue.Parse("application/json"), ToStream("{}"))) { }
            using (await manager.HttpPutAsync(location, "text/plain", ToStream("beta"))) { }
            using (await manager.HttpPutAsync(location, MediaTypeHeaderValue.Parse("application/json"), ToStream("{}"))) { }
            using (await manager.HttpPatchAsync(location, "text/plain", ToStream("gamma"))) { }
            using (await manager.HttpPatchAsync(location, MediaTypeHeaderValue.Parse("application/json"), ToStream("{}"))) { }
            using (await manager.HttpAsync(HttpMethod.Post, location, "application/xml", ToStream("<a />"))) { }
            using (await manager.HttpAsync(HttpMethod.Put, location, MediaTypeHeaderValue.Parse("application/octet-stream"), ToStream("bin"))) { }
            using (await manager.HttpAsync(location, o => o.Request.Method = HttpMethod.Get)) { }

            Assert.Equal(new[]
            {
                HttpMethod.Delete.Method,
                HttpMethod.Get.Method,
                HttpMethod.Head.Method,
                HttpMethod.Options.Method,
                HttpMethod.Trace.Method,
                HttpMethod.Post.Method,
                HttpMethod.Post.Method,
                HttpMethod.Put.Method,
                HttpMethod.Put.Method,
                "PATCH",
                "PATCH",
                HttpMethod.Post.Method,
                HttpMethod.Put.Method,
                HttpMethod.Get.Method
            }, handler.Requests.Select(r => r.Method.Method).ToArray());
            Assert.Equal("text/plain", handler.Requests[5].Content.Headers.ContentType.MediaType);
            Assert.Equal("application/json", handler.Requests[6].Content.Headers.ContentType.MediaType);
            Assert.Equal("application/octet-stream", handler.Requests[12].Content.Headers.ContentType.MediaType);
            Assert.Throws<ArgumentNullException>(() => new HttpManager((Func<HttpClient>)null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => manager.HttpAsync((Uri)null, o => o.Request.Method = HttpMethod.Get));
            await Assert.ThrowsAsync<ArgumentNullException>(() => manager.HttpAsync(location, (Action<HttpRequestOptions>)null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => manager.HttpAsync(null, location, MediaTypeHeaderValue.Parse("text/plain"), ToStream("x")));
            await Assert.ThrowsAsync<ArgumentNullException>(() => manager.HttpAsync(HttpMethod.Get, location, (MediaTypeHeaderValue)null, ToStream("x")));
            await Assert.ThrowsAsync<ArgumentNullException>(() => manager.HttpAsync(HttpMethod.Get, location, MediaTypeHeaderValue.Parse("text/plain"), null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => manager.HttpAsync(HttpMethod.Get, location, (string)null, ToStream("x")));
        }

        [Fact]
        public async Task HttpManager_ShouldApplyOptionsToCreatedClient()
        {
            var handler = new RecordingHttpMessageHandler();
            using var manager = new HttpManager(o =>
            {
                o.HandlerFactory = () => handler;
                o.DefaultRequestHeaders.Add("X-Test", "alpha");
                o.Timeout = TimeSpan.FromSeconds(15);
            });
            var options = new HttpManagerOptions();
            var watcherOptions = new HttpWatcherOptions();

            options.ValidateOptions();
            watcherOptions.ValidateOptions();

            Assert.True(manager.DefaultRequestHeaders.Contains("Connection"));
            Assert.True(manager.DefaultRequestHeaders.Contains("X-Test"));
            Assert.Equal(TimeSpan.FromSeconds(15), manager.Timeout);
            Assert.False(options.DisposeHandler);
            Assert.False(watcherOptions.ReadResponseBody);

            using (await manager.HttpGetAsync(new Uri("https://example.com/headers"))) { }
            Assert.Equal("alpha", handler.Requests.Single().Headers.GetValues("X-Test").Single());
        }

        [Fact]
        public async Task HttpWatcher_ShouldReactToChecksumEntityTagAndLastModifiedChanges()
        {
            var bodyHandler = new SequenceHttpMessageHandler(_ => ResponseWithBody("first"), _ => ResponseWithBody("second"));
            var bodyWatcher = new TestHttpWatcher(new Uri("https://example.com/body"), o =>
            {
                o.ReadResponseBody = true;
                o.ClientFactory = () => new HttpClient(bodyHandler, false);
                o.DueTime = Timeout.InfiniteTimeSpan;
                o.Period = Timeout.InfiniteTimeSpan;
            });
            var bodyChanges = 0;
            bodyWatcher.Changed += (_, _) => bodyChanges++;

            await bodyWatcher.SignalAsync();
            await bodyWatcher.SignalAsync();

            Assert.NotNull(bodyWatcher.Checksum);
            Assert.Equal(1, bodyChanges);
            Assert.All(bodyHandler.Requests, request => Assert.True(request.Headers.Contains("Listener-Object")));

            var etagHandler = new SequenceHttpMessageHandler(_ => ResponseWithHeaders("\"v1\"", null), _ => ResponseWithHeaders("\"v2\"", null));
            var etagWatcher = new TestHttpWatcher(new Uri("https://example.com/etag"), o =>
            {
                o.ClientFactory = () => new HttpClient(etagHandler, false);
                o.DueTime = Timeout.InfiniteTimeSpan;
                o.Period = Timeout.InfiniteTimeSpan;
            });
            var etagChanges = 0;
            etagWatcher.Changed += (_, _) => etagChanges++;

            await etagWatcher.SignalAsync();
            await etagWatcher.SignalAsync();

            Assert.Equal(1, etagChanges);

            var expectedUtc = DateTimeOffset.UtcNow.AddMinutes(-5);
            var lastModifiedHandler = new SequenceHttpMessageHandler(_ => ResponseWithHeaders(null, expectedUtc));
            var lastModifiedWatcher = new TestHttpWatcher(new Uri("https://example.com/head"), o =>
            {
                o.ClientFactory = () => new HttpClient(lastModifiedHandler, false);
                o.DueTime = Timeout.InfiniteTimeSpan;
                o.Period = Timeout.InfiniteTimeSpan;
            });
            var lastModifiedChanges = 0;
            lastModifiedWatcher.Changed += (_, _) => lastModifiedChanges++;

            await lastModifiedWatcher.SignalAsync();

            Assert.Equal(1, lastModifiedChanges);
            Assert.Equal(expectedUtc.UtcDateTime, lastModifiedWatcher.UtcLastModified);

            var invalidHandler = new SequenceHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(Array.Empty<byte>()) });
            var invalidWatcher = new TestHttpWatcher(new Uri("https://example.com/invalid"), o =>
            {
                o.ClientFactory = () => new HttpClient(invalidHandler, false);
                o.DueTime = Timeout.InfiniteTimeSpan;
                o.Period = Timeout.InfiniteTimeSpan;
            });

            await Assert.ThrowsAsync<InvalidOperationException>(() => invalidWatcher.SignalAsync());
            Assert.Throws<ArgumentNullException>(() => new HttpWatcher(null));
        }

        [Fact]
        public async Task HttpDependency_ShouldRaiseDependencyChanged_WhenWatcherSignals()
        {
            var handler = new SequenceHttpMessageHandler(_ => ResponseWithHeaders("\"v1\"", null), _ => ResponseWithHeaders("\"v2\"", null));
            var watcher = new TestHttpWatcher(new Uri("https://example.com/dependency"), o =>
            {
                o.ClientFactory = () => new HttpClient(handler, false);
                o.DueTime = Timeout.InfiniteTimeSpan;
                o.Period = Timeout.InfiniteTimeSpan;
            });
            await watcher.SignalAsync();

            var dependency = new HttpDependency(new Lazy<HttpWatcher>(() => watcher));
            var changed = new TaskCompletionSource<DateTime?>(TaskCreationOptions.RunContinuationsAsynchronously);
            dependency.DependencyChanged += (_, e) => changed.TrySetResult(e.UtcLastModified);

            await dependency.StartAsync();
            watcher.ChangeSignaling(TimeSpan.Zero, Timeout.InfiniteTimeSpan);

            var modified = await changed.Task.WaitAsync(TimeSpan.FromSeconds(5));
            Assert.True(dependency.HasChanged);
            Assert.Equal(modified, dependency.UtcLastModified);
            Assert.Throws<ArgumentNullException>(() => new HttpDependency((Lazy<HttpWatcher>)null));
        }

        private static MemoryStream ToStream(string value)
        {
            return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(value));
        }

        private static HttpResponseMessage ResponseWithBody(string value)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(value))
            };
        }

        private static HttpResponseMessage ResponseWithHeaders(string entityTag, DateTimeOffset? lastModified)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(Array.Empty<byte>())
            };
            if (entityTag != null) { response.Headers.ETag = new EntityTagHeaderValue(entityTag); }
            if (lastModified.HasValue) { response.Content.Headers.LastModified = lastModified; }
            return response;
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

        private sealed class SequenceHttpMessageHandler : HttpMessageHandler
        {
            private readonly Queue<Func<HttpRequestMessage, HttpResponseMessage>> _responses;

            public SequenceHttpMessageHandler(params Func<HttpRequestMessage, HttpResponseMessage>[] responses)
            {
                _responses = new Queue<Func<HttpRequestMessage, HttpResponseMessage>>(responses);
            }

            public List<HttpRequestMessage> Requests { get; } = new List<HttpRequestMessage>();

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Requests.Add(request);
                return Task.FromResult(_responses.Dequeue().Invoke(request));
            }
        }

        private sealed class TestHttpWatcher : HttpWatcher
        {
            public TestHttpWatcher(Uri location, Action<HttpWatcherOptions> setup = null) : base(location, setup)
            {
            }

            public Task SignalAsync()
            {
                return HandleSignalingAsync();
            }
        }
    }
}
