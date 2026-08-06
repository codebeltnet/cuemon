using System;

namespace Cuemon.AspNetCore.Mvc;
/// <summary>
/// Defines options that is related to <see cref="ICacheableObjectResult"/> operations.
/// </summary>
/// <typeparam name="T">The type of the object to make cacheable.</typeparam>
public interface IContentBasedObjectResultOptions<T>
{
    /// <summary>
    /// Gets or sets the function delegate that resolves a checksum defining the data integrity of the specified <typeparamref name="T"/>.
    /// </summary>
    /// <value>The function delegate that resolves a checksum defining the data integrity of the specified <typeparamref name="T"/>.</value>
    Func<T, byte[]> ChecksumProvider { get; set; }

    /// <summary>
    /// Gets or sets the function delegate that resolves a value hinting whether the specified <see cref="ChecksumProvider"/> resembles a weak or a strong checksum strength.
    /// </summary>
    /// <value>The function delegate that resolves a value hinting whether the specified <see cref="ChecksumProvider"/> resembles a weak or a strong checksum strength.</value>
    Func<T, bool> WeakChecksumProvider { get; set; }
}
