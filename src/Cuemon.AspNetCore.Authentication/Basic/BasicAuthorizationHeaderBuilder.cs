namespace Cuemon.AspNetCore.Authentication.Basic;
/// <summary>
/// Provides a way to fluently represent a HTTP Basic Authentication header.
/// </summary>
public class BasicAuthorizationHeaderBuilder : AuthorizationHeaderBuilder<BasicAuthorizationHeader, BasicAuthorizationHeaderBuilder>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BasicAuthorizationHeaderBuilder"/> class.
    /// </summary>
    public BasicAuthorizationHeaderBuilder() : base(BasicAuthorizationHeader.Scheme)
    {
        MapRelation(nameof(AddUserName), BasicFields.UserName);
        MapRelation(nameof(AddPassword), BasicFields.Password);
    }


    /// <summary>
    /// Adds the credential scope that defines the remote resource.
    /// </summary>
    /// <param name="username">The credential scope that defines the remote resource.</param>
    /// <returns>An <see cref="BasicAuthorizationHeaderBuilder"/> that can be used to further build the HTTP HMAC Authentication header.</returns>
    public BasicAuthorizationHeaderBuilder AddUserName(string username)
    {
        return AddOrUpdate(BasicFields.UserName, username);
    }

    /// <summary>
    /// Adds the client identifier that is the public key of the signing process.
    /// </summary>
    /// <param name="password">The client identifier that is the public key of the signing process.</param>
    /// <returns>An <see cref="BasicAuthorizationHeaderBuilder"/> that can be used to further build the HTTP HMAC Authentication header.</returns>
    public BasicAuthorizationHeaderBuilder AddPassword(string password)
    {
        return AddOrUpdate(BasicFields.Password, password);
    }

    /// <summary>
    /// Builds an instance of <see cref="BasicAuthorizationHeader"/> that implements <see cref="AuthorizationHeader" />.
    /// </summary>
    /// <returns>An instance of <see cref="BasicAuthorizationHeader"/>.</returns>
    public override BasicAuthorizationHeader Build()
    {
        ValidateData(BasicFields.UserName, BasicFields.Password);
        return new BasicAuthorizationHeader(Data[BasicFields.UserName], Data[BasicFields.Password]);
    }
}
