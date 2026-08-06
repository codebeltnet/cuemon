using System;

namespace Cuemon.Net.Http;
/// <summary>
/// Defines the official HTTP data transfer method (such as GET, POST, or HEAD) used by the client to query a web server.
/// </summary>
/// <remarks>
/// These are the official HTTP methods as specified in RFC 2616, section 9 (except for the CONNECT method).<br/>
/// RFC 2616: http://www.w3.org/Protocols/rfc2616/rfc2616.html, RFC 2616 section 9: http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html#sec9.<br/>
/// Includes RFC 5789: https://tools.ietf.org/html/rfc5789 also; Patch, as some public APIs has started using this.
/// <br/><br/>
/// This enumeration has a <see cref="FlagsAttribute"/> that allows a bitwise combination of its member values.
/// </remarks>
[Flags]
public enum HttpMethods
{
    /// <summary>
    /// Represents an HTTP OPTIONS protocol method.
    /// </summary>
    Options = 1,
    /// <summary>
    /// Represents an HTTP GET protocol method.
    /// </summary>
    Get = 2,
    /// <summary>
    /// Represents an HTTP HEAD protocol method. The HEAD method is identical to GET except that the server only returns message-headers in the response, without a message-body.
    /// </summary>
    Head = 4,
    /// <summary>
    /// Represents an HTTP POST protocol method that is used to post a new entity as an addition to a URI.
    /// </summary>
    Post = 8,
    /// <summary>
    /// Represents an HTTP PUT protocol method that is used to replace an entity identified by a URI.
    /// </summary>
    Put = 16,
    /// <summary>
    /// Represents an HTTP DELETE protocol method.
    /// </summary>
    Delete = 32,
    /// <summary>
    /// Represents an HTTP TRACE protocol method.
    /// </summary>
    Trace = 64,
    /// <summary>
    /// Represents an HTTP PATCH protocol method.
    /// </summary>
    Patch = 128
}
