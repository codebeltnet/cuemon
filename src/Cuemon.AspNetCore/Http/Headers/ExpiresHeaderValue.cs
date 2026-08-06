using System;

namespace Cuemon.AspNetCore.Http.Headers;
/// <summary>
/// Represents a HTTP Expires header that contains the date/time after which the response is considered stale.
/// </summary>
public class ExpiresHeaderValue
{
    private readonly DateTime _expires;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpiresHeaderValue"/> class.
    /// </summary>
    /// <param name="expires">The <see cref="TimeSpan"/> value for when the client-cache expires.</param>
    public ExpiresHeaderValue(TimeSpan expires)
    {
        _expires = DateTime.UtcNow.Add(expires);
    }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return _expires.ToString("R");
    }
}
