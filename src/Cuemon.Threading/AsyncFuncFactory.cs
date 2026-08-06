using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Threading;
/// <summary>
/// Provides access to factory methods for creating <see cref="AsyncFuncFactory{TTuple,TResult}"/> instances that encapsulate a <see cref="Task{TResult}"/> based function delegate with a variable amount of generic arguments.
/// </summary>
public static class AsyncFuncFactory
{
    /// <summary>
    /// Creates a new <see cref="AsyncFuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <returns>An instance of <see cref="AsyncFuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/>.</returns>
    public static AsyncFuncFactory<MutableTuple, TResult> Create<TResult>(Func<CancellationToken, Task<TResult>> method)
    {
        return new AsyncFuncFactory<MutableTuple, TResult>((_, token) => method(token), new MutableTuple(), method);
    }

    /// <summary>
    /// Creates a new <see cref="AsyncFuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and one generic argument.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="arg">The parameter of the function delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="AsyncFuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and one generic argument.</returns>
    public static AsyncFuncFactory<MutableTuple<T>, TResult> Create<T, TResult>(Func<T, CancellationToken, Task<TResult>> method, T arg)
    {
        return new AsyncFuncFactory<MutableTuple<T>, TResult>((tuple, token) => method(tuple.Arg1, token), new MutableTuple<T>(arg), method);
    }

    /// <summary>
    /// Creates a new <see cref="AsyncFuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and two generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="AsyncFuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and two generic arguments.</returns>
    public static AsyncFuncFactory<MutableTuple<T1, T2>, TResult> Create<T1, T2, TResult>(Func<T1, T2, CancellationToken, Task<TResult>> method, T1 arg1, T2 arg2)
    {
        return new AsyncFuncFactory<MutableTuple<T1, T2>, TResult>((tuple, token) => method(tuple.Arg1, tuple.Arg2, token), new MutableTuple<T1, T2>(arg1, arg2), method);
    }

    /// <summary>
    /// Creates a new <see cref="AsyncFuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and three generic arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="method"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
    /// <param name="method">The function delegate to invoke.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="method"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="method"/>.</param>
    /// <returns>An instance of <see cref="AsyncFuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and three generic arguments.</returns>
    public static AsyncFuncFactory<MutableTuple<T1, T2, T3>, TResult> Create<T1, T2, T3, TResult>(Func<T1, T2, T3, CancellationToken, Task<TResult>> method, T1 arg1, T2 arg2, T3 arg3)
    {
        return new AsyncFuncFactory<MutableTuple<T1, T2, T3>, TResult>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, token), new MutableTuple<T1, T2, T3>(arg1, arg2, arg3), method);
    }

    /// <summary>
    /// Creates a new <see cref="AsyncFuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and four generic arguments.
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
    /// <returns>An instance of <see cref="AsyncFuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and four generic arguments.</returns>
    public static AsyncFuncFactory<MutableTuple<T1, T2, T3, T4>, TResult> Create<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, CancellationToken, Task<TResult>> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return new AsyncFuncFactory<MutableTuple<T1, T2, T3, T4>, TResult>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, token), new MutableTuple<T1, T2, T3, T4>(arg1, arg2, arg3, arg4), method);
    }

    /// <summary>
    /// Creates a new <see cref="AsyncFuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and five generic arguments.
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
    /// <returns>An instance of <see cref="AsyncFuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and five generic arguments.</returns>
    public static AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5>, TResult> Create<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, CancellationToken, Task<TResult>> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return new AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5>, TResult>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, token), new MutableTuple<T1, T2, T3, T4, T5>(arg1, arg2, arg3, arg4, arg5), method);
    }

    /// <summary>
    /// Creates a new <see cref="AsyncFuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and six generic arguments.
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
    /// <returns>An instance of <see cref="AsyncFuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and six generic arguments.</returns>
    public static AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6>, TResult> Create<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, CancellationToken, Task<TResult>> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return new AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6>, TResult>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, token), new MutableTuple<T1, T2, T3, T4, T5, T6>(arg1, arg2, arg3, arg4, arg5, arg6), method);
    }

    /// <summary>
    /// Creates a new <see cref="AsyncFuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and seven generic arguments.
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
    /// <returns>An instance of <see cref="AsyncFuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and seven generic arguments.</returns>
    public static AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7>, TResult> Create<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, Task<TResult>> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return new AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7>, TResult>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, token), new MutableTuple<T1, T2, T3, T4, T5, T6, T7>(arg1, arg2, arg3, arg4, arg5, arg6, arg7), method);
    }

    /// <summary>
    /// Creates a new <see cref="AsyncFuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and eight generic arguments.
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
    /// <returns>An instance of <see cref="AsyncFuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and eight generic arguments.</returns>
    public static AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8>, TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, Task<TResult>> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        return new AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8>, TResult>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, token), new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), method);
    }

    /// <summary>
    /// Creates a new <see cref="AsyncFuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and nine generic arguments.
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
    /// <returns>An instance of <see cref="AsyncFuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and nine generic arguments.</returns>
    public static AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>, TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, Task<TResult>> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        return new AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>, TResult>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, token), new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), method);
    }

    /// <summary>
    /// Creates a new <see cref="AsyncFuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and ten generic arguments.
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
    /// <returns>An instance of <see cref="AsyncFuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and ten generic arguments.</returns>
    public static AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, Task<TResult>> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
    {
        return new AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, TResult>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, token), new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), method);
    }

    /// <summary>
    /// Creates a new <see cref="AsyncFuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and eleven generic arguments.
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
    /// <returns>An instance of <see cref="AsyncFuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and eleven generic arguments.</returns>
    public static AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, CancellationToken, Task<TResult>> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
    {
        return new AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, TResult>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, token), new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), method);
    }

    /// <summary>
    /// Creates a new <see cref="AsyncFuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and twelfth generic arguments.
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
    /// <returns>An instance of <see cref="AsyncFuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and twelfth generic arguments.</returns>
    public static AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, CancellationToken, Task<TResult>> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
    {
        return new AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, TResult>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, token), new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), method);
    }

    /// <summary>
    /// Creates a new <see cref="AsyncFuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and thirteen generic arguments.
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
    /// <returns>An instance of <see cref="AsyncFuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and thirteen generic arguments.</returns>
    public static AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, CancellationToken, Task<TResult>> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
    {
        return new AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, TResult>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, tuple.Arg13, token), new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13), method);
    }

    /// <summary>
    /// Creates a new <see cref="AsyncFuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and fourteen generic arguments.
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
    /// <returns>An instance of <see cref="AsyncFuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and fourteen generic arguments.</returns>
    public static AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, CancellationToken, Task<TResult>> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
    {
        return new AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, TResult>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, tuple.Arg13, tuple.Arg14, token), new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14), method);
    }

    /// <summary>
    /// Creates a new <see cref="AsyncFuncFactory{TTuple,TResult}"/> instance encapsulating the specified <paramref name="method"/> and fifteen generic arguments.
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
    /// <returns>An instance of <see cref="AsyncFuncFactory{TTuple,TResult}"/> object initialized with the specified <paramref name="method"/> and fifteen generic arguments.</returns>
    public static AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, CancellationToken, Task<TResult>> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
    {
        return new AsyncFuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, TResult>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, tuple.Arg13, tuple.Arg14, tuple.Arg15, token), new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15), method);
    }
}
