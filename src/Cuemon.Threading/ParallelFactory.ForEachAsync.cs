using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Collections.Generic;

namespace Cuemon.Threading;
public static partial class ParallelFactory
{
    /// <summary>
    /// Executes a parallel foreach loop.
    /// </summary>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <param name="source">The sequence to iterate over parallel.</param>
    /// <param name="worker">The delegate that is invoked once per iteration.</param>
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static Task ForEachAsync<TSource>(IEnumerable<TSource> source, Func<TSource, CancellationToken, Task> worker, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        return ForEachCoreAsync(source, AsyncActionFactory.Create(worker, default), setup);
    }

    /// <summary>
    /// Executes a parallel foreach loop.
    /// </summary>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <typeparam name="T">The type of the parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <param name="source">The sequence to iterate over parallel.</param>
    /// <param name="worker">The delegate that is invoked once per iteration.</param>
    /// <param name="arg">The parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static Task ForEachAsync<TSource, T>(IEnumerable<TSource> source, Func<TSource, T, CancellationToken, Task> worker, T arg, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        return ForEachCoreAsync(source, AsyncActionFactory.Create(worker, default, arg), setup);
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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static Task ForEachAsync<TSource, T1, T2>(IEnumerable<TSource> source, Func<TSource, T1, T2, CancellationToken, Task> worker, T1 arg1, T2 arg2, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        return ForEachCoreAsync(source, AsyncActionFactory.Create(worker, default, arg1, arg2), setup);
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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static Task ForEachAsync<TSource, T1, T2, T3>(IEnumerable<TSource> source, Func<TSource, T1, T2, T3, CancellationToken, Task> worker, T1 arg1, T2 arg2, T3 arg3, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        return ForEachCoreAsync(source, AsyncActionFactory.Create(worker, default, arg1, arg2, arg3), setup);
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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static Task ForEachAsync<TSource, T1, T2, T3, T4>(IEnumerable<TSource> source, Func<TSource, T1, T2, T3, T4, CancellationToken, Task> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        return ForEachCoreAsync(source, AsyncActionFactory.Create(worker, default, arg1, arg2, arg3, arg4), setup);
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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static Task ForEachAsync<TSource, T1, T2, T3, T4, T5>(IEnumerable<TSource> source, Func<TSource, T1, T2, T3, T4, T5, CancellationToken, Task> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        return ForEachCoreAsync(source, AsyncActionFactory.Create(worker, default, arg1, arg2, arg3, arg4, arg5), setup);
    }

    private static async Task ForEachCoreAsync<TSource, TWorker>(IEnumerable<TSource> source, AsyncActionFactory<TWorker> workerFactory, Action<AsyncWorkloadOptions> setup) where TWorker : MutableTuple<TSource>
    {
        var options = Patterns.Configure(setup);
        var partitioner = new PartitionerEnumerable<TSource>(source, options.PartitionSize);

        while (partitioner.HasPartitions)
        {
            var queue = new List<Task>();
            foreach (var item in partitioner)
            {
                workerFactory.GenericArguments.Arg1 = item;
                queue.Add(workerFactory.ExecuteMethodAsync(options.CancellationToken));
            }
            if (queue.Count == 0) { break; }
            await Task.WhenAll(queue).ConfigureAwait(false);
        }
    }
}
