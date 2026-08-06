using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Cuemon.Resilience;
internal class AsyncTransientWorker : Transient<AsyncTransientOperationOptions>
{
    protected AsyncTransientWorker(MethodInfo delegateInfo, object[] runtimeArguments, Action<AsyncTransientOperationOptions> setup) : base(delegateInfo, runtimeArguments, setup)
    {
    }

    protected async Task<bool> ResilientCatchAsync(int attempts, TimeSpan waitTime, Exception ex, Func<Task> awaiter)
    {
        try
        {
            await ResilientCatchInnerTry(attempts, waitTime, ex, awaiter).ConfigureAwait(false);
            return false;
        }
        catch (Exception)
        {
            ResilientCatchInnerCatch(attempts);
            return true;
        }
    }

    private async Task ResilientCatchInnerTry(int attempts, TimeSpan waitTime, Exception ex, Func<Task> awaiter)
    {
        lock (_lock) { AggregatedExceptions.Insert(0, ex); }
        IsTransientFault = Options.DetectionStrategy(ex);
        if (attempts >= Options.RetryAttempts) { ExceptionDispatchInfo.Capture(ex).Throw(); }
        if (!IsTransientFault) { ExceptionDispatchInfo.Capture(ex).Throw(); }
        LastWaitTime = waitTime;
        TotalWaitTime = TotalWaitTime.Add(waitTime);
        await awaiter().ConfigureAwait(false);
        Latency = DateTime.UtcNow.Subtract(TimeStamp).Subtract(TotalWaitTime);
    }
}
