using System;
using Cuemon.Configuration;

namespace Cuemon.AspNetCore.Mvc;
/// <summary>
/// Specifies options that is related to the <see cref="ICacheableObjectResult"/> interface.
/// </summary>
/// <seealso cref="CacheableFactory"/>
/// <seealso cref="IValidatableParameterObject"/>
public class ContentBasedObjectResultOptions<T> : IContentBasedObjectResultOptions<T>, IValidatableParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContentBasedObjectResultOptions{T}"/> class.
    /// </summary>
    public ContentBasedObjectResultOptions()
    {
    }

    /// <summary>
    /// Gets or sets the function delegate that resolves a checksum defining the data integrity of the specified <typeparamref name="T"/>.
    /// </summary>
    /// <value>The function delegate that resolves a checksum defining the data integrity of the specified <typeparamref name="T"/>.</value>
    public Func<T, byte[]> ChecksumProvider { get; set; }

    /// <summary>
    /// Gets or sets the function delegate that resolves a value hinting whether the specified <see cref="ChecksumProvider"/> resembles a weak or a strong checksum strength.
    /// </summary>
    /// <value>The function delegate that resolves a value hinting whether the specified <see cref="ChecksumProvider"/> resembles a weak or a strong checksum strength.</value>
    public Func<T, bool> WeakChecksumProvider { get; set; }

    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <see cref="ChecksumProvider"/> cannot be null.
    /// </exception>
    /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
    public void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(ChecksumProvider == null);
    }
}
