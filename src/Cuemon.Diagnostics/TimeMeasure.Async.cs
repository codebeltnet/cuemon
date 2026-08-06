using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Reflection;
using Cuemon.Threading;

namespace Cuemon.Diagnostics;
public static partial class TimeMeasure
{
    /// <summary>
    /// Profile and time measure the specified <paramref name="action"/> delegate.
    /// </summary>
    /// <param name="action">The delegate to time measure.</param>
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="action"/> cannot be null.
    /// </exception>
    public static Task<TimeMeasureProfiler> WithActionAsync(Func<CancellationToken, Task> action, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = AsyncActionFactory.Create(action);
        return WithActionAsyncCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="action"/> delegate.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <param name="action">The delegate to time measure.</param>
    /// <param name="arg">The parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="action"/> cannot be null.
    /// </exception>
    public static Task<TimeMeasureProfiler> WithActionAsync<T>(Func<T, CancellationToken, Task> action, T arg, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = AsyncActionFactory.Create(action, arg);
        return WithActionAsyncCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="action"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the <paramref name="action" /> delegate.</typeparam>
    /// <param name="action">The delegate to time measure.</param>
    /// <param name="arg1">The first parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="arg2">The second parameter of the <paramref name="action" /> delegate.</param>
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="action"/> cannot be null.
    /// </exception>
    public static Task<TimeMeasureProfiler> WithActionAsync<T1, T2>(Func<T1, T2, CancellationToken, Task> action, T1 arg1, T2 arg2, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = AsyncActionFactory.Create(action, arg1, arg2);
        return WithActionAsyncCore(factory, setup);
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
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="action"/> cannot be null.
    /// </exception>
    public static Task<TimeMeasureProfiler> WithActionAsync<T1, T2, T3>(Func<T1, T2, T3, CancellationToken, Task> action, T1 arg1, T2 arg2, T3 arg3, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = AsyncActionFactory.Create(action, arg1, arg2, arg3);
        return WithActionAsyncCore(factory, setup);
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
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="action"/> cannot be null.
    /// </exception>
    public static Task<TimeMeasureProfiler> WithActionAsync<T1, T2, T3, T4>(Func<T1, T2, T3, T4, CancellationToken, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = AsyncActionFactory.Create(action, arg1, arg2, arg3, arg4);
        return WithActionAsyncCore(factory, setup);
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
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="action"/> cannot be null.
    /// </exception>
    public static Task<TimeMeasureProfiler> WithActionAsync<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, CancellationToken, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = AsyncActionFactory.Create(action, arg1, arg2, arg3, arg4, arg5);
        return WithActionAsyncCore(factory, setup);
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
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="action"/> cannot be null.
    /// </exception>
    public static Task<TimeMeasureProfiler> WithActionAsync<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, CancellationToken, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = AsyncActionFactory.Create(action, arg1, arg2, arg3, arg4, arg5, arg6);
        return WithActionAsyncCore(factory, setup);
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
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="action"/> cannot be null.
    /// </exception>
    public static Task<TimeMeasureProfiler> WithActionAsync<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = AsyncActionFactory.Create(action, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        return WithActionAsyncCore(factory, setup);
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
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="action"/> cannot be null.
    /// </exception>
    public static Task<TimeMeasureProfiler> WithActionAsync<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = AsyncActionFactory.Create(action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        return WithActionAsyncCore(factory, setup);
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
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="action"/> cannot be null.
    /// </exception>
    public static Task<TimeMeasureProfiler> WithActionAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = AsyncActionFactory.Create(action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        return WithActionAsyncCore(factory, setup);
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
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler"/> with the result of the time measuring.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="action"/> cannot be null.
    /// </exception>
    public static Task<TimeMeasureProfiler> WithActionAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(action);
        var factory = AsyncActionFactory.Create(action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        return WithActionAsyncCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="function"/> delegate.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the <paramref name="function"/> delegate.</typeparam>
    /// <param name="function">The function delegate to time measure.</param>

    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static Task<TimeMeasureProfiler<TResult>> WithFuncAsync<TResult>(Func<CancellationToken, Task<TResult>> function, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = AsyncFuncFactory.Create(function);
        return WithFunctionAsyncCore(factory, setup);
    }

    /// <summary>
    /// Profile and time measure the specified <paramref name="function"/> delegate.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the <paramref name="function" /> delegate.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the <paramref name="function"/> delegate.</typeparam>
    /// <param name="function">The function delegate to time measure.</param>
    /// <param name="arg">The parameter of the <paramref name="function" /> delegate.</param>
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static Task<TimeMeasureProfiler<TResult>> WithFuncAsync<T, TResult>(Func<T, CancellationToken, Task<TResult>> function, T arg, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = AsyncFuncFactory.Create(function, arg);
        return WithFunctionAsyncCore(factory, setup);
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
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static Task<TimeMeasureProfiler<TResult>> WithFuncAsync<T1, T2, TResult>(Func<T1, T2, CancellationToken, Task<TResult>> function, T1 arg1, T2 arg2, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = AsyncFuncFactory.Create(function, arg1, arg2);
        return WithFunctionAsyncCore(factory, setup);
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
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static Task<TimeMeasureProfiler<TResult>> WithFuncAsync<T1, T2, T3, TResult>(Func<T1, T2, T3, CancellationToken, Task<TResult>> function, T1 arg1, T2 arg2, T3 arg3, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = AsyncFuncFactory.Create(function, arg1, arg2, arg3);
        return WithFunctionAsyncCore(factory, setup);
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
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static Task<TimeMeasureProfiler<TResult>> WithFuncAsync<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, CancellationToken, Task<TResult>> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = AsyncFuncFactory.Create(function, arg1, arg2, arg3, arg4);
        return WithFunctionAsyncCore(factory, setup);
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
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static Task<TimeMeasureProfiler<TResult>> WithFuncAsync<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, CancellationToken, Task<TResult>> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = AsyncFuncFactory.Create(function, arg1, arg2, arg3, arg4, arg5);
        return WithFunctionAsyncCore(factory, setup);
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
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static Task<TimeMeasureProfiler<TResult>> WithFuncAsync<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, CancellationToken, Task<TResult>> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = AsyncFuncFactory.Create(function, arg1, arg2, arg3, arg4, arg5, arg6);
        return WithFunctionAsyncCore(factory, setup);
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
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static Task<TimeMeasureProfiler<TResult>> WithFuncAsync<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, Task<TResult>> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = AsyncFuncFactory.Create(function, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        return WithFunctionAsyncCore(factory, setup);
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
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static Task<TimeMeasureProfiler<TResult>> WithFuncAsync<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, Task<TResult>> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = AsyncFuncFactory.Create(function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        return WithFunctionAsyncCore(factory, setup);
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
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static Task<TimeMeasureProfiler<TResult>> WithFuncAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, Task<TResult>> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = AsyncFuncFactory.Create(function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        return WithFunctionAsyncCore(factory, setup);
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
    /// <param name="setup">The <see cref="AsyncTimeMeasureOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TimeMeasureProfiler{TResult}"/> with the result of the time measuring and the encapsulated <paramref name="function"/> delegate.</returns>
    public static Task<TimeMeasureProfiler<TResult>> WithFuncAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, Task<TResult>> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, Action<AsyncTimeMeasureOptions> setup = null)
    {
        Validator.ThrowIfNull(function);
        var factory = AsyncFuncFactory.Create(function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        return WithFunctionAsyncCore(factory, setup);
    }

    private static async Task<TimeMeasureProfiler> WithActionAsyncCore<TTuple>(AsyncActionFactory<TTuple> factory, Action<AsyncTimeMeasureOptions> setup) where TTuple : MutableTuple
    {
        var options = Patterns.Configure(setup);
        var descriptor = options.MethodDescriptor?.Invoke() ?? new MethodDescriptor(factory.DelegateInfo);
        var profiler = new TimeMeasureProfiler()
        {
            Member = descriptor,
            Data = descriptor.MergeParameters(options.RuntimeParameters ?? factory.GenericArguments.ToArray())
        };
        await PerformTimeMeasuringAsync(profiler, options, async _ => await factory.ExecuteMethodAsync(options.CancellationToken).ConfigureAwait(false)).ConfigureAwait(false);
        return profiler;
    }

    private static async Task<TimeMeasureProfiler<TResult>> WithFunctionAsyncCore<TTuple, TResult>(AsyncFuncFactory<TTuple, TResult> factory, Action<AsyncTimeMeasureOptions> setup) where TTuple : MutableTuple
    {
        var options = Patterns.Configure(setup);
        var descriptor = options.MethodDescriptor?.Invoke() ?? new MethodDescriptor(factory.DelegateInfo);
        var profiler = new TimeMeasureProfiler<TResult>()
        {
            Member = descriptor,
            Data = descriptor.MergeParameters(options.RuntimeParameters ?? factory.GenericArguments.ToArray())
        };
        await PerformTimeMeasuringAsync(profiler, options, async p => p.Result = await factory.ExecuteMethodAsync(options.CancellationToken).ConfigureAwait(false)).ConfigureAwait(false);
        return profiler;
    }

    private static async Task PerformTimeMeasuringAsync<T>(T profiler, AsyncTimeMeasureOptions options, Func<T, Task> handler) where T : TimeMeasureProfiler
    {
        try
        {
            profiler.Timer.Start();
            await handler(profiler).ConfigureAwait(false);
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
