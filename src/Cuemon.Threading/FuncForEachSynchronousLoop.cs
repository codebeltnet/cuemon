using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cuemon.Threading;
internal sealed class FuncForEachSynchronousLoop<TSource, TResult> : ForEachSynchronousLoop<TSource>
{
    public FuncForEachSynchronousLoop(IEnumerable<TSource> source, Action<AsyncTaskFactoryOptions> setup) : base(source, setup)
    {
    }

    protected override void FillWorkQueueWorkerFactory<TWorker>(MutableTupleFactory<TWorker> worker, long sorter)
    {
        if (worker is FuncFactory<TWorker, TResult> wf)
        {
            var presult = wf.ExecuteMethod();
            Result.TryAdd(sorter, presult);
        }
    }

    private ConcurrentDictionary<long, TResult> Result { get; } = new ConcurrentDictionary<long, TResult>();

    public IReadOnlyCollection<TResult> GetResult<TWorker>(MutableTupleFactory<TWorker> worker) where TWorker : MutableTuple<TSource>
    {
        PrepareExecution(worker);
        return new ReadOnlyCollection<TResult>(Result.Values.ToList());
    }
}
