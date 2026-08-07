using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cuemon.Threading;
public static partial class AdvancedParallelFactory
{

    /// <summary>
    /// Executes a parallel for loop that offers control of the loop control variable and loop sections where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.
    /// </summary>
    /// <typeparam name="TOperand">The type of the number used with the loop control variable.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
    /// <param name="rules">The rules of a for-loop control flow statement.</param>
    /// <param name="worker">The delegate that is invoked once per iteration.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{T}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rules"/> cannot be null -or-
    /// <paramref name="worker"/> cannot be null.
    /// </exception>
    public static IReadOnlyCollection<TResult> ForResult<TOperand, TResult>(ForLoopRuleset<TOperand> rules, Func<TOperand, TResult> worker, Action<AsyncTaskFactoryOptions> setup = null)
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        Validator.ThrowIfNull(rules);
        Validator.ThrowIfNull(worker);
        var factory = new FuncFactory<MutableTuple<TOperand>, TResult>(tuple => worker(tuple.Arg1), new MutableTuple<TOperand>(default), worker);
        return ForResultCore(rules, factory, setup);
    }

    /// <summary>
    /// Executes a parallel for loop that offers control of the loop control variable and loop sections where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.
    /// </summary>
    /// <typeparam name="TOperand">The type of the number used with the loop control variable.</typeparam>
    /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
    /// <param name="rules">The rules of a for-loop control flow statement.</param>
    /// <param name="worker">The delegate that is invoked once per iteration.</param>
    /// <param name="arg">The parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rules"/> cannot be null -or-
    /// <paramref name="worker"/> cannot be null.
    /// </exception>
    public static IReadOnlyCollection<TResult> ForResult<TOperand, T, TResult>(ForLoopRuleset<TOperand> rules, Func<TOperand, T, TResult> worker, T arg, Action<AsyncTaskFactoryOptions> setup = null)
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        Validator.ThrowIfNull(rules);
        Validator.ThrowIfNull(worker);
        var factory = new FuncFactory<MutableTuple<TOperand, T>, TResult>(tuple => worker(tuple.Arg1, tuple.Arg2), new MutableTuple<TOperand, T>(default, arg), worker);
        return ForResultCore(rules, factory, setup);
    }

    /// <summary>
    /// Executes a parallel for loop that offers control of the loop control variable and loop sections where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.
    /// </summary>
    /// <typeparam name="TOperand">The type of the number used with the loop control variable.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
    /// <param name="rules">The rules of a for-loop control flow statement.</param>
    /// <param name="worker">The delegate that is invoked once per iteration.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rules"/> cannot be null -or-
    /// <paramref name="worker"/> cannot be null.
    /// </exception>
    public static IReadOnlyCollection<TResult> ForResult<TOperand, T1, T2, TResult>(ForLoopRuleset<TOperand> rules, Func<TOperand, T1, T2, TResult> worker, T1 arg1, T2 arg2, Action<AsyncTaskFactoryOptions> setup = null)
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        Validator.ThrowIfNull(rules);
        Validator.ThrowIfNull(worker);
        var factory = new FuncFactory<MutableTuple<TOperand, T1, T2>, TResult>(tuple => worker(tuple.Arg1, tuple.Arg2, tuple.Arg3), new MutableTuple<TOperand, T1, T2>(default, arg1, arg2), worker);
        return ForResultCore(rules, factory, setup);
    }

    /// <summary>
    /// Executes a parallel for loop that offers control of the loop control variable and loop sections where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.
    /// </summary>
    /// <typeparam name="TOperand">The type of the number used with the loop control variable.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
    /// <param name="rules">The rules of a for-loop control flow statement.</param>
    /// <param name="worker">The delegate that is invoked once per iteration.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rules"/> cannot be null -or-
    /// <paramref name="worker"/> cannot be null.
    /// </exception>
    public static IReadOnlyCollection<TResult> ForResult<TOperand, T1, T2, T3, TResult>(ForLoopRuleset<TOperand> rules, Func<TOperand, T1, T2, T3, TResult> worker, T1 arg1, T2 arg2, T3 arg3, Action<AsyncTaskFactoryOptions> setup = null)
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        Validator.ThrowIfNull(rules);
        Validator.ThrowIfNull(worker);
        var factory = new FuncFactory<MutableTuple<TOperand, T1, T2, T3>, TResult>(tuple => worker(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4), new MutableTuple<TOperand, T1, T2, T3>(default, arg1, arg2, arg3), worker);
        return ForResultCore(rules, factory, setup);
    }

    /// <summary>
    /// Executes a parallel for loop that offers control of the loop control variable and loop sections where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.
    /// </summary>
    /// <typeparam name="TOperand">The type of the number used with the loop control variable.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
    /// <param name="rules">The rules of a for-loop control flow statement.</param>
    /// <param name="worker">The delegate that is invoked once per iteration.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rules"/> cannot be null -or-
    /// <paramref name="worker"/> cannot be null.
    /// </exception>
    public static IReadOnlyCollection<TResult> ForResult<TOperand, T1, T2, T3, T4, TResult>(ForLoopRuleset<TOperand> rules, Func<TOperand, T1, T2, T3, T4, TResult> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<AsyncTaskFactoryOptions> setup = null)
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        Validator.ThrowIfNull(rules);
        Validator.ThrowIfNull(worker);
        var factory = new FuncFactory<MutableTuple<TOperand, T1, T2, T3, T4>, TResult>(tuple => worker(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5), new MutableTuple<TOperand, T1, T2, T3, T4>(default, arg1, arg2, arg3, arg4), worker);
        return ForResultCore(rules, factory, setup);
    }

    /// <summary>
    /// Executes a parallel for loop that offers control of the loop control variable and loop sections where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.
    /// </summary>
    /// <typeparam name="TOperand">The type of the number used with the loop control variable.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
    /// <param name="rules">The rules of a for-loop control flow statement.</param>
    /// <param name="worker">The delegate that is invoked once per iteration.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="arg5">The fifth parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rules"/> cannot be null -or-
    /// <paramref name="worker"/> cannot be null.
    /// </exception>
    public static IReadOnlyCollection<TResult> ForResult<TOperand, T1, T2, T3, T4, T5, TResult>(ForLoopRuleset<TOperand> rules, Func<TOperand, T1, T2, T3, T4, T5, TResult> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<AsyncTaskFactoryOptions> setup = null)
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        Validator.ThrowIfNull(rules);
        Validator.ThrowIfNull(worker);
        var factory = new FuncFactory<MutableTuple<TOperand, T1, T2, T3, T4, T5>, TResult>(tuple => worker(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6), new MutableTuple<TOperand, T1, T2, T3, T4, T5>(default, arg1, arg2, arg3, arg4, arg5), worker);
        return ForResultCore(rules, factory, setup);
    }

    private static IReadOnlyCollection<TResult> ForResultCore<TWorker, TOperand, TResult>(ForLoopRuleset<TOperand> rules, FuncFactory<TWorker, TResult> workerFactory, Action<AsyncTaskFactoryOptions> setup)
        where TWorker : MutableTuple<TOperand>
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        return new FuncForSynchronousLoop<TOperand, TResult>(rules, setup).GetResult(workerFactory);
    }
}
