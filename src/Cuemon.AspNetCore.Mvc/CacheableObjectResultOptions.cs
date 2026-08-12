using System;
using Cuemon.Configuration;

namespace Cuemon.AspNetCore.Mvc;
/// <summary>
/// Specifies options that is related to the <see cref="ICacheableObjectResult"/> interface.
/// </summary>
/// <seealso cref="CacheableFactory"/>
/// <seealso cref="IValidatableParameterObject"/>
public class CacheableObjectResultOptions<T> : IContentBasedObjectResultOptions<T>, ITimeBasedObjectResultOptions<T>, IValidatableParameterObject
{
    private readonly TimeBasedObjectResultOptions<T> _timeBasedOptions;
    private readonly ContentBasedObjectResultOptions<T> _contentBasedOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheableObjectResultOptions{T}"/> class.
    /// </summary>
    public CacheableObjectResultOptions()
    {
        _contentBasedOptions = new ContentBasedObjectResultOptions<T>();
        _timeBasedOptions = new TimeBasedObjectResultOptions<T>();
    }

    /// <summary>
    /// Gets or sets the function delegate that resolves a checksum defining the data integrity of the specified <typeparamref name="T"/>.
    /// </summary>
    /// <value>The function delegate that resolves a checksum defining the data integrity of the specified <typeparamref name="T"/>.</value>
    public Func<T, byte[]> ChecksumProvider
    {
        get => _contentBasedOptions.ChecksumProvider;
        set => _contentBasedOptions.ChecksumProvider = value;
    }


    /// <summary>
    /// Gets or sets the function delegate that resolves a value hinting whether the specified <see cref="ChecksumProvider"/> resembles a weak or a strong checksum strength.
    /// </summary>
    /// <value>The function delegate that resolves a value hinting whether the specified <see cref="ChecksumProvider"/> resembles a weak or a strong checksum strength.</value>
    public Func<T, bool> WeakChecksumProvider
    {
        get => _contentBasedOptions.WeakChecksumProvider;
        set => _contentBasedOptions.WeakChecksumProvider = value;
    }

    /// <summary>
    /// Gets or sets the function delegate that resolves a timestamp from when the specified <typeparamref name="T"/> was first created, expressed as the Coordinated Universal Time (UTC).
    /// </summary>
    /// <value>The function delegate that resolves a timestamp from when the specified <typeparamref name="T"/> was first created.</value>
    public Func<T, DateTime> TimestampProvider
    {
        get => _timeBasedOptions.TimestampProvider;
        set => _timeBasedOptions.TimestampProvider = value;
    }

    /// <summary>
    /// Gets or sets the function delegate that resolves a timestamp from when the specified <typeparamref name="T"/> was last modified, expressed as the Coordinated Universal Time (UTC).
    /// </summary>
    /// <value>The function delegate that resolves a timestamp from when the specified <typeparamref name="T"/> was last modified.</value>
    public Func<T, DateTime> ChangedTimestampProvider
    {
        get => _timeBasedOptions.ChangedTimestampProvider;
        set => _timeBasedOptions.ChangedTimestampProvider = value;
    }

    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <see cref="ChecksumProvider"/> cannot be null - or -
    /// <see cref="TimestampProvider"/> cannot be null.
    /// </exception>
    /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
    public void ValidateOptions()
    {
        _contentBasedOptions.ValidateOptions();
        _timeBasedOptions.ValidateOptions();
    }
}
