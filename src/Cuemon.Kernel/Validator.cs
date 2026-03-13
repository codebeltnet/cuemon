using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Cuemon.Configuration;
using System.Runtime.CompilerServices;
using Cuemon.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// Provides a generic way to validate different types of arguments passed to members.
    /// </summary>
    public sealed class Validator
    {
        private static readonly Validator ExtendedValidator = new();

        /// <summary>
        /// Gets the singleton instance of the Validator functionality allowing for extensions methods like: <c>Validator.ThrowIf.InvalidJsonDocument()</c>.
        /// </summary>
        /// <value>The singleton instance of the Validator functionality.</value>
        public static Validator ThrowIf { get; } = ExtendedValidator;

        /// <summary>
        /// Provides a convenient way to verify a desired state from the provided <paramref name="validator"/> while returning the specified <paramref name="argument"/> unaltered.
        /// </summary>
        /// <typeparam name="T">The type of the object to evaluate.</typeparam>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="validator">The delegate that must throw an <see cref="Exception"/> if the specified <paramref name="argument"/> is not valid.</param>
        /// <returns>The specified <paramref name="argument"/> unaltered.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="validator"/> cannot be null.
        /// </exception>
        /// <remarks>Typically used when nesting calls from a constructor perspective.</remarks>
        public static T CheckParameter<T>(T argument, Action validator)
        {
            ThrowIfNull(validator);
            validator();
            return argument;
        }

        /// <summary>
        /// Provides a convenient way to verify a desired state from the provided <paramref name="validator"/> while returning a result that reflects this.
        /// </summary>
        /// <typeparam name="TResult">The type of the object to return.</typeparam>
        /// <param name="validator">The function delegate that must throw an <see cref="Exception"/> if a desired state for <typeparamref name="TResult"/> cannot be achieved.</param>
        /// <returns>The result of function delegate <paramref name="validator"/>.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="validator"/> cannot be null.
        /// </exception>
        /// <remarks>Typically used when nesting calls from a constructor perspective.</remarks>
        public static TResult CheckParameter<TResult>(Func<TResult> validator)
        {
            ThrowIfNull(validator);
            return validator();
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> results in an instance of invalid <paramref name="options"/>.
        /// </summary>
        /// <typeparam name="TOptions">The type of the object that potentially is implementing the <seealso cref="IValidatableParameterObject"/> interface.</typeparam>
        /// <param name="argument">The delegate that will configure the public read-write properties of <paramref name="options"/>.</param>
        /// <param name="options">The default parameter-less constructed instance of <typeparamref name="TOptions"/> configured with <paramref name="argument"/> delegate.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> failed to configure an instance <paramref name="options"/> in a valid state.
        /// </exception>
        public static void ThrowIfInvalidConfigurator<TOptions>(Action<TOptions> argument, out TOptions options, string message = "Delegate must configure the public read-write properties to be in a valid state.", [CallerArgumentExpression(nameof(argument))] string paramName = null) where TOptions : class, IParameterObject, new()
        {
            options = Patterns.Configure(argument);
            ThrowIfInvalidOptions(options, message, paramName);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> are not in a valid state.
        /// </summary>
        /// <typeparam name="TOptions">The type of the object that potentially is implementing the <seealso cref="IValidatableParameterObject"/> interface.</typeparam>
        /// <param name="argument">The configured options to validate.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="argument"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> are not in a valid state.
        /// </exception>
        /// <remarks><paramref name="message"/> will have the name of the <typeparamref name="TOptions"/> if possible; otherwise Options.</remarks>
        public static void ThrowIfInvalidOptions<TOptions>(TOptions argument, string message = "{0} are not in a valid state.", [CallerArgumentExpression(nameof(argument))] string paramName = null) where TOptions : class, IParameterObject, new()
        {
            ThrowIfNull(argument, paramName);
            try
            {
                if (argument is IPostConfigurableParameterObject postConfigurable) { postConfigurable.PostConfigureOptions(); }
                if (argument is IValidatableParameterObject validatableArgument) { validatableArgument.ValidateOptions(); }
            }
            catch (Exception e)
            {
                if (message?.Equals("{0} are not in a valid state.", StringComparison.Ordinal) ?? false) { message = string.Format(CultureInfo.InvariantCulture, message, Patterns.InvokeOrDefault(() => ToFriendlyTypeName(typeof(TOptions)), "Options")); }
                throw new ArgumentException(message, paramName, e);
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="InvalidOperationException" /> if the specified <paramref name="condition" /> is <c>true</c>.
        /// </summary>
        /// <param name="condition">The value that determines if an <see cref="InvalidOperationException"/> is thrown.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="expression">The <paramref name="condition"/> expressed as a string.</param>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="condition" /> is <c>true</c>.
        /// </exception>
        /// <remarks>This guard should be called when validating the state of an object - not when validating arguments passed to a member.</remarks>
        public static void ThrowIfInvalidState(bool condition, string message = "Operation is not valid due to the current state of the object.", [CallerArgumentExpression(nameof(condition))] string expression = null)
        {
            if (condition) { throw new InvalidOperationException($"{message} (Expression '{expression}')"); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ObjectDisposedException" /> if the specified <paramref name="condition" /> is <c>true</c>.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="instance">The object whose type's full name should be included in any resulting <see cref="ObjectDisposedException" />.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ObjectDisposedException">
        /// The <paramref name="condition" /> is <c>true</c>.
        /// </exception>
        /// <remarks>This guard should be called when performing an operation on a disposed object - not when validating arguments passed to a member.</remarks>
        public static void ThrowIfDisposed(bool condition, object instance, string message = "Cannot access a disposed object.")
        {
            ThrowIfDisposed(condition, instance?.GetType(), message);
        }

        /// <summary>
        /// Validates and throws an <see cref="ObjectDisposedException" /> if the specified <paramref name="condition" /> is <c>true</c>.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="type">The type whose full name should be included in any resulting <see cref="ObjectDisposedException" />.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ObjectDisposedException">
        /// The <paramref name="condition" /> is <c>true</c>.
        /// </exception>
        /// <remarks>This guard should be called when performing an operation on a disposed object - not when validating arguments passed to a member.</remarks>
        public static void ThrowIfDisposed(bool condition, Type type, string message = "Cannot access a disposed object.")
        {
            if (condition) { throw new ObjectDisposedException(type == null ? null : ToFriendlyTypeName(type, true), message); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> is a number.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="styles">A bitwise combination of <see cref="NumberStyles"/> values that indicates the permitted format of <paramref name="argument"/>.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information about <paramref name="argument"/>.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> cannot be a number.
        /// </exception>
        public static void ThrowIfNumber(string argument, NumberStyles styles = NumberStyles.Number, IFormatProvider provider = null, string message = "Value cannot be a number.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (Condition.IsNumeric(argument, styles, provider ?? CultureInfo.InvariantCulture)) { throw new ArgumentException(message, paramName); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> is not a number.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="styles">A bitwise combination of <see cref="NumberStyles"/> values that indicates the permitted format of <paramref name="argument"/>.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information about <paramref name="argument"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> must be a number.
        /// </exception>
        public static void ThrowIfNotNumber(string argument, NumberStyles styles = NumberStyles.Number, IFormatProvider provider = null, string message = "Value must be a number.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (!Condition.IsNumeric(argument, styles, provider ?? CultureInfo.InvariantCulture)) { throw new ArgumentException(message, paramName); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentNullException"/> if the specified <paramref name="argument"/> is null.
        /// </summary>
        /// <typeparam name="T">The type of the inner object denoted by <paramref name="argument"/>.</typeparam>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="inner">The inner object of <paramref name="argument"/>.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="argument"/> cannot be null - or -
        /// <see cref="IDecorator{T}.Inner"/> property of <paramref name="argument"/> cannot be null.
        /// </exception>
        public static void ThrowIfNull<T>(IDecorator<T> argument, out T inner, string message = "Value cannot be null.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            ThrowIfNull(argument, message, argument?.ArgumentName ?? paramName);
            ThrowIfNull(argument!.Inner, message, argument.ArgumentName ?? paramName);
            inner = argument.Inner;
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentNullException"/> if the specified <paramref name="argument"/> is null.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="argument"/> cannot be null.
        /// </exception>
        public static void ThrowIfNull(object argument, string message = "Value cannot be null.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (argument is null) { throw new ArgumentNullException(paramName, message); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException" /> if the specified <paramref name="condition" /> is <c>false</c>.
        /// </summary>
        /// <param name="condition">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="expression">The <paramref name="condition"/> expressed as a string.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="condition" /> is <c>false</c>.
        /// </exception>
        public static void ThrowIfFalse(bool condition, string paramName, string message = "Value is not in a valid state.", [CallerArgumentExpression(nameof(condition))] string expression = null)
        {
            if (Condition.IsFalse(condition)) { throw new ArgumentException($"{message} (Expression '{expression}')", paramName); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException" /> if the specified <paramref name="predicate" /> returns <c>false</c>.
        /// </summary>
        /// <param name="predicate">The function delegate that determines if an <see cref="ArgumentException"/> is thrown.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="expression">The <paramref name="predicate"/> expressed as a string.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="predicate" /> returned <c>false</c>.
        /// </exception>
        public static void ThrowIfFalse(Func<bool> predicate, string paramName, string message = "Value is not in a valid state.", [CallerArgumentExpression(nameof(predicate))] string expression = null)
        {
            if (Condition.IsFalse(predicate?.Invoke() ?? true)) { throw new ArgumentException($"{message} (Expression '{expression}')", paramName); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException" /> if the specified <paramref name="condition" /> is <c>true</c>.
        /// </summary>
        /// <param name="condition">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="expression">The <paramref name="condition"/> expressed as a string.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="condition" /> is <c>true</c>.
        /// </exception>
        public static void ThrowIfTrue(bool condition, string paramName, string message = "Value is not in a valid state.", [CallerArgumentExpression(nameof(condition))] string expression = null)
        {
            if (Condition.IsTrue(condition)) { throw new ArgumentException($"{message} (Expression '{expression}')", paramName); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException" /> if the specified <paramref name="predicate" /> returns <c>true</c>.
        /// </summary>
        /// <param name="predicate">The function delegate that determines if an <see cref="ArgumentException"/> is thrown.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="expression">The <paramref name="predicate"/> expressed as a string.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="predicate" /> returned <c>true</c>.
        /// </exception>
        public static void ThrowIfTrue(Func<bool> predicate, string paramName, string message = "Value is not in a valid state.", [CallerArgumentExpression(nameof(predicate))] string expression = null)
        {
            if (Condition.IsTrue(predicate?.Invoke() ?? false)) { throw new ArgumentException($"{message} (Expression '{expression}')", paramName); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> has no elements.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> contains no elements.
        /// </exception>
        /// <remarks>This method will not throw an exception if <paramref name="argument"/> is null.</remarks>
        public static void ThrowIfSequenceEmpty<T>(IEnumerable<T> argument, string message = "Value contains no elements.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (Condition.IsFalse(argument?.Any() ?? true)) { throw new ArgumentException(message, paramName); }
        }

        /// <summary>
        /// Validates and throws either an <see cref="ArgumentNullException"/> or <see cref="ArgumentException"/> if the specified <paramref name="argument"/> is respectively null or has no elements.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="argument"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> contains no elements.
        /// </exception>
        public static void ThrowIfSequenceNullOrEmpty<T>(IEnumerable<T> argument, string message = "Value is either null or contains no elements.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            ThrowIfNull(argument, message, paramName);
            ThrowIfSequenceEmpty(argument, message, paramName);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> is empty.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> cannot be empty.
        /// </exception>
        /// <remarks>This method will not throw an exception if <paramref name="argument"/> is null.</remarks>
        public static void ThrowIfEmpty(string argument, string message = "Value cannot be empty.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (Condition.IsEmpty(argument)) { throw new ArgumentException(message, paramName); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> consist only of white-space characters.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> cannot consist only of white-space characters.
        /// </exception>
        /// <remarks>This method will not throw an exception if <paramref name="argument"/> is null.</remarks>
        public static void ThrowIfWhiteSpace(string argument, string message = "Value cannot consist only of white-space characters.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (Condition.IsWhiteSpace(argument)) { throw new ArgumentException(message, paramName); }
        }

        /// <summary>
        /// Validates and throws either an <see cref="ArgumentNullException"/> or <see cref="ArgumentException"/> if the specified <paramref name="argument"/> is respectively null or empty.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="message">A message that describes the error to your liking.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="argument"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> cannot be empty.
        /// </exception>
        public static void ThrowIfNullOrEmpty(string argument, string message = null, [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (message == null)
            {
                ThrowIfNull(argument, paramName: paramName);
                ThrowIfEmpty(argument, paramName: paramName);
                return;
            }
            ThrowIfNull(argument, message, paramName);
            ThrowIfEmpty(argument, message, paramName);
        }

        /// <summary>
        /// Validates and throws either an <see cref="ArgumentNullException"/> or <see cref="ArgumentException"/> if the specified <paramref name="argument"/> is respectively null, empty or consist only of white-space characters.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="argument"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        public static void ThrowIfNullOrWhitespace(string argument, string message = null, [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (message == null)
            {
                ThrowIfNullOrEmpty(argument, paramName: paramName);
                ThrowIfWhiteSpace(argument, paramName: paramName);
                return;
            }
            ThrowIfNullOrEmpty(argument, message, paramName);
            ThrowIfWhiteSpace(argument, message, paramName);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> object are of the same instance as the <paramref name="y" /> object.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> and <paramref name="y"/> are of the same instance.
        /// </exception>
        public static void ThrowIfSame<T>(T x, T y, string paramName, string message = null)
        {
            if (Condition.AreSame(x, y)) { throw new ArgumentOutOfRangeException(paramName, FormattableString.Invariant($"{x} <==> {y}"), message ?? FormattableString.Invariant($"Specified arguments {nameof(x)} and {nameof(y)} are of the same instance.")); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> object are not of the same instance as the <paramref name="y" /> object.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> and <paramref name="y"/> are not of the same instance.
        /// </exception>
        public static void ThrowIfNotSame<T>(T x, T y, string paramName, string message = null)
        {
            if (Condition.AreNotSame(x, y)) { throw new ArgumentOutOfRangeException(paramName, FormattableString.Invariant($"{x} <!=> {y}"), message ?? FormattableString.Invariant($"Specified arguments {nameof(x)} and {nameof(y)} are not of the same instance.")); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> object are equal to the <paramref name="y" /> object.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing <paramref name="x"/> and <paramref name="y"/>.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> and <paramref name="y"/> are equal to one another.
        /// </exception>
        public static void ThrowIfEqual<T>(T x, T y, string paramName, IEqualityComparer<T> comparer = null, string message = null)
        {
            if (Condition.AreEqual(x, y, comparer ?? EqualityComparer<T>.Default)) { throw new ArgumentOutOfRangeException(paramName, FormattableString.Invariant($"{x} == {y}"), message ?? FormattableString.Invariant($"Specified arguments {nameof(x)} and {nameof(y)} are equal to one another.")); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> object are not equal to the <paramref name="y" /> object.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing <paramref name="x"/> and <paramref name="y"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> and <paramref name="y"/> are not equal to one another.
        /// </exception>
        public static void ThrowIfNotEqual<T>(T x, T y, string paramName, IEqualityComparer<T> comparer = null, string message = null)
        {
            if (Condition.AreNotEqual(x, y, comparer ?? EqualityComparer<T>.Default)) { throw new ArgumentOutOfRangeException(paramName, FormattableString.Invariant($"{x} != {y}"), message ?? FormattableString.Invariant($"Specified arguments {nameof(x)} and {nameof(y)} are not equal to one another.")); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> is greater than <paramref name="y" />.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> is greater than <paramref name="y"/>.
        /// </exception>
        public static void ThrowIfGreaterThan<T>(T x, T y, string paramName, string message = null) where T : struct, IConvertible
        {
            if (Condition.IsGreaterThan(x, y)) { throw new ArgumentOutOfRangeException(paramName, FormattableString.Invariant($"{x} > {y}"), message ?? FormattableString.Invariant($"Specified arguments {nameof(x)} is greater than {nameof(y)}.")); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> is greater than or equal to <paramref name="y" />.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> is greater than or equal to <paramref name="y"/>.
        /// </exception>
        public static void ThrowIfGreaterThanOrEqual<T>(T x, T y, string paramName, string message = null) where T : struct, IConvertible
        {
            if (Condition.IsGreaterThanOrEqual(x, y)) { throw new ArgumentOutOfRangeException(paramName, FormattableString.Invariant($"{x} >= {y}"), message ?? FormattableString.Invariant($"Specified arguments {nameof(x)} is greater than or equal to {nameof(y)}.")); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> is lower than <paramref name="y" />.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> is lower than <paramref name="y"/>.
        /// </exception>
        public static void ThrowIfLowerThan<T>(T x, T y, string paramName, string message = null) where T : struct, IConvertible
        {
            if (Condition.IsLowerThan(x, y)) { throw new ArgumentOutOfRangeException(paramName, FormattableString.Invariant($"{x} < {y}"), message ?? FormattableString.Invariant($"Specified arguments {nameof(x)} is lower than {nameof(y)}.")); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> is lower than or equal to <paramref name="y" />.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> is lower than or equal to <paramref name="y"/>.
        /// </exception>
        public static void ThrowIfLowerThanOrEqual<T>(T x, T y, string paramName, string message = null) where T : struct, IConvertible
        {
            if (Condition.IsLowerThanOrEqual(x, y)) { throw new ArgumentOutOfRangeException(paramName, FormattableString.Invariant($"{x} <= {y}"), message ?? FormattableString.Invariant($"Specified arguments {nameof(x)} is lower than or equal to {nameof(y)}.")); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> is hexadecimal.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> cannot be hexadecimal.
        /// </exception>
        public static void ThrowIfHex(string argument, string message = "Specified argument cannot be hexadecimal.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (Condition.IsHex(argument)) { throw new ArgumentException(message, paramName); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> is not hexadecimal.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> must be hexadecimal.
        /// </exception>
        public static void ThrowIfNotHex(string argument, string message = "Value must be hexadecimal.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (!Condition.IsHex(argument)) { throw new ArgumentException(message, paramName); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> has the format of an email address.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> cannot be an email address.
        /// </exception>
        public static void ThrowIfEmailAddress(string argument, string message = "Value cannot be an email address.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (Condition.IsEmailAddress(argument)) { throw new ArgumentException(message, paramName); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> does not have the format of an email address.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> must be an email address.
        /// </exception>
        public static void ThrowIfNotEmailAddress(string argument, string message = "Value must be an email address.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (!Condition.IsEmailAddress(argument)) { throw new ArgumentException(message, paramName); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> has the format of a <see cref="Guid"/>.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="format">A bitmask comprised of one or more <see cref="GuidFormats"/> that specify how the GUID parsing is conducted.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> cannot be a <see cref="Guid"/>.
        /// </exception>
        public static void ThrowIfGuid(string argument, GuidFormats format = GuidFormats.B | GuidFormats.D | GuidFormats.P, string message = "Value cannot be a Guid.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (Condition.IsGuid(argument, format)) { throw new ArgumentException(message, paramName); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> does not have the format of a <see cref="Guid"/>.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="format">A bitmask comprised of one or more <see cref="GuidFormats"/> that specify how the GUID parsing is conducted.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> must be a <see cref="Guid"/>.
        /// </exception>
        public static void ThrowIfNotGuid(string argument, GuidFormats format = GuidFormats.B | GuidFormats.D | GuidFormats.P, string message = "Value must be a Guid.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (!Condition.IsGuid(argument, format)) { throw new ArgumentException(message, paramName); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> has the format of a <see cref="Uri"/>.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="uriKind">The type of the URI.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> cannot be a <see cref="Uri"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="uriKind"/> was set to an indeterminate value of <see cref="UriKind.RelativeOrAbsolute"/>.
        /// </exception>
        public static void ThrowIfUri(string argument, UriKind uriKind = UriKind.Absolute, string message = "Value cannot be a URI.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (uriKind == UriKind.RelativeOrAbsolute) { throw new ArgumentOutOfRangeException(paramName, uriKind, $"{nameof(UriKind)} must be either {nameof(UriKind.Absolute)} or {nameof(UriKind.Relative)}; indeterminate value of {nameof(UriKind.RelativeOrAbsolute)} is not supported."); }
            if (Condition.IsUri(argument, o => o.Kind = uriKind)) { throw new ArgumentException(message, paramName); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> does not have the format of a <see cref="Uri"/>.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="uriKind">The type of the URI.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> must be a <see cref="Uri"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="uriKind"/> was set to an indeterminate value of <see cref="UriKind.RelativeOrAbsolute"/>.
        /// </exception>
        public static void ThrowIfNotUri(string argument, UriKind uriKind = UriKind.Absolute, string message = "Value must be a URI.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (uriKind == UriKind.RelativeOrAbsolute) { throw new ArgumentOutOfRangeException(paramName, uriKind, $"{nameof(UriKind)} must be either {nameof(UriKind.Absolute)} or {nameof(UriKind.Relative)}; indeterminate value of {nameof(UriKind.RelativeOrAbsolute)} is not supported."); }
            if (!Condition.IsUri(argument, o => o.Kind = uriKind)) { throw new ArgumentException(message, paramName); }
        }

        /// <summary>
        /// Validates and throws a <see cref="TypeArgumentOutOfRangeException"/> if the specified <typeparamref name="T"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments (that must be an interface) to match with the type of <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentOutOfRangeException">
        /// <typeparamref name="T"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="types"/> does not satisfy the condition of being an interface.
        /// </exception>
        public static void ThrowIfContainsInterface<T>(string typeParamName, params Type[] types)
        {
            ThrowIfContainsInterface<T>(typeParamName, FormattableString.Invariant($"Specified argument is contained within at least one of {nameof(types)}."), types);
        }

        /// <summary>
        /// Validates and throws an <see cref="TypeArgumentOutOfRangeException"/> if the specified <typeparamref name="T"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments (that must be an interface) to match with the type of <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentOutOfRangeException">
        /// <typeparamref name="T"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="types"/> does not satisfy the condition of being an interface.
        /// </exception>
        public static void ThrowIfContainsInterface<T>(string typeParamName, string message, params Type[] types)
        {
            ThrowIfNull(types);
            ThrowIfFalse(types.All(type => type.IsInterface), nameof(types), $"At least one of the specified {nameof(types)} is not an interface.");
            if (HasInterfaces(typeof(T), types)) { throw new TypeArgumentOutOfRangeException(typeParamName, CreateTypeList(types), message); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="argument"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="types">A <see cref="Type"/> array that contains zero or more types (that each must be an interface) to match with the type of <paramref name="argument"/>.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="argument"/> is null - or - <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="argument"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="types"/> does not satisfy the condition of being an interface.
        /// </exception>
        /// <remarks>Use <see cref="Arguments.ToArrayOf{T}"/> to substitute earlier signature of <c>params Type[] types</c>.</remarks>
        public static void ThrowIfContainsInterface(Type argument, Type[] types, string message = "Specified argument is contained within at least one of the specified types.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            ThrowIfNull(argument);
            ThrowIfNull(types);
            ThrowIfFalse(types.All(type => type.IsInterface), nameof(types), $"At least one of the specified {nameof(types)} is not an interface.");
            if (HasInterfaces(argument, types)) { throw new ArgumentOutOfRangeException(paramName, CreateTypeList(types), message); }
        }

        /// <summary>
        /// Validates and throws a <see cref="TypeArgumentOutOfRangeException"/> if the specified <typeparamref name="T"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments (that must be an interface) to match with the type of <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentOutOfRangeException">
        /// <typeparamref name="T"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="types"/> does not satisfy the condition of being an interface.
        /// </exception>
        public static void ThrowIfNotContainsInterface<T>(string typeParamName, params Type[] types)
        {
            ThrowIfNotContainsInterface<T>(typeParamName, FormattableString.Invariant($"Specified argument is not contained within at least one of {nameof(types)}."), types);
        }

        /// <summary>
        /// Validates and throws a <see cref="TypeArgumentOutOfRangeException"/> if the specified <typeparamref name="T"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments (that must be an interface) to match with the type of <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentOutOfRangeException">
        /// <typeparamref name="T"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="types"/> does not satisfy the condition of being an interface.
        /// </exception>
        public static void ThrowIfNotContainsInterface<T>(string typeParamName, string message, params Type[] types)
        {
            ThrowIfNull(types);
            ThrowIfFalse(types.All(type => type.IsInterface), nameof(types), $"At least one of the specified {nameof(types)} is not an interface.");
            if (!HasInterfaces(typeof(T), types)) { throw new TypeArgumentOutOfRangeException(typeParamName, CreateTypeList(types), message); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="argument"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="types">A <see cref="Type"/> array that contains zero or more types (that each must be an interface) to match with the type of <paramref name="argument"/>.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="argument"/> is null - or - <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="argument"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="types"/> does not satisfy the condition of being an interface.
        /// </exception>
        /// <remarks>Use <see cref="Arguments.ToArrayOf{T}"/> to substitute earlier signature of <c>params Type[] types</c>.</remarks>
        public static void ThrowIfNotContainsInterface(Type argument, Type[] types, string message = "Specified argument is not contained within at least one of the specified types.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            ThrowIfNull(argument);
            ThrowIfNull(types);
            ThrowIfFalse(types.All(type => type.IsInterface), nameof(types), $"At least one of the specified {nameof(types)} is not an interface.");
            if (!HasInterfaces(argument, types)) { throw new ArgumentOutOfRangeException(paramName, CreateTypeList(types), message); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="argument"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="types">A <see cref="Type"/> array that contains zero or more types to match with the type of <paramref name="argument"/>.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="argument"/> is null - or - <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="argument"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        /// <remarks>Use <see cref="Arguments.ToArrayOf{T}"/> to substitute earlier signature of <c>params Type[] types</c>.</remarks>
        public static void ThrowIfContainsType(object argument, Type[] types, string message = "Specified argument is contained within at least one of the specified types.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            ThrowIfContainsType(argument?.GetType(), types, message, paramName);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="argument"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="types">A <see cref="Type"/> array that contains zero or more types to match with the type of <paramref name="argument"/>.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="argument"/> is null - or - <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="argument"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        /// <remarks>Use <see cref="Arguments.ToArrayOf{T}"/> to substitute earlier signature of <c>params Type[] types</c>.</remarks>
        public static void ThrowIfContainsType(Type argument, Type[] types, string message = "Specified argument is contained within at least one of the specified types.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            ThrowIfNull(argument);
            ThrowIfNull(types);
            if (HasTypes(argument, types)) { throw new ArgumentOutOfRangeException(paramName, CreateTypeList(types), message); }
        }

        /// <summary>
        /// Validates and throws a <see cref="TypeArgumentOutOfRangeException"/> if the specified <typeparamref name="T"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments to match with the type of <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentOutOfRangeException">
        /// <typeparamref name="T"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        public static void ThrowIfContainsType<T>(string typeParamName, params Type[] types)
        {
            ThrowIfContainsType<T>(typeParamName, FormattableString.Invariant($"Specified argument is contained within at least one of {nameof(types)}."), types);
        }

        /// <summary>
        /// Validates and throws a <see cref="TypeArgumentOutOfRangeException"/> if the specified <typeparamref name="T"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments to match with the type of <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentOutOfRangeException">
        /// <typeparamref name="T"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        public static void ThrowIfContainsType<T>(string typeParamName, string message, params Type[] types)
        {
            ThrowIfNull(types);
            if (HasTypes(typeof(T), types)) { throw new TypeArgumentOutOfRangeException(typeParamName, CreateTypeList(types), message); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="argument"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="types">A <see cref="Type"/> array that contains zero or more types to match with the type of <paramref name="argument"/>.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="argument"/> is null - or - <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="argument"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        /// <remarks>Use <see cref="Arguments.ToArrayOf{T}"/> to substitute earlier signature of <c>params Type[] types</c>.</remarks>
        public static void ThrowIfNotContainsType(Type argument, Type[] types, string message = "Specified argument is not contained within at least one of the specified types.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            ThrowIfNull(argument);
            ThrowIfNull(types);
            if (!HasTypes(argument, types)) { throw new ArgumentOutOfRangeException(paramName, CreateTypeList(types), message); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="argument"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="types">A <see cref="Type"/> array that contains zero or more types to match with the type of <paramref name="argument"/>.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="argument"/> is null - or - <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="argument"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        /// <remarks>Use <see cref="Arguments.ToArrayOf{T}"/> to substitute earlier signature of <c>params Type[] types</c>.</remarks>
        public static void ThrowIfNotContainsType(object argument, Type[] types, string message = "Specified argument is not contained within at least one of the specified types.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            ThrowIfNotContainsType(argument?.GetType(), types, message, paramName);
        }

        /// <summary>
        /// Validates and throws a <see cref="TypeArgumentOutOfRangeException"/> if the specified <typeparamref name="T"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments to match with the type of <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentOutOfRangeException">
        /// <typeparamref name="T"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        public static void ThrowIfNotContainsType<T>(string typeParamName, params Type[] types)
        {
            ThrowIfNotContainsType<T>(typeParamName, FormattableString.Invariant($"Specified argument is not contained within at least one of {nameof(types)}."), types);
        }

        /// <summary>
        /// Validates and throws a <see cref="TypeArgumentOutOfRangeException"/> if the specified <typeparamref name="T"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments to match with the type of <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentOutOfRangeException">
        /// <typeparamref name="T"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        public static void ThrowIfNotContainsType<T>(string typeParamName, string message, params Type[] types)
        {
            ThrowIfNull(types);
            if (!HasTypes(typeof(T), types)) { throw new TypeArgumentOutOfRangeException(typeParamName, CreateTypeList(types), message); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> represents an enumeration.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore case; <c>false</c> to regard case.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> represents an enumeration.
        /// </exception>
        public static void ThrowIfEnum<TEnum>(string argument, bool ignoreCase = true, string message = "Value represents an enumeration.", [CallerArgumentExpression(nameof(argument))] string paramName = null) where TEnum : struct, IConvertible
        {
            if (Condition.IsEnum<TEnum>(argument, o => o.IgnoreCase = ignoreCase)) { throw new ArgumentException(message, paramName); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> does not represents an enumeration.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore case; <c>false</c> to regard case.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> does not represents an enumeration.
        /// </exception>
        public static void ThrowIfNotEnum<TEnum>(string argument, bool ignoreCase = true, string message = "Value does not represents an enumeration.", [CallerArgumentExpression(nameof(argument))] string paramName = null) where TEnum : struct, IConvertible
        {
            if (!Condition.IsEnum<TEnum>(argument, o => o.IgnoreCase = ignoreCase)) { throw new ArgumentException(message, paramName); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> represents an enumeration.
        /// </summary>
        /// <param name="argument">The type to check is an enumeration.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the type parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> represents an enumeration.
        /// </exception>
        /// <remarks>This method will not throw an exception if <paramref name="argument"/> is null.</remarks>
        public static void ThrowIfEnumType(Type argument, string message = "Value represents an enumeration.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (Condition.IsTrue(argument?.GetTypeInfo().IsEnum ?? false)) { throw new ArgumentException(message, paramName); }
        }

        /// <summary>
        /// Validates and throws a <see cref="TypeArgumentException"/> if the specified <typeparamref name="TEnum"/> represents an enumeration.
        /// </summary>
        /// <typeparam name="TEnum">The type to check is an enumeration.</typeparam>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="TypeArgumentException">
        /// <typeparamref name="TEnum"/> represents an enumeration.
        /// </exception>
        public static void ThrowIfEnumType<TEnum>(string typeParamName, string message = "Value represents an enumeration.")
        {
            if (typeof(TEnum).GetTypeInfo().IsEnum) { throw new TypeArgumentException(typeParamName, message); }
        }

        /// <summary>
        /// Validates and throws a <see cref="TypeArgumentException"/> if the specified <typeparamref name="TEnum"/> does not represents an enumeration.
        /// </summary>
        /// <typeparam name="TEnum">The type to check is not an enumeration.</typeparam>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="TypeArgumentException">
        /// <typeparamref name="TEnum"/> does not represents an enumeration.
        /// </exception>
        public static void ThrowIfNotEnumType<TEnum>(string typeParamName, string message = "Value does not represents an enumeration.")
        {
            if (!typeof(TEnum).GetTypeInfo().IsEnum) { throw new TypeArgumentException(typeParamName, message); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> does not represents an enumeration.
        /// </summary>
        /// <param name="argument">The type to check is not an enumeration.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the type parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> does not represents an enumeration.
        /// </exception>
        public static void ThrowIfNotEnumType(Type argument, string message = "Value does not represents an enumeration.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (Condition.IsFalse(argument?.GetTypeInfo().IsEnum ?? true)) { throw new ArgumentException(message, paramName); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="argument"/> consist of anything besides binary digits.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="argument"/> must consist only of binary digits.
        /// </exception>
        public static void ThrowIfNotBinaryDigits(string argument, string message = "Value must consist only of binary digits.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (!Condition.IsBinaryDigits(argument)) { throw new ArgumentOutOfRangeException(paramName, argument, message); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="argument"/> consist of anything besides a base-64 structure.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="argument"/> must consist only of base-64 digits.
        /// </exception>
        public static void ThrowIfNotBase64String(string argument, string message = "Value must consist only of base-64 digits.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (!Condition.IsBase64(argument)) { throw new ArgumentOutOfRangeException(paramName, argument, message); }
        }

        /// <summary>
        /// Validates and throws a <see cref="ArgumentReservedKeywordException"/> if the specified <paramref name="argument"/> is found in the sequence of <paramref name="reservedKeywords"/>.
        /// </summary>
        /// <param name="argument">The keyword to compare with <paramref name="reservedKeywords"/>.</param>
        /// <param name="reservedKeywords">The reserved keywords to compare with <paramref name="argument"/>.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentReservedKeywordException">
        /// The specified <paramref name="argument"/> is contained within <paramref name="reservedKeywords"/>.
        /// </exception>
        public static void ThrowIfContainsReservedKeyword(string argument, IEnumerable<string> reservedKeywords, string message = null, [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            ThrowIfContainsReservedKeyword(argument, reservedKeywords, null, message, paramName);
        }

        /// <summary>
        /// Validates and throws a <see cref="ArgumentReservedKeywordException"/> if the specified <paramref name="argument"/> is found in the sequence of <paramref name="reservedKeywords"/>.
        /// </summary>
        /// <param name="argument">The keyword to compare with <paramref name="reservedKeywords"/>.</param>
        /// <param name="reservedKeywords">The reserved keywords to compare with <paramref name="argument"/>.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing <paramref name="reservedKeywords"/> with <paramref name="argument"/>.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentReservedKeywordException">
        /// The specified <paramref name="argument"/> is contained within <paramref name="reservedKeywords"/>.
        /// </exception>
        public static void ThrowIfContainsReservedKeyword(string argument, IEnumerable<string> reservedKeywords, IEqualityComparer<string> comparer, string message = null, [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (argument == null || reservedKeywords == null) { return; }
            if (reservedKeywords.Contains(argument, comparer ?? EqualityComparer<string>.Default)) { throw new ArgumentReservedKeywordException(paramName, argument, message); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if there is a difference between <paramref name="second"/> and <paramref name="first"/>.
        /// </summary>
        /// <param name="first">The value that specifies valid characters.</param>
        /// <param name="second">The value to compare with <paramref name="first"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// There is a difference between <paramref name="second"/> and <paramref name="first"/>.
        /// </exception>
        public static void ThrowIfDifferent(string first, string second, string paramName, string message = null)
        {
            message ??= FormattableString.Invariant($"Specified arguments has a difference between {nameof(second)} and {nameof(first)}.");
            if (Condition.HasDifference(first, second, out var invalidCharacters)) { throw new ArgumentOutOfRangeException(paramName, invalidCharacters, message); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if there is no difference between <paramref name="second"/> and <paramref name="first"/>.
        /// </summary>
        /// <param name="first">The value that specifies valid characters.</param>
        /// <param name="second">The value to compare with <paramref name="first"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// There is no difference between <paramref name="second"/> and <paramref name="first"/>.
        /// </exception>
        public static void ThrowIfNotDifferent(string first, string second, string paramName, string message = null)
        {
            message ??= FormattableString.Invariant($"Specified arguments does not have a difference between {nameof(second)} and {nameof(first)}.");
            if (!Condition.HasDifference(first, second, out _)) { throw new ArgumentOutOfRangeException(paramName, message); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if any of the <paramref name="characters"/> occurs within the <paramref name="argument"/>.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="characters">The sequence of <see cref="char"/> to search within <paramref name="argument"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="argument"/> contains one or more of the specified <paramref name="characters"/>.
        /// </exception>
        public static void ThrowIfContainsAny(string argument, char[] characters, StringComparison comparison = StringComparison.OrdinalIgnoreCase, string message = "One or more character matches were found.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            ThrowIfNull(characters);
            if (ContainsAny(argument, characters, comparison))
            {
                throw new ArgumentOutOfRangeException(paramName, CreateCharacterList(argument.Where(c => characters.Any(find => string.Equals(c.ToString(), find.ToString(), comparison))).Distinct()), message);
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if any of the <paramref name="characters"/> does not occur within the <paramref name="argument"/>.
        /// </summary>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="characters">The sequence of <see cref="char"/> to search within <paramref name="argument"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="argument"/> does not contain any of the specified <paramref name="characters"/>.
        /// </exception>
        public static void ThrowIfNotContainsAny(string argument, char[] characters, StringComparison comparison = StringComparison.OrdinalIgnoreCase, string message = "No matching characters were found.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            ThrowIfNull(characters);
            if (!ContainsAny(argument, characters, comparison))
            {
                throw new ArgumentOutOfRangeException(paramName, CreateCharacterList(characters.Distinct()), message);
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> (or a derived counterpart) from the specified delegate <paramref name="condition"/>.
        /// </summary>
        /// <param name="condition">The delegate that evaluates, creates and ultimately throws an <see cref="ArgumentException"/> (or a derived counterpart) from within a given scenario.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null.
        /// </exception>
        public static void ThrowWhen(Action<ExceptionCondition<ArgumentException>> condition)
        {
            ThrowIfNull(condition);
            Patterns.CreateInstance(condition);
        }

        private static bool ContainsAny(string argument, IEnumerable<char> characters, StringComparison comparison)
        {
            if (argument == null) { return false; }
            foreach (var character in characters)
            {
                if (argument.IndexOf(character.ToString(), comparison) >= 0) { return true; }
            }
            return false;
        }

        private static string CreateCharacterList(IEnumerable<char> characters)
        {
            return string.Join(",", characters.Select(c => $"'{c}'"));
        }

        private static string CreateTypeList(IEnumerable<Type> types)
        {
            return string.Join(", ", types.Select(type => ToFriendlyTypeName(type, true)));
        }

        private static bool HasInterfaces(Type source, IEnumerable<Type> interfaceTypes)
        {
            var implementedInterfaces = source.IsInterface ? Enumerable.Repeat(source, 1).Concat(source.GetInterfaces()).ToList() : source.GetInterfaces().ToList();
            foreach (var interfaceType in interfaceTypes.Where(type => type.IsInterface))
            {
                foreach (var implementedInterface in implementedInterfaces)
                {
                    if (implementedInterface.IsGenericType && interfaceType == implementedInterface.GetGenericTypeDefinition()) { return true; }
                    if (interfaceType == implementedInterface) { return true; }
                }
            }
            return false;
        }

        private static bool HasTypes(Type source, IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var current = source;
                while (current != null)
                {
                    if (current.IsGenericType && type == current.GetGenericTypeDefinition()) { return true; }
                    if (current == type) { return true; }
                    current = current.BaseType;
                }
            }
            return false;
        }

        private static string ToFriendlyTypeName(Type type, bool fullName = false)
        {
            if (type.IsByRef) { return $"{ToFriendlyTypeName(type.GetElementType()!, fullName)}&"; }
            if (type.IsPointer) { return $"{ToFriendlyTypeName(type.GetElementType()!, fullName)}*"; }
            if (type.IsArray) { return $"{ToFriendlyTypeName(type.GetElementType()!, fullName)}[]"; }
            if (!type.GetTypeInfo().IsGenericType) { return fullName ? type.FullName ?? type.Name : type.Name; }

            var genericType = type.IsGenericTypeDefinition ? type : type.GetGenericTypeDefinition();
            var genericTypeName = fullName ? genericType.FullName ?? genericType.Name : genericType.Name;
            var arityIndex = genericTypeName.IndexOf('`');
            if (arityIndex > -1) { genericTypeName = genericTypeName.Substring(0, arityIndex); }

            return FormattableString.Invariant($"{genericTypeName}<{string.Join(",", type.GetGenericArguments().Select(argument => ToFriendlyTypeName(argument, fullName)))}>");
        }
    }
}
