using System;

namespace Cuemon.Text;
/// <summary>
/// Provides a way to support assigning a stem to a value.
/// </summary>
public sealed class Stem
{
    /// <summary>
    /// Performs an implicit conversion from <see cref="string"/> to <see cref="Stem"/>.
    /// </summary>
    /// <param name="stem">The <see cref="string"/> to convert.</param>
    /// <returns>A <see cref="Stem"/> that is equivalent to <paramref name="stem"/>.</returns>
    public static implicit operator Stem(string stem)
    {
        return new Stem(stem);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="Stem"/> to <see cref="string"/>.
    /// </summary>
    /// <param name="stem">The <see cref="Stem"/> to convert.</param>
    /// <returns>A <see cref="string"/> that is equivalent to <paramref name="stem"/>.</returns>
    public static implicit operator string(Stem stem)
    {
        return stem.Value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Stem"/> class.
    /// </summary>
    /// <param name="value">The stem to apply affixes.</param>
    public Stem(string value)
    {
        Validator.ThrowIfNull(value);
        Value = value;
    }

    /// <summary>
    /// Gets the value of this instance.
    /// </summary>
    /// <value>The value of this instance.</value>
    public string Value { get; private set; }

    /// <summary>
    /// Attaches the specified <paramref name="suffix"/> to the stem of this instance.
    /// </summary>
    /// <param name="suffix">The affix that must appear after the stem of this instance.</param>
    /// <returns>A string where the specified <paramref name="suffix"/> appears after the stem of this instance.</returns>
    /// <remarks>This method attaches the <paramref name="suffix"/> to the stem only if not already part of the ending.</remarks>
    public Stem AttachSuffix(string suffix)
    {
        return AttachSuffix(suffix, s => !s.EndsWith(suffix ?? "", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Attaches the specified <paramref name="suffix"/> to the stem of this instance.
    /// </summary>
    /// <param name="suffix">The affix that must appear after the stem of this instance.</param>
    /// <param name="condition">The function delegate that provides a condition for when to attach the <paramref name="suffix"/> to the stem.</param>
    /// <returns>A string where the specified <paramref name="suffix"/> appears after the stem of this instance.</returns>
    public Stem AttachSuffix(string suffix, Func<string, bool> condition)
    {
        if (condition == null || condition(this)) { Value = string.Concat(Value, suffix); }
        return this;
    }

    /// <summary>
    /// Attaches the specified <paramref name="prefix"/> to the stem of this instance.
    /// </summary>
    /// <param name="prefix">The affix that must appear before the stem of this instance.</param>
    /// <returns>A string where the specified <paramref name="prefix"/> appears before the stem of this instance.</returns>
    /// <remarks>This method attaches the <paramref name="prefix"/> to the stem only if not already part of the beginning.</remarks>
    public Stem AttachPrefix(string prefix)
    {
        return AttachPrefix(prefix, s => !s.StartsWith(prefix ?? "", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Attaches the specified <paramref name="prefix"/> to the stem of this instance.
    /// </summary>
    /// <param name="prefix">The affix that must appear before the stem of this instance.</param>
    /// <param name="condition">The function delegate that provides a condition for when to attach the <paramref name="prefix"/> to the stem.</param>
    /// <returns>A string where the specified <paramref name="prefix"/> appears before the stem of this instance.</returns>
    public Stem AttachPrefix(string prefix, Func<string, bool> condition)
    {
        if (condition == null || condition(this)) { Value = string.Concat(prefix, Value); }
        return this;
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        return Value;
    }
}
