using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Net.Http
{
    public class SlimHttpClientFactoryTest : Test
    {
        public SlimHttpClientFactoryTest(ITestOutputHelper output) : base(output)
        {
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
}
