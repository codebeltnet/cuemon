using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Threading;
/// <summary>
/// Provides a generic way to support different types of design patterns and practices with small utility methods scoped to <see cref="Task"/>.
/// </summary>
public sealed class AsyncPatterns
{
    private static readonly AsyncPatterns ExtendedPatterns = new();

    /// <summary>
    /// Gets the singleton instance of the AsyncPatterns functionality allowing for extensions methods like: <c>AsyncPatterns.Use.SomeIngeniousMethod()</c>.
    /// </summary>
    /// <value>The singleton instance of the AsyncPatterns functionality.</value>
    public static AsyncPatterns Use { get; } = ExtendedPatterns;

    /// <summary>
    /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
    /// <param name="initializer">The function delegate that initializes an object implementing the <see cref="IDisposable"/> interface.</param>
    /// <param name="tester">The function delegate that is used to ensure that operations performed on <typeparamref name="TResult"/> abides CA2000.</param>
    /// <param name="catcher">The function delegate that will handle any exceptions that might have been thrown by <paramref name="tester"/>.</param>
    /// <param name="ct">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
    public static Task<TResult> SafeInvokeAsync<TResult>(Func<TResult> initializer, Func<TResult, CancellationToken, Task<TResult>> tester, Func<Exception, CancellationToken, Task> catcher = null, CancellationToken ct = default) where TResult : class, IDisposable
    {
        Validator.ThrowIfNull(initializer);
        Validator.ThrowIfNull(tester);
        var f1 = AsyncFuncFactory.Create(tester, default);
        var f2 = AsyncActionFactory.Create(catcher, default);
        return SafeInvokeAsyncCore(f1, initializer, f2, ct);
    }

    /// <summary>
    /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
    /// <param name="initializer">The function delegate that initializes an object implementing the <see cref="IDisposable"/> interface.</param>
    /// <param name="tester">The function delegate that is used to ensure that operations performed on <typeparamref name="TResult"/> abides CA2000.</param>
    /// <param name="arg">The parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
    /// <param name="catcher">The function delegate that will handle any exceptions that might have been thrown by <paramref name="tester"/>.</param>
    /// <param name="ct">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
    public static Task<TResult> SafeInvokeAsync<T, TResult>(Func<TResult> initializer, Func<TResult, T, CancellationToken, Task<TResult>> tester, T arg, Func<Exception, T, CancellationToken, Task> catcher = null, CancellationToken ct = default) where TResult : class, IDisposable
    {
        Validator.ThrowIfNull(initializer);
        Validator.ThrowIfNull(tester);
        var f1 = AsyncFuncFactory.Create(tester, default, arg);
        var f2 = AsyncActionFactory.Create(catcher, default, arg);
        return SafeInvokeAsyncCore(f1, initializer, f2, ct);
    }

    /// <summary>
    /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
    /// <param name="initializer">The function delegate that initializes an object implementing the <see cref="IDisposable"/> interface.</param>
    /// <param name="tester">The function delegate that is used to ensure that operations performed on <typeparamref name="TResult"/> abides CA2000.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
    /// <param name="catcher">The function delegate that will handle any exceptions that might have been thrown by <paramref name="tester"/>.</param>
    /// <param name="ct">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
    public static Task<TResult> SafeInvokeAsync<T1, T2, TResult>(Func<TResult> initializer, Func<TResult, T1, T2, CancellationToken, Task<TResult>> tester, T1 arg1, T2 arg2, Func<Exception, T1, T2, CancellationToken, Task> catcher = null, CancellationToken ct = default) where TResult : class, IDisposable
    {
        Validator.ThrowIfNull(initializer);
        Validator.ThrowIfNull(tester);
        var f1 = AsyncFuncFactory.Create(tester, default, arg1, arg2);
        var f2 = AsyncActionFactory.Create(catcher, default, arg1, arg2);
        return SafeInvokeAsyncCore(f1, initializer, f2, ct);
    }

    /// <summary>
    /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
    /// <param name="initializer">The function delegate that initializes an object implementing the <see cref="IDisposable"/> interface.</param>
    /// <param name="tester">The function delegate that is used to ensure that operations performed on <typeparamref name="TResult"/> abides CA2000.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
    /// <param name="catcher">The function delegate that will handle any exceptions that might have been thrown by <paramref name="tester"/>.</param>
    /// <param name="ct">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
    public static Task<TResult> SafeInvokeAsync<T1, T2, T3, TResult>(Func<TResult> initializer, Func<TResult, T1, T2, T3, CancellationToken, Task<TResult>> tester, T1 arg1, T2 arg2, T3 arg3, Func<Exception, T1, T2, T3, CancellationToken, Task> catcher = null, CancellationToken ct = default) where TResult : class, IDisposable
    {
        Validator.ThrowIfNull(initializer);
        Validator.ThrowIfNull(tester);
        var f1 = AsyncFuncFactory.Create(tester, default, arg1, arg2, arg3);
        var f2 = AsyncActionFactory.Create(catcher, default, arg1, arg2, arg3);
        return SafeInvokeAsyncCore(f1, initializer, f2, ct);
    }

    /// <summary>
    /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
    /// <param name="initializer">The function delegate that initializes an object implementing the <see cref="IDisposable"/> interface.</param>
    /// <param name="tester">The function delegate that is used to ensure that operations performed on <typeparamref name="TResult"/> abides CA2000.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
    /// <param name="catcher">The function delegate that will handle any exceptions that might have been thrown by <paramref name="tester"/>.</param>
    /// <param name="ct">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
    public static Task<TResult> SafeInvokeAsync<T1, T2, T3, T4, TResult>(Func<TResult> initializer, Func<TResult, T1, T2, T3, T4, CancellationToken, Task<TResult>> tester, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Func<Exception, T1, T2, T3, T4, CancellationToken, Task> catcher = null, CancellationToken ct = default) where TResult : class, IDisposable
    {
        Validator.ThrowIfNull(initializer);
        Validator.ThrowIfNull(tester);
        var f1 = AsyncFuncFactory.Create(tester, default, arg1, arg2, arg3, arg4);
        var f2 = AsyncActionFactory.Create(catcher, default, arg1, arg2, arg3, arg4);
        return SafeInvokeAsyncCore(f1, initializer, f2, ct);
    }

    /// <summary>
    /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
    /// <param name="initializer">The function delegate that initializes an object implementing the <see cref="IDisposable"/> interface.</param>
    /// <param name="tester">The function delegate that is used to ensure that operations performed on <typeparamref name="TResult"/> abides CA2000.</param>
    /// <param name="arg1">The first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
    /// <param name="arg2">The second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
    /// <param name="arg3">The third parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
    /// <param name="arg4">The fourth parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
    /// <param name="arg5">The fifth parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
    /// <param name="catcher">The function delegate that will handle any exceptions that might have been thrown by <paramref name="tester"/>.</param>
    /// <param name="ct">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
    public static Task<TResult> SafeInvokeAsync<T1, T2, T3, T4, T5, TResult>(Func<TResult> initializer, Func<TResult, T1, T2, T3, T4, T5, CancellationToken, Task<TResult>> tester, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Func<Exception, T1, T2, T3, T4, T5, CancellationToken, Task> catcher = null, CancellationToken ct = default) where TResult : class, IDisposable
    {
        Validator.ThrowIfNull(initializer);
        Validator.ThrowIfNull(tester);
        var f1 = AsyncFuncFactory.Create(tester, default, arg1, arg2, arg3, arg4, arg5);
        var f2 = AsyncActionFactory.Create(catcher, default, arg1, arg2, arg3, arg4, arg5);
        return SafeInvokeAsyncCore(f1, initializer, f2, ct);
    }

    private static async Task<TResult> SafeInvokeAsyncCore<TTester, TResult, TCatcher>(AsyncFuncFactory<TTester, TResult> testerFactory, Func<TResult> initializer, AsyncActionFactory<TCatcher> catcherFactory, CancellationToken ct)
        where TResult : class, IDisposable
        where TTester : MutableTuple<TResult>
        where TCatcher : MutableTuple<Exception>
    {
        TResult result = null;
        try
        {
            testerFactory.GenericArguments.Arg1 = initializer();
            testerFactory.GenericArguments.Arg1 = await testerFactory.ExecuteMethodAsync(ct).ConfigureAwait(false);
            result = testerFactory.GenericArguments.Arg1;
            testerFactory.GenericArguments.Arg1 = null;
        }
        catch (Exception e)
        {
            if (!catcherFactory.HasDelegate)
            {
                throw;
            }
            else
            {
                catcherFactory.GenericArguments.Arg1 = e;
                await catcherFactory.ExecuteMethodAsync(ct).ConfigureAwait(false);
            }
        }
        finally
        {
            testerFactory.GenericArguments.Arg1?.Dispose();
        }
        return result;
    }
}
