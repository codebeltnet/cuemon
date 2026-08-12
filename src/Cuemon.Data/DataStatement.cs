using System;
using System.Data;
using System.Linq;

namespace Cuemon.Data;
/// <summary>
/// Represents a Transact-SQL statement or stored procedure to execute against a SQL Server database.
/// </summary>
public class DataStatement
{
    /// <summary>
    /// Performs an implicit conversion from the specified <paramref name="text"/> to <see cref="DataStatement"/>.
    /// </summary>
    /// <param name="text">The SQL statement or stored procedure to convert.</param>
    /// <returns>A <see cref="DataStatement"/> that is equivalent to <paramref name="text"/>.</returns>
    public static implicit operator DataStatement(string text)
    {
        return new DataStatement(text);
    }

    /// <summary>
	/// Initializes a new instance of the <see cref="DataStatement"/> class.
	/// </summary>
    /// <param name="text">The command text to execute.</param>
    /// <param name="setup">The <see cref="DataStatementOptions"/> which may be configured.</param>
    public DataStatement(string text, Action<DataStatementOptions> setup = null)
    {
        Validator.ThrowIfNullOrWhitespace(text);
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        Text = text;
        Type = options.Type;
        Timeout = options.Timeout;
        Parameters = options.Parameters.ToArray();
    }

    /// <summary>
    /// Gets the command text to execute.
    /// </summary>
    /// <value>The command text to execute.</value>
    public string Text { get; }

    /// <summary>
    /// Gets the command type value to execute.
    /// </summary>
    /// <value>The command type value to execute. Default type value is <see cref="CommandType.Text"/>.</value>
    public CommandType Type { get; }

    /// <summary>
    /// Gets the wait time before terminating the attempt to execute a command and generating an error.
    /// </summary>
    /// <value>The timespan to wait for the command to execute. Default value is 1 minute and 30 seconds.</value>
    public TimeSpan Timeout { get; }

    /// <summary>
    /// Gets the parameters associated with this data statement.
    /// </summary>
    /// <value>The parameters associated with this data statement.</value>
    public IDataParameter[] Parameters { get; }
}
