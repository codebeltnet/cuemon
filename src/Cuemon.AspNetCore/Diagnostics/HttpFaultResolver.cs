using System;
using Cuemon.Diagnostics;

namespace Cuemon.AspNetCore.Diagnostics;
/// <summary>
/// Provides a way to evaluate an exception and provide details about it in a developer friendly way, optimized for open- and otherwise public application programming interfaces (API).
/// </summary>
public class HttpFaultResolver : FaultHandler<HttpExceptionDescriptor>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpFaultResolver"/> class.
    /// </summary>
    /// <param name="validator">The function delegate that evaluates an <see cref="Exception"/>.</param>
    /// <param name="descriptor">The function delegate that provides details about an <see cref="Exception"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="validator"/> cannot be null -or-
    /// <paramref name="descriptor"/> cannot be null.
    /// </exception>
    public HttpFaultResolver(Func<Exception, bool> validator, Func<Exception, HttpExceptionDescriptor> descriptor) : base(validator, descriptor)
    {
    }
}
