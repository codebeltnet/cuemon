using System.Security.Claims;

namespace Cuemon.AspNetCore.Authentication.Hmac;
/// <summary>
/// Represents the method that defines an Authenticator typically assigned on <see cref="HmacAuthenticationOptions"/>.
/// </summary>
/// <param name="clientId">The public key to match and lookup the paired shared <paramref name="clientSecret"/>.</param>
/// <param name="clientSecret">The shared secret-private key paired with <paramref name="clientId"/>.</param>
/// <returns>A <see cref="ClaimsPrincipal"/> that is associated with the result of <paramref name="clientSecret"/>.</returns>
public delegate ClaimsPrincipal HmacAuthenticator(string clientId, out string clientSecret);
