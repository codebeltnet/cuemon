using System;
using System.Diagnostics;
using System.Text;

namespace Cuemon.Diagnostics;
/// <summary>
/// Represents a profiler that is optimized for time measuring operations.
/// </summary>
/// <seealso cref="Profiler" />
public class TimeMeasureProfiler : Profiler
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeMeasureProfiler"/> class.
    /// </summary>
    public TimeMeasureProfiler()
    {
        Timer = new Stopwatch();
    }

    /// <summary>
    /// Gets the actual timer of this profiler.
    /// </summary>
    /// <value>The actual timer of this profiler.</value>
    public Stopwatch Timer { get; }

    /// <summary>
    /// Gets the total elapsed time measured by this profiler.
    /// </summary>
    /// <value>A read-only <see cref="TimeSpan"/> representing the total elapsed time measured by this profiler.</value>
    public TimeSpan Elapsed => Timer.Elapsed;

    /// <summary>
    /// Gets a value indicating whether this time measuring profiler is still running.
    /// </summary>
    /// <value><c>true</c> if this time measuring profiler is still running; otherwise, <c>false</c>.</value>
    public bool IsRunning => Timer.IsRunning;

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        var result = new StringBuilder(FormattableString.Invariant($"{Member} took {Elapsed.Hours:D2}:{Elapsed.Minutes:D2}:{Elapsed.Seconds:D2}.{Elapsed.Milliseconds:D3} to execute."));
        if (Data.Count > 0)
        {
            result.Append(" Parameters: { ");
            result.Append(DelimitedString.Create(Data, o =>
            {
                o.Delimiter = ", ";
                o.StringConverter = pair => FormattableString.Invariant($"{pair.Key}={Generate.ObjectPortrayal(pair.Value)}");
            }));
            result.Append(" }");
        }
        return result.ToString();
    }
}

/// <summary>
/// Represents a profiler that is optimized for time measuring operations that provides a return value. This class cannot be inherited.
/// </summary>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <seealso cref="TimeMeasureProfiler" />
public sealed class TimeMeasureProfiler<TResult> : TimeMeasureProfiler
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeMeasureProfiler{TResult}"/> class.
    /// </summary>
    internal TimeMeasureProfiler()
    {
    }

    /// <summary>
    /// Gets or sets the result of a time measuring operation that returned a value.
    /// </summary>
    /// <value>The result of a time measuring operation that returned a value.</value>
    public TResult Result { get; set; }
}
