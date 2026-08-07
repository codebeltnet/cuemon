using System;
namespace Cuemon.Data;
/// <summary>
/// Represents the column meta information of a table-row in a database. This class cannot be inherited.
/// </summary>
public sealed class DataTransferColumn
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataTransferColumn"/> class.
    /// </summary>
    /// <param name="ordinal">The ordinal position of the column.</param>
    /// <param name="name">The name of the column.</param>
    /// <param name="dataType">The type of data stored in the column.</param>
    internal DataTransferColumn(int ordinal, string name, Type dataType)
    {
        Ordinal = ordinal;
        Name = name;
        DataType = dataType;
    }

    /// <summary>
    /// Gets the (zero-based) position of the column.
    /// </summary>
    /// <value>The position of the column.</value>
    public int Ordinal { get; private set; }

    /// <summary>
    /// Gets the name of the column.
    /// </summary>
    /// <value>The name of the column.</value>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the type of data stored in the column.
    /// </summary>
    /// <value>A <see cref="Type"/> object that represents the column data type.</value>
    public Type DataType { get; private set; }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return Name;
    }
}
