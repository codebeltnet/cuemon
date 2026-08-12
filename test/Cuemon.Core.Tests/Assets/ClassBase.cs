using System;

namespace Cuemon.Assets;
public class ClassBase
{
    public virtual int GetSomeNumber()
    {
        return int.MinValue;
    }

    public virtual Guid Id { get; } = Guid.Empty;
}
