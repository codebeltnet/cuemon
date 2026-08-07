using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cuemon.Configuration;
using Cuemon.IO;
using Cuemon.Reflection;
using Cuemon.Runtime.Serialization.Formatters;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Cuemon.AspNetCore.Mvc.Formatters;
/// <summary>
/// Provides a way to write an object in a given text format to the output stream with the constraint that <typeparamref name="TFormatter"/> must be assignable from <see cref="Formatter{Stream}"/>.
/// Implements the <see cref="ConfigurableOutputFormatter{TOptions}" />
/// </summary>
/// <typeparam name="TFormatter">The type of the <seealso cref="Formatter{TFormat}"/>.</typeparam>
/// <typeparam name="TOptions">The type of the configured options.</typeparam>
public abstract class StreamOutputFormatter<TFormatter, TOptions> : ConfigurableOutputFormatter<TOptions>
    where TFormatter : Formatter<Stream>
    where TOptions : class, IParameterObject, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StreamOutputFormatter{TFormatter, TOptions}"/> class.
    /// </summary>
    /// <param name="options">The <typeparamref name="TOptions"/> which need to be configured.</param>
    protected StreamOutputFormatter(TOptions options) : base(options)
    {
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    /// <summary>
    /// write response body as an asynchronous operation.
    /// </summary>
    /// <param name="context">The formatter context associated with the call.</param>
    /// <param name="selectedEncoding">The <see cref="Encoding" /> that should be used to write the response.</param>
    /// <returns>A <see cref="Task" /> which can write the response body.</returns>
    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        Validator.ThrowIfNull(context);
        Validator.ThrowIfNull(selectedEncoding);
        var value = context.Object;
        if (value == null) { return; }
        using (var textWriter = context.WriterFactory(context.HttpContext.Response.Body, selectedEncoding))
        {
            var formatter = ActivatorFactory.CreateInstance<TOptions, TFormatter>(Options);
            using (var streamReader = new StreamReader(formatter.Serialize(value), selectedEncoding))
            {
                await Decorator.Enclose(streamReader).CopyToAsync(textWriter).ConfigureAwait(false);
            }
            await textWriter.FlushAsync().ConfigureAwait(false);
        }
    }
}
