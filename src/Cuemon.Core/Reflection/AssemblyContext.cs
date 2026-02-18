using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cuemon.Collections.Generic;

namespace Cuemon.Reflection;

/// <summary>
/// Provides a set of static methods and properties to manage and filter assemblies in the current application domain.
/// </summary>
public static class AssemblyContext
{
    /// <summary>
    /// Gets the qualified assemblies from the current application domain.
    /// </summary>
    /// <param name="setup">The <see cref="AssemblyContextOptions"/> which may be configured.</param>
    /// <returns>A read-only list of <see cref="Assembly"/> instances that match the filter criteria defined in <see cref="AssemblyContextOptions"/>.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="setup"/> failed to configure an instance of <see cref="AssemblyContextOptions"/> in a valid state.
    /// </exception>
    public static IReadOnlyList<Assembly> GetCurrentDomainAssemblies(Action<AssemblyContextOptions> setup = null)
    {
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        return AppDomain
            .CurrentDomain
            .GetAssemblies()
            .Where(options.AssemblyFilter)
            .SelectMany(options.IncludeReferencedAssemblies
                ? a => GetReferencedAssemblies(a, options.ReferencedAssemblyFilter)
                : (Func<Assembly, IEnumerable<Assembly>>)(a => new[] { a }))
            .Distinct()
            .Except(options.ExcludedAssemblies)
            .ToList()
            .AsReadOnly();
    }

    /// <summary>
    /// Recursively enumerates an <paramref name="assembly"/> and all of its referenced assemblies that satisfy the <paramref name="assemblyReferenceFilter"/>.
    /// </summary>
    /// <param name="assembly">The root <see cref="Assembly"/> from which to start traversal.</param>
    /// <param name="assemblyReferenceFilter">A predicate used to filter which <see cref="AssemblyName"/> references are followed during traversal.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Assembly"/> instances reachable from <paramref name="assembly"/> that pass the <paramref name="assemblyReferenceFilter"/>.</returns>
    private static IEnumerable<Assembly> GetReferencedAssemblies(Assembly assembly, Func<AssemblyName, bool> assemblyReferenceFilter)
    {
        var stack = new Stack<Assembly>();
        var guard = new HashSet<string>();

        yield return assembly;

        stack.Push(assembly);
        guard.Add(assembly.FullName);

        while (TryPop(stack, out var assemblyToTraverse))
        {
            foreach (var assemblyName in assemblyToTraverse.GetReferencedAssemblies().Where(assemblyReferenceFilter))
            {
                if (!guard.Add(assemblyName.FullName)) { continue; }
                if (Patterns.TryInvoke(() => Assembly.Load(assemblyName), out var referencedAssembly) && referencedAssembly != null)
                {
                    stack.Push(referencedAssembly);
                    yield return referencedAssembly;
                }
            }
        }
    }

    private static bool TryPop(Stack<Assembly> stack, out Assembly assembly)
    {
#if NET9_0_OR_GREATER
        return stack.TryPop(out assembly);
#else
        return Decorator.RawEnclose(stack).TryPop(out assembly);
#endif
    }
}
