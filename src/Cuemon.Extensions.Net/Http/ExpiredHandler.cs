using System;
using System.Net.Http;

namespace Cuemon.Extensions.Net.Http;
internal sealed class ExpiredHandler
{
    private readonly WeakReference _tracker;

    public ExpiredHandler(ActiveHandler origin)
    {
        _tracker = new WeakReference(origin.Handler);
        Name = origin.Name;
        InnerHandler = origin.Handler.InnerHandler;
    }

    public bool CanDispose => !_tracker.IsAlive;

    public string Name { get; }

    public HttpMessageHandler InnerHandler { get; }
}
