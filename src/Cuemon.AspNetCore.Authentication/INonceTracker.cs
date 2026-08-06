using System;

namespace Cuemon.AspNetCore.Authentication;
/// <summary>
/// Represents tracking of server-generated nonce values.
/// </summary>
/// <seealso cref="IDisposable" />
public interface INonceTracker
{
    /// <summary>
    /// Attempts to get the <see cref="NonceTrackerEntry"/> associated with the specified <paramref name="nonce"/> from the tracker.
    /// </summary>
    /// <param name="nonce">The unique identifier of the tracker.</param>
    /// <param name="entry">When this method returns, contains the entry associated with the specified <paramref name="nonce"/>, or <c>null</c> if the operation failed.</param>
    /// <returns><c>true</c> if the <paramref name="entry"/> was found in the tracker; otherwise, <c>false</c>.</returns>
    bool TryGetEntry(string nonce, out NonceTrackerEntry entry);

    /// <summary>
    /// Attempts to insert a <see cref="NonceTrackerEntry"/> into the tracker.
    /// </summary>
    /// <param name="nonce">The unique identifier of the tracker.</param>
    /// <param name="count">The number or bit string that should be used only once.</param>
    /// <returns><c>true</c> if insertion succeeded; otherwise, <c>false</c> when there is already an entry in the tracker with the same key.</returns>
    bool TryAddEntry(string nonce, int count);

    /// <summary>
    /// Attempts to remove an entry from the tracker.
    /// </summary>
    /// <param name="nonce">The unique identifier of the tracker.</param>
    /// <returns><c>true</c> if the entry is removed from the tracker; otherwise, <c>false</c>.</returns>
    bool TryRemoveEntry(string nonce);
}
