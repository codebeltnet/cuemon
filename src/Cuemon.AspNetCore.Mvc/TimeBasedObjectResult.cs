using System;
using Cuemon.Data.Integrity;

namespace Cuemon.AspNetCore.Mvc;
internal class TimeBasedObjectResult : CacheableObjectResult, IEntityDataTimestamp
{
    internal TimeBasedObjectResult(object instance, DateTime created, DateTime? modified) : base(instance)
    {
        Created = created;
        Modified = modified;
    }

    public DateTime Created { get; }

    public DateTime? Modified { get; }
}

internal sealed class TimeBasedObjectResult<T> : TimeBasedObjectResult
{
    internal TimeBasedObjectResult(T instance, DateTime created, DateTime? modified) : base(instance, created, modified)
    {
    }
}
