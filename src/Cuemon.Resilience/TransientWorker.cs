using System;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace Cuemon.Resilience;
internal class TransientWorker : Transient<TransientOperationOptions>
{
    protected TransientWorker(MethodInfo delegateInfo, object[] runtimeArguments, Action<TransientOperationOptions> setup) : base(delegateInfo, runtimeArguments, setup)
    {
    }

    protected bool ResilientCatch(int attempts, TimeSpan waitTime, Exception ex, Action awaiter)
    {
        try
        {
            ResilientCatchInnerTry(attempts, waitTime, ex, awaiter);
            return false;
        }
        catch (Exception)
        {
            ResilientCatchInnerCatch(attempts);
            return true;
        }
    }

    private void ResilientCatchInnerTry(int attempts, TimeSpan waitTime, Exception ex, Action awaiter)
    {
        lock (_lock) { AggregatedExceptions.Insert(0, ex); }
        IsTransientFault = Options.DetectionStrategy(ex);
        if (attempts >= Options.RetryAttempts) { ExceptionDispatchInfo.Capture(ex).Throw(); }
        if (!IsTransientFault) { ExceptionDispatchInfo.Capture(ex).Throw(); }
        LastWaitTime = waitTime;
        TotalWaitTime = TotalWaitTime.Add(waitTime);
        awaiter();
        Latency = DateTime.UtcNow.Subtract(TimeStamp).Subtract(TotalWaitTime);
    }
}
