using System;

namespace Cuemon;
/// <summary>
/// Provides a way to change any instance of the same generic type.
/// </summary>
public static class Tweaker
{
    /// <summary>
    /// Adjust the specified <paramref name="value"/> with the function delegate <paramref name="converter"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value to convert.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <param name="converter">The function delegate that will convert the specified <paramref name="value"/>.</param>
    /// <returns>The <paramref name="value"/> in its original or converted form.</returns>
    /// <remarks>This is thought to be a more severe change than the one provided by <see cref="Alter{T}"/> (e.g., potentially convert the entire <paramref name="value"/> to a new instance).</remarks>
    public static T Adjust<T>(T value, Func<T, T> converter)
    {
        return converter == null ? value : converter.Invoke(value);
    }

    /// <summary>
    /// Adjust the specified <paramref name="value"/> with the <paramref name="modifier"/> delegate.
    /// </summary>
    /// <typeparam name="T">The type of the value to adjust.</typeparam>
    /// <param name="value">The value to adjust.</param>
    /// <param name="modifier">The delegate that will adjust the specified <paramref name="value"/>.</param>
    /// <returns>The <paramref name="value"/> in its original or adjusted form.</returns>
    /// <remarks>This is thought to be a more relaxed change than the one provided by <see cref="Adjust{T}"/> (e.g., applying changes only to the current <paramref name="value"/>).</remarks>
    public static T Alter<T>(T value, Action<T> modifier)
    {
        modifier?.Invoke(value);
        return value;
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
    public static TResult Change<T, TResult>(T value, Func<T, TResult> converter)
    {
        Validator.ThrowIfNull(value);
        return converter == null ? default : converter(value);
    }
}
