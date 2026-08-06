using System;
using Cuemon.Configuration;

namespace Cuemon.Threading;
/// <summary>
/// Provides options that are related to asynchronous run operations.
/// </summary>
/// <seealso cref="AsyncOptions"/>
/// <seealso cref="IValidatableParameterObject"/>
public class AsyncRunOptions : AsyncOptions, IValidatableParameterObject
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
    ///     <item>
    ///         <term><see cref="MaximumAttempts"/></term>
    ///         <description><c>0</c> (no explicit attempt limit)</description>
    ///     </item>
    /// </list>
    /// </remarks>
    public AsyncRunOptions()
    {
        Timeout = TimeSpan.FromSeconds(5);
        Delay = TimeSpan.FromMilliseconds(100);
    }

    /// <summary>
    /// Gets or sets the total retry window for the asynchronous operation.
    /// </summary>
    /// <value>The total retry window for the asynchronous operation. The default is 5 seconds.</value>
    /// <remarks>
    /// The retry window begins immediately before the initial invocation. A value of <see cref="TimeSpan.Zero"/> still permits the initial invocation.
    /// The value must not be negative.
    /// </remarks>
    public TimeSpan Timeout { get; set; }

    /// <summary>
    /// Gets or sets the configured delay between unsuccessful asynchronous operation attempts.
    /// </summary>
    /// <value>The configured delay between unsuccessful asynchronous operation attempts. The default is 100 milliseconds.</value>
    /// <remarks>
    /// The effective delay is capped to the remaining <see cref="Timeout"/> window. Positive fractional-millisecond delays are rounded up to the next whole millisecond when the retry delay is scheduled. The value must not be negative.
    /// </remarks>
    public TimeSpan Delay { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of total invocations, including the initial invocation.
    /// </summary>
    /// <value>The maximum number of total invocations. The default is 0.</value>
    /// <remarks>
    /// When this property is 0, retries continue until the operation succeeds, the <see cref="Timeout"/> window closes, or cancellation is requested.
    /// When <see cref="Delay"/> is <see cref="TimeSpan.Zero"/>, this property must be configured with a positive value.
    /// </remarks>
    public int MaximumAttempts { get; set; }

    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <see cref="Timeout"/> or <see cref="Delay"/> is negative.
    /// -or-
    /// <see cref="MaximumAttempts"/> is negative.
    /// -or-
    /// <see cref="Delay"/> is <see cref="TimeSpan.Zero"/> and <see cref="MaximumAttempts"/> is not configured with a positive value.
    /// </exception>
    /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
    public void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(Timeout < TimeSpan.Zero, $"{nameof(Timeout)} cannot be negative.");
        Validator.ThrowIfInvalidState(Delay < TimeSpan.Zero, $"{nameof(Delay)} cannot be negative.");
        Validator.ThrowIfInvalidState(MaximumAttempts < 0, $"{nameof(MaximumAttempts)} cannot be negative.");
        Validator.ThrowIfInvalidState(Delay == TimeSpan.Zero && MaximumAttempts <= 0, $"{nameof(MaximumAttempts)} must be configured with a positive value when {nameof(Delay)} is {nameof(TimeSpan)}.{nameof(TimeSpan.Zero)} to prevent an unbounded retry loop.");
    }
}
