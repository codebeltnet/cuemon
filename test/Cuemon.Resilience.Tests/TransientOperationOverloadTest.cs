using System;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Resilience;

public class TransientOperationOverloadTest : Test
{
    public TransientOperationOverloadTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void WithFunc_ShouldReturnExpectedValues_WhenUsingZeroOneFourAndFiveArgumentOverloads()
    {
        var zero = TransientOperation.WithFunc(() => "zero");
        var one = TransientOperation.WithFunc((string arg1) => arg1, "one");
        var four = TransientOperation.WithFunc((string arg1, string arg2, string arg3, string arg4) => string.Concat(arg1, arg2, arg3, arg4), "one", "two", "three", "four");
        var five = TransientOperation.WithFunc((string arg1, string arg2, string arg3, string arg4, string arg5) => string.Concat(arg1, arg2, arg3, arg4, arg5), "one", "two", "three", "four", "five");

        Assert.Equal("zero", zero);
        Assert.Equal("one", one);
        Assert.Equal("onetwothreefour", four);
        Assert.Equal("onetwothreefourfive", five);
    }

    [Fact]
    public void WithAction_ShouldCaptureExpectedValues_WhenUsingZeroOneFourAndFiveArgumentOverloads()
    {
        var zeroInvoked = false;
        var one = string.Empty;
        var four = string.Empty;
        var five = string.Empty;

        TransientOperation.WithAction(() => zeroInvoked = true);
        TransientOperation.WithAction((string arg1) => one = arg1, "one");
        TransientOperation.WithAction((string arg1, string arg2, string arg3, string arg4) => four = string.Concat(arg1, arg2, arg3, arg4), "one", "two", "three", "four");
        TransientOperation.WithAction((string arg1, string arg2, string arg3, string arg4, string arg5) => five = string.Concat(arg1, arg2, arg3, arg4, arg5), "one", "two", "three", "four", "five");

        Assert.True(zeroInvoked);
        Assert.Equal("one", one);
        Assert.Equal("onetwothreefour", four);
        Assert.Equal("onetwothreefourfive", five);
    }

    [Fact]
    public async Task WithFuncAsync_ShouldReturnExpectedValues_WhenUsingZeroOneFourAndFiveArgumentOverloads()
    {
        var token = new CancellationTokenSource().Token;
        var zeroToken = CancellationToken.None;
        var oneToken = CancellationToken.None;
        var fourToken = CancellationToken.None;
        var fiveToken = CancellationToken.None;
        Action<AsyncTransientOperationOptions> setup = o => o.CancellationToken = token;

        var zero = await TransientOperation.WithFuncAsync(ct =>
        {
            zeroToken = ct;
            return Task.FromResult("zero");
        }, setup);
        var one = await TransientOperation.WithFuncAsync((string arg1, CancellationToken ct) =>
        {
            oneToken = ct;
            return Task.FromResult(arg1);
        }, "one", setup);
        var four = await TransientOperation.WithFuncAsync((string arg1, string arg2, string arg3, string arg4, CancellationToken ct) =>
        {
            fourToken = ct;
            return Task.FromResult(string.Concat(arg1, arg2, arg3, arg4));
        }, "one", "two", "three", "four", setup);
        var five = await TransientOperation.WithFuncAsync((string arg1, string arg2, string arg3, string arg4, string arg5, CancellationToken ct) =>
        {
            fiveToken = ct;
            return Task.FromResult(string.Concat(arg1, arg2, arg3, arg4, arg5));
        }, "one", "two", "three", "four", "five", setup);

        Assert.Equal("zero", zero);
        Assert.Equal("one", one);
        Assert.Equal("onetwothreefour", four);
        Assert.Equal("onetwothreefourfive", five);
        Assert.Equal(token, zeroToken);
        Assert.Equal(token, oneToken);
        Assert.Equal(token, fourToken);
        Assert.Equal(token, fiveToken);
    }

    [Fact]
    public async Task WithActionAsync_ShouldCaptureExpectedValues_WhenUsingZeroOneFourAndFiveArgumentOverloads()
    {
        var token = new CancellationTokenSource().Token;
        var zeroToken = CancellationToken.None;
        var oneToken = CancellationToken.None;
        var fourToken = CancellationToken.None;
        var fiveToken = CancellationToken.None;
        var zeroInvoked = false;
        var one = string.Empty;
        var four = string.Empty;
        var five = string.Empty;
        Action<AsyncTransientOperationOptions> setup = o => o.CancellationToken = token;

        await TransientOperation.WithActionAsync(ct =>
        {
            zeroInvoked = true;
            zeroToken = ct;
            return Task.CompletedTask;
        }, setup);
        await TransientOperation.WithActionAsync((string arg1, CancellationToken ct) =>
        {
            one = arg1;
            oneToken = ct;
            return Task.CompletedTask;
        }, "one", setup);
        await TransientOperation.WithActionAsync((string arg1, string arg2, string arg3, string arg4, CancellationToken ct) =>
        {
            four = string.Concat(arg1, arg2, arg3, arg4);
            fourToken = ct;
            return Task.CompletedTask;
        }, "one", "two", "three", "four", setup);
        await TransientOperation.WithActionAsync((string arg1, string arg2, string arg3, string arg4, string arg5, CancellationToken ct) =>
        {
            five = string.Concat(arg1, arg2, arg3, arg4, arg5);
            fiveToken = ct;
            return Task.CompletedTask;
        }, "one", "two", "three", "four", "five", setup);

        Assert.True(zeroInvoked);
        Assert.Equal("one", one);
        Assert.Equal("onetwothreefour", four);
        Assert.Equal("onetwothreefourfive", five);
        Assert.Equal(token, zeroToken);
        Assert.Equal(token, oneToken);
        Assert.Equal(token, fourToken);
        Assert.Equal(token, fiveToken);
    }
}
