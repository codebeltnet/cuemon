using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Cuemon.Diagnostics;
internal sealed class ProcessInfo
{
    internal ProcessInfo(Process process = null)
    {
        Process = process ?? Process.GetCurrentProcess();
    }

    private Process Process { get; }

    public override string ToString()
    {
        var builder = new StringBuilder();
        try
        {
            builder.Append(FormattableString.Invariant($"Id: {Process.Id}"));
            builder.Append(Alphanumeric.CaretChar);
            builder.Append(FormattableString.Invariant($"Name: {Process.ProcessName}"));
            builder.Append(Alphanumeric.CaretChar);
            builder.Append(FormattableString.Invariant($"PriorityClass: {Process.PriorityClass}"));
            builder.Append(Alphanumeric.CaretChar);
            builder.Append(FormattableString.Invariant($"HandleCount: {Process.HandleCount}"));
            builder.Append(Alphanumeric.CaretChar);
            builder.Append(FormattableString.Invariant($"ThreadCount: {Process.Threads.Count}"));
            builder.Append(Alphanumeric.CaretChar);
            builder.Append(FormattableString.Invariant($"WorkingSet64: {Process.WorkingSet64.ToString(CultureInfo.InvariantCulture)}"));
            builder.Append(Alphanumeric.CaretChar);
            builder.Append(FormattableString.Invariant($"TotalWorkingSet64: {Process.GetProcesses().Select(p => p.WorkingSet64).Sum().ToString(CultureInfo.InvariantCulture)}"));
            builder.Append(Alphanumeric.CaretChar);
            builder.Append(FormattableString.Invariant($"TotalProcessorTime: {Process.TotalProcessorTime.ToString("G", CultureInfo.InvariantCulture)}"));
        }
        catch (Exception)
        {
            // ignore platform exceptions and the likes hereof
        }
        return builder.ToString();
    }
}
