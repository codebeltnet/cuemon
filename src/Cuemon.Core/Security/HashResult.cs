using System;

namespace Cuemon.Security;
/// <summary>
/// Represents the result of a computed checksum operation.
/// </summary>
public class HashResult : IEquatable<HashResult>
{
    private readonly byte[] _input;

    /// <summary>
    /// Initializes a new instance of the <see cref="HashResult"/> class.
    /// </summary>
    /// <param name="input">The computed checksum represented as a <see cref="byte"/> array.</param>
    public HashResult(byte[] input)
    {
        _input = input ?? Array.Empty<byte>();
    }

    /// <summary>
    /// Gets a value indicating whether this instance has a checksum representation that consist of at least one byte.
    /// </summary>
    /// <value><c>true</c> if this instance has a checksum representation that consist of at least one byte; otherwise, <c>false</c>.</value>
    public bool HasValue => _input.Length > 0;

    /// <summary>
    /// Creates a copy of the original value that reflects a computed operation.
    /// </summary>
    /// <value>The copy of the original value that reflects a computed operation.</value>
    public byte[] GetBytes()
    {
        if (_input.Length == 0) { return Array.Empty<byte>(); }
        var copy = new byte[_input.Length];
        Array.Copy(_input, copy, copy.Length);
        return copy;
    }

    /// <summary>
    /// Converts the underlying value of this instance to its equivalent hexadecimal representation.
    /// </summary>
    /// <returns>A <see cref="string"/> representation, in hexadecimal, of the contents of the underlying value of this instance.</returns>
    public virtual string ToHexadecimalString()
    {
        return StringFactory.CreateHexadecimal(_input);
    }

    /// <summary>
    /// Converts the underlying value of this instance to its equivalent string representation that is encoded with base-64 digits.
    /// </summary>
    /// <returns>A <see cref="string"/> representation, in base 64, of the contents of the underlying value of this instance.</returns>
    public virtual string ToBase64String()
    {
        return Convert.ToBase64String(_input);
    }

    /// <summary>
    /// Converts the underlying value of this instance to its equivalent string representation that is encoded with base-64 digits, which is usable for transmission on the URL.
    /// </summary>
    /// <returns>A <see cref="string"/> representation, in base 64 which is usable for transmission on the URL, of the contents of the underlying value of this instance.</returns>
    public virtual string ToUrlEncodedBase64String()
    {
        return StringFactory.CreateUrlEncodedBase64(_input);
    }

    /// <summary>
    /// Converts the underlying value of this instance to its equivalent binary representation.
    /// </summary>
    /// <returns>A <see cref="string"/> representation, in binary, of the contents of the underlying value of this instance.</returns>
    public virtual string ToBinaryString()
    {
        return StringFactory.CreateBinaryDigits(_input);
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode()
    {
        return _input.GetHashCode();
    }

    /// <summary>
    /// Determines whether the specified <see cref="object" /> is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object" /> to compare with the current <see cref="System.Object" />.</param>
    /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
    public override bool Equals(object obj)
    {
        if (obj is not HashResult hr) { return false; }
        return Equals(hr);
    }

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>. </returns>
    public virtual bool Equals(HashResult other)
    {
        if (other == null) { return false; }
        return GetHashCode() == other.GetHashCode();
    }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return ToHexadecimalString();
    }

    /// <summary>
    /// Provides a generic converter of a <see cref="byte"/> array.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="converter">The function delegate that takes the underlying value of this instance and converts it into <typeparamref name="T"/>.</param>
    /// <returns>An instance or value of <typeparamref name="T"/>.</returns>
    public T To<T>(Func<byte[], T> converter)
    {
        return converter(_input);
    }
}
