using System;

namespace Cuemon.Diagnostics;
/// <summary>
/// Provides a way to evaluate an exception and provide details about it in a developer friendly way.
/// </summary>
public class FaultResolver : FaultHandler<ExceptionDescriptor>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FaultResolver"/> class.
    /// </summary>
    /// <param name="validator">The function delegate that evaluates an <see cref="Exception"/>.</param>
    /// <param name="descriptor">The function delegate that provides details about an <see cref="Exception"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="validator"/> cannot be null -or-
    /// <paramref name="descriptor"/> cannot be null.
    /// </exception>
    public FaultResolver(Func<Exception, bool> validator, Func<Exception, ExceptionDescriptor> descriptor) : base(validator, descriptor)
    {
    }
}
