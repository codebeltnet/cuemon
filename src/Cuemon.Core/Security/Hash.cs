using System;
using System.Collections.Generic;
using System.IO;
using Cuemon.Collections.Generic;
using Cuemon.Configuration;
using Cuemon.IO;
using Cuemon.Text;

namespace Cuemon.Security;
/// <summary>
/// Represents the base class from which all implementations of hash algorithms and checksums should derive.
/// </summary>
/// <typeparam name="TOptions">The type of the configured options.</typeparam>
/// <seealso cref="ConvertibleOptions"/>
/// <seealso cref="IConfigurable{TOptions}" />
/// <seealso cref="IHash" />
public abstract class Hash<TOptions> : Hash, IConfigurable<TOptions> where TOptions : ConvertibleOptions, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Hash{TOptions}"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    protected Hash(Action<TOptions> setup)
    {
        Options = Patterns.Configure(setup);
    }

    /// <summary>
    /// Gets the configured options of this instance.
    /// </summary>
    /// <value>The configured options of this instance.</value>
    public TOptions Options { get; }


    /// <summary>
    /// The endian-initializer of this instance.
    /// </summary>
    /// <param name="options">An instance of the configured options.</param>
    protected sealed override void EndianInitializer(EndianOptions options)
    {
        options.ByteOrder = Options.ByteOrder;
    }
}

/// <summary>
/// Represents the base class that defines the public facing structure to expose.
/// </summary>
/// <seealso cref="IHash" />
public abstract class Hash : IHash
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Hash"/> class.
    /// </summary>
    protected Hash()
    {
    }

    /// <summary>
    /// Computes the hash value for the specified <see cref="bool"/>.
    /// </summary>
    /// <param name="input">The <see cref="bool"/> to compute the hash code for.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public virtual HashResult ComputeHash(bool input)
    {
        return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
    }

    /// <summary>
    /// Computes the hash value for the specified <see cref="byte"/>.
    /// </summary>
    /// <param name="input">The <see cref="byte"/> to compute the hash code for.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public virtual HashResult ComputeHash(byte input)
    {
        return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
    }

    /// <summary>
    /// Computes the hash value for the specified <see cref="char"/>.
    /// </summary>
    /// <param name="input">The <see cref="char"/> to compute the hash code for.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public virtual HashResult ComputeHash(char input)
    {
        return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
    }

    /// <summary>
    /// Computes the hash value for the specified <see cref="DateTime"/>.
    /// </summary>
    /// <param name="input">The <see cref="DateTime"/> to compute the hash code for.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public virtual HashResult ComputeHash(DateTime input)
    {
        return ComputeHash(Convertible.GetBytes(input));
    }

    /// <summary>
    /// Computes the hash value for the specified <see cref="DBNull"/>.
    /// </summary>
    /// <param name="input">The <see cref="DBNull"/> to compute the hash code for.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public virtual HashResult ComputeHash(DBNull input)
    {
        return ComputeHash(Convertible.GetBytes(input));
    }

    /// <summary>
    /// Computes the hash value for the specified <see cref="decimal"/>.
    /// </summary>
    /// <param name="input">The <see cref="decimal"/> to compute the hash code for.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public virtual HashResult ComputeHash(decimal input)
    {
        return ComputeHash(Convertible.GetBytes(input));
    }

    /// <summary>
    /// Computes the hash value for the specified <see cref="double"/>.
    /// </summary>
    /// <param name="input">The <see cref="double"/> to compute the hash code for.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public virtual HashResult ComputeHash(double input)
    {
        return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
    }

    /// <summary>
    /// Computes the hash value for the specified <see cref="short"/>.
    /// </summary>
    /// <param name="input">The <see cref="short"/> to compute the hash code for.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public virtual HashResult ComputeHash(short input)
    {
        return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
    }

    /// <summary>
    /// Computes the hash value for the specified <see cref="int"/>.
    /// </summary>
    /// <param name="input">The <see cref="int"/> to compute the hash code for.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public virtual HashResult ComputeHash(int input)
    {
        return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
    }

    /// <summary>
    /// Computes the hash value for the specified <see cref="long"/>.
    /// </summary>
    /// <param name="input">The <see cref="long"/> to compute the hash code for.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public virtual HashResult ComputeHash(long input)
    {
        return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
    }

    /// <summary>
    /// Computes the hash value for the specified <see cref="sbyte"/>.
    /// </summary>
    /// <param name="input">The <see cref="sbyte"/> to compute the hash code for.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public virtual HashResult ComputeHash(sbyte input)
    {
        return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
    }

    /// <summary>
    /// Computes the hash value for the specified <see cref="float"/>.
    /// </summary>
    /// <param name="input">The <see cref="float"/> to compute the hash code for.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public virtual HashResult ComputeHash(float input)
    {
        return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
    }

    /// <summary>
    /// Computes the hash value for the specified <see cref="ushort"/>.
    /// </summary>
    /// <param name="input">The <see cref="ushort"/> to compute the hash code for.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public virtual HashResult ComputeHash(ushort input)
    {
        return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
    }

    /// <summary>
    /// Computes the hash value for the specified <see cref="uint"/>.
    /// </summary>
    /// <param name="input">The <see cref="uint"/> to compute the hash code for.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public virtual HashResult ComputeHash(uint input)
    {
        return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
    }

    /// <summary>
    /// Computes the hash value for the specified <see cref="ulong"/>.
    /// </summary>
    /// <param name="input">The <see cref="ulong"/> to compute the hash code for.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public virtual HashResult ComputeHash(ulong input)
    {
        return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
    }

    /// <summary>
    /// Computes the hash value for the specified <see cref="string"/>.
    /// </summary>
    /// <param name="input">The <see cref="string"/> to compute the hash code for.</param>
    /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public virtual HashResult ComputeHash(string input, Action<EncodingOptions> setup = null)
    {
        return ComputeHash(Convertible.GetBytes(input, setup));
    }

    /// <summary>
    /// Computes the hash value for the specified <see cref="Enum"/>.
    /// </summary>
    /// <param name="input">The <see cref="Enum"/> to compute the hash code for.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public virtual HashResult ComputeHash(Enum input)
    {
        return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
    }

    /// <summary>
    /// Computes the hash value for the specified <see cref="IConvertible"/> array.
    /// </summary>
    /// <param name="input">The <see cref="IConvertible"/> array to compute the hash code for.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public virtual HashResult ComputeHash(params IConvertible[] input)
    {
        return ComputeHash(Arguments.ToEnumerableOf(input));
    }

    /// <summary>
    /// Computes the hash value for the specified sequence of <see cref="IConvertible"/>.
    /// </summary>
    /// <param name="input">The sequence of <see cref="IConvertible"/> to compute the hash code for.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public virtual HashResult ComputeHash(IEnumerable<IConvertible> input)
    {
        return ComputeHash(Convertible.GetBytes(input));
    }

    /// <summary>
    /// Computes the hash value for the specified <see cref="byte"/> array.
    /// </summary>
    /// <param name="input">The <see cref="byte"/> array to compute the hash code for.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public abstract HashResult ComputeHash(byte[] input);

    /// <summary>
    /// Computes the hash value for the specified <see cref="Stream"/>.
    /// </summary>
    /// <param name="input">The <see cref="Stream"/> to compute the hash code for.</param>
    /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
    public virtual HashResult ComputeHash(Stream input)
    {
        return ComputeHash(Patterns.SafeInvoke(() => new MemoryStream(), destination =>
        {
            Decorator.Enclose(input).CopyStream(destination);
            return destination;
        }).ToArray());
    }

    /// <summary>
    /// Defines the initializer that <see cref="Hash{TOptions}"/> must implement.
    /// </summary>
    /// <param name="options">An instance of the configured options.</param>
    protected abstract void EndianInitializer(EndianOptions options);
}
