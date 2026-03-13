namespace Cuemon
{
    /// <summary>
    /// Provides a way to indicate a successful operation. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="ConditionalValue" />
    public sealed class SuccessfulValue : ConditionalValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessfulValue"/> class.
        /// </summary>
        public SuccessfulValue() : base(true, null)
        {
        }
    }

    /// <summary>
    /// Provides a way to indicate a successful operation. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the operation.</typeparam>
    /// <seealso cref="ConditionalValue{TResult}" />
    public sealed class SuccessfulValue<TResult> : ConditionalValue<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessfulValue{TResult}"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        public SuccessfulValue(TResult result) : base(true, result, null)
        {
        }
    }
}
