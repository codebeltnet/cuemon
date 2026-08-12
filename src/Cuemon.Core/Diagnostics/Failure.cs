using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cuemon.Diagnostics;
/// <summary>
/// Represents a failure model with detailed information about an exception.
/// </summary>
public record Failure : IReadOnlyDictionary<string, object>
{
    private readonly Exception _exception;
    private readonly Type _exceptionType;
    private readonly IDictionary<string, object> _properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
    private readonly FaultSensitivityDetails _sensitivity;

    /// <summary>
    /// Initializes a new instance of the <see cref="Failure"/> class.
    /// </summary>
    /// <param name="exception">The <see cref="Exception"/> that represents the failure.</param>
    /// <param name="sensitivity">The <see cref="FaultSensitivityDetails"/> of the fault.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="exception"/> is null.</exception>
    public Failure(Exception exception, FaultSensitivityDetails sensitivity)
    {
        Validator.ThrowIfNull(exception);

        _exception = exception;
        _exceptionType = exception.GetType();
        _sensitivity = sensitivity;

        if (exception.Data.Count > 0 && sensitivity.HasFlag(FaultSensitivityDetails.Data))
        {
            foreach (DictionaryEntry entry in exception.Data)
            {
                Data.Add(entry.Key.ToString()!, entry.Value);
            }
        }

        if (exception.StackTrace != null && sensitivity.HasFlag(FaultSensitivityDetails.StackTrace))
        {
            Stack = exception.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
        }

        var properties = Decorator.Enclose(exception.GetType()).GetRuntimePropertiesExceptOf<AggregateException>();
        foreach (var property in properties)
        {
            var value = property.GetValue(_exception);
            if (value == null) { continue; }
            _properties.Add(property.Name, value);
        }
    }

    /// <summary>
    /// Gets the type of the underlying exception.
    /// </summary>
    public string Type => _exceptionType?.FullName;

    /// <summary>
    /// Gets the namespace of the underlying exception.
    /// </summary>
    public string Namespace => _exceptionType?.Namespace;

    /// <summary>
    /// Gets the source of the underlying exception.
    /// </summary>
    public string Source => _exception?.Source;

    /// <summary>
    /// Gets the message of the underlying exception.
    /// </summary>
    public string Message => _exception?.Message;

    /// <summary>
    /// Gets the stack trace of the underlying exception.
    /// </summary>
    public IEnumerable<string> Stack { get; } = Enumerable.Empty<string>();

    /// <summary>
    /// Gets the data associated with the underlying exception.
    /// </summary>
    public IDictionary<string, object> Data { get; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the underlying exception to this <see cref="Failure"/>.
    /// </summary>
    /// <returns>The underlying exception to this <see cref="Failure"/>.</returns>
    public Exception GetUnderlyingException() => _exception;

    /// <summary>
    /// Gets the underlying sensitivity details of this <see cref="Failure"/>.
    /// </summary>
    /// <returns>The underlying sensitivity details of this <see cref="Failure"/>.</returns>
    public FaultSensitivityDetails GetUnderlyingSensitivity() => _sensitivity;

    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
    {
        return _properties.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_properties).GetEnumerator();
    }

    int IReadOnlyCollection<KeyValuePair<string, object>>.Count => _properties.Count;

    bool IReadOnlyDictionary<string, object>.ContainsKey(string key)
    {
        return _properties.ContainsKey(key);
    }

    bool IReadOnlyDictionary<string, object>.TryGetValue(string key, out object value)
    {
        return _properties.TryGetValue(key, out value);
    }

    object IReadOnlyDictionary<string, object>.this[string key] => _properties[key];

    IEnumerable<string> IReadOnlyDictionary<string, object>.Keys => _properties.Keys;

    IEnumerable<object> IReadOnlyDictionary<string, object>.Values => _properties.Values;
}
