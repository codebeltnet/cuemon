using System;
using System.Net.Http;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http;
using Cuemon.Data.Integrity;
using Microsoft.AspNetCore.Http;

namespace Cuemon.Extensions.AspNetCore.Http;
/// <summary>
/// Extension methods for the <see cref="HttpResponse"/> class.
/// </summary>
public static class HttpResponseExtensions
{
    /// <summary>
    /// Attempts to add or update the necessary HTTP response headers needed to provide entity tag header information.
    /// </summary>
    /// <param name="response">The <see cref="HttpResponse"/> to extend.</param>
    /// <param name="request">An instance of the <see cref="HttpRequest"/> object.</param>
    /// <param name="builder">A <see cref="ChecksumBuilder"/> that represents the integrity of the client.</param>
    /// <param name="isWeak">A value that indicates if this entity-tag header is a weak validator.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="response"/> cannot be null -or-
    /// <paramref name="request"/> cannot be null -or-
    /// <paramref name="builder"/> cannot be null.
    /// </exception>
    public static void AddOrUpdateEntityTagHeader(this HttpResponse response, HttpRequest request, ChecksumBuilder builder, bool isWeak = false)
    {
        Validator.ThrowIfNull(response);
        Decorator.Enclose(response).AddOrUpdateEntityTagHeader(request, builder, isWeak);
    }

    /// <summary>
    /// Attempts to add or update the necessary HTTP response headers needed to provide last-modified information.
    /// </summary>
    /// <param name="response">The <see cref="HttpResponse"/> to extend.</param>
    /// <param name="request">An instance of the <see cref="HttpRequest"/> object.</param>
    /// <param name="lastModified">A value that represents when the resource was either created or last modified.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="response"/> cannot be null -or-
    /// <paramref name="request"/> cannot be null.
    /// </exception>
    public static void AddOrUpdateLastModifiedHeader(this HttpResponse response, HttpRequest request, DateTime lastModified)
    {
        Validator.ThrowIfNull(response);
        Decorator.Enclose(response).AddOrUpdateLastModifiedHeader(request, lastModified);
    }

    /// <summary>
    /// Asynchronously writes a sequence of bytes to the response body stream and advances the current position within this stream by the number of bytes written.    
    /// </summary>
    /// <param name="response">The <see cref="HttpResponse"/> to extend.</param>
    /// <param name="body">The function delegate that resolves the bytes to write.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public static async Task WriteBodyAsync(this HttpResponse response, Func<byte[]> body)
    {
        var bodyContent = body?.Invoke();
        if (bodyContent != null)
        {
            await response.Body.WriteAsync(bodyContent.AsMemory(0, bodyContent.Length)).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Transfers the specified <paramref name="message"/> to the HTTP <paramref name="response"/> pipeline using the <paramref name="transformer"/> delegate.
    /// </summary>
    /// <param name="response">The <see cref="HttpResponse"/> to extend.</param>
    /// <param name="message">The <see cref="HttpResponseMessage"/> to convert into an HTTP equivalent <see cref="HttpResponse"/>.</param>
    /// <param name="transformer">The delegate that converts a <see cref="HttpResponseMessage"/> to an HTTP equivalent <see cref="HttpResponse"/>.</param>
    public static void OnStartingInvokeTransformer(this HttpResponse response, HttpResponseMessage message, Action<HttpResponseMessage, HttpResponse> transformer)
    {
        Validator.ThrowIfNull(message);
        Validator.ThrowIfNull(response);
        response.OnStarting(() =>
        {
            transformer?.Invoke(message, response);
            return Task.CompletedTask;
        });
    }
}
