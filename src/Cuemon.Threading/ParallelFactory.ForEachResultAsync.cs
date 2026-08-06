using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Collections.Generic;

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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.</returns>
    public static Task<IReadOnlyCollection<TResult>> ForEachResultAsync<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, CancellationToken, Task<TResult>> worker, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        return ForEachResultCoreAsync(source, AsyncFuncFactory.Create(worker, default), setup);
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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.</returns>
    public static Task<IReadOnlyCollection<TResult>> ForEachResultAsync<TSource, T, TResult>(IEnumerable<TSource> source, Func<TSource, T, CancellationToken, Task<TResult>> worker, T arg, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        return ForEachResultCoreAsync(source, AsyncFuncFactory.Create(worker, default, arg), setup);
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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.</returns>
    public static Task<IReadOnlyCollection<TResult>> ForEachResultAsync<TSource, T1, T2, TResult>(IEnumerable<TSource> source, Func<TSource, T1, T2, CancellationToken, Task<TResult>> worker, T1 arg1, T2 arg2, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        return ForEachResultCoreAsync(source, AsyncFuncFactory.Create(worker, default, arg1, arg2), setup);
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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.</returns>
    public static Task<IReadOnlyCollection<TResult>> ForEachResultAsync<TSource, T1, T2, T3, TResult>(IEnumerable<TSource> source, Func<TSource, T1, T2, T3, CancellationToken, Task<TResult>> worker, T1 arg1, T2 arg2, T3 arg3, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        return ForEachResultCoreAsync(source, AsyncFuncFactory.Create(worker, default, arg1, arg2, arg3), setup);
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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.</returns>
    public static Task<IReadOnlyCollection<TResult>> ForEachResultAsync<TSource, T1, T2, T3, T4, TResult>(IEnumerable<TSource> source, Func<TSource, T1, T2, T3, T4, CancellationToken, Task<TResult>> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        return ForEachResultCoreAsync(source, AsyncFuncFactory.Create(worker, default, arg1, arg2, arg3, arg4), setup);
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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.</returns>
    public static Task<IReadOnlyCollection<TResult>> ForEachResultAsync<TSource, T1, T2, T3, T4, T5, TResult>(IEnumerable<TSource> source, Func<TSource, T1, T2, T3, T4, T5, CancellationToken, Task<TResult>> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<AsyncWorkloadOptions> setup = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(worker);
        return ForEachResultCoreAsync(source, AsyncFuncFactory.Create(worker, default, arg1, arg2, arg3, arg4, arg5), setup);
    }

    private static async Task<IReadOnlyCollection<TResult>> ForEachResultCoreAsync<TSource, TWorker, TResult>(IEnumerable<TSource> source, AsyncFuncFactory<TWorker, TResult> workerFactory, Action<AsyncWorkloadOptions> setup)
        where TWorker : MutableTuple<TSource>
    {
        var options = Patterns.Configure(setup);
        var result = new ConcurrentDictionary<long, TResult>();
        long sorter = 0;

        var partitioner = new PartitionerEnumerable<TSource>(source, options.PartitionSize);
        while (partitioner.HasPartitions)
        {
            var queue = new Dictionary<long, Task<TResult>>();
            foreach (var item in partitioner)
            {
                workerFactory.GenericArguments.Arg1 = item;
                queue.Add(sorter, workerFactory.ExecuteMethodAsync(options.CancellationToken));
                sorter++;
            }
            if (queue.Count == 0) { break; }
            await Task.WhenAll(queue.Values).ConfigureAwait(false);
            foreach (var item in queue) { result.TryAdd(item.Key, item.Value.Result); }
        }
        return new ReadOnlyCollection<TResult>(result.Values.ToList());
    }
}
