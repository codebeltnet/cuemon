using System;
using System.IO;
using Cuemon.AspNetCore.Http;
using Cuemon.Configuration;
using Cuemon.Data.Integrity;
using Cuemon.IO;
using Cuemon.Security;
using Cuemon.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable;
/// <summary>
/// Specifies options that is related to the <see cref="HttpEntityTagHeaderFilter" />.
/// </summary>
/// <seealso cref="HttpCacheableFilter"/>
public class HttpEntityTagHeaderOptions : IParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpEntityTagHeaderOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="HttpEntityTagHeaderOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="EntityTagProvider"/></term>
    ///         <description><code>
    ///            (integrity, context) =>
    ///            {
    ///                 var builder = new ChecksumBuilder(integrity.Checksum.GetBytes(), () => HashFactory.CreateFnv128());
    ///                 Decorator.Enclose(context.Response).TryAddOrUpdateEntityTagHeader(context.Request, builder, integrity.Validation == EntityDataIntegrityValidation.Weak);
    ///            };
    ///         </code></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="EntityTagResponseParser"/></term>
    ///         <description><code>
    ///             (body, request, response) =>
    ///             {
    ///                 var ms = new MemoryStream();
    ///                 Decorator.Enclose(body).CopyStream(ms);
    ///                 var builder = new ChecksumBuilder(ms.ToArray(), () => UnkeyedHashFactory.CreateCryptoMd5());
    ///                 Decorator.Enclose(response).TryAddOrUpdateEntityTagHeader(request, builder);
    ///             };
    ///         </code></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="UseEntityTagResponseParser"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public HttpEntityTagHeaderOptions()
    {
        EntityTagProvider = (integrity, context) =>
        {
            var builder = new ChecksumBuilder(integrity.Checksum.GetBytes(), () => HashFactory.CreateFnv128());
            Decorator.Enclose(context.Response).AddOrUpdateEntityTagHeader(context.Request, builder, integrity.Validation == EntityDataIntegrityValidation.Weak);
        };
        EntityTagResponseParser = (body, request, response) =>
        {
            var ms = new MemoryStream();
            Decorator.Enclose(body).CopyStream(ms);
            var builder = new ChecksumBuilder(ms.ToArray(), () => UnkeyedHashFactory.CreateCryptoMd5());
            Decorator.Enclose(response).AddOrUpdateEntityTagHeader(request, builder);
        };
        UseEntityTagResponseParser = false;
    }

    /// <summary>
    /// Gets or sets a value indicating whether to use computation of the HTTP ETag header reading and copying the <seealso cref="HttpResponse.Body"/>.
    /// </summary>
    /// <value><c>true</c> to compute the HTTP ETag header from reading and copying the <seealso cref="HttpResponse.Body"/>; otherwise, <c>false</c>.</value>
    public bool UseEntityTagResponseParser { get; set; }

    /// <summary>
    /// Gets or sets the delegate that is invoked when a result of a <see cref="ResultExecutingContext"/> is an <see cref="ObjectResult"/> and the value is an <see cref="IEntityDataIntegrity"/> implementation.
    /// </summary>
    /// <value>The delegate that provides an HTTP ETag header.</value>
    public Action<IEntityDataIntegrity, HttpContext> EntityTagProvider { get; set; }

    /// <summary>
    /// Gets or sets the delegate that is invoked as a fallback from the <see cref="EntityTagProvider"/> when <see cref="UseEntityTagResponseParser"/> is set to <c>true</c>.
    /// </summary>
    /// <value>The delegate that computes a HTTP ETag from the <see cref="HttpResponse.Body"/>.</value>
    public Action<Stream, HttpRequest, HttpResponse> EntityTagResponseParser { get; set; }

    /// <summary>
    /// Gets a value indicating whether this instance has an <see cref="EntityTagProvider"/>.
    /// </summary>
    /// <value><c>true</c> if this instance has an <see cref="EntityTagProvider"/>; otherwise, <c>false</c>.</value>
    public bool HasEntityTagProvider => EntityTagProvider != null;

    /// <summary>
    /// Gets a value indicating whether this instance has an <see cref="EntityTagResponseParser"/>.
    /// </summary>
    /// <value><c>true</c> if this instance has an <see cref="EntityTagResponseParser"/>; otherwise, <c>false</c>.</value>
    public bool HasEntityTagResponseParser => EntityTagResponseParser != null;
}
