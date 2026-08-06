using System;
using System.ComponentModel;
using Cuemon.Configuration;

namespace Cuemon.Security.Cryptography;
/// <summary>
/// Configuration options for <see cref="AesCryptor.GenerateKey"/>.
/// </summary>
public class AesKeyOptions : IParameterObject
{
    private AesSize _size;
    private Func<AesSize, string> _randomStringProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AesKeyOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="AesKeyOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="RandomStringProvider"/></term>
    ///         <description>
    ///         <code>
    ///size =>
    ///{
    ///    if (size &gt; AesSize.Aes256 || size &lt; AesSize.Aes128) { throw new InvalidEnumArgumentException(nameof(size), (int)size, typeof(AesSize)); }
    ///    var characters = Alphanumeric.Letters + Alphanumeric.PunctuationMarks;
    ///    switch (size)
    ///    {
    ///        case AesSize.Aes128:
    ///            return Generate.RandomString(128 / ByteUnit.BitsPerByte, characters);
    ///        case AesSize.Aes192:
    ///            return Generate.RandomString(192 / ByteUnit.BitsPerByte, characters);
    ///        default:
    ///            return Generate.RandomString(256 / ByteUnit.BitsPerByte, characters);
    ///    }
    ///};
    ///         </code>
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Size"/></term>
    ///         <description><see cref="AesSize.Aes256"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public AesKeyOptions()
    {
        Size = AesSize.Aes256;
        RandomStringProvider = size =>
        {
            if (size > AesSize.Aes256 || size < AesSize.Aes128) { throw new InvalidEnumArgumentException(nameof(size), (int)size, typeof(AesSize)); }
            var characters = Alphanumeric.Letters + Alphanumeric.PunctuationMarks;
            switch (size)
            {
                case AesSize.Aes128:
                    return Generate.RandomString(128 / Convertible.BitsPerByte, characters);
                case AesSize.Aes192:
                    return Generate.RandomString(192 / Convertible.BitsPerByte, characters);
                default:
                    return Generate.RandomString(256 / Convertible.BitsPerByte, characters);
            }
        };
    }

    /// <summary>
    /// Gets or sets the function delegate that provides a random generated string.
    /// </summary>
    /// <value>The function delegate that provides a random generated string.</value>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    public Func<AesSize, string> RandomStringProvider
    {
        get => _randomStringProvider;
        set
        {
            Validator.ThrowIfNull(value);
            _randomStringProvider = value;
        }
    }

    /// <summary>
    /// Gets or sets the size of the Advanced Encryption Standard (AES) symmetric algorithm.
    /// </summary>
    /// <value>The size of the Advanced Encryption Standard (AES) symmetric algorithm.</value>
    /// <exception cref="InvalidEnumArgumentException">
    /// <paramref name="value"/> is not a valid value of <see cref="AesSize"/>.
    /// </exception>
    public AesSize Size
    {
        get => _size;
        set
        {
            if (value > AesSize.Aes256 || value < AesSize.Aes128) { throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(AesSize)); }
            _size = value;
        }
    }
}
