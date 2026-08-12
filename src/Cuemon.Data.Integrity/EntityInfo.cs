using System;
using Cuemon.Security;

namespace Cuemon.Data.Integrity;
/// <summary>
/// Represents the metadata information normally associated with an entity/resource.
/// Implements the <see cref="IEntityInfo" />
/// </summary>
/// <seealso cref="IEntityInfo" />
public class EntityInfo : IEntityInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityInfo"/> class.
    /// </summary>
    /// <param name="created">A <see cref="DateTime" /> value for when data this instance represents was first created.</param>
    public EntityInfo(DateTime created) : this(created, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityInfo"/> class.
    /// </summary>
    /// <param name="created">A <see cref="DateTime" /> value for when data this instance represents was first created.</param>
    /// <param name="modified">A <see cref="DateTime" /> value for when data this instance represents was last modified.</param>
    public EntityInfo(DateTime created, DateTime? modified) : this(created, modified, null, EntityDataIntegrityValidation.Unspecified)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityInfo"/> class.
    /// </summary>
    /// <param name="created">A <see cref="DateTime" /> value for when data this instance represents was first created.</param>
    /// <param name="modified">A <see cref="DateTime" /> value for when data this instance represents was last modified.</param>
    /// <param name="checksum">A <see cref="byte"/> array containing a checksum of the data this instance represents.</param>
    /// <param name="validation">A <see cref="EntityDataIntegrityValidation"/> enumeration value that indicates the validation strength of the specified <paramref name="checksum"/>. Default is <see cref="EntityDataIntegrityValidation.Weak"/>.</param>
    public EntityInfo(DateTime created, DateTime? modified, byte[] checksum, EntityDataIntegrityValidation validation = EntityDataIntegrityValidation.Weak)
    {
        Created = created.ToUniversalTime();
        Modified = modified?.ToUniversalTime();
        Checksum = new HashResult(checksum);
        Validation = validation;
    }

    /// <summary>
    /// Gets a <see cref="DateTime"/> value from when data this resource represents was first created, expressed as the Coordinated Universal Time (UTC).
    /// </summary>
    /// <value>The timestamp from when data this resource represents was first created.</value>
    public DateTime Created { get; }

    /// <summary>
    /// Gets a <see cref="DateTime"/> value from when data this resource represents was last modified, expressed as the Coordinated Universal Time (UTC).
    /// </summary>
    /// <value>The timestamp from when data this resource represents was last modified.</value>
    public DateTime? Modified { get; }

    /// <summary>
    /// Gets a <see cref="HashResult"/> that represents the integrity of this instance.
    /// </summary>
    /// <value>The checksum that represents the integrity of this instance.</value>
    public HashResult Checksum { get; }

    /// <summary>
    /// Gets the validation strength of the integrity of this resource.
    /// </summary>
    /// <value>The validation strength of the integrity of this resource.</value>
    public EntityDataIntegrityValidation Validation { get; }
}
