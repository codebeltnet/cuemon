---
uid: Cuemon.Extensions
summary: *content
---
Write more expressive, fluent code by calling extension methods directly on .NET built-in types — `myString.ToUri()` replaces `new Uri(myString)`, `myException.Flatten()` unwraps an `AggregateException` in one call. Use this namespace when you want to reduce ceremony around common .NET operations. The `Cuemon.Extensions` namespace extends `String`, `DateTime`, `Object`, `Type`, `TimeSpan`, `Exception`, and many more types. If you are new to this namespace, start with the `String` or `Object` extension groups for the most frequently needed conversions and transformations.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Cuemon namespace](/api/dotnet/Cuemon.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|Action<TOptions>|⬇️|`Configure<TOptions>`|
|Action<T>|⬇️|`CreateInstance<T>`|
|Byte|⬇️|`ToEncodedString`, `ToHexadecimalString`, `ToBinaryString`, `ToUrlEncodedBase64String`, `ToBase64String`, `TryDetectUnicodeEncoding`|
|Char|⬇️|`ToEnumerable`, `FromChars`|
|Condition|⬇️|`HasDifference`|
|DateTime|⬇️|`ToUnixEpochTime`, `ToUtcKind`, `ToLocalKind`, `ToDefaultKind`, `IsWithinRange`, `IsTimeOfDayNight`, `IsTimeOfDayMorning`, `IsTimeOfDayForenoon`, `IsTimeOfDayAfternoon`, `IsTimeOfDayEvening`, `Floor`, `Ceiling`, `Round`|
|Double|⬇️|`FromUnixEpochTime`, `ToTimeSpan`, `Factorial`, `RoundOff`|
|Exception|⬇️|`Flatten`|
|IEnumerable<T>|⬇️|`GetHashCode32<T>`, `GetHashCode64<T>`, `ToDelimitedString<T>`|
|IEnumerable<String>|⬇️|`IsSequenceOf<T>`|
|Int*|⬇️|`Min`, `Max`, `IsPrime`, `IsCountableSequence`, `IsEven`, `IsOdd`|
|Mapping|⬇️|`AddMapping`|
|MethodDescriptor|⬇️|`HasParameters`|
|Object|⬇️|`As<T>`, `As`, `IsNullable<T>`|
|String|⬇️|`Difference`, `ToByteArray`, `FromUrlEncodedBase64`, `ToGuid`, `FromBinaryDigits`, `FromBase64`, `ToCasing`, `ToUri`, `IsNullOrEmpty`, `IsNullOrWhiteSpace`, `IsEmailAddress`, `IsGuid`, `IsHex`, `IsNumeric`, `IsBase64`, `SplitDelimited`, `Count`, `RemoveAll`, `ReplaceAll`, `JsEscape`, `JsUnescape`, `ContainsAny`, `ContainsAll`, `EqualsAny`, `StartsWith`, `TrimAll`, `IsSequenceOf<T>`, `FromHexadecimal`, `ToHexadecimal`, `ToEnum<TEnum>`, `ToTimeSpan`, `SubstringBefore`, `Chunk`, `SuffixWith`, `SuffixWithForwardingSlash`, `PrefixWith`|
|T|⬇️|`UseWrapper<T>`, `As<T, TResult>`, `Adjust<T>`, `Alter<T>`, `IsNullable<T>`|
|TimeSpan|⬇️|`GetTotalNanoseconds`, `GetTotalMicroseconds`, `Floor`, `Ceiling`, `Round`|
|Type|⬇️|`ToFriendlyName`, `ToTypeCode`, `HasEqualityComparerImplementation`, `HasComparableImplementation`, `HasComparerImplementation`, `HasEnumerableImplementation`, `HasDictionaryImplementation`, `HasKeyValuePairImplementation`, `IsNullable<T>`, `IsNullable`, `HasAnonymousCharacteristics`, `IsComplex`, `IsSimple`, `GetDefaultValue`, `HasTypes`, `HasInterfaces`, `HasAttributes`|
|Validator|⬇️|`ContainsReservedKeyword`, `HasDifference`, `NoDifference`|
