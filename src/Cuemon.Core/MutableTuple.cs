using System;
using Cuemon.Collections.Generic;

namespace Cuemon;
/// <summary>
/// Represents a <see cref="MutableTuple"/> with an empty value.
/// </summary>
public class MutableTuple
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple"/> class.
    /// </summary>
    public MutableTuple()
    {
    }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public virtual object[] ToArray()
    {
        return Array.Empty<object>();
    }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance concatenated with the specified <paramref name="additionalArgs"/>.
    /// </summary>
    /// <param name="additionalArgs">The additional arguments to concatenate with the objects that represent the arguments passed to this instance.</param>
    /// <returns>An array of objects that represent the arguments passed to this instance concatenated with the specified <paramref name="additionalArgs"/>.</returns>
    public object[] ToArray(params object[] additionalArgs)
    {
        return Arguments.Concat(ToArray(), additionalArgs);
    }

    /// <summary>
    /// Gets a value indicating whether this <see cref="MutableTuple"/> is empty.
    /// </summary>
    /// <value><c>true</c> if this <see cref="MutableTuple"/> is empty; otherwise, <c>false</c>.</value>
    public virtual bool IsEmpty => true;

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return DelimitedString.Create(ToArray(), o =>
        {
            o.Delimiter = ", ";
            o.StringConverter = i => Generate.ObjectPortrayal(i);
        });
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple"/> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple"/> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public virtual MutableTuple Clone()
    {
        return new MutableTuple();
    }
}

/// <summary>
/// Represents a <see cref="MutableTuple"/> with a single generic value.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of this <see cref="MutableTuple"/>.</typeparam>
public class MutableTuple<T1> : MutableTuple
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple{T1}"/> class.
    /// </summary>
    /// <param name="arg1">The value of the parameter of this <see cref="MutableTuple"/>.</param>
    public MutableTuple(T1 arg1)
    {
        Arg1 = arg1;
    }

    /// <summary>
    /// Gets or sets the first parameter of this instance.
    /// </summary>
    /// <value>The first parameter of this instance.</value>
    public T1 Arg1 { get; set; }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public override object[] ToArray()
    {
        return new object[] { Arg1 };
    }

    /// <summary>
    /// Gets a value indicating whether this <see cref="MutableTuple" /> is empty.
    /// </summary>
    /// <value><c>true</c> if this <see cref="MutableTuple" /> is empty; otherwise, <c>false</c>.</value>
    public override bool IsEmpty => false;

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple" /> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple" /> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTuple Clone()
    {
        return new MutableTuple<T1>(Arg1);
    }
}

/// <summary>
/// Represents a <see cref="MutableTuple"/> with two generic values.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T2">The type of the second parameter of this <see cref="MutableTuple"/>.</typeparam>
public class MutableTuple<T1, T2> : MutableTuple<T1>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple{T1,T2}"/> class.
    /// </summary>
    /// <param name="arg1">The value of the first parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg2">The value of the second parameter of this <see cref="MutableTuple"/>.</param>
    public MutableTuple(T1 arg1, T2 arg2) : base(arg1)
    {
        Arg2 = arg2;
    }

    /// <summary>
    /// Gets or sets the second parameter of this instance.
    /// </summary>
    /// <value>The second parameter of this instance.</value>
    public T2 Arg2 { get; set; }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public override object[] ToArray()
    {
        return new object[] { Arg1, Arg2 };
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple" /> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple" /> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTuple Clone()
    {
        return new MutableTuple<T1, T2>(Arg1, Arg2);
    }
}

/// <summary>
/// Represents a <see cref="MutableTuple"/> with three generic values.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T2">The type of the second parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T3">The type of the third parameter of this <see cref="MutableTuple"/>.</typeparam>
public class MutableTuple<T1, T2, T3> : MutableTuple<T1, T2>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple{T1,T2,T3}"/> class.
    /// </summary>
    /// <param name="arg1">The value of the first parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg2">The value of the second parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg3">The value of the third parameter of this <see cref="MutableTuple"/>.</param>
    public MutableTuple(T1 arg1, T2 arg2, T3 arg3) : base(arg1, arg2)
    {
        Arg3 = arg3;
    }

    /// <summary>
    /// Gets or sets the third parameter of this instance.
    /// </summary>
    /// <value>The third parameter of this instance.</value>
    public T3 Arg3 { get; set; }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public override object[] ToArray()
    {
        return new object[] { Arg1, Arg2, Arg3 };
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple" /> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple" /> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTuple Clone()
    {
        return new MutableTuple<T1, T2, T3>(Arg1, Arg2, Arg3);
    }
}

/// <summary>
/// Represents a <see cref="MutableTuple"/> with four generic values.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T2">The type of the second parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T3">The type of the third parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter of this <see cref="MutableTuple"/>.</typeparam>
public class MutableTuple<T1, T2, T3, T4> : MutableTuple<T1, T2, T3>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple{T1,T2,T3,T4}"/> class.
    /// </summary>
    /// <param name="arg1">The value of the first parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg2">The value of the second parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg3">The value of the third parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg4">The value of the fourth parameter of this <see cref="MutableTuple"/>.</param>
    public MutableTuple(T1 arg1, T2 arg2, T3 arg3, T4 arg4) : base(arg1, arg2, arg3)
    {
        Arg4 = arg4;
    }

    /// <summary>
    /// Gets or sets the fourth parameter of this instance.
    /// </summary>
    /// <value>The fourth parameter of this instance.</value>
    public T4 Arg4 { get; set; }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public override object[] ToArray()
    {
        return new object[] { Arg1, Arg2, Arg3, Arg4 };
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple" /> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple" /> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTuple Clone()
    {
        return new MutableTuple<T1, T2, T3, T4>(Arg1, Arg2, Arg3, Arg4);
    }
}

/// <summary>
/// Represents a <see cref="MutableTuple"/> with five generic values.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T2">The type of the second parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T3">The type of the third parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter of this <see cref="MutableTuple"/>.</typeparam>
public class MutableTuple<T1, T2, T3, T4, T5> : MutableTuple<T1, T2, T3, T4>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple{T1,T2,T3,T4,T5}"/> class.
    /// </summary>
    /// <param name="arg1">The value of the first parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg2">The value of the second parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg3">The value of the third parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg4">The value of the fourth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg5">The value of the fifth parameter of this <see cref="MutableTuple"/>.</param>
    public MutableTuple(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) : base(arg1, arg2, arg3, arg4)
    {
        Arg5 = arg5;
    }

    /// <summary>
    /// Gets or sets the fifth parameter of this instance.
    /// </summary>
    /// <value>The fifth parameter of this instance.</value>
    public T5 Arg5 { get; set; }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public override object[] ToArray()
    {
        return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5 };
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple" /> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple" /> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTuple Clone()
    {
        return new MutableTuple<T1, T2, T3, T4, T5>(Arg1, Arg2, Arg3, Arg4, Arg5);
    }
}

/// <summary>
/// Represents a <see cref="MutableTuple"/> with six generic values.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T2">The type of the second parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T3">The type of the third parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter of this <see cref="MutableTuple"/>.</typeparam>
public class MutableTuple<T1, T2, T3, T4, T5, T6> : MutableTuple<T1, T2, T3, T4, T5>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple{T1,T2,T3,T4,T5,T6}"/> class.
    /// </summary>
    /// <param name="arg1">The value of the first parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg2">The value of the second parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg3">The value of the third parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg4">The value of the fourth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg5">The value of the fifth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg6">The value of the sixth parameter of this <see cref="MutableTuple"/>.</param>
    public MutableTuple(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) : base(arg1, arg2, arg3, arg4, arg5)
    {
        Arg6 = arg6;
    }

    /// <summary>
    /// Gets or sets the sixth parameter of this instance.
    /// </summary>
    /// <value>The sixth parameter of this instance.</value>
    public T6 Arg6 { get; set; }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public override object[] ToArray()
    {
        return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6 };
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple" /> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple" /> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTuple Clone()
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
    }
}

/// <summary>
/// Represents a <see cref="MutableTuple"/> with seven generic values.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T2">The type of the second parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T3">The type of the third parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T7">The type of the seventh parameter of this <see cref="MutableTuple"/>.</typeparam>
public class MutableTuple<T1, T2, T3, T4, T5, T6, T7> : MutableTuple<T1, T2, T3, T4, T5, T6>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple{T1,T2,T3,T4,T5,T6,T7}"/> class.
    /// </summary>
    /// <param name="arg1">The value of the first parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg2">The value of the second parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg3">The value of the third parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg4">The value of the fourth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg5">The value of the fifth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg6">The value of the sixth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg7">The value of the seventh parameter of this <see cref="MutableTuple"/>.</param>
    public MutableTuple(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) : base(arg1, arg2, arg3, arg4, arg5, arg6)
    {
        Arg7 = arg7;
    }

    /// <summary>
    /// Gets or sets the seventh parameter of this instance.
    /// </summary>
    /// <value>The seventh parameter of this instance.</value>
    public T7 Arg7 { get; set; }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public override object[] ToArray()
    {
        return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7 };
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple" /> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple" /> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTuple Clone()
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
    }
}

/// <summary>
/// Represents a <see cref="MutableTuple"/> with eight generic values.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T2">The type of the second parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T3">The type of the third parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T7">The type of the seventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T8">The type of the eighth parameter of this <see cref="MutableTuple"/>.</typeparam>
public class MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8> : MutableTuple<T1, T2, T3, T4, T5, T6, T7>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple{T1,T2,T3,T4,T5,T6,T7,T8}"/> class.
    /// </summary>
    /// <param name="arg1">The value of the first parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg2">The value of the second parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg3">The value of the third parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg4">The value of the fourth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg5">The value of the fifth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg6">The value of the sixth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg7">The value of the seventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg8">The value of the eighth parameter of this <see cref="MutableTuple"/>.</param>
    public MutableTuple(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7)
    {
        Arg8 = arg8;
    }

    /// <summary>
    /// Gets or sets the eighth parameter of this instance.
    /// </summary>
    /// <value>The eighth parameter of this instance.</value>
    public T8 Arg8 { get; set; }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public override object[] ToArray()
    {
        return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8 };
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple" /> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple" /> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTuple Clone()
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
    }
}

/// <summary>
/// Represents a <see cref="MutableTuple"/> with nine generic values.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T2">The type of the second parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T3">The type of the third parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T7">The type of the seventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T8">The type of the eighth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T9">The type of the ninth parameter of this <see cref="MutableTuple"/>.</typeparam>
public class MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> : MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple{T1,T2,T3,T4,T5,T6,T7,T8,T9}"/> class.
    /// </summary>
    /// <param name="arg1">The value of the first parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg2">The value of the second parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg3">The value of the third parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg4">The value of the fourth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg5">The value of the fifth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg6">The value of the sixth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg7">The value of the seventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg8">The value of the eighth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg9">The value of the ninth parameter of this <see cref="MutableTuple"/>.</param>
    public MutableTuple(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8)
    {
        Arg9 = arg9;
    }

    /// <summary>
    /// Gets or sets the ninth parameter of this instance.
    /// </summary>
    /// <value>The ninth parameter of this instance.</value>
    public T9 Arg9 { get; set; }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public override object[] ToArray()
    {
        return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9 };
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple" /> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple" /> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTuple Clone()
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
    }
}

/// <summary>
/// Represents a <see cref="MutableTuple"/> with ten generic values.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T2">The type of the second parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T3">The type of the third parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T7">The type of the seventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T8">The type of the eighth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T9">The type of the ninth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T10">The type of the tenth parameter of this <see cref="MutableTuple"/>.</typeparam>
public class MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10}"/> class.
    /// </summary>
    /// <param name="arg1">The value of the first parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg2">The value of the second parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg3">The value of the third parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg4">The value of the fourth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg5">The value of the fifth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg6">The value of the sixth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg7">The value of the seventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg8">The value of the eighth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg9">The value of the ninth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg10">The value of the tenth parameter of this <see cref="MutableTuple"/>.</param>
    public MutableTuple(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9)
    {
        Arg10 = arg10;
    }

    /// <summary>
    /// Gets or sets the tenth parameter of this instance.
    /// </summary>
    /// <value>The tenth parameter of this instance.</value>
    public T10 Arg10 { get; set; }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public override object[] ToArray()
    {
        return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10 };
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple" /> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple" /> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTuple Clone()
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
    }
}

/// <summary>
/// Represents a <see cref="MutableTuple"/> with eleven generic values.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T2">The type of the second parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T3">The type of the third parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T7">The type of the seventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T8">The type of the eighth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T9">The type of the ninth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T10">The type of the tenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T11">The type of the eleventh parameter of this <see cref="MutableTuple"/>.</typeparam>
public class MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11}"/> class.
    /// </summary>
    /// <param name="arg1">The value of the first parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg2">The value of the second parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg3">The value of the third parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg4">The value of the fourth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg5">The value of the fifth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg6">The value of the sixth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg7">The value of the seventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg8">The value of the eighth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg9">The value of the ninth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg10">The value of the tenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg11">The value of the eleventh parameter of this <see cref="MutableTuple"/>.</param>
    public MutableTuple(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10)
    {
        Arg11 = arg11;
    }

    /// <summary>
    /// Gets or sets the eleventh parameter of this instance.
    /// </summary>
    /// <value>The eleventh parameter of this instance.</value>
    public T11 Arg11 { get; set; }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public override object[] ToArray()
    {
        return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11 };
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple" /> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple" /> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTuple Clone()
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11);
    }
}

/// <summary>
/// Represents a <see cref="MutableTuple"/> with twelve generic values.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T2">The type of the second parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T3">The type of the third parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T7">The type of the seventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T8">The type of the eighth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T9">The type of the ninth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T10">The type of the tenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T11">The type of the eleventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T12">The type of the twelfth parameter of this <see cref="MutableTuple"/>.</typeparam>
public class MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12}"/> class.
    /// </summary>
    /// <param name="arg1">The value of the first parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg2">The value of the second parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg3">The value of the third parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg4">The value of the fourth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg5">The value of the fifth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg6">The value of the sixth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg7">The value of the seventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg8">The value of the eighth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg9">The value of the ninth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg10">The value of the tenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg11">The value of the eleventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg12">The value of the twelfth parameter of this <see cref="MutableTuple"/>.</param>
    public MutableTuple(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11)
    {
        Arg12 = arg12;
    }

    /// <summary>
    /// Gets or sets the twelfth parameter of this instance.
    /// </summary>
    /// <value>The twelfth parameter of this instance.</value>
    public T12 Arg12 { get; set; }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public override object[] ToArray()
    {
        return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12 };
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple" /> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple" /> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTuple Clone()
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12);
    }
}

/// <summary>
/// Represents a <see cref="MutableTuple"/> with thirteen generic values.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T2">The type of the second parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T3">The type of the third parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T7">The type of the seventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T8">The type of the eighth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T9">The type of the ninth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T10">The type of the tenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T11">The type of the eleventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T12">The type of the twelfth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T13">The type of the thirteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
public class MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13}"/> class.
    /// </summary>
    /// <param name="arg1">The value of the first parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg2">The value of the second parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg3">The value of the third parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg4">The value of the fourth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg5">The value of the fifth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg6">The value of the sixth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg7">The value of the seventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg8">The value of the eighth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg9">The value of the ninth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg10">The value of the tenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg11">The value of the eleventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg12">The value of the twelfth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg13">The value of the thirteenth parameter of this <see cref="MutableTuple"/>.</param>
    public MutableTuple(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12)
    {
        Arg13 = arg13;
    }

    /// <summary>
    /// Gets or sets the thirteenth parameter of this instance.
    /// </summary>
    /// <value>The thirteenth parameter of this instance.</value>
    public T13 Arg13 { get; set; }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public override object[] ToArray()
    {
        return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13 };
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple" /> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple" /> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTuple Clone()
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13);
    }
}

/// <summary>
/// Represents a <see cref="MutableTuple"/> with fourteen generic values.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T2">The type of the second parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T3">The type of the third parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T7">The type of the seventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T8">The type of the eighth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T9">The type of the ninth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T10">The type of the tenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T11">The type of the eleventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T12">The type of the twelfth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T13">The type of the thirteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T14">The type of the fourteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
public class MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14}"/> class.
    /// </summary>
    /// <param name="arg1">The value of the first parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg2">The value of the second parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg3">The value of the third parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg4">The value of the fourth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg5">The value of the fifth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg6">The value of the sixth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg7">The value of the seventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg8">The value of the eighth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg9">The value of the ninth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg10">The value of the tenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg11">The value of the eleventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg12">The value of the twelfth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg13">The value of the thirteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg14">The value of the fourteenth parameter of this <see cref="MutableTuple"/>.</param>
    public MutableTuple(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13)
    {
        Arg14 = arg14;
    }

    /// <summary>
    /// Gets or sets the fourteenth parameter of this instance.
    /// </summary>
    /// <value>The fourteenth parameter of this instance.</value>
    public T14 Arg14 { get; set; }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public override object[] ToArray()
    {
        return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14 };
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple" /> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple" /> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTuple Clone()
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14);
    }
}

/// <summary>
/// Represents a <see cref="MutableTuple"/> with fifteen generic values.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T2">The type of the second parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T3">The type of the third parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T7">The type of the seventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T8">The type of the eighth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T9">The type of the ninth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T10">The type of the tenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T11">The type of the eleventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T12">The type of the twelfth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T13">The type of the thirteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T14">The type of the fourteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T15">The type of the fifteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
public class MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15}"/> class.
    /// </summary>
    /// <param name="arg1">The value of the first parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg2">The value of the second parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg3">The value of the third parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg4">The value of the fourth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg5">The value of the fifth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg6">The value of the sixth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg7">The value of the seventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg8">The value of the eighth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg9">The value of the ninth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg10">The value of the tenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg11">The value of the eleventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg12">The value of the twelfth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg13">The value of the thirteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg14">The value of the fourteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg15">The value of the fifteenth parameter of this <see cref="MutableTuple"/>.</param>
    public MutableTuple(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14)
    {
        Arg15 = arg15;
    }

    /// <summary>
    /// Gets or sets the fifteenth parameter of this instance.
    /// </summary>
    /// <value>The fifteenth parameter of this instance.</value>
    public T15 Arg15 { get; set; }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public override object[] ToArray()
    {
        return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15 };
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple" /> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple" /> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTuple Clone()
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15);
    }
}

/// <summary>
/// Represents a <see cref="MutableTuple"/> with sixteen generic values.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T2">The type of the second parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T3">The type of the third parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T7">The type of the seventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T8">The type of the eighth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T9">The type of the ninth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T10">The type of the tenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T11">The type of the eleventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T12">The type of the twelfth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T13">The type of the thirteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T14">The type of the fourteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T15">The type of the fifteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T16">The type of the sixteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
public class MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16}"/> class.
    /// </summary>
    /// <param name="arg1">The value of the first parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg2">The value of the second parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg3">The value of the third parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg4">The value of the fourth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg5">The value of the fifth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg6">The value of the sixth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg7">The value of the seventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg8">The value of the eighth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg9">The value of the ninth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg10">The value of the tenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg11">The value of the eleventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg12">The value of the twelfth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg13">The value of the thirteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg14">The value of the fourteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg15">The value of the fifteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg16">The value of the sixteenth parameter of this <see cref="MutableTuple"/>.</param>
    public MutableTuple(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15)
    {
        Arg16 = arg16;
    }

    /// <summary>
    /// Gets or sets the sixteenth parameter of this instance.
    /// </summary>
    /// <value>The sixteenth parameter of this instance.</value>
    public T16 Arg16 { get; set; }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public override object[] ToArray()
    {
        return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16 };
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple" /> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple" /> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTuple Clone()
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16);
    }
}

/// <summary>
/// Represents a <see cref="MutableTuple"/> with seventeen generic values.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T2">The type of the second parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T3">The type of the third parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T7">The type of the seventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T8">The type of the eighth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T9">The type of the ninth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T10">The type of the tenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T11">The type of the eleventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T12">The type of the twelfth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T13">The type of the thirteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T14">The type of the fourteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T15">The type of the fifteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T16">The type of the sixteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T17">The type of the seventeenth parameter of this <see cref="MutableTuple"/>.</typeparam>
public class MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> : MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17}"/> class.
    /// </summary>
    /// <param name="arg1">The value of the first parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg2">The value of the second parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg3">The value of the third parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg4">The value of the fourth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg5">The value of the fifth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg6">The value of the sixth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg7">The value of the seventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg8">The value of the eighth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg9">The value of the ninth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg10">The value of the tenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg11">The value of the eleventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg12">The value of the twelfth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg13">The value of the thirteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg14">The value of the fourteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg15">The value of the fifteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg16">The value of the sixteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg17">The value of the seventeenth parameter of this <see cref="MutableTuple"/>.</param>
    public MutableTuple(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16)
    {
        Arg17 = arg17;
    }

    /// <summary>
    /// Gets or sets the seventeenth parameter of this instance.
    /// </summary>
    /// <value>The seventeenth parameter of this instance.</value>
    public T17 Arg17 { get; set; }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public override object[] ToArray()
    {
        return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16, Arg17 };
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple" /> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple" /> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTuple Clone()
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16, Arg17);
    }
}

/// <summary>
/// Represents a <see cref="MutableTuple"/> with eighteen generic values.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T2">The type of the second parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T3">The type of the third parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T7">The type of the seventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T8">The type of the eighth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T9">The type of the ninth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T10">The type of the tenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T11">The type of the eleventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T12">The type of the twelfth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T13">The type of the thirteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T14">The type of the fourteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T15">The type of the fifteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T16">The type of the sixteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T17">The type of the seventeenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T18">The type of the eighteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
public class MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> : MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18}"/> class.
    /// </summary>
    /// <param name="arg1">The value of the first parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg2">The value of the second parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg3">The value of the third parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg4">The value of the fourth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg5">The value of the fifth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg6">The value of the sixth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg7">The value of the seventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg8">The value of the eighth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg9">The value of the ninth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg10">The value of the tenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg11">The value of the eleventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg12">The value of the twelfth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg13">The value of the thirteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg14">The value of the fourteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg15">The value of the fifteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg16">The value of the sixteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg17">The value of the seventeenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg18">The value of the eighteenth parameter of this <see cref="MutableTuple"/>.</param>
    public MutableTuple(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17)
    {
        Arg18 = arg18;
    }

    /// <summary>
    /// Gets or sets the eighteenth parameter of this instance.
    /// </summary>
    /// <value>The eighteenth parameter of this instance.</value>
    public T18 Arg18 { get; set; }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public override object[] ToArray()
    {
        return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16, Arg17, Arg18 };
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple" /> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple" /> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTuple Clone()
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16, Arg17, Arg18);
    }
}

/// <summary>
/// Represents a <see cref="MutableTuple"/> with nineteen generic values.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T2">The type of the second parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T3">The type of the third parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T7">The type of the seventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T8">The type of the eighth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T9">The type of the ninth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T10">The type of the tenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T11">The type of the eleventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T12">The type of the twelfth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T13">The type of the thirteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T14">The type of the fourteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T15">The type of the fifteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T16">The type of the sixteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T17">The type of the seventeenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T18">The type of the eighteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T19">The type of the nineteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
public class MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> : MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18,T19}"/> class.
    /// </summary>
    /// <param name="arg1">The value of the first parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg2">The value of the second parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg3">The value of the third parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg4">The value of the fourth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg5">The value of the fifth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg6">The value of the sixth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg7">The value of the seventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg8">The value of the eighth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg9">The value of the ninth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg10">The value of the tenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg11">The value of the eleventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg12">The value of the twelfth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg13">The value of the thirteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg14">The value of the fourteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg15">The value of the fifteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg16">The value of the sixteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg17">The value of the seventeenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg18">The value of the eighteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg19">The value of the nineteenth parameter of this <see cref="MutableTuple"/>.</param>
    public MutableTuple(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18)
    {
        Arg19 = arg19;
    }

    /// <summary>
    /// Gets or sets the nineteenth parameter of this instance.
    /// </summary>
    /// <value>The nineteenth parameter of this instance.</value>
    public T19 Arg19 { get; set; }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public override object[] ToArray()
    {
        return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16, Arg17, Arg18, Arg19 };
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple" /> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple" /> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTuple Clone()
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16, Arg17, Arg18, Arg19);
    }
}

/// <summary>
/// Represents a <see cref="MutableTuple"/> with twenty generic values.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T2">The type of the second parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T3">The type of the third parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T7">The type of the seventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T8">The type of the eighth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T9">The type of the ninth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T10">The type of the tenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T11">The type of the eleventh parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T12">The type of the twelfth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T13">The type of the thirteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T14">The type of the fourteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T15">The type of the fifteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T16">The type of the sixteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T17">The type of the seventeenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T18">The type of the eighteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T19">The type of the nineteenth parameter of this <see cref="MutableTuple"/>.</typeparam>
/// <typeparam name="T20">The type of the twentieth parameter of this <see cref="MutableTuple"/>.</typeparam>
public class MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> : MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableTuple{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18,T19,T20}"/> class.
    /// </summary>
    /// <param name="arg1">The value of the first parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg2">The value of the second parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg3">The value of the third parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg4">The value of the fourth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg5">The value of the fifth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg6">The value of the sixth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg7">The value of the seventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg8">The value of the eighth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg9">The value of the ninth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg10">The value of the tenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg11">The value of the eleventh parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg12">The value of the twelfth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg13">The value of the thirteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg14">The value of the fourteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg15">The value of the fifteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg16">The value of the sixteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg17">The value of the seventeenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg18">The value of the eighteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg19">The value of the nineteenth parameter of this <see cref="MutableTuple"/>.</param>
    /// <param name="arg20">The value of the twentieth parameter of this <see cref="MutableTuple"/>.</param>
    public MutableTuple(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, T20 arg20) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19)
    {
        Arg20 = arg20;
    }

    /// <summary>
    /// Gets or sets the twentieth parameter of this instance.
    /// </summary>
    /// <value>The twentieth parameter of this instance.</value>
    public T20 Arg20 { get; set; }

    /// <summary>
    /// Returns an array of objects that represent the arguments passed to this instance.
    /// </summary>
    /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
    public override object[] ToArray()
    {
        return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16, Arg17, Arg18, Arg19, Arg20 };
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="MutableTuple" /> object.
    /// </summary>
    /// <returns>A new <see cref="MutableTuple" /> that is a copy of this instance.</returns>
    /// <remarks>When thread safety is required this is the method to invoke.</remarks>
    public override MutableTuple Clone()
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16, Arg17, Arg18, Arg19, Arg20);
    }
}
