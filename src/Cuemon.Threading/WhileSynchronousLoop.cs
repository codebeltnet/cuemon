using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Threading;
internal abstract class WhileSynchronousLoop<TReader, TSource> : SynchronousLoop<TSource>
{
    private int _workChunks;

    protected WhileSynchronousLoop(ForwardIterator<TReader, TSource> iterator, Action<AsyncTaskFactoryOptions> setup) : base(setup)
    {
        Iterator = iterator;
        ReadForward = true;
        WhileCondition = () => true;
        Sorter = 0;
    }

    private ForwardIterator<TReader, TSource> Iterator { get; }

    private bool ReadForward { get; set; }

    private long Sorter { get; set; }

    protected override void OnWhileExecutingBeforeFillWorkQueue()
    {
        _workChunks = Options.PartitionSize;
    }

    protected sealed override void FillWorkQueue<TWorker>(MutableTupleFactory<TWorker> worker, IList<Func<Task>> queue)
    {
        while (_workChunks > 0 && ReadForward)
        {
            ReadForward = Iterator.Read();
            if (!ReadForward) { return; }
            var shallowWorkerFactory = worker.Clone();
            shallowWorkerFactory.GenericArguments.Arg1 = Iterator.Current;
            var current = Sorter;
            queue.Add(() => Task.Factory.StartNew(swf =>
            {
                try
                {
                    Interlocked.Decrement(ref _workChunks);
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
