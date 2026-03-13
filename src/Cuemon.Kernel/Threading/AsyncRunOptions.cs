using System;

namespace Cuemon.Threading
{
    /// <summary>
    /// Provides options that are related to asynchronous run operations.
    /// </summary>
    /// <seealso cref="AsyncOptions"/>
    public class AsyncRunOptions : AsyncOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRunOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="AsyncRunOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Timeout"/></term>
        ///         <description><c>00:00:05</c> (5 seconds)</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Delay"/></term>
        ///         <description><c>00:00:00.1000000</c> (100 milliseconds)</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public AsyncRunOptions()
        {
            Timeout = TimeSpan.FromSeconds(5);
            Delay = TimeSpan.FromMilliseconds(100);
        }

        /// <summary>
        /// Gets or sets the timeout for the asynchronous operation.
        /// </summary>
        /// <value>The timeout for the asynchronous operation. The default is 5 seconds.</value>
        public TimeSpan Timeout { get; set; }
        
        /// <summary>
        /// Gets or sets the delay between asynchronous operation attempts.
        /// </summary>
        /// <value>The delay between asynchronous operation attempts. The default is 100 milliseconds.</value>
        public TimeSpan Delay { get; set; }
    }
}
