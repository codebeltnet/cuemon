using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Cuemon.Net.Http;
using Xunit;

namespace Cuemon.Net.Http
{
    /// <summary>
    /// Tests for the <see cref="HttpDependency"/> class.
    /// </summary>
    public class HttpDependencyTest : Test
    {
        public HttpDependencyTest(ITestOutputHelper output) : base(output)
        {
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

            var modified = await WaitOrThrowAsync(changed.Task, TimeSpan.FromSeconds(5));
            Assert.True(dependency.HasChanged);
            Assert.Equal(modified, dependency.UtcLastModified);
            Assert.Throws<ArgumentNullException>(() => new HttpDependency((Lazy<HttpWatcher>)null));
        }

        private static async Task<T> WaitOrThrowAsync<T>(Task<T> task, TimeSpan timeout)
        {
            var timeoutTask = Task.Delay(timeout);
            if (await Task.WhenAny(task, timeoutTask) != task) { throw new TimeoutException(); }
            return await task;
        }

        private static HttpResponseMessage ResponseWithHeaders(string entityTag, DateTimeOffset? lastModified)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(System.Array.Empty<byte>())
            };
            if (entityTag != null) { response.Headers.ETag = new EntityTagHeaderValue(entityTag); }
            if (lastModified.HasValue) { response.Content.Headers.LastModified = lastModified; }
            return response;
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
