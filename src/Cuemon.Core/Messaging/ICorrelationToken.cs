namespace Cuemon.Messaging;
/// <summary>
/// Provides a Correlation ID (also known as a Request ID) that is a unique identifier which is attached to requests and messages that allow reference to a particular transaction or event chain.
/// </summary>
public interface ICorrelationToken
{
    /// <summary>
    /// Gets the unique correlation identifier.
    /// </summary>
    /// <value>The unique correlation identifier.</value>
    string CorrelationId { get; }
}
