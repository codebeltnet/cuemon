using System;

namespace Cuemon;
/// <summary>
/// Represents a generic way to provide information about arbitrary data.
/// </summary>
public class DataPair
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataPair"/> class.
    /// </summary>
    /// <param name="name">The name of the data pair.</param>
    /// <param name="value">The value of the data pair.</param>
    /// <param name="typeOf">The type of the data pair.</param>
    public DataPair(string name, object value, Type typeOf)
    {
        Validator.ThrowIfNullOrEmpty(name);
        Validator.ThrowIfNull(typeOf);
        Name = name;
        Value = value;
        Type = typeOf;
    }

    /// <summary>
    /// Gets the name of the data pair.
    /// </summary>
    /// <value>The name of the data pair.</value>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the value of the data pair.
    /// </summary>
    /// <value>The value of the data pair.</value>
    public object Value { get; private set; }

    /// <summary>
    /// Gets a value indicating whether <see cref="Value"/> is not null.
    /// </summary>
    /// <value><c>true</c> if <see cref="Value"/> is not null; otherwise, <c>false</c>.</value>
    public bool HasValue => Value != null;

    /// <summary>
    /// Gets the type of the data pair value.
    /// </summary>
    /// <value>The type of the data pair value.</value>
    public Type Type { get; protected set; }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return FormattableString.Invariant($"Name: {Name}, Value: {Value ?? "<null>"}, Type: {Type.Name}");
    }
}

/// <summary>
/// Represents a generic way to provide information about arbitrary data.
/// </summary>
/// <typeparam name="T">The type of the data value being represented by this instance.</typeparam>
public class DataPair<T> : DataPair
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataPair" /> class.
    /// </summary>
    /// <param name="name">The name of the data pair.</param>
    /// <param name="value">The value of the data pair.</param>
    public DataPair(string name, T value) : this(name, value, typeof(T))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataPair" /> class.
    /// </summary>
    /// <param name="name">The name of the data pair.</param>
    /// <param name="value">The value of the data pair.</param>
    /// <param name="typeOf">The type of the data pair.</param>
    public DataPair(string name, T value, Type typeOf) : base(name, value, typeOf)
    {
    }
}
