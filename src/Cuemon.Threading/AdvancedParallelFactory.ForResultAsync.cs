using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{T}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rules"/> cannot be null -or-
    /// <paramref name="worker"/> cannot be null.
    /// </exception>
    public static Task<IReadOnlyCollection<TResult>> ForResultAsync<TOperand, TResult>(ForLoopRuleset<TOperand> rules, Func<TOperand, CancellationToken, Task<TResult>> worker, Action<AsyncWorkloadOptions> setup = null)
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        Validator.ThrowIfNull(rules);
        Validator.ThrowIfNull(worker);
        return ForResultCoreAsync(rules, AsyncFuncFactory.Create(worker, default), setup);
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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rules"/> cannot be null -or-
    /// <paramref name="worker"/> cannot be null.
    /// </exception>
    public static Task<IReadOnlyCollection<TResult>> ForResultAsync<TOperand, T, TResult>(ForLoopRuleset<TOperand> rules, Func<TOperand, T, CancellationToken, Task<TResult>> worker, T arg, Action<AsyncWorkloadOptions> setup = null)
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        Validator.ThrowIfNull(rules);
        Validator.ThrowIfNull(worker);
        return ForResultCoreAsync(rules, AsyncFuncFactory.Create(worker, default, arg), setup);
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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rules"/> cannot be null -or-
    /// <paramref name="worker"/> cannot be null.
    /// </exception>
    public static Task<IReadOnlyCollection<TResult>> ForResultAsync<TOperand, T1, T2, TResult>(ForLoopRuleset<TOperand> rules, Func<TOperand, T1, T2, CancellationToken, Task<TResult>> worker, T1 arg1, T2 arg2, Action<AsyncWorkloadOptions> setup = null)
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        Validator.ThrowIfNull(rules);
        Validator.ThrowIfNull(worker);
        return ForResultCoreAsync(rules, AsyncFuncFactory.Create(worker, default, arg1, arg2), setup);
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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rules"/> cannot be null -or-
    /// <paramref name="worker"/> cannot be null.
    /// </exception>
    public static Task<IReadOnlyCollection<TResult>> ForResultAsync<TOperand, T1, T2, T3, TResult>(ForLoopRuleset<TOperand> rules, Func<TOperand, T1, T2, T3, CancellationToken, Task<TResult>> worker, T1 arg1, T2 arg2, T3 arg3, Action<AsyncWorkloadOptions> setup = null)
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        Validator.ThrowIfNull(rules);
        Validator.ThrowIfNull(worker);
        return ForResultCoreAsync(rules, AsyncFuncFactory.Create(worker, default, arg1, arg2, arg3), setup);
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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rules"/> cannot be null -or-
    /// <paramref name="worker"/> cannot be null.
    /// </exception>
    public static Task<IReadOnlyCollection<TResult>> ForResultAsync<TOperand, T1, T2, T3, T4, TResult>(ForLoopRuleset<TOperand> rules, Func<TOperand, T1, T2, T3, T4, CancellationToken, Task<TResult>> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<AsyncWorkloadOptions> setup = null)
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        Validator.ThrowIfNull(rules);
        Validator.ThrowIfNull(worker);
        return ForResultCoreAsync(rules, AsyncFuncFactory.Create(worker, default, arg1, arg2, arg3, arg4), setup);
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
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
    /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rules"/> cannot be null -or-
    /// <paramref name="worker"/> cannot be null.
    /// </exception>
    public static Task<IReadOnlyCollection<TResult>> ForResultAsync<TOperand, T1, T2, T3, T4, T5, TResult>(ForLoopRuleset<TOperand> rules, Func<TOperand, T1, T2, T3, T4, T5, CancellationToken, Task<TResult>> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<AsyncWorkloadOptions> setup = null)
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        Validator.ThrowIfNull(rules);
        Validator.ThrowIfNull(worker);
        return ForResultCoreAsync(rules, AsyncFuncFactory.Create(worker, default, arg1, arg2, arg3, arg4, arg5), setup);
    }

    private static async Task<IReadOnlyCollection<TResult>> ForResultCoreAsync<TWorker, TOperand, TResult>(ForLoopRuleset<TOperand> rules, AsyncFuncFactory<TWorker, TResult> workerFactory, Action<AsyncWorkloadOptions> setup)
        where TWorker : MutableTuple<TOperand>
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        var from = rules.From;
        var options = Patterns.Configure(setup);
        var result = new ConcurrentDictionary<TOperand, TResult>();
        TOperand processed = default;

        while (true)
        {
            var workChunks = options.PartitionSize;
            var queue = new Dictionary<TOperand, Task<TResult>>();
            for (var i = from; rules.Condition(i, rules.Relation, rules.To); i = rules.Iterator(i, rules.Assignment, rules.Step))
            {
                workerFactory.GenericArguments.Arg1 = i;
                queue.Add(i, workerFactory.ExecuteMethodAsync(options.CancellationToken));

                processed = i;
                workChunks--;

                if (workChunks == 0) { break; }
            }
            from = Calculator.Calculate(processed, rules.Assignment, rules.Step);
            if (queue.Count == 0) { break; }
            await Task.WhenAll(queue.Values).ConfigureAwait(false);
            foreach (var item in queue) { result.TryAdd(item.Key, item.Value.Result); }
        }
        return new ReadOnlyCollection<TResult>(result.Values.ToList());
    }
}
