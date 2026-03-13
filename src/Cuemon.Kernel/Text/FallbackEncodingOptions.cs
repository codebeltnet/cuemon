using System;
using System.Text;

namespace Cuemon.Text
{
    /// <summary>
    /// Configuration options related to <see cref="Encoding"/>.
    /// </summary>
    /// <seealso cref="EncodingOptions" />
    public class FallbackEncodingOptions : EncodingOptions
    {
        private Encoding _targetEncoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="FallbackEncodingOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="FallbackEncodingOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="EncoderFallback"/></term>
        ///         <description><c>EncoderFallback.ExceptionFallback</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="DecoderFallback"/></term>
        ///         <description><c>DecoderFallback.ExceptionFallback</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public FallbackEncodingOptions()
        {
            EncoderFallback = EncoderFallback.ExceptionFallback;
            DecoderFallback = DecoderFallback.ExceptionFallback;
        }

        /// <summary>
        /// Gets or sets the object that provides an error-handling procedure when a character cannot be encoded.
        /// </summary>
        /// <value>The object that provides an error-handling procedure when a character cannot be encoded.</value>
        public EncoderFallback EncoderFallback { get; set; }

        /// <summary>
        /// Gets or sets the object that provides an error-handling procedure when a byte sequence cannot be decoded.
        /// </summary>
        /// <value>The object that provides an error-handling procedure when a byte sequence cannot be decoded.</value>
        public DecoderFallback DecoderFallback { get; set; }

        /// <summary>
        /// Gets or sets the target encoding for the operation.
        /// </summary>
        /// <value>The target encoding for the operation.</value>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        public Encoding TargetEncoding
        {
            get
            {
                if (_targetEncoding == null) { throw new InvalidOperationException($"{nameof(TargetEncoding)} cannot be null; an encoding must be specified."); }
                return _targetEncoding;
            }
            set
            {
                Validator.ThrowIfNull(value);
                _targetEncoding = value;
            }
        }
    }
}