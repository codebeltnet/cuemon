using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using Cuemon.Diagnostics;
using Cuemon.Reflection;
using Cuemon.Threading;

namespace Cuemon;
/// <summary>
/// Provides a set of static methods for embedding environment specific insights to an exception.
/// </summary>
public static class ExceptionInsights
{
    private static readonly string EmptyBase64 = Convert.ToBase64String(Convertible.GetBytes(""));

    /// <summary>
    /// The <see cref="Key"/> used when applying insights to the <see cref="Exception.Data"/> dictionary.
    /// </summary>
    public const string Key = "$(___exceptionInsights___)";

    /// <summary>
    /// Enriches and embed insights about an <see cref="Exception"/>.
    /// </summary>
    /// <typeparam name="T">The type of the <paramref name="exception"/>.</typeparam>
    /// <param name="exception">The exception to enrich.</param>
    /// <param name="runtimeParameters">The runtime parameters of the method that threw the <paramref name="exception"/>.</param>
    /// <param name="snapshots">A bitwise combination of the enumeration values that specify which areas of a system to capture.</param>
    /// <returns>The provided <paramref name="exception"/> enriched with an embedded entry of insights.</returns>
    public static T Embed<T>(T exception, object[] runtimeParameters = null, SystemSnapshots snapshots = SystemSnapshots.None) where T : Exception
    {
        return Embed(exception, null, runtimeParameters, snapshots);
    }

    /// <summary>
    /// Enriches and embed insights about an <see cref="Exception"/>.
    /// </summary>
    /// <typeparam name="T">The type of the <paramref name="exception"/>.</typeparam>
    /// <param name="exception">The exception to enrich.</param>
    /// <param name="thrower">The method that threw the <paramref name="exception"/>.</param>
    /// <param name="runtimeParameters">The optional runtime parameters of the <paramref name="thrower"/>.</param>
    /// <param name="snapshots">A bitwise combination of the enumeration values that specify which areas of a system to capture.</param>
    /// <returns>The provided <paramref name="exception"/> enriched with an embedded entry of insights.</returns>
    public static T Embed<T>(T exception, MethodBase thrower, object[] runtimeParameters = null, SystemSnapshots snapshots = SystemSnapshots.None) where T : Exception
    {
        Validator.ThrowIfNull(exception);
        var builder = new StringBuilder();
        var empty = EmptyBase64;
        if (thrower != null || exception.TargetSite != null)
        {
            var descriptor = new MethodDescriptor(thrower ?? exception.TargetSite);
            builder.Append(Convert.ToBase64String(Convertible.GetBytes(FormattableString.Invariant($"{descriptor.ToString()}"))));
            builder.Append('.');
            if (runtimeParameters != null && runtimeParameters.Length != 0)
            {
                var rp = DelimitedString.Create(MethodDescriptor.MergeParameters(descriptor, runtimeParameters), o =>
                {
                    o.StringConverter = pair => FormattableString.Invariant($"{pair.Key}={pair.Value ?? "null"}");
                    o.Delimiter = FormattableString.Invariant($"{Environment.NewLine}");
                });
                builder.Append(Convert.ToBase64String(Convertible.GetBytes(FormattableString.Invariant($"{rp}"))));
            }
            else
            {
                builder.Append(empty);
            }
        }
        else
        {
            builder.Append(empty);
            builder.Append('.');
            builder.Append(empty);
        }
        EmbedSystemSnapshot(builder, snapshots, empty);
        if (exception.Data[Key] == null) { exception.Data.Add(Key, builder.ToString()); }
        return exception;
    }

    private static void EmbedSystemSnapshot(StringBuilder builder, SystemSnapshots snapshots, string empty)
    {
        builder.Append('.');
        if (snapshots.HasFlag(SystemSnapshots.CaptureThreadInfo))
        {
            var ti = string.Join(Environment.NewLine, new ThreadInfo(Thread.CurrentThread).ToString().Split(Alphanumeric.CaretChar));
            builder.Append(ti.Length > 0 ? Convert.ToBase64String(Convertible.GetBytes(FormattableString.Invariant($"{ti}"))) : empty);
        }
        else
        {
            builder.Append(empty);
        }
        builder.Append('.');
        if (snapshots.HasFlag(SystemSnapshots.CaptureProcessInfo))
        {
            var pi = string.Join(Environment.NewLine, new ProcessInfo(Process.GetCurrentProcess()).ToString().Split(Alphanumeric.CaretChar));
            builder.Append(pi.Length > 0 ? Convert.ToBase64String(Convertible.GetBytes(FormattableString.Invariant($"{pi}"))) : empty);
        }
        else
        {
            builder.Append(empty);
        }
        builder.Append('.');
        if (snapshots.HasFlag(SystemSnapshots.CaptureEnvironmentInfo))
        {
            var ei = string.Join(Environment.NewLine, new EnvironmentInfo().ToString().Split(Alphanumeric.CaretChar));
            builder.Append(ei.Length > 0 ? Convert.ToBase64String(Convertible.GetBytes(FormattableString.Invariant($"{ei}"))) : empty);
        }
        else
        {
            builder.Append(empty);
        }
    }
}
