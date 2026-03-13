using System.Threading;

namespace Cuemon.Threading
{
    /// <summary>
    /// Defines options that is related to asynchronous operations.
    /// </summary>
    public interface IAsyncOptions
    {
        /// <summary>
        /// Gets or sets the cancellation token of an asynchronous operations.
        /// </summary>
        /// <value>The cancellation token of an asynchronous operations.</value>
        CancellationToken CancellationToken { get; set; }
    }
}
