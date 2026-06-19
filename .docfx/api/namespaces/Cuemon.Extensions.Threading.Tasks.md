---
uid: Cuemon.Extensions.Threading.Tasks
summary: *content
---
Control the synchronization context in async task continuations explicitly with `ContinueWithCapturedContext` (resume on original context) and `ContinueWithSuppressedContext` (suppress context). Use this namespace when you need predictable async behavior in libraries without manual `ConfigureAwait` calls. Start with `ContinueWithSuppressedContext` on `Task` to avoid deadlocks in synchronous blocking patterns.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [System.Threading.Tasks namespace](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks) 🔗

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|Task<TResult>|⬇️|`ContinueWithCapturedContext<TResult>`, `ContinueWithSuppressedContext<TResult>`|
|Task|⬇️|`ContinueWithCapturedContext`, `ContinueWithSuppressedContext`|
