using System;
using System.IO;
using Cuemon.IO;

namespace Cuemon.Data.Integrity
{
    /// <summary>
    /// Configuration options for <see cref="FileInfo"/>.
    /// </summary>
    public class FileIntegrityOptions : FileInfoOptions
    {
        private Func<FileInfo, byte[], IDataIntegrity> _integrityConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileIntegrityOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="FileIntegrityOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="IntegrityConverter"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public FileIntegrityOptions()
        {
        }

        /// <summary>
        /// Gets or sets the function delegate that will convert an instance of <see cref="FileInfo"/> and a <see cref="byte"/> array into an object implementing the <see cref="IDataIntegrity"/> interface.
        /// </summary>
        /// <value>The function delegate that returns an object implementing the <see cref="IDataIntegrity"/> interface.</value>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        public Func<FileInfo, byte[], IDataIntegrity> IntegrityConverter
        {
            get => _integrityConverter;
            set
            {
                Validator.ThrowIfNull(value);
                _integrityConverter = value;
            }
        }
    }
}