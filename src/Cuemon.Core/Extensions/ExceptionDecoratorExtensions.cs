using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Collections.Generic;

namespace Cuemon;
/// <summary>
/// Extension methods for the <see cref="Exception"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class ExceptionDecoratorExtensions
{
    /// <summary>
    /// Flattens any inner exceptions from the enclosed <see cref="Exception"/> of the <paramref name="decorator"/> into an <see cref="IEnumerable{T}"/> sequence of exceptions.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns>An empty <see cref="IEnumerable{T}"/> sequence if no inner exception(s) was specified; otherwise any inner exception(s) chained to the enclosed <see cref="Exception"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <remarks>
    /// If any inner exceptions are referenced, this method will iterative flatten them all from the enclosed <see cref="Exception"/> of the <paramref name="decorator"/>.<br/>
    /// Should the enclosed <see cref="Exception"/> of the <paramref name="decorator"/> be of <see cref="AggregateException"/>, the return sequence of this method will be equal to the result of the InnerExceptions property after a call to <see cref="AggregateException.Flatten"/>.
    /// </remarks>
    public static IEnumerable<Exception> Flatten(this IDecorator<Exception> decorator)
    {
        Validator.ThrowIfNull(decorator);
        if (decorator.Inner is AggregateException ae) { return ae.Flatten().InnerExceptions; }
        return Decorator.RawEnclose(decorator.Inner).TraverseWhileNotEmpty(FlattenCallback).Skip(1);
    }

    private static IEnumerable<Exception> FlattenCallback(Exception source)
    {
        if (source is AggregateException ae) { return ae.Flatten().InnerExceptions; }
        return source.InnerException == null ? Enumerable.Empty<Exception>() : Arguments.Yield(source.InnerException);
    }
}
