using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Cuemon;
/// <summary>
/// Extension methods for the <see cref="object"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class ObjectDecoratorExtensions
{
    /// <summary>
    /// Returns an <see cref="object"/> of the specified <typeparamref name="T"/> whose value is equivalent to the enclosed <see cref="object"/> of the specified <paramref name="decorator"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to return.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
    /// <param name="fallbackResult">The value to return when a conversion is not possible. Default is <c>default</c> of <typeparamref name="T"/>.</param>
    /// <param name="setup">The <see cref="ObjectFormattingOptions"/> which may be configured.</param>
    /// <returns>An <see cref="object"/> of type <typeparamref name="T"/> equivalent to the enclosed <see cref="object"/> of the specified <paramref name="decorator"/> when a conversion is possible; otherwise <paramref name="fallbackResult"/> is returned.</returns>
    /// <remarks>This method first checks if the enclosed <see cref="object"/> of the specified <paramref name="decorator"/> is compatible with <typeparamref name="T"/>; if incompatible the method continues with <see cref="ChangeType{T}"/> for the operation.</remarks>
    public static T ChangeTypeOrDefault<T>(this IDecorator<object> decorator, T fallbackResult = default, Action<ObjectFormattingOptions> setup = null)
    {
        if (decorator.Inner is T result) { return result; }
        return Patterns.InvokeOrDefault(() => ChangeType<T>(decorator, setup), fallbackResult);
    }

    /// <summary>
    /// Returns an <see cref="object"/> of the specified <typeparamref name="T"/> whose value is equivalent to the enclosed <see cref="object"/> of the specified <paramref name="decorator"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to return.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
    /// <param name="setup">The <see cref="ObjectFormattingOptions"/> which may be configured.</param>
    /// <returns>An <see cref="object"/> of type <typeparamref name="T"/> equivalent to the enclosed <see cref="object"/> of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <exception cref="AggregateException">
    /// The enclosed <see cref="object"/> of <paramref name="decorator"/> could not be converted.
    /// </exception>
    /// <remarks>What differs from the <see cref="Convert.ChangeType(object,Type)"/> is, that this converter supports generics and enums. Fallback uses <see cref="TypeDescriptor"/> and checks if the underlying <see cref="IFormatProvider"/> of <see cref="FormattingOptions.FormatProvider"/> is a <see cref="CultureInfo"/>, then this will be used in the conversion together with <see cref="ObjectFormattingOptions.DescriptorContext"/>.</remarks>
    /// <seealso cref="Convert.ChangeType(object,Type)"/>
    /// <seealso cref="TypeDescriptor.GetConverter(Type)"/>
    public static T ChangeType<T>(this IDecorator<object> decorator, Action<ObjectFormattingOptions> setup = null)
    {
        return (T)ChangeType(decorator, typeof(T), setup);
    }

    /// <summary>
    /// Returns an <see cref="object"/> of a specified <paramref name="targetType"/> whose value is equivalent to the enclosed <see cref="object"/> of the specified <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
    /// <param name="targetType">The type of the object to return.</param>
    /// <param name="setup">The <see cref="ObjectFormattingOptions"/> which may be configured.</param>
    /// <returns>An <see cref="object"/> of type <paramref name="targetType"/> equivalent to the enclosed <see cref="object"/> of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <exception cref="AggregateException">
    /// The enclosed <see cref="object"/> of <paramref name="decorator"/> could not be converted.
    /// </exception>
    /// <remarks>What differs from the <see cref="Convert.ChangeType(object,Type)"/> is, that this converter supports generics and enums. Fallback uses <see cref="TypeDescriptor"/> and checks if the underlying <see cref="IFormatProvider"/> of <see cref="FormattingOptions.FormatProvider"/> is a <see cref="CultureInfo"/>, then this will be used in the conversion together with <see cref="ObjectFormattingOptions.DescriptorContext"/>.</remarks>
    /// <seealso cref="Convert.ChangeType(object,Type)"/>
    /// <seealso cref="TypeDescriptor.GetConverter(Type)"/>
    public static object ChangeType(this IDecorator<object> decorator, Type targetType, Action<ObjectFormattingOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfNull(targetType);
        if (decorator.Inner == null) { return null; }
        var options = Patterns.Configure(setup);
        try
        {
            var isEnum = targetType.GetTypeInfo().IsEnum;
            var isNullable = Decorator.Enclose(targetType).IsNullable();
            switch (targetType)
            {
                case { } dt when dt == typeof(DateTime) && decorator.Inner is string dtValue && dtValue.EndsWith("Z", StringComparison.OrdinalIgnoreCase):
                    return DateTime.Parse(dtValue, options.FormatProvider, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
                default:
                    return Convert.ChangeType(isEnum ? Enum.Parse(targetType, decorator.Inner.ToString()!) : decorator.Inner, (isNullable ? Nullable.GetUnderlyingType(targetType) : targetType)!, options.FormatProvider);
            }
        }
        catch (Exception first)
        {
            try
            {
                if (options.FormatProvider is CultureInfo ci) { return TypeDescriptor.GetConverter(targetType).ConvertFrom(options.DescriptorContext, ci, decorator.Inner); }
                return TypeDescriptor.GetConverter(targetType).ConvertFrom(decorator.Inner);
            }
            catch (Exception second)
            {
                throw new AggregateException(first, second);
            }
        }
    }

    /// <summary>
    /// Resolves the default value of a property from the enclosed object of the specified <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="pi">The <see cref="PropertyInfo"/> of the property to resolve the value for.</param>
    /// <returns>The value of the property if the enclosed object is not null; otherwise, <c>null</c>.</returns>
    /// <remarks>This API supports the product infrastructure and is not intended to be used directly from your code.</remarks>
    public static object DefaultPropertyValueResolver(this IDecorator<object> decorator, PropertyInfo pi)
    {
        var source = decorator?.Inner;
        return source == null
            ? null
            : pi?.GetValue(source, null);
    }

    /// <summary>
    /// Invokes the specified <paramref name="traversal"/> path of the enclosed object of the specified <paramref name="decorator"/> until obstructed by an empty sequence.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="traversal">The function delegate that is invoked until the traveled path is obstructed by an empty sequence.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> sequence equal to the traveled path of the enclosed object of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> -or- <paramref name="traversal"/> is null.
    /// </exception>
    public static IEnumerable<TSource> TraverseWhileNotEmpty<TSource>(this IDecorator<TSource> decorator, Func<TSource, IEnumerable<TSource>> traversal) where TSource : class
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfNull(traversal);

        var source = decorator.Inner;
        var stack = new Stack<TSource>();
        stack.Push(source);
        while (stack.Count != 0)
        {
            var current = stack.Pop();
            foreach (var element in traversal(current))
            {
                stack.Push(element);
            }
            yield return current;
        }
    }
}
