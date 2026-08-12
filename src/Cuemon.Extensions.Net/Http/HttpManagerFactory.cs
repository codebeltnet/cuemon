using System.Net.Http;
using Cuemon.Net.Http;

namespace Cuemon.Extensions.Net.Http;
/// <summary>
/// Provides access to factory methods for creating and configuring <see cref="HttpManager"/> instances.
/// </summary>
public static class HttpManagerFactory
{
    /// <summary>
    /// Creates and returns an <see cref="HttpManager"/> from the specified <paramref name="factory"/>.
    /// </summary>
    /// <param name="factory">The <see cref="IHttpClientFactory"/> that determines the <see cref="HttpClient"/> to use.</param>
    /// <param name="name">The logical name of the client to create.</param>
    /// <returns>HttpManager.</returns>
    public static HttpManager CreateManager(IHttpClientFactory factory, string name = null)
    {
        return new HttpManager(() => factory.CreateClient(name ?? string.Empty));
    }
}
