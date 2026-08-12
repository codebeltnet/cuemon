using System;

namespace Cuemon.Assets;
public class ClassWithDefaultValue : IEquatable<ClassWithDefaultValue>
{
    public ClassWithDefaultValue() : this(int.MaxValue)
    {
    }

    public ClassWithDefaultValue(int dv)
    {
        Value = dv;
    }

    public int Value { get; }

    public bool Equals(ClassWithDefaultValue other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ClassWithDefaultValue)obj);
    }

    public override int GetHashCode()
    {
        return Value;
    }
}
