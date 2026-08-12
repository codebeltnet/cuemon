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

namespace Cuemon.Net.Http;
/// <summary>
/// Tests for the <see cref="HttpWatcher"/> class.
/// </summary>
public class HttpWatcherTest : Test
{
    public HttpWatcherTest(ITestOutputHelper output) : base(output)
    {
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

        var invalidHandler = new SequenceHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(System.Array.Empty<byte>()) });
        var invalidWatcher = new TestHttpWatcher(new Uri("https://example.com/invalid"), o =>
        {
            o.ClientFactory = () => new HttpClient(invalidHandler, false);
            o.DueTime = Timeout.InfiniteTimeSpan;
            o.Period = Timeout.InfiniteTimeSpan;
        });

        await Assert.ThrowsAsync<InvalidOperationException>(() => invalidWatcher.SignalAsync());
        Assert.Throws<ArgumentNullException>(() => new HttpWatcher(null));
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
