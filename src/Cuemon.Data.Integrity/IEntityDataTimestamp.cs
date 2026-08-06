using System;

namespace Cuemon.Data.Integrity;
/// <summary>
/// An interface that represents the timestamp of data that is normally associated with an entity/resource.
/// </summary>
public interface IEntityDataTimestamp
{
    /// <summary>
    /// Gets a <see cref="DateTime"/> value from when data this resource represents was first created, expressed as the Coordinated Universal Time (UTC).
    /// </summary>
    /// <value>The timestamp from when data this resource represents was first created.</value>
    DateTime Created { get; }

    /// <summary>
    /// Gets a <see cref="DateTime"/> value from when data this resource represents was last modified, expressed as the Coordinated Universal Time (UTC).
    /// </summary>
    /// <value>The timestamp from when data this resource represents was last modified.</value>
    DateTime? Modified { get; }
}
