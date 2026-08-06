using System.Net.Http;

namespace Cuemon.Extensions.Net.Http;
internal sealed class TrackingHttpMessageHandler : DelegatingHandler
{
    public TrackingHttpMessageHandler(HttpMessageHandler inner) : base(inner)
    {
    }

    protected override void Dispose(bool disposing)
    {
        // The lifetime of this is tracked separately by ActiveHandler
    }
}
