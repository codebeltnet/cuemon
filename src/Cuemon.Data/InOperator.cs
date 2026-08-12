using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Cuemon.Data;
/// <summary>
/// Provides a safe way to include a Transact-SQL WHERE clause with an IN operator.
/// </summary>
public abstract class InOperator<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InOperator{T}" /> class.
    /// </summary>
    /// <param name="parameterPrefixGenerator">The function delegate that generates a random prefix for a parameter name.</param>
    protected InOperator(Func<string> parameterPrefixGenerator = null)
    {
        ParameterPrefix = parameterPrefixGenerator?.Invoke() ?? FormattableString.Invariant($"@param{Generate.RandomString(1, Alphanumeric.UppercaseLetters)}{Generate.RandomString(5, Alphanumeric.LowercaseLetters)}");
    }

    /// <summary>
    /// Gets the prefix of the parameter name that will be concatenated with <c>index</c> of both <see cref="ArgumentsSelector"/> and <see cref="ParametersSelector"/>.
    /// </summary>
    /// <value>The prefix of the parameter name that will be concatenated with <c>index</c>.</value>
    protected string ParameterPrefix { get; }

    /// <summary>
    /// A callback method that is responsible for the values passed to the <see cref="ToSafeResult(T[])"/> method.
    /// </summary>
    /// <param name="expression">An expression to test for a match in the IN operator.</param>
    /// <param name="index">The index of the <paramref name="expression"/>.</param>
    /// <returns>A <see cref="string"/> representing the argument of the <paramref name="expression"/>.</returns>
    /// <remarks>Default is <c>@param</c> concatenated with <see cref="ParameterPrefix"/> and <paramref name="index"/>, eg. <c>@paramAbcdef0</c>.</remarks>
    protected virtual string ArgumentsSelector(T expression, int index)
    {
        return string.Concat(ParameterPrefix, index);
    }

    /// <summary>
    /// A callback method that is responsible for the values passed to the <see cref="ToSafeResult(T[])"/> method.
    /// </summary>
    /// <param name="expression">An expression to test for a match in the IN operator.</param>
    /// <param name="index">The index of the <paramref name="expression"/>.</param>
    /// <returns>An <see cref="IDbDataParameter"/> representing the value of the <paramref name="expression"/>.</returns>
    protected abstract IDbDataParameter ParametersSelector(T expression, int index);

    /// <summary>
    /// Converts the specified sequence of <paramref name="expressions"/> to a SQL injection safe <see cref="InOperatorResult"/>.
    /// </summary>
    /// <param name="expressions">The expressions to test for a match in the IN operator of the WHERE clause.</param>
    /// <returns>A new instance of <see cref="InOperatorResult"/>.</returns>
    public InOperatorResult ToSafeResult(params T[] expressions)
    {
        return ToSafeResult(expressions as IEnumerable<T>);
    }

    /// <summary>
    /// Converts the specified sequence of <paramref name="expressions"/> to a SQL injection safe <see cref="InOperatorResult"/>.
    /// </summary>
    /// <param name="expressions">The expressions to test for a match in the IN operator of the WHERE clause.</param>
    /// <param name="argumentsStringConverter">The function delegate arguments string converter.</param>
    /// <returns>A new instance of <see cref="InOperatorResult"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="expressions"/> cannot be null.
    /// </exception>
    public InOperatorResult ToSafeResult(IEnumerable<T> expressions, Func<IEnumerable<string>, string> argumentsStringConverter = null)
    {
        Validator.ThrowIfNull(expressions);
        var elements = expressions as List<T> ?? new List<T>(expressions);
        return new InOperatorResult(elements.Select(ArgumentsSelector), elements.Select(ParametersSelector), argumentsStringConverter);
    }
}
