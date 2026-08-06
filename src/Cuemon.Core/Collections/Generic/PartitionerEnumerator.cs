using System;
using System.Collections;
using System.Collections.Generic;

namespace Cuemon.Collections.Generic;
internal sealed class PartitionerEnumerator<T> : Disposable, IEnumerator<T>
{
    public PartitionerEnumerator(IEnumerator<T> enumerator, int take, Action moveNextIncrementer, Action endOfSequenceNotifier)
    {
        Enumerator = enumerator;
        Take = take;
        MoveNextIncrementer = moveNextIncrementer;
        EndOfSequenceNotifier = endOfSequenceNotifier;
    }

    private IEnumerator<T> Enumerator { get; }

    public int IteratedCount { get; private set; }

    public bool EndOfSequence { get; private set; }

    public int Take { get; }

    private Action EndOfSequenceNotifier { get; }

    private Action MoveNextIncrementer { get; }

    public bool MoveNext()
    {
        var mn = Enumerator.MoveNext();
        if (mn)
        {
            if (IteratedCount < Take)
            {
                MoveNextIncrementer();
                IteratedCount += 1;
            }
            else
            {
                return false;
            }
        }
        else
        {
            EndOfSequenceNotifier();
            EndOfSequence = true;
        }
        return mn;
    }

    public void Reset()
    {
        Enumerator.Reset();
    }

    public T Current => Enumerator.Current;

    object IEnumerator.Current => Current;

    protected override void OnDisposeManagedResources()
    {
        Enumerator?.Dispose();
    }
}
