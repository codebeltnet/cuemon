namespace Cuemon.AspNetCore.Authentication.Basic;
/// <summary>
/// A collection of constants for <see cref="BasicAuthorizationHeaderBuilder"/>.
/// </summary>
public static class BasicFields
{
    /// <summary>
    /// The realm field of a HTTP Basic access authentication.
    /// </summary>
    public const string Realm = "realm";

    /// <summary>
    /// The username of the <see cref="Credentials"/>.
    /// </summary>
    public const string UserName = "username";

    /// <summary>
    /// The password of the <see cref="Credentials"/>.
    /// </summary>
    public const string Password = "password";

    /// <summary>
    /// The credentials of the HTTP Basic access authentication.
    /// </summary>
    public const string Credentials = "credentials";
}
