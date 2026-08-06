using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Threading;
public static partial class AdvancedParallelFactory
{
    /// <summary>
    /// Executes a parallel for loop that offers control of the loop control variable and loop sections.
    /// </summary>
    /// <typeparam name="TOperand">The type of the number used with the loop control variable.</typeparam>
    /// <param name="rules">The rules of a for-loop control flow statement.</param>
    /// <param name="worker">The <see cref="Task"/> based function delegate that is invoked once per iteration.</param>
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rules"/> cannot be null -or-
    /// <paramref name="worker"/> cannot be null.
    /// </exception>
    public static Task ForAsync<TOperand>(ForLoopRuleset<TOperand> rules, Func<TOperand, CancellationToken, Task> worker, Action<AsyncWorkloadOptions> setup = null)
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        Validator.ThrowIfNull(rules);
        Validator.ThrowIfNull(worker);
        return ForCoreAsync(rules, AsyncActionFactory.Create(worker, default), setup);
    }

    /// <summary>
    /// Executes a parallel for loop that offers control of the loop control variable and loop sections.
    /// </summary>
    /// <typeparam name="TOperand">The type of the number used with the loop control variable.</typeparam>
    /// <typeparam name="T">The type of the parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <param name="rules">The rules of a for-loop control flow statement.</param>
    /// <param name="worker">The <see cref="Task"/> based function delegate that is invoked once per iteration.</param>
    /// <param name="arg">The parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rules"/> cannot be null -or-
    /// <paramref name="worker"/> cannot be null.
    /// </exception>
    public static Task ForAsync<TOperand, T>(ForLoopRuleset<TOperand> rules, Func<TOperand, T, CancellationToken, Task> worker, T arg, Action<AsyncWorkloadOptions> setup = null)
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        Validator.ThrowIfNull(rules);
        Validator.ThrowIfNull(worker);
        return ForCoreAsync(rules, AsyncActionFactory.Create(worker, default, arg), setup);
    }

    /// <summary>
    /// Executes a parallel for loop that offers control of the loop control variable and loop sections.
    /// </summary>
    /// <typeparam name="TOperand">The type of the number used with the loop control variable.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <param name="rules">The rules of a for-loop control flow statement.</param>
    /// <param name="worker">The <see cref="Task"/> based function delegate that is invoked once per iteration.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rules"/> cannot be null -or-
    /// <paramref name="worker"/> cannot be null.
    /// </exception>
    public static Task ForAsync<TOperand, T1, T2>(ForLoopRuleset<TOperand> rules, Func<TOperand, T1, T2, CancellationToken, Task> worker, T1 arg1, T2 arg2, Action<AsyncWorkloadOptions> setup = null)
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        Validator.ThrowIfNull(rules);
        Validator.ThrowIfNull(worker);
        return ForCoreAsync(rules, AsyncActionFactory.Create(worker, default, arg1, arg2), setup);
    }

    /// <summary>
    /// Executes a parallel for loop that offers control of the loop control variable and loop sections.
    /// </summary>
    /// <typeparam name="TOperand">The type of the number used with the loop control variable.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <param name="rules">The rules of a for-loop control flow statement.</param>
    /// <param name="worker">The <see cref="Task"/> based function delegate that is invoked once per iteration.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rules"/> cannot be null -or-
    /// <paramref name="worker"/> cannot be null.
    /// </exception>
    public static Task ForAsync<TOperand, T1, T2, T3>(ForLoopRuleset<TOperand> rules, Func<TOperand, T1, T2, T3, CancellationToken, Task> worker, T1 arg1, T2 arg2, T3 arg3, Action<AsyncWorkloadOptions> setup = null)
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        Validator.ThrowIfNull(rules);
        Validator.ThrowIfNull(worker);
        return ForCoreAsync(rules, AsyncActionFactory.Create(worker, default, arg1, arg2, arg3), setup);
    }

    /// <summary>
    /// Executes a parallel for loop that offers control of the loop control variable and loop sections.
    /// </summary>
    /// <typeparam name="TOperand">The type of the number used with the loop control variable.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <param name="rules">The rules of a for-loop control flow statement.</param>
    /// <param name="worker">The <see cref="Task"/> based function delegate that is invoked once per iteration.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rules"/> cannot be null -or-
    /// <paramref name="worker"/> cannot be null.
    /// </exception>
    public static Task ForAsync<TOperand, T1, T2, T3, T4>(ForLoopRuleset<TOperand> rules, Func<TOperand, T1, T2, T3, T4, CancellationToken, Task> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<AsyncWorkloadOptions> setup = null)
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        Validator.ThrowIfNull(rules);
        Validator.ThrowIfNull(worker);
        return ForCoreAsync(rules, AsyncActionFactory.Create(worker, default, arg1, arg2, arg3, arg4), setup);
    }

    /// <summary>
    /// Executes a parallel for loop that offers control of the loop control variable and loop sections.
    /// </summary>
    /// <typeparam name="TOperand">The type of the number used with the loop control variable.</typeparam>
    /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="worker" />.</typeparam>
    /// <param name="rules">The rules of a for-loop control flow statement.</param>
    /// <param name="worker">The <see cref="Task"/> based function delegate that is invoked once per iteration.</param>
    /// <param name="arg1">The first parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="arg2">The second parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="arg3">The third parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="arg4">The fourth parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="arg5">The fifth parameter of the delegate <paramref name="worker" />.</param>
    /// <param name="setup">The <see cref="AsyncWorkloadOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rules"/> cannot be null -or-
    /// <paramref name="worker"/> cannot be null.
    /// </exception>
    public static Task ForAsync<TOperand, T1, T2, T3, T4, T5>(ForLoopRuleset<TOperand> rules, Func<TOperand, T1, T2, T3, T4, T5, CancellationToken, Task> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<AsyncWorkloadOptions> setup = null)
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        Validator.ThrowIfNull(rules);
        Validator.ThrowIfNull(worker);
        return ForCoreAsync(rules, AsyncActionFactory.Create(worker, default, arg1, arg2, arg3, arg4, arg5), setup);
    }

    private static async Task ForCoreAsync<TWorker, TOperand>(ForLoopRuleset<TOperand> rules, AsyncActionFactory<TWorker> workerFactory, Action<AsyncWorkloadOptions> setup)
        where TWorker : MutableTuple<TOperand>
        where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
    {
        var from = rules.From;
        var options = Patterns.Configure(setup);
        TOperand processed = default;

        while (true)
        {
            var workChunks = options.PartitionSize;
            var queue = new List<Task>();
            for (var i = from; rules.Condition(i, rules.Relation, rules.To); i = rules.Iterator(i, rules.Assignment, rules.Step))
            {
                workerFactory.GenericArguments.Arg1 = i;
                queue.Add(workerFactory.ExecuteMethodAsync(options.CancellationToken));

                processed = i;
                workChunks--;

                if (workChunks == 0) { break; }
            }
            from = Calculator.Calculate(processed, rules.Assignment, rules.Step);
            if (queue.Count == 0) { break; }
            await Task.WhenAll(queue).ConfigureAwait(false);
        }
    }
}
