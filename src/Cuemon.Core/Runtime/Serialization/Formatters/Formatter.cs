using System;

namespace Cuemon.Runtime.Serialization.Formatters;
/// <summary>
/// Provides a set of static methods that complements serialization and deserialization of an object.
/// </summary>
public static class Formatter
{
    /// <summary>
    /// Gets the <see cref="Type"/> with the specified <paramref name="typeName"/>, performing a case-sensitive search.
    /// </summary>
    /// <param name="typeName">The assembly-qualified name of the type to get. See <see cref="Type.AssemblyQualifiedName"/>. If the type is in the currently executing assembly or in mscorlib.dll/System.Private.CoreLib.dll, it is sufficient to supply the type name qualified by its namespace.</param>
    /// <returns>The type with the specified <paramref name="typeName"/>. If the type is not found, null is returned.</returns>
    public static Type GetType(string typeName)
    {
        if (TryGetType(typeName, out var type))
        {
            return type;
        }
        return null;
    }

    /// <summary>
    /// Attempts to get the <see cref="Type"/> with the specified <paramref name="typeName"/>, performing a case-sensitive search.
    /// </summary>
    /// <param name="typeName">The assembly-qualified name of the type to get. See <see cref="Type.AssemblyQualifiedName"/>. If the type is in the currently executing assembly or in mscorlib.dll/System.Private.CoreLib.dll, it is sufficient to supply the type name qualified by its namespace.</param>
    /// <param name="type">The type with the specified <paramref name="typeName"/>. If the type is not found, null is returned.</param>
    /// <returns><c>true</c> if the <see cref="Type"/> was found, <c>false</c> otherwise.</returns>
    public static bool TryGetType(string typeName, out Type type)
    {
        if (typeName == null)
        {
            type = null;
            return false;
        }

        typeName = typeName.Trim();
        Patterns.TryInvoke(() => Type.GetType(typeName, false), out type);
        if (type != null) { return true; }

        foreach (var assemblyType in AppDomain.CurrentDomain.GetAssemblies())
        {
            Patterns.TryInvoke(() => assemblyType.GetType(typeName, false), out type);
            if (type != null) { return true; }
        }

        return false;
    }
}

/// <summary>
/// An abstract class that supports serialization and deserialization of an object, in a given format.
/// </summary>
/// <typeparam name="TFormat">The type of format which serialization and deserialization is invoked.</typeparam>
public abstract class Formatter<TFormat>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Formatter{TFormat}"/> class.
    /// </summary>
    protected Formatter()
    {
    }

    /// <summary>
    /// Serializes the specified <paramref name="source"/> to an object of <typeparamref name="TFormat" />.
    /// </summary>
    /// <param name="source">The object to serialize to a given format.</param>
    /// <returns>An object of the serialized <paramref name="source" />.</returns>
    public TFormat Serialize(object source)
    {
        Validator.ThrowIfNull(source);
        return Serialize(source, source.GetType());
    }

    /// <summary>
    /// Serializes the object of this instance to an object of <typeparamref name="TFormat" />.
    /// </summary>
    /// <param name="source">The object to serialize to a given format.</param>
    /// <param name="objectType">The type of the object to serialize.</param>
    /// <returns>An object of the serialized <paramref name="source"/>.</returns>
    public abstract TFormat Serialize(object source, Type objectType);

    /// <summary>
    /// Deserializes the specified <paramref name="value"/> of <typeparamref name="TFormat"/> into an object of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to return.</typeparam>
    /// <param name="value">The object from which to deserialize the object graph.</param>
    /// <returns>An object of <typeparamref name="T" />.</returns>
    public T Deserialize<T>(TFormat value)
    {
        Validator.ThrowIfNull(value);
        return (T)Deserialize(value, typeof(T));
    }

    /// <summary>
    /// Deserializes the specified <paramref name="value"/> of <typeparamref name="TFormat"/> into an object of <paramref name="objectType"/>.
    /// </summary>
    /// <param name="value">The object from which to deserialize the object graph.</param>
    /// <param name="objectType">The type of the deserialized object.</param>
    /// <returns>An object of <paramref name="objectType"/>.</returns>
    public abstract object Deserialize(TFormat value, Type objectType);
}
