using System.Data;
using Cuemon.Configuration;

namespace Cuemon.Data;
/// <summary>
/// Configuration options for <see cref="DataManager"/>.
/// </summary>
public class DataManagerOptions : IValidatableParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataManagerOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="DataManagerOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="LeaveConnectionOpen"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="LeaveCommandOpen"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="PreferredReaderBehavior"/></term>
    ///         <description><see cref="CommandBehavior.CloseConnection"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public DataManagerOptions()
    {
        LeaveConnectionOpen = false;
        LeaveCommandOpen = false;
        PreferredReaderBehavior = CommandBehavior.CloseConnection;
    }

    /// <summary>
    /// Gets or sets the string used to open a database connection.
    /// </summary>
    /// <value>The string that includes the source database name, and other parameters needed to establish a database connection.</value>
    public string ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets a bitwise combination of the enumeration values that specify the preferred <see cref="CommandBehavior"/> for <see cref="IDataReader"/> operations.
    /// </summary>
    /// <value>The enumeration values that specify which command behavior to apply for <see cref="IDataReader"/> operations.</value>
    public CommandBehavior PreferredReaderBehavior { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether an <see cref="IDbConnection"/> should bypass the mechanism for releasing unmanaged resources. Default is <c>false</c>.
    /// </summary>
    /// <value><c>true</c> if an <see cref="IDbConnection"/> should bypass the mechanism for releasing unmanaged resources; otherwise, <c>false</c>.</value>
    public bool LeaveConnectionOpen { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether an <see cref="IDbCommand"/> should bypass the mechanism for releasing unmanaged resources. Default is <c>false</c>.
    /// </summary>
    /// <value><c>true</c> if an <see cref="IDbCommand"/> should bypass the mechanism for releasing unmanaged resources; otherwise, <c>false</c>.</value>
    public bool LeaveCommandOpen { get; set; }

    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
    public void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(string.IsNullOrWhiteSpace(ConnectionString));
    }
}
