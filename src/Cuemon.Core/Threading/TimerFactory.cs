using System;
using System.Threading;

namespace Cuemon.Threading;
/// <summary>
/// Provides access to factory methods for creating and configuring <see cref="Timer"/> instances.
/// </summary>
public static class TimerFactory
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Timer"/> class that suppress capturing the ExecutionContext.
    /// </summary>
    /// <param name="callback">A delegate representing a method to be executed.</param>
    /// <param name="state">An object containing information to be used by the callback method, or <c>null</c>.</param>
    /// <param name="dueTime">The amount of time to delay before the callback is invoked. Specify <see cref="Timeout.InfiniteTimeSpan"/> to prevent the timer from starting. Specify <see cref="TimeSpan.Zero"/> to start the timer immediately.</param>
    /// <param name="period">The time interval between invocations of callback. Specify <see cref="Timeout.InfiniteTimeSpan"/> to disable periodic signaling.</param>
    /// <returns>A new <see cref="Timer"/> instance that suppress capturing the ExecutionContext.</returns>
    /// <remarks>Used by Microsoft internally in various scenarios: https://github.com/dotnet/runtime/blob/master/src/libraries/Common/src/Extensions/NonCapturingTimer/NonCapturingTimer.cs</remarks>
    public static Timer CreateNonCapturingTimer(TimerCallback callback, object state, TimeSpan dueTime, TimeSpan period)
    {
        Validator.ThrowIfNull(callback);
        var restoreFlow = false;
        try
        {
            if (!ExecutionContext.IsFlowSuppressed())
            {
                ExecutionContext.SuppressFlow();
                restoreFlow = true;
            }
            return new Timer(callback, state, dueTime, period);
        }
        finally
        {
            if (restoreFlow)
            {
                ExecutionContext.RestoreFlow();
            }
        }
    }
}
