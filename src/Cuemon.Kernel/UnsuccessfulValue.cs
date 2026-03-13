using System;

namespace Cuemon
{
    /// <summary>
    /// Provides a way to indicate a faulted operation. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="ConditionalValue" />
    public sealed class UnsuccessfulValue : ConditionalValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulValue"/> class.
        /// </summary>
        /// <param name="failure">The <see cref="Exception"/> that caused the faulted operation.</param>
        public UnsuccessfulValue(Exception failure = null) : base(false, failure)
        {
        }
    }

    /// <summary>
    /// Provides a way to indicate a faulted operation. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the operation.</typeparam>
    /// <seealso cref="ConditionalValue{TResult}" />
    public sealed class UnsuccessfulValue<TResult> : ConditionalValue<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulValue{TResult}"/> class.
        /// </summary>
        /// <param name="result">The optional value of result.</param>
        public UnsuccessfulValue(TResult result = default) : this(null, result)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulValue{TResult}"/> class.
        /// </summary>
        /// <param name="failure">The <see cref="Exception"/> that caused the faulted operation.</param>
        /// <param name="result">The optional value of result.</param>
        public UnsuccessfulValue(Exception failure, TResult result = default) : base(false, result, failure)
        {
        }
    }
}
