using System;

namespace Cuemon.AspNetCore.Mvc;
/// <summary>
/// Defines options that is related to <see cref="ICacheableObjectResult"/> operations.
/// </summary>
/// <typeparam name="T">The type of the object to make cacheable.</typeparam>
public interface ITimeBasedObjectResultOptions<T>
{
    /// <summary>
    /// Gets or sets the function delegate that resolves a timestamp from when the specified <typeparamref name="T"/> was first created, expressed as the Coordinated Universal Time (UTC).
    /// </summary>
    /// <value>The function delegate that resolves a timestamp from when the specified <typeparamref name="T"/> was first created.</value>
    Func<T, DateTime> TimestampProvider { get; set; }

    /// <summary>
    /// Gets or sets the function delegate that resolves a timestamp from when the specified <typeparamref name="T"/> was last modified, expressed as the Coordinated Universal Time (UTC).
    /// </summary>
    /// <value>The function delegate that resolves a timestamp from when the specified <typeparamref name="T"/> was last modified.</value>
    Func<T, DateTime> ChangedTimestampProvider { get; set; }
}
