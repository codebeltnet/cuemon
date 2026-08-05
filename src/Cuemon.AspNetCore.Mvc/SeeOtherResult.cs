using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Mvc
{
    /// <summary>
    /// An <see cref="ActionResult" /> that returns a SeeOther (303) response with a Location header to the supplied URL.
    /// </summary>
    public class SeeOtherResult : StatusCodeResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeeOtherResult"/> class.
        /// </summary>
        /// <param name="location">The location of the URL to redirect to.</param>
        public SeeOtherResult(Uri location) : base(StatusCodes.Status303SeeOther)
        {
            Validator.ThrowIfNull(location);
            Location = location;
        }

        /// <summary>
        /// Gets the location of the URL to redirect to.
        /// </summary>
        /// <value>The location of the URL to redirect to.</value>
        public Uri Location { get; }

        /// <summary>
        /// Executes the result operation of the action method asynchronously. This method is called by MVC to process the result of an action method.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes information about the action that was executed and request information.</param>
        /// <returns>A task that represents the asynchronous execute operation.</returns>
        public override Task ExecuteResultAsync(ActionContext context)
        {
            Validator.ThrowIfNull(context);
            context.HttpContext.Response.Headers[HeaderNames.Location] = Location.OriginalString;
            return base.ExecuteResultAsync(context);
        }
    }
}