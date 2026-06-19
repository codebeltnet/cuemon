---
uid: Cuemon.Extensions.AspNetCore.Http.HttpRequestExtensions
example:
- *content
---

The following example demonstrates how to use HttpRequestExtensions to inspect HTTP request properties such as accepted MIME types, HTTP method checks, and client-side caching status using ETags and Last-Modified headers.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Data.Integrity;
using Cuemon.Extensions.AspNetCore.Http;
using Cuemon.Security;
using Microsoft.AspNetCore.Http;

namespace MyApp.AspNetCore.Http
{
    public class HttpRequestExtensionsExample
    {
        public void Demonstrate(HttpRequest request)
        {
            // Get ordered MIME types from the Accept header by quality value
            request.Headers["Accept"] = "text/html;q=0.8, application/json;q=0.9, */*;q=0.1";
            IEnumerable<string> preferredTypes = request.AcceptMimeTypesOrderedByQuality();
            Console.WriteLine(string.Join(", ", preferredTypes));
            // Output: "application/json, text/html, */*" (sorted by q-value descending)

            // Check if the request uses GET or HEAD method
            request.Method = "GET";
            bool isGetOrHead = request.IsGetOrHeadMethod();
            Console.WriteLine(isGetOrHead); // True

            request.Method = "POST";
            isGetOrHead = request.IsGetOrHeadMethod();
            Console.WriteLine(isGetOrHead); // False

            // Check if the client has a cached version using If-None-Match (ETag)
            request.Method = "GET";
            request.Headers["If-None-Match"] = "\"abc123\"";
            var checksumBuilder = new ChecksumBuilder(() => HashFactory.CreateFnv128());
            bool isCached = request.IsClientSideResourceCached(checksumBuilder);
            Console.WriteLine(isCached); // True or False depending on checksum match

            // Check if the client has a cached version using If-Modified-Since
            request.Headers["If-Modified-Since"] = "Tue, 15 Jun 2024 10:00:00 GMT";
            var lastModified = new DateTime(2024, 6, 15, 9, 0, 0, DateTimeKind.Utc);
            isCached = request.IsClientSideResourceCached(lastModified);
            Console.WriteLine(isCached); // False (resource modified after If-Modified-Since date)

}}
}

```
