using System;
using System.Collections.Generic;

namespace Cuemon.Extensions;
/// <summary>
/// Extension methods for the <see cref="Exception"/> class.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Flattens any inner exceptions from the specified <paramref name="exception"/> into an <see cref="IEnumerable{T}"/> sequence of exceptions.
    /// </summary>
    /// <param name="exception">The <see cref="Exception"/> to extend.</param>
    /// <returns>An empty <see cref="IEnumerable{T}"/> sequence if no inner exception(s) was specified; otherwise any inner exception(s) chained to the specified <paramref name="exception"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="exception"/> is null.
    /// </exception>
    /// <remarks>
    /// If any inner exceptions are referenced, this method will iterative flatten them all from the specified <paramref name="exception"/>.<br/>
    /// Should the <paramref name="exception"/> be of the new <see cref="AggregateException"/> introduced with .NET 4.0, the return sequence of this method will be equal to the result of the InnerExceptions property after a call to <see cref="AggregateException.Flatten"/>.
    /// </remarks>
    public static IEnumerable<Exception> Flatten(this Exception exception)
    {
        Validator.ThrowIfNull(exception);
        return Decorator.Enclose(exception).Flatten();
    }
}
