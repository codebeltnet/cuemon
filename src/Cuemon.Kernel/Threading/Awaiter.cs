using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Cuemon.Threading;
/// <summary>
/// Provides a set of static methods for awaiting asynchronous operations.
/// </summary>
public static class Awaiter
{
    /// <summary>
    /// Repeatedly invokes the specified asynchronous <paramref name="method"/> until it succeeds, cancellation is requested, the configured attempt limit is reached, or the configured <see cref="AsyncRunOptions.Timeout"/> retry window closes.
    /// </summary>
    /// <param name="method">The asynchronous function delegate to execute, returning a <see cref="ConditionalValue"/> indicating success or failure.</param>
    /// <param name="setup">The <see cref="AsyncRunOptions"/> which may be configured.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the successful <see cref="ConditionalValue"/> returned by <paramref name="method"/>, or an unsuccessful value that aggregates caught exceptions when the retry policy completes without success.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="method"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The configured <see cref="AsyncRunOptions"/> are not in a valid state.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="method"/> completed successfully but returned a null <see cref="ConditionalValue"/>.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Cancellation was requested before an attempt began, while a retry delay was pending, or <paramref name="method"/> threw an <see cref="OperationCanceledException"/>.
    /// </exception>
    /// <remarks>
    /// The retry window begins immediately before the initial invocation. The initial invocation always occurs, even when <see cref="AsyncRunOptions.Timeout"/> is <see cref="TimeSpan.Zero"/>.
    /// No new invocation begins after the timeout deadline or once <see cref="AsyncRunOptions.MaximumAttempts"/> is reached. After an unsuccessful attempt or a caught exception, the next delay is capped to the smaller of <see cref="AsyncRunOptions.Delay"/> and the remaining timeout window. When the configured delay exceeds the remaining timeout window, the operation waits out the remainder of the window and completes without starting another invocation. Positive fractional-millisecond retry delays are rounded up to the next whole millisecond when the delay is scheduled.
    /// When <see cref="AsyncRunOptions.Delay"/> is <see cref="TimeSpan.Zero"/>, <see cref="AsyncRunOptions.MaximumAttempts"/> must be configured with a positive value.
    /// <para>
    /// Cancellation is resolved from <see cref="AsyncOptions.CancellationToken"/> immediately before each attempt and immediately before each retry delay.
    /// Because this overload does not pass a <see cref="System.Threading.CancellationToken"/> into <paramref name="method"/>, timeout and cancellation cannot terminate work that is already executing inside the delegate; they only prevent additional retries or delays once the current invocation completes.
    /// </para>
    /// <para>
    /// A completed invocation returning <c>null</c> is treated as a programming error and causes an <see cref="InvalidOperationException"/> without retrying.
    /// Potential exceptions thrown by <paramref name="method"/> are caught and collected. If the operation does not succeed before the retry policy completes, <see cref="UnsuccessfulValue"/> will be conditionally initialized as follows:
    /// 1: No caught exceptions; initialized with the default constructor.
    /// 2: One caught exception; initialized with the caught exception.
    /// 3: Two or more caught exceptions; initialized with an <see cref="AggregateException"/> containing the caught exceptions in encounter order.
    /// </para>
    /// <para>
    /// Timeout does not abort an invocation already in progress. The current invocation is allowed to complete, and a successful result is returned even when it arrives after the timeout deadline.
    /// If an in-flight invocation completes unsuccessfully or throws after the deadline, the retry policy ends without another delay or attempt.
    /// </para>
    /// </remarks>
    public static Task<ConditionalValue> RunUntilSuccessfulOrTimeoutAsync(Func<Task<ConditionalValue>> method, Action<AsyncRunOptions> setup = null)
    {
        Validator.ThrowIfNull(method);
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        return RunUntilSuccessfulOrTimeoutCoreAsync(method, options);
    }

    private static async Task<ConditionalValue> RunUntilSuccessfulOrTimeoutCoreAsync(Func<Task<ConditionalValue>> method, AsyncRunOptions options)
    {
        var startedAt = Stopwatch.GetTimestamp();
        var initialAttempt = true;
        var attemptCount = 0;
        Exception firstException = null;
        List<Exception> exceptions = null;

        while (initialAttempt || !HasReachedTimeout(startedAt, options.Timeout))
        {
            initialAttempt = false;

            var attemptToken = options.CancellationToken;
            attemptToken.ThrowIfCancellationRequested();
            attemptCount++;
            TimeSpan retryDelay;
            var stopAfterDelay = false;

            ConditionalValue conditionalValue;
            try
            {
                conditionalValue = await method().ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is not OperationCanceledException && Patterns.IsRecoverableException(ex))
            {
                CaptureException(ref firstException, ref exceptions, ex);
                if (!TryGetRetryDelay(startedAt, options, attemptCount, out retryDelay, out stopAfterDelay)) { break; }
                await DelayAsync(options, retryDelay).ConfigureAwait(false);
                if (stopAfterDelay) { break; }
                continue;
            }

            if (conditionalValue == null) { throw new InvalidOperationException("The specified delegate returned a null ConditionalValue."); }
            if (conditionalValue.Succeeded) { return conditionalValue; }

            if (!TryGetRetryDelay(startedAt, options, attemptCount, out retryDelay, out stopAfterDelay)) { break; }
            await DelayAsync(options, retryDelay).ConfigureAwait(false);
            if (stopAfterDelay) { break; }
        }

        return GetUnsuccessfulValue(firstException, exceptions);
    }

    private static bool HasReachedTimeout(long startedAt, TimeSpan timeout)
    {
        return GetElapsedTime(startedAt) >= timeout;
    }

    private static bool TryGetRetryDelay(long startedAt, AsyncRunOptions options, int attemptCount, out TimeSpan delay, out bool stopAfterDelay)
    {
        stopAfterDelay = false;

        if (options.MaximumAttempts > 0 && attemptCount >= options.MaximumAttempts)
        {
            delay = TimeSpan.Zero;
            return false;
        }

        var remaining = options.Timeout - GetElapsedTime(startedAt);
        if (remaining <= TimeSpan.Zero)
        {
            delay = TimeSpan.Zero;
            return false;
        }

        if (options.Delay > remaining)
        {
            delay = remaining;
            stopAfterDelay = true;
            return true;
        }

        delay = options.Delay;
        return true;
    }

    private static TimeSpan GetElapsedTime(long startedAt)
    {
#if NET9_0_OR_GREATER
        return Stopwatch.GetElapsedTime(startedAt);
#else
        var elapsedTimestamp = Stopwatch.GetTimestamp() - startedAt;

        var wholeSeconds = elapsedTimestamp / Stopwatch.Frequency;

        var remainingTimestamp = elapsedTimestamp % Stopwatch.Frequency;

        var elapsedTicks =
            (wholeSeconds * TimeSpan.TicksPerSecond) +
            ((remainingTimestamp * TimeSpan.TicksPerSecond) /
             Stopwatch.Frequency);

        return TimeSpan.FromTicks(elapsedTicks);
#endif
    }

    private static Task DelayAsync(AsyncRunOptions options, TimeSpan delay)
    {
        if (delay == TimeSpan.Zero) { return Task.CompletedTask; }

        var delayToken = options.CancellationToken;
        delayToken.ThrowIfCancellationRequested();
        return Task.Delay(NormalizeDelay(delay), delayToken);
    }

    private static TimeSpan NormalizeDelay(TimeSpan delay)
    {
        var remainder = delay.Ticks % TimeSpan.TicksPerMillisecond;
        if (remainder == 0) { return delay; }

        // Task.Delay uses whole-millisecond resolution; round up so capped retry windows do not collapse into zero-length delays.
        var adjustment = TimeSpan.TicksPerMillisecond - remainder;
        return delay.Ticks > TimeSpan.MaxValue.Ticks - adjustment
            ? TimeSpan.MaxValue
            : TimeSpan.FromTicks(delay.Ticks + adjustment);
    }

    private static void CaptureException(ref Exception firstException, ref List<Exception> exceptions, Exception exception)
    {
        if (firstException == null)
        {
            firstException = exception;
            return;
        }

        if (exceptions == null)
        {
            exceptions = new List<Exception>
            {
                firstException,
                exception
            };
            return;
        }

        exceptions.Add(exception);
    }

    private static ConditionalValue GetUnsuccessfulValue(Exception firstException, List<Exception> exceptions)
    {
        if (exceptions != null) { return new UnsuccessfulValue(new AggregateException(exceptions)); }
        if (firstException != null) { return new UnsuccessfulValue(firstException); }
        return new UnsuccessfulValue();
    }
}
