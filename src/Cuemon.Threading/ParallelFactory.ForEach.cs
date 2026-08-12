using System;
using System.Collections.Generic;

namespace Cuemon.Threading;
public static partial class ParallelFactory
{
    /// <summary>
    /// Executes a parallel foreach loop.
    /// </summary>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <param name="source">The sequence to iterate over parallel.</param>
    /// <param name="worker">The delegate that is invoked once per iteration.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    public static void ForEach<TSource>(IEnumerable<TSource> source, Action<TSource> worker, Action<AsyncTaskFactoryOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        var factory = new ActionFactory<MutableTuple<TSource>>(tuple => worker(tuple.Arg1), new MutableTuple<TSource>(default), worker);
        ForEachCore(source, factory, setup);
    }

    /// <summary>
    /// Executes a parallel foreach loop.
    /// </summary>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <typeparam name="T">The type of the parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <param name="source">The sequence to iterate over parallel.</param>
    /// <param name="worker">The delegate that is invoked once per iteration.</param>
    /// <param name="arg">The parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    public static void ForEach<TSource, T>(IEnumerable<TSource> source, Action<TSource, T> worker, T arg, Action<AsyncTaskFactoryOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        var factory = new ActionFactory<MutableTuple<TSource, T>>(tuple => worker(tuple.Arg1, tuple.Arg2), new MutableTuple<TSource, T>(default, arg), worker);
        ForEachCore(source, factory, setup);
    }

    /// <summary>
    /// Executes a parallel foreach loop.
    /// </summary>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <param name="source">The sequence to iterate over parallel.</param>
    /// <param name="worker">The delegate that is invoked once per iteration.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    public static void ForEach<TSource, T1, T2>(IEnumerable<TSource> source, Action<TSource, T1, T2> worker, T1 arg1, T2 arg2, Action<AsyncTaskFactoryOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        var factory = new ActionFactory<MutableTuple<TSource, T1, T2>>(tuple => worker(tuple.Arg1, tuple.Arg2, tuple.Arg3), new MutableTuple<TSource, T1, T2>(default, arg1, arg2), worker);
        ForEachCore(source, factory, setup);
    }

    /// <summary>
    /// Executes a parallel foreach loop.
    /// </summary>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <param name="source">The sequence to iterate over parallel.</param>
    /// <param name="worker">The delegate that is invoked once per iteration.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    public static void ForEach<TSource, T1, T2, T3>(IEnumerable<TSource> source, Action<TSource, T1, T2, T3> worker, T1 arg1, T2 arg2, T3 arg3, Action<AsyncTaskFactoryOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        var factory = new ActionFactory<MutableTuple<TSource, T1, T2, T3>>(tuple => worker(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4), new MutableTuple<TSource, T1, T2, T3>(default, arg1, arg2, arg3), worker);
        ForEachCore(source, factory, setup);
    }

    /// <summary>
    /// Executes a parallel foreach loop.
    /// </summary>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <param name="source">The sequence to iterate over parallel.</param>
    /// <param name="worker">The delegate that is invoked once per iteration.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    public static void ForEach<TSource, T1, T2, T3, T4>(IEnumerable<TSource> source, Action<TSource, T1, T2, T3, T4> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<AsyncTaskFactoryOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        var factory = new ActionFactory<MutableTuple<TSource, T1, T2, T3, T4>>(tuple => worker(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5), new MutableTuple<TSource, T1, T2, T3, T4>(default, arg1, arg2, arg3, arg4), worker);
        ForEachCore(source, factory, setup);
    }

    /// <summary>
    /// Executes a parallel foreach loop.
    /// </summary>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <param name="source">The sequence to iterate over parallel.</param>
    /// <param name="worker">The delegate that is invoked once per iteration.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="arg5">The fifth parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
    public static void ForEach<TSource, T1, T2, T3, T4, T5>(IEnumerable<TSource> source, Action<TSource, T1, T2, T3, T4, T5> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<AsyncTaskFactoryOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        var factory = new ActionFactory<MutableTuple<TSource, T1, T2, T3, T4, T5>>(tuple => worker(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6), new MutableTuple<TSource, T1, T2, T3, T4, T5>(default, arg1, arg2, arg3, arg4, arg5), worker);
        ForEachCore(source, factory, setup);
    }

    private static void ForEachCore<TSource, TWorker>(IEnumerable<TSource> source, ActionFactory<TWorker> workerFactory, Action<AsyncTaskFactoryOptions> setup) where TWorker : MutableTuple<TSource>
    {
        new ActionForEachSynchronousLoop<TSource>(source, setup).PrepareExecution(workerFactory);
    }
}
