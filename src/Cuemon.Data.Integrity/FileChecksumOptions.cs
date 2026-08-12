using System.IO;
using Cuemon.IO;

namespace Cuemon.Data.Integrity;
/// <summary>
/// Configuration options for <see cref="FileInfo"/>.
/// </summary>
public class FileChecksumOptions : FileInfoOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileChecksumOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="FileChecksumOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="Method"/></term>
    ///         <description><see cref="EntityDataIntegrityMethod.Unaltered"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public FileChecksumOptions()
    {
        Method = EntityDataIntegrityMethod.Unaltered;
    }


    /// <summary>
    /// Gets an enumeration value of <see cref="EntityDataIntegrityMethod"/> indicating how a checksum is generated.
    /// </summary>
    /// <value>One of the enumeration values of <see cref="EntityDataIntegrityMethod"/> that indicates how a checksum is generated.</value>
    public EntityDataIntegrityMethod Method { get; set; }
}
