using System;
using System.Data;
using Cuemon.Configuration;

namespace Cuemon.Data;
/// <summary>
/// Configuration options for <see cref="DataStatement"/>.
/// </summary>
public class DataStatementOptions : IValidatableParameterObject
{
    /// <summary>
    /// Gets or sets the default wait time before terminating the attempt to execute a command and generating an error.
    /// </summary>
    /// <value>
    /// The <see cref="TimeSpan"/> to wait for the command to execute. Default value is 1 minute and 30 seconds.
    /// </value>
    public static TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(90);

    /// <summary>
    /// Initializes a new instance of the <see cref="DataStatementOptions"/> class.
    /// </summary>
    public DataStatementOptions()
    {
        Type = CommandType.Text;
        Timeout = DefaultTimeout;
        Parameters = Array.Empty<IDataParameter>();
    }

    /// <summary>
    /// Gets the command type value to execute.
    /// </summary>
    /// <value>The command type value to execute. Default type value is <see cref="CommandType.Text"/>.</value>
    public CommandType Type { get; set; }

    /// <summary>
    /// Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
    /// </summary>
    /// <value>The timespan to wait for the command to execute. Default value is 1 minute and 30 seconds.</value>
    public TimeSpan Timeout { get; set; }

    /// <summary>
    /// Gets or sets the parameters to use in the command.
    /// </summary>
    /// <value>The parameters to use in the command.</value>
    public IDataParameter[] Parameters { get; set; }

    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
    public void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(Parameters == null);
    }
}
