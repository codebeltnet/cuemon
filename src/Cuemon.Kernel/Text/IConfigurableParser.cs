using System;
using Cuemon.Configuration;

namespace Cuemon.Text;
/// <summary>
/// Defines methods that converts a <see cref="string"/> to an <see cref="object"/> of a particular type.
/// </summary>
/// <typeparam name="TOptions">The type of the delegate setup.</typeparam>
/// <seealso cref="IParser" />
public interface IConfigurableParser<out TOptions> where TOptions : class, IParameterObject, new()
{
    /// <summary>
    /// Converts the specified <paramref name="input"/> to an object of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to return.</typeparam>
    /// <param name="input">The string to convert.</param>
    /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
    /// <returns>An object of <typeparamref name="T"/> equivalent to <paramref name="input"/>.</returns>
    T Parse<T>(string input, Action<TOptions> setup = null);

    /// <summary>
    /// Converts the specified <paramref name="input"/> to an object of <paramref name="targetType"/>.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <param name="targetType">The type of the object to return.</param>
    /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
    /// <returns>An object of <paramref name="targetType"/> equivalent to <paramref name="input"/>.</returns>
    object Parse(string input, Type targetType, Action<TOptions> setup = null);

    /// <summary>
    /// Converts the specified <paramref name="input"/> to an object of <typeparamref name="T"/>. A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <typeparam name="T">The type of the object to return.</typeparam>
    /// <param name="input">The string to convert.</param>
    /// <param name="result">When this method returns, contains the object of <typeparamref name="T"/> equivalent to <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
    /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
    /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
    bool TryParse<T>(string input, out T result, Action<TOptions> setup = null);

    /// <summary>
    /// Converts the specified <paramref name="input"/> to an object of <paramref name="targetType"/>. A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <param name="targetType">The type of the object to return.</param>
    /// <param name="result">When this method returns, contains the object of <paramref name="targetType"/> equivalent to <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
    /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
    /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
    bool TryParse(string input, Type targetType, out object result, Action<TOptions> setup = null);
}

/// <summary>
/// Defines methods that converts a <see cref="string"/> to a generic object of <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TResult">The type of the converted result.</typeparam>
/// <typeparam name="TOptions">The type of the delegate setup.</typeparam>
public interface IConfigurableParser<TResult, out TOptions> where TOptions : class, new()
{
    /// <summary>
    /// Converts the <paramref name="input"/> to its <typeparamref name="TResult"/> equivalent.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
    /// <returns>A <typeparamref name="TResult"/> equivalent to <paramref name="input"/>.</returns>
    TResult Parse(string input, Action<TOptions> setup = null);

    /// <summary>
    /// Converts the <paramref name="input"/> to its <typeparamref name="TResult"/> equivalent. A return input indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <param name="result">When this method returns, contains the <typeparamref name="TResult"/> equivalent of the <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
    /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
    /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
    bool TryParse(string input, out TResult result, Action<TOptions> setup = null);
}
