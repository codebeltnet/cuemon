using System;

namespace Cuemon.Threading;
internal abstract class Loop<TOptions> where TOptions : AsyncOptions, new()
{
    protected Loop(Action<TOptions> setup)
    {
        Options = Patterns.Configure(setup);
    }

    protected TOptions Options { get; }
}
