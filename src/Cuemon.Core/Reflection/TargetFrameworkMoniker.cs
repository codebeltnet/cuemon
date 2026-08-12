using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;

namespace Cuemon.Reflection;

/// <summary>
/// Provides a set of static methods for resolving target framework monikers from assemblies and the current application context.
/// </summary>
public static class TargetFrameworkMoniker
{
    /// <summary>
    /// Parses the specified <paramref name="frameworkNameOrTargetFrameworkMoniker"/> into its short target framework moniker representation.
    /// </summary>
    /// <param name="frameworkNameOrTargetFrameworkMoniker">The framework name or target framework moniker to parse.</param>
    /// <returns>The parsed target framework moniker, or <see langword="null"/> if <paramref name="frameworkNameOrTargetFrameworkMoniker"/> could not be parsed as a supported target framework moniker.</returns>
    public static string Parse(string frameworkNameOrTargetFrameworkMoniker)
    {
        _ = TryParse(frameworkNameOrTargetFrameworkMoniker, out var targetFrameworkMoniker);
        return targetFrameworkMoniker;
    }

    /// <summary>
    /// Attempts to parse the specified <paramref name="frameworkNameOrTargetFrameworkMoniker"/> into its short target framework moniker representation.
    /// </summary>
    /// <param name="frameworkNameOrTargetFrameworkMoniker">The framework name or target framework moniker to parse.</param>
    /// <param name="targetFrameworkMoniker">When this method returns, contains the parsed target framework moniker, or <see langword="null"/> if <paramref name="frameworkNameOrTargetFrameworkMoniker"/> could not be parsed as a supported target framework moniker.</param>
    /// <returns><c>true</c> if <paramref name="frameworkNameOrTargetFrameworkMoniker"/> was parsed successfully; otherwise, <c>false</c>.</returns>
    public static bool TryParse(string frameworkNameOrTargetFrameworkMoniker, out string targetFrameworkMoniker)
    {
        targetFrameworkMoniker = null;
        if (string.IsNullOrWhiteSpace(frameworkNameOrTargetFrameworkMoniker))
        {
            return false;
        }

        return TryParseCandidate(frameworkNameOrTargetFrameworkMoniker, out targetFrameworkMoniker) ||
               TryParseFrameworkName(frameworkNameOrTargetFrameworkMoniker, out targetFrameworkMoniker);
    }

    /// <summary>
    /// Parses the specified <paramref name="frameworkName"/> into its short target framework moniker representation.
    /// </summary>
    /// <param name="frameworkName">The <see cref="FrameworkName"/> to parse.</param>
    /// <returns>The parsed target framework moniker, or <see langword="null"/> if <paramref name="frameworkName"/> could not be parsed as a supported target framework moniker.</returns>
    public static string Parse(FrameworkName frameworkName)
    {
        _ = TryParse(frameworkName, out var targetFrameworkMoniker);
        return targetFrameworkMoniker;
    }

    /// <summary>
    /// Attempts to parse the specified <paramref name="frameworkName"/> into its short target framework moniker representation.
    /// </summary>
    /// <param name="frameworkName">The <see cref="FrameworkName"/> to parse.</param>
    /// <param name="targetFrameworkMoniker">When this method returns, contains the parsed target framework moniker, or <see langword="null"/> if <paramref name="frameworkName"/> could not be parsed as a supported target framework moniker.</param>
    /// <returns><c>true</c> if <paramref name="frameworkName"/> was parsed successfully; otherwise, <c>false</c>.</returns>
    public static bool TryParse(FrameworkName frameworkName, out string targetFrameworkMoniker)
    {
        targetFrameworkMoniker = null;
        if (frameworkName == null)
        {
            return false;
        }

        var version = frameworkName.Version;
        if (frameworkName.Identifier.Equals(".NETFramework", StringComparison.OrdinalIgnoreCase))
        {
            targetFrameworkMoniker = $"net{version.Major}{version.Minor}";
            if (version.Major >= 4 && version.Build > 0)
            {
                targetFrameworkMoniker += version.Build;
            }

            return true;
        }

        if (frameworkName.Identifier.Equals(".NETStandard", StringComparison.OrdinalIgnoreCase))
        {
            targetFrameworkMoniker = $"netstandard{version.Major}.{version.Minor}";
            return true;
        }

        if (frameworkName.Identifier.Equals(".NETCoreApp", StringComparison.OrdinalIgnoreCase))
        {
            targetFrameworkMoniker = version.Major <= 3 ? $"netcoreapp{version.Major}.{version.Minor}" : $"net{version.Major}.{version.Minor}";
            return true;
        }

        return false;
    }

    /// <summary>
    /// Resolves the target framework moniker of the specified <paramref name="assembly"/>.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly"/> to inspect for a <see cref="TargetFrameworkAttribute"/>.</param>
    /// <returns>The resolved target framework moniker of the specified <paramref name="assembly"/>, or <see langword="null"/> if no supported moniker could be resolved.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="assembly"/> is null.
    /// </exception>
    public static string Resolve(Assembly assembly)
    {
        TryResolve(assembly, out var targetFrameworkMoniker);
        return targetFrameworkMoniker;
    }

    /// <summary>
    /// Attempts to resolve the target framework moniker of the specified <paramref name="assembly"/>.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly"/> to inspect for a <see cref="TargetFrameworkAttribute"/>.</param>
    /// <param name="targetFrameworkMoniker">When this method returns, contains the resolved target framework moniker of the specified <paramref name="assembly"/>, or <see langword="null"/> if the operation failed.</param>
    /// <returns><c>true</c> if the target framework moniker of the specified <paramref name="assembly"/> was resolved successfully; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="assembly"/> is null.
    /// </exception>
    public static bool TryResolve(Assembly assembly, out string targetFrameworkMoniker)
    {
        Validator.ThrowIfNull(assembly);
        return TryResolveFromAssembly(assembly, out targetFrameworkMoniker);
    }

    /// <summary>
    /// Resolves the target framework moniker of the current application context.
    /// </summary>
    /// <returns>The resolved target framework moniker of the current application context, or <see langword="null"/> if no supported moniker could be resolved.</returns>
    /// <remarks>
    /// Resolution first inspects the entry assembly for a <see cref="TargetFrameworkAttribute"/>. If no supported framework name is found, the directory hierarchy rooted at <see cref="AppContext.BaseDirectory"/> is inspected for a target-framework-like folder name.
    /// </remarks>
    public static string ResolveCurrent()
    {
        TryResolveCurrent(out var targetFrameworkMoniker);
        return targetFrameworkMoniker;
    }

    /// <summary>
    /// Attempts to resolve the target framework moniker of the current application context.
    /// </summary>
    /// <param name="targetFrameworkMoniker">When this method returns, contains the resolved target framework moniker of the current application context, or <see langword="null"/> if the operation failed.</param>
    /// <returns><c>true</c> if the target framework moniker of the current application context was resolved successfully; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// Resolution first inspects the entry assembly for a <see cref="TargetFrameworkAttribute"/>. If no supported framework name is found, the directory hierarchy rooted at <see cref="AppContext.BaseDirectory"/> is inspected for a target-framework-like folder name.
    /// </remarks>
    public static bool TryResolveCurrent(out string targetFrameworkMoniker)
    {
        if (TryResolveFromAssembly(Assembly.GetEntryAssembly(), out targetFrameworkMoniker))
        {
            return true;
        }

        return TryResolveFromPath(AppContext.BaseDirectory, out targetFrameworkMoniker);
    }

    /// <summary>
    /// Resolves the nearest target framework moniker found in the specified <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The path whose directory hierarchy should be inspected for a target-framework-like folder name.</param>
    /// <returns>The nearest resolved target framework moniker found in the specified <paramref name="path"/>, or <see langword="null"/> if no supported moniker could be resolved.</returns>
    public static string ResolveFromPath(string path)
    {
        TryResolveFromPath(path, out var targetFrameworkMoniker);
        return targetFrameworkMoniker;
    }

    /// <summary>
    /// Attempts to resolve the nearest target framework moniker found in the specified <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The path whose directory hierarchy should be inspected for a target-framework-like folder name.</param>
    /// <param name="targetFrameworkMoniker">When this method returns, contains the nearest resolved target framework moniker found in the specified <paramref name="path"/>, or <see langword="null"/> if the operation failed.</param>
    /// <returns><c>true</c> if a target framework moniker was resolved successfully from the specified <paramref name="path"/>; otherwise, <c>false</c>.</returns>
    public static bool TryResolveFromPath(string path, out string targetFrameworkMoniker)
    {
        targetFrameworkMoniker = null;
        if (string.IsNullOrWhiteSpace(path))
        {
            return false;
        }

        var directory = new DirectoryInfo(path);
        while (directory != null)
        {
            if (TryParseCandidate(directory.Name, out targetFrameworkMoniker))
            {
                return true;
            }

            directory = directory.Parent;
        }

        return false;
    }

    private static bool TryResolveFromAssembly(Assembly assembly, out string targetFrameworkMoniker)
    {
        targetFrameworkMoniker = null;
        return assembly != null && TryParse(assembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName, out targetFrameworkMoniker);
    }

    private static bool TryParseFrameworkName(string frameworkName, out string targetFrameworkMoniker)
    {
        targetFrameworkMoniker = null;
        FrameworkName parsedFrameworkName;
        try
        {
            parsedFrameworkName = new FrameworkName(frameworkName);
        }
        catch (ArgumentException)
        {
            return false;
        }

        return TryParse(parsedFrameworkName, out targetFrameworkMoniker);
    }

    private static bool TryParseCandidate(string candidate, out string targetFrameworkMoniker)
    {
        targetFrameworkMoniker = null;
        if (string.IsNullOrWhiteSpace(candidate))
        {
            return false;
        }

        var normalizedCandidate = candidate.Trim().ToLowerInvariant();
        var platformSeparatorIndex = normalizedCandidate.IndexOf('-');
        var frameworkCandidate = platformSeparatorIndex > 0 ? normalizedCandidate.Substring(0, platformSeparatorIndex) : normalizedCandidate;

        if (!IsTargetFrameworkMonikerCandidate(frameworkCandidate))
        {
            return false;
        }

        targetFrameworkMoniker = normalizedCandidate;
        return true;
    }

    private static bool IsTargetFrameworkMonikerCandidate(string candidate)
    {
        if (candidate.StartsWith("netstandard", StringComparison.Ordinal))
        {
            return HasMajorMinorVersionSuffix(candidate, "netstandard");
        }

        if (candidate.StartsWith("netcoreapp", StringComparison.Ordinal))
        {
            return HasMajorMinorVersionSuffix(candidate, "netcoreapp");
        }

        return candidate.StartsWith("net", StringComparison.Ordinal) && HasNetVersionSuffix(candidate.Substring(3));
    }

    private static bool HasMajorMinorVersionSuffix(string candidate, string prefix)
    {
        var versionSuffix = candidate.Substring(prefix.Length);
        return HasSingleDotSeparatedVersion(versionSuffix);
    }

    private static bool HasNetVersionSuffix(string versionSuffix)
    {
        if (string.IsNullOrEmpty(versionSuffix))
        {
            return false;
        }

        if (versionSuffix.IndexOf('.') >= 0)
        {
            return HasSingleDotSeparatedVersion(versionSuffix);
        }

        return versionSuffix.Length >= 2 && HasDigitsOnly(versionSuffix);
    }

    private static bool HasSingleDotSeparatedVersion(string versionSuffix)
    {
        if (string.IsNullOrEmpty(versionSuffix))
        {
            return false;
        }

        var separatorIndex = versionSuffix.IndexOf('.');
        return separatorIndex > 0 &&
               separatorIndex == versionSuffix.LastIndexOf('.') &&
               separatorIndex < versionSuffix.Length - 1 &&
               Version.TryParse(versionSuffix, out _);
    }

    private static bool HasDigitsOnly(string value)
    {
        foreach (var character in value)
        {
            if (!char.IsDigit(character))
            {
                return false;
            }
        }

        return true;
    }
}
