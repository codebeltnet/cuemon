using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace Cuemon.Extensions.FileProviders;

/// <summary>
/// Provides portable, case-insensitive path resolution for files and directories rooted in the physical file system.
/// </summary>
/// <remarks>
/// <para>
/// This provider decorates <see cref="PhysicalFileProvider"/> and resolves each path segment using ordinal, case-insensitive comparison before delegating file metadata, streams, directory contents, and change notifications to the underlying provider.
/// </para>
/// <para>
/// This provides consistent case-insensitive lookup behavior across case-sensitive and case-insensitive file systems while preserving the casing of files and directories maintained by the underlying physical file system.
/// </para>
/// <para>
/// A path segment is resolved when exactly one physical entry matches using <see cref="StringComparison.OrdinalIgnoreCase"/>. For example, when the physical file is named <c>logo.svg</c>, requests for <c>logo.svg</c>, <c>Logo.svg</c>, and <c>LOGO.SVG</c> all resolve to that file.
/// </para>
/// <para>
/// Path segments that differ only by casing have the same logical identity. If a case-sensitive file system contains multiple matching entries, such as <c>logo.svg</c> and <c>Logo.svg</c>, or a file and directory like <c>logo</c> and <c>Logo/</c>, the path segment is treated as a casing collision and none of the matching entries are selected, even when the requested casing exactly matches one of them.
/// </para>
/// <para>
/// A casing collision does not throw an exception. <see cref="GetFileInfo"/> returns a <see cref="NotFoundFileInfo"/>, <see cref="GetDirectoryContents"/> returns <see cref="NotFoundDirectoryContents.Singleton"/>, and <see cref="Watch"/> returns <see cref="NullChangeToken.Singleton"/> for a colliding literal filter.
/// </para>
/// <para>
/// Wildcard watch filters are delegated unchanged and are not inspected for casing collisions. Literal file filters, and literal directory filters that end with a trailing directory separator, are resolved to their physical casing when the corresponding entry exists without a collision. Literal directory filters without a trailing separator are delegated unchanged and follow the normal interpretation of <see cref="PhysicalFileProvider"/>. <see cref="PhysicalFileProvider"/> distinguishes wildcard watch patterns by the presence of <c>*</c>.
/// </para>
/// <para>
/// Successfully resolved paths are cached for the lifetime of this provider by using case-insensitive logical keys that normalize supported directory separators together with redundant leading, repeated, and directory-only trailing separators. Equivalent successful requests such as <c>assets/logo.svg</c>, <c>/assets/logo.svg</c>, and <c>assets//logo.svg</c> therefore share the same cache entry.
/// </para>
/// <para>
/// Resolving an uncached path enumerates each directory that must be traversed and fully inspects each segment until uniqueness or collision is determined. A cold lookup therefore scales approximately with the sum of the sibling counts in the traversed directories, so wide directories are materially more expensive than warm cache hits.
/// </para>
/// <para>
/// Misses and collisions are not cached and are re-evaluated on each call. Repeated unresolved requests, especially for wide directories or user-controlled arbitrary paths, can therefore be substantially more expensive than successful cache hits. Consumers that expose arbitrary paths should consider upstream validation, response caching, rate limiting, or other suitable controls.
/// </para>
/// <para>
/// The root should therefore have a stable naming topology for previously successful logical paths. After a case-only rename, or after introducing or removing a casing collision for a previously successful logical path, create a new provider instance to guarantee that the path is resolved again.
/// </para>
/// </remarks>
public sealed class PortablePhysicalFileProvider : Disposable, IFileProvider
{
    private const char CanonicalDirectorySeparator = '/';
    private static readonly char[] CanonicalPathSeparators = { CanonicalDirectorySeparator };
    private readonly PhysicalFileProvider _provider;
    private readonly ConcurrentDictionary<string, string> _files = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, string> _directories = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the <see cref="PortablePhysicalFileProvider"/> class at the given root directory.
    /// </summary>
    /// <param name="root">The absolute directory to use as the provider root.</param>
    /// <param name="filters">A bitwise combination of values that specifies which files or directories are excluded.</param>
    public PortablePhysicalFileProvider(string root, ExclusionFilters filters = ExclusionFilters.Sensitive)
    {
        _provider = new PhysicalFileProvider(root, filters);
    }

    /// <inheritdoc cref="PhysicalFileProvider.Root"/>
    public string Root => _provider.Root;

    /// <inheritdoc cref="PhysicalFileProvider.UsePollingFileWatcher"/>
    public bool UsePollingFileWatcher
    {
        get => _provider.UsePollingFileWatcher;
        set => _provider.UsePollingFileWatcher = value;
    }

    /// <inheritdoc cref="PhysicalFileProvider.UseActivePolling"/>
    public bool UseActivePolling
    {
        get => _provider.UseActivePolling;
        set => _provider.UseActivePolling = value;
    }

    /// <inheritdoc />
    public IFileInfo GetFileInfo(string subpath)
    {
        var resolvedPath = ResolvePath(subpath, finalSegmentIsDirectory: false, out var collision);
        return ResolveFileInfo(subpath, resolvedPath, collision);
    }

    /// <inheritdoc />
    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        var resolvedPath = ResolvePath(subpath, finalSegmentIsDirectory: true, out var collision);
        return ResolveDirectoryContents(resolvedPath, collision);
    }

    /// <inheritdoc />
    public IChangeToken Watch(string filter)
    {
        // Preserve PhysicalFileProvider behavior for null, empty, and wildcard filters.
        // PhysicalFileProvider uses '*' to distinguish wildcard watch patterns.
        if (string.IsNullOrEmpty(filter) || filter.IndexOf('*') >= 0)
        {
            return _provider.Watch(filter);
        }

#if NETSTANDARD2_0
        var finalSegmentIsDirectory = filter[filter.Length - 1] is '/' or '\\';
#else
        var finalSegmentIsDirectory = filter[^1] is '/' or '\\';
#endif
        var resolvedFilter = ResolvePath(filter, finalSegmentIsDirectory, out var collision);
        return ResolveWatchToken(resolvedFilter, finalSegmentIsDirectory, collision);
    }

    /// <inheritdoc />
    protected override void OnDisposeManagedResources()
    {
        _provider.Dispose();
    }

    private string ResolvePath(string subpath, bool finalSegmentIsDirectory, out bool collision)
    {
        return ResolvePath(subpath, finalSegmentIsDirectory, path => _provider.GetDirectoryContents(path), out collision);
    }

    private IFileInfo ResolveFileInfo(string subpath, string resolvedPath, bool collision)
    {
        return collision ? new NotFoundFileInfo(subpath) : _provider.GetFileInfo(resolvedPath);
    }

    private IDirectoryContents ResolveDirectoryContents(string resolvedPath, bool collision)
    {
        return collision ? NotFoundDirectoryContents.Singleton : _provider.GetDirectoryContents(resolvedPath);
    }

    private IChangeToken ResolveWatchToken(string resolvedFilter, bool finalSegmentIsDirectory, bool collision)
    {
        if (collision)
        {
            return NullChangeToken.Singleton;
        }

        if (finalSegmentIsDirectory && resolvedFilter.Length > 0 &&
#if NETSTANDARD2_0
            resolvedFilter[resolvedFilter.Length - 1] is not ('/' or '\\'))
#else
            resolvedFilter[^1] is not ('/' or '\\'))
#endif
        {
            resolvedFilter += Path.DirectorySeparatorChar;
        }

        return _provider.Watch(resolvedFilter);
    }

    private string ResolvePath(string subpath, bool finalSegmentIsDirectory, Func<string, IEnumerable<IFileInfo>> entriesFactory, out bool collision)
    {
        collision = false;

        if (string.IsNullOrEmpty(subpath))
        {
            return subpath;
        }

        var cache = finalSegmentIsDirectory ? _directories : _files;

        if (cache.TryGetValue(subpath, out var cachedPath))
        {
            return cachedPath;
        }

        if (!TryCreateCanonicalSubpath(subpath, finalSegmentIsDirectory, out var canonicalSubpath))
        {
            return subpath;
        }

        if (!ReferenceEquals(canonicalSubpath, subpath) && cache.TryGetValue(canonicalSubpath, out cachedPath))
        {
            return cachedPath;
        }

        var segments = canonicalSubpath.Split(CanonicalPathSeparators, StringSplitOptions.RemoveEmptyEntries);
        var currentPath = string.Empty;

        for (var i = 0; i < segments.Length; i++)
        {
            var requestedName = segments[i];
            var isDirectory = i < segments.Length - 1 || finalSegmentIsDirectory;
            var entry = FindMatchingEntry(requestedName, () => entriesFactory(currentPath), out collision);

            if (collision || entry is null)
            {
                return subpath;
            }

            if (entry.IsDirectory != isDirectory)
            {
                return subpath;
            }

            currentPath = currentPath.Length == 0 ? entry.Name : string.Concat(currentPath, "/", entry.Name);
        }

        cache.TryAdd(canonicalSubpath, currentPath);
        return currentPath;
    }

    private static IFileInfo FindMatchingEntry(string requestedName, Func<IEnumerable<IFileInfo>> entriesFactory, out bool collision)
    {
        collision = false;

        try
        {
            IFileInfo caseInsensitiveMatch = null;

            foreach (var entry in entriesFactory())
            {
                if (!string.Equals(entry.Name, requestedName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (caseInsensitiveMatch is not null)
                {
                    collision = true;
                    return null;
                }

                caseInsensitiveMatch = entry;
            }

            return caseInsensitiveMatch;
        }
        catch (Exception ex) when (ex is ArgumentException or DirectoryNotFoundException or IOException or SecurityException or UnauthorizedAccessException)
        {
            return null;
        }
    }

    private static bool TryCreateCanonicalSubpath(string subpath, bool finalSegmentIsDirectory, out string canonicalSubpath)
    {
        canonicalSubpath = null;

        var containsSegment = false;
        var endsWithDirectorySeparator = false;
        var previousWasDirectorySeparator = true;
        var requiresNormalization = false;

        for (var i = 0; i < subpath.Length; i++)
        {
            var character = subpath[i];

            if (IsSupportedDirectorySeparator(character))
            {
                if (previousWasDirectorySeparator || character != CanonicalDirectorySeparator)
                {
                    requiresNormalization = true;
                }

                previousWasDirectorySeparator = true;
                endsWithDirectorySeparator = true;
                continue;
            }

            containsSegment = true;
            endsWithDirectorySeparator = false;
            previousWasDirectorySeparator = false;
        }

        if (!containsSegment)
        {
            return false;
        }

        if (endsWithDirectorySeparator)
        {
            if (!finalSegmentIsDirectory)
            {
                return false;
            }

            requiresNormalization = true;
        }

        if (!requiresNormalization)
        {
            canonicalSubpath = subpath;
            return true;
        }

        var buffer = new char[subpath.Length];
        var length = 0;
        previousWasDirectorySeparator = true;

        for (var i = 0; i < subpath.Length; i++)
        {
            var character = subpath[i];

            if (IsSupportedDirectorySeparator(character))
            {
                if (previousWasDirectorySeparator)
                {
                    continue;
                }

                previousWasDirectorySeparator = true;
                buffer[length++] = CanonicalDirectorySeparator;
                continue;
            }

            previousWasDirectorySeparator = false;
            buffer[length++] = character;
        }

        if (length > 0 && buffer[length - 1] == CanonicalDirectorySeparator)
        {
            length--;
        }

        canonicalSubpath = new string(buffer, 0, length);
        return true;
    }

    private static bool IsSupportedDirectorySeparator(char character)
    {
        return character == Path.DirectorySeparatorChar || character == Path.AltDirectorySeparatorChar;
    }
}
