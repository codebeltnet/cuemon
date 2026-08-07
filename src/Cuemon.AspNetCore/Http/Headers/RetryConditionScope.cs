namespace Cuemon.AspNetCore.Http.Headers;
/// <summary>
/// Specifies a set of values defining what value to use with a given HTTP header in regards to a retry condition. Recommended value is always <see cref="DeltaSeconds"/> as it does not rely on clock synchronization and is resilient to clock skew between client and server.
/// </summary>
public enum RetryConditionScope
{
    /// <summary>
    /// A non-negative decimal integer indicating the seconds to delay after the response is received.
    /// </summary>
    DeltaSeconds,
    /// <summary>
    /// A date after which to retry.
    /// </summary>
    HttpDate
}
