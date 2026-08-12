using System;

namespace Cuemon.Extensions.Net.Http;
internal sealed class ActiveHandler
{
    public ActiveHandler(string name, DateTime expires, TrackingHttpMessageHandler handler)
    {
        Name = name;
        Expires = expires;
        Handler = handler;
    }

    public string Name { get; }

    public DateTime Expires { get; }

    public TrackingHttpMessageHandler Handler { get; }
}
