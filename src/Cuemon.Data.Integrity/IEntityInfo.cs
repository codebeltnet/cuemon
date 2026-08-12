namespace Cuemon.Data.Integrity;
/// <summary>
/// An interface that represents both the timestamp and integrity of data that is normally associated with an entity/resource.
/// </summary>
/// <seealso cref="IEntityDataTimestamp" />
/// <seealso cref="IEntityDataIntegrity" />
public interface IEntityInfo : IEntityDataTimestamp, IEntityDataIntegrity
{
}
