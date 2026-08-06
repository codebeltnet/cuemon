using System;
using System.Reflection;
using System.Threading;

namespace Cuemon.Resilience;
internal sealed class FuncTransientWorker<TResult> : TransientWorker
{
    internal FuncTransientWorker(MethodInfo delegateInfo, object[] runtimeArguments, Action<TransientOperationOptions> setup) : base(delegateInfo, runtimeArguments, setup)
    {
    }

    public TResult ResilientFunc(Func<TResult> operation)
    {
        var result = default(TResult);
        for (var attempts = 0; ;)
        {
            var waitTime = Options.RetryStrategy(attempts);
            try
            {
                ResilientTry();
                result = operation();
                break;
            }
            catch (Exception ex)
            {
                var sleep = waitTime;
                if (ResilientCatch(attempts, waitTime, ex, () => new ManualResetEvent(false).WaitOne(sleep)))
                {
                    break;
                }
                attempts++;
            }
            finally
            {
                ResilientFinally(result);
            }
        }
        ResilientThrower();
        return result;
    }
}
