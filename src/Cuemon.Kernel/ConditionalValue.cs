using System;

namespace Cuemon
{
    /// <summary>
    /// Represents the base class to determine whether an operation was a success or not.
    /// </summary>
    public abstract class ConditionalValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalValue" /> class.
        /// </summary>
        /// <param name="succeeded">Indicates whether the operation was successful or not.</param>
        /// <param name="failure">The <see cref="Exception"/> to associate with a faulted operation.</param>
        protected ConditionalValue(bool succeeded, Exception failure)
        {
            Succeeded = succeeded;
            Failure = failure;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ConditionalValue"/> is succeeded.
        /// </summary>
        /// <value><c>true</c> if the operation succeeded; otherwise, <c>false</c>.</value>
        public bool Succeeded { get; }

        /// <summary>
        /// Gets the <see cref="Exception"/> that caused the faulted operation.
        /// </summary>
        /// <value>The deeper cause of the faulted operation.</value>
        public Exception Failure { get; }
    }

    /// <summary>
    /// Represents the base class to support the Try-Parse pattern of an operation that may be asynchronous in nature.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the operation.</typeparam>
    /// <seealso cref="ConditionalValue" />
    /// <remarks>Try-Parse pattern: https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/exceptions-and-performance</remarks>
    public abstract class ConditionalValue<TResult> : ConditionalValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalValue{TResult}"/> class.
        /// </summary>
        /// <param name="succeeded">Indicates whether the operation was successful (an instance of <typeparamref name="TResult"/> has been created) or not.</param>
        /// <param name="result">The value returned from the operation; otherwise the default value for <typeparamref name="TResult"/> of the <paramref name="result"/> parameter.</param>
        /// <param name="failure">The <see cref="Exception"/> to associate with a faulted operation.</param>
        protected ConditionalValue(bool succeeded, TResult result, Exception failure) : base(succeeded, failure)
        {
            Result = result;
        }

        /// <summary>
        /// Gets the result of the operation.
        /// </summary>
        /// <value>The result of the operation.</value>
        public TResult Result { get; }
    }
}
