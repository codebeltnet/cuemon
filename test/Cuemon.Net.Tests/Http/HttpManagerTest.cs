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
using Cuemon.Net.Http;
using Xunit;

namespace Cuemon.Net.Http;
/// <summary>
/// Tests for the <see cref="HttpManager"/> class.
/// </summary>
public class HttpManagerTest : Test
{
    public HttpManagerTest(ITestOutputHelper output) : base(output)
    {
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

    private static MemoryStream ToStream(string value)
    {
        return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(value));
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
