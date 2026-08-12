using System;
using System.Linq;

namespace Cuemon.Extensions.DependencyInjection;
/// <summary>
/// Extension methods for the <see cref="Type"/> class.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Returns a value that indicates whether the specified <paramref name="type"/> contains an <see cref="IDependencyInjectionMarker{TMarker}"/> interface.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to extend.</param>
    /// <param name="result">When this method returns, contains the <see cref="Type"/> of the generic parameter <c>TMarker</c> of <see cref="IDependencyInjectionMarker{TMarker}"/>; otherwise <c>null</c> if the specified <paramref name="type"/> does not have an implemenation of the <see cref="IDependencyInjectionMarker{TMarker}"/> interface.</param>
    /// <returns><c>true</c> if the specified <paramref name="type"/> contains an <see cref="IDependencyInjectionMarker{TMarker}"/> interface, <c>false</c> otherwise.</returns>
    public static bool TryGetDependencyInjectionMarker(this Type type, out Type result)
    {
        Validator.ThrowIfNull(type);
        var dim = type.GetInterfaces().SingleOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IDependencyInjectionMarker<>));
        if (dim != null)
        {
            result = dim.GenericTypeArguments.First();
            return true;
        }
        result = null;
        return false;
    }
}
