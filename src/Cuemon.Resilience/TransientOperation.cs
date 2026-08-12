using System;

namespace Cuemon.Resilience;
/// <summary>
/// Provides a set of static methods that enable developers to make their applications more resilient by adding robust transient fault handling logic ideal for temporary condition such as network connectivity issues or service unavailability.
/// </summary>
public static partial class TransientOperation
{
    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
    /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
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
    public static TResult WithFunc<TResult>(Func<TResult> faultSensitiveMethod, Action<TransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = new FuncFactory<MutableTuple, TResult>(_ => faultSensitiveMethod(), new MutableTuple(), faultSensitiveMethod);
        return WithFuncCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="arg">The parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
    /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
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
    public static TResult WithFunc<T, TResult>(Func<T, TResult> faultSensitiveMethod, T arg, Action<TransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = new FuncFactory<MutableTuple<T>, TResult>(tuple => faultSensitiveMethod(tuple.Arg1), new MutableTuple<T>(arg), faultSensitiveMethod);
        return WithFuncCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
    /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
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
    public static TResult WithFunc<T1, T2, TResult>(Func<T1, T2, TResult> faultSensitiveMethod, T1 arg1, T2 arg2, Action<TransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = new FuncFactory<MutableTuple<T1, T2>, TResult>(tuple => faultSensitiveMethod(tuple.Arg1, tuple.Arg2), new MutableTuple<T1, T2>(arg1, arg2), faultSensitiveMethod);
        return WithFuncCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
    /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
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
    public static TResult WithFunc<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, Action<TransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = new FuncFactory<MutableTuple<T1, T2, T3>, TResult>(tuple => faultSensitiveMethod(tuple.Arg1, tuple.Arg2, tuple.Arg3), new MutableTuple<T1, T2, T3>(arg1, arg2, arg3), faultSensitiveMethod);
        return WithFuncCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
    /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
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
    public static TResult WithFunc<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<TransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = new FuncFactory<MutableTuple<T1, T2, T3, T4>, TResult>(tuple => faultSensitiveMethod(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4), new MutableTuple<T1, T2, T3, T4>(arg1, arg2, arg3, arg4), faultSensitiveMethod);
        return WithFuncCore(factory, setup);
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
    /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg5">The fifth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
    /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
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
    public static TResult WithFunc<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<TransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = new FuncFactory<MutableTuple<T1, T2, T3, T4, T5>, TResult>(tuple => faultSensitiveMethod(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5), new MutableTuple<T1, T2, T3, T4, T5>(arg1, arg2, arg3, arg4, arg5), faultSensitiveMethod);
        return WithFuncCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
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
    public static void WithAction(Action faultSensitiveMethod, Action<TransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = new ActionFactory<MutableTuple>(_ => faultSensitiveMethod(), new MutableTuple(), faultSensitiveMethod);
        WithActionCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="arg">The parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
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
    public static void WithAction<T>(Action<T> faultSensitiveMethod, T arg, Action<TransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = new ActionFactory<MutableTuple<T>>(tuple => faultSensitiveMethod(tuple.Arg1), new MutableTuple<T>(arg), faultSensitiveMethod);
        WithActionCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
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
    public static void WithAction<T1, T2>(Action<T1, T2> faultSensitiveMethod, T1 arg1, T2 arg2, Action<TransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = new ActionFactory<MutableTuple<T1, T2>>(tuple => faultSensitiveMethod(tuple.Arg1, tuple.Arg2), new MutableTuple<T1, T2>(arg1, arg2), faultSensitiveMethod);
        WithActionCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
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
    public static void WithAction<T1, T2, T3>(Action<T1, T2, T3> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, Action<TransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = new ActionFactory<MutableTuple<T1, T2, T3>>(tuple => faultSensitiveMethod(tuple.Arg1, tuple.Arg2, tuple.Arg3), new MutableTuple<T1, T2, T3>(arg1, arg2, arg3), faultSensitiveMethod);
        WithActionCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
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
    public static void WithAction<T1, T2, T3, T4>(Action<T1, T2, T3, T4> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<TransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = new ActionFactory<MutableTuple<T1, T2, T3, T4>>(tuple => faultSensitiveMethod(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4), new MutableTuple<T1, T2, T3, T4>(arg1, arg2, arg3, arg4), faultSensitiveMethod);
        WithActionCore(factory, setup);
    }

    /// <summary>
    /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
    /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="arg5">The fifth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
    /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
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
    public static void WithAction<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<TransientOperationOptions> setup = null)
    {
        Validator.ThrowIfNull(faultSensitiveMethod);
        var factory = new ActionFactory<MutableTuple<T1, T2, T3, T4, T5>>(tuple => faultSensitiveMethod(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5), new MutableTuple<T1, T2, T3, T4, T5>(arg1, arg2, arg3, arg4, arg5), faultSensitiveMethod);
        WithActionCore(factory, setup);
    }

    /// <summary>
    /// Gets or sets the callback delegate that is invoked when a transient fault occurs.
    /// </summary>
    /// <value>A callback delegate that is invoked when a transient fault occurs.</value>
    public static Action<TransientFaultEvidence> FaultCallback { get; set; }

    private static void WithActionCore<TTuple>(ActionFactory<TTuple> factory, Action<TransientOperationOptions> setup) where TTuple : MutableTuple
    {
        new ActionTransientWorker(factory.DelegateInfo, factory.GenericArguments.ToArray(), setup).ResilientAction(factory.ExecuteMethod);
    }

    private static TResult WithFuncCore<TTuple, TResult>(FuncFactory<TTuple, TResult> factory, Action<TransientOperationOptions> setup) where TTuple : MutableTuple
    {
        return new FuncTransientWorker<TResult>(factory.DelegateInfo, factory.GenericArguments.ToArray(), setup).ResilientFunc(factory.ExecuteMethod);
    }
}
