using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Text;

namespace Cuemon.Data;
/// <summary>
/// Provides a generic way of reading a forward-only stream of rows from a <typeparamref name="TRead"/> based data source. This is an abstract class.
/// </summary>
/// <typeparam name="TRead">The type of the value that this <see cref="IDataReader"/> will read.</typeparam>
/// <seealso cref="Disposable" />
/// <seealso cref="IDataReader" />
public abstract class DataReader<TRead> : Disposable, IDataReader
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataReader{TRead}"/> class.
    /// </summary>
    protected DataReader()
    {
        Fields = new OrderedDictionary(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Determines whether this instance contains a column with the specified name.
    /// </summary>
    /// <param name="name">The name of the column to find.</param>
    /// <returns><c>true</c> if this instance contains a column with the specified name; otherwise, <c>false</c>.</returns>
    public bool Contains(string name)
    {
        return Fields.Contains(name);
    }

    /// <summary>
    /// Gets the column with the specified name.
    /// </summary>
    /// <param name="name">The name of the column to find.</param>
    /// <returns>The column with the specified name as an <see cref="object"/>.</returns>
    public object this[string name] => Fields[name];

    /// <summary>
    /// Gets the column located at the specified index.
    /// </summary>
    /// <param name="i">The zero-based index of the column to get.</param>
    /// <returns>The column located at the specified index as an <see cref="object"/>.</returns>
    public object this[int i] => Fields[i];

    private IOrderedDictionary Fields { get; set; }

    /// <summary>
    /// Gets the currently processed row count of this instance.
    /// </summary>
    /// <value>The currently processed row count of this instance.</value>
    public abstract int RowCount { get; protected set; }

    /// <summary>
    /// Gets the number of columns in the current row.
    /// </summary>
    /// <value>When not positioned in a valid recordset, 0; otherwise, the number of columns in the current record.</value>
    public int FieldCount => Fields.Count;

    /// <summary>
    /// Returns a <see cref="string" /> that represents the current row of this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents the current row of this instance.</returns>
    public override string ToString()
    {
        var builder = new StringBuilder();
        for (var i = 0; i < FieldCount; i++)
        {
            builder.AppendFormat(CultureInfo.InvariantCulture, "{0}={1}, ", GetName(i), GetValue(i));
        }
        if (builder.Length > 0) { builder.Remove(builder.Length - 2, 2); }
        return builder.ToString();
    }

    /// <summary>
    /// Gets the value that indicates that no more rows exists.
    /// </summary>
    /// <value>The value that indicates that no more rows exists.</value>
    protected abstract TRead NullRead { get; }

    /// <summary>
    /// Advances the <see cref="IDataReader" /> to the next record.
    /// </summary>
    /// <returns><typeparamref name="TRead"/> for as long as there are rows; <see cref="NullRead"/> when no more rows exists.</returns>
    protected abstract TRead ReadNext(TRead columns);

    /// <summary>
    /// Sets the fields of the current record invoked by <see cref="ReadNext"/>.
    /// </summary>
    /// <param name="fields">The fields of the current record invoked by <see cref="ReadNext"/>.</param>
    protected void SetFields(IOrderedDictionary fields)
    {
        Fields = fields;
    }

    /// <summary>
    /// Gets the value of the specified column as a Boolean.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>The value of the column.</returns>
    public bool GetBoolean(int i)
    {
        return Convert.ToBoolean(GetValue(i), CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets the 8-bit unsigned integer value of the specified column.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>The 8-bit unsigned integer value of the specified column.</returns>
    public byte GetByte(int i)
    {
        return Convert.ToByte(GetValue(i), CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Reads a stream of bytes from the specified column, starting at location indicated by dataOffset, into the buffer, starting at the location indicated by bufferOffset.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <param name="fieldOffset">The index within the row from which to begin the read operation.</param>
    /// <param name="buffer">The buffer into which to copy the data.</param>
    /// <param name="bufferoffset">The index with the buffer to which the data will be copied.</param>
    /// <param name="length">The maximum number of characters to read.</param>
    /// <returns>The actual number of bytes read.</returns>
    public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
    {
        return 0;
    }

    /// <summary>
    /// Gets the character value of the specified column.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>The character value of the specified column.</returns>
    public char GetChar(int i)
    {
        return Convert.ToChar(GetValue(i), CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets the date and time data value of the specified field.
    /// </summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The date and time data value of the specified field.</returns>
    public DateTime GetDateTime(int i)
    {
        return (DateTime)GetValue(i);
    }

    /// <summary>
    /// Gets the fixed-position numeric value of the specified field.
    /// </summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The fixed-position numeric value of the specified field.</returns>
    public decimal GetDecimal(int i)
    {
        return Convert.ToDecimal(GetValue(i), CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets the double-precision floating point number of the specified field.
    /// </summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The double-precision floating point number of the specified field.</returns>
    public double GetDouble(int i)
    {
        return Convert.ToDouble(GetValue(i), CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets the <see cref="Type" /> information corresponding to the type of <see cref="System.Object" /> that would be returned from <see cref="IDataRecord.GetValue(System.Int32)" />.
    /// </summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The <see cref="Type" /> information corresponding to the type of <see cref="System.Object" /> that would be returned from <see cref="IDataRecord.GetValue(System.Int32)" />.</returns>
    public Type GetFieldType(int i)
    {
        return GetValue(i).GetType();
    }

    /// <summary>
    /// Gets the single-precision floating point number of the specified field.
    /// </summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The single-precision floating point number of the specified field.</returns>
    public float GetFloat(int i)
    {
        return Convert.ToSingle(GetValue(i), CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Returns the GUID value of the specified field.
    /// </summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The GUID value of the specified field.</returns>
    public Guid GetGuid(int i)
    {
        return (Guid)GetValue(i);
    }

    /// <summary>
    /// Gets the 16-bit signed integer value of the specified field.
    /// </summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The 16-bit signed integer value of the specified field.</returns>
    public short GetInt16(int i)
    {
        return Convert.ToInt16(GetValue(i), CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets the 32-bit signed integer value of the specified field.
    /// </summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The 32-bit signed integer value of the specified field.</returns>
    public int GetInt32(int i)
    {
        return Convert.ToInt32(GetValue(i), CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets the 64-bit signed integer value of the specified field.
    /// </summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The 64-bit signed integer value of the specified field.</returns>
    public long GetInt64(int i)
    {
        return Convert.ToInt64(GetValue(i), CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets the name for the field to find.
    /// </summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The name of the field or the empty string (""), if there is no value to return.</returns>
    public string GetName(int i)
    {
        var current = 0;
        foreach (string name in Fields.Keys)
        {
            if (i == current) { return name; }
            current++;
        }
        return string.Empty;
    }

    /// <summary>
    /// Return the index of the named field.
    /// </summary>
    /// <param name="name">The name of the field to find.</param>
    /// <returns>The index of the named field.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="name"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="name"/> is not a valid column name.
    /// </exception>
    public int GetOrdinal(string name)
    {
        Validator.ThrowIfNull(name);
        var current = 0;
        foreach (string columnName in Fields.Keys)
        {
            if (columnName.Equals(name, StringComparison.OrdinalIgnoreCase)) { return current; }
            current++;
        }
        throw new ArgumentOutOfRangeException(nameof(name), "The name specified is not a valid column name.");
    }

    /// <summary>
    /// Gets the string value of the specified field.
    /// </summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The string value of the specified field.</returns>
    public string GetString(int i)
    {
        return GetValue(i)?.ToString();
    }

    /// <summary>
    /// Return the value of the specified field.
    /// </summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The <see cref="System.Object" /> which will contain the field value upon return.</returns>
    public object GetValue(int i)
    {
        return Fields[i];
    }

    /// <summary>
    /// Return whether the specified field is set to null.
    /// </summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>true if the specified field is set to null; otherwise, false.</returns>
    public bool IsDBNull(int i)
    {
        return GetValue(i) == null || GetValue(i) == DBNull.Value;
    }

    /// <summary>
    /// Advances the <see cref="IDataReader" /> to the next record.
    /// </summary>
    /// <returns><see langword="true" /> if there are more rows; otherwise, <see langword="false" />.</returns>
    public abstract bool Read();

    /// <summary>
    /// Gets a value indicating the depth of nesting for the current row.
    /// </summary>
    /// <value>The level of nesting.</value>
    public virtual int Depth => 0;

    /// <summary>
    /// Populates an array of objects with the column values of the current record.
    /// </summary>
    /// <param name="values">An array of <see cref="System.Object" /> to copy the attribute fields into.</param>
    /// <returns>The number of instances of <see cref="System.Object" /> in the array.</returns>
    public int GetValues(object[] values)
    {
        Validator.ThrowIfNull(values);
        var length = FieldCount;
        for (var i = 0; i < length; i++)
        {
            values[i] = GetValue(i);
        }
        return length;
    }

    int IDataReader.RecordsAffected => -1;

    bool IDataReader.IsClosed => Disposed;

    void IDataReader.Close()
    {
        Dispose();
    }

    DataTable IDataReader.GetSchemaTable()
    {
        return null;
    }

    bool IDataReader.NextResult()
    {
        return false;
    }

    long IDataRecord.GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
    {
        return 0;
    }

    IDataReader IDataRecord.GetData(int i)
    {
        throw new NotSupportedException();
    }

    string IDataRecord.GetDataTypeName(int i)
    {
        return typeof(string).ToString();
    }
}
