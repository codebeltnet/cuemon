using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Cuemon.Threading
{
    /// <summary>
    /// Provides a set of static methods for awaiting asynchronous operations.
    /// </summary>
    public static class Awaiter
    {
            /// <summary>
            /// Repeatedly invokes the specified asynchronous <paramref name="method"/> until it succeeds or the configured <see cref="AsyncRunOptions.Timeout"/> is reached.
            /// </summary>
            /// <param name="method">The asynchronous function delegate to execute, returning a <see cref="ConditionalValue"/> indicating success or failure.</param>
            /// <param name="setup">The <see cref="AsyncRunOptions"/> which may be configured.</param>
            /// <returns>
            /// A task that represents the asynchronous operation. The task result contains the <see cref="ConditionalValue"/> returned by the last invocation of <paramref name="method"/>, or an unsuccessful value if the timeout is reached.
            /// </returns>
            /// <remarks>
            /// The <paramref name="method"/> is invoked repeatedly with a delay specified by <see cref="AsyncRunOptions.Delay"/> until it returns a successful <see cref="ConditionalValue"/> or the timeout specified by <see cref="AsyncRunOptions.Timeout"/> is reached.
            /// <br/>
            /// Potential exceptions thrown by <paramref name="method"/> are caught and collected. If the operation does not succeed before the timeout, <see cref="UnsuccessfulValue"/> will be conditionally initialized:<br/>
            /// 1: No caught exceptions; initialized with default constructor,<br/>
            /// 2: One caught exception; initialized with caught exception,<br/>
            /// 3: Two or more exceptions; initialized with <see cref="AggregateException"/> containing all exceptions.
            /// </remarks>
        public static async Task<ConditionalValue> RunUntilSuccessfulOrTimeoutAsync(Func<Task<ConditionalValue>> method, Action<AsyncRunOptions> setup = null)
        {
            var options = Patterns.Configure(setup);
            var stopwatch = Stopwatch.StartNew();
            var exceptions = new List<Exception>();

            ConditionalValue conditionalValue = null;
            while (stopwatch.Elapsed <= options.Timeout)
            {
                try
                {
                    options.CancellationToken.ThrowIfCancellationRequested();
                    conditionalValue = await method().ConfigureAwait(false);
                    if (conditionalValue.Succeeded) { break; }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }

                await Task.Delay(options.Delay).ConfigureAwait(false);
            }

            return (conditionalValue?.Succeeded ?? false)
                ? conditionalValue 
                : await GetUnsuccessfulValue(exceptions).ConfigureAwait(false);
        }

        private static Task<ConditionalValue> GetUnsuccessfulValue(IList<Exception> exceptions)
        {
            if (exceptions.Count == 0) { return Task.FromResult<ConditionalValue>(new UnsuccessfulValue()); }
            if (exceptions.Count == 1) { return Task.FromResult<ConditionalValue>(new UnsuccessfulValue(exceptions.Single())); }
            return Task.FromResult<ConditionalValue>(new UnsuccessfulValue(new AggregateException(exceptions)));
        }
    }
}
