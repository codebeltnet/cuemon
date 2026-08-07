using System;

namespace Cuemon.Threading;
internal sealed class ForwardIterator<TReader, TElement>
{
    internal ForwardIterator(TReader reader, Func<bool> condition, Func<TReader, TElement> provider)
    {
        Reader = reader;
        Condition = condition;
        Provider = provider;
    }

    private TReader Reader { get; }

    private Func<bool> Condition { get; }

    private Func<TReader, TElement> Provider { get; }

    public TElement Current { get; private set; }

    public bool Read()
    {
        if (Condition())
        {
            Current = Provider(Reader);
            return true;
        }
        Current = default;
        return false;
    }
}
