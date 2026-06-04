using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;

namespace Cuemon.Resilience.Assets
{
    public static class FuncTransientOperation
    {
        public static string MethodThatReturnsOkString(Guid id, ConcurrentDictionary<Guid, int> retryTracker)
        {
            retryTracker[id] += 1;
            return "OK";
        }

        public static string FailUntilExpectedRetryAttemptsIsReached(Guid id, int expectedRetryAttempts, ConcurrentDictionary<Guid, int> retryTracker)
        {
            retryTracker[id] += 1;
            if (retryTracker[id] < expectedRetryAttempts) { throw new HttpRequestException(); }
            return "OK";
        }

        public static string TriggerTransientFaultException(Guid id, ConcurrentDictionary<Guid, int> retryTracker)
        {
            retryTracker[id] += 1;
            throw new HttpRequestException();
        }

        public static string TriggerLatencyException(Guid id, ConcurrentDictionary<Guid, int> retryTracker)
        {
            Thread.Sleep(750);
            retryTracker[id] += 1;
            throw new HttpRequestException();
        }

        public static string FailWithNonTransientFaultException(Guid id, int expectedRetryAttempts, ConcurrentDictionary<Guid, int> retryTracker)
        {
            retryTracker[id] += 1;
            if (retryTracker[id] < expectedRetryAttempts) { throw new HttpRequestException(); }
            throw new InvalidOperationException();
        }
    }
}
