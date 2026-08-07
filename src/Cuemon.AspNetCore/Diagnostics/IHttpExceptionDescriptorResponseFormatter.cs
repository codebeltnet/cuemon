using System.Collections.Generic;

namespace Cuemon.AspNetCore.Diagnostics;
/// <summary>
/// Defines a way to support content negotiation for exceptions in the application.
/// </summary>
public interface IHttpExceptionDescriptorResponseFormatter
{
    /// <summary>
    /// Gets the collection of <see cref="HttpExceptionDescriptorResponseHandler"/> instances.
    /// </summary>
    /// <value>The collection of <see cref="HttpExceptionDescriptorResponseHandler"/> instances.</value>
    ICollection<HttpExceptionDescriptorResponseHandler> ExceptionDescriptorHandlers { get; }
}
