using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Threading
{
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
        /// Cancellation was requested before an attempt began, while an attempt was running, or while a retry delay was pending.
        /// </exception>
        /// <remarks>
        /// The retry window begins immediately before the initial invocation. The initial invocation always occurs, even when <see cref="AsyncRunOptions.Timeout"/> is <see cref="TimeSpan.Zero"/>.
        /// No new invocation begins after the timeout deadline. After an unsuccessful attempt or a caught exception, the next delay is capped to the smaller of <see cref="AsyncRunOptions.Delay"/> and the remaining timeout window.
        /// When <see cref="AsyncRunOptions.Delay"/> is <see cref="TimeSpan.Zero"/>, <see cref="AsyncRunOptions.MaximumAttempts"/> must be configured with a positive value.
        /// <para>
        /// Cancellation is resolved from <see cref="AsyncOptions.CancellationToken"/> before each attempt and before each retry delay. Because this overload does not pass a <see cref="CancellationToken"/> into <paramref name="method"/>, cancellation cannot cooperatively stop work already executing inside the delegate; it only prevents further retries or delays once the current invocation completes.
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
            return RunUntilSuccessfulOrTimeoutCoreAsync(method, static (callback, _) => callback(), options);
        }

        /// <summary>
        /// Repeatedly invokes the specified asynchronous <paramref name="method"/> until it succeeds, cancellation is requested, the configured attempt limit is reached, or the configured <see cref="AsyncRunOptions.Timeout"/> retry window closes.
        /// </summary>
        /// <param name="method">The asynchronous function delegate to execute, receiving the cancellation token resolved from the configured <see cref="AsyncRunOptions"/> and returning a <see cref="ConditionalValue"/> indicating success or failure.</param>
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
        /// Cancellation was requested before an attempt began, while an attempt was running, or while a retry delay was pending.
        /// </exception>
        /// <remarks>
        /// The retry window begins immediately before the initial invocation. The initial invocation always occurs, even when <see cref="AsyncRunOptions.Timeout"/> is <see cref="TimeSpan.Zero"/>.
        /// No new invocation begins after the timeout deadline. After an unsuccessful attempt or a caught exception, the next delay is capped to the smaller of <see cref="AsyncRunOptions.Delay"/> and the remaining timeout window.
        /// When <see cref="AsyncRunOptions.Delay"/> is <see cref="TimeSpan.Zero"/>, <see cref="AsyncRunOptions.MaximumAttempts"/> must be configured with a positive value.
        /// <para>
        /// The cancellation token is resolved from <see cref="AsyncOptions.CancellationToken"/> immediately before each attempt and passed unchanged to <paramref name="method"/> for that attempt. The token is resolved again immediately before each retry delay. This enables cooperative cancellation without requiring callers to supply a second token.
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
        /// If an in-flight invocation completes unsuccessfully or throws after the deadline, the retry policy ends without another delay or attempt. Underlying work may still continue if the delegate ignores cancellation.
        /// </para>
        /// </remarks>
        public static Task<ConditionalValue> RunUntilSuccessfulOrTimeoutAsync(Func<CancellationToken, Task<ConditionalValue>> method, Action<AsyncRunOptions> setup = null)
        {
            Validator.ThrowIfNull(method);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            return RunUntilSuccessfulOrTimeoutCoreAsync(method, static (callback, cancellationToken) => callback(cancellationToken), options);
        }

        private static async Task<ConditionalValue> RunUntilSuccessfulOrTimeoutCoreAsync<TState>(TState state, Func<TState, CancellationToken, Task<ConditionalValue>> callback, AsyncRunOptions options)
        {
            var deadline = options.TimeProvider.GetUtcNow().Add(options.Timeout);
            var attemptCount = 0;
            Exception firstException = null;
            List<Exception> exceptions = null;

            while (true)
            {
                if (attemptCount > 0 && HasReachedTimeout(options, deadline)) { break; }

                var attemptToken = options.CancellationToken;
                attemptToken.ThrowIfCancellationRequested();
                attemptCount++;
                TimeSpan retryDelay;

                ConditionalValue conditionalValue;
                try
                {
                    conditionalValue = await callback(state, attemptToken).ConfigureAwait(false);
                }
                catch (Exception ex) when (Patterns.IsRecoverableException(ex) && ex is not OperationCanceledException)
                {
                    attemptToken.ThrowIfCancellationRequested();
                    CaptureException(ref firstException, ref exceptions, ex);
                    if (!TryGetRetryDelay(options, deadline, attemptCount, out retryDelay)) { break; }
                    await DelayAsync(options, retryDelay).ConfigureAwait(false);
                    continue;
                }

                if (conditionalValue == null) { throw new InvalidOperationException("The specified delegate returned a null ConditionalValue."); }

                attemptToken.ThrowIfCancellationRequested();
                if (conditionalValue.Succeeded) { return conditionalValue; }

                if (!TryGetRetryDelay(options, deadline, attemptCount, out retryDelay)) { break; }
                await DelayAsync(options, retryDelay).ConfigureAwait(false);
            }

            return GetUnsuccessfulValue(firstException, exceptions);
        }

        private static bool HasReachedTimeout(AsyncRunOptions options, DateTimeOffset deadline)
        {
            return options.TimeProvider.GetUtcNow() >= deadline;
        }

        private static bool TryGetRetryDelay(AsyncRunOptions options, DateTimeOffset deadline, int attemptCount, out TimeSpan delay)
        {
            delay = TimeSpan.Zero;
            if (options.MaximumAttempts.HasValue && attemptCount >= options.MaximumAttempts.Value) { return false; }
            var remaining = deadline - options.TimeProvider.GetUtcNow();
            if (remaining <= TimeSpan.Zero) { return false; }
            delay = options.Delay <= remaining ? options.Delay : remaining;
            return true;
        }

        private static async Task DelayAsync(AsyncRunOptions options, TimeSpan delay)
        {
            if (delay == TimeSpan.Zero) { return; }

            var delayToken = options.CancellationToken;
            delayToken.ThrowIfCancellationRequested();

            if (ReferenceEquals(options.TimeProvider, TimeProvider.System))
            {
                await Task.Delay(delay, delayToken).ConfigureAwait(false);
            }
            else
            {
                var delayCompletion = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
                using (var timer = options.TimeProvider.CreateTimer(static state =>
                {
                    var completion = (TaskCompletionSource<object>)state;
                    completion.TrySetResult(null);
                }, delayCompletion, delay, Timeout.InfiniteTimeSpan))
                {
                    var completedTask = await Task.WhenAny(delayCompletion.Task, Task.Delay(Timeout.Infinite, delayToken)).ConfigureAwait(false);
                    await completedTask.ConfigureAwait(false);
                }
            }
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
}
