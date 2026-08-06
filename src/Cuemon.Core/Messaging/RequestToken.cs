using System;

namespace Cuemon.Messaging;
/// <summary>
/// Provides a default implementation of the <see cref="IRequestToken"/> interface.
/// </summary>
/// <seealso cref="IRequestToken" />
public record RequestToken : IRequestToken
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequestToken"/> class.
    /// </summary>
    /// <param name="requestId">The string that uniquely identifies a request. Default value is a GUID expressed as 32 digits.</param>
    public RequestToken(string requestId = null)
    {
        RequestId = requestId ?? Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// Gets the unique request identifier.
    /// </summary>
    /// <value>The unique request identifier.</value>
    public string RequestId { get; }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return RequestId;
    }
}
