using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Cuemon.Net.Http;
/// <summary>
/// Defines a way to support content negotiation for HTTP enabled applications.
/// </summary>
public interface IContentNegotiation
{
    /// <summary>
    /// Gets the collection of media types supported by the HTTP enabled application.
    /// </summary>
    /// <value>The collection of media types supported by the HTTP enabled application.</value>
    IReadOnlyCollection<MediaTypeHeaderValue> SupportedMediaTypes { get; }
}
