using System;
using System.Reflection;
using Cuemon.Reflection;

namespace Cuemon.Diagnostics;
/// <summary>
/// Provides a flexible, generic and lambda friendly way to perform time measuring operations.
/// </summary>
public static partial class TimeMeasure
{
    /// <summary>
    /// Gets or sets the callback that is invoked when a time measuring operation is completed.
    /// </summary>
    /// <value>A <see cref="Action{T}"/>. The default value is <c>null</c>.</value>
    public static Action<TimeMeasureProfiler> CompletedCallback { get; set; }

    /// <summary>
    /// Profile and time measure the specified <paramref name="action"/> delegate.
    /// </summary>
    /// <param name="action">The delegate to time measure.</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    public static TimeMeasureProfiler WithAction(Action action, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = new ActionFactory<MutableTuple>(_ => action(), new MutableTuple(), action);
        return WithActionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="action"/> delegate.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <param name="action">The delegate to time measure.</param>
    /// <param name="arg">The parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    public static TimeMeasureProfiler WithAction<T>(Action<T> action, T arg, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = new ActionFactory<MutableTuple<T>>(tuple => action(tuple.Arg1), new MutableTuple<T>(arg), action);
        return WithActionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="action"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <param name="action">The delegate to time measure.</param>
    /// <param name="arg1">The first parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg2">The second parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    public static TimeMeasureProfiler WithAction<T1, T2>(Action<T1, T2> action, T1 arg1, T2 arg2, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = new ActionFactory<MutableTuple<T1, T2>>(tuple => action(tuple.Arg1, tuple.Arg2), new MutableTuple<T1, T2>(arg1, arg2), action);
        return WithActionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="action"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <param name="action">The delegate to time measure.</param>
    /// <param name="arg1">The first parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg2">The second parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg3">The third parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    public static TimeMeasureProfiler WithAction<T1, T2, T3>(Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = new ActionFactory<MutableTuple<T1, T2, T3>>(tuple => action(tuple.Arg1, tuple.Arg2, tuple.Arg3), new MutableTuple<T1, T2, T3>(arg1, arg2, arg3), action);
        return WithActionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="action"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <param name="action">The delegate to time measure.</param>
    /// <param name="arg1">The first parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg2">The second parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg3">The third parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg4">The fourth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    public static TimeMeasureProfiler WithAction<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = new ActionFactory<MutableTuple<T1, T2, T3, T4>>(tuple => action(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4), new MutableTuple<T1, T2, T3, T4>(arg1, arg2, arg3, arg4), action);
        return WithActionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="action"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <param name="action">The delegate to time measure.</param>
    /// <param name="arg1">The first parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg2">The second parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg3">The third parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg4">The fourth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg5">The fifth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    public static TimeMeasureProfiler WithAction<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = new ActionFactory<MutableTuple<T1, T2, T3, T4, T5>>(tuple => action(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5), new MutableTuple<T1, T2, T3, T4, T5>(arg1, arg2, arg3, arg4, arg5), action);
        return WithActionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="action"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <param name="action">The delegate to time measure.</param>
    /// <param name="arg1">The first parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg2">The second parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg3">The third parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg4">The fourth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg5">The fifth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg6">The sixth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    public static TimeMeasureProfiler WithAction<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = new ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6>>(tuple => action(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6), new MutableTuple<T1, T2, T3, T4, T5, T6>(arg1, arg2, arg3, arg4, arg5, arg6), action);
        return WithActionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="action"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <param name="action">The delegate to time measure.</param>
    /// <param name="arg1">The first parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg2">The second parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg3">The third parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg4">The fourth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg5">The fifth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg6">The sixth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg7">The seventh parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    public static TimeMeasureProfiler WithAction<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = new ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7>>(tuple => action(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7), new MutableTuple<T1, T2, T3, T4, T5, T6, T7>(arg1, arg2, arg3, arg4, arg5, arg6, arg7), action);
        return WithActionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="action"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <param name="action">The delegate to time measure.</param>
    /// <param name="arg1">The first parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg2">The second parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg3">The third parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg4">The fourth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg5">The fifth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg6">The sixth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg7">The seventh parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg8">The eighth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    public static TimeMeasureProfiler WithAction<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = new ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8>>(tuple => action(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8), new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), action);
        return WithActionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="action"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <param name="action">The delegate to time measure.</param>
    /// <param name="arg1">The first parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg2">The second parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg3">The third parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg4">The fourth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg5">The fifth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg6">The sixth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg7">The seventh parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg8">The eighth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg9">The ninth parameter of the <paramref name="action" /> delegate .</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    public static TimeMeasureProfiler WithAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = new ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>>(tuple => action(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9), new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), action);
        return WithActionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="action"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <param name="action">The delegate to time measure.</param>
    /// <param name="arg1">The first parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg2">The second parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg3">The third parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg4">The fourth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg5">The fifth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg6">The sixth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg7">The seventh parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg8">The eighth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg9">The ninth parameter of the <paramref name="action" /> delegate .</param>
    /// <param name="arg10">The tenth parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    public static TimeMeasureProfiler WithAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = new ActionFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>(tuple => action(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10), new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), action);
        return WithActionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="function"/> delegate.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the <paramref name="function"/> delegate.</typeparam>
    /// <param name="function">The function delegate to time measure.</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static TimeMeasureProfiler<TResult> WithFunc<TResult>(Func<TResult> function, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = new FuncFactory<MutableTuple, TResult>(_ => function(), new MutableTuple(), function);
        return WithFunctionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="function"/> delegate.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the <paramref name="function"/> delegate.</typeparam>
    /// <param name="function">The function delegate to time measure.</param>
    /// <param name="arg">The parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static TimeMeasureProfiler<TResult> WithFunc<T, TResult>(Func<T, TResult> function, T arg, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = new FuncFactory<MutableTuple<T>, TResult>(tuple => function(tuple.Arg1), new MutableTuple<T>(arg), function);
        return WithFunctionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="function"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the <paramref name="function"/> delegate.</typeparam>
    /// <param name="function">The function delegate to time measure.</param>
    /// <param name="arg1">The first parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg2">The second parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static TimeMeasureProfiler<TResult> WithFunc<T1, T2, TResult>(Func<T1, T2, TResult> function, T1 arg1, T2 arg2, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = new FuncFactory<MutableTuple<T1, T2>, TResult>(tuple => function(tuple.Arg1, tuple.Arg2), new MutableTuple<T1, T2>(arg1, arg2), function);
        return WithFunctionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="function"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the <paramref name="function"/> delegate.</typeparam>
    /// <param name="function">The function delegate to time measure.</param>
    /// <param name="arg1">The first parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg2">The second parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg3">The third parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static TimeMeasureProfiler<TResult> WithFunc<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> function, T1 arg1, T2 arg2, T3 arg3, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = new FuncFactory<MutableTuple<T1, T2, T3>, TResult>(tuple => function(tuple.Arg1, tuple.Arg2, tuple.Arg3), new MutableTuple<T1, T2, T3>(arg1, arg2, arg3), function);
        return WithFunctionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="function"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the <paramref name="function"/> delegate.</typeparam>
    /// <param name="function">The function delegate to time measure.</param>
    /// <param name="arg1">The first parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg2">The second parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg3">The third parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg4">The fourth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static TimeMeasureProfiler<TResult> WithFunc<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = new FuncFactory<MutableTuple<T1, T2, T3, T4>, TResult>(tuple => function(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4), new MutableTuple<T1, T2, T3, T4>(arg1, arg2, arg3, arg4), function);
        return WithFunctionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="function"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the <paramref name="function"/> delegate.</typeparam>
    /// <param name="function">The function delegate to time measure.</param>
    /// <param name="arg1">The first parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg2">The second parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg3">The third parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg4">The fourth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg5">The fifth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static TimeMeasureProfiler<TResult> WithFunc<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = new FuncFactory<MutableTuple<T1, T2, T3, T4, T5>, TResult>(tuple => function(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5), new MutableTuple<T1, T2, T3, T4, T5>(arg1, arg2, arg3, arg4, arg5), function);
        return WithFunctionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="function"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the <paramref name="function"/> delegate.</typeparam>
    /// <param name="function">The function delegate to time measure.</param>
    /// <param name="arg1">The first parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg2">The second parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg3">The third parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg4">The fourth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg5">The fifth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg6">The sixth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static TimeMeasureProfiler<TResult> WithFunc<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = new FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6>, TResult>(tuple => function(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6), new MutableTuple<T1, T2, T3, T4, T5, T6>(arg1, arg2, arg3, arg4, arg5, arg6), function);
        return WithFunctionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="function"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the <paramref name="function"/> delegate.</typeparam>
    /// <param name="function">The function delegate to time measure.</param>
    /// <param name="arg1">The first parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg2">The second parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg3">The third parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg4">The fourth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg5">The fifth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg6">The sixth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg7">The seventh parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static TimeMeasureProfiler<TResult> WithFunc<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = new FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7>, TResult>(tuple => function(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7), new MutableTuple<T1, T2, T3, T4, T5, T6, T7>(arg1, arg2, arg3, arg4, arg5, arg6, arg7), function);
        return WithFunctionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="function"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the <paramref name="function"/> delegate.</typeparam>
    /// <param name="function">The function delegate to time measure.</param>
    /// <param name="arg1">The first parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg2">The second parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg3">The third parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg4">The fourth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg5">The fifth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg6">The sixth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg7">The seventh parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg8">The eighth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static TimeMeasureProfiler<TResult> WithFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = new FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8>, TResult>(tuple => function(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8), new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), function);
        return WithFunctionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="function"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the <paramref name="function"/> delegate.</typeparam>
    /// <param name="function">The function delegate to time measure.</param>
    /// <param name="arg1">The first parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg2">The second parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg3">The third parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg4">The fourth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg5">The fifth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg6">The sixth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg7">The seventh parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg8">The eighth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg9">The ninth parameter of the <paramref name="function" /> delegate .</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static TimeMeasureProfiler<TResult> WithFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = new FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>, TResult>(tuple => function(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9), new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), function);
        return WithFunctionCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="function"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the <paramref name="function"/> delegate.</typeparam>
    /// <param name="function">The function delegate to time measure.</param>
    /// <param name="arg1">The first parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg2">The second parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg3">The third parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg4">The fourth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg5">The fifth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg6">The sixth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg7">The seventh parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg8">The eighth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="arg9">The ninth parameter of the <paramref name="function" /> delegate .</param>
    /// <param name="arg10">The tenth parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="setup">The <see cref="TimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static TimeMeasureProfiler<TResult> WithFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, Action<TimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = new FuncFactory<MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, TResult>(tuple => function(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10), new MutableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), function);
        return WithFunctionCore(factory, setup);
    }

    private static TimeMeasureProfiler WithActionCore<TTuple>(ActionFactory<TTuple> factory, Action<TimeMeasureOptions> setup) where TTuple : MutableTuple
    {
        var options = Patterns.Configure(setup);
        var descriptor = options.MethodDescriptor?.Invoke() ?? new MethodDescriptor(factory.DelegateInfo);
        var profiler = new TimeMeasureProfiler()
        {
            Member = descriptor,
            Data = descriptor.MergeParameters(options.RuntimeParameters ?? factory.GenericArguments.ToArray())
        };
        PerformTimeMeasuring(profiler, options, _ => factory.ExecuteMethod());
        return profiler;
    }

    private static TimeMeasureProfiler<TResult> WithFunctionCore<TTuple, TResult>(FuncFactory<TTuple, TResult> factory, Action<TimeMeasureOptions> setup) where TTuple : MutableTuple
    {
        var options = Patterns.Configure(setup);
        var descriptor = options.MethodDescriptor?.Invoke() ?? new MethodDescriptor(factory.DelegateInfo);
        var profiler = new TimeMeasureProfiler<TResult>()
        {
            Member = descriptor,
            Data = descriptor.MergeParameters(options.RuntimeParameters ?? factory.GenericArguments.ToArray())
        };
        PerformTimeMeasuring(profiler, options, p => p.Result = factory.ExecuteMethod());
        return profiler;
    }

    private static void PerformTimeMeasuring<T>(T profiler, TimeMeasureOptions options, Action<T> handler) where T : TimeMeasureProfiler
    {
        try
        {
            profiler.Timer.Start();
            handler(profiler);
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException ?? ex; // don't confuse the end-user with reflection related details; return the originating exception
        }
        finally
        {
            profiler.Timer.Stop();
            if (options.TimeMeasureCompletedThreshold == TimeSpan.Zero || profiler.Elapsed > options.TimeMeasureCompletedThreshold)
            {
                CompletedCallback?.Invoke(profiler);
            }
        }
    }
}
