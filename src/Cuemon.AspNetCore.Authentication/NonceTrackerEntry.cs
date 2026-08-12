using System;

namespace Cuemon.AspNetCore.Authentication;
/// <summary>
/// Represents an individual nonce entry in the <see cref="INonceTracker"/>.
/// </summary>
public class NonceTrackerEntry
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NonceTrackerEntry"/> class.
    /// </summary>
    /// <param name="count">The number that should be used only once.</param>
    /// <param name="created">The timestamp from when this entry was created.</param>
    public NonceTrackerEntry(int count, DateTime created)
    {
        Count = count;
        Created = created;
    }

    /// <summary>
    /// Gets the number that should be used only once.
    /// </summary>
    /// <value>The number that should be used only once.</value>
    public int Count { get; }

    /// <summary>
    /// Gets the timestamp from when this entry was created.
    /// </summary>
    /// <value>The timestamp from when this entry was created.</value>
    public DateTime Created { get; }
}
