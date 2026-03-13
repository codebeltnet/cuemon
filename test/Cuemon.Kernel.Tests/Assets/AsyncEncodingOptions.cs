using System.Threading;
using Cuemon.Text;
using Cuemon.Threading;

namespace Cuemon.Assets
{
    /// <summary>
    /// Specifies options that is related to the <see cref="System.Text.Encoding"/> class.
    /// </summary>
    public sealed class AsyncEncodingOptions : EncodingOptions, IAsyncOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncodingOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="EncodingOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="CancellationToken"/></term>
        ///         <description><c>default</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public AsyncEncodingOptions()
        {
            CancellationToken = default;
        }

        /// <summary>
        /// Gets or sets the cancellation token of an asynchronous operations.
        /// </summary>
        /// <value>The cancellation token of an asynchronous operations.</value>
        public CancellationToken CancellationToken { get; set; }
    }
}
