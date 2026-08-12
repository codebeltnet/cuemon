using System.Collections.Generic;

namespace Cuemon.Diagnostics;
/// <summary>
/// Provides evidence about a member.
/// </summary>
public class MemberEvidence
{
    internal MemberEvidence(string memberSignature, IDictionary<string, string> runtimeParameters)
    {
        Validator.ThrowIfNullOrWhitespace(memberSignature);
        MemberSignature = memberSignature;
        RuntimeParameters = runtimeParameters ?? new Dictionary<string, string>();
    }

    /// <summary>
    /// Gets the member signature.
    /// </summary>
    /// <value>The signature of the member.</value>
    public string MemberSignature { get; }

    /// <summary>
    /// Gets the runtime parameters of the member.
    /// </summary>
    /// <value>The runtime parameters of the member.</value>
    public IDictionary<string, string> RuntimeParameters { get; }
}
