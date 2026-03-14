using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using Cuemon.Threading;

namespace Cuemon.Extensions.Net.Http
{
#if NET10_0_OR_GREATER
    /// <summary>
    /// Provides a simple and lightweight implementation of the <see cref="IHttpClientFactory"/> interface.
    /// </summary>
    /// <seealso cref="IHttpClientFactory" />
    /// <seealso cref="IHttpMessageHandlerFactory" />
    /// <remarks>Inspiration taken from https://github.com/dotnet/runtime/blob/master/src/libraries/Microsoft.Extensions.Http/src/DefaultHttpClientFactory.cs</remarks>
#else
    /// <summary>
    /// Provides a simple and lightweight implementation of the <see cref="IHttpClientFactory"/> interface.
    /// </summary>
    /// <seealso cref="IHttpClientFactory" />
    /// <remarks>Inspiration taken from https://github.com/dotnet/runtime/blob/master/src/libraries/Microsoft.Extensions.Http/src/DefaultHttpClientFactory.cs</remarks>
#endif
    public class SlimHttpClientFactory : IHttpClientFactory
#if NET10_0_OR_GREATER
        , IHttpMessageHandlerFactory
#endif
    {
        private readonly ConcurrentDictionary<string, Lazy<ActiveHandler>> _activeHandlers = new();
        private readonly ConcurrentQueue<ExpiredHandler> _expiredHandlers = new();
        private readonly Func<HttpClientHandler> _handlerFactory;
#if NET10_0_OR_GREATER
        private readonly Lock _lock = new();
#else
        private readonly object _lock = new();
#endif
        private readonly SlimHttpClientFactoryOptions _options;
        internal static readonly TimeSpan ExpirationTimerDueTime = TimeSpan.FromSeconds(15);
        private Timer _expirationTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimHttpClientFactory"/> class.
        /// </summary>
        /// <param name="handlerFactory">The function delegate that creates and configures an <see cref="HttpClientHandler"/>.</param>
        /// <param name="setup">The <see cref="SlimHttpClientFactoryOptions" /> which may be configured.</param>
        public SlimHttpClientFactory(Func<HttpClientHandler> handlerFactory, Action<SlimHttpClientFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(handlerFactory);
            _handlerFactory = handlerFactory;
            _options = Patterns.Configure(setup);
        }

        /// <summary>
        /// Creates and configures an <see cref="T:System.Net.Http.HttpClient" /> instance using the configuration that corresponds to the logical name specified by <paramref name="name" />.
        /// </summary>
        /// <param name="name">The logical name of the client to create.</param>
        /// <returns>A new <see cref="T:System.Net.Http.HttpClient" /> instance.</returns>
        public HttpClient CreateClient(string name)
        {
            var handler = CreateHandler(name);
            return new HttpClient(handler, false);
        }

        /// <summary>
        /// Creates and configures an <see cref="T:System.Net.Http.HttpMessageHandler" /> instance using the configuration that corresponds to the logical name specified by <paramref name="name" />.
        /// </summary>
        /// <param name="name">The logical name of the message handler to create.</param>
        /// <returns>A new <see cref="T:System.Net.Http.HttpMessageHandler" /> instance.</returns>
        public HttpMessageHandler CreateHandler(string name)
        {
            StartExpirationTimer(name);
            return _activeHandlers.GetOrAdd(name, new Lazy<ActiveHandler>(() => new ActiveHandler(name, DateTime.UtcNow.Add(_options.HandlerLifetime), new TrackingHttpMessageHandler(_handlerFactory.Invoke())), LazyThreadSafetyMode.ExecutionAndPublication)).Value.Handler;
        }

        private void StartExpirationTimer(string name)
        {
            lock (_lock)
            {
                if (_expirationTimer == null)
                {
                    _expirationTimer = TimerFactory.CreateNonCapturingTimer(ExpirationTimerInvoking, name, ExpirationTimerDueTime, Timeout.InfiniteTimeSpan);
                    Debug.WriteLine($"{nameof(StartExpirationTimer)} initialized and started {nameof(_expirationTimer)} that is due in {ExpirationTimerDueTime}.");
                }
            }
        }

        private void StopExpirationTimer()
        {
            lock (_lock)
            {
                _expirationTimer.Dispose();
                _expirationTimer = null;
                Debug.WriteLine($"{nameof(StopExpirationTimer)} disposed and nullified {nameof(_expirationTimer)}.");
            }
        }

        private void ExpirationTimerInvoking(object state)
        {
            var name = (string)state;
            SetActiveHandlerToExpiredHandler(name);
        }

        private void SetActiveHandlerToExpiredHandler(string name)
        {
            StopExpirationTimer();
            if (_activeHandlers.TryGetValue(name, out var lazyHandler) && DateTime.UtcNow >= lazyHandler.Value.Expires)
            {
                _expiredHandlers.Enqueue(new ExpiredHandler(lazyHandler.Value));
                _activeHandlers[name] = null;
                _activeHandlers.TryRemove(name, out _);
                Debug.WriteLine($"{nameof(SetActiveHandlerToExpiredHandler)} marked {name} as expired.");
            }
            ExpiredHandlersSweep();
            StartExpirationTimer(name);
        }

        private void ExpiredHandlersSweep()
        {
            lock (_lock)
            {
                var queueCount = _expiredHandlers.Count;
                Debug.WriteLine($"{nameof(ExpiredHandlersSweep)} has {queueCount} expired handlers to sweep.");
                for (var i = 0; i < queueCount; i++)
                {
                    if (_expiredHandlers.TryDequeue(out var expiredHandler))
                    {
                        if (expiredHandler.CanDispose)
                        {
                            expiredHandler.InnerHandler.Dispose();
                            Debug.WriteLine($"{nameof(ExpiredHandlersSweep)} finalized {expiredHandler.Name} with call to dispose.");
                        }
                        else
                        {
                            _expiredHandlers.Enqueue(expiredHandler);
                            Debug.WriteLine($"{nameof(ExpiredHandlersSweep)} was unable to dispose expired handler {expiredHandler.Name}.");
                        }
                    }
                }
            }
        }
    }
}
