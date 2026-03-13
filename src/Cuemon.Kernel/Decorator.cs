using System;
using System.Runtime.CompilerServices;

namespace Cuemon
{
    /// <summary>
    /// Provides a way to support hiding of non-common extension methods by enclosing/wrapping an object within the <see cref="IDecorator{T}"/> interface.
    /// </summary>
    /// <remarks>
    /// <para/>The original idea for this class was due to feedback from developers; often they are overwhelmed by vast numbers of extensions methods for various types of various libraries.
    /// <para/>To help reduce the cognitive load inferred from this feedback (and to avoid traditional Utility/Helper classes), these interfaces and classes was added, leaving a sub-convenient way (fully backed by IntelliSense) to use non-common extension methods.
    /// <para/>Pure extension methods should (IMO) be used in the way Microsoft has a paved path to the many NuGet packages complementing the .NET platform and overall follow these guidelines: https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/extension-methods.
    /// <para/>Non-common extension methods could also include those rare cases were you need to support your product infrastructure cross-assembly.
    /// </remarks>
    public static class Decorator
    {
        /// <summary>
        /// Encloses the specified <paramref name="inner"/> so that it can be extended by non-common extension methods.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="inner"/> to wrap for non-common extension methods.</typeparam>
        /// <param name="inner">The object to extend for non-common extension methods.</param>
        /// <param name="throwIfNull"><c>true</c> to throw an <see cref="ArgumentNullException"/> when <paramref name="inner"/> is null; <c>false</c> to allow <paramref name="inner"/> to be null. Default is <c>true</c>.</param>
        /// <returns>An instance of <see cref="Decorator{T}"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="inner"/> cannot be null.
        /// </exception>
        public static Decorator<T> Enclose<T>(T inner, bool throwIfNull = true)
        {
            return new Decorator<T>(inner, throwIfNull);
        }

        /// <summary>
        /// Encloses the specified <paramref name="inner"/> so that it can be extended by non-common extension methods.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="inner"/> to wrap for non-common extension methods.</typeparam>
        /// <param name="inner">The object to extend for non-common extension methods.</param>
        /// <returns>An instance of <see cref="Decorator{T}"/>.</returns>
        /// <remarks>Unlike <see cref="Enclose{T}"/>, this method does not perform a null-check when wrapping the value.</remarks>
        public static Decorator<T> RawEnclose<T>(T inner)
        {
            return Enclose(inner, false);
        }

        /// <summary>
        /// Encloses the specified <paramref name="inner"/> so that it can be extended by both common and non-common extension methods.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="inner"/> to wrap for non-common extension methods.</typeparam>
        /// <param name="inner">The object to extend for non-common extension methods.</param>
        /// <param name="throwIfNull"><c>true</c> to throw an <see cref="ArgumentNullException"/> when <paramref name="inner"/> is null; <c>false</c> to allow <paramref name="inner"/> to be null. Default is <c>true</c>.</param>
        /// <param name="argumentName">The name of the argument from which <paramref name="inner"/> parameter was provided.</param>
        /// <returns>An instance of <see cref="Decorator{T}"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="inner"/> cannot be null.
        /// </exception>
        /// <remarks>This should be used to re-use non-common extension methods from native extension methods without double-validating arguments.</remarks>
        public static Decorator<T> EncloseToExpose<T>(T inner, bool throwIfNull = true, [CallerArgumentExpression(nameof(inner))] string argumentName = null)
        {
            return new Decorator<T>(inner, throwIfNull, argumentName);
        }

        /// <summary>
        /// Syntactic sugar for the rare cases where retrieving properties exposed as methods is a necessity.
        /// </summary>
        /// <typeparam name="T">The type to wrap for non-common extension methods.</typeparam>
        /// <returns>An instance of <see cref="Decorator{T}"/> where the <see cref="Decorator{T}.Inner"/> defaults to <typeparamref name="T"/>.</returns>
        public static Decorator<T> Syntactic<T>()
        {
            return new Decorator<T>();
        }
    }

    /// <summary>
    /// Provides a generic way to support hiding of non-common extension methods by enclosing/wrapping an object within the <see cref="IDecorator{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of the inner wrapped object.</typeparam>
    /// <seealso cref="IDecorator{T}" />
    public class Decorator<T> : IDecorator<T>
    {
        internal Decorator()
        {
            Inner = default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Decorator{T}"/> class.
        /// </summary>
        /// <param name="inner">The object to extend for non-common extension methods.</param>
        /// <param name="throwIfNull"><c>true</c> to throw an <see cref="ArgumentNullException"/> when <paramref name="inner"/> is null; <c>false</c> to allow <paramref name="inner"/> to be null.</param>
        /// <param name="argumentName">The name of the argument from which <paramref name="inner"/> parameter was provided.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="inner"/> cannot be null.
        /// </exception>
        internal Decorator(T inner, bool throwIfNull, string argumentName = null)
        {
            if (throwIfNull) { Validator.ThrowIfNull(inner, paramName: argumentName ?? nameof(inner)); }
            Inner = inner;
            ArgumentName = argumentName;
        }

        /// <summary>
        /// Gets the inner object of this decorator.
        /// </summary>
        /// <value>The inner object of this decorator.</value>
        public T Inner { get; }

        /// <summary>
        /// Gets the name of the argument from which this decorator originates.
        /// </summary>
        /// <value>The name of the argument from which this decorator originates.</value>
        public string ArgumentName { get; }
    }
}
