---
uid: Cuemon
summary: *content
---
The `Cuemon` namespace is the root of the Cuemon for .NET framework. It provides foundational types — value types, reference types, factories, utility classes, interfaces, attributes, and rich delegates — that underpin every Cuemon package. Use these types when you need common utilities like `Decorator`, `DateSpan`, `StringUtility`, or the functional delegates that enable a more expressive coding style.

If you are new to Cuemon for .NET, start with `Decorator<T>` for wrapping and extending existing instances, `DateSpan` for human-readable date ranges, or the `Condition` delegate for predicate-based control flow. For extension methods on core .NET types, see the [Cuemon.Extensions namespace](/api/extensions/dotnet/Cuemon.Extensions.html).

For mutable tuple scenarios, start with `MutableTuple<T1>` — the one-arity variant that serves as the anchor for all arity levels. Higher-arity variants (`MutableTuple<T1, T2>` through `MutableTuple<T1, ..., T20>`) follow the same pattern; choose the arity that matches your number of tuple elements.

For Try-style delegate scenarios, start with `TesterFunc<TResult, TSuccess>` — the two-arity variant that serves as the anchor for the full family. Higher-arity variants follow the same pattern; choose the arity that matches your number of input parameters plus the result and success type parameters.

For exception handling scenarios, start with `ExceptionHandler<T>` (one type parameter) or `ExceptionHandler<T, T>` (two type parameters). The one-parameter variant handles a single exception type, while the two-parameter variant handles distinct input and exception types. Choose the variant that matches your handler's input and exception requirements.

For exception invocation scenarios, start with `ExceptionInvoker<T>` to invoke operations with one type parameter or `ExceptionInvoker<T1, T2>` for two type parameters. Higher-arity variants follow the same pattern; choose the arity that matches your exception handler's needs.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [System namespace](https://docs.microsoft.com/en-us/dotnet/api/system) 🔗

Related: [Cuemon.Extensions namespace](/api/extensions/dotnet/Cuemon.Extensions.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IDecorator<Byte[]>|⬇️|`ToEncodedString`, `ToHexadecimalString`, `ToBinaryString`, `ToUrlEncodedBase64String`, `ToBase64String`, `TryDetectUnicodeEncoding`, `ToStream`|
|IDecorator<IEnumerable<Char>>|⬇️|`ToEnumerable`, `ToStringEquivalent`|
|IDecorator<DateTime>|⬇️|`GetUnixEpoch`, `ToUnixEpochTime`, `ToUtcKind`, `ToLocalKind`, `ToDefaultKind`|
|IDecorator<Delegate>|⬇️|`ResolveDelegateInfo`|
|IDecorator<Double>|⬇️|`ToTimeSpan`|
|IDecorator<Exception>|⬇️|`Flatten`|
|IDecorator<Int32>|⬇️|`Max`, `Min`, `IsPrime`, `IsCountableSequence`, `IsEven`, `IsOdd`|
|IDecorator<Object>|⬇️|`ChangeType`, `ChangeType<T>`, `ChangeTypeOrDefault<T>`, `DefaultPropertyValueResolver`|
|IDecorator<String>|⬇️|`Difference`, `ToByteArray`, `FromUrlEncodedBase64`, `ToCasing`, `ToAsciiEncodedString`, `ToStream`, `ToUri`, `StartsWith`, `ContainsAny`|
|IDecorator<TSource>|⬇️|`TraverseWhileNotEmpty<TSource>`|
|IDecorator<Type>|⬇️|`ToFriendlyName`, `IsNullable`, `IsComplex`, `IsSimple`, `HasAnonymousCharacteristics`, `HasDefaultConstructor`, `HasEqualityComparerImplementation`, `HasComparableImplementation`, `HasComparerImplementation`, `HasEnumerableImplementation`, `HasDictionaryImplementation`, `HasKeyValuePairImplementation`, `HasTypes`, `HasInterfaces`, `HasAttribute`, `HasCircularReference`, `MatchMember`, `GetDefaultValue`, `GetAllProperties`, `GetAllFields`, `GetAllEvents`, `GetAllMethods`, `GetRuntimePropertiesExceptOf<T>`, `GetInheritedTypes`, `GetDerivedTypes`, `GetHierarchyTypes`|
