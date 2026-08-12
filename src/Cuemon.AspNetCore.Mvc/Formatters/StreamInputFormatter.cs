using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Configuration;
using Cuemon.Reflection;
using Cuemon.Runtime.Serialization.Formatters;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Cuemon.AspNetCore.Mvc.Formatters;
/// <summary>
/// Provides a way to read an object from a request body with a text format with the constraint that <typeparamref name="TFormatter"/> must be assignable from <see cref="Formatter{Stream}"/>.
/// Implements the <see cref="ConfigurableInputFormatter{TOptions}" />
/// </summary>
/// <typeparam name="TFormatter">The type of the <seealso cref="Formatter{TFormat}"/>.</typeparam>
/// <typeparam name="TOptions">The type of the configured options.</typeparam>
public abstract class StreamInputFormatter<TFormatter, TOptions> : ConfigurableInputFormatter<TOptions>
    where TFormatter : Formatter<Stream>
    where TOptions : class, IParameterObject, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StreamInputFormatter{TFormatter, TOptions}"/> class.
    /// </summary>
    /// <param name="options">The <typeparamref name="TOptions"/> which need to be configured.</param>
    protected StreamInputFormatter(TOptions options) : base(options)
    {
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    /// <summary>
    /// Reads an object from the request body.
    /// </summary>
    /// <param name="context">The <see cref="InputFormatterContext" />.</param>
    /// <param name="encoding">The <see cref="Encoding" /> used to read the request body.</param>
    /// <returns>A <see cref="Task" /> that on completion deserializes the request body.</returns>
    /// <remarks>In this implementation <paramref name="encoding"/> is disregarded.</remarks>
    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
    {
        Validator.ThrowIfNull(context);
        var requestBody = new MemoryStream();
        await context.HttpContext.Request.BodyReader.CopyToAsync(requestBody).ConfigureAwait(false);
        requestBody.Position = 0;
        var formatter = ActivatorFactory.CreateInstance<TOptions, TFormatter>(Options);
        var deserializedObject = formatter.Deserialize(requestBody, context.ModelType);
        context.HttpContext.Items.Add(HttpRequestEvidence.HttpContextItemsKeyForCapturedRequestBody, requestBody);
        return await InputFormatterResult.SuccessAsync(deserializedObject).ConfigureAwait(false);
    }
}
