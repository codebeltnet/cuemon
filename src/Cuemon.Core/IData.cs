using System.Collections.Generic;

namespace Cuemon;
/// <summary>
/// Provides a way to supply information about the class implementing this interface.
/// </summary>
public interface IData
{
    /// <summary>
    /// Gets a collection of key/value pairs that provide information about this class.
    /// </summary>
    /// <value>An object that implements the <see cref="IDictionary{TKey,TValue}"/> interface and contains a collection of key/value pairs.</value>
    IDictionary<string, object> Data { get; }
}
