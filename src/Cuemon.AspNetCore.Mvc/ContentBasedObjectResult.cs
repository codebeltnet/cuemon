using Cuemon.Data.Integrity;
using Cuemon.Security;

namespace Cuemon.AspNetCore.Mvc;
internal class ContentBasedObjectResult : CacheableObjectResult, IEntityDataIntegrity
{
    internal ContentBasedObjectResult(object instance, byte[] checksum, bool isWeak = false) : base(instance)
    {
        Checksum = new HashResult(checksum);
        Validation = checksum == null || checksum.Length == 0
            ? EntityDataIntegrityValidation.Unspecified
            : isWeak
                ? EntityDataIntegrityValidation.Weak
                : EntityDataIntegrityValidation.Strong;
    }

    public EntityDataIntegrityValidation Validation { get; }

    public HashResult Checksum { get; }
}

internal sealed class ContentBasedObjectResult<T> : ContentBasedObjectResult
{
    internal ContentBasedObjectResult(T instance, byte[] checksum, bool isWeak = false) : base(instance, checksum, isWeak)
    {
    }
}
