using System;

namespace Cuemon.Messaging;
/// <summary>
/// Provides a default implementation of the <see cref="ICorrelationToken"/> interface.
/// </summary>
/// <seealso cref="ICorrelationToken" />
public record CorrelationToken : ICorrelationToken
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CorrelationToken"/> class.
    /// </summary>
    /// <param name="correlationId">The string that uniquely identifies a correlation. Default value is a GUID expressed as 32 digits.</param>
    public CorrelationToken(string correlationId = null)
    {
        CorrelationId = correlationId ?? Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// Gets the unique correlation identifier.
    /// </summary>
    /// <value>The unique correlation identifier.</value>
    public string CorrelationId { get; }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return CorrelationId;
    }
}
