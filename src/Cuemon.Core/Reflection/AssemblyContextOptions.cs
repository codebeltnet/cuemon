using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Cuemon.Configuration;

namespace Cuemon.Reflection;

/// <summary>
/// Provides configuration options for the <see cref="AssemblyContext"/> class.
/// </summary>
/// <seealso cref="IValidatableParameterObject"/>
public class AssemblyContextOptions : IValidatableParameterObject
{
    private const string SystemPrefix = nameof(System);
    private const string MicrosoftPrefix = nameof(Microsoft);
    private static readonly ConcurrentDictionary<string, bool> FrameworkNamespaceCache = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyContextOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="AssemblyContextOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="IncludeReferencedAssemblies"/></term>
    ///         <description><c>true</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="AssemblyFilter"/></term>
    ///         <description>The default implementation excludes assemblies whose full name starts with <c>System</c>, <c>Microsoft</c> or have a root namespace of <c>System</c> or <c>Microsoft</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="ReferencedAssemblyFilter"/></term>
    ///         <description>The default implementation excludes assemblies whose full name starts with <c>System</c> or <c>Microsoft</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="ExcludedAssemblies"/></term>
    ///         <description>The default implementation excludes this assembly, e.g., Cuemon.Core</description>
    ///     </item>
    /// </list>
    /// </remarks>
    public AssemblyContextOptions()
    {
        AssemblyFilter = DefaultAssemblyFilter;
        ReferencedAssemblyFilter = DefaultReferencedAssemblyFilter;
        ExcludedAssemblies = new List<Assembly>()
        {
            typeof(AssemblyContextOptions).Assembly
        };
    }

    private static Func<Assembly, bool> DefaultAssemblyFilter { get; } = assembly =>
    {
        var fullName = assembly?.FullName;
        if (string.IsNullOrEmpty(fullName)) { return false; }

        if (fullName.StartsWith(SystemPrefix, StringComparison.Ordinal) ||
            fullName.StartsWith(MicrosoftPrefix, StringComparison.Ordinal))
        {
            return false;
        }

        return !HasFrameworkRootNamespace(assembly);
    };

    private static Func<AssemblyName, bool> DefaultReferencedAssemblyFilter { get; } = assemblyName =>
    {
        var fullName = assemblyName?.FullName;
        return !string.IsNullOrEmpty(fullName) &&
               !fullName.StartsWith(SystemPrefix, StringComparison.Ordinal) &&
               !fullName.StartsWith(MicrosoftPrefix, StringComparison.Ordinal);
    };

    private static bool HasFrameworkRootNamespace(IEnumerable<Type> types)
    {
        if (types == null) { return false; }

        return types
            .Select(t => t?.Namespace)
            .Where(ns => !string.IsNullOrEmpty(ns))
            .Select(ns =>
            {
                var dot = ns.IndexOf('.');
                return dot < 0 ? ns : ns.Substring(0, dot);
            })
            .Any(root => root == SystemPrefix || root == MicrosoftPrefix);
    }

    private static bool HasFrameworkRootNamespace(Assembly assembly)
    {
        var key = assembly.FullName;
        if (string.IsNullOrEmpty(key)) { return false; }

        return FrameworkNamespaceCache.GetOrAdd(key, _ =>
        {
            try
            {
                if (HasFrameworkRootNamespace(assembly.GetExportedTypes()))
                {
                    return true;
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                if (HasFrameworkRootNamespace(ex.Types))
                {
                    return true;
                }
            }
            catch (Exception ex) when (Patterns.IsRecoverableException(ex))
            {
                return false;
            }
            return false;
        });
    }

    /// <summary>
    /// Gets or sets the collection of assemblies that are unconditionally excluded from the result of <see cref="AssemblyContext.GetCurrentDomainAssemblies"/>.
    /// </summary>
    /// <value>A mutable <see cref="ICollection{T}"/> of <see cref="Assembly"/> instances that will never appear in the resolved output, regardless of the <see cref="AssemblyFilter"/> or <see cref="ReferencedAssemblyFilter"/> predicates.</value>
    /// <remarks>
    /// By default this collection contains the assembly that defines <see cref="AssemblyContextOptions"/> itself (i.e., <c>Cuemon.Core</c>),
    /// preventing internal infrastructure assemblies from leaking into consumer results.
    /// Add additional assemblies to this collection when callers must suppress specific entries from the resolved set.
    /// </remarks>
    public ICollection<Assembly> ExcludedAssemblies { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether referenced assemblies should be included when resolving assembly context.
    /// </summary>
    /// <value><c>true</c> if referenced assemblies should be included; otherwise, <c>false</c>.</value>
    /// <remarks>When set to <c>true</c>, the assembly context will include assemblies that are referenced by the assemblies in the current application domain. When set to <c>false</c>, only the assemblies in the current application domain will be included.</remarks>
    public bool IncludeReferencedAssemblies { get; set; } = true;

    /// <summary>
    /// Gets or sets a predicate used to filter which assemblies are included during assembly context resolution.
    /// </summary>
    /// <value>A <see cref="Func{T, TResult}"/> that receives an <see cref="Assembly"/> and returns <c>true</c> if the assembly should be included; otherwise, <c>false</c>.</value>
    /// <remarks>
    /// This filter is applied to assemblies in the current application domain.
    /// The default predicate excludes any assembly whose <see cref="Assembly.FullName"/> starts with <c>System</c> or <c>Microsoft</c>,
    /// and additionally excludes any assembly whose exported types are rooted in the <c>System</c> or <c>Microsoft</c> namespace,
    /// limiting resolution to application-level dependencies. Results of the type-scan are cached per assembly identity to avoid
    /// repeated enumeration on subsequent calls.
    /// </remarks>
    public Func<Assembly, bool> AssemblyFilter { get; set; }

    /// <summary>
    /// Gets or sets a predicate used to filter which referenced assemblies are included during assembly context resolution.
    /// </summary>
    /// <value>A <see cref="Func{T, TResult}"/> that receives an <see cref="AssemblyName"/> and returns <c>true</c> if the assembly should be included; otherwise, <c>false</c>.</value>
    /// <remarks>
    /// This filter is only applied when <see cref="IncludeReferencedAssemblies"/> is <c>true</c>.
    /// The default predicate excludes any assembly whose <see cref="AssemblyName.FullName"/> starts with <c>System</c> or <c>Microsoft</c>,
    /// limiting resolution to application-level dependencies.
    /// </remarks>
    public Func<AssemblyName, bool> ReferencedAssemblyFilter { get; set; }

    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <see cref="AssemblyFilter"/> cannot be null - or -
    /// <see cref="ExcludedAssemblies"/> cannot be null - or -
    /// <see cref="ReferencedAssemblyFilter"/> cannot be null.
    /// </exception>
    public void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(AssemblyFilter is null);
        Validator.ThrowIfInvalidState(ExcludedAssemblies is null);
        Validator.ThrowIfInvalidState(ReferencedAssemblyFilter is null);
    }
}
