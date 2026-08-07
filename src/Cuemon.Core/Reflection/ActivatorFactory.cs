using System;
using System.Globalization;
using System.Reflection;

namespace Cuemon.Reflection;
/// <summary>
/// Provides access to factory methods for creating instances of the specified generic type parameter.
/// </summary>
public static class ActivatorFactory
{
    /// <summary>
    /// Creates an instance of <typeparamref name="TInstance"/> using the parameterless constructor.
    /// </summary>
    /// <typeparam name="TInstance">The type to create.</typeparam>
    /// <param name="setup">The <see cref="ActivatorOptions" /> which may be configured.</param>
    /// <returns>A reference to the newly created object.</returns>
    /// <seealso cref="Activator.CreateInstance(Type, BindingFlags, Binder, object[], CultureInfo)"/>.
    public static TInstance CreateInstance<TInstance>(Action<ActivatorOptions> setup = null)
    {
        var factory = new FuncFactory<MutableTuple, TInstance>(null, new MutableTuple());
        return CreateInstanceCore(factory, setup);
    }

    /// <summary>
    /// Creates an instance of <typeparamref name="TInstance" /> using a constructor of one parameters.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the constructor.</typeparam>
    /// <typeparam name="TInstance">The type to create.</typeparam>
    /// <param name="arg">The parameter of the constructor.</param>
    /// <param name="setup">The <see cref="ActivatorOptions" /> which may be configured.</param>
    /// <returns>A reference to the newly created object.</returns>
    /// <seealso cref="Activator.CreateInstance(Type, BindingFlags, Binder, object[], CultureInfo)"/>.
    public static TInstance CreateInstance<T, TInstance>(T arg, Action<ActivatorOptions> setup = null)
    {
        var factory = new FuncFactory<MutableTuple<T>, TInstance>(null, new MutableTuple<T>(arg));
        return CreateInstanceCore(factory, setup);
    }

    /// <summary>
    /// Creates an instance of <typeparamref name="TInstance" /> using a constructor of two parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the constructor.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the constructor.</typeparam>
    /// <typeparam name="TInstance">The type to create.</typeparam>
    /// <param name="arg1">The first parameter of the constructor.</param>
    /// <param name="arg2">The second parameter of the constructor.</param>
    /// <param name="setup">The <see cref="ActivatorOptions" /> which may be configured.</param>
    /// <returns>A reference to the newly created object.</returns>
    /// <seealso cref="Activator.CreateInstance(Type, BindingFlags, Binder, object[], CultureInfo)"/>.
    public static TInstance CreateInstance<T1, T2, TInstance>(T1 arg1, T2 arg2, Action<ActivatorOptions> setup = null)
    {
        var factory = new FuncFactory<MutableTuple<T1, T2>, TInstance>(null, new MutableTuple<T1, T2>(arg1, arg2));
        return CreateInstanceCore(factory, setup);
    }

    /// <summary>
    /// Creates an instance of <typeparamref name="TInstance" /> using a constructor of three parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the constructor.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the constructor.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the constructor.</typeparam>
    /// <typeparam name="TInstance">The type to create.</typeparam>
    /// <param name="arg1">The first parameter of the constructor.</param>
    /// <param name="arg2">The second parameter of the constructor.</param>
    /// <param name="arg3">The third parameter of the constructor.</param>
    /// <param name="setup">The <see cref="ActivatorOptions" /> which may be configured.</param>
    /// <returns>A reference to the newly created object.</returns>
    /// <seealso cref="Activator.CreateInstance(Type, BindingFlags, Binder, object[], CultureInfo)"/>.
    public static TInstance CreateInstance<T1, T2, T3, TInstance>(T1 arg1, T2 arg2, T3 arg3, Action<ActivatorOptions> setup = null)
    {
        var factory = new FuncFactory<MutableTuple<T1, T2, T3>, TInstance>(null, new MutableTuple<T1, T2, T3>(arg1, arg2, arg3));
        return CreateInstanceCore(factory, setup);
    }

    /// <summary>
    /// Creates an instance of <typeparamref name="TInstance" /> using a constructor of four parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the constructor.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the constructor.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the constructor.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the constructor.</typeparam>
    /// <typeparam name="TInstance">The type to create.</typeparam>
    /// <param name="arg1">The first parameter of the constructor.</param>
    /// <param name="arg2">The second parameter of the constructor.</param>
    /// <param name="arg3">The third parameter of the constructor.</param>
    /// <param name="arg4">The fourth parameter of the constructor.</param>
    /// <param name="setup">The <see cref="ActivatorOptions" /> which may be configured.</param>
    /// <returns>A reference to the newly created object.</returns>
    /// <seealso cref="Activator.CreateInstance(Type, BindingFlags, Binder, object[], CultureInfo)"/>.
    public static TInstance CreateInstance<T1, T2, T3, T4, TInstance>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<ActivatorOptions> setup = null)
    {
        var factory = new FuncFactory<MutableTuple<T1, T2, T3, T4>, TInstance>(null, new MutableTuple<T1, T2, T3, T4>(arg1, arg2, arg3, arg4));
        return CreateInstanceCore(factory, setup);
    }

    /// <summary>
    /// Creates an instance of <typeparamref name="TInstance" /> using a constructor of five parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the constructor.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the constructor.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the constructor.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the constructor.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the constructor.</typeparam>
    /// <typeparam name="TInstance">The type to create.</typeparam>
    /// <param name="arg1">The first parameter of the constructor.</param>
    /// <param name="arg2">The second parameter of the constructor.</param>
    /// <param name="arg3">The third parameter of the constructor.</param>
    /// <param name="arg4">The fourth parameter of the constructor.</param>
    /// <param name="arg5">The fifth parameter of the constructor.</param>
    /// <param name="setup">The <see cref="ActivatorOptions" /> which may be configured.</param>
    /// <returns>A reference to the newly created object.</returns>
    /// <seealso cref="Activator.CreateInstance(Type, BindingFlags, Binder, object[], CultureInfo)"/>.
    public static TInstance CreateInstance<T1, T2, T3, T4, T5, TInstance>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<ActivatorOptions> setup = null)
    {
        var factory = new FuncFactory<MutableTuple<T1, T2, T3, T4, T5>, TInstance>(null, new MutableTuple<T1, T2, T3, T4, T5>(arg1, arg2, arg3, arg4, arg5));
        return CreateInstanceCore(factory, setup);
    }

    private static TInstance CreateInstanceCore<TTuple, TInstance>(FuncFactory<TTuple, TInstance> factory, Action<ActivatorOptions> setup = null) where TTuple : MutableTuple
    {
        var options = Patterns.Configure(setup);
        return (TInstance)Activator.CreateInstance(typeof(TInstance), options.Flags, options.Binder, factory.GenericArguments.ToArray(), options.FormatProvider as CultureInfo);
    }
}
