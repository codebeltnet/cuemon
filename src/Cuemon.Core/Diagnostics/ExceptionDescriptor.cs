using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cuemon.Collections.Generic;
using Cuemon.Text;

namespace Cuemon.Diagnostics;
/// <summary>
/// Provides information about an <see cref="Exception"/>, in a developer friendly way.
/// </summary>
public class ExceptionDescriptor
{
    private readonly IDictionary<string, object> _evidence;
    private readonly Lazy<IReadOnlyDictionary<string, object>> _lazyEvidence;
    private const int IndexOfThrower = 0;
    private const int IndexOfRuntimeParameters = 1;
    private const int IndexOfThreadInfo = 2;
    private const int IndexOfProcessInfo = 3;
    private const int IndexOfEnvironmentInfo = 4;

    /// <summary>
    /// Converts the specified <paramref name="exception"/> to a developer friendly <see cref="ExceptionDescriptor"/>.
    /// </summary>
    /// <param name="exception">The <see cref="Exception"/> to convert.</param>
    /// <param name="code">The error code that uniquely identifies the type of failure. Default is "UnhandledException".</param>
    /// <param name="message">The message that explains the reason for the failure. Default is "An unhandled exception occurred.".</param>
    /// <param name="helpLink">The optional link to a help page associated with this failure.</param>
    /// <returns>An <see cref="ExceptionDescriptor"/> initialized with the provided arguments, and (if embedded) enriched with insights of the <paramref name="exception"/>.</returns>
    /// <remarks>Any meta data embedded by <see cref="ExceptionInsights.Embed{T}(T,object[],SystemSnapshots)"/> will be extracted and added as evidence to the final result.</remarks>
    public static ExceptionDescriptor Extract(Exception exception, string code = "UnhandledException", string message = "An unhandled exception occurred.", Uri helpLink = null)
    {
        Validator.ThrowIfNull(exception);
        var ed = new ExceptionDescriptor(exception, code, message, helpLink);
        var base64Segments = RetrieveAndSweepExceptionData(exception);
        if (!string.IsNullOrWhiteSpace(base64Segments))
        {
            var insights = base64Segments.Split('.');
            if (insights.Length == 5)
            {
                var memberSignature = Convertible.ToString(Convert.FromBase64String(insights[IndexOfThrower]));
                var runtimeParameters = Convertible.ToString(Convert.FromBase64String(insights[IndexOfRuntimeParameters]));
                ed.AddEvidence("Thrower", new MemberEvidence(memberSignature, string.IsNullOrWhiteSpace(runtimeParameters) ? null : runtimeParameters.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToDictionary(k => k.Substring(0, k.IndexOf('=')), v =>
                {
                    var t = v.Substring(v.IndexOf('=') + 1);
                    if (t == "null") { return null; }
                    return t;
                })), evidence => evidence);
                TryAddEvidence(ed, "Thread", insights[IndexOfThreadInfo]);
                TryAddEvidence(ed, "Process", insights[IndexOfProcessInfo]);
                TryAddEvidence(ed, "Environment", insights[IndexOfEnvironmentInfo]);
            }
        }
        return ed;
    }

    private static string RetrieveAndSweepExceptionData(Exception exception)
    {
        string result = null;
        var stack = new Stack<Exception>();
        stack.Push(exception);
        while (stack.Count > 0)
        {
            var e = stack.Pop();
            if (e.Data[ExceptionInsights.Key] is string base64Segments && !string.IsNullOrWhiteSpace(base64Segments))
            {
                result ??= base64Segments;
                e.Data.Remove(ExceptionInsights.Key);
            }
            if (e.InnerException != null) { stack.Push(e.InnerException); }
        }
        return result;
    }

    private static void TryAddEvidence(ExceptionDescriptor ed, string context, string base64)
    {
        var info = Convertible.ToString(Convert.FromBase64String(base64));
        if (!string.IsNullOrWhiteSpace(info))
        {
            ed.AddEvidence(context, info.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries), s => s.ToDictionary(k => k.Substring(0, k.IndexOf(':')), v =>
            {
                var presult = v.Substring(v.IndexOf(' ') + 1);
                return ParserFactory.FromValueType().Parse(presult == "null" ? null : presult);
            }));
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionDescriptor"/> class.
    /// </summary>
    protected ExceptionDescriptor()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionDescriptor" /> class.
    /// </summary>
    /// <param name="failure">The <see cref="Exception"/> that caused the current failure.</param>
    /// <param name="code">The error code that uniquely identifies the type of failure.</param>
    /// <param name="message">The message that explains the reason for the failure.</param>
    /// <param name="helpLink">The optional link to a help page associated with this failure.</param>
    /// <remarks><paramref name="code"/> will remove any spaces that might be present.</remarks>
    public ExceptionDescriptor(Exception failure, string code, string message, Uri helpLink = null)
    {
        Validator.ThrowIfNull(failure);
        Validator.ThrowIfNullOrWhitespace(message);
        Code = StringReplacePair.RemoveAll(code, ' ');
        Message = message;
        HelpLink = helpLink;
        Failure = failure;
        _evidence = new Dictionary<string, object>();
        _lazyEvidence = new Lazy<IReadOnlyDictionary<string, object>>(() => new ReadOnlyDictionary<string, object>(_evidence));
    }

    /// <summary>
    /// Gets a collection of key/value pairs that can provide additional information about the failure.
    /// </summary>
    /// <value>An optional collection of key/value pairs that can provide additional information about the failure.</value>
    public IReadOnlyDictionary<string, object> Evidence => _lazyEvidence.Value;

    /// <summary>
    /// Adds an element of evidence to associate with the faulted operation.
    /// </summary>
    /// <typeparam name="T">The type of the <paramref name="evidence"/>.</typeparam>
    /// <param name="context">The context of the evidence.</param>
    /// <param name="evidence">The evidence itself.</param>
    /// <param name="evidenceProvider">The function delegate that provides the evidence.</param>
    public void AddEvidence<T>(string context, T evidence, Func<T, object> evidenceProvider)
    {
        if (_evidence.ContainsKey(context)) { return; }
        _evidence.Add(context, evidenceProvider?.Invoke(evidence));
    }

    /// <summary>
    /// Gets an error code that uniquely identifies the type of failure.
    /// </summary>
    /// <value>The number that identifies the type of failure.</value>
    public string Code { get; private set; }

    /// <summary>
    /// Gets a message that describes the current failure.
    /// </summary>
    /// <value>The message that explains the reason for the failure.</value>
    public string Message { get; private set; }

    /// <summary>
    /// Gets or sets a link to the help page associated with this failure.
    /// </summary>
    /// <value>The location of an optional help page associated with this failure.</value>
    public Uri HelpLink { get; set; }

    /// <summary>
    /// Gets the <see cref="Exception"/> that caused the current failure.
    /// </summary>
    /// <value>The deeper cause of the failure.</value>
    public Exception Failure { get; }

    /// <summary>
    /// Post initialize this instance with the specified <paramref name="attribute"/>.
    /// </summary>
    public void PostInitializeWith(ExceptionDescriptorAttribute attribute)
    {
        PostInitializeWith(Arguments.Yield(attribute));
    }

    /// <summary>
    /// Post initialize this instance with a matching <see cref="ExceptionDescriptorAttribute"/> from the specified <paramref name="attributes"/>.
    /// </summary>
    /// <param name="attributes">The attributes to find a match within.</param>
    public void PostInitializeWith(IEnumerable<ExceptionDescriptorAttribute> attributes)
    {
        var attribute = attributes?.SingleOrDefault(eda => eda.FailureType == Failure.GetType());
        if (attribute != null)
        {
            if (!string.IsNullOrWhiteSpace(attribute.Code)) { Code = attribute.Code; }
            if (!string.IsNullOrWhiteSpace(attribute.Message)) { Message = attribute.Message; }
            if (!string.IsNullOrWhiteSpace(attribute.HelpLink)) { HelpLink = new Uri(attribute.HelpLink); }
        }
    }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return Failure.ToString();
    }
}
