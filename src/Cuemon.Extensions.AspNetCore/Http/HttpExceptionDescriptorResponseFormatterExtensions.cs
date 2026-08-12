using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.AspNetCore.Diagnostics;

namespace Cuemon.Extensions.AspNetCore.Http;
/// <summary>
/// Extension methods for the <see cref="IHttpExceptionDescriptorResponseFormatter"/> interface.
/// </summary>
public static class HttpExceptionDescriptorResponseFormatterExtensions
{
    /// <summary>
    /// Projects each element of <see cref="IHttpExceptionDescriptorResponseFormatter.ExceptionDescriptorHandlers"/> from the specified <paramref name="formatters"/> into one sequence.
    /// </summary>
    /// <param name="formatters">The sequence of <see cref="IHttpExceptionDescriptorResponseFormatter"/> to extend.</param>
    /// <returns>A sequence of <see cref="HttpExceptionDescriptorResponseHandler"/> from the specified <paramref name="formatters"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="formatters"/> cannot be null.
    /// </exception>
    public static IEnumerable<HttpExceptionDescriptorResponseHandler> SelectExceptionDescriptorHandlers(this IEnumerable<IHttpExceptionDescriptorResponseFormatter> formatters)
    {
        Validator.ThrowIfNull(formatters);
        return formatters.SelectMany(formatter => formatter.ExceptionDescriptorHandlers);
    }
}
