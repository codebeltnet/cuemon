using System;

namespace Cuemon.Threading;
internal sealed class ActionForSynchronousLoop<TOperand> : ForSynchronousLoop<TOperand> where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
{
    public ActionForSynchronousLoop(ForLoopRuleset<TOperand> rules, Action<AsyncTaskFactoryOptions> setup) : base(rules, setup)
    {
    }

    protected override void FillWorkQueueWorkerFactory<TWorker>(MutableTupleFactory<TWorker> worker)
    {
        if (worker is ActionFactory<TWorker> wf) { wf.ExecuteMethod(); }
    }
}
