using System.Linq;
using Cuemon.Reflection;

namespace Cuemon.Extensions;
/// <summary>
/// Extension methods for the <see cref="MethodDescriptor"/> class.
/// </summary>
public static class MethodDescriptorExtensions
{
    /// <summary>
    /// Determines whether the underlying method has parameters.
    /// </summary>
    /// <param name="descriptor">The <see cref="MethodDescriptor"/> to extend.</param>
    /// <returns><c>true</c> if the specified descriptor has parameters; otherwise, <c>false</c>.</returns>
    public static bool HasParameters(this MethodDescriptor descriptor)
    {
        return descriptor.Parameters.Any();
    }
}
