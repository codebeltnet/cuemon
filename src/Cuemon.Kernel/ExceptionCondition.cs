using System;

namespace Cuemon
{
    /// <summary>
    /// Provides a fluent and generic way to setup a condition for raising an <see cref="Exception" />.
    /// </summary>
    /// <typeparam name="TException">The type of the <see cref="Exception"/>.</typeparam>
    public class ExceptionCondition<TException> where TException : Exception
    {
        /// <summary>
        /// Indicates that the specified function delegate <paramref name="condition"/> must evaluate <c>true</c>.
        /// </summary>
        /// <param name="condition">The function delegate that determines if an <see cref="Exception"/> is thrown.</param>
        /// <returns>An <see cref="ExceptionHandler{T}"/> with the specified <paramref name="condition"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> cannot be null.
        /// </exception>
        public ExceptionHandler<TException> IsTrue(Func<bool> condition)
        {
            Validator.ThrowIfNull(condition);
            return new ExceptionHandler<TException>(condition, true);
        }

        /// <summary>
        /// Indicates that the specified function delegate <paramref name="condition"/> must evaluate <c>true</c>.
        /// </summary>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <param name="condition">The function delegate that determines if an <see cref="Exception"/> is thrown.</param>
        /// <returns>An <see cref="ExceptionHandler{T}"/> with the specified <paramref name="condition"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> cannot be null.
        /// </exception>
        public ExceptionHandler<TException, TResult> IsTrue<TResult>(TesterFunc<TResult, bool> condition)
        {
            Validator.ThrowIfNull(condition);
            return new ExceptionHandler<TException, TResult>(condition, true);
        }

        /// <summary>
        /// Indicates that the specified function delegate <paramref name="condition"/> must evaluate <c>false</c>.
        /// </summary>
        /// <param name="condition">The function delegate that determines if an <see cref="Exception"/> is thrown.</param>
        /// <returns>An <see cref="ExceptionHandler{T}"/> with the specified <paramref name="condition"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> cannot be null.
        /// </exception>
        public ExceptionHandler<TException> IsFalse(Func<bool> condition)
        {
            Validator.ThrowIfNull(condition);
            return new ExceptionHandler<TException>(condition, false);
        }

        /// <summary>
        /// Indicates that the specified function delegate <paramref name="condition"/> must evaluate <c>false</c>.
        /// </summary>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <param name="condition">The function delegate that determines if an <see cref="Exception"/> is thrown.</param>
        /// <returns>An <see cref="ExceptionHandler{T}"/> with the specified <paramref name="condition"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> cannot be null.
        /// </exception>
        public ExceptionHandler<TException, TResult> IsFalse<TResult>(TesterFunc<TResult, bool> condition)
        {
            Validator.ThrowIfNull(condition);
            return new ExceptionHandler<TException, TResult>(condition, false);
        }
    }

    /// <summary>
    /// Provides a generic way to handle an <see cref="Exception"/>.
    /// </summary>
    /// <typeparam name="TException">The type of the <see cref="Exception"/>.</typeparam>
    public class ExceptionHandler<TException> where TException : Exception
    {
        internal ExceptionHandler(Func<bool> condition, bool expected)
        {
            Condition = condition;
            Expected = expected;
        }

        private bool Expected { get; }

        private Func<bool> Condition { get; }

        /// <summary>
        /// Specifies the function delegate that determines the <see cref="Exception"/> to be thrown.
        /// </summary>
        /// <param name="handler">The function delegate that determines the <see cref="Exception"/> to be thrown.</param>
        /// <returns>An <see cref="ExceptionInvoker{T}"/> with the specified <paramref name="handler"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> cannot be null.
        /// </exception>
        public ExceptionInvoker<TException> Create(Func<TException> handler)
        {
            Validator.ThrowIfNull(handler);
            return new ExceptionInvoker<TException>(Condition, Expected, handler);
        }
    }

    /// <summary>
    /// Provides a generic way to handle an <see cref="Exception"/>.
    /// </summary>
    /// <typeparam name="TException">The type of the <see cref="Exception"/>.</typeparam>
    /// <typeparam name="TResult">The type of the out result value of a <see cref="TesterFunc{TResult,TSuccess}"/>.</typeparam>
    public class ExceptionHandler<TException, TResult> where TException : Exception
    {
        internal ExceptionHandler(TesterFunc<TResult, bool> condition, bool expected)
        {
            TesterCondition = condition;
            Expected = expected;
        }

        private bool Expected { get; }

        private TesterFunc<TResult, bool> TesterCondition { get; }

        /// <summary>
        /// Specifies the function delegate that determines the <see cref="Exception"/> to be thrown.
        /// </summary>
        /// <param name="handler">The function delegate that determines the <see cref="Exception"/> to be thrown.</param>
        /// <returns>An <see cref="ExceptionInvoker{T}"/> with the specified <paramref name="handler"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> cannot be null.
        /// </exception>
        public ExceptionInvoker<TException, TResult> Create(Func<TResult, TException> handler)
        {
            Validator.ThrowIfNull(handler);
            return new ExceptionInvoker<TException, TResult>(TesterCondition, Expected, handler);
        }
    }

    /// <summary>
    /// Provides a generic way to throw an <see cref="Exception"/>.
    /// </summary>
    /// <typeparam name="TException">The type of the <see cref="Exception"/>.</typeparam>
    public class ExceptionInvoker<TException> where TException : Exception
    {
        internal ExceptionInvoker(Func<bool> condition, bool expected, Func<TException> handler)
        {
            Condition = condition;
            Handler = handler;
            Expected = expected;
        }

        private Func<bool> Condition { get; }

        private Func<TException> Handler { get; }

        private bool Expected { get; }

        /// <summary>
        /// Determines whether an <see cref="Exception"/> of type <typeparamref name="TException"/> should be thrown.
        /// </summary>
        public void TryThrow()
        {
            if (Condition() == Expected) { throw Handler(); }
        }
    }

    /// <summary>
    /// Provides a generic way to throw an <see cref="Exception"/>.
    /// </summary>
    /// <typeparam name="TException">The type of the <see cref="Exception"/>.</typeparam>
    /// <typeparam name="TResult">The type of the out result value of a <see cref="TesterFunc{TResult,TSuccess}"/>.</typeparam>
    public class ExceptionInvoker<TException, TResult> where TException : Exception
    {
        internal ExceptionInvoker(TesterFunc<TResult, bool> condition, bool expected, Func<TResult, TException> handler)
        {
            TesterCondition = condition;
            Handler = handler;
            Expected = expected;
        }

        private TesterFunc<TResult, bool> TesterCondition { get; }

        private Func<TResult, TException> Handler { get; }

        private bool Expected { get; }

        /// <summary>
        /// Determines whether an <see cref="Exception"/> of type <typeparamref name="TException"/> should be thrown.
        /// </summary>
        public void TryThrow()
        {
            if (TesterCondition(out var result) == Expected) { throw Handler(result); }
        }
    }
}
