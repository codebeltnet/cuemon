using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Cuemon.Data.SqlClient;
/// <summary>
/// Provides a safe way to include a Transact-SQL WHERE clause with an IN operator to execute against a SQL Server database.
/// </summary>
/// <typeparam name="T">The type of the data in the IN operation of the WHERE clause to execute against a SQL Server database.</typeparam>
public class SqlInOperator<T> : InOperator<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InOperator{T}"/> class.
    /// </summary>
    /// <param name="parameterPrefixGenerator">The function delegate that generates a random prefix for a parameter name.</param>
    public SqlInOperator(Func<string> parameterPrefixGenerator = null) : base(parameterPrefixGenerator)
    {
    }

    /// <summary>
    /// A callback method that is responsible for the values passed to the <see cref="InOperator{T}.ToSafeResult(T[])"/> method.
    /// </summary>
    /// <param name="expression">An expression to test for a match in the IN operator.</param>
    /// <param name="index">The index of the <paramref name="expression"/>.</param>
    /// <returns>An <see cref="IDbDataParameter"/> representing the value of the <paramref name="expression"/>.</returns>
    protected override IDbDataParameter ParametersSelector(T expression, int index)
    {
        return new SqlParameter(string.Concat(ParameterPrefix, index), expression);
    }
}
