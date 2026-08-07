using System;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Cuemon.Resilience;
public static partial class TransientOperation
{
    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive <see cref="Task{TResult}"/> based function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="setup">The <see cref="AsyncTransientOperationOptions"/> which may be configured.</param>
    /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
    /// <returns>A task that represents the asynchronous operation. The task result contains the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="faultSensitiveMethod"/> cannot be null.
    /// </exception>
    /// <exception cref="LatencyException">
    /// The <paramref name="faultSensitiveMethod"/> exceeded the maximum time allowed to network latency.
    /// </exception>
    /// <exception cref="AggregateException">
    /// The <paramref name="faultSensitiveMethod"/> was a victim of a transient fault. The <see cref="AggregateException.InnerExceptions"/> collection contains a <see cref="TransientFaultException"/> object.
    ///
    /// -or
    ///
    /// An exception was thrown during the invocation of the <paramref name="faultSensitiveMethod"/>. The <see cref="AggregateException.InnerExceptions"/> collection contains information about the exception or exceptions.
    /// </exception>
    public static Task<TResult> WithFuncAsync<TResult>(Func<CancellationToken, Task<TResult>> faultSensitiveMethod, Action<AsyncTransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = AsyncFuncFactory.Create(faultSensitiveMethod);
        return WithFuncAsyncCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive <see cref="Task{TResult}"/> based function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="arg">The parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="setup">The <see cref="AsyncTransientOperationOptions"/> which may be configured.</param>
    /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
    /// <returns>A task that represents the asynchronous operation. The task result contains the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="faultSensitiveMethod"/> cannot be null.
    /// </exception>
    /// <exception cref="LatencyException">
    /// The <paramref name="faultSensitiveMethod"/> exceeded the maximum time allowed to network latency.
    /// </exception>
    /// <exception cref="AggregateException">
    /// The <paramref name="faultSensitiveMethod"/> was a victim of a transient fault. The <see cref="AggregateException.InnerExceptions"/> collection contains a <see cref="TransientFaultException"/> object.
    ///
    /// -or
    ///
    /// An exception was thrown during the invocation of the <paramref name="faultSensitiveMethod"/>. The <see cref="AggregateException.InnerExceptions"/> collection contains information about the exception or exceptions.
    /// </exception>
    public static Task<TResult> WithFuncAsync<T, TResult>(Func<T, CancellationToken, Task<TResult>> faultSensitiveMethod, T arg, Action<AsyncTransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = AsyncFuncFactory.Create(faultSensitiveMethod, arg);
        return WithFuncAsyncCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive <see cref="Task{TResult}"/> based function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="setup">The <see cref="AsyncTransientOperationOptions"/> which may be configured.</param>
    /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
    /// <returns>A task that represents the asynchronous operation. The task result contains the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="faultSensitiveMethod"/> cannot be null.
    /// </exception>
    /// <exception cref="LatencyException">
    /// The <paramref name="faultSensitiveMethod"/> exceeded the maximum time allowed to network latency.
    /// </exception>
    /// <exception cref="AggregateException">
    /// The <paramref name="faultSensitiveMethod"/> was a victim of a transient fault. The <see cref="AggregateException.InnerExceptions"/> collection contains a <see cref="TransientFaultException"/> object.
    ///
    /// -or
    ///
    /// An exception was thrown during the invocation of the <paramref name="faultSensitiveMethod"/>. The <see cref="AggregateException.InnerExceptions"/> collection contains information about the exception or exceptions.
    /// </exception>
    public static Task<TResult> WithFuncAsync<T1, T2, TResult>(Func<T1, T2, CancellationToken, Task<TResult>> faultSensitiveMethod, T1 arg1, T2 arg2, Action<AsyncTransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = AsyncFuncFactory.Create(faultSensitiveMethod, arg1, arg2);
        return WithFuncAsyncCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive <see cref="Task{TResult}"/> based function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="setup">The <see cref="AsyncTransientOperationOptions"/> which may be configured.</param>
    /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
    /// <returns>A task that represents the asynchronous operation. The task result contains the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="faultSensitiveMethod"/> cannot be null.
    /// </exception>
    /// <exception cref="LatencyException">
    /// The <paramref name="faultSensitiveMethod"/> exceeded the maximum time allowed to network latency.
    /// </exception>
    /// <exception cref="AggregateException">
    /// The <paramref name="faultSensitiveMethod"/> was a victim of a transient fault. The <see cref="AggregateException.InnerExceptions"/> collection contains a <see cref="TransientFaultException"/> object.
    ///
    /// -or
    ///
    /// An exception was thrown during the invocation of the <paramref name="faultSensitiveMethod"/>. The <see cref="AggregateException.InnerExceptions"/> collection contains information about the exception or exceptions.
    /// </exception>
    public static Task<TResult> WithFuncAsync<T1, T2, T3, TResult>(Func<T1, T2, T3, CancellationToken, Task<TResult>> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, Action<AsyncTransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = AsyncFuncFactory.Create(faultSensitiveMethod, arg1, arg2, arg3);
        return WithFuncAsyncCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive <see cref="Task{TResult}"/> based function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="setup">The <see cref="AsyncTransientOperationOptions"/> which may be configured.</param>
    /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
    /// <returns>A task that represents the asynchronous operation. The task result contains the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="faultSensitiveMethod"/> cannot be null.
    /// </exception>
    /// <exception cref="LatencyException">
    /// The <paramref name="faultSensitiveMethod"/> exceeded the maximum time allowed to network latency.
    /// </exception>
    /// <exception cref="AggregateException">
    /// The <paramref name="faultSensitiveMethod"/> was a victim of a transient fault. The <see cref="AggregateException.InnerExceptions"/> collection contains a <see cref="TransientFaultException"/> object.
    ///
    /// -or
    ///
    /// An exception was thrown during the invocation of the <paramref name="faultSensitiveMethod"/>. The <see cref="AggregateException.InnerExceptions"/> collection contains information about the exception or exceptions.
    /// </exception>
    public static Task<TResult> WithFuncAsync<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, CancellationToken, Task<TResult>> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<AsyncTransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = AsyncFuncFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4);
        return WithFuncAsyncCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive <see cref="Task{TResult}"/> based function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg5">The fifth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="setup">The <see cref="AsyncTransientOperationOptions"/> which may be configured.</param>
    /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
    /// <returns>A task that represents the asynchronous operation. The task result contains the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="faultSensitiveMethod"/> cannot be null.
    /// </exception>
    /// <exception cref="LatencyException">
    /// The <paramref name="faultSensitiveMethod"/> exceeded the maximum time allowed to network latency.
    /// </exception>
    /// <exception cref="AggregateException">
    /// The <paramref name="faultSensitiveMethod"/> was a victim of a transient fault. The <see cref="AggregateException.InnerExceptions"/> collection contains a <see cref="TransientFaultException"/> object.
    ///
    /// -or
    ///
    /// An exception was thrown during the invocation of the <paramref name="faultSensitiveMethod"/>. The <see cref="AggregateException.InnerExceptions"/> collection contains information about the exception or exceptions.
    /// </exception>
    public static Task<TResult> WithFuncAsync<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, CancellationToken, Task<TResult>> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<AsyncTransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = AsyncFuncFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4, arg5);
        return WithFuncAsyncCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <param name="faultSensitiveMethod">The fault sensitive <see cref="Task"/> based function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="setup">The <see cref="AsyncTransientOperationOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="faultSensitiveMethod"/> cannot be null.
    /// </exception>
    /// <exception cref="LatencyException">
    /// The <paramref name="faultSensitiveMethod"/> exceeded the maximum time allowed to network latency.
    /// </exception>
    /// <exception cref="AggregateException">
    /// The <paramref name="faultSensitiveMethod"/> was a victim of a transient fault. The <see cref="AggregateException.InnerExceptions"/> collection contains a <see cref="TransientFaultException"/> object.
    ///
    /// -or
    ///
    /// An exception was thrown during the invocation of the <paramref name="faultSensitiveMethod"/>. The <see cref="AggregateException.InnerExceptions"/> collection contains information about the exception or exceptions.
    /// </exception>
    public static Task WithActionAsync(Func<CancellationToken, Task> faultSensitiveMethod, Action<AsyncTransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = AsyncActionFactory.Create(faultSensitiveMethod);
        return WithActionAsyncCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive <see cref="Task"/> based function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="arg">The parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="setup">The <see cref="AsyncTransientOperationOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="faultSensitiveMethod"/> cannot be null.
    /// </exception>
    /// <exception cref="LatencyException">
    /// The <paramref name="faultSensitiveMethod"/> exceeded the maximum time allowed to network latency.
    /// </exception>
    /// <exception cref="AggregateException">
    /// The <paramref name="faultSensitiveMethod"/> was a victim of a transient fault. The <see cref="AggregateException.InnerExceptions"/> collection contains a <see cref="TransientFaultException"/> object.
    ///
    /// -or
    ///
    /// An exception was thrown during the invocation of the <paramref name="faultSensitiveMethod"/>. The <see cref="AggregateException.InnerExceptions"/> collection contains information about the exception or exceptions.
    /// </exception>
    public static Task WithActionAsync<T>(Func<T, CancellationToken, Task> faultSensitiveMethod, T arg, Action<AsyncTransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = AsyncActionFactory.Create(faultSensitiveMethod, arg);
        return WithActionAsyncCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive <see cref="Task"/> based function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="setup">The <see cref="AsyncTransientOperationOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="faultSensitiveMethod"/> cannot be null.
    /// </exception>
    /// <exception cref="LatencyException">
    /// The <paramref name="faultSensitiveMethod"/> exceeded the maximum time allowed to network latency.
    /// </exception>
    /// <exception cref="AggregateException">
    /// The <paramref name="faultSensitiveMethod"/> was a victim of a transient fault. The <see cref="AggregateException.InnerExceptions"/> collection contains a <see cref="TransientFaultException"/> object.
    ///
    /// -or
    ///
    /// An exception was thrown during the invocation of the <paramref name="faultSensitiveMethod"/>. The <see cref="AggregateException.InnerExceptions"/> collection contains information about the exception or exceptions.
    /// </exception>
    public static Task WithActionAsync<T1, T2>(Func<T1, T2, CancellationToken, Task> faultSensitiveMethod, T1 arg1, T2 arg2, Action<AsyncTransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = AsyncActionFactory.Create(faultSensitiveMethod, arg1, arg2);
        return WithActionAsyncCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive <see cref="Task"/> based function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="setup">The <see cref="AsyncTransientOperationOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="faultSensitiveMethod"/> cannot be null.
    /// </exception>
    /// <exception cref="LatencyException">
    /// The <paramref name="faultSensitiveMethod"/> exceeded the maximum time allowed to network latency.
    /// </exception>
    /// <exception cref="AggregateException">
    /// The <paramref name="faultSensitiveMethod"/> was a victim of a transient fault. The <see cref="AggregateException.InnerExceptions"/> collection contains a <see cref="TransientFaultException"/> object.
    ///
    /// -or
    ///
    /// An exception was thrown during the invocation of the <paramref name="faultSensitiveMethod"/>. The <see cref="AggregateException.InnerExceptions"/> collection contains information about the exception or exceptions.
    /// </exception>
    public static Task WithActionAsync<T1, T2, T3>(Func<T1, T2, T3, CancellationToken, Task> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, Action<AsyncTransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = AsyncActionFactory.Create(faultSensitiveMethod, arg1, arg2, arg3);
        return WithActionAsyncCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive <see cref="Task"/> based function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="setup">The <see cref="AsyncTransientOperationOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="faultSensitiveMethod"/> cannot be null.
    /// </exception>
    /// <exception cref="LatencyException">
    /// The <paramref name="faultSensitiveMethod"/> exceeded the maximum time allowed to network latency.
    /// </exception>
    /// <exception cref="AggregateException">
    /// The <paramref name="faultSensitiveMethod"/> was a victim of a transient fault. The <see cref="AggregateException.InnerExceptions"/> collection contains a <see cref="TransientFaultException"/> object.
    ///
    /// -or
    ///
    /// An exception was thrown during the invocation of the <paramref name="faultSensitiveMethod"/>. The <see cref="AggregateException.InnerExceptions"/> collection contains information about the exception or exceptions.
    /// </exception>
    public static Task WithActionAsync<T1, T2, T3, T4>(Func<T1, T2, T3, T4, CancellationToken, Task> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<AsyncTransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = AsyncActionFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4);
        return WithActionAsyncCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive <see cref="Task"/> based function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg5">The fifth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="setup">The <see cref="AsyncTransientOperationOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="faultSensitiveMethod"/> cannot be null.
    /// </exception>
    /// <exception cref="LatencyException">
    /// The <paramref name="faultSensitiveMethod"/> exceeded the maximum time allowed to network latency.
    /// </exception>
    /// <exception cref="AggregateException">
    /// The <paramref name="faultSensitiveMethod"/> was a victim of a transient fault. The <see cref="AggregateException.InnerExceptions"/> collection contains a <see cref="TransientFaultException"/> object.
    ///
    /// -or
    ///
    /// An exception was thrown during the invocation of the <paramref name="faultSensitiveMethod"/>. The <see cref="AggregateException.InnerExceptions"/> collection contains information about the exception or exceptions.
    /// </exception>
    public static Task WithActionAsync<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, CancellationToken, Task> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<AsyncTransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = AsyncActionFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4, arg5);
        return WithActionAsyncCore(factory, setup);
    }

    private static Task WithActionAsyncCore<TTuple>(AsyncActionFactory<TTuple> factory, Action<AsyncTransientOperationOptions> setup) where TTuple : MutableTuple
    {
        return new AsyncActionTransientWorker(factory.DelegateInfo, factory.GenericArguments.ToArray(), setup).ResilientActionAsync(factory.ExecuteMethodAsync);
    }

    private static Task<TResult> WithFuncAsyncCore<TTuple, TResult>(AsyncFuncFactory<TTuple, TResult> factory, Action<AsyncTransientOperationOptions> setup) where TTuple : MutableTuple
    {
        return new AsyncFuncTransientWorker<TResult>(factory.DelegateInfo, factory.GenericArguments.ToArray(), setup).ResilientFuncAsync(factory.ExecuteMethodAsync);
    }
}
