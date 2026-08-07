using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Cuemon.Extensions;
/// <summary>
/// Extension methods for the <see cref="object"/> class.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Wrap and extend an existing object of <typeparamref name="T"/> with additional data.
    /// </summary>
    /// <typeparam name="T">The type of the object to extend.</typeparam>
    /// <param name="instance">The instance to wrap and extend.</param>
    /// <param name="extender">The delegate that provides an easy way of supplying additional data to an object.</param>
    /// <returns>An implementation of <see cref="IWrapper{T}"/> encapsulating the specified <paramref name="instance"/>.</returns>
    public static IWrapper<T> UseWrapper<T>(this T instance, Action<IDictionary<string, object>> extender = null)
    {
        return UseWrapper(instance, null, extender);
    }

    /// <summary>
    /// Wrap and extend an existing object of <typeparamref name="T"/> with additional data.
    /// </summary>
    /// <typeparam name="T">The type of the object to extend.</typeparam>
    /// <param name="instance">The instance to wrap and extend.</param>
    /// <param name="memberReference">The optional member reference to assign <see cref="IWrapper{T}.MemberReference"/>.</param>
    /// <param name="extender">The delegate that provides an easy way of supplying additional data to an object.</param>
    /// <returns>An implementation of <see cref="IWrapper{T}"/> encapsulating the specified <paramref name="instance"/>.</returns>
    public static IWrapper<T> UseWrapper<T>(this T instance, MemberInfo memberReference, Action<IDictionary<string, object>> extender = null)
    {
        var wrapper = new Wrapper<T>(instance, memberReference);
        extender?.Invoke(wrapper.Data);
        return wrapper;
    }

    /// <summary>
    /// Attempts to converts the specified <paramref name="value"/> to a given type. If the conversion is not possible the result is set to <paramref name="fallbackResult"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to return.</typeparam>
    /// <param name="value">The object to convert the underlying type.</param>
    /// <param name="fallbackResult">The value to return when a conversion is not possible. Default is <c>default</c> of <typeparamref name="T"/>.</param>
    /// <param name="setup">The <see cref="ObjectFormattingOptions"/> which may be configured.</param>
    /// <returns>The <paramref name="value"/> converted to the specified <typeparamref name="T"/>.</returns>
    public static T As<T>(this object value, T fallbackResult = default, Action<ObjectFormattingOptions> setup = null)
    {
        return Decorator.Enclose(value, false).ChangeTypeOrDefault(fallbackResult, setup);
    }

    /// <summary>
    /// Converts the specified <paramref name="value"/> to a value of <typeparamref name="TResult"/>. 
    /// </summary>
    /// <typeparam name="T">The type of the value to convert.</typeparam>
    /// <typeparam name="TResult">The type of the value to return.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <param name="converter">The function delegate that will perform the conversion.</param>
    /// <returns>The <paramref name="value"/> converted to the specified <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    public static TResult As<T, TResult>(this T value, Func<T, TResult> converter)
    {
        Validator.ThrowIfNull(value);
        return Tweaker.Change(value, converter);
    }

    /// <summary>
    /// Attempts to converts the specified <paramref name="value"/> to the given <paramref name="targetType"/>.
    /// </summary>
    /// <param name="value">The object to convert the underlying type.</param>
    /// <param name="targetType">The type of the object to return.</param>
    /// <param name="setup">The <see cref="ObjectFormattingOptions"/> which may be configured.</param>
    /// <returns>An <see cref="object"/> of type <paramref name="targetType"/> equivalent to <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null - or -
    /// <paramref name="targetType"/> cannot be null.
    /// </exception>
    /// <exception cref="AggregateException">
    /// <paramref name="value"/> could not be converted.
    /// </exception>
    /// <remarks>What differs from the <see cref="Convert.ChangeType(object,Type)"/> is, that this converter supports generics and enums. Fallback uses <see cref="TypeDescriptor"/> and checks if the underlying <see cref="IFormatProvider"/> of <see cref="FormattingOptions.FormatProvider"/> is a <see cref="CultureInfo"/>, then this will be used in the conversion together with <see cref="ObjectFormattingOptions.DescriptorContext"/>.</remarks>
    /// <seealso cref="Convert.ChangeType(object,Type)"/>
    /// <seealso cref="TypeDescriptor.GetConverter(Type)"/>
    public static object As(this object value, Type targetType, Action<ObjectFormattingOptions> setup = null)
    {
        Validator.ThrowIfNull(value);
        Validator.ThrowIfNull(targetType);
        return Decorator.Enclose(value, false).ChangeType(targetType, setup);
    }

    /// <summary>
    /// Computes a suitable hash code from the specified sequence of <paramref name="convertibles"/>.
    /// </summary>
    /// <param name="convertibles">A sequence of objects implementing the <see cref="IConvertible"/> interface.</param>
    /// <returns>A 32-bit signed integer that is the hash code of <paramref name="convertibles"/>.</returns>
    public static int GetHashCode32<T>(this IEnumerable<T> convertibles) where T : IConvertible
    {
        return Generate.HashCode32(convertibles.Cast<IConvertible>());
    }

    /// <summary>
    /// Computes a suitable hash code from the specified sequence of <paramref name="convertibles"/>.
    /// </summary>
    /// <param name="convertibles">A sequence of objects implementing the <see cref="IConvertible"/> interface.</param>
    /// <returns>A 64-bit signed integer that is the hash code of <paramref name="convertibles"/>.</returns>
    public static long GetHashCode64<T>(this IEnumerable<T> convertibles) where T : IConvertible
    {
        return Generate.HashCode64(convertibles.Cast<IConvertible>());
    }

    /// <summary>
    /// Converts the specified <paramref name="source"/> to a string of delimited values.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the sequence to convert.</typeparam>
    /// <param name="source">A sequence of elements to be converted.</param>
    /// <param name="setup">The <see cref="DelimitedStringOptions{T}"/> which may be configured.</param>
    /// <returns>A <see cref="string"/> of delimited values.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> cannot be null.
    /// </exception>
    public static string ToDelimitedString<T>(this IEnumerable<T> source, Action<DelimitedStringOptions<T>> setup = null)
    {
        Validator.ThrowIfNull(source);
        return DelimitedString.Create(source, setup);
    }

    /// <summary>
    /// Adjust the specified <paramref name="value"/> with the function delegate <paramref name="converter"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value to convert.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <param name="converter">The function delegate that will convert the specified <paramref name="value"/>.</param>
    /// <returns>The <paramref name="value"/> in its original or converted form.</returns>
    /// <remarks>This is thought to be a more severe change than the one provided by <see cref="Alter{T}"/> (e.g., potentially convert the entire <paramref name="value"/> to a new instance).</remarks>
    public static T Adjust<T>(this T value, Func<T, T> converter)
    {
        return Tweaker.Adjust(value, converter);
    }

    /// <summary>
    /// Adjust the specified <paramref name="value"/> with the <paramref name="modifier"/> delegate.
    /// </summary>
    /// <typeparam name="T">The type of the value to adjust.</typeparam>
    /// <param name="value">The value to adjust.</param>
    /// <param name="modifier">The delegate that will adjust the specified <paramref name="value"/>.</param>
    /// <returns>The <paramref name="value"/> in its original or adjusted form.</returns>
    /// <remarks>This is thought to be a more relaxed change than the one provided by <see cref="Adjust{T}"/> (e.g., applying changes only to the current <paramref name="value"/>).</remarks>
    public static T Alter<T>(this T value, Action<T> modifier)
    {
        return Tweaker.Alter(value, modifier);
    }

    /// <summary>
    /// Determines whether the specified source is a nullable <see cref="ValueType"/>.
    /// </summary>
    /// <typeparam name="T">The type of the <paramref name="_"/> of <typeparamref name="T"/>.</typeparam>
    /// <param name="_">The source type to check for nullable <see cref="ValueType"/>.</param>
    /// <returns>
    ///   <c>true</c> if the specified source is nullable; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNullable<T>(this T _)
    {
        return typeof(T).IsNullable();
    }
}
