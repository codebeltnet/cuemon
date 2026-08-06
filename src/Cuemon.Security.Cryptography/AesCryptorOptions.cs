using System.Security.Cryptography;
using Cuemon.Configuration;

namespace Cuemon.Security.Cryptography;
/// <summary>
/// Configuration options for <see cref="AesCryptor"/>.
/// </summary>
/// <seealso cref="IParameterObject"/>
public class AesCryptorOptions : IParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AesCryptorOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="AesCryptorOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="Mode"/></term>
    ///         <description><see cref="CipherMode.CBC"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Padding"/></term>
    ///         <description><see cref="PaddingMode.PKCS7"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public AesCryptorOptions()
    {
        Padding = PaddingMode.PKCS7;
        Mode = CipherMode.CBC;
    }

    /// <summary>
    /// Gets or sets the padding mode used in the symmetric algorithm.
    /// </summary>
    /// <value>The padding mode used in the symmetric algorithm. The default is <see cref="PaddingMode.PKCS7"/>.</value>
    public PaddingMode Padding { get; set; }

    /// <summary>
    /// Gets or sets the mode for operation of the symmetric algorithm.
    /// </summary>
    /// <value>The mode for operation of the symmetric algorithm. The default is <see cref="CipherMode.CBC"/>.</value>
    public CipherMode Mode { get; set; }
}
