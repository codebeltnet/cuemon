using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.Collections.Generic;

namespace Cuemon.Threading;
internal abstract class ForEachSynchronousLoop<TSource> : SynchronousLoop<TSource>
{
    protected ForEachSynchronousLoop(IEnumerable<TSource> source, Action<AsyncTaskFactoryOptions> setup) : base(setup)
    {
        Partitioner = new PartitionerEnumerable<TSource>(source, Options.PartitionSize);
        WhileCondition = () => Partitioner.HasPartitions;
        Sorter = 0;
    }

    private PartitionerEnumerable<TSource> Partitioner { get; set; }

    private long Sorter { get; set; }

    protected sealed override void FillWorkQueue<TWorker>(MutableTupleFactory<TWorker> worker, IList<Func<Task>> queue)
    {
        foreach (var item in Partitioner)
        {
            var shallowWorkerFactory = worker.Clone();
            shallowWorkerFactory.GenericArguments.Arg1 = item;
            var current = Sorter;
            queue.Add(() => Task.Factory.StartNew(swf =>
            {
                try
                {
                    FillWorkQueueWorkerFactory(swf as MutableTupleFactory<TWorker>, current);
                }
                catch (Exception e)
                {
                    Exceptions.Add(e);
                }
            }, shallowWorkerFactory, Options.CancellationToken, Options.CreationOptions, Options.Scheduler));
            Sorter++;
        }
    }

    protected abstract void FillWorkQueueWorkerFactory<TWorker>(MutableTupleFactory<TWorker> worker, long sorter) where TWorker : MutableTuple<TSource>;
}
