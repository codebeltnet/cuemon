using System.Text.Json;

namespace Cuemon.Extensions.Text.Json;
/// <summary>
/// Extension methods for the <see cref="JsonNamingPolicy"/> class.
/// </summary>
public static class JsonNamingPolicyExtensions
{
    /// <summary>
    /// Returns the specified <paramref name="name"/> following the <see cref="JsonNamingPolicy"/>.
    /// </summary>
    /// <param name="policy">The policy to apply.</param>
    /// <param name="name">The name to apply to a JSON property.</param>
    /// <returns>When <paramref name="policy"/> is null, the specified <paramref name="name"/> is returned unaltered; otherwise it is converted according to the <see cref="JsonNamingPolicy"/>.</returns>
    public static string DefaultOrConvertName(this JsonNamingPolicy policy, string name)
    {
        return policy == null ? name : policy.ConvertName(name);
    }
}
