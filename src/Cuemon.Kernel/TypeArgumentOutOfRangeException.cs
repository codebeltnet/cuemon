using System;

namespace Cuemon
{
    /// <summary>
    /// The exception that is thrown when the value of an type argument is outside the allowable range of values as defined by the invoked method.
    /// </summary>
    public class TypeArgumentOutOfRangeException : ArgumentOutOfRangeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentOutOfRangeException"/> class.
        /// </summary>
        public TypeArgumentOutOfRangeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentOutOfRangeException"/> class.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        public TypeArgumentOutOfRangeException(string typeParamName) : this(typeParamName, "Specified type argument was out of the range of valid values.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentOutOfRangeException"/> class.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="message">The message that describes the error.</param>
        public TypeArgumentOutOfRangeException(string typeParamName, string message) : base(typeParamName, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentOutOfRangeException"/> class.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="actualValue">The value of the argument that causes this exception.</param>
        /// <param name="message">The message that describes the error.</param>
        public TypeArgumentOutOfRangeException(string typeParamName, object actualValue, string message) : base(typeParamName, actualValue, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentOutOfRangeException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public TypeArgumentOutOfRangeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
