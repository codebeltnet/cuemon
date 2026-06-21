using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;

namespace Cuemon.Resilience.Assets
{
    public static class ActionTransientOperation
    {
        public static void MethodThatReturnsOkString(Guid id, ConcurrentDictionary<Guid, int> retryTracker)
        {
            retryTracker[id] += 1;
        }

        public static void FailUntilExpectedRetryAttemptsIsReached(Guid id, int expectedRetryAttempts, ConcurrentDictionary<Guid, int> retryTracker)
        {
            retryTracker[id] += 1;
            if (retryTracker[id] < expectedRetryAttempts) { throw new HttpRequestException(); }
        }

        public static void TriggerTransientFaultException(Guid id, ConcurrentDictionary<Guid, int> retryTracker)
        {
            retryTracker[id] += 1;
            throw new HttpRequestException();
        }

        public static void TriggerLatencyException(Guid id, ConcurrentDictionary<Guid, int> retryTracker)
        {
            Thread.Sleep(750);
            retryTracker[id] += 1;
            throw new HttpRequestException();
        }

        public static void FailWithNonTransientFaultException(Guid id, int expectedRetryAttempts, ConcurrentDictionary<Guid, int> retryTracker)
        {
            retryTracker[id] += 1;
            if (retryTracker[id] < expectedRetryAttempts) { throw new HttpRequestException(); }
            throw new InvalidOperationException();
        }
    }
}
