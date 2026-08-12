using System.Security.Claims;

namespace Cuemon.AspNetCore.Authentication.Basic;
/// <summary>
/// Represents the method that defines an Authenticator typically assigned on <see cref="BasicAuthenticationOptions"/>.
/// </summary>
/// <param name="username">The username that must be paired with <paramref name="password"/>.</param>
/// <param name="password">The password that must be paired with <paramref name="username"/>.</param>
/// <returns>A <see cref="ClaimsPrincipal"/> that is associated with the result of <paramref name="username"/> and <paramref name="password"/>.</returns>
public delegate ClaimsPrincipal BasicAuthenticator(string username, string password);
