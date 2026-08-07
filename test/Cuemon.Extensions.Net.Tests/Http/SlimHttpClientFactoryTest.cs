using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Net.Http;
public class SlimHttpClientFactoryTest : Test
{
    public SlimHttpClientFactoryTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void SlimHttpClientFactory_ShouldValidateNullHandlerFactory()
    {
        Assert.Throws<ArgumentNullException>(() => new SlimHttpClientFactory(null));
    }

    [Fact]
    public void SlimHttpClientFactory_ShouldCreateClientsAndExposeOptions()
    {
        var invocations = 0;
        var firstInner = new TrackingAwareHttpClientHandler();
        var created = new Queue<TrackingAwareHttpClientHandler>(new[] { firstInner });
        var sut = new SlimHttpClientFactory(() =>
        {
            invocations++;
            return created.Dequeue();
        }, o => o.HandlerLifetime = TimeSpan.Zero);

        using var client = sut.CreateClient("alpha");

        Assert.Equal(1, invocations);
        Assert.NotNull(client);
    }

#if NET9_0_OR_GREATER
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
    }

    [Fact]
    public void SlimHttpClientFactory_ShouldCacheHandlersPerKey()
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

        // Request handlers for different keys
        var handler1 = factory.CreateHandler("key1");
        Assert.Equal(1, invocations);

        var handler2 = factory.CreateHandler("key1");
        Assert.Equal(1, invocations); // Should reuse cached handler for same key

        var handler3 = factory.CreateHandler("key2");
        Assert.Equal(2, invocations); // Should create new handler for different key

        // Verify caching and distinctness
        Assert.Same(handler1, handler2); // Same key, same handler
        Assert.NotSame(handler1, handler3); // Different keys, different handlers
    }
#endif

    private sealed class TrackingAwareHttpClientHandler : HttpClientHandler
    {
        public bool WasDisposed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            WasDisposed = true;
            base.Dispose(disposing);
        }
    }
}
