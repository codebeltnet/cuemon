using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Cuemon.Data;
/// <summary>
/// Provides the result of an <see cref="InOperator{T}"/> operation.
/// </summary>
public class InOperatorResult
{
    internal InOperatorResult(IEnumerable<string> arguments, IEnumerable<IDataParameter> parameters, Func<IEnumerable<string>, string> argumentsStringConverter)
    {
        Arguments = arguments;
        Parameters = parameters;
        ArgumentsStringConverter = argumentsStringConverter ?? (args => DelimitedString.Create(args));
    }

    private Func<IEnumerable<string>, string> ArgumentsStringConverter { get; }

    /// <summary>
    /// Gets the arguments for the IN operator.
    /// </summary>
    /// <value>The arguments for the IN operator.</value>
    /// <remarks>Default format of arguments is <c>@paramAbcdef0, @paramAbcdef1, @paramAbcdef2</c>, etc. and is controlled by the <see cref="InOperator{T}.ArgumentsSelector"/> method.</remarks>
    public IEnumerable<string> Arguments { get; }

    /// <summary>
    /// Gets the parameters for the IN operator.
    /// </summary>
    /// <value>The parameters for the IN operator.</value>
    public IEnumerable<IDataParameter> Parameters { get; }

    /// <summary>
    /// Converts the parameters for the IN operator to an <see cref="IDbDataParameter"/> array.
    /// </summary>
    /// <returns>An array of <see cref="IDbDataParameter"/>.</returns>
    public IDataParameter[] ToParametersArray()
    {
        return Parameters.ToArray();
    }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return ArgumentsStringConverter.Invoke(Arguments);
    }
}
