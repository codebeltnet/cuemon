using System;
using System.Threading;
using Cuemon.Configuration;

namespace Cuemon.Threading
{
    /// <summary>
    /// Specifies options that is related to asynchronous operations.
    /// </summary>
    /// <seealso cref="IParameterObject"/>
    public class AsyncOptions : IAsyncOptions, IParameterObject
    {
        private CancellationToken _cancellationToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="AsyncOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="CancellationToken"/></term>
        ///         <description><c>default</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="CancellationTokenProvider"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public AsyncOptions()
        {
            CancellationToken = CancellationToken.None;
        }

        /// <summary>
        /// Gets or sets the cancellation token of an asynchronous operations.
        /// </summary>
        /// <value>The cancellation token of an asynchronous operations.</value>
        /// <remarks><see cref="CancellationTokenProvider"/> takes precedence when set, meaning that the getter of this property will invoke said mentioned function delegate.</remarks>
        public CancellationToken CancellationToken
        {
            get => CancellationTokenProvider?.Invoke() ?? _cancellationToken;
            set => _cancellationToken = value;
        }

        /// <summary>
        /// Gets or sets the function delegate that is invoked when a <see cref="CancellationToken"/> is requested.
        /// </summary>
        /// <value>The function delegate that is invoked when a <see cref="CancellationToken"/> is requested.</value>
        /// <remarks>This function delegate is meant for edge cases where this instance might be stored as a singleton or similar use case.</remarks>
        public Func<CancellationToken> CancellationTokenProvider { get; set; }
    }
}
