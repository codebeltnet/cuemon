namespace Cuemon.Messaging;
/// <summary>
/// Provides a Request ID that is a unique identifier which is attached to requests and messages that allow reference to a particular transaction.
/// </summary>
public interface IRequestToken
{
    /// <summary>
    /// Gets the unique request identifier.
    /// </summary>
    /// <value>The unique request identifier.</value>
    string RequestId { get; }
}
