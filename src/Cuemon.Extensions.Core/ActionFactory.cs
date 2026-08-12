using System;

namespace Cuemon.Extensions;
/// <summary>
/// Provides access to factory methods for creating <see cref="ActionFactory{TTuple}"/> instances that encapsulate a delegate with a variable amount of generic arguments.
/// </summary>
public static class ActionFactory
{
    /// <summary>
    /// Creates a new <see cref="ActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/>.
    /// </summary>
    /// <param name="method">The delegate to invoke.</param>
    /// <returns>An instance of <see cref="ActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/>.</returns>
    public static ActionFactory<MutableTuple> Create(Action method)
    {
        return new ActionFactory<MutableTuple>(_ => method(), MutableTupleFactory.CreateZero(), method);
    }

    /// <summary>
    /// Creates a new <see cref="ActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and one generic argument.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="arg">The parameter of the delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="ActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and one generic argument.</returns>
    public static ActionFactory<MutableTuple<T>> Create<T>(Action<T> method, T arg)
    {
        return new ActionFactory<MutableTuple<T>>(tuple => method(tuple.Arg1), MutableTupleFactory.CreateOne(arg), method);
    }

    /// <summary>
    /// Creates a new <see cref="ActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and two generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="ActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and two generic arguments.</returns>
    public static ActionFactory<MutableTuple<T1, T2>> Create<T1, T2>(Action<T1, T2> method, T1 arg1, T2 arg2)
    {
        return new ActionFactory<MutableTuple<T1, T2>>(tuple => method(tuple.Arg1, tuple.Arg2), MutableTupleFactory.CreateTwo(arg1, arg2), method);
    }

    /// <summary>
    /// Creates a new <see cref="ActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and three generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="ActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and three generic arguments.</returns>
    public static ActionFactory<MutableTuple<T1, T2, T3>> Create<T1, T2, T3>(Action<T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3)
    {
        return new ActionFactory<MutableTuple<T1, T2, T3>>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3), MutableTupleFactory.CreateThree(arg1, arg2, arg3), method);
    }

    /// <summary>
    /// Creates a new <see cref="ActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and four generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="ActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and four generic arguments.</returns>
    public static ActionFactory<MutableTuple<T1, T2, T3, T4>> Create<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return new ActionFactory<MutableTuple<T1, T2, T3, T4>>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4), MutableTupleFactory.CreateFour(arg1, arg2, arg3, arg4), method);
    }

    /// <summary>
    /// Creates a new <see cref="ActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and five generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="ActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and five generic arguments.</returns>
    public static ActionFactory<MutableTuple<T1, T2, T3, T4, T5>> Create<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return new ActionFactory<MutableTuple<T1, T2, T3, T4, T5>>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5), MutableTupleFactory.CreateFive(arg1, arg2, arg3, arg4, arg5), method);
    }

    /// <summary>
    /// Creates a new <see cref="ActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and six generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg6">The sixth parameter of the delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="ActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and six generic arguments.</returns>
    public static ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6>> Create<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return new ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6>>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6), MutableTupleFactory.CreateSix(arg1, arg2, arg3, arg4, arg5, arg6), method);
    }

    /// <summary>
    /// Creates a new <see cref="ActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and seven generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg6">The sixth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg7">The seventh parameter of the delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="ActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and seven generic arguments.</returns>
    public static ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7>> Create<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return new ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7>>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7), MutableTupleFactory.CreateSeven(arg1, arg2, arg3, arg4, arg5, arg6, arg7), method);
    }

    /// <summary>
    /// Creates a new <see cref="ActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and eight generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg6">The sixth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg7">The seventh parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg8">The eighth parameter of the delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="ActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and eight generic arguments.</returns>
    public static ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8>> Create<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        return new ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8>>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8), MutableTupleFactory.CreateEight(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), method);
    }

    /// <summary>
    /// Creates a new <see cref="ActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and nine generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg6">The sixth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg7">The seventh parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg8">The eighth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg9">The ninth parameter of the delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="ActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and nine generic arguments.</returns>
    public static ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        return new ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9), MutableTupleFactory.CreateNine(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), method);
    }

    /// <summary>
    /// Creates a new <see cref="ActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and ten generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg6">The sixth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg7">The seventh parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg8">The eighth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg9">The ninth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg10">The tenth parameter of the delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="ActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and ten generic arguments.</returns>
    public static ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
    {
        return new ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10), MutableTupleFactory.CreateTen(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), method);
    }

    /// <summary>
    /// Creates a new <see cref="ActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and eleven generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg6">The sixth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg7">The seventh parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg8">The eighth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg9">The ninth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg10">The tenth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg11">The eleventh parameter of the delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="ActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and eleven generic arguments.</returns>
    public static ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
    {
        return new ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11), MutableTupleFactory.CreateEleven(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), method);
    }

    /// <summary>
    /// Creates a new <see cref="ActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and twelfth generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg6">The sixth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg7">The seventh parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg8">The eighth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg9">The ninth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg10">The tenth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg11">The eleventh parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg12">The twelfth parameter of the delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="ActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and twelfth generic arguments.</returns>
    public static ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
    {
        return new ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12), MutableTupleFactory.CreateTwelve(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), method);
    }

    /// <summary>
    /// Creates a new <see cref="ActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and thirteen generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg6">The sixth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg7">The seventh parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg8">The eighth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg9">The ninth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg10">The tenth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg11">The eleventh parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg12">The twelfth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg13">The thirteenth parameter of the delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="ActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and thirteen generic arguments.</returns>
    public static ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
    {
        return new ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, tuple.Arg13), MutableTupleFactory.CreateThirteen(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13), method);
    }

    /// <summary>
    /// Creates a new <see cref="ActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and fourteen generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg6">The sixth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg7">The seventh parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg8">The eighth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg9">The ninth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg10">The tenth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg11">The eleventh parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg12">The twelfth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg13">The thirteenth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg14">The fourteenth parameter of the delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="ActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and fourteen generic arguments.</returns>
    public static ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
    {
        return new ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, tuple.Arg13, tuple.Arg14), MutableTupleFactory.CreateFourteen(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14), method);
    }

    /// <summary>
    /// Creates a new <see cref="ActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and fifteen generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of the delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg6">The sixth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg7">The seventh parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg8">The eighth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg9">The ninth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg10">The tenth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg11">The eleventh parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg12">The twelfth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg13">The thirteenth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg14">The fourteenth parameter of the delegate <paramref name="method"/>.</param>
    /// <param name="arg15">The fifteenth parameter of the delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="ActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and fifteen generic arguments.</returns>
    public static ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
    {
        return new ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, tuple.Arg13, tuple.Arg14, tuple.Arg15), MutableTupleFactory.CreateFifteen(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15), method);
    }

    /// <summary>
    /// Invokes the specified delegate <paramref name="method"/> with a n-<paramref name="tuple"/> argument.
    /// </summary>
    /// <typeparam name="TTuple">The type of the n-tuple representation of a <see cref="MutableTuple"/>.</typeparam>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="tuple">The n-tuple argument of <paramref name="method"/>.</param>
    public static void Invoke<TTuple>(Action<TTuple> method, TTuple tuple) where TTuple : MutableTuple
    {
        var factory = new ActionFactory<TTuple>(method, tuple);
        factory.ExecuteMethod();
    }
}
