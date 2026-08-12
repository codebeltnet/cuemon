using System;
using System.Collections.Generic;

namespace Cuemon.Reflection;
/// <summary>
/// Represents different representations of a version scheme in a consistent way.
/// </summary>
public class VersionResult
{
    private readonly Version _version;
    private readonly string _alphanumericVersion;

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionResult"/> class.
    /// </summary>
    /// <param name="alphanumericVersion">The <see cref="string"/> that represents a potential alphanumeric version.</param>
    public VersionResult(string alphanumericVersion)
    {
        if (Version.TryParse(alphanumericVersion, out var version) && version.ToString().Equals(alphanumericVersion, StringComparison.Ordinal))
        {
            _version = version;
        }
        else
        {
            _alphanumericVersion = alphanumericVersion;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionResult"/> class.
    /// </summary>
    /// <param name="version">The <see cref="Version"/> that represents a numerical version {major.minor.build.revision}.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="version"/> cannot be null.
    /// </exception>
    public VersionResult(Version version)
    {
        Validator.ThrowIfNull(version);
        _version = version;
    }

    /// <summary>
    /// Gets the alphanumeric version assigned to this instance.
    /// </summary>
    /// <value>The alphanumeric version assigned to this instance.</value>
    public string AlphanumericVersion => _alphanumericVersion;

    /// <summary>
    /// Gets a value indicating whether this instance has alphanumeric version assigned.
    /// </summary>
    /// <value><c>true</c> if this instance has alphanumeric version assigned; otherwise, <c>false</c>.</value>
    public bool HasAlphanumericVersion => !string.IsNullOrEmpty(_alphanumericVersion);

    /// <summary>
    /// Gets the value of the version passed to this object.
    /// </summary>
    /// <value>The value of the version passed to this object.</value>
    public string Value => _version?.ToString() ?? _alphanumericVersion;

    /// <summary>
    /// Determines whether this instance represents a semantic version.
    /// </summary>
    /// <param name="alphanumericVersion">The <see cref="string"/> that represents a potential alphanumeric version.</param>
    /// <returns><c>true</c> if this instance represents a semantic version; otherwise, <c>false</c>.</returns>
    public static bool IsSemanticVersion(string alphanumericVersion)
    {
        if (string.IsNullOrWhiteSpace(alphanumericVersion)) { return false; }
        var isSemantic = true;
        var versions = alphanumericVersion.Split('.');
        foreach (var version in versions)
        {
            isSemantic &= int.TryParse(version, out _);
        }
        return !isSemantic;
    }

    /// <summary>
    /// Determines whether this instance represents a semantic version.
    /// </summary>
    /// <returns><c>true</c> if this instance represents a semantic version; otherwise, <c>false</c>.</returns>
    public bool IsSemanticVersion()
    {
        return IsSemanticVersion(_alphanumericVersion);
    }

    /// <summary>
    /// Converts this instance to an equivalent <see cref="Version"/> object.
    /// </summary>
    /// <returns>An equivalent <see cref="Version"/> object of this instance.</returns>
    public Version ToVersion()
    {
        if (_version != null) { return _version; }
        var versionComponents = new List<int>();
        var versions = _alphanumericVersion.Split('.');
        var valid = true;
        foreach (var version in versions)
        {
            valid &= int.TryParse(version, out var v);
            if (valid) { versionComponents.Add(v); }
            if (versionComponents.Count == 4) { break; }
        }
        if (versionComponents.Count < 2) { throw new InvalidOperationException($"{nameof(AlphanumericVersion)} has fewer than two compatible components to qualify for a Version object."); }
        return new Version(DelimitedString.Create(versionComponents, o => o.Delimiter = "."));
    }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return Value;
    }
}
