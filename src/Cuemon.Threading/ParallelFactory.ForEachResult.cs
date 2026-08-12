using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cuemon.Threading;
public static partial class ParallelFactory
{
    /// <summary>
    /// Executes a parallel foreach loop where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.
    /// </summary>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
    /// <param name="source">The sequence to iterate over parallel.</param>
    /// <param name="worker">The delegate that is invoked once per iteration.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.</returns>
    public static IReadOnlyCollection<TResult> ForEachResult<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> worker, Action<AsyncTaskFactoryOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        var factory = new FuncFactory<MutableTuple<TSource>, TResult>(tuple => worker(tuple.Arg1), new MutableTuple<TSource>(default), worker);
        return ForEachResultCore(source, factory, setup);
    }

    /// <summary>
    /// Executes a parallel foreach loop where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.
    /// </summary>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
    /// <param name="source">The sequence to iterate over parallel.</param>
    /// <param name="worker">The delegate that is invoked once per iteration.</param>
    /// <param name="arg">The parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    /// <returns>An <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.</returns>
    public static IReadOnlyCollection<TResult> ForEachResult<TSource, T, TResult>(IEnumerable<TSource> source, Func<TSource, T, TResult> worker, T arg, Action<AsyncTaskFactoryOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        var factory = new FuncFactory<MutableTuple<TSource, T>, TResult>(tuple => worker(tuple.Arg1, tuple.Arg2), new MutableTuple<TSource, T>(default, arg), worker);
        return ForEachResultCore(source, factory, setup);
    }

    /// <summary>
    /// Executes a parallel foreach loop where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.
    /// </summary>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
    /// <param name="source">The sequence to iterate over parallel.</param>
    /// <param name="worker">The delegate that is invoked once per iteration.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    /// <returns>An <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.</returns>
    public static IReadOnlyCollection<TResult> ForEachResult<TSource, T1, T2, TResult>(IEnumerable<TSource> source, Func<TSource, T1, T2, TResult> worker, T1 arg1, T2 arg2, Action<AsyncTaskFactoryOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        var factory = new FuncFactory<MutableTuple<TSource, T1, T2>, TResult>(tuple => worker(tuple.Arg1, tuple.Arg2, tuple.Arg3), new MutableTuple<TSource, T1, T2>(default, arg1, arg2), worker);
        return ForEachResultCore(source, factory, setup);
    }

    /// <summary>
    /// Executes a parallel foreach loop where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.
    /// </summary>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
    /// <param name="source">The sequence to iterate over parallel.</param>
    /// <param name="worker">The delegate that is invoked once per iteration.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    /// <returns>An <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.</returns>
    public static IReadOnlyCollection<TResult> ForEachResult<TSource, T1, T2, T3, TResult>(IEnumerable<TSource> source, Func<TSource, T1, T2, T3, TResult> worker, T1 arg1, T2 arg2, T3 arg3, Action<AsyncTaskFactoryOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        var factory = new FuncFactory<MutableTuple<TSource, T1, T2, T3>, TResult>(tuple => worker(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4), new MutableTuple<TSource, T1, T2, T3>(default, arg1, arg2, arg3), worker);
        return ForEachResultCore(source, factory, setup);
    }

    /// <summary>
    /// Executes a parallel foreach loop where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.
    /// </summary>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
    /// <param name="source">The sequence to iterate over parallel.</param>
    /// <param name="worker">The delegate that is invoked once per iteration.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    /// <returns>An <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.</returns>
    public static IReadOnlyCollection<TResult> ForEachResult<TSource, T1, T2, T3, T4, TResult>(IEnumerable<TSource> source, Func<TSource, T1, T2, T3, T4, TResult> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<AsyncTaskFactoryOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        var factory = new FuncFactory<MutableTuple<TSource, T1, T2, T3, T4>, TResult>(tuple => worker(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5), new MutableTuple<TSource, T1, T2, T3, T4>(default, arg1, arg2, arg3, arg4), worker);
        return ForEachResultCore(source, factory, setup);
    }

    /// <summary>
    /// Executes a parallel foreach loop    
    /// </summary>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
    /// <param name="source">The sequence to iterate over parallel.</param>
    /// <param name="worker">The delegate that is invoked once per iteration.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="arg5">The fifth parameter of the function delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions" /> which may be configured.</param>
    /// <returns>An <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.</returns>
    public static IReadOnlyCollection<TResult> ForEachResult<TSource, T1, T2, T3, T4, T5, TResult>(IEnumerable<TSource> source, Func<TSource, T1, T2, T3, T4, T5, TResult> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<AsyncTaskFactoryOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        var factory = new FuncFactory<MutableTuple<TSource, T1, T2, T3, T4, T5>, TResult>(tuple => worker(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6), new MutableTuple<TSource, T1, T2, T3, T4, T5>(default, arg1, arg2, arg3, arg4, arg5), worker);
        return ForEachResultCore(source, factory, setup);
    }

    private static IReadOnlyCollection<TResult> ForEachResultCore<TSource, TWorker, TResult>(IEnumerable<TSource> source, FuncFactory<TWorker, TResult> workerFactory, Action<AsyncTaskFactoryOptions> setup)
        where TWorker : MutableTuple<TSource>
    {
        return new FuncForEachSynchronousLoop<TSource, TResult>(source, setup).GetResult(workerFactory);
    }
}
