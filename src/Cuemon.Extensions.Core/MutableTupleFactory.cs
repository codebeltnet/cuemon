namespace Cuemon.Extensions;
/// <summary>
/// Provides access to factory methods for creating <see cref="MutableTuple"/> objects.
/// </summary>
public static class MutableTupleFactory
{
    /// <summary>
    /// Creates a new 0-tuple, or empty tuple, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <returns>A 0-tuple (empty) with no value.</returns>
    public static MutableTuple CreateZero()
    {
        return new MutableTuple();
    }

    /// <summary>
    /// Creates a new 1-tuple, or single, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <typeparam name="T">The type of the only parameter of the tuple.</typeparam>
    /// <param name="arg">The value of the only parameter of the tuple.</param>
    /// <returns>A 1-tuple (single) whose value is (arg1).</returns>
    public static MutableTuple<T> CreateOne<T>(T arg)
    {
        return new MutableTuple<T>(arg);
    }

    /// <summary>
    /// Creates a new 2-tuple, or double, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
    /// <param name="arg1">The value of the first parameter of the tuple.</param>
    /// <param name="arg2">The value of the second parameter of the tuple.</param>
    /// <returns>A 2-tuple (double) whose value is (arg1, arg2).</returns>
    public static MutableTuple<T1, T2> CreateTwo<T1, T2>(T1 arg1, T2 arg2)
    {
        return new MutableTuple<T1, T2>(arg1, arg2);
    }

    /// <summary>
    /// Creates a new 3-tuple, or triple, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
    /// <param name="arg1">The value of the first parameter of the tuple.</param>
    /// <param name="arg2">The value of the second parameter of the tuple.</param>
    /// <param name="arg3">The value of the third parameter of the tuple.</param>
    /// <returns>A 3-tuple (triple) whose value is (arg1, arg2, arg3).</returns>
    public static MutableTuple<T1, T2, T3> CreateThree<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
    {
        return new MutableTuple<T1, T2, T3>(arg1, arg2, arg3);
    }

    /// <summary>
    /// Creates a new 4-tuple, or quadruple, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
    /// <param name="arg1">The value of the first parameter of the tuple.</param>
    /// <param name="arg2">The value of the second parameter of the tuple.</param>
    /// <param name="arg3">The value of the third parameter of the tuple.</param>
    /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
    /// <returns>A 4-tuple (quadruple) whose value is (arg1, arg2, arg3, arg4).</returns>
    public static MutableTuple<T1, T2, T3, T4> CreateFour<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return new MutableTuple<T1, T2, T3, T4>(arg1, arg2, arg3, arg4);
    }

    /// <summary>
    /// Creates a new 5-tuple, or quintuple, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
    /// <param name="arg1">The value of the first parameter of the tuple.</param>
    /// <param name="arg2">The value of the second parameter of the tuple.</param>
    /// <param name="arg3">The value of the third parameter of the tuple.</param>
    /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
    /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
    /// <returns>A 5-tuple (quintuple) whose value is (arg1, arg2, arg3, arg4, arg5).</returns>
    public static MutableTuple<T1, T2, T3, T4, T5> CreateFive<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return new MutableTuple<T1, T2, T3, T4, T5>(arg1, arg2, arg3, arg4, arg5);
    }

    /// <summary>
    /// Creates a new 6-tuple, or septuple, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
    /// <param name="arg1">The value of the first parameter of the tuple.</param>
    /// <param name="arg2">The value of the second parameter of the tuple.</param>
    /// <param name="arg3">The value of the third parameter of the tuple.</param>
    /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
    /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
    /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
    /// <returns>A 6-tuple (sextuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6).</returns>
    public static MutableTuple<T1, T2, T3, T4, T5, T6> CreateSix<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6>(arg1, arg2, arg3, arg4, arg5, arg6);
    }

    /// <summary>
    /// Creates a new 7-tuple, or septuple, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
    /// <param name="arg1">The value of the first parameter of the tuple.</param>
    /// <param name="arg2">The value of the second parameter of the tuple.</param>
    /// <param name="arg3">The value of the third parameter of the tuple.</param>
    /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
    /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
    /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
    /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
    /// <returns>An 7-tuple (septuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7).</returns>
    public static MutableTuple<T1, T2, T3, T4, T5, T6, T7> CreateSeven<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7>(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }

    /// <summary>
    /// Creates a new 8-tuple, or octuple, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
    /// <param name="arg1">The value of the first parameter of the tuple.</param>
    /// <param name="arg2">The value of the second parameter of the tuple.</param>
    /// <param name="arg3">The value of the third parameter of the tuple.</param>
    /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
    /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
    /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
    /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
    /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
    /// <returns>An 8-tuple (octuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8).</returns>
    public static MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8> CreateEight<T1, T2, T3, T4, T5, T6, T7, T8>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    /// <summary>
    /// Creates a new 9-tuple, or nonuple, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
    /// <param name="arg1">The value of the first parameter of the tuple.</param>
    /// <param name="arg2">The value of the second parameter of the tuple.</param>
    /// <param name="arg3">The value of the third parameter of the tuple.</param>
    /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
    /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
    /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
    /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
    /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
    /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
    /// <returns>A 9-tuple (nonuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9).</returns>
    public static MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> CreateNine<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    /// <summary>
    /// Creates a new 10-tuple, or decuple, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
    /// <param name="arg1">The value of the first parameter of the tuple.</param>
    /// <param name="arg2">The value of the second parameter of the tuple.</param>
    /// <param name="arg3">The value of the third parameter of the tuple.</param>
    /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
    /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
    /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
    /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
    /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
    /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
    /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
    /// <returns>A 10-tuple (decuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10).</returns>
    public static MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CreateTen<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    /// <summary>
    /// Creates a new 11-tuple, or undecuple, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the tuple.</typeparam>
    /// <param name="arg1">The value of the first parameter of the tuple.</param>
    /// <param name="arg2">The value of the second parameter of the tuple.</param>
    /// <param name="arg3">The value of the third parameter of the tuple.</param>
    /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
    /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
    /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
    /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
    /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
    /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
    /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
    /// <param name="arg11">The value of the eleventh parameter of the tuple.</param>
    /// <returns>A 11-tuple (undecuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11).</returns>
    public static MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> CreateEleven<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    /// <summary>
    /// Creates a new 12-tuple, or duodecuple, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the tuple.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the tuple.</typeparam>
    /// <param name="arg1">The value of the first parameter of the tuple.</param>
    /// <param name="arg2">The value of the second parameter of the tuple.</param>
    /// <param name="arg3">The value of the third parameter of the tuple.</param>
    /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
    /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
    /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
    /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
    /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
    /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
    /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
    /// <param name="arg11">The value of the eleventh parameter of the tuple.</param>
    /// <param name="arg12">The value of the twelfth parameter of the tuple.</param>
    /// <returns>A 12-tuple (duodecuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12).</returns>
    public static MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CreateTwelve<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    /// <summary>
    /// Creates a new 13-tuple, or tredecuple, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the tuple.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the tuple.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the tuple.</typeparam>
    /// <param name="arg1">The value of the first parameter of the tuple.</param>
    /// <param name="arg2">The value of the second parameter of the tuple.</param>
    /// <param name="arg3">The value of the third parameter of the tuple.</param>
    /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
    /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
    /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
    /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
    /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
    /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
    /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
    /// <param name="arg11">The value of the eleventh parameter of the tuple.</param>
    /// <param name="arg12">The value of the twelfth parameter of the tuple.</param>
    /// <param name="arg13">The value of the thirteenth parameter of the tuple.</param>
    /// <returns>A 13-tuple (tredecuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13).</returns>
    public static MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CreateThirteen<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    /// <summary>
    /// Creates a new 14-tuple, or quattuordecuple, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the tuple.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the tuple.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the tuple.</typeparam>
    /// <param name="arg1">The value of the first parameter of the tuple.</param>
    /// <param name="arg2">The value of the second parameter of the tuple.</param>
    /// <param name="arg3">The value of the third parameter of the tuple.</param>
    /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
    /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
    /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
    /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
    /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
    /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
    /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
    /// <param name="arg11">The value of the eleventh parameter of the tuple.</param>
    /// <param name="arg12">The value of the twelfth parameter of the tuple.</param>
    /// <param name="arg13">The value of the thirteenth parameter of the tuple.</param>
    /// <param name="arg14">The value of the fourteenth parameter of the tuple.</param>
    /// <returns>A 14-tuple (quattuordecuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14).</returns>
    public static MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CreateFourteen<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    /// <summary>
    /// Creates a new 15-tuple, or quindecuple, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the tuple.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the tuple.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of the tuple.</typeparam>
    /// <param name="arg1">The value of the first parameter of the tuple.</param>
    /// <param name="arg2">The value of the second parameter of the tuple.</param>
    /// <param name="arg3">The value of the third parameter of the tuple.</param>
    /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
    /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
    /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
    /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
    /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
    /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
    /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
    /// <param name="arg11">The value of the eleventh parameter of the tuple.</param>
    /// <param name="arg12">The value of the twelfth parameter of the tuple.</param>
    /// <param name="arg13">The value of the thirteenth parameter of the tuple.</param>
    /// <param name="arg14">The value of the fourteenth parameter of the tuple.</param>
    /// <param name="arg15">The value of the fifteenth parameter of the tuple.</param>
    /// <returns>A 15-tuple (quindecuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15).</returns>
    public static MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CreateFifteen<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    /// <summary>
    /// Creates a new 16-tuple, or sexdecuple, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the tuple.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the tuple.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth parameter of the tuple.</typeparam>
    /// <param name="arg1">The value of the first parameter of the tuple.</param>
    /// <param name="arg2">The value of the second parameter of the tuple.</param>
    /// <param name="arg3">The value of the third parameter of the tuple.</param>
    /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
    /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
    /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
    /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
    /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
    /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
    /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
    /// <param name="arg11">The value of the eleventh parameter of the tuple.</param>
    /// <param name="arg12">The value of the twelfth parameter of the tuple.</param>
    /// <param name="arg13">The value of the thirteenth parameter of the tuple.</param>
    /// <param name="arg14">The value of the fourteenth parameter of the tuple.</param>
    /// <param name="arg15">The value of the fifteenth parameter of the tuple.</param>
    /// <param name="arg16">The value of the sixteenth parameter of the tuple.</param>
    /// <returns>A 16-tuple (sexdecuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16).</returns>
    public static MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CreateSixteen<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    /// <summary>
    /// Creates a new 17-tuple, or septendecuple, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the tuple.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the tuple.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T17">The type of the seventeenth parameter of the tuple.</typeparam>
    /// <param name="arg1">The value of the first parameter of the tuple.</param>
    /// <param name="arg2">The value of the second parameter of the tuple.</param>
    /// <param name="arg3">The value of the third parameter of the tuple.</param>
    /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
    /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
    /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
    /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
    /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
    /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
    /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
    /// <param name="arg11">The value of the eleventh parameter of the tuple.</param>
    /// <param name="arg12">The value of the twelfth parameter of the tuple.</param>
    /// <param name="arg13">The value of the thirteenth parameter of the tuple.</param>
    /// <param name="arg14">The value of the fourteenth parameter of the tuple.</param>
    /// <param name="arg15">The value of the fifteenth parameter of the tuple.</param>
    /// <param name="arg16">The value of the sixteenth parameter of the tuple.</param>
    /// <param name="arg17">The value of the seventeenth parameter of the tuple.</param>
    /// <returns>A 17-tuple (septendecuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17).</returns>
    public static MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> CreateSeventeen<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17)
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17);
    }

    /// <summary>
    /// Creates a new 18-tuple, or octodecuple, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the tuple.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the tuple.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T17">The type of the seventeenth parameter of the tuple.</typeparam>
    /// <typeparam name="T18">The type of the eighteenth parameter of the tuple.</typeparam>
    /// <param name="arg1">The value of the first parameter of the tuple.</param>
    /// <param name="arg2">The value of the second parameter of the tuple.</param>
    /// <param name="arg3">The value of the third parameter of the tuple.</param>
    /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
    /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
    /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
    /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
    /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
    /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
    /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
    /// <param name="arg11">The value of the eleventh parameter of the tuple.</param>
    /// <param name="arg12">The value of the twelfth parameter of the tuple.</param>
    /// <param name="arg13">The value of the thirteenth parameter of the tuple.</param>
    /// <param name="arg14">The value of the fourteenth parameter of the tuple.</param>
    /// <param name="arg15">The value of the fifteenth parameter of the tuple.</param>
    /// <param name="arg16">The value of the sixteenth parameter of the tuple.</param>
    /// <param name="arg17">The value of the seventeenth parameter of the tuple.</param>
    /// <param name="arg18">The value of the eighteenth parameter of the tuple.</param>
    /// <returns>An 18-tuple (octodecuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18).</returns>
    public static MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> CreateEighteen<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18)
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18);
    }

    /// <summary>
    /// Creates a new 19-tuple, or novemdecuple, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the tuple.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the tuple.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T17">The type of the seventeenth parameter of the tuple.</typeparam>
    /// <typeparam name="T18">The type of the eighteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T19">The type of the nineteenth parameter of the tuple.</typeparam>
    /// <param name="arg1">The value of the first parameter of the tuple.</param>
    /// <param name="arg2">The value of the second parameter of the tuple.</param>
    /// <param name="arg3">The value of the third parameter of the tuple.</param>
    /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
    /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
    /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
    /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
    /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
    /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
    /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
    /// <param name="arg11">The value of the eleventh parameter of the tuple.</param>
    /// <param name="arg12">The value of the twelfth parameter of the tuple.</param>
    /// <param name="arg13">The value of the thirteenth parameter of the tuple.</param>
    /// <param name="arg14">The value of the fourteenth parameter of the tuple.</param>
    /// <param name="arg15">The value of the fifteenth parameter of the tuple.</param>
    /// <param name="arg16">The value of the sixteenth parameter of the tuple.</param>
    /// <param name="arg17">The value of the seventeenth parameter of the tuple.</param>
    /// <param name="arg18">The value of the eighteenth parameter of the tuple.</param>
    /// <param name="arg19">The value of the nineteenth parameter of the tuple.</param>
    /// <returns>A 19-tuple (novemdecuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19).</returns>
    public static MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateNineteen<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19)
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19);
    }

    /// <summary>
    /// Creates a new 20-tuple, or viguple, representation of a <see cref="MutableTuple"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the tuple.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the tuple.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T17">The type of the seventeenth parameter of the tuple.</typeparam>
    /// <typeparam name="T18">The type of the eighteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T19">The type of the nineteenth parameter of the tuple.</typeparam>
    /// <typeparam name="T20">The type of the twentieth parameter of the tuple.</typeparam>
    /// <param name="arg1">The value of the first parameter of the tuple.</param>
    /// <param name="arg2">The value of the second parameter of the tuple.</param>
    /// <param name="arg3">The value of the third parameter of the tuple.</param>
    /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
    /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
    /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
    /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
    /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
    /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
    /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
    /// <param name="arg11">The value of the eleventh parameter of the tuple.</param>
    /// <param name="arg12">The value of the twelfth parameter of the tuple.</param>
    /// <param name="arg13">The value of the thirteenth parameter of the tuple.</param>
    /// <param name="arg14">The value of the fourteenth parameter of the tuple.</param>
    /// <param name="arg15">The value of the fifteenth parameter of the tuple.</param>
    /// <param name="arg16">The value of the sixteenth parameter of the tuple.</param>
    /// <param name="arg17">The value of the seventeenth parameter of the tuple.</param>
    /// <param name="arg18">The value of the eighteenth parameter of the tuple.</param>
    /// <param name="arg19">The value of the nineteenth parameter of the tuple.</param>
    /// <param name="arg20">The value of the twentieth parameter of the tuple.</param>
    /// <returns>A 20-tuple (viguple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20).</returns>
    public static MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> CreateTwenty<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, T20 arg20)
    {
        return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20);
    }
}
