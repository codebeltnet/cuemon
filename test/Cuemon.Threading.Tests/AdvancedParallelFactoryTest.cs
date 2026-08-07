using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Threading;
[Trait("Category", "Threading")]
public class AdvancedParallelFactoryTest : Test
{
    private readonly TimeSpan _maxAllowedTestTime = TimeSpan.FromMinutes(1);

    public AdvancedParallelFactoryTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Condition_ShouldEvaluateSupportedOperators_WhenInvoked()
    {
        Assert.True(AdvancedParallelFactory.Condition(8, RelationalOperator.Equal, 8));
        Assert.True(AdvancedParallelFactory.Condition(9, RelationalOperator.GreaterThan, 8));
        Assert.True(AdvancedParallelFactory.Condition(8, RelationalOperator.GreaterThanOrEqual, 8));
        Assert.True(AdvancedParallelFactory.Condition(7, RelationalOperator.LessThan, 8));
        Assert.True(AdvancedParallelFactory.Condition(8, RelationalOperator.LessThanOrEqual, 8));
        Assert.True(AdvancedParallelFactory.Condition(7, RelationalOperator.NotEqual, 8));
        Assert.Equal(10, AdvancedParallelFactory.Iterator(7, AssignmentOperator.Addition, 3));

        Assert.Throws<ArgumentOutOfRangeException>(() => AdvancedParallelFactory.Condition(8, (RelationalOperator)int.MaxValue, 8));
    }

    [Fact]
    public void For_ShouldExecuteAllOverloads_WhenInvoked()
    {
        var count = 6;
        var expected = Enumerable.Range(0, count).ToArray();

        AssertEquivalent(expected, ExecuteFor(count, (rules, bag, setup) => AdvancedParallelFactory.For(rules, i => bag.Add(i), setup)));
        AssertEquivalent(expected.Select(i => i + 10), ExecuteFor(count, (rules, bag, setup) => AdvancedParallelFactory.For(rules, (i, a) => bag.Add(i + a), 10, setup)));
        AssertEquivalent(expected.Select(i => i + 30), ExecuteFor(count, (rules, bag, setup) => AdvancedParallelFactory.For(rules, (i, a, b) => bag.Add(i + a + b), 10, 20, setup)));
        AssertEquivalent(expected.Select(i => i + 60), ExecuteFor(count, (rules, bag, setup) => AdvancedParallelFactory.For(rules, (i, a, b, c) => bag.Add(i + a + b + c), 10, 20, 30, setup)));
        AssertEquivalent(expected.Select(i => i + 100), ExecuteFor(count, (rules, bag, setup) => AdvancedParallelFactory.For(rules, (i, a, b, c, d) => bag.Add(i + a + b + c + d), 10, 20, 30, 40, setup)));
        AssertEquivalent(expected.Select(i => i + 150), ExecuteFor(count, (rules, bag, setup) => AdvancedParallelFactory.For(rules, (i, a, b, c, d, e) => bag.Add(i + a + b + c + d + e), 10, 20, 30, 40, 50, setup)));
    }

    [Fact]
    public void ForResult_ShouldReturnExpectedResults_WhenInvoked()
    {
        var count = 6;
        var expected = Enumerable.Range(0, count).ToArray();

        AssertEquivalent(expected, AdvancedParallelFactory.ForResult(CreateRules(count), i => i, CreateSyncSetup(CancellationToken.None)));
        AssertEquivalent(expected.Select(i => i + 10), AdvancedParallelFactory.ForResult(CreateRules(count), (i, a) => i + a, 10, CreateSyncSetup(CancellationToken.None)));
        AssertEquivalent(expected.Select(i => i + 30), AdvancedParallelFactory.ForResult(CreateRules(count), (i, a, b) => i + a + b, 10, 20, CreateSyncSetup(CancellationToken.None)));
        AssertEquivalent(expected.Select(i => i + 60), AdvancedParallelFactory.ForResult(CreateRules(count), (i, a, b, c) => i + a + b + c, 10, 20, 30, CreateSyncSetup(CancellationToken.None)));
        AssertEquivalent(expected.Select(i => i + 100), AdvancedParallelFactory.ForResult(CreateRules(count), (i, a, b, c, d) => i + a + b + c + d, 10, 20, 30, 40, CreateSyncSetup(CancellationToken.None)));
        AssertEquivalent(expected.Select(i => i + 150), AdvancedParallelFactory.ForResult(CreateRules(count), (i, a, b, c, d, e) => i + a + b + c + d + e, 10, 20, 30, 40, 50, CreateSyncSetup(CancellationToken.None)));
    }

    [Fact]
    public void While_ShouldExecuteAllOverloads_WhenInvoked()
    {
        var count = 6;
        var expected = Enumerable.Range(0, count).ToArray();

        AssertEquivalent(expected, ExecuteWhile(count, (reader, condition, provider, bag, setup) => AdvancedParallelFactory.While(reader, condition, provider, i => bag.Add(i), setup)));
        AssertEquivalent(expected.Select(i => i + 10), ExecuteWhile(count, (reader, condition, provider, bag, setup) => AdvancedParallelFactory.While(reader, condition, provider, (i, a) => bag.Add(i + a), 10, setup)));
        AssertEquivalent(expected.Select(i => i + 30), ExecuteWhile(count, (reader, condition, provider, bag, setup) => AdvancedParallelFactory.While(reader, condition, provider, (i, a, b) => bag.Add(i + a + b), 10, 20, setup)));
        AssertEquivalent(expected.Select(i => i + 60), ExecuteWhile(count, (reader, condition, provider, bag, setup) => AdvancedParallelFactory.While(reader, condition, provider, (i, a, b, c) => bag.Add(i + a + b + c), 10, 20, 30, setup)));
        AssertEquivalent(expected.Select(i => i + 100), ExecuteWhile(count, (reader, condition, provider, bag, setup) => AdvancedParallelFactory.While(reader, condition, provider, (i, a, b, c, d) => bag.Add(i + a + b + c + d), 10, 20, 30, 40, setup)));
        AssertEquivalent(expected.Select(i => i + 150), ExecuteWhile(count, (reader, condition, provider, bag, setup) => AdvancedParallelFactory.While(reader, condition, provider, (i, a, b, c, d, e) => bag.Add(i + a + b + c + d + e), 10, 20, 30, 40, 50, setup)));
    }

    [Fact]
    public void WhileResult_ShouldReturnExpectedResults_WhenInvoked()
    {
        var count = 6;
        var expected = Enumerable.Range(0, count).ToArray();

        AssertEquivalent(expected, ExecuteWhileResult(count, (reader, condition, provider, setup) => AdvancedParallelFactory.WhileResult(reader, condition, provider, i => i, setup)));
        AssertEquivalent(expected.Select(i => i + 10), ExecuteWhileResult(count, (reader, condition, provider, setup) => AdvancedParallelFactory.WhileResult(reader, condition, provider, (i, a) => i + a, 10, setup)));
        AssertEquivalent(expected.Select(i => i + 30), ExecuteWhileResult(count, (reader, condition, provider, setup) => AdvancedParallelFactory.WhileResult(reader, condition, provider, (i, a, b) => i + a + b, 10, 20, setup)));
        AssertEquivalent(expected.Select(i => i + 60), ExecuteWhileResult(count, (reader, condition, provider, setup) => AdvancedParallelFactory.WhileResult(reader, condition, provider, (i, a, b, c) => i + a + b + c, 10, 20, 30, setup)));
        AssertEquivalent(expected.Select(i => i + 100), ExecuteWhileResult(count, (reader, condition, provider, setup) => AdvancedParallelFactory.WhileResult(reader, condition, provider, (i, a, b, c, d) => i + a + b + c + d, 10, 20, 30, 40, setup)));
        AssertEquivalent(expected.Select(i => i + 150), ExecuteWhileResult(count, (reader, condition, provider, setup) => AdvancedParallelFactory.WhileResult(reader, condition, provider, (i, a, b, c, d, e) => i + a + b + c + d + e, 10, 20, 30, 40, 50, setup)));
    }

    [Fact]
    public void For_ShouldRunConcurrently_WhenConfiguredWithMultiplePartitions()
    {
        var ready = new CountdownEvent(3);
        var active = 0;
        var maxActive = 0;

        AdvancedParallelFactory.For(CreateRules(3), i =>
        {
            var current = Interlocked.Increment(ref active);
            CaptureMax(ref maxActive, current);
            ready.Signal();
            SpinWait.SpinUntil(() => ready.IsSet, _maxAllowedTestTime);
            Thread.Sleep(25);
            Interlocked.Decrement(ref active);
        }, CreateSyncSetup(CancellationToken.None, 3));

        Assert.True(maxActive > 1, $"Expected more than one concurrent worker but observed {maxActive}.");
    }

    [Fact]
    public void For_ShouldThrowAggregateException_WhenWorkerFaults()
    {
        var exception = Assert.Throws<AggregateException>(() => AdvancedParallelFactory.For(CreateRules(6), i =>
        {
            if (i == 3) { throw new InvalidOperationException("boom"); }
        }, CreateSyncSetup(CancellationToken.None)));

        Assert.IsType<InvalidOperationException>(Assert.Single(exception.InnerExceptions));
    }

    [Fact]
    public void While_ShouldThrowAggregateException_WhenWorkerFaults()
    {
        var queue = CreateQueue(6);
        var exception = Assert.Throws<AggregateException>(() => AdvancedParallelFactory.While(queue, () => queue.Count > 0, q => q.Dequeue(), i =>
        {
            if (i == 3) { throw new InvalidOperationException("boom"); }
        }, CreateSyncSetup(CancellationToken.None)));

        Assert.IsType<InvalidOperationException>(Assert.Single(exception.InnerExceptions));
    }

    [Fact]
    public void For_ShouldThrowArgumentNullException_WhenWorkerIsNull()
    {
        Action<int> worker = null;

        Assert.Throws<ArgumentNullException>(() => AdvancedParallelFactory.For(CreateRules(1), worker));
    }

    [Fact]
    public void While_ShouldThrowArgumentNullException_WhenConditionIsNull()
    {
        Func<bool> condition = null;
        var queue = CreateQueue(1);

        Assert.Throws<ArgumentNullException>(() => AdvancedParallelFactory.While(queue, condition, q => q.Dequeue(), i => { }));
    }

    [Fact]
    public async Task ForAsync_ShouldExecuteAllOverloads_WhenInvoked()
    {
        var count = 6;
        var expected = Enumerable.Range(0, count).ToArray();

        AssertEquivalent(expected, await ExecuteForAsync(count, (rules, bag, setup) => AdvancedParallelFactory.ForAsync(rules, async (i, ct) =>
        {
            await Task.Yield();
            bag.Add(i);
        }, setup)));
        AssertEquivalent(expected.Select(i => i + 10), await ExecuteForAsync(count, (rules, bag, setup) => AdvancedParallelFactory.ForAsync(rules, async (i, a, ct) =>
        {
            await Task.Yield();
            bag.Add(i + a);
        }, 10, setup)));
        AssertEquivalent(expected.Select(i => i + 30), await ExecuteForAsync(count, (rules, bag, setup) => AdvancedParallelFactory.ForAsync(rules, async (i, a, b, ct) =>
        {
            await Task.Yield();
            bag.Add(i + a + b);
        }, 10, 20, setup)));
        AssertEquivalent(expected.Select(i => i + 60), await ExecuteForAsync(count, (rules, bag, setup) => AdvancedParallelFactory.ForAsync(rules, async (i, a, b, c, ct) =>
        {
            await Task.Yield();
            bag.Add(i + a + b + c);
        }, 10, 20, 30, setup)));
        AssertEquivalent(expected.Select(i => i + 100), await ExecuteForAsync(count, (rules, bag, setup) => AdvancedParallelFactory.ForAsync(rules, async (i, a, b, c, d, ct) =>
        {
            await Task.Yield();
            bag.Add(i + a + b + c + d);
        }, 10, 20, 30, 40, setup)));
        AssertEquivalent(expected.Select(i => i + 150), await ExecuteForAsync(count, (rules, bag, setup) => AdvancedParallelFactory.ForAsync(rules, async (i, a, b, c, d, e, ct) =>
        {
            await Task.Yield();
            bag.Add(i + a + b + c + d + e);
        }, 10, 20, 30, 40, 50, setup)));
    }

    [Fact]
    public async Task ForResultAsync_ShouldReturnExpectedResults_WhenInvoked()
    {
        var count = 6;
        var expected = Enumerable.Range(0, count).ToArray();

        AssertEquivalent(expected, await AdvancedParallelFactory.ForResultAsync(CreateRules(count), async (i, ct) =>
        {
            await Task.Yield();
            return i;
        }, CreateAsyncSetup(CancellationToken.None)));
        AssertEquivalent(expected.Select(i => i + 10), await AdvancedParallelFactory.ForResultAsync(CreateRules(count), async (i, a, ct) =>
        {
            await Task.Yield();
            return i + a;
        }, 10, CreateAsyncSetup(CancellationToken.None)));
        AssertEquivalent(expected.Select(i => i + 30), await AdvancedParallelFactory.ForResultAsync(CreateRules(count), async (i, a, b, ct) =>
        {
            await Task.Yield();
            return i + a + b;
        }, 10, 20, CreateAsyncSetup(CancellationToken.None)));
        AssertEquivalent(expected.Select(i => i + 60), await AdvancedParallelFactory.ForResultAsync(CreateRules(count), async (i, a, b, c, ct) =>
        {
            await Task.Yield();
            return i + a + b + c;
        }, 10, 20, 30, CreateAsyncSetup(CancellationToken.None)));
        AssertEquivalent(expected.Select(i => i + 100), await AdvancedParallelFactory.ForResultAsync(CreateRules(count), async (i, a, b, c, d, ct) =>
        {
            await Task.Yield();
            return i + a + b + c + d;
        }, 10, 20, 30, 40, CreateAsyncSetup(CancellationToken.None)));
        AssertEquivalent(expected.Select(i => i + 150), await AdvancedParallelFactory.ForResultAsync(CreateRules(count), async (i, a, b, c, d, e, ct) =>
        {
            await Task.Yield();
            return i + a + b + c + d + e;
        }, 10, 20, 30, 40, 50, CreateAsyncSetup(CancellationToken.None)));
    }

    [Fact]
    public async Task WhileAsync_ShouldExecuteAllOverloads_WhenInvoked()
    {
        var count = 6;
        var expected = Enumerable.Range(0, count).ToArray();

        AssertEquivalent(expected, await ExecuteWhileAsync(count, (reader, condition, provider, bag, setup) => AdvancedParallelFactory.WhileAsync(reader, condition, provider, async (i, ct) =>
        {
            await Task.Yield();
            bag.Add(i);
        }, setup)));
        AssertEquivalent(expected.Select(i => i + 10), await ExecuteWhileAsync(count, (reader, condition, provider, bag, setup) => AdvancedParallelFactory.WhileAsync(reader, condition, provider, async (i, a, ct) =>
        {
            await Task.Yield();
            bag.Add(i + a);
        }, 10, setup)));
        AssertEquivalent(expected.Select(i => i + 30), await ExecuteWhileAsync(count, (reader, condition, provider, bag, setup) => AdvancedParallelFactory.WhileAsync(reader, condition, provider, async (i, a, b, ct) =>
        {
            await Task.Yield();
            bag.Add(i + a + b);
        }, 10, 20, setup)));
        AssertEquivalent(expected.Select(i => i + 60), await ExecuteWhileAsync(count, (reader, condition, provider, bag, setup) => AdvancedParallelFactory.WhileAsync(reader, condition, provider, async (i, a, b, c, ct) =>
        {
            await Task.Yield();
            bag.Add(i + a + b + c);
        }, 10, 20, 30, setup)));
        AssertEquivalent(expected.Select(i => i + 100), await ExecuteWhileAsync(count, (reader, condition, provider, bag, setup) => AdvancedParallelFactory.WhileAsync(reader, condition, provider, async (i, a, b, c, d, ct) =>
        {
            await Task.Yield();
            bag.Add(i + a + b + c + d);
        }, 10, 20, 30, 40, setup)));
        AssertEquivalent(expected.Select(i => i + 150), await ExecuteWhileAsync(count, (reader, condition, provider, bag, setup) => AdvancedParallelFactory.WhileAsync(reader, condition, provider, async (i, a, b, c, d, e, ct) =>
        {
            await Task.Yield();
            bag.Add(i + a + b + c + d + e);
        }, 10, 20, 30, 40, 50, setup)));
    }

    [Fact]
    public async Task WhileResultAsync_ShouldReturnExpectedResults_WhenInvoked()
    {
        var count = 6;
        var expected = Enumerable.Range(0, count).ToArray();

        AssertEquivalent(expected, await ExecuteWhileResultAsync(count, (reader, condition, provider, setup) => AdvancedParallelFactory.WhileResultAsync(reader, condition, provider, async (i, ct) =>
        {
            await Task.Yield();
            return i;
        }, setup)));
        AssertEquivalent(expected.Select(i => i + 10), await ExecuteWhileResultAsync(count, (reader, condition, provider, setup) => AdvancedParallelFactory.WhileResultAsync(reader, condition, provider, async (i, a, ct) =>
        {
            await Task.Yield();
            return i + a;
        }, 10, setup)));
        AssertEquivalent(expected.Select(i => i + 30), await ExecuteWhileResultAsync(count, (reader, condition, provider, setup) => AdvancedParallelFactory.WhileResultAsync(reader, condition, provider, async (i, a, b, ct) =>
        {
            await Task.Yield();
            return i + a + b;
        }, 10, 20, setup)));
        AssertEquivalent(expected.Select(i => i + 60), await ExecuteWhileResultAsync(count, (reader, condition, provider, setup) => AdvancedParallelFactory.WhileResultAsync(reader, condition, provider, async (i, a, b, c, ct) =>
        {
            await Task.Yield();
            return i + a + b + c;
        }, 10, 20, 30, setup)));
        AssertEquivalent(expected.Select(i => i + 100), await ExecuteWhileResultAsync(count, (reader, condition, provider, setup) => AdvancedParallelFactory.WhileResultAsync(reader, condition, provider, async (i, a, b, c, d, ct) =>
        {
            await Task.Yield();
            return i + a + b + c + d;
        }, 10, 20, 30, 40, setup)));
        AssertEquivalent(expected.Select(i => i + 150), await ExecuteWhileResultAsync(count, (reader, condition, provider, setup) => AdvancedParallelFactory.WhileResultAsync(reader, condition, provider, async (i, a, b, c, d, e, ct) =>
        {
            await Task.Yield();
            return i + a + b + c + d + e;
        }, 10, 20, 30, 40, 50, setup)));
    }

    [Fact]
    public async Task ForAsync_ShouldRunConcurrently_WhenConfiguredWithMultiplePartitions()
    {
        var ready = new CountdownEvent(3);
        var active = 0;
        var maxActive = 0;

        await AdvancedParallelFactory.ForAsync(CreateRules(3), async (i, ct) =>
        {
            var current = Interlocked.Increment(ref active);
            CaptureMax(ref maxActive, current);
            ready.Signal();
            while (!ready.IsSet)
            {
                await Task.Delay(1, ct);
            }
            await Task.Delay(25, ct);
            Interlocked.Decrement(ref active);
        }, CreateAsyncSetup(CancellationToken.None, 3));

        Assert.True(maxActive > 1, $"Expected more than one concurrent worker but observed {maxActive}.");
    }

    [Fact]
    public async Task ForAsync_ShouldThrowInvalidOperationException_WhenWorkerFaults()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() => AdvancedParallelFactory.ForAsync(CreateRules(6), (i, ct) =>
        {
            if (i == 3) { return Task.FromException(new InvalidOperationException("boom")); }
            return Task.CompletedTask;
        }, CreateAsyncSetup(CancellationToken.None)));
    }

    [Fact]
    public async Task WhileAsync_ShouldThrowInvalidOperationException_WhenWorkerFaults()
    {
        var queue = CreateQueue(6);

        await Assert.ThrowsAsync<InvalidOperationException>(() => AdvancedParallelFactory.WhileAsync(queue, () => Task.FromResult(queue.Count > 0), q => q.Dequeue(), (i, ct) =>
        {
            if (i == 3) { return Task.FromException(new InvalidOperationException("boom")); }
            return Task.CompletedTask;
        }, CreateAsyncSetup(CancellationToken.None)));
    }

    [Fact]
    public async Task ForAsync_ShouldThrowArgumentNullException_WhenWorkerIsNull()
    {
        Func<int, CancellationToken, Task> worker = null;

        await Assert.ThrowsAsync<ArgumentNullException>(() => AdvancedParallelFactory.ForAsync(CreateRules(1), worker));
    }

    [Fact]
    public async Task WhileAsync_ShouldThrowArgumentNullException_WhenConditionIsNull()
    {
        Func<Task<bool>> condition = null;
        var queue = CreateQueue(1);

        await Assert.ThrowsAsync<ArgumentNullException>(() => AdvancedParallelFactory.WhileAsync(queue, condition, q => q.Dequeue(), (i, ct) => Task.CompletedTask));
    }

    private IEnumerable<int> ExecuteFor(int count, Action<ForLoopRuleset<int>, ConcurrentBag<int>, Action<AsyncTaskFactoryOptions>> execute)
    {
        var bag = new ConcurrentBag<int>();
        execute(CreateRules(count), bag, CreateSyncSetup(CancellationToken.None));
        return bag;
    }

    private async Task<IEnumerable<int>> ExecuteForAsync(int count, Func<ForLoopRuleset<int>, ConcurrentBag<int>, Action<AsyncWorkloadOptions>, Task> execute)
    {
        var bag = new ConcurrentBag<int>();
        await execute(CreateRules(count), bag, CreateAsyncSetup(CancellationToken.None));
        return bag;
    }

    private IEnumerable<int> ExecuteWhile(int count, Action<Queue<int>, Func<bool>, Func<Queue<int>, int>, ConcurrentBag<int>, Action<AsyncTaskFactoryOptions>> execute)
    {
        var reader = CreateQueue(count);
        var bag = new ConcurrentBag<int>();
        execute(reader, () => reader.Count > 0, q => q.Dequeue(), bag, CreateSyncSetup(CancellationToken.None));
        return bag;
    }

    private async Task<IEnumerable<int>> ExecuteWhileAsync(int count, Func<Queue<int>, Func<Task<bool>>, Func<Queue<int>, int>, ConcurrentBag<int>, Action<AsyncWorkloadOptions>, Task> execute)
    {
        var reader = CreateQueue(count);
        var bag = new ConcurrentBag<int>();
        await execute(reader, () => Task.FromResult(reader.Count > 0), q => q.Dequeue(), bag, CreateAsyncSetup(CancellationToken.None));
        return bag;
    }

    private IEnumerable<int> ExecuteWhileResult(int count, Func<Queue<int>, Func<bool>, Func<Queue<int>, int>, Action<AsyncTaskFactoryOptions>, IReadOnlyCollection<int>> execute)
    {
        var reader = CreateQueue(count);
        return execute(reader, () => reader.Count > 0, q => q.Dequeue(), CreateSyncSetup(CancellationToken.None));
    }

    private async Task<IEnumerable<int>> ExecuteWhileResultAsync(int count, Func<Queue<int>, Func<Task<bool>>, Func<Queue<int>, int>, Action<AsyncWorkloadOptions>, Task<IReadOnlyCollection<int>>> execute)
    {
        var reader = CreateQueue(count);
        return await execute(reader, () => Task.FromResult(reader.Count > 0), q => q.Dequeue(), CreateAsyncSetup(CancellationToken.None));
    }

    private static void AssertEquivalent(IEnumerable<int> expected, IEnumerable<int> actual)
    {
        Assert.Equal(expected.OrderBy(i => i), actual.OrderBy(i => i));
    }

    private static ForLoopRuleset<int> CreateRules(int count)
    {
        return new ForLoopRuleset<int>(0, count, 1);
    }

    private static Queue<int> CreateQueue(int count)
    {
        return new Queue<int>(Enumerable.Range(0, count));
    }

    private Action<AsyncTaskFactoryOptions> CreateSyncSetup(CancellationToken cancellationToken, int partitionSize = 3)
    {
        return o =>
        {
            o.CancellationToken = cancellationToken;
            o.CreationOptions = TaskCreationOptions.None;
            o.PartitionSize = partitionSize;
        };
    }

    private static Action<AsyncWorkloadOptions> CreateAsyncSetup(CancellationToken cancellationToken, int partitionSize = 3)
    {
        return o =>
        {
            o.CancellationToken = cancellationToken;
            o.PartitionSize = partitionSize;
        };
    }

    private static void CaptureMax(ref int target, int candidate)
    {
        while (true)
        {
            var snapshot = target;
            if (snapshot >= candidate) { return; }
            if (Interlocked.CompareExchange(ref target, candidate, snapshot) == snapshot) { return; }
        }
    }
}
