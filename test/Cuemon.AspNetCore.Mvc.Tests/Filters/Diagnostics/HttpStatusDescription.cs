using System;
using System.Net.Http;
using Cuemon.Reflection;
using Microsoft.AspNetCore.WebUtilities;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
public static class HttpStatusDescription
{
    public static string Get(int statusCode) // bug in Microsoft internals; they use HttpStatusDescription.Get instead of ReasonPhrases.GetReasonPhrase why we need to make a wrapper using reflection :-/
    {
        var httpStatusDescription = typeof(HttpResponseMessage).Assembly.GetType("System.Net.HttpStatusDescription");
        var getMethod = httpStatusDescription?.GetMethod("Get", MemberReflection.Everything, null, new Type[] { typeof(int) }, null);
        return getMethod?.Invoke(null, new object[] { statusCode }) as string ?? ReasonPhrases.GetReasonPhrase(statusCode);
    }
}
