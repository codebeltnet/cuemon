using System;
using System.Collections.Generic;
using Cuemon.Collections.Generic;

namespace Cuemon.Threading;
internal sealed class ActionForEachSynchronousLoop<TSource> : ForEachSynchronousLoop<TSource>
{
    public ActionForEachSynchronousLoop(IEnumerable<TSource> source, Action<AsyncTaskFactoryOptions> setup) : base(source, setup)
    {
        Partitioner = new PartitionerEnumerable<TSource>(source, Options.PartitionSize);
        WhileCondition = () => Partitioner.HasPartitions;
    }

    private PartitionerEnumerable<TSource> Partitioner { get; set; }

    protected override void FillWorkQueueWorkerFactory<TWorker>(MutableTupleFactory<TWorker> worker, long sorter)
    {
        if (worker is ActionFactory<TWorker> wf) { wf.ExecuteMethod(); }
    }
}
