using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Resilience;
internal sealed class AsyncFuncTransientWorker<TResult> : AsyncTransientWorker
{
    internal AsyncFuncTransientWorker(MethodInfo delegateInfo, object[] runtimeArguments, Action<AsyncTransientOperationOptions> setup) : base(delegateInfo, runtimeArguments, setup)
    {
    }

    public async Task<TResult> ResilientFuncAsync(Func<CancellationToken, Task<TResult>> operation)
    {
        var result = default(TResult);
        for (var attempts = 0; ;)
        {
            var waitTime = Options.RetryStrategy(attempts);
            try
            {
                ResilientTry();
                result = await operation(Options.CancellationToken).ConfigureAwait(false);
                break;
            }
            catch (Exception ex)
            {
                var sleep = waitTime;
                if (await ResilientCatchAsync(attempts, waitTime, ex, () => Task.Delay(sleep, Options.CancellationToken)).ConfigureAwait(false))
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
