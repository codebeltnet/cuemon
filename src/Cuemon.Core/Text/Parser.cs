using System;
using Cuemon.Configuration;

namespace Cuemon.Text;
internal sealed class Parser : IParser
{
    private readonly Func<string, Type, object> _parser;

    internal Parser(Func<string, Type, object> parser)
    {
        _parser = parser;
    }

    public T Parse<T>(string input)
    {
        return (T)Parse(input, typeof(T));
    }

    public object Parse(string input, Type targetType)
    {
        return _parser(input, targetType);
    }

    public bool TryParse<T>(string input, out T result)
    {
        return Patterns.TryInvoke(() => Parse<T>(input), out result);
    }

    public bool TryParse(string input, Type targetType, out object result)
    {
        return Patterns.TryInvoke(() => Parse(input, targetType), out result);
    }
}

internal sealed class Parser<TResult> : IParser<TResult>
{
    private readonly Func<string, TResult> _parser;

    internal Parser(Func<string, TResult> parser)
    {
        _parser = parser;
    }

    public TResult Parse(string input)
    {
        return _parser(input);
    }

    public bool TryParse(string input, out TResult result)
    {
        return Patterns.TryInvoke(() => Parse(input), out result);
    }
}

internal sealed class ConfigurableParser<TResult, TOptions> : IConfigurableParser<TResult, TOptions> where TOptions : class, IParameterObject, new()
{
    private readonly Func<string, Action<TOptions>, TResult> _parser;

    internal ConfigurableParser(Func<string, Action<TOptions>, TResult> parser)
    {
        _parser = parser;
    }

    public TResult Parse(string input, Action<TOptions> setup = null)
    {
        return _parser(input, setup);
    }

    public bool TryParse(string input, out TResult result, Action<TOptions> setup = null)
    {
        return Patterns.TryInvoke(() => Parse(input, setup), out result);
    }
}

internal sealed class ConfigurableParser<TOptions> : IConfigurableParser<TOptions> where TOptions : class, IParameterObject, new()
{
    private readonly Func<string, Type, Action<TOptions>, object> _parser;

    internal ConfigurableParser(Func<string, Type, Action<TOptions>, object> parser)
    {
        _parser = parser;
    }

    public T Parse<T>(string input, Action<TOptions> setup = null)
    {
        return (T)Parse(input, typeof(T), setup);
    }

    public object Parse(string input, Type targetType, Action<TOptions> setup = null)
    {
        return _parser(input, targetType, setup);
    }

    public bool TryParse<T>(string input, out T result, Action<TOptions> setup = null)
    {
        return Patterns.TryInvoke(() => Parse<T>(input, setup), out result);
    }

    public bool TryParse(string input, Type targetType, out object result, Action<TOptions> setup = null)
    {
        return Patterns.TryInvoke(() => Parse(input, targetType, setup), out result);
    }
}
