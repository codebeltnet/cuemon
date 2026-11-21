---
uid: Cuemon.Data.UniqueIndexViolationException
example:
- *content
---

`UniqueIndexViolationException` represents a unique index or constraint violation error, with support for inner exceptions and parameterless construction. This example throws a new instance with a descriptive message about a duplicate key in `dbo.Users` and catches it to print the message. It also creates a wrapped exception with an inner `InvalidOperationException` as the cause, and demonstrates that the default parameterless constructor has no inner exception.

```csharp
// Program.cs
using System;
using Cuemon.Data;

try
{
    throw new UniqueIndexViolationException("Cannot insert duplicate key row in object 'dbo.Users'.");
}
catch (UniqueIndexViolationException ex)
{
    Console.WriteLine(ex.Message);
}

var wrapped = new UniqueIndexViolationException(
    "Failed to register user.",
    new InvalidOperationException("IX_Users_Email was violated."));

Console.WriteLine(wrapped.Message);
Console.WriteLine(wrapped.InnerException?.Message);

var empty = new UniqueIndexViolationException();
Console.WriteLine($"Default exception has no inner exception: {empty.InnerException is null}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="ca3a997bc3bdcb81405e2bed"><a href="~/run-actions/ca3a997bc3bdcb81405e2bed/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/ca3a997bc3bdcb81405e2bed/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
