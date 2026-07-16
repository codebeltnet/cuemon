using System;
using System.Globalization;
using Cuemon.Configuration;

namespace Cuemon
{
    /// <summary>
    /// Configuration options for <see cref="IFormatProvider"/>.
    /// </summary>
    /// <seealso cref="IParameterObject"/>
    public class FormattingOptions : IValidatableParameterObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormattingOptions"/> class.
        /// </summary>
        public FormattingOptions()
        {
            FormatProvider = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Gets or sets the <see cref="IFormatProvider"/> that provides a mechanism for retrieving an object to control formatting.
        /// </summary>
        /// <value>The <see cref="IFormatProvider"/> that provides a mechanism for retrieving an object to control formatting.</value>
        public IFormatProvider FormatProvider { get; set; }

        /// <inheritdoc />
        public virtual void ValidateOptions()
        {
            Validator.ThrowIfInvalidState(FormatProvider == null);
        }
    }
}
