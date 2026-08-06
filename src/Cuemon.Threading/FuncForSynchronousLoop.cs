using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cuemon.Threading;
internal sealed class FuncForSynchronousLoop<TOperand, TResult> : ForSynchronousLoop<TOperand> where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
{
    public FuncForSynchronousLoop(ForLoopRuleset<TOperand> rules, Action<AsyncTaskFactoryOptions> setup) : base(rules, setup)
    {
    }

    private ConcurrentDictionary<TOperand, TResult> Result { get; } = new ConcurrentDictionary<TOperand, TResult>();


    protected override void FillWorkQueueWorkerFactory<TWorker>(MutableTupleFactory<TWorker> worker)
    {
        if (worker is FuncFactory<TWorker, TResult> wf)
        {
            var presult = wf.ExecuteMethod();
            Result.TryAdd(wf.GenericArguments.Arg1, presult);
        }
    }

    public IReadOnlyCollection<TResult> GetResult<TWorker>(MutableTupleFactory<TWorker> worker) where TWorker : MutableTuple<TOperand>
    {
        PrepareExecution(worker);
        return new ReadOnlyCollection<TResult>(Result.Values.ToList());
    }
}
