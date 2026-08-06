using System;
using System.Reflection;
using System.Threading;

namespace Cuemon.Resilience;
internal sealed class ActionTransientWorker : TransientWorker
{
    internal ActionTransientWorker(MethodInfo delegateInfo, object[] runtimeArguments, Action<TransientOperationOptions> setup) : base(delegateInfo, runtimeArguments, setup)
    {
    }

    public void ResilientAction(Action operation)
    {
        for (var attempts = 0; ;)
        {
            var waitTime = Options.RetryStrategy(attempts);
            try
            {
                ResilientTry();
                operation();
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
        }
        ResilientThrower();
    }
}
