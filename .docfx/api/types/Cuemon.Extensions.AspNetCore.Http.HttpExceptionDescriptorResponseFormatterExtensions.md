---
uid: Cuemon.Extensions.AspNetCore.Http.HttpExceptionDescriptorResponseFormatterExtensions
example:
- *content
---

The following example demonstrates how to project all exception descriptor handlers from a sequence of formatters into a single enumerable sequence.

```csharp
using System.Collections.Generic;
using System.Linq;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Http;

namespace Examples;

public class FormatterHandlerProjection
{
    public IEnumerable<HttpExceptionDescriptorResponseHandler> GetAllHandlers(IEnumerable<IHttpExceptionDescriptorResponseFormatter> formatters)
    {
        return formatters.SelectExceptionDescriptorHandlers();

}
}

```
