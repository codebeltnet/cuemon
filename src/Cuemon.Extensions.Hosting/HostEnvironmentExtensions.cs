using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Hosting;

/// <summary>
/// Extension methods for the <see cref="IHostEnvironment"/> interface.
/// </summary>
public static class HostEnvironmentExtensions
{
    /// <summary>
    /// Determines whether the specified <paramref name="environment"/> is equal to <c>LocalDevelopment</c>.
    /// </summary>
    /// <param name="environment">The <see cref="IHostingEnvironment"/> to extend.</param>
    /// <returns><c>true</c> if <paramref name="environment"/> is <c>LocalDevelopment</c>; otherwise <c>false</c></returns>
    public static bool IsLocalDevelopment(this IHostEnvironment environment)
    {
        return environment.IsEnvironment(Environments.LocalDevelopment);
    }

    /// <summary>
    /// Determines whether the specified <paramref name="environment"/> is different from <c>Production</c>.
    /// </summary>
    /// <param name="environment">The <see cref="IHostingEnvironment"/> to extend.</param>
    /// <returns><c>true</c> if <paramref name="environment"/> is not <c>Production</c>; otherwise <c>false</c></returns>
    public static bool IsNonProduction(this IHostEnvironment environment)
    {
        return !environment.IsProduction();
    }
}
