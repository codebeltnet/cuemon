using System;
using Cuemon.Configuration;

namespace Cuemon.AspNetCore.Mvc;
/// <summary>
/// Specifies options that is related to the <see cref="ICacheableObjectResult"/> interface.
/// </summary>
/// <seealso cref="CacheableFactory"/>
/// <seealso cref="IValidatableParameterObject"/>
public class TimeBasedObjectResultOptions<T> : ITimeBasedObjectResultOptions<T>, IValidatableParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeBasedObjectResultOptions{T}"/> class.
    /// </summary>
    public TimeBasedObjectResultOptions()
    {
    }

    /// <summary>
    /// Gets or sets the function delegate that resolves a timestamp from when the specified <typeparamref name="T"/> was first created, expressed as the Coordinated Universal Time (UTC).
    /// </summary>
    /// <value>The function delegate that resolves a timestamp from when the specified <typeparamref name="T"/> was first created.</value>
    public Func<T, DateTime> TimestampProvider { get; set; }

    /// <summary>
    /// Gets or sets the function delegate that resolves a timestamp from when the specified <typeparamref name="T"/> was last modified, expressed as the Coordinated Universal Time (UTC).
    /// </summary>
    /// <value>The function delegate that resolves a timestamp from when the specified <typeparamref name="T"/> was last modified.</value>
    public Func<T, DateTime> ChangedTimestampProvider { get; set; }

    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <see cref="TimestampProvider"/> cannot be null.
    /// </exception>
    /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
    public void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(TimestampProvider == null);
    }
}
