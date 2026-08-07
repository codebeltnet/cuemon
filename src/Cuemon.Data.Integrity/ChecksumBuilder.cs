using System;
using System.Collections.Generic;
using Cuemon.Security;

namespace Cuemon.Data.Integrity;
/// <summary>
/// Provides a way to fluently represent checksum values of arbitrary data.
/// </summary>
public class ChecksumBuilder : IDataIntegrity, IEquatable<ChecksumBuilder>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChecksumBuilder"/> class.
    /// </summary>
    /// <param name="hashFactory">The function delegate that is invoked to produce the <see cref="HashResult"/>.</param>
    public ChecksumBuilder(Func<Hash> hashFactory) : this(null, hashFactory)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChecksumBuilder"/> class.
    /// </summary>
    /// <param name="checksum">A <see cref="byte"/> array containing a checksum of the data this instance represents.</param>
    /// <param name="hashFactory">The function delegate that is invoked to produce the <see cref="HashResult"/>.</param>
    public ChecksumBuilder(byte[] checksum, Func<Hash> hashFactory)
    {
        Validator.ThrowIfNull(hashFactory);
        Bytes = checksum == null ? new List<byte>() : new List<byte>(checksum);
        HashFactory = hashFactory;
    }

    /// <summary>
    /// Gets the value factory of this instance.
    /// </summary>
    /// <value>The value factory of this instance.</value>
    protected Func<Hash> HashFactory { get; }

    /// <summary>
    /// Gets a byte array that is the result of the associated <see cref="ChecksumBuilder"/>.
    /// </summary>
    /// <value>The byte array that is the result of the associated <see cref="ChecksumBuilder"/>.</value>
    protected List<byte> Bytes { get; set; }

    /// <summary>
    /// Gets a <see cref="HashResult"/> containing a computed hash value of the data this instance represents.
    /// </summary>
    /// <value>A <see cref="HashResult"/> containing a computed hash value of the data this instance represents.</value>
    public HashResult Checksum => ComputedHash ??= HashFactory.Invoke().ComputeHash(Bytes.ToArray());

    /// <summary>
    /// Gets or sets the computed checksum of <see cref="Bytes"/>.
    /// </summary>
    /// <value>The computed checksum of <see cref="Bytes"/>.</value>
    protected HashResult ComputedHash { get; set; }

    /// <summary>
    /// Combines the <paramref name="additionalChecksum" /> to the representation of this instance.
    /// </summary>
    /// <param name="additionalChecksum">A <see cref="byte"/> array containing a checksum of the additional data this instance must represent.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public virtual ChecksumBuilder CombineWith(byte[] additionalChecksum)
    {
        ComputedHash = null;
        Bytes.AddRange(additionalChecksum);
        return this;
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode()
    {
        return Generate.HashCode32(Checksum.ToHexadecimalString());
    }

    /// <summary>
    /// Determines whether the specified <see cref="object" /> is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object" /> to compare with the current <see cref="System.Object" />.</param>
    /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
    public override bool Equals(object obj)
    {
        if (obj is not ChecksumBuilder builder) { return false; }
        return Equals(builder);
    }

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>. </returns>
    public virtual bool Equals(ChecksumBuilder other)
    {
        if (other == null) { return false; }
        return Checksum.GetHashCode() == other.Checksum.GetHashCode();
    }

    /// <summary>
    /// Converts the the <see cref="Bytes"/> of this instance to its equivalent hexadecimal representation.
    /// </summary>
    /// <returns>A hexadecimal representation of this instance.</returns>
    public override string ToString()
    {
        return Checksum.ToHexadecimalString();
    }
}
