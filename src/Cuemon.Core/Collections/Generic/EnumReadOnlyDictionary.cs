using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cuemon.Collections.Generic;
/// <summary>
/// Represents a read-only collection of key/value pairs that provides information about the specified <typeparamref name="TEnum"/>.
/// Implements the <see cref="ReadOnlyDictionary{TKey, TValue}" />
/// </summary>
/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
/// <seealso cref="ReadOnlyDictionary{TKey, TValue}" />
public class EnumReadOnlyDictionary<TEnum> : ReadOnlyDictionary<IConvertible, string> where TEnum : struct, IConvertible
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumReadOnlyDictionary{TEnum}"/> class.
    /// </summary>
    /// <exception cref="TypeArgumentException">
    /// <typeparamref name="TEnum"/> does not represents an enumeration.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <typeparamref name="TEnum"/> is a type from an assembly loaded in a reflection-only context.
    /// </exception>
    public EnumReadOnlyDictionary() : base(Initialize())
    {
    }

    private static IDictionary<IConvertible, string> Initialize()
    {
        Validator.ThrowIfNotEnumType<TEnum>(nameof(TEnum));
        var dictionary = new Dictionary<IConvertible, string>();
        var values = Enum.GetValues(typeof(TEnum));
        var integral = Enum.GetUnderlyingType(typeof(TEnum));
        foreach (var value in values)
        {
            dictionary.Add((IConvertible)Decorator.Enclose(value).ChangeType(integral), value.ToString());
        }
        return dictionary;
    }
}
