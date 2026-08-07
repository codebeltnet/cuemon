using System;

namespace Cuemon.Threading;
internal sealed class ActionWhileSynchronousLoop<TReader, TSource> : WhileSynchronousLoop<TReader, TSource>
{
    public ActionWhileSynchronousLoop(ForwardIterator<TReader, TSource> iterator, Action<AsyncTaskFactoryOptions> setup) : base(iterator, setup)
    {
    }


    protected override void FillWorkQueueWorkerFactory<TWorker>(MutableTupleFactory<TWorker> worker, long sorter)
    {
        if (worker is ActionFactory<TWorker> wf) { wf.ExecuteMethod(); }
    }
}
