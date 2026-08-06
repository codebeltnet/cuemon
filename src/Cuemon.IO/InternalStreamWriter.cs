using System;
using System.IO;

namespace Cuemon.IO;
internal sealed class InternalStreamWriter : StreamWriter
{
    internal InternalStreamWriter(Stream output, StreamWriterOptions options) : base(output, options.Encoding, options.BufferSize, options.LeaveOpen)
    {
        FormatProvider = options.FormatProvider;
        AutoFlush = options.AutoFlush;
        NewLine = options.NewLine;
    }

    public override IFormatProvider FormatProvider { get; }
}
