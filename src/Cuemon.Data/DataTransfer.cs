using System;
using System.Data;

namespace Cuemon.Data;
/// <summary>
/// Provides a way to convert an <see cref="IDataReader"/> implementation to a table-like data transfer object.
/// </summary>
public static class DataTransfer
{
    /// <summary>
    /// Converts the specified <paramref name="reader"/> implementation to a table-like data transfer object collection.
    /// </summary>
    /// <param name="reader">The reader to be converted.</param>
    /// <returns>A <see cref="DataTransferRowCollection"/> that is the result of the specified <paramref name="reader"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="reader"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="reader"/> is closed.
    /// </exception>
    public static DataTransferRowCollection GetRows(IDataReader reader)
    {
        Validator.ThrowIfNull(reader);
        Validator.ThrowIfTrue(reader.IsClosed, nameof(reader), "Reader was closed.");
        return new DataTransferRowCollection(reader);
    }

    /// <summary>
    /// Converts the specified and read-initialized <paramref name="reader"/> implementation to a column-like data transfer object collection.
    /// </summary>
    /// <param name="reader">The read-initialized reader to be converted.</param>
    /// <returns>A <see cref="DataTransferColumnCollection"/> that is the result of the specified and read-initialized <paramref name="reader"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="reader"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="reader"/> is closed.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Invalid attempt to read from <paramref name="reader"/> when no data is present.
    /// </exception>
    public static DataTransferColumnCollection GetColumns(IDataReader reader)
    {
        Validator.ThrowIfNull(reader);
        Validator.ThrowIfTrue(reader.IsClosed, nameof(reader), "Reader was closed.");
        return new DataTransferColumnCollection(reader);
    }
}
