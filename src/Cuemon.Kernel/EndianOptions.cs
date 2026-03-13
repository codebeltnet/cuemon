using System;
using Cuemon.Configuration;

namespace Cuemon
{
    /// <summary>
    /// Configuration options for <see cref="BitConverter"/>.
    /// </summary>
    /// <seealso cref="IParameterObject"/>
    public class EndianOptions : IParameterObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndianOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="EndianOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="ByteOrder"/></term>
        ///         <description><code>BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian;</code></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public EndianOptions()
        {
            ByteOrder = BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian;
        }

        /// <summary>
        /// Gets or sets the endian byte order enumeration.
        /// </summary>
        /// <value>The byte order enumeration.</value>
        public Endianness ByteOrder { get; set; }
    }
}
