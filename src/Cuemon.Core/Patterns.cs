using System;
using System.Linq;
using System.Runtime.InteropServices;
using Cuemon.Configuration;

namespace Cuemon
{
    /// <summary>
    /// Provides a generic way to support different types of design patterns and practices with small utility methods.
    /// </summary>
    public sealed class Patterns
    {
        private static readonly Patterns ExtendedPatterns = new();

        /// <summary>
        /// Gets the singleton instance of the Patterns functionality allowing for extensions methods like: <c>Patterns.Use.SomeIngeniousMethod()</c>.
        /// </summary>
        /// <value>The singleton instance of the Patterns functionality.</value>
        public static Patterns Use { get; } = ExtendedPatterns;

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="method"/> can be invoked without an exception.
        /// </summary>
        /// <param name="method">The delegate that to invoke.</param>
        /// <returns><c>true</c> if <paramref name="method"/> was called without raising an exception; otherwise <c>false</c>.</returns>
        /// <remarks>Actually an anti-pattern in regards to swallowing exception. That said, there are situations where this is a perfectly valid approach.</remarks>
        public static bool TryInvoke(Action method)
        {
            try
            {
                Validator.ThrowIfNull(method);
                method();
                return true;
            }
            catch (Exception ex)
            {
                if (ex is OutOfMemoryException ||
                    ex is StackOverflowException ||
                    ex is SEHException ||
                    ex is AccessViolationException ||
#pragma warning disable CS0618 // Type or member is obsolete
                    ex is ExecutionEngineException) // fatal exceptions; re-throw for .NET "legacy" (.NET Core will handle these by a high-level catch-all handler)
#pragma warning restore CS0618 // Type or member is obsolete
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="method"/> can be invoked without an exception.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that will resolve <paramref name="result"/>.</param>
        /// <param name="result">When this method returns, contains the value returned from <paramref name="method"/>; otherwise the default value for the type of the <paramref name="result"/> parameter if an exception is thrown.</param>
        /// <returns><c>true</c> if an instance of <typeparamref name="TResult"/> has been created; otherwise <c>false</c>.</returns>
        /// <remarks>Often referred to as the Try-Parse pattern: https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/exceptions-and-performance</remarks>
        public static bool TryInvoke<TResult>(Func<TResult> method, out TResult result)
        {
            try
            {
                Validator.ThrowIfNull(method);
                result = method();
                return true;
            }
            catch (Exception ex)
            {
                if (ex is OutOfMemoryException ||
                    ex is StackOverflowException ||
                    ex is SEHException ||
                    ex is AccessViolationException ||
#pragma warning disable CS0618 // Type or member is obsolete
                    ex is ExecutionEngineException) // fatal exceptions; re-throw for .NET "legacy" (.NET Core will handle these by a high-level catch-all handler)
#pragma warning restore CS0618 // Type or member is obsolete
                {
                    throw;
                }
                result = default;
                return false;
            }
        }

        /// <summary>
        /// Returns an object of <typeparamref name="TResult"/>, or a default value if the specified <paramref name="method"/> throws an exception.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that will return an instance of <typeparamref name="TResult"/>.</param>
        /// <param name="fallbackResult">The value to return when the specified <paramref name="method"/> throws an exception. Default is <c>default</c> of <typeparamref name="TResult"/>.</param>
        /// <returns>An object of <typeparamref name="TResult"/> when the specified <paramref name="method"/> can be invoked without an exception; otherwise <paramref name="fallbackResult"/> is returned.</returns>
        public static TResult InvokeOrDefault<TResult>(Func<TResult> method, TResult fallbackResult = default)
        {
            return TryInvoke(method, out var result) ? result : fallbackResult;
        }


        /// <summary>
        /// Provides a generic way to initialize the default, parameterless constructed instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the class having a default constructor.</typeparam>
        /// <param name="factory">The delegate that will initialize the public write properties of <typeparamref name="T"/>.</param>
        /// <returns>A default constructed instance of <typeparamref name="T"/> initialized with <paramref name="factory"/>.</returns>
        public static T CreateInstance<T>(Action<T> factory) where T : class, new()
        {
            var options = new T();
            factory?.Invoke(options);
            return options;
        }

        /// <summary>
        /// Returns the default parameter-less constructed instance of <typeparamref name="TOptions"/> configured with <paramref name="setup"/> delegate.
        /// </summary>
        /// <typeparam name="TOptions">The type of the configuration options class having a default constructor.</typeparam>
        /// <param name="setup">The delegate that will configure the public read-write properties of <typeparamref name="TOptions"/>.</param>
        /// <param name="initializer">The optional delegate that will initialize the default parameter-less constructed instance of <typeparamref name="TOptions"/>. Should only be used with third party libraries or for validation purposes.</param>
        /// <param name="validator">The optional delegate that will validate the <typeparamref name="TOptions"/> configured by the <paramref name="setup"/> delegate.</param>
        /// <returns>A default constructed instance of <typeparamref name="TOptions"/> initialized with the options of <paramref name="setup"/>.</returns>
        /// <remarks>Often referred to as part the Options pattern: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options</remarks>
        public static TOptions Configure<TOptions>(Action<TOptions> setup, Action<TOptions> initializer = null, Action<TOptions> validator = null) where TOptions : class, IParameterObject, new()
        {
            var options = new TOptions();
            initializer?.Invoke(options);
            setup?.Invoke(options);
            validator?.Invoke(options);
            return options;
        }

        /// <summary>
        /// Returns the delegate that will configure the public read-write properties of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the configuration options class having a default constructor.</typeparam>
        /// <typeparam name="TResult">The type of the configuration options class having a default constructor.</typeparam>
        /// <param name="setup">The delegate that will configure the public read-write properties of <typeparamref name="TSource"/>.</param>
        /// <param name="initializer">The delegate that will exchange the parameter of <paramref name="setup"/> from <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <returns>An <see cref="Action{TExchangeOptions}"/> otherwise equivalent to <paramref name="setup"/>.</returns>
        public static Action<TResult> ConfigureExchange<TSource, TResult>(Action<TSource> setup, Action<TSource, TResult> initializer = null)
            where TSource : class, IParameterObject, new()
            where TResult : class, new()
        {
            var io = Configure(setup);
            initializer ??= (i, o) =>
            {
                var match = false;
                var typeOfInput = typeof(TSource);
                var typeOfOutput = typeof(TResult);
                var ips = typeOfInput.GetProperties().Where(pi => pi.CanRead && pi.CanWrite).ToList();
                var ops = typeOfOutput.GetProperties().Where(pi => pi.CanRead && pi.CanWrite).ToList();
                foreach (var ip in ips)
                {
                    var op = ops.SingleOrDefault(opi => opi.Name == ip.Name && opi.PropertyType == ip.PropertyType);
                    if (op != null)
                    {
                        op.SetValue(o, ip.GetValue(i));
                        match = true;
                    }
                }

                if (!match)
                {
                    throw new InvalidOperationException(FormattableString.Invariant($"Unable to use default converter for exchange of {nameof(TSource)} ({DelimitedString.Create(ips)}) with {nameof(TResult)} ({DelimitedString.Create(ops)}); no match on public read-write properties."));
                }
            };
            return oo => initializer(io, oo);
        }

        /// <summary>
        /// Returns a delegate that will be initialized by <paramref name="initializer"/> with the values from <paramref name="options"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the configuration options having a default constructor.</typeparam>
        /// <typeparam name="TResult">The type of the configuration options having a default constructor.</typeparam>
        /// <param name="options">The configured options to apply an instance of <typeparamref name="TResult"/>.</param>
        /// <param name="initializer">The delegate that will initialize a default instance of <typeparamref name="TResult"/> with the values from <paramref name="options"/>.</param>
        /// <returns>An <see cref="Action{TOptions}"/> with the values from <paramref name="options"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="options"/> cannot be null -or-
        /// <paramref name="initializer"/> cannot be null.
        /// </exception>
        public static Action<TResult> ConfigureRevertExchange<TSource, TResult>(TSource options, Action<TSource, TResult> initializer = null)
            where TSource : class, IParameterObject, new()
            where TResult : class, new()
        {
            return ConfigureExchange(ConfigureRevert(options), initializer);
        }

        /// <summary>
        /// Returns the delegate that will configure the public read-write properties of <typeparamref name="TOptions"/>.
        /// </summary>
        /// <typeparam name="TOptions">The type of the configuration options having a default constructor.</typeparam>
        /// <param name="options">An instance of the configured options.</param>
        /// <returns>An <see cref="Action{TOptions}"/> otherwise equivalent to <paramref name="options"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        public static Action<TOptions> ConfigureRevert<TOptions>(TOptions options) where TOptions : class, new()
        {
            Validator.ThrowIfNull(options);
            return o =>
            {
                var to = typeof(TOptions);
                var tops = to.GetProperties().Where(pi => pi.CanRead && pi.CanWrite).ToList();
                foreach (var p in tops) { p.SetValue(o, p.GetValue(options)); }
            };
        }

        /// <summary>
        /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <param name="initializer">The function delegate that initializes an object implementing the <see cref="IDisposable"/> interface.</param>
        /// <param name="tester">The function delegate that is used to ensure that operations performed on <typeparamref name="TResult"/> abides CA2000.</param>
        /// <param name="catcher">The delegate that will handle any exceptions that might have been thrown by <paramref name="tester"/>.</param>
        /// <returns>The return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
        public static TResult SafeInvoke<TResult>(Func<TResult> initializer, Func<TResult, TResult> tester, Action<Exception> catcher = null) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer);
            Validator.ThrowIfNull(tester);
            var f1 = new FuncFactory<MutableTuple<TResult>, TResult>(tuple => tester(tuple.Arg1), new MutableTuple<TResult>(default), tester);
            var f2 = new ActionFactory<MutableTuple<Exception>>(tuple => catcher?.Invoke(tuple.Arg1), new MutableTuple<Exception>(default), catcher);
            return SafeInvokeCore(f1, initializer, f2);
        }

        /// <summary>
        /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <param name="initializer">The function delegate that initializes an object implementing the <see cref="IDisposable"/> interface.</param>
        /// <param name="tester">The function delegate that is used to ensure that operations performed on <typeparamref name="TResult"/> abides CA2000.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="catcher">The delegate that will handle any exceptions that might have been thrown by <paramref name="tester"/>.</param>
        /// <returns>The return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
        public static TResult SafeInvoke<T, TResult>(Func<TResult> initializer, Func<TResult, T, TResult> tester, T arg, Action<Exception, T> catcher = null) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer);
            Validator.ThrowIfNull(tester);
            var f1 = new FuncFactory<MutableTuple<TResult, T>, TResult>(tuple => tester(tuple.Arg1, tuple.Arg2), new MutableTuple<TResult, T>(default, arg), tester);
            var f2 = new ActionFactory<MutableTuple<Exception, T>>(tuple => catcher?.Invoke(tuple.Arg1, tuple.Arg2), new MutableTuple<Exception, T>(default, arg), catcher);
            return SafeInvokeCore(f1, initializer, f2);
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
        /// <param name="catcher">The delegate that will handle any exceptions that might have been thrown by <paramref name="tester"/>.</param>
        /// <returns>The return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
        public static TResult SafeInvoke<T1, T2, TResult>(Func<TResult> initializer, Func<TResult, T1, T2, TResult> tester, T1 arg1, T2 arg2, Action<Exception, T1, T2> catcher = null) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer);
            Validator.ThrowIfNull(tester);
            var f1 = new FuncFactory<MutableTuple<TResult, T1, T2>, TResult>(tuple => tester(tuple.Arg1, tuple.Arg2, tuple.Arg3), new MutableTuple<TResult, T1, T2>(default, arg1, arg2), tester);
            var f2 = new ActionFactory<MutableTuple<Exception, T1, T2>>(tuple => catcher?.Invoke(tuple.Arg1, tuple.Arg2, tuple.Arg3), new MutableTuple<Exception, T1, T2>(default, arg1, arg2), catcher);
            return SafeInvokeCore(f1, initializer, f2);
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
        /// <param name="catcher">The delegate that will handle any exceptions that might have been thrown by <paramref name="tester"/>.</param>
        /// <returns>The return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
        public static TResult SafeInvoke<T1, T2, T3, TResult>(Func<TResult> initializer, Func<TResult, T1, T2, T3, TResult> tester, T1 arg1, T2 arg2, T3 arg3, Action<Exception, T1, T2, T3> catcher = null) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer);
            Validator.ThrowIfNull(tester);
            var f1 = new FuncFactory<MutableTuple<TResult, T1, T2, T3>, TResult>(tuple => tester(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4), new MutableTuple<TResult, T1, T2, T3>(default, arg1, arg2, arg3), tester);
            var f2 = new ActionFactory<MutableTuple<Exception, T1, T2, T3>>(tuple => catcher?.Invoke(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4), new MutableTuple<Exception, T1, T2, T3>(default, arg1, arg2, arg3), catcher);
            return SafeInvokeCore(f1, initializer, f2);
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
        /// <param name="catcher">The delegate that will handle any exceptions that might have been thrown by <paramref name="tester"/>.</param>
        /// <returns>The return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
        public static TResult SafeInvoke<T1, T2, T3, T4, TResult>(Func<TResult> initializer, Func<TResult, T1, T2, T3, T4, TResult> tester, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<Exception, T1, T2, T3, T4> catcher = null) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer);
            Validator.ThrowIfNull(tester);
            var f1 = new FuncFactory<MutableTuple<TResult, T1, T2, T3, T4>, TResult>(tuple => tester(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5), new MutableTuple<TResult, T1, T2, T3, T4>(default, arg1, arg2, arg3, arg4), tester);
            var f2 = new ActionFactory<MutableTuple<Exception, T1, T2, T3, T4>>(tuple => catcher?.Invoke(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5), new MutableTuple<Exception, T1, T2, T3, T4>(default, arg1, arg2, arg3, arg4), catcher);
            return SafeInvokeCore(f1, initializer, f2);
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
        /// <param name="catcher">The delegate that will handle any exceptions that might have been thrown by <paramref name="tester"/>.</param>
        /// <returns>The return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
        public static TResult SafeInvoke<T1, T2, T3, T4, T5, TResult>(Func<TResult> initializer, Func<TResult, T1, T2, T3, T4, T5, TResult> tester, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<Exception, T1, T2, T3, T4, T5> catcher = null) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer);
            Validator.ThrowIfNull(tester);
            var f1 = new FuncFactory<MutableTuple<TResult, T1, T2, T3, T4, T5>, TResult>(tuple => tester(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6), new MutableTuple<TResult, T1, T2, T3, T4, T5>(default, arg1, arg2, arg3, arg4, arg5), tester);
            var f2 = new ActionFactory<MutableTuple<Exception, T1, T2, T3, T4, T5>>(tuple => catcher?.Invoke(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6), new MutableTuple<Exception, T1, T2, T3, T4, T5>(default, arg1, arg2, arg3, arg4, arg5), catcher);
            return SafeInvokeCore(f1, initializer, f2);
        }

        private static TResult SafeInvokeCore<TTester, TResult, TCatcher>(FuncFactory<TTester, TResult> testerFactory, Func<TResult> initializer, ActionFactory<TCatcher> catcherFactory)
            where TResult : class, IDisposable
            where TTester : MutableTuple<TResult>
            where TCatcher : MutableTuple<Exception>
        {
            TResult result = null;
            try
            {
                testerFactory.GenericArguments.Arg1 = initializer();
                testerFactory.GenericArguments.Arg1 = testerFactory.ExecuteMethod();
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
                    catcherFactory.ExecuteMethod();
                }
            }
            finally
            {
                testerFactory.GenericArguments.Arg1?.Dispose();
            }
            return result;
        }
    }
}
