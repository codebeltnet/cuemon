using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Resilience.Assets
{
    public static class AsyncActionTransientOperation
    {
        public static Task MethodThatReturnsOkStringAsync(Guid id, ConcurrentDictionary<Guid, int> retryTracker, CancellationToken ct)
        {
            retryTracker[id] += 1;
            return Task.CompletedTask;
        }

        public static Task FailUntilExpectedRetryAttemptsIsReachedAsync(Guid id, int expectedRetryAttempts, ConcurrentDictionary<Guid, int> retryTracker, CancellationToken ct)
        {
            retryTracker[id] += 1;
            if (retryTracker[id] < expectedRetryAttempts) { throw new HttpRequestException(); }
            return Task.CompletedTask;
        }

        public static Task TriggerTransientFaultExceptionAsync(Guid id, ConcurrentDictionary<Guid, int> retryTracker, CancellationToken ct)
        {
            retryTracker[id] += 1;
            throw new HttpRequestException();
        }

        public static Task TriggerLatencyExceptionAsync(Guid id, ConcurrentDictionary<Guid, int> retryTracker, CancellationToken ct)
        {
            Thread.Sleep(750);
            retryTracker[id] += 1;
            throw new HttpRequestException();
        }

        public static Task FailWithNonTransientFaultExceptionAsync(Guid id, int expectedRetryAttempts, ConcurrentDictionary<Guid, int> retryTracker, CancellationToken ct)
        {
            retryTracker[id] += 1;
            if (retryTracker[id] < expectedRetryAttempts) { throw new HttpRequestException(); }
            throw new InvalidOperationException();
        }
    }
}
