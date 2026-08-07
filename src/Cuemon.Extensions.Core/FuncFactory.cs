using System;

namespace Cuemon.Extensions;
/// <summary>
/// Provides access to factory methods for creating <see cref="FuncFactory{TTuple,TResult}"/> instances that encapsulate a function delegate with a variable amount of generic arguments.
/// </summary>
public static class FuncFactory
{
    /// <summary>
    /// Creates a new <see cref="FuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <returns>An instance of <see cref="FuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/>.</returns>
    public static FuncFactory<MutableTuple, TResult> Create<TResult>(Func<TResult> method)
    {
        return new FuncFactory<MutableTuple, TResult>(_ => method(), MutableTupleFactory.CreateZero(), method);
    }

    /// <summary>
    /// Creates a new <see cref="FuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and one generic argument.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="arg">The parameter of the function delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="FuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and one generic argument.</returns>
    public static FuncFactory<MutableTuple<T>, TResult> Create<T, TResult>(Func<T, TResult> method, T arg)
    {
        return new FuncFactory<MutableTuple<T>, TResult>(tuple => method(tuple.Arg1), MutableTupleFactory.CreateOne(arg), method);
    }

    /// <summary>
    /// Creates a new <see cref="FuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and two generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="FuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and two generic arguments.</returns>
    public static FuncFactory<MutableTuple<T1, T2>, TResult> Create<T1, T2, TResult>(Func<T1, T2, TResult> method, T1 arg1, T2 arg2)
    {
        return new FuncFactory<MutableTuple<T1, T2>, TResult>(tuple => method(tuple.Arg1, tuple.Arg2), MutableTupleFactory.CreateTwo(arg1, arg2), method);
    }

    /// <summary>
    /// Creates a new <see cref="FuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and three generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="FuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and three generic arguments.</returns>
    public static FuncFactory<MutableTuple<T1, T2, T3>, TResult> Create<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> method, T1 arg1, T2 arg2, T3 arg3)
    {
        return new FuncFactory<MutableTuple<T1, T2, T3>, TResult>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3), MutableTupleFactory.CreateThree(arg1, arg2, arg3), method);
    }

    /// <summary>
    /// Creates a new <see cref="FuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and four generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="FuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and four generic arguments.</returns>
    public static FuncFactory<MutableTuple<T1, T2, T3, T4>, TResult> Create<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return new FuncFactory<MutableTuple<T1, T2, T3, T4>, TResult>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4), MutableTupleFactory.CreateFour(arg1, arg2, arg3, arg4), method);
    }

    /// <summary>
    /// Creates a new <see cref="FuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and five generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the function delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="FuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and five generic arguments.</returns>
    public static FuncFactory<MutableTuple<T1, T2, T3, T4, T5>, TResult> Create<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return new FuncFactory<MutableTuple<T1, T2, T3, T4, T5>, TResult>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5), MutableTupleFactory.CreateFive(arg1, arg2, arg3, arg4, arg5), method);
    }

    /// <summary>
    /// Creates a new <see cref="FuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and six generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg6">The sixth parameter of the function delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="FuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and six generic arguments.</returns>
    public static FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6>, TResult> Create<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return new FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6>, TResult>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6), MutableTupleFactory.CreateSix(arg1, arg2, arg3, arg4, arg5, arg6), method);
    }

    /// <summary>
    /// Creates a new <see cref="FuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and seven generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg6">The sixth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg7">The seventh parameter of the function delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="FuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and seven generic arguments.</returns>
    public static FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7>, TResult> Create<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return new FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7>, TResult>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7), MutableTupleFactory.CreateSeven(arg1, arg2, arg3, arg4, arg5, arg6, arg7), method);
    }

    /// <summary>
    /// Creates a new <see cref="FuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and eight generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg6">The sixth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg7">The seventh parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg8">The eighth parameter of the function delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="FuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and eight generic arguments.</returns>
    public static FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8>, TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        return new FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8>, TResult>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8), MutableTupleFactory.CreateEight(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), method);
    }

    /// <summary>
    /// Creates a new <see cref="FuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and nine generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg6">The sixth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg7">The seventh parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg8">The eighth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg9">The ninth parameter of the function delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="FuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and nine generic arguments.</returns>
    public static FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>, TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        return new FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>, TResult>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9), MutableTupleFactory.CreateNine(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), method);
    }

    /// <summary>
    /// Creates a new <see cref="FuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and ten generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg6">The sixth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg7">The seventh parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg8">The eighth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg9">The ninth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg10">The tenth parameter of the function delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="FuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and ten generic arguments.</returns>
    public static FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
    {
        return new FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, TResult>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10), MutableTupleFactory.CreateTen(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), method);
    }

    /// <summary>
    /// Creates a new <see cref="FuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and eleven generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg6">The sixth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg7">The seventh parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg8">The eighth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg9">The ninth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg10">The tenth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg11">The eleventh parameter of the function delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="FuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and eleven generic arguments.</returns>
    public static FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
    {
        return new FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, TResult>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11), MutableTupleFactory.CreateEleven(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), method);
    }

    /// <summary>
    /// Creates a new <see cref="FuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and twelfth generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg6">The sixth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg7">The seventh parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg8">The eighth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg9">The ninth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg10">The tenth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg11">The eleventh parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg12">The twelfth parameter of the function delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="FuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and twelfth generic arguments.</returns>
    public static FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
    {
        return new FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, TResult>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12), MutableTupleFactory.CreateTwelve(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), method);
    }

    /// <summary>
    /// Creates a new <see cref="FuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and thirteen generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg6">The sixth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg7">The seventh parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg8">The eighth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg9">The ninth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg10">The tenth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg11">The eleventh parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg12">The twelfth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg13">The thirteenth parameter of the function delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="FuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and thirteen generic arguments.</returns>
    public static FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
    {
        return new FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, TResult>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, tuple.Arg13), MutableTupleFactory.CreateThirteen(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13), method);
    }

    /// <summary>
    /// Creates a new <see cref="FuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and fourteen generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg6">The sixth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg7">The seventh parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg8">The eighth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg9">The ninth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg10">The tenth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg11">The eleventh parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg12">The twelfth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg13">The thirteenth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg14">The fourteenth parameter of the function delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="FuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and fourteen generic arguments.</returns>
    public static FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
    {
        return new FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, TResult>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, tuple.Arg13, tuple.Arg14), MutableTupleFactory.CreateFourteen(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14), method);
    }

    /// <summary>
    /// Creates a new <see cref="FuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and fifteen generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg5">The fifth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg6">The sixth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg7">The seventh parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg8">The eighth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg9">The ninth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg10">The tenth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg11">The eleventh parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg12">The twelfth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg13">The thirteenth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg14">The fourteenth parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg15">The fifteenth parameter of the function delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="FuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and fifteen generic arguments.</returns>
    public static FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
    {
        return new FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, TResult>(tuple => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, tuple.Arg13, tuple.Arg14, tuple.Arg15), MutableTupleFactory.CreateFifteen(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15), method);
    }

    /// <summary>
    /// Invokes the specified delegate <paramref name="method" /> with a n-<paramref name="tuple" /> argument.
    /// </summary>
    /// <typeparam name="TTuple">The type of the n-tuple representation of a <see cref="MutableTuple" />.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method" />.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="tuple">The n-tuple argument of <paramref name="method" />.</param>
    /// <returns>The result of the function delegate <paramref name="method"/>.</returns>
    public static TResult Invoke<TTuple, TResult>(Func<TTuple, TResult> method, TTuple tuple) where TTuple : MutableTuple
    {
        var factory = new FuncFactory<TTuple, TResult>(method, tuple);
        return factory.ExecuteMethod();
    }
}
