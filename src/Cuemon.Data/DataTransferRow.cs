using System.Globalization;
using System.Text;

namespace Cuemon.Data;
/// <summary>
/// Represents the row of a table in a database. This class cannot be inherited.
/// </summary>
public sealed class DataTransferRow
{
    internal DataTransferRow(DataTransferRowCollection main, int rowNumber)
    {
        Number = rowNumber;
        Main = main;
    }

    private DataTransferRowCollection Main { get; }

    /// <summary>
    /// Gets the row number.
    /// </summary>
    /// <value>The row number.</value>
    public int Number { get; private set; }

    private int NumberFromZero => Number - 1;

    /// <summary>
    /// Gets the associated columns of this row.
    /// </summary>
    /// <value>The associated columns of this row.</value>
    public DataTransferColumnCollection Columns => Main.Columns;

    private int GetIndexLocation(int ordinal)
    {
        return (NumberFromZero * Main.Columns.Count) + ordinal;
    }

    /// <summary>
    /// Gets the value of a <see cref="DataTransferColumn"/> from the <see cref="Columns"/> with the specified <paramref name="column"/>.
    /// </summary>
    /// <param name="column">The column from which to return the value from.</param>
    /// <returns>An <see cref="object"/> that contains the data of the column.</returns>
    public object this[DataTransferColumn column] => column == null ? null : this[column.Ordinal];

    /// <summary>
    /// Gets the value of a <see cref="DataTransferColumn"/> from the <see cref="Columns"/> with the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The name of the column from which to return the value from.</param>
    /// <returns>An <see cref="object"/> that contains the data of the column.</returns>
    public object this[string name]
    {
        get
        {
            var index = Main.Columns.GetIndex(name);
            return index.HasValue ? this[index.Value] : null;
        }
    }

    /// <summary>
    /// Gets the value of a <see cref="DataTransferColumn"/> from the <see cref="Columns"/> with the specified <paramref name="index"/>.
    /// </summary>
    /// <param name="index">The zero-based index of the column from which to return the value from.</param>
    /// <returns>An <see cref="object"/> that contains the data of the column.</returns>
    public object this[int index] => index < 0 ? null : Main.Data[GetIndexLocation(index)];

    /// <summary>
    /// Gets the value of a <see cref="DataTransferColumn"/> from the <see cref="Columns"/> with the specified <paramref name="column"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="column">The column from which to return the value from.</param>
    /// <returns>The value associated with the <paramref name="column"/> converted to the specified <typeparamref name="TResult"/>.</returns>
    public TResult As<TResult>(DataTransferColumn column)
    {
        return column == null ? default : As<TResult>(column.Ordinal);
    }

    /// <summary>
    /// Gets the value of a <see cref="DataTransferColumn"/> from the <see cref="Columns"/> with the specified <paramref name="name"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="name">The name of the column from which to return the value from.</param>
    /// <returns>The value associated with the <paramref name="name"/> of a column converted to the specified <typeparamref name="TResult"/>.</returns>
    public TResult As<TResult>(string name)
    {
        var index = Main.Columns.GetIndex(name);
        return index.HasValue ? As<TResult>(index.Value) : default;
    }

    /// <summary>
    /// Gets the value of a <see cref="DataTransferColumn"/> from the <see cref="Columns"/> with the specified <paramref name="index"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="index">The zero-based index of the column from which to return the value from.</param>
    /// <returns>The value associated with the zero-based <paramref name="index"/> of a column converted to the specified <typeparamref name="TResult"/>.</returns>
    public TResult As<TResult>(int index)
    {
        if (index < 0) { return default; }
        var target = typeof(TResult);
        var source = Main.Columns[index].DataType;
        if (source != target) { throw new TypeArgumentOutOfRangeException("TResult", string.Format(CultureInfo.InvariantCulture, "There is a mismatch between the specified column referenced by 'index' and the type parameter 'TResult'. Expected type of 'TResult' was '{0}'.", Decorator.Enclose(source).ToFriendlyName(o => o.FullName = true))); }
        return (TResult)this[index];
    }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        var builder = new StringBuilder();
        for (var i = 0; i < Columns.Count; i++)
        {
            var column = Columns[i];
            builder.AppendFormat(CultureInfo.InvariantCulture, "{0}={1} [{2}],", column.Name, Main.Data[GetIndexLocation(column.Ordinal)], column.DataType.Name);
        }
        return builder.ToString(0, builder.Length - 1);
    }
}
