using System.Net.Http;
using Cuemon.Threading;

namespace Cuemon.Net.Http;
/// <summary>
/// Specifies options that is related to <see cref="HttpManager"/> operations.
/// </summary>
public class HttpRequestOptions : AsyncOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpRequestOptions"/> class.
    /// </summary>
    public HttpRequestOptions()
    {
        Request = new HttpRequestMessage();
    }

    /// <summary>
    /// Gets the HTTP request message to send.
    /// </summary>
    /// <value>The HTTP request message to send.</value>
    public HttpRequestMessage Request { get; }

    /// <summary>
    /// Gets the recommended completion option of a response.
    /// </summary>
    /// <value>The recommended completion option of a response.</value>
    public HttpCompletionOption CompletionOption => (Request.Method == HttpMethod.Head || Request.Method == HttpMethod.Trace) ? HttpCompletionOption.ResponseHeadersRead : HttpCompletionOption.ResponseContentRead;
}
