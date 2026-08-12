using System;

namespace Cuemon.Diagnostics;
/// <summary>
/// Provides a generic way to implement a fault resolver that evaluate an exception and provide details about it in a developer friendly way.
/// </summary>
public abstract class FaultHandler<TDescriptor> where TDescriptor : ExceptionDescriptor
{
    private readonly Func<Exception, TDescriptor> _descriptorCallback;
    private readonly Func<Exception, bool> _validatorCallback;

    /// <summary>
    /// Initializes a new instance of the <see cref="FaultResolver"/> class.
    /// </summary>
    /// <param name="validator">The function delegate that evaluates an <see cref="Exception"/>.</param>
    /// <param name="descriptor">The function delegate that provides details about an <see cref="Exception"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="validator"/> cannot be null -or-
    /// <paramref name="descriptor"/> cannot be null.
    /// </exception>
    protected FaultHandler(Func<Exception, bool> validator, Func<Exception, TDescriptor> descriptor)
    {
        Validator.ThrowIfNull(validator);
        Validator.ThrowIfNull(descriptor);
        _validatorCallback = validator;
        _descriptorCallback = descriptor;
    }

    /// <summary>
    /// Attempts to resolve <typeparamref name="TDescriptor"/> from the underlying function delegates.
    /// </summary>
    /// <param name="failure">The <see cref="Exception"/> that caused the current failure.</param>
    /// <param name="descriptor">The resulting <typeparamref name="TDescriptor"/> instance providing the reason of the <paramref name="failure"/>, or <c>default</c>.</param>
    /// <returns><c>true</c> if an instance of <typeparamref name="TDescriptor"/> providing the reason for the <paramref name="failure"/> is available, <c>false</c> otherwise.</returns>
    public bool TryResolveFault(Exception failure, out TDescriptor descriptor)
    {
        descriptor = default;
        if (_validatorCallback(failure))
        {
            descriptor = _descriptorCallback(failure);
            return true;
        }
        return false;
    }
}
