using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cuemon.Configuration;

namespace Cuemon.Runtime.Serialization.Formatters;
/// <summary>
/// Serializes and deserializes an object, in <see cref="Stream"/> format.
/// </summary>
/// <seealso cref="Formatter{TFormat}" />.
public abstract class StreamFormatter<TOptions> : Formatter<Stream>, IConfigurable<TOptions> where TOptions : class, IParameterObject, new()
{
    private static readonly Type OptionsType = typeof(TOptions);
    private static readonly List<Type> StreamFormatterTypes = OptionsType.Assembly.GetTypes().Where(type => type.BaseType == typeof(StreamFormatter<TOptions>)).ToList();

    /// <summary>
    /// Serializes the specified <paramref name="source"/> to an object of <see cref="Stream"/>.
    /// </summary>
    /// <param name="source">The object to serialize to <see cref="Stream"/> format.</param>
    /// <returns>A <see cref="Stream"/> of the serialized <paramref name="source"/>.</returns>
    public static Stream SerializeObject(object source)
    {
        return SerializeObject(source, null, (Action<TOptions>)null);
    }

    /// <summary>
    /// Serializes the specified <paramref name="source"/> to an object of <see cref="Stream"/>.
    /// </summary>
    /// <param name="source">The object to serialize to <see cref="Stream"/> format.</param>
    /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Stream"/> of the serialized <paramref name="source"/>.</returns>
    public static Stream SerializeObject(object source, Action<TOptions> setup)
    {
        return SerializeObject(source, null, setup);
    }

    /// <summary>
    /// Serializes the specified <paramref name="source"/> to an object of <see cref="Stream"/>.
    /// </summary>
    /// <param name="source">The object to serialize to <see cref="Stream"/> format.</param>
    /// <param name="objectType">The type of the object to serialize.</param>
    /// <returns>A <see cref="Stream"/> of the serialized <paramref name="source"/>.</returns>
    public static Stream SerializeObject(object source, Type objectType)
    {
        return SerializeObject(source, objectType, (Action<TOptions>)null);
    }

    /// <summary>
    /// Serializes the specified <paramref name="source"/> to an object of <see cref="Stream"/>.
    /// </summary>
    /// <param name="source">The object to serialize to <see cref="Stream"/> format.</param>
    /// <param name="objectType">The type of the object to serialize.</param>
    /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Stream"/> of the serialized <paramref name="source"/>.</returns>
    public static Stream SerializeObject(object source, Type objectType, Action<TOptions> setup)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        return SerializeObject(source, objectType, options);
    }

    /// <summary>
    /// Serializes the specified <paramref name="source"/> to an object of <see cref="Stream"/>.
    /// </summary>
    /// <param name="source">The object to serialize to <see cref="Stream"/> format.</param>
    /// <param name="options">The configured <typeparamref name="TOptions"/>.</param>
    /// <returns>A <see cref="Stream"/> of the serialized <paramref name="source"/>.</returns>
    public static Stream SerializeObject(object source, TOptions options)
    {
        return SerializeObject(source, null, options);
    }

    /// <summary>
    /// Serializes the specified <paramref name="source"/> to an object of <see cref="Stream"/>.
    /// </summary>
    /// <param name="source">The object to serialize to <see cref="Stream"/> format.</param>
    /// <param name="objectType">The type of the object to serialize.</param>
    /// <param name="options">The configured <typeparamref name="TOptions"/>.</param>
    /// <returns>A <see cref="Stream"/> of the serialized <paramref name="source"/>.</returns>
    public static Stream SerializeObject(object source, Type objectType, TOptions options)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfInvalidOptions(options);
        var formatter = GetFormatter(options);
        return formatter!.Serialize(source, objectType ?? source?.GetType());
    }

    /// <summary>
    /// Deserializes the specified <paramref name="value"/> into an object of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to return.</typeparam>
    /// <param name="value">The object from which to deserialize the object graph.</param>
    /// <returns>An object of <typeparamref name="T" />.</returns>
    public static T DeserializeObject<T>(Stream value)
    {
        return (T)DeserializeObject(value, typeof(T), (Action<TOptions>)null);
    }

    /// <summary>
    /// Deserializes the specified <paramref name="value"/> into an object of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to return.</typeparam>
    /// <param name="value">The object from which to deserialize the object graph.</param>
    /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
    /// <returns>An object of <typeparamref name="T" />.</returns>
    public static T DeserializeObject<T>(Stream value, Action<TOptions> setup)
    {
        return (T)DeserializeObject(value, typeof(T), setup);
    }

    /// <summary>
    /// Deserializes the specified <paramref name="value" /> into an object of <paramref name="objectType"/>.
    /// </summary>
    /// <param name="value">The string from which to deserialize the object graph.</param>
    /// <param name="objectType">The type of the deserialized object.</param>
    /// <returns>An object of <paramref name="objectType"/>.</returns>
    public static object DeserializeObject(Stream value, Type objectType)
    {
        return DeserializeObject(value, objectType, (Action<TOptions>)null);
    }

    /// <summary>
    /// Deserializes the specified <paramref name="value" /> into an object of <paramref name="objectType"/>.
    /// </summary>
    /// <param name="value">The string from which to deserialize the object graph.</param>
    /// <param name="objectType">The type of the deserialized object.</param>
    /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
    /// <returns>An object of <paramref name="objectType"/>.</returns>
    public static object DeserializeObject(Stream value, Type objectType, Action<TOptions> setup)
    {
        Validator.ThrowIfNull(value);
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        return DeserializeObject(value, objectType, options);
    }

    /// <summary>
    /// Deserializes the specified <paramref name="value"/> into an object of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to return.</typeparam>
    /// <param name="value">The object from which to deserialize the object graph.</param>
    /// <param name="options">The configured <typeparamref name="TOptions"/>.</param>
    /// <returns>An object of <typeparamref name="T" />.</returns>
    public static T DeserializeObject<T>(Stream value, TOptions options)
    {
        return (T)DeserializeObject(value, typeof(T), options);
    }

    /// <summary>
    /// Deserializes the specified <paramref name="value" /> into an object of <paramref name="objectType"/>.
    /// </summary>
    /// <param name="value">The string from which to deserialize the object graph.</param>
    /// <param name="objectType">The type of the deserialized object.</param>
    /// <param name="options">The configured <typeparamref name="TOptions"/>.</param>
    /// <returns>An object of <paramref name="objectType"/>.</returns>
    public static object DeserializeObject(Stream value, Type objectType, TOptions options)
    {
        Validator.ThrowIfNull(value);
        Validator.ThrowIfInvalidOptions(options);
        var formatter = GetFormatter(options);
        return formatter!.Deserialize(value, objectType);
    }

    private static StreamFormatter<TOptions> GetFormatter(TOptions options)
    {
        var formatter = StreamFormatterTypes.SingleOrDefault();
        var constructor = formatter?.GetConstructor(new[] { OptionsType });
        return constructor?.Invoke(new object[] { options }) as StreamFormatter<TOptions> ?? throw new InvalidOperationException($"Cannot resolve a StreamFormatter{nameof(TOptions)} implementation from {OptionsType.Assembly.FullName}; try a concrete instantiation.");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StreamFormatter{TOptions}"/> class.
    /// </summary>
    protected StreamFormatter() : this((Action<TOptions>)null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StreamFormatter{TOptions}"/> class.
    /// </summary>
    /// <param name="setup">The setup delegate which need to be configured.</param>
    protected StreamFormatter(Action<TOptions> setup) : this(Patterns.Configure(setup))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StreamFormatter{TOptions}"/> class.
    /// </summary>
    /// <param name="options">The configured options.</param>
    protected StreamFormatter(TOptions options)
    {
        Validator.ThrowIfNull(options);
        Options = options;
    }

    /// <summary>
    /// Gets the configured options of this <see cref="StreamFormatter{TOptions}"/>.
    /// </summary>
    /// <value>The configured options of this <see cref="StreamFormatter{TOptions}"/>.</value>
    public TOptions Options { get; }
}
