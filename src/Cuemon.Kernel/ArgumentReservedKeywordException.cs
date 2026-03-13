using System;

namespace Cuemon
{
    /// <summary>
    /// The exception that is thrown when the value of an argument is a reserved keyword.
    /// </summary>
    public class ArgumentReservedKeywordException : ArgumentOutOfRangeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentReservedKeywordException"/> class.
        /// </summary>
        public ArgumentReservedKeywordException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentReservedKeywordException"/> class.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        public ArgumentReservedKeywordException(string paramName) : this(paramName, (string)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentReservedKeywordException"/> class.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">The message that describes the error.</param>
        public ArgumentReservedKeywordException(string paramName, string message) : base(paramName, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentReservedKeywordException"/> class.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="actualValue">The value of the argument that causes this exception.</param>
        /// <param name="message">The message that describes the error.</param>
        public ArgumentReservedKeywordException(string paramName, string actualValue, string message) : base(paramName, actualValue, message ?? "Specified argument is a reserved keyword.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentReservedKeywordException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public ArgumentReservedKeywordException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
