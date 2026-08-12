using System;
using System.Collections.Generic;

namespace Cuemon.Collections.Generic;
/// <summary>
/// Extension methods for the <see cref="IDictionary{TKey,TValue}"/> interface hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class DictionaryDecoratorExtensions
{
    /// <summary>
    /// Copies all elements from the enclosed <see cref="IDictionary{TKey,TValue}"/> of <paramref name="decorator"/> to <paramref name="destination"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="destination">The <see cref="IDictionary{TKey,TValue}"/> to which the elements of the enclosed <see cref="IDictionary{TKey,TValue}"/> will be copied.</param>
    /// <returns>An <see cref="IDictionary{TKey,TValue}"/> that is the result of the populated <paramref name="destination"/>.</returns>
    public static IDictionary<TKey, TValue> CopyTo<TKey, TValue>(this IDecorator<IDictionary<TKey, TValue>> decorator, IDictionary<TKey, TValue> destination)
    {
        return CopyTo(decorator, destination, (s, d) =>
        {
            foreach (var item in s)
            {
                d.Add(item);
            }
        });
    }

    /// <summary>
    /// Copies elements from the enclosed <see cref="IDictionary{TKey,TValue}"/> of <paramref name="decorator"/> to <paramref name="destination"/> using the <paramref name="copier"/> delegate.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="destination">The <see cref="IDictionary{TKey,TValue}"/> to which the elements of the enclosed <see cref="IDictionary{TKey,TValue}"/> will be copied.</param>
    /// <param name="copier">The delegate that will populate a copy of the enclosed <see cref="IDictionary{TKey,TValue}"/> to the specified <paramref name="destination"/>.</param>
    /// <returns>An <see cref="IDictionary{TKey,TValue}"/> that is the result of the populated <paramref name="destination"/>.</returns>
    public static IDictionary<TKey, TValue> CopyTo<TKey, TValue>(this IDecorator<IDictionary<TKey, TValue>> decorator, IDictionary<TKey, TValue> destination, Action<IDictionary<TKey, TValue>, IDictionary<TKey, TValue>> copier)
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfNull(destination);
        Validator.ThrowIfNull(copier);
        copier(decorator.Inner, destination);
        return destination;
    }

    /// <summary>
    /// Gets the value associated with the specified <paramref name="key"/> or <c>default</c> when the key does not exists in the enclosed <see cref="IDictionary{TKey,TValue}"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="key">The key of the value to get.</param>
    /// <returns>Either the value associated with the specified <paramref name="key"/> or <c>default(<typeparamref name="TValue"/>)</c> when the key does not exists.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null -or-
    /// <paramref name="key"/> cannot be null.
    /// </exception>
    public static TValue GetValueOrDefault<TKey, TValue>(this IDecorator<IDictionary<TKey, TValue>> decorator, TKey key)
    {
        Validator.ThrowIfNull(decorator);
        return decorator.GetValueOrDefault(key, () => default);
    }

    /// <summary>
    /// Gets the value associated with the specified <paramref name="key"/> or a default value through <paramref name="defaultProvider"/> when the key does not exists in the enclosed <see cref="IDictionary{TKey,TValue}"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="key">The key of the value to get.</param>
    /// <param name="defaultProvider">The function delegate that will provide a default value when the <paramref name="key"/> does not exists in the enclosed <see cref="IDictionary{TKey,TValue}"/> of the <paramref name="decorator"/>.</param>
    /// <returns>Either the value associated with the specified <paramref name="key"/> or a default value through <paramref name="defaultProvider"/> when the key does not exists.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null -or-
    /// <paramref name="key"/> cannot be null -or-
    /// <paramref name="defaultProvider"/> cannot be null.
    /// </exception>
    public static TValue GetValueOrDefault<TKey, TValue>(this IDecorator<IDictionary<TKey, TValue>> decorator, TKey key, Func<TValue> defaultProvider)
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfNull(key);
        Validator.ThrowIfNull(defaultProvider);
        return decorator.Inner.TryGetValue(key, out var value) ? value : defaultProvider();
    }

    /// <summary>
    /// Gets the <paramref name="value"/> associated with the specified <paramref name="key"/> from the enclosed <see cref="IDictionary{TKey,TValue}"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="key">The key of the value to get.</param>
    /// <param name="fallbackKeySelector">The function delegate that will, as a fallback, resolve an alternate key from the specified <paramref name="key"/>.</param>
    /// <param name="value">When this method returns, contains the value associated with the specified <paramref name="key"/> or the alternate key resolved from <paramref name="fallbackKeySelector"/>, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
    /// <returns><c>true</c> if the enclosed <see cref="IDictionary{TKey,TValue}"/> of the <paramref name="decorator"/> contains an element with the specified <paramref name="key"/> or the alternate key resolved from <paramref name="fallbackKeySelector"/>, <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool TryGetValueOrFallback<TKey, TValue>(this IDecorator<IDictionary<TKey, TValue>> decorator, TKey key, Func<IEnumerable<TKey>, TKey> fallbackKeySelector, out TValue value)
    {
        Validator.ThrowIfNull(decorator);
        value = default;
        if (key == null) { return false; }
        if (!decorator.Inner.TryGetValue(key, out value))
        {
            if (fallbackKeySelector == null) { return false; }
            var alternateKey = fallbackKeySelector(decorator.Inner.Keys);
            return alternateKey != null && decorator.Inner.TryGetValue(alternateKey, out value);
        }
        return true;
    }

    /// <summary>
    /// Returns the enclosed <see cref="IDictionary{TKey,TValue}"/> of the <paramref name="decorator"/> typed as <see cref="KeyValuePair{TKey,TValue}"/> sequence.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns>A <see cref="KeyValuePair{TKey,TValue}"/> equivalent sequence of the enclosed <see cref="IDictionary{TKey,TValue}"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static IEnumerable<KeyValuePair<TKey, TValue>> ToEnumerable<TKey, TValue>(this IDecorator<IDictionary<TKey, TValue>> decorator)
    {
        Validator.ThrowIfNull(decorator);
        return decorator.Inner;
    }

    /// <summary>
    /// Attempts to add the specified <paramref name="key"/> and <paramref name="value"/> to the enclosed <see cref="IDictionary{TKey,TValue}"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="value">The value of the element to add.</param>
    /// <param name="condition">The function delegate that specifies the condition for adding the element.</param>
    /// <returns><c>true</c> if the key/value pair was added to the enclosed <see cref="IDictionary{TKey,TValue}"/> of the <paramref name="decorator"/> successfully; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null -or-
    /// <paramref name="key"/> cannot be null -or-
    /// <paramref name="condition"/> cannot be null.
    /// </exception>
    public static bool TryAdd<TKey, TValue>(this IDecorator<IDictionary<TKey, TValue>> decorator, TKey key, TValue value, Func<IDictionary<TKey, TValue>, bool> condition)
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfNull(key);
        Validator.ThrowIfNull(condition);
        return condition(decorator.Inner) && Patterns.TryInvoke(() => decorator.Inner.Add(key, value));
    }

    /// <summary>
    /// Attempts to add the specified <paramref name="key"/> and <paramref name="value"/> to the enclosed <see cref="IDictionary{TKey,TValue}"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="value">The value of the element to add.</param>
    /// <returns><c>true</c> if the key/value pair was added to the enclosed <see cref="IDictionary{TKey,TValue}"/> of the <paramref name="decorator"/> successfully; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null -or-
    /// <paramref name="key"/> cannot be null.
    /// </exception>
    public static bool TryAdd<TKey, TValue>(this IDecorator<IDictionary<TKey, TValue>> decorator, TKey key, TValue value)
    {
        return decorator.TryAdd(key, value, d => !d.ContainsKey(key));
    }

    /// <summary>
    /// Attempts to add or update an existing element with the provided <paramref name="key"/> to the enclosed <see cref="IDictionary{TKey,TValue}"/> of the <paramref name="decorator"/> with the specified <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="key">The key of the element to add or update.</param>
    /// <param name="value">The value of the element to add or update.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null -or-
    /// <paramref name="key"/> cannot be null.
    /// </exception>
    public static void AddOrUpdate<TKey, TValue>(this IDecorator<IDictionary<TKey, TValue>> decorator, TKey key, TValue value)
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfNull(key);
        Condition.FlipFlop(decorator.Inner.ContainsKey(key), () => Patterns.TryInvoke(() => { decorator.Inner[key] = value; }), () => TryAdd(decorator, key, value));
    }

    /// <summary>
    /// Gets the level of nesting of the enclosed <see cref="IDictionary{TKey,TValue}"/> of the <paramref name="decorator"/>.
    /// This API supports the product infrastructure and is not intended to be used directly from your code.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="readerDepth">The value that provides the depth of the embedded reader.</param>
    /// <param name="index">The index to associate with a <paramref name="nesting"/>.</param>
    /// <param name="nesting">The level of nesting.</param>
    /// <returns>The index at the specified <paramref name="nesting"/>.</returns>
    public static int GetDepthIndex(this IDecorator<IDictionary<int, Dictionary<int, int>>> decorator, int readerDepth, int index, int nesting)
    {
        Validator.ThrowIfNull(decorator);

        var depthIndexes = decorator.Inner;
        if (depthIndexes.TryGetValue(nesting, out var row))
        {
            if (!row.TryGetValue(readerDepth, out _))
            {
                row.Add(readerDepth, index);
            }
        }
        else
        {
            depthIndexes.Add(nesting, new Dictionary<int, int>());
            if (nesting == 0)
            {
                depthIndexes[nesting].Add(readerDepth, index);
            }
            else
            {
                depthIndexes[nesting].Add(readerDepth, depthIndexes[nesting - 1][readerDepth]);
            }
        }
        return depthIndexes[nesting][readerDepth];
    }
}
