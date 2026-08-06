using System;
using System.Linq;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Cuemon.AspNetCore.Diagnostics;
/// <summary>
/// Extension methods for <see cref="HttpExceptionDescriptor"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
public static class HttpExceptionDescriptorDecoratorExtensions
{
    /// <summary>
    /// Converts the specified <see cref="IDecorator{T}"/> of <see cref="HttpExceptionDescriptor"/> to an instance of <see cref="ProblemDetails"/>.
    /// </summary>
    /// <param name="decorator">The decorator that encapsulates the <see cref="HttpExceptionDescriptor"/>.</param>
    /// <param name="sensitivity">The sensitivity details to include in the <see cref="ProblemDetails"/>.</param>
    /// <returns>An instance of <see cref="ProblemDetails"/> that represents the specified <see cref="HttpExceptionDescriptor"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static ProblemDetails ToProblemDetails(this IDecorator<HttpExceptionDescriptor> decorator, FaultSensitivityDetails sensitivity)
    {
        Validator.ThrowIfNull(decorator, out var descriptor);

        var pd = new ProblemDetails()
        {
            Detail = descriptor.Message,
            Status = descriptor.StatusCode,
            Title = descriptor.Code,
            Type = descriptor.HelpLink?.ToString() ?? "about:blank",
            Instance = descriptor.Instance?.OriginalString,
            Extensions =
                {
                    { nameof(HttpExceptionDescriptor.CorrelationId), descriptor.CorrelationId },
                    { nameof(HttpExceptionDescriptor.RequestId), descriptor.RequestId },
                    { nameof(HttpExceptionDescriptor.TraceId), descriptor.TraceId }
                }
        };

        if (sensitivity.HasFlag(FaultSensitivityDetails.Failure))
        {
            pd.Extensions.Add(nameof(FaultSensitivityDetails.Failure), new Failure(descriptor.Failure, sensitivity));
        }

        if (sensitivity.HasFlag(FaultSensitivityDetails.Evidence) && descriptor.Evidence.Any())
        {
            foreach (var evidence in descriptor.Evidence)
            {
                pd.Extensions.Add(evidence.Key, evidence.Value);
            }
        }

        return pd;
    }
}
