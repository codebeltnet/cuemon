using System.Net;

namespace Cuemon.Extensions.Net;
/// <summary>
/// Extension methods for the <see cref="HttpStatusCode"/> enum.
/// </summary>
public static class HttpStatusCodeExtensions
{
    /// <summary>
    /// Determines whether the specified <paramref name="statusCode"/> is within the informational range.
    /// </summary>
    /// <param name="statusCode">The <see cref="HttpStatusCode"/> to evaluate.</param>
    /// <returns><c>true</c> if <see cref="HttpStatusCode"/> was in the <b>Information</b> range (100-199); otherwise, <c>false</c>.</returns>
    public static bool IsInformationStatusCode(this HttpStatusCode statusCode)
    {
        var statusCodeInt = (int)statusCode;
        return (statusCodeInt >= 100 && statusCodeInt <= 199);
    }

    /// <summary>
    /// Determines whether the specified <paramref name="statusCode"/> is within the successful range.
    /// </summary>
    /// <param name="statusCode">The <see cref="HttpStatusCode"/> to evaluate.</param>
    /// <returns><c>true</c> if <see cref="HttpStatusCode"/> was in the <b>Successful</b> range (200-299); otherwise, <c>false</c>.</returns>
    public static bool IsSuccessStatusCode(this HttpStatusCode statusCode)
    {
        var statusCodeInt = (int)statusCode;
        return (statusCodeInt >= 200 && statusCodeInt <= 299);
    }

    /// <summary>
    /// Determines whether the specified <paramref name="statusCode"/> is within the redirecting range.
    /// </summary>
    /// <param name="statusCode">The <see cref="HttpStatusCode"/> to evaluate.</param>
    /// <returns><c>true</c> if <see cref="HttpStatusCode"/> was in the <b>Redirection</b> range (300-399); otherwise, <c>false</c>.</returns>
    public static bool IsRedirectionStatusCode(this HttpStatusCode statusCode)
    {
        var statusCodeInt = (int)statusCode;
        return (statusCodeInt >= 300 && statusCodeInt <= 399);
    }

    /// <summary>
    /// Determines whether the specified <paramref name="statusCode"/> is within the client error related range.
    /// </summary>
    /// <param name="statusCode">The <see cref="HttpStatusCode"/> to evaluate.</param>
    /// <returns><c>true</c> if <see cref="HttpStatusCode"/> was in the <b>Client Error</b> range (400-499); otherwise, <c>false</c>.</returns>
    public static bool IsClientErrorStatusCode(this HttpStatusCode statusCode)
    {
        var statusCodeInt = (int)statusCode;
        return (statusCodeInt >= 400 && statusCodeInt <= 499);
    }

    /// <summary>
    /// Determines whether the specified <paramref name="statusCode"/> is within the server error related range.
    /// </summary>
    /// <param name="statusCode">The <see cref="HttpStatusCode"/> to evaluate.</param>
    /// <returns><c>true</c> if <see cref="HttpStatusCode"/> was in the <b>Server Error</b> range (500-599); otherwise, <c>false</c>.</returns>
    public static bool IsServerErrorStatusCode(this HttpStatusCode statusCode)
    {
        var statusCodeInt = (int)statusCode;
        return (statusCodeInt >= 500 && statusCodeInt <= 599);
    }
}
