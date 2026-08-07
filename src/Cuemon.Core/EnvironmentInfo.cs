using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace Cuemon;
internal sealed class EnvironmentInfo
{
    public override string ToString()
    {
        var builder = new StringBuilder();
        try
        {
            var targetPlatform = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName ?? RuntimeInformation.FrameworkDescription;
            builder.Append(FormattableString.Invariant($"CommandLine: {Environment.CommandLine}"));
            builder.Append(Alphanumeric.CaretChar);
            builder.Append(FormattableString.Invariant($"Is64BitOperatingSystem: {Environment.Is64BitOperatingSystem}"));
            builder.Append(Alphanumeric.CaretChar);
            builder.Append(FormattableString.Invariant($"Is64BitProcess: {Environment.Is64BitProcess}"));
            builder.Append(Alphanumeric.CaretChar);
            builder.Append(FormattableString.Invariant($"MachineName: {Environment.MachineName}"));
            builder.Append(Alphanumeric.CaretChar);
            builder.Append(FormattableString.Invariant($"OperatingSystem: {Environment.OSVersion}"));
            builder.Append(Alphanumeric.CaretChar);
            builder.Append(FormattableString.Invariant($"ProcessorCount: {Environment.ProcessorCount}"));
            builder.Append(Alphanumeric.CaretChar);
            builder.Append(FormattableString.Invariant($"TargetPlatform: {targetPlatform}"));
        }
        catch (Exception)
        {
            // ignore platform exceptions and the likes hereof
        }
        return builder.ToString();
    }
}
