using System;
using System.Text;
using System.Threading;

namespace Cuemon.Threading;
internal sealed class ThreadInfo
{
    internal ThreadInfo(Thread thread = null)
    {
        Thread = thread ?? Thread.CurrentThread;
    }

    private Thread Thread { get; }

    public override string ToString()
    {
        var builder = new StringBuilder();
        try
        {
            builder.Append(FormattableString.Invariant($"Culture: {Thread.CurrentCulture}"));
            builder.Append(Alphanumeric.CaretChar);
            builder.Append(FormattableString.Invariant($"UICulture: {Thread.CurrentUICulture}"));
            builder.Append(Alphanumeric.CaretChar);
            builder.Append(FormattableString.Invariant($"ManagedId: {Thread.ManagedThreadId}"));
            builder.Append(Alphanumeric.CaretChar);
            builder.Append(FormattableString.Invariant($"Name: {Thread.Name ?? "null"}"));
            builder.Append(Alphanumeric.CaretChar);
            builder.Append(FormattableString.Invariant($"State: {Thread.ThreadState}"));
            builder.Append(Alphanumeric.CaretChar);
            builder.Append(FormattableString.Invariant($"Priority: {Thread.Priority}"));
            builder.Append(Alphanumeric.CaretChar);
            builder.Append(FormattableString.Invariant($"IsThreadPoolThread: {Thread.IsThreadPoolThread}"));
            builder.Append(Alphanumeric.CaretChar);
            builder.Append(FormattableString.Invariant($"IsBackground: {Thread.IsBackground}"));
        }
        catch (Exception)
        {
            // ignore platform exceptions and the likes hereof
        }
        return builder.ToString();
    }
}
