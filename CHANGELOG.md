# Changelog

All notable changes to this project, from version 6.0.0 and forward, will be documented in this file, aggregated.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

For more details, please refer to `PackageReleaseNotes.txt` on a per assembly basis in the `.nuget` folder.

## [10.5.3] - 2026-06-03

This is a patch release focused on fixing request service provider resolution for wrapped providers and significantly expanding test coverage across the library. The release improves code coverage with 36 focused, System-Under-Test (SUT)-specific unit test classes covering core utilities, extension methods, serialization, resilience, threading, and infrastructure components.

### Fixed

- `ServiceProviderExtensions.GetServiceDescriptors()` method to properly traverse and handle wrapped service providers, including detection and rejection of cyclic provider graphs; the method now correctly resolves descriptors when the provider is wrapped by third-party components such as AspVersioning's InjectApiVersion and throws `NotSupportedException` with descriptive messages when ambiguous or cyclic provider structures are encountered,
- `UseFaultDescriptorExceptionHandler()` middleware to work correctly when request services are wrapped by external decorators or proxies.

### Added

- `ServiceProviderExtensionsTest` unit test class with comprehensive test coverage for `GetServiceDescriptors()` method, including scenarios for delegating providers, ambiguous multi-provider cases, and cyclic provider graph detection,
- Functional test in `ApplicationBuilderExtensionsTest` to verify fault descriptor exception handling works correctly with wrapped request services,
- Focused unit test classes for core utilities (replacing generic CoverageTest approach): `ActionFactoryTest`, `FuncFactoryTest`, `TesterFuncFactoryTest`, `WrapperTest`, and `VerticalDirectionTest`,
- Focused unit test classes for extension methods: `RegionInfoExtensionsTest`, `MethodDescriptorExtensionsTest`, `CacheValidatorTest`, `ChecksumBuilderTest`, `ChecksumBuilderExtensionsTest`, `FileInfoExtensionsTest`, `HostBuilderExtensionsTest`, and `UriExtensionsTest`,
- Focused unit test classes for Hierarchy-related functionality: `HierarchyTest`, `HierarchyOptionsTest`, `HierarchyDecoratorExtensionsTest`, and `HierarchySerializerTest`,
- Focused unit test classes for ASP.NET Core MVC formatters: `MvcBuilderExtensionsTest` and `MvcCoreBuilderExtensionsTest` for both Text.Json and Xml formatters,
- Focused unit test classes for XML serialization converters: `ExceptionConverterTest`, `DefaultXmlConverterTest`, `DynamicXmlConverterTest`, and related converter functionality across the Cuemon.Extensions.Xml namespace,
- Focused unit test classes for resilience and threading: `LatencyExceptionTest`, `TransientOperationOverloadTest`, `AdvancedParallelFactoryTest`, `AsyncPatternsTest`, and `ParallelFactoryOverloadTest`,
- Focused unit test classes for mail and data infrastructure: `MailDistributorTest` and focused coverage for `Cuemon.Data`, `Cuemon.Extensions.Net`, `Cuemon.Net`, and `Cuemon.Resilience` namespaces,
- Enhanced unit test guidelines in `.github/copilot-instructions.md` with clarifications on namespace conventions, base class usage, public facade testing patterns, and best practices for test organization.

## [10.5.2] - 2026-05-18

This is a service update that focuses on package dependencies.

### Changed

- Codebelt.Extensions.BenchmarkDotNet.Console updated from 1.2.5 to 1.2.6,
- Codebelt.Extensions.Xunit and related testing packages (Xunit, Xunit.Hosting, Xunit.Hosting.AspNetCore) updated from 11.0.8 to 11.0.9,
- Microsoft.NET.Test.Sdk updated from 18.4.0 to 18.5.1,
- coverlet test instrumentation packages (collector and msbuild) updated from 8.0.1 to 10.0.1,
- Microsoft.Data.Sqlite updated to latest compatible versions (10.0.6 → 10.0.8 for .NET Standard 2.0 and .NET 10, 9.0.15 → 9.0.16 for .NET 9),
- Microsoft.Extensions packages (Configuration, DependencyInjection, Hosting, Http, Options.ConfigurationExtensions) updated to latest compatible versions (10.0.6 → 10.0.8 for .NET Standard 2.0 and .NET 10, 9.0.15 → 9.0.16 for .NET 9),
- Microsoft.Data.SqlClient updated from 7.0.0 to 7.0.1 for .NET 9 and .NET 10,
- System.Text.Json updated from 10.0.6 to 10.0.8 for .NET Standard 2.0,
- Microsoft.Bcl.AsyncInterfaces updated from 10.0.6 to 10.0.8 for .NET Standard 2.0,
- jobs-nuget-push GitHub Actions workflow updated from v2 to v3,
- NGINX container base image updated from 1.30.0-alpine to 1.31.0-alpine in DocFX Dockerfile,
- Ubuntu test runner Docker images pinned to specific patch versions (9 → 9.0.314, 10 → 10.0.300) for reproducible test environments.

## [10.5.1] - 2026-04-16

This is a patch release focused on kernel assembly refinement, XML serialization reliability, and comprehensive test coverage expansion. The release relocates foundational configuration types into the kernel layer while maintaining full backward compatibility through type forwarding.

### Changed

- `IConfigurable` and `Configurable` types relocated from `Cuemon.Core` to `Cuemon.Kernel` for improved architectural layering; `Cuemon.Core` maintains backward compatibility through `[TypeForwardedTo]` attributes,
- `XmlConverterDecoratorExtensions` class in the Cuemon.Extensions.Xml.Serialization.Converters namespace with simplified collection writing logic for improved maintainability,
- All package dependencies updated to latest compatible versions.

### Added

- Comprehensive unit tests for XML serialization converters in `Cuemon.Xml.Tests` covering `XmlConverter`, `DefaultXmlConverter`, `ExceptionConverter`, `FailureConverter`, `DynamicXmlConverter`, and related functionality,
- Test assets `LinkResponse` and `WrapperResponse` in `Cuemon.Xml.Tests` for XML serialization validation.

### Fixed

- `DefaultXmlConverter` class in the `Cuemon.Xml.Serialization.Converters` namespace to add `XmlRootAttribute` fallback when `MemberReference` is null, improving robustness in edge-case serialization scenarios.

## [10.5.0] - 2026-03-13

Okay, so this might look like a major release — and effort-wise it certainly felt like one — but it's actually a fully backward-compatible minor bump. Thanks to a helping hand from AI and a long-standing wish to carve out a small, lean core from Cuemon, we've introduced a brand-new assembly: **Cuemon.Kernel**. Think of it as the nano (or at least noticeably smaller) sibling of `Cuemon.Core`, shipping only the most essential and much-loved types — validators, conditions, decorators, disposable patterns, the options/parameter-object infrastructure, text encoding helpers and async primitives.

If you've ever wondered about Microsoft's own split between `mscorlib.dll`, `System.dll`, `System.Core.dll` and `System.Private.CoreLib.dll` — well, `Cuemon.Kernel` is probably our equivalent of `mscorlib.dll` and `System.dll`. Not quite as dramatic, but you get the idea.

The key detail: every public API stays exactly where it was. `Cuemon.Core` now references `Cuemon.Kernel` and uses `TypeForwardedTo` attributes for every type that moved, so existing consumers won't notice a thing. No breaking changes, no missing types, just a cleaner layering underneath.

> [!NOTE]
> To be clear, this is not a refactoring release. The code in `Cuemon.Core` that moved to `Cuemon.Kernel` is still exactly the same (only exception being the bug fixes), just compiled into a different assembly. The only "refactoring" aspect is the introduction of `TypeForwardedTo` attributes in `Cuemon.Core` to maintain backward compatibility.

This release also bumps all package dependencies to their latest compatible versions and includes minor XML documentation improvements in `DelimitedString` and `StringFactory` as well as bug fixing.

### Added

- **`Cuemon.Kernel`** assembly (new) — a lightweight, foundational subset of `Cuemon.Core` containing the most essential primitives:
  - `Alphanumeric`, `CasingMethod`, `GuidFormats`, `Endianness`, `UriScheme` enums and constants,
  - `Condition`, `ConditionalValue`, `ExceptionCondition` guard and condition utilities,
  - `Validator` with the full suite of `ThrowIf*` guard-clause methods,
  - `Patterns` with safe-invoke, configure, and exception-filter helpers,
  - `Decorator` / `IDecorator<T>` decoration infrastructure,
  - `Disposable`, `FinalizeDisposable`, `DisposableOptions` lifecycle types,
  - `Convertible`, `ConvertibleConverterDictionary`, `ConvertibleOptions` conversion primitives,
  - `IParameterObject`, `IValidatableParameterObject`, `IPostConfigurableParameterObject` configuration contracts,
  - `TesterFunc` delegate family and `SuccessfulValue` / `UnsuccessfulValue` result types,
  - `Arguments` collection helper,
  - `StreamDecoratorExtensions` stream utilities,
  - `AsyncOptions`, `AsyncRunOptions`, `IAsyncOptions`, `Awaiter` async primitives,
  - `ByteOrderMark`, `EncodingOptions`, `FallbackEncodingOptions`, `IEncodingOptions`, `PreambleSequence`, `EnumStringOptions` text encoding helpers,
  - `IParser`, `IConfigurableParser`, `UriStringOptions`, `ProtocolRelativeUriStringOptions` parser contracts,
  - `EndianOptions` byte-order configuration,
  - `TypeArgumentException`, `TypeArgumentOutOfRangeException`, `ArgumentReservedKeywordException` custom exception types,
  - `CallerArgumentExpressionAttribute` polyfill (for `netstandard2.0` and pre-.NET Core 3.0 targets),
- **`Cuemon.Kernel.Tests`** test project with comprehensive unit tests; a few migrated from `Cuemon.Core.Tests`, but the majority written beforehand creation of `Cuemon.Kernel`.

### Changed

- `Cuemon.Core` assembly now depends on `Cuemon.Kernel` and uses `[TypeForwardedTo]` attributes for all types that moved, preserving full backward compatibility,
- `Cuemon.Core` package description updated to reflect its role as higher-level building blocks on top of `Cuemon.Kernel`,
- `DelimitedString` class in the `Cuemon` namespace received improved XML documentation for the `Split` method,
- `StringFactory` class in the `Cuemon` namespace received improved XML documentation across factory methods,
- `Directory.Build.targets` simplified the `PreparePackageReleaseNotesFromFile` target to use `System.IO.File.ReadAllText` instead of `ReadLinesFromFile`.

### Fixed

- `Arguments` class in the `Cuemon.Collections.Generic` namespace so `Concat<T>` appends the second array at the correct offset when the input arrays have different lengths,
- `ByteOrderMark` class in the `Cuemon.Text` namespace to correctly detect UTF-8 and UTF-16 BOMs from short byte buffers and short streams, while only removing a preamble when the byte sequence matches the full encoding preamble exactly.

## [10.4.0] - 2026-02-27

This is a minor release that introduces support for flattening items in enumerable XML serialization, while also improving the behavior of the default XML converter to prevent duplicate element wrapping. A few bug fixes and improvements were also included in this release.

### Added

- `ServiceCollectionExtensions` class in the Cuemon.Extensions.AspNetCore.Xml namespace with `AddMinimalXmlOptions` extension method,
- `VaryAcceptMiddleware` class in the Cuemon.AspNetCore.Http.Headers namespace that adds the Vary header with the value of Accept to the HTTP response,
- `ApplicationBuilderExtensions` class in the Cuemon.Extensions.AspNetCore.Http.Headers namespace was extended with the UseVaryAccept extension method that adds the Vary header with the value of Accept to the HTTP response.

### Fixed

- `DefaultXmlConverter` class in the `Cuemon.Xml.Serialization.Converters` namespace to prevent duplicate element wrapping,
- `World` class in the Cuemon.Globalization namespace to include countries not previously included in the `GetStatisticalRegion` method.

### Changed

- `World` class in the Cuemon.Globalization namespace to exclude redundancies in `RegionInfo` handling,
- `XmlConverterExtensions` class in the Cuemon.Extensions.Xml.Serialization.Converters namespace to support flattening items when serializing in the `AddEnumerableConverter` method,
- `XmlSerializerOptions` class in the `Cuemon.Xml.Serialization` namespace was extended with `FlattenCollectionItems` property to control item flattening during enumerable XML serialization,
- 

## [10.3.0] - 2026-02-19

This is a minor release that introduces assembly discovery utilities and improved minimal API formatter integration, while also tightening option registration behavior across ASP.NET Core formatter setup.

### Added

- `AssemblyContext` class in the Cuemon.Reflection namespace that provides filtered discovery of assemblies in the current application domain, with optional traversal of referenced assemblies,
- `AssemblyContextOptions` class in the Cuemon.Reflection namespace that configures inclusion/exclusion filters and referenced-assembly traversal for `AssemblyContext`,
- `Patterns` class in the Cuemon namespace was extended with two new methods: `IsFatalException` and `IsRecoverableException`,
- `MinimalJsonOptions` class in the Cuemon.Extensions.AspNetCore.Text.Json namespace was added to map `JsonFormatterOptions` into ASP.NET Core `JsonOptions` for minimal APIs,
- `ServiceCollectionExtensions` class in the Cuemon.Extensions.AspNetCore.Text.Json namespace was extended with a new method: `AddMinimalJsonOptions`,
- `StackDecoratorExtensions` class in the Cuemon.Collections.Generic namespace was extended with a new `TryPop` extension method for `IDecorator<Stack<T>>`,
- `StackExtensions` class in the Cuemon.Extensions.Collections.Generic namespace was extended with a new `TryPop` extension method for `Stack<T>` (for compatible target frameworks without native `Stack<T>.TryPop`).

### Changed

- `ServiceCollectionExtensions` class in the Cuemon.Extensions.DependencyInjection namespace to use `TryConfigure` in option-registration overloads to avoid duplicate configuration registration,
- `ServiceCollectionExtensions` class in the Cuemon.Extensions.AspNetCore.Text.Json.Formatters namespace to use `TryConfigure` in `AddJsonFormatterOptions`,
- `ServiceCollectionExtensions` class in the Cuemon.Extensions.AspNetCore.Xml.Formatters namespace to use `TryConfigure` in `AddXmlFormatterOptions`,
- `Patterns` class in the Cuemon namespace to apply exception filters via `IsFatalException` / `IsRecoverableException` in `TryInvoke` overloads,
- `World` class in the Cuemon.Globalization namespace where `StatisticalRegions` now returns `IEnumerable<StatisticalRegionInfo>`.

### Fixed

- Prevented repeated `IConfigureOptions<TOptions>` registrations when formatter/options extension methods are called multiple times.

## [10.2.1] - 2026-02-12

This is a service update that focuses on package dependencies; however, it also includes a small performance optimization to reduce allocations and improve throughput when working with streams.

### Changed

- `StreamDecoratorExtensions` class in the Cuemon.IO namespace to reduce memory allocations and improve throughput of InvokeToByteArray by eliminating redundant buffer copies for non-MemoryStream inputs (allocation ratio reduced from ~2x to ~1x for typical payload sizes)
- `StreamDecoratorExtensions` class in the Cuemon.IO namespace to cache Length access and avoid repeated virtual calls on seekable streams
- `StreamDecoratorExtensions` class in the Cuemon.IO namespace to return Array.Empty<byte>() for zero-length seekable streams to avoid unnecessary MemoryStream allocation


## [10.2.0] - 2026-02-09

This is a minor release that introduces support for UN M.49 statistical regions and countries.

### Added

- `StatisticalRegionInfo` class in the Cuemon.Globalization namespace that provides information about a specific statistical region,
- `StatisticalRegionKind` enum in the Cuemon.Globalization namespace that defines the kind of statistical region,
- `StatisticalRegionExtensions` class in the Cuemon.Extensions.Globalization namespace that consist of extension methods for the `StatisticalRegionInfo` class: `IsWorld`, `IsRegion`, `IsSubregion`, `IsIntermediateRegion`, `IsCountryOrTerritory`, `IsArea`, `HasIsoCodes` and `HasRegionInfo`.

### Changed

- `World` class in the Cuemon.Globalization namespace with a new method: `GetStatisticalRegions`, which retrieves a list of statistical regions for a given UN M.49 country code,
- `World` class in the Cuemon.Globalization namespace with a new method: `GetCountry`, which retrieves the country information for a given UN M.49 country code,
- `World` class in the Cuemon.Globalization namespace with a new method: `GetCountry`, which retrieves the country information for a given `RegionInfo` object,
- `World` class in the Cuemon.Globalization namespace with a new property: `StatisticalRegions`, which retrieves a list of all statistical regions.

## [10.1.2] - 2026-01-21

This is a service update that focuses on package dependencies.

## [10.1.1] - 2025-12-12

This is a service update that focuses on package dependencies while embracing the latest addition to Codebelt; [Codebelt.Extensions.BenchmarkDotNet](https://github.com/codebeltnet/benchmarkdotnet).

## [10.1.0] - 2025-12-06

This is a minor release that introduces support for post-configuration of parameter objects while delivering a significant series of performance improvements across various computations.

These optimizations yielded a significant performance boost with impressive reductions in memory allocations, enhancing overall efficiency and responsiveness.

### Added

- `IPostConfigurableParameterObject` interface in the Cuemon.Configuration namespace that defines a contract for parameter objects that require post-configuration logic.

### Changed

- `Validator` class in the Cuemon namespace was extended to support invoking parameter objects implementing the `IPostConfigurableParameterObject` interface,
- `FowlerNollVoHash` class in the Cuemon.Security namespace was optimized with specialized compute paths for 32-bit, 64-bit, and multi-word (≥128-bit) hashes,
- `DateSpan` struct in the Cuemon namespace was optimized for better performance when computing hash codes and equality checks,
- `RandomString` method on the `Generate` class in the Cuemon namespace with a sequential char-array builder and length guard, reducing overhead and allocations
- `DelimitedString` class in the Cuemon namespace was optimized with a specialized fast-path for single-character delimiters and qualifiers, reducing overhead and allocations
- `AesCryptor` class in the Cuemon.Security.Cryptography namespace was optimized to use `TransformFinalBlock` instead of stream-based transforms, reducing overhead and allocations
- `CyclicRedundancyCheck` class in the Cuemon.Security namespace was optimized by switching CRC lookup tables to ulong[] and simplifying initialization/loop structure, increasing throughput and reducing unnecessary copying

### Fixed

- `Patterns` class in the Cuemon namespace had all `Activator.CreateInstance<T>()` calls replaced with direct `new T()` (generic object creation expressions), to eliminate reflection overhead
- `SecureHashAlgorithm512` class in the Cuemon.Security.Cryptography namespace to allow optional setup delegate

## [10.0.0] - 2025-11-11

This is a major release that focuses on adapting to the latest .NET 10 (LTS) release, while also removing support for .NET 8 (LTS).

### Changed

- Lock statement has been updated to use the new System.Threading.Lock class where applicable

### Removed

- Backport.System.Threading.Lock (since System.Threading.Lock is now available in modern TFMs) dependency from Cuemon.Extensions.Threading

> [!NOTE]
> Although a tough decision (as I love FOSS and the community behind), this is aligned with the original idea and concept behind **Cuemon for .NET**; to enhance the developer experience with additional features,
> functionality and extensions for namespaces within Base Class Library (BCL) while adhering to Framework Design Guidelines (FDG)
> and not relying on 3rd party libraries that are not part of the BCL.<br/><br/>
> _A warm thank you to [Mark Cilia Vincenti](https://github.com/MarkCiliaVincenti) for his idea and contribution to include [Backport.System.Threading.Lock library](https://www.nuget.org/packages/Backport.System.Threading.Lock) into the Cuemon assembly family._

## [9.0.10] - 2025-10-17

This is a service update that focuses on package dependencies.

## [9.0.9] - 2025-09-13

This is a service update that focuses on package dependencies.

## [9.0.8] - 2025-08-07

This is a service update that focuses on package dependencies.

## [9.0.7] - 2025-07-09

This is a service update that focuses on package dependencies and a few bug fixes.

### Changed

- `System.Data.SqlClient` to `Microsoft.Data.SqlClient` since the former is officially deprecated in `Cuemon.Data.SqlClient` assembly

## [9.0.6] - 2025-06-14

This is a service update that focuses on package dependencies and a minor new handy feature; `Cuemon.Threading.Awaiter.RunUntilSuccessfulOrTimeoutAsync` method.

### Added

- Awaiter class in the Cuemon.Threading namespace that provides a set of static methods for awaiting asynchronous operations:
  - RunUntilSuccessfulOrTimeoutAsync repeatedly invokes a specified asynchronous lambda expression until it succeeds or a configured timeout is reached
- AsyncRunOptions class in the Cuemon.Threading namespace that provides configuration options for the RunUntilSuccessfulOrTimeoutAsync method


## [9.0.5] - 2025-05-13

This is a service update that focuses on package dependencies and a few bug fixes.

### Added

- SecureHashAlgorithm512256 class in the Cuemon.Security.Cryptography namespace that provides a SHA-512-256 implementation of the SHA (Secure Hash Algorithm) cryptographic hashing algorithm for 512-bit hash values
- SHA512256 class in the Cuemon.Security.Cryptography namespace that represents the SHA-512/256 cryptographic hash algorithm, which produces a 256-bit hash value using the SHA-512 algorithm as its base
- DigestCryptoAlgorithm enum in the Cuemon.AspNetCore.Authentication.Digest namespace that specifies the supported digest algorithms for HTTP Digest Access Authentication
- DigestHashFactory class in the Cuemon.AspNetCore.Authentication.Digest namespace that provides access to factory methods for creating and configuring Hash instances based on UnkeyedCryptoHash{TAlgorithm}

### Changed

- UnkeyedCryptoAlgorithm enum in the Cuemon.Security.Cryptography namespace was extended with a new value: Sha512Slash256
- UnkeyedHashFactory class in the Cuemon.Security.Cryptography namespace was extended with a new method: CreateCryptoSha512Slash256
- DigestAuthenticationOptions class in the Cuemon.AspNetCore.Authentication.Digest namespace was extended with a property named DigestAlgorithm that specifies the digest algorithm to use for HTTP Digest Access Authentication; default is DigestCryptoAlgorithm.Sha256

### Fixed

- DigestAuthenticationHandler class in the Cuemon.AspNetCore.Authentication.Digest namespace to use newly added DigestCryptoAlgorithm enum in the Cuemon.AspNetCore.Authentication.Digest namespace
- DigestAuthenticationMiddleware class in the Cuemon.AspNetCore.Authentication.Digest namespace to use newly added DigestCryptoAlgorithm enum in the Cuemon.AspNetCore.Authentication.Digest namespace
- DigestAuthorizationHeaderBuilder class in the Cuemon.AspNetCore.Authentication.Digest namespace to use newly added DigestCryptoAlgorithm enum in the Cuemon.AspNetCore.Authentication.Digest namespace

## [9.0.4] - 2025-04-10

This is a service update that focuses on package dependencies and a few bug fixes.

> [!WARNING]
> The fix applied to the `DigestAuthenticationHandler`, `DigestAuthenticationMiddleware`, and `DigestAuthorizationHeader` classes in the `Cuemon.AspNetCore.Authentication.Digest` namespace changes both the `WWW-Authenticate` and `Authorization` headers. Justification for this patch is mentioned in [GitHub Issue #115](https://github.com/gimlichael/Cuemon/issues/115), but may affect existing implementations that rely on the previous behavior.

### Fixed

- Updated the `DigestAuthenticationHandler` class in the `Cuemon.AspNetCore.Authentication.Digest` namespace to remove quoted string values for the `stale` and `algorithm` parameters,
- Updated the `DigestAuthenticationMiddleware` class in the `Cuemon.AspNetCore.Authentication.Digest` namespace to remove quoted string values for the `stale` and `algorithm` parameters,
- Updated the `DigestAuthorizationHeader` class in the `Cuemon.AspNetCore.Authentication.Digest` namespace to:
  - Remove quoted string values for the `algorithm`, `qop`, and `nc` parameters,
  - Exclude the `stale` parameter and mark the previous implementation as obsolete,
  - Address issues outlined in [GitHub Issue #115](https://github.com/gimlichael/Cuemon/issues/115).

## [9.0.3] - 2025-03-31

This is a service update that focuses on package dependencies.

## [9.0.2] - 2025-03-31

This is a service update that focuses on package dependencies.

### Fixed

- Disposable class in the Cuemon namespace to set the disposed flag to true immediately after the Dispose method is called after thread safety check (previously it was set to true after the Dispose method had completed)

## [9.0.1] - 2024-01-28

> [!IMPORTANT]  
> The following projects has been removed with this release and migrated to [Codebelt](https://github.com/codebeltnet):
> 
> - Cuemon.Extensions.Globalization was removed from the solution and migrated to [Codebelt.Extensions.Globalization](https://github.com/codebeltnet/globalization)
>
> Dependencies used for targeting .NET Standard 2.0 has been updated to use .NET 8.0 (LTS) instead of .NET Core 2.1.

This is a service update that primarily focuses on package dependencies and minor improvements.

### Changed

- `Cuemon.Extensions.Hosting` namespace no longer have a dependency to IHostingEnvironment (TFM netstandard2.0)

## [9.0.0] - 2024-11-13

> [!IMPORTANT]  
> The following projects has been removed with this release and migrated to [Codebelt](https://github.com/codebeltnet):
> 
> - Cuemon.Extensions.Xunit, Cuemon.Extensions.Xunit.Hosting and Cuemon.Extensions.Xunit.Hosting.AspNetCore was removed from the solution and migrated to [Codebelt.Extensions.Xunit](https://github.com/codebeltnet/xunit)
> - Cuemon.Extensions.YamlDotNet was removed from the solution and migrated to [Codebelt.Extensions.YamlDotNet](https://github.com/codebeltnet/yamldotnet)
> - Cuemon.Extensions.AspNetCore, Cuemon.Extensions.AspNetCore.Mvc and Cuemon.Extensions.Diagnostics was inflicted with a breaking change as a result of this migration due to removal of YAML related code
> - Cuemon.Extensions.Asp.Versioning was removed from the solution and migrated to [Codebelt.Extensions.Asp.Versioning](https://github.com/codebeltnet/asp-versioning)
> - Cuemon.Extensions.Swashbuckle.AspNetCore was removed from the solution and migrated to [Codebelt.Extensions.Swashbuckle.AspNetCore](https://github.com/codebeltnet/swashbuckle-aspnetcore)
> - Cuemon.Extensions.Newtonsoft.Json was removed from the solution and migrated to [Codebelt.Extensions.Newtonsoft.Json](https://github.com/codebeltnet/newtonsoft-json)
> - Cuemon.Extensions.AspNetCore.Authentication.AwsSignature4 was removed from the solution and migrated to [Codebelt.Extensions.AwsSignature4](https://github.com/codebeltnet/aws-signature-v4)

> [!NOTE]
> Types that are removed from this solution (but otherwise fits naturally together) will be migrated to smaller and more focused repositories in the [Codebelt](https://github.com/codebeltnet) organization, renamed to better reflect their purpose and published as standalone packages on NuGet.
>
> - [Codebelt.Unitify](https://github.com/codebeltnet/unitify) is an example of a repository that received types from this solution and is now a standalone library that provides a uniform way of working with prefixes and multiples in the context of units of measure.

This major release is first and foremost focused on ironing out any wrinkles that have been introduced with .NET 9 preview releases so the final release is production ready together with the official launch from Microsoft.

Next focus point will be ensuring a consistent developer experience while ironing out some of the more stale legacy code in the project.

Expect breaking changes with this major release.

Highlighted features included in this release:

- Support for both **FaultDetails** (HttpExceptionDescriptor) and **ProblemDetails** in the context of ASP.NET (both vanilla and MVC)
- Reduced footprint in the core assemblies by removing obsolete and redundant code
  - Went from approximately 700 KB to 500 KB in the core assembly
  - Types reduced with approximately 20% which is about 100 types
- Central Package Management (CPM) for all projects in the solution

### Added

- PreferredFaultDescriptor enum in the Cuemon.AspNetCore.Diagnostics namespace that specifies the preferred output format of an Exception raised in the context of either vanilla ASP.NET or ASP.NET MVC
- Failure record in the Cuemon.Diagnostics namespace that represents a failure model with detailed information about an exception
- FailureConverter class in the Cuemon.Extensions.Text.Json.Converters namespace to convert FailureConverter to JSON
- FailureConverter class in the Cuemon.Xml.Serialization.Converters namespace to convert FailureConverter to XML
- Support for System.Threading.Lock object that targets TFMs prior to .NET 9 (credits to Mark Cilia Vincenti, https://github.com/MarkCiliaVincenti/Backport.System.Threading.Lock)
- JsonConverterCollectionExtensions class in the Cuemon.Extensions.AspNetCore.Text.Json.Converters namespace to include two new extension methods: AddProblemDetailsConverter and AddHeaderDictionaryConverter
- XmlConverterExtensions class in the Cuemon.Extensions.AspNetCore.Xml.Converters namespace was extended to include one new extension method: AddProblemDetailsConverter
- XmlConverterExtensions class in the Cuemon.Extensions.Xml.Serialization.Converters namespace was extended to include one new extension method: AddFailureConverter
- JsonConverterCollectionExtensions class in the Cuemon.Extensions.Text.Json.Converters namespace was extended to include three new extension methods: AddFailureConverter, RemoveAllOf and RemoveAllOf{T}
- JsonSerializerOptionsExtensions class in the Cuemon.Extensions.Text.Json namespace was extended to include one new extension method: Clone
- AsyncPatterns class in the Cuemon.Threading namespace that provides a generic way to support different types of design patterns and practices with small utility methods scoped to Task
- AsyncActionFactory class in the Cuemon.Threading namespace that provides access to factory methods for creating AsyncActionFactory{TTuple} instances that encapsulate a Task based action delegate with a variable amount of generic arguments
- AsyncFuncFactory class in the Cuemon.Threading namespace that provides access to factory methods for creating AsyncFuncFactory{TTuple,TResult} instances that encapsulate a Task{TResult} based function delegate with a variable amount of generic arguments
- Decorator class in the Cuemon namespace was extended with an additional method: RawEnclose
- ActionFactory class in the Cuemon.Extensions namespace that provides access to factory methods for creating ActionFactory{TTuple} objects that encapsulate a delegate with a variable amount of generic arguments
- FuncFactory class in the Cuemon.Extensions namespace that provides access to factory methods for creating FuncFactory{TTuple, TResult} objects that encapsulate a function delegate with a variable amount of generic arguments
- MutableTupleFactory class in the Cuemon.Extensions namespace that provides access to factory methods for creating MutableTuple objects
- TesterFuncFactory class in the Cuemon.Extensions namespace that provides access to factory methods for creating TesterFuncFactory{TTuple, TResult, TSuccess} objects that encapsulate a tester function delegate with a variable amount of generic arguments
- AsyncDisposable class in the Cuemon.Extensions namespace that provides a mechanism for asynchronously releasing both managed and unmanaged resources with focus on the former
- VerticalDirection enum in the Cuemon.Extensions namespace that specifies a set of values defining a vertical direction
- IWrapper interface in the Cuemon.Extensions namespace that defines a generic way to wrap an object instance inside another object
- Wrapper class in the Cuemon.Extensions namespace that provides a way to wrap an object instance inside another object
- Hierarchy class in the Cuemon.Extensions.Runtime namespace that represents a way to expose a node of a hierarchical structure, including the node object type
- HierarchyDecoratorExtensions class in the Cuemon.Extensions.Runtime namespace that provides (hidden) extensions to the IHierarchy interface
- HierarchyOptions class in the Cuemon.Extensions.Runtime namespace that represents a set of options to configure the behavior of the Hierarchy and HierarchySerializer class
- IHierarchy interface in the Cuemon.Extensions.Runtime namespace that defines a way to expose a node of a hierarchical structure
- HierarchySerializer class in the Cuemon.Extensions.Runtime.Serialization namespace that provides a way to serialize objects to nodes of IHierarchy

### Changed

- ReservedKeywordException class in the Cuemon namespace was renamed to ArgumentReservedKeywordException (breaking change)
- Condition class in the Cuemon namespace was extended with an additional method: HasDifference
- Validator class in the Cuemon namespace was extended with five new methods: ThrowIfContainsReservedKeyword, ThrowIfNotDifferent, ThrowIfDifferent, ThrowIfContainsAny and ThrowIfNotContainsAny
- Validator class in the Cuemon namespace to comply with [RSPEC-3343](https://rules.sonarsource.com/csharp/type/Bug/RSPEC-3343/)
  - ThrowIfInvalidConfigurator (breaking change)
  - ThrowIfInvalidOptions (breaking change)
  - ThrowIfNumber (breaking change)
  - ThrowIfNotNumber (breaking change)
  - ThrowIfNull (breaking change)
  - ThrowIfSequenceEmpty (breaking change)
  - ThrowIfSequenceNullOrEmpty (breaking change)
  - ThrowIfEmpty (breaking change)
  - ThrowIfWhiteSpace (breaking change)
  - ThrowIfNullOrEmpty (breaking change)
  - ThrowIfNullOrWhitespace (breaking change)
  - ThrowIfHex (breaking change)
  - ThrowIfNotHex (breaking change)
  - ThrowIfEmailAddress (breaking change)
  - ThrowIfNotEmailAddress (breaking change)
  - ThrowIfGuid (breaking change)
  - ThrowIfNotGuid (breaking change)
  - ThrowIfUri (breaking change)
  - ThrowIfNotUri (breaking change)
  - ThrowIfContainsInterface (breaking change)
  - ThrowIfNotContainsInterface (breaking change)
  - ThrowIfContainsType (breaking change)
  - ThrowIfNotContainsType (breaking change)
  - ThrowIfEnum (breaking change)
  - ThrowIfNotEnum (breaking change)
  - ThrowIfNotEnumType (breaking change)
  - ThrowIfNotBinaryDigits (breaking change)
  - ThrowIfNotBase64String (breaking change)
- ExceptionDescriptorResult class in the Cuemon.AspNetCore.Mvc namespace to have an extra overload that accepts ProblemDetails
- FaultDescriptorFilter class in the Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace to support preferred fault descriptor (e.g., FaultDetails or ProblemDetails)
- FaultDescriptorOptions class in the Cuemon.AspNetCore.Diagnostics namespace to include a property named FaultDescriptor (PreferredFaultDescriptor); default is PreferredFaultDescriptor.FaultDetails
- HttpExceptionDescriptor class in the Cuemon.AspNetCore.Diagnostics namespace to include two new properties; Instance (Uri) and TraceId (string)
- ApplicationBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Diagnostics namespace to support preferred fault descriptor (e.g., FaultDetails or ProblemDetails) in the UseFaultDescriptorExceptionHandler extension method
- XmlConverter{T} class in the Cuemon.Xml.Serialization.Converters namespace to use generic type T with two new abstract methods: WriteXml and ReadXml
- CultureInfoExtensions class in the Cuemon.Extensions.Globalization namespace to use Codebelt.Extensions.YamlDotNet assembly instead of previously built-in YAML support from Codebelt.Extensions.YamlDotNet assembly
- TaskActionFactory class in the Cuemon namespace was renamed to AsyncActionFactory and moved to the Cuemon.Threading namespace (breaking change)
- TaskFuncFactory class in the Cuemon namespace was renamed to AsyncFuncFactory and moved to the Cuemon.Threading namespace (breaking change)
- Template class in the Cuemon namespace was renamed to MutableTuple (breaking change)
- TemplateFactory class in the Cuemon namespace was renamed to MutableTupleFactory (breaking change)

### Fixed

- ExceptionConverter class in the Cuemon.Xml.Serialization.Converters namespace to use Environment.NewLine instead of Alphanumeric.NewLine (vital for non-Windows operating systems)
- The JSON converter in the Cuemon.Extensions.Text.Json.Converters namespace that converts a Failure to JSON so it uses the actual key-value from the Data property of an exception instead of always writing key

### Removed

- Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc project due to redundancies with Cuemon.Extensions.Xunit.Hosting.AspNetCore (breaking change)
- IMiddlewareTest interface from the Cuemon.Extensions.Xunit.Hosting.AspNetCore namespace (breaking change)
- MiddlewareTestFactory static class from the Cuemon.Extensions.Xunit.Hosting.AspNetCore namespace (breaking change)
- HttpExceptionDescriptorResponseHandlerExtensions class from the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace (breaking change)
- HttpExceptionDescriptorResponseHandlerExtensions class from the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json namespace (breaking change)
- HttpExceptionDescriptorResponseHandlerExtensions class from the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml namespace (breaking change)
- HttpExceptionDescriptorResponseHandlerExtensions class from the Cuemon.Extensions.AspNetCore.Diagnostics namespace (breaking change)
- BadRequestMessage property from the class from the ApiKeySentinelOptions class in the Cuemon.AspNetCore.Http.Headers namespace (breaking change)
- DefaultYamlConverter class from the Cuemon.Runtime.Serialization.Converters namespace (breaking change)
- YamlSerializer class from the Cuemon.Runtime.Serialization namespace (breaking change)
- YamlSerializerOptions class from the Cuemon.Runtime.Serialization namespace (breaking change)
- YamlTextReader class from the Cuemon.Runtime.Serialization namespace (breaking change)
- YamlTextWriter class from the Cuemon.Runtime.Serialization namespace (breaking change)
- YamlTokenType class from the Cuemon.Runtime.Serialization namespace (breaking change)
- ExceptionConverter class from the Cuemon.Text.Yaml.Converters namespace (breaking change)
- ExceptionDescriptorConverter class from the Cuemon.Text.Yaml.Converters namespace (breaking change)
- YamlConverter class from the Cuemon.Text.Yaml.Converters namespace (breaking change)
- YamlFormatter class from the Cuemon.Text.Yaml.Converters namespace (breaking change)
- YamlFormatterOptions class from the Cuemon.Text.Yaml.Converters namespace (breaking change)
- YamlConverterFactory class from the Cuemon.Text.Yaml namespace (breaking change)
- YamlNamingPolicy class from the Cuemon.Text.Yaml namespace (breaking change)
- ConditionExtensions class from the Cuemon.Extensions namespace and moved members to Condition class in the Cuemon.Core assembly (Cuemon namespace)
- ValidatorExtensions class from the Cuemon.Extensions namespace and moved members to Validator class in the Cuemon.Core assembly (Cuemon namespace)
- TFM net6.0 for all projects due to [EOL](https://endoflife.date/dotnet) on November 12th, 2024 (presumable same date as .NET 9 release)
- YamlConverterExtensions class from the Cuemon.Extensions.AspNetCore.Text.Yaml.Converters namespace (breaking change)
- ServiceCollectionExtensions class from the Cuemon.Extensions.AspNetCore.Text.Yaml.Formatters namespace (breaking change)
- MvcBuilderExtensions class from the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml namespace (breaking change)
- MvcCoreBuilderExtensions class from the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml namespace (breaking change)
- YamlSerializationInputFormatter class from the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml namespace (breaking change)
- YamlSerializationMvcOptionsSetup class from the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml namespace (breaking change)
- YamlSerializationOutputFormatter class from the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml namespace (breaking change)
- MvcBuilderExtensions class from the Cuemon.Extensions.AspNetCore.Mvc.Filters namespace (breaking change)
- MvcCoreBuilderExtensions class from the Cuemon.Extensions.AspNetCore.Mvc.Filters namespace (breaking change)
- ExceptionDescriptorExtensions class from the Cuemon.Extensions.Diagnostics namespace (breaking change)
- ReplaceLineEndings extension method from the StringExtensions class in the Cuemon.Extensions namespace (breaking change)
- SafeInvokeAsync methods from the Patterns class in the Cuemon namespace (breaking change - moved to AsyncPatterns class in the Cuemon.Threading assembly)
- ActionFactory static class from the Cuemon namespace (moved to the Cuemon.Extensions namespace in the Cuemon.Extensions.Core assembly)
- FuncFactory static class from the Cuemon namespace (moved to the Cuemon.Extensions namespace in the Cuemon.Extensions.Core assembly)
- TesterFuncFactory static class from the Cuemon namespace (moved to the Cuemon.Extensions namespace in the Cuemon.Extensions.Core assembly)
- BinaryPrefix class from the Cuemon namespace (breaking change)
- BitStorageCapacity class from the Cuemon namespace (breaking change)
- BitUnit class from the Cuemon namespace (breaking change)
- ByteStorageCapacity class from the Cuemon namespace
- ByteUnit class from the Cuemon namespace (breaking change)
- DecimalPrefix class from the Cuemon namespace (breaking change)
- IPrefixMultiple interface from the Cuemon namespace (breaking change)
- IUnit interface from the Cuemon namespace (breaking change)
- MultipleTable class from the Cuemon namespace (breaking change)
- NamingStyle enum from the Cuemon namespace (breaking change)
- PrefixMultiple class from the Cuemon namespace (breaking change)
- PrefixUnit class from the Cuemon namespace (breaking change)
- StorageCapacity class from the Cuemon namespace (breaking change)
- StorageCapacityOptions class from the Cuemon namespace (breaking change)
- UnitFormatOptions class from the Cuemon namespace (breaking change)
- UnitPrefix enum from the Cuemon namespace (breaking change)
- UnitPrefixFormatter class from the Cuemon namespace (breaking change)
- ZeroPrefix class from the Cuemon namespace (breaking change)
- DataPairCollection class from the Cuemon.Collections namespace
- DataPairDictionary class from the Cuemon.Collections namespace
- Initializer class from the Cuemon namespace
- InitializerBuilder class from the Cuemon namespace
- Mapping class from the Cuemon namespace
- IndexMapping class from the Cuemon namespace
- HorizontalDirection enum from the Cuemon namespace
- VerticalDirection enum from the Cuemon namespace (moved to the Cuemon.Extensions namespace in the Cuemon.Extensions.Core assembly)
- IWrapper interface from the Cuemon namespace (moved to the Cuemon.Extensions namespace in the Cuemon.Extensions.Core assembly)
- Wrapper class from the Cuemon namespace (moved to the Cuemon.Extensions namespace in the Cuemon.Extensions.Core assembly)
- HierarchyDecoratorExtensions class from the Cuemon namespace (moved to the Cuemon.Extensions.Runtime namespace in the Cuemon.Extensions.Core assembly)
- Hierarchy class from the Cuemon namespace (moved to the Cuemon.Extensions.Runtime namespace in the Cuemon.Extensions.Core assembly)
- IHierarchy interface from the Cuemon namespace (moved to the Cuemon.Extensions.Runtime namespace in the Cuemon.Extensions.Core assembly)
- ObjectHierarchyOptions class from the Cuemon.Reflection namespace (moved to the Cuemon.Extensions.Runtime namespace in the Cuemon.Extensions.Core assembly and renamed to HierarchyOptions)
- HierarchySerializer class from the Cuemon.Runtime.Serialization namespace (moved to the Cuemon.Extensions.Runtime.Serialization namespace in the Cuemon.Extensions.Core assembly)

## [8.3.2] - 2024-08-04

### Dependencies

- Cuemon.Extensions.Swashbuckle.AspNetCore updated to latest and greatest with respect to TFMs
- Cuemon.Extensions.Text.Json updated to latest and greatest with respect to TFMs (fixes [CVE-2024-30105](https://github.com/advisories/GHSA-hh2w-p6rv-4g7w))
- Cuemon.Extensions.Xunit updated to latest and greatest with respect to TFMs
- Cuemon.Extensions.Xunit.Hosting updated to latest and greatest with respect to TFMs
- Cuemon.Extensions.Xunit.Hosting.AspNetCore updated to latest and greatest with respect to TFMs
- Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc updated to latest and greatest with respect to TFMs
- Cuemon.Extensions.YamlDotNet updated to latest and greatest with respect to TFMs

### Fixed

- YamlFormatter class in the Cuemon.Extensions.YamlDotNet.Formatters namespace to use WithCaseInsensitivePropertyMatching (https://github.com/aaubry/YamlDotNet/discussions/946)
  - Although v16.0.0 of YamlDotNet has breaking changes, this is not reflected in the API from Cuemon.Extensions.YamlDotNet until next major release

### Removed

- TFM net7.0 for all projects due to [EOL](https://endoflife.date/dotnet)


## [8.3.1] - 2024-06-01

This release was primarily focused on adapting a more modern way of performing CI/CD while making 3rd party tools more agnostic to the ever changing world of technology. Highlight of non-code changes:

- Azure DevOps pipelines has been replaced with GitHub Actions
- DocFX documentation now supports hosting on ARM based platforms
- Adapted trunk based branching that is more aligned with todays DevSecOps practices
- Added branch protection rules that ensures a linear git history and requires pull request before merging

### Dependencies

- Cuemon.Extensions.Swashbuckle.AspNetCore updated to latest and greatest with respect to TFMs
- Cuemon.Extensions.Xunit updated to latest and greatest with respect to TFMs
- Cuemon.Extensions.Xunit.Hosting updated to latest and greatest with respect to TFMs
- Cuemon.Extensions.Xunit.Hosting.AspNetCore updated to latest and greatest with respect to TFMs
- Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc updated to latest and greatest with respect to TFMs
- Cuemon.Extensions.YamlDotNet updated to latest and greatest with respect to TFMs

### Added

- IWebHostTest interface in the Cuemon.Extensions.Xunit.Hosting.AspNetCore namespace that represents the members needed for ASP.NET Core (including but not limited to MVC, Razor and related) testing
- WebHostTestFactory class in the Cuemon.Extensions.Xunit.Hosting.AspNetCore namespace that provides a set of static methods for ASP.NET Core (including, but not limited to MVC, Razor and related) unit testing

### Deprecated

- IMiddlewareTest interface in the Cuemon.Extensions.Xunit.Hosting.AspNetCore namespace; use the consolidated IWebHostTest instead
- MiddlewareTestFactory class in the Cuemon.Extensions.Xunit.Hosting.AspNetCore namespace; use the consolidated WebHostTestFactory instead
- IWebApplicationTest interface in the Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc namespace; use the consolidated IWebHostTest in the Cuemon.Extensions.Xunit.Hosting.AspNetCore namespace instead
- WebApplicationTestFactory class in the Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc namespace; use the consolidated WebHostTestFactory in the Cuemon.Extensions.Xunit.Hosting.AspNetCore namespace instead

## [8.3.0] - 2024-04-09

### Added

- Test class in the Cuemon.Extensions.Xunit namespace was extended with one new static method: Match
- WildcardOptions class in the Cuemon.Extensions.Xunit namespace that provides configuration options for the Match method on the Test class
- ExceptionConverter class in the Cuemon.Extensions.YamlDotNet.Converters namespace that converts an Exception to YAML
- ExceptionDescriptorConverter class in the Cuemon.Extensions.YamlDotNet.Converters namespace that converts an ExceptionDescriptor to YAML
- YamlConverter class in the Cuemon.Extensions.YamlDotNet.Converters namespace that converts an object to and from YAML (YAML ain't markup language)
- YamlFormatter class in the Cuemon.Extensions.YamlDotNet.Formatters namespace that serializes and deserializes an object, in YAML format
- YamlFormatterOptions class in the Cuemon.Extensions.YamlDotNet.Formatters namespace that provides configuration options for YamlFormatter
- YamlFormatterOptionsExtensions class in the Cuemon.Extensions.YamlDotNet.Formatters namespace that consist of one extension method for the YamlFormatterOptions class: SetPropertyName
- EmitterExtensions class in the Cuemon.Extensions.YamlDotNet namespace that consist of many extension method for the IEmitter interface: WriteStartObject, WriteString, WritePropertyName, WriteValue, WriteEndObject, WriteStartArray, WriteEndArray and WriteObject
- NodeOptions class in the Cuemon.Extensions.YamlDotNet namespace that provides configuration options for EmitterExtensions
- ScalarOptions class in the Cuemon.Extensions.YamlDotNet namespace that provides configuration options for EmitterExtensions
- YamlConverterFactory class in the Cuemon.Extensions.YamlDotNet namespace that provides a factory based way to create and wrap an YamlConverter implementations
- YamlSerializerOptions class in the Cuemon.Extensions.YamlDotNet namespace that provides configuration options for SerializerBuilder and DeserializerBuilder

### Changed

- ValidatorExtensions class in the Cuemon.Extensions namespace was extended with two new extension methods for the Validator class: ContainsAny and NotContainsAny
- HttpExceptionDescriptorResponseHandlerExtensions class in the Cuemon.Extensions.AspNetCore.Diagnostics namespace to use Cuemon.Extensions.YamlDotNet assembly instead of legacy YAML support in Cuemon.Core assembly
- YamlConverterExtensions class in the Cuemon.Extensions.AspNetCore.Text.Yaml.Converters namespace to use Cuemon.Extensions.YamlDotNet assembly instead of legacy YAML support in Cuemon.Core assembly
- ServiceCollectionExtensions class in the Cuemon.Extensions.AspNetCore.Text.Yaml.Formatters namespace to use Cuemon.Extensions.YamlDotNet assembly instead of legacy YAML support in Cuemon.Core assembly
- MvcBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml namespace to use Cuemon.Extensions.YamlDotNet assembly instead of legacy YAML support in Cuemon.Core assembly
- MvcCoreBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml namespace to use Cuemon.Extensions.YamlDotNet assembly instead of legacy YAML support in Cuemon.Core assembly
- YamlSerializationInputFormatter class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml namespace to use Cuemon.Extensions.YamlDotNet assembly instead of legacy YAML support in Cuemon.Core assembly
- YamlSerializationMvcOptionsSetup class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml namespace to use Cuemon.Extensions.YamlDotNet assembly instead of legacy YAML support in Cuemon.Core assembly
- YamlSerializationOutputFormatter class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml namespace to use Cuemon.Extensions.YamlDotNet assembly instead of legacy YAML support in Cuemon.Core assembly
- CultureInfoExtensions class in the Cuemon.Extensions.Globalization namespace to use Cuemon.Extensions.YamlDotNet assembly instead of legacy YAML support in Cuemon.Core assembly
- CultureInfoSurrogate class in the Cuemon.Extensions.Globalization namespace to use Cuemon.Extensions.YamlDotNet assembly instead of legacy YAML support in Cuemon.Core assembly
- tooling/gse to use Cuemon.Extensions.YamlDotNet assembly instead of legacy YAML support in Cuemon.Core assembly
- ExceptionDescriptorExtensions class in the Cuemon.Extensions.Diagnostics namespace to use Cuemon.Extensions.YamlDotNet assembly instead of legacy YAML support in Cuemon.Core assembly
- DefaultYamlConverter class in the Cuemon.Runtime.Serialization.Converters to include the ObsoleteAttribute with this message: All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version
- YamlSerializer class in the Cuemon.Runtime.Serialization to include the ObsoleteAttribute with this message: All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version
- YamlSerializerOptions class in the Cuemon.Runtime.Serialization to include the ObsoleteAttribute with this message: All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version
- YamlTextReader class in the Cuemon.Runtime.Serialization to include the ObsoleteAttribute with this message: All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version
- YamlTextWriter class in the Cuemon.Runtime.Serialization to include the ObsoleteAttribute with this message: All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version
- YamlTokenType enum in the Cuemon.Runtime.Serialization to include the ObsoleteAttribute with this message: All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version
- ExceptionDescriptorConverter class in the Cuemon.Text.Yaml.Converters to include the ObsoleteAttribute with this message: All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version
- YamlConverter class in the Cuemon.Text.Yaml.Converters to include the ObsoleteAttribute with this message: All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version
- YamlFormatter class in the Cuemon.Text.Yaml.Formatters to include the ObsoleteAttribute with this message: All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version
- YamlFormatterOptions class in the Cuemon.Text.Yaml.Formatters to include the ObsoleteAttribute with this message: All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version
- YamlConverterFactory class in the Cuemon.Text.Yaml to include the ObsoleteAttribute with this message: All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version
- YamlNamingPolicy class in the Cuemon.Text.Yaml to include the ObsoleteAttribute with this message: All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version

### Fixed

- Alphanumeric class in the Cuemon namespace so that the WhiteSpace constant field does not include the U+180E (Mongolian vowel separator) since it is no longer considered a white space as per Unicode 6.3.0
- ExceptionConverter class in the Cuemon.Text.Yaml.Converters namespace to use Environment.NewLine instead of Alphanumeric.NewLine (vital for non-Windows operating systems)
- ExceptionConverter class in the Cuemon.Extensions.Newtonsoft.Json.Converters namespace to use Environment.NewLine instead of Alphanumeric.NewLine (vital for non-Windows operating systems)
- ExceptionConverter class in the Cuemon.Extensions.Text.Json.Converters namespace to use Environment.NewLine instead of Alphanumeric.NewLine (vital for non-Windows operating systems)

## [8.2.0] - 2024-03-03

### Added

- AuthenticationHandlerFeature class in the Cuemon.AspNetCore.Authentication namespace that provides a combined default implementation of IAuthenticateResultFeature and IHttpAuthenticationFeature so that AuthenticateResult and User is consistent with each other
- SwaggerGenOptionsExtensions class in the Cuemon.Extensions.Swashbuckle.AspNetCore namespace was extended with one new extension method for the SwaggerGenOptions class: AddBasicAuthenticationSecurity
- GoneResult class in the Cuemon.AspNetCore.Mvc namespace that is an ActionResult that returns a Gone (410) response
- HostBuilderExtensions class in the Cuemon.Extensions.Hosting namespace that consist of extension methods for the IHostBuilder interface: ConfigureConfigurationSources and RemoveConfigurationSource (for .NET 6, .NET 7 and .NET 8)

### Changed

- BasicAuthenticationHandler class in the Cuemon.AspNetCore.Authentication.Basic namespace to propagate IAuthenticateResultFeature and IHttpAuthenticationFeature as part of HandleChallengeAsync
- DigestAuthenticationHandler class in the Cuemon.AspNetCore.Authentication.Digest namespace to propagate IAuthenticateResultFeature and IHttpAuthenticationFeature as part of HandleChallengeAsync
- HmacAuthenticationHandler class in the Cuemon.AspNetCore.Authentication.Hmac namespace to propagate IAuthenticateResultFeature and IHttpAuthenticationFeature as part of HandleChallengeAsync
- AuthorizationResponseHandler class in the Cuemon.Extensions.AspNetCore.Authentication namespace to rely on IAuthenticateResultFeature
- AuthorizationResponseHandlerOptions class in the Cuemon.Extensions.AspNetCore.Authentication namespace to include a function delegate property named AuthorizationFailureHandler that provides the reason/requirement/generic message of the failed authorization
- RestfulSwaggerOptions class in the Cuemon.Extensions.Swashbuckle.AspNetCore namespace to include a function delegate property named JsonSerializerOptionsFactory that will resolve a JsonSerializerOptions instance in a more flexible way than provided by the Swagger team
- ServiceCollectionExtensions class in the Cuemon.Extensions.Swashbuckle.AspNetCore namespace to support JsonSerializerOptionsFactory in the AddRestfulSwagger extension method
- Validator class in the Cuemon namespace to throw an ArgumentOutOfRangeException when ThrowIfUri is called with uriKind set to an indeterminate value of UriKind.RelativeOrAbsolute

### Fixed

- Validator class in the Cuemon namespace to have one less redundant ThrowIfNullOrWhitespace method while simplifying the remainder
- ParserFactory class in the Cuemon.Text namespace so that FromUri method now validates both Absolute and Relative URI correct (prior to this fix, only Absolute was validated correctly)

## [8.1.0] - 2024-02-11

### Added

- ApiKeySentinelAttribute class in the Cuemon.AspNetCore.Mvc.Filters.Headers namespace to provide a convenient way to protect your API with an ApiKeySentinelFilter
- ConfigurableAsyncAuthorizationFilter in the Cuemon.AspNetCore.Mvc.Filters namespace that provides a base class implementation of a filter that asynchronously confirms request authorization
- ForbiddenObjectResult in the Cuemon.AspNetCore.Mvc namespace that is an ObjectResult that when executed will produce a Forbidden (403) response
- ForbiddenResult in the Cuemon.AspNetCore.Mvc namespace that is an ActionResult that returns a Forbidden (403) response
- BasicAuthenticationHandler class in the Cuemon.AspNetCore.Authentication.Basic namespace to provide a HTTP Basic Authentication implementation of AuthenticationHandler{TOptions}
- AuthenticationBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Authentication namespace that consist of extension methods for the AuthenticationBuilder class: AddBasic, AddDigestAccess and AddHmac
- MvcBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Filters namespace that consist of extension methods for the IMvcBuilder interface: AddApiKeySentinelOptions, AddThrottlingSentinelOptions, AddUserAgentSentinelOptions, AddFaultDescriptorOptions and AddHttpCacheableOptions
- LoggerExtensions class in the Cuemon.Extensions.Xunit.Hosting namespace that consist of extension methods for the ILogger{T} interface: GetTestStore{T}
- ServiceCollectionExtensions class in the Cuemon.Extensions.Xunit.Hosting namespace that consist of extension methods for the IServiceCollection interface: AddXunitTestLogging
- TestLoggerEntry record in the Cuemon.Extensions.Xunit.Hosting namespace that represents a captured log-entry for testing purposes
- ServerTimingOptions class in the Cuemon.AspNetCore.Diagnostics namespace that provides configuration options for ServerTimingMiddleware and related
- XmlConverterExtensions class in the Cuemon.Extensions.AspNetCore.Xml.Converters namespace that consist of extension methods for the XmlConverter class: AddHttpExceptionDescriptorConverter, AddStringValuesConverter, AddHeaderDictionaryConverter, AddQueryCollectionConverter, AddFormCollectionConverter and AddCookieCollectionConverter
- ServiceCollectionExtensions class in the Cuemon.Extensions.AspNetCore.Xml.Formatters namespace that consist of extension methods for the IServiceCollection interface: AddXmlFormatterOptions and AddXmlExceptionResponseFormatter
- JsonConverterCollectionExtensions class in the Cuemon.Extensions.AspNetCore.Text.Json.Converters namespace that consist of extension methods for the JsonConverter class: AddHttpExceptionDescriptorConverter and AddStringValuesConverter
- ServiceCollectionExtensions class in the Cuemon.Extensions.AspNetCore.Text.Json.Formatters namespace that consist of extension methods for the IServiceCollection interface: AddJsonFormatterOptions and AddJsonExceptionResponseFormatter
- JsonConverterCollectionExtensions class in the Cuemon.Extensions.AspNetCore.Newtonsoft.Json.Converters namespace that consist of extension methods for the JsonConverter class: AddHttpExceptionDescriptorConverter and AddStringValuesConverter
- ServiceCollectionExtensions class in the Cuemon.Extensions.AspNetCore.Newtonsoft.Json.Formatters namespace that consist of extension methods for the IServiceCollection interface: AddNewtonsoftJsonFormatterOptions and AddNewtonsoftJsonExceptionResponseFormatter
- HttpExceptionDescriptorResponseFormatter{T} class in the Cuemon.AspNetCore.Diagnostics namespace that provides a generic way to support content negotiation for exceptions in the application
- IHttpExceptionDescriptorResponseFormatter interface in the Cuemon.AspNetCore.Diagnostics namespace that defines a way to support content negotiation for exceptions in the application
- IContentNegotiation interface in the Cuemon.Net.Http namespace that defines a way to support content negotiation for HTTP enabled applications
- ServiceProviderExtensions class in the Cuemon.Extensions.AspNetCore.Diagnostics namespace that consist of extension methods for the IServiceProvider interface: GetExceptionResponseFormatters
- HttpExceptionDescriptorResponseFormatterExtensions class in the Cuemon.Extensions.AspNetCore.Http namespace that consist of extension methods for the IHttpExceptionDescriptorResponseFormatter interface: SelectExceptionDescriptorHandlers
- ServiceCollectionExtensions class in the Cuemon.Extensions.AspNetCore.Text.Yaml.Formatters namespace that consist of extension methods for the IServiceCollection interface: AddYamlFormatterOptions and AddYamlExceptionResponseFormatter
- MvcBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml namespace that consist of extension methods for the IMvcBuilder interface: AddYamlFormatters and AddYamlFormattersOptions
- MvcCoreBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml namespace that consist of extension methods for the IMvcCoreBuilder interface: AddYamlFormatters and AddYamlFormattersOptions
- YamlSerializationInputFormatter class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml namespace that handles deserialization of YAML to objects using YamlFormatter
- YamlSerializationMvcOptionsSetup class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml namespace that is a ConfigureOptions{TOptions} implementation which will add the YAML serializer formatters to MvcOptions
- YamlSerializationOutputFormatter class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml namespace that handles serialization of objects to YAML using YamlFormatter
- MvcBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text namespace that consist of extension methods for the IMvcBuilder interface: AddExceptionDescriptorOptions
- MvcCoreBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text namespace that consist of extension methods for the IMvcCoreBuilder interface: AddExceptionDescriptorOptions
- ServiceProviderExtensions class in the Cuemon.Extensions.DependencyInjection namespace with extension methods for the IServiceProvider interface: GetServiceDescriptor
- AuthorizationResponseHandler class in the Cuemon.Extensions.AspNetCore.Authentication namespace that provides an opinionated implementation of IAuthorizationMiddlewareResultHandler that is optimized to deliver meaningful responses based on HTTP content negotiation
- AuthorizationResponseHandlerOptions class in the Cuemon.Extensions.AspNetCore.Authentication namespace that specifies options that is related to AuthorizationResponseHandler operations
- ServiceCollectionExtensions class in the Cuemon.Extensions.AspNetCore.Authentication namespace that consist of extension methods for the IServiceCollection interface: AddAuthorizationResponseHandler

### Fixed

- DigestAuthenticationOptions class in the Cuemon.AspNetCore.Authentication.Digest namespace to include UseServerSideHa1Storage that finally allows the server to bypass calculation of HA1 password representation
- ServerTimingFilter class in the Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace to only use embedded profiler when used in combination with ServerTimingAttribute
- HttpExceptionDescriptorResponseHandlerExtensions class in the Cuemon.Extensions.AspNetCore.Diagnostics namespace so that AddYamlResponseHandler now enumerates all supported media types in regards to content negotiation
- HttpExceptionDescriptorResponseHandlerExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml namespace so that AddXmlResponseHandler now enumerates all supported media types in regards to content negotiation
- HttpExceptionDescriptorResponseHandlerExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json namespace so that AddJsonResponseHandler now enumerates all supported media types in regards to content negotiation
- HttpExceptionDescriptorResponseHandlerExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace so that AddNewtonsoftJsonResponseHandler now enumerates all supported media types in regards to content negotiation
- HttpEntityTagHeaderFilter class in the Cuemon.AspNetCore.Mvc.Filters.Cacheable namespace so the new body stream is not disposed of prematurely leading to 500 errors on subsequent requests
- HttpCacheableFilter class in the Cuemon.AspNetCore.Mvc.Filters.Cacheable namespace so that an odd if statement is applied in general instead of confined to the scope of ObjectResult
- YamlTextWriter class in the Cuemon.Runtime.Serialization namespace to be slightly more compliant with the YAML standard (next major version will opt-in for a 3rd party library that adhere to the standard in both terms of serializing and deserializing)

### Changed

- ApiKeySentinelOptions class in the Cuemon.AspNetCore.Http.Headers namespace to include two new properties, GenericClientStatusCode and GenericClientMessage, rendering the existing BadRequestMessage property obsolete
- ApiKeySentinelFilter class in the Cuemon.AspNetCore.Mvc.Filters.Headers namespace from an action based filter to an authorization based filter
- BasicAuthenticationMiddleware class in the Cuemon.AspNetCore.Authentication.Basic namespace to be slightly more reusable in the confines of the Cuemon.AspNetCore.Authentication assembly
- BasicAuthenticationOptions class in the Cuemon.AspNetCore.Authentication.Basic namespace to include ValidateOptions to ensure that public read-write properties are in a valid state
- AuthenticationOptions class in the Cuemon.AspNetCore.Authentication namespace to inherit from AuthenticationSchemeOptions and implement IValidatableParameterObject (replacing earlier IParameterObject) that ensures UnauthorizedMessage property is in a valid state
- TryAuthenticate{T} signature on the static Authenticator class in the Cuemon.AspNetCore.Authentication namespace to honor the otherwise suggested Try-Parse pattern (although breaking the method was assessed to have low risk of external callers)
- Authenticate{T} signature on the static Authenticator class in the Cuemon.AspNetCore.Authentication namespace to return a ClaimsPrincipal (wrapped inside a ConditionalValue) instead of assigning this directly to the User of the HttpContext instance (although breaking the method was assessed to have low risk of external callers)
- DigestAuthenticationMiddleware class in the Cuemon.AspNetCore.Authentication.Digest namespace to be slightly more reusable in the confines of the Cuemon.AspNetCore.Authentication assembly
- DigestAuthenticationOptions class in the Cuemon.AspNetCore.Authentication.Digest namespace to include ValidateOptions to ensure that public read-write properties are in a valid state
- DigestAuthorizationHeaderBuilder class in the Cuemon.AspNetCore.Authentication.Digest namespace to include an extra overload of AddFromWwwAuthenticateHeader that accepts an instance of HttpResponseHeaders
- HmacAuthenticationMiddleware class in the Cuemon.AspNetCore.Authentication.Hmac namespace to be slightly more reusable in the confines of the Cuemon.AspNetCore.Authentication assembly
- HmacAuthenticationOptions class in the Cuemon.AspNetCore.Authentication.Hmac namespace to include ValidateOptions to ensure that public read-write properties are in a valid state
- HmacAuthorizationHeaderBuilder class in the Cuemon.AspNetCore.Authentication.Digest namespace to include an extra overload of AddFromRequest that accepts an instance of HttpRequestMessage
- HttpExceptionDescriptor class in the Cuemon.AspNetCore.Diagnostics namespace to favor default values from HttpStatusCodeException derived exceptions
- MiddlewareBuilderFactory class in the Cuemon.AspNetCore.Builder namespace to support validation of setup delegate when invoking UseConfigurableMiddleware{TMiddleware, TOptions}
- ServiceCollectionExtensions class in the Cuemon.Extensions.DependencyInjection namespace was extended with two new extension methods for the IServiceCollection interface: TryConfigure{TOptions} and PostConfigureAllOf{TOptions}
- NewtonsoftJsonFormatterOptions class in the Cuemon.Extensions.Newtonsoft.Json.Formatters namespace to derive from IExceptionDescriptorOptions
- JsonFormatterOptions class in the Cuemon.Extensions.Text.Json.Formatters namespace to derive from IExceptionDescriptorOptions
- XmlFormatterOptions class in the Cuemon.Xml.Serialization.Formatters namespace to derive from IExceptionDescriptorOptions
- Validator class in the Cuemon namespace to ease on constraint from IValidatableParameterObject --> IParameterObject on ThrowIfInvalidConfigurator and ThrowIfInvalidOptions; functionality remains the same but opens up for more flexibility
- ServerTimingMiddleware class in the Cuemon.AspNetCore.Diagnostics namespace to inherit from ConfigurableMiddleware with support for ILogger{ServerTimingMiddleware}, IHostEnvironment and ServerTimingOptions
- ServerTimingAttribute class in the Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace to include two new properties, ServerTimingLogLevel and EnvironmentName, while removing traces of excessive responsibility
- ServerTimingFilter class in the Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace to match the recent changes introduced to both ServerTimingMiddleware and ServerTimingAttribute
- FaultDescriptorOptions class in the Cuemon.AspNetCore.Diagnostics namespace marking the NonMvcResponseHandlers property obsolete
- HttpRequestExtensions class in the Cuemon.Extensions.AspNetCore.Http namespace was extended with one new extension method for the HttpRequest class: AcceptMimeTypesOrderedByQuality
- ApplicationBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Diagnostics namespace to allow the UseFaultDescriptorExceptionHandler method to rely solely on RequestServices internally, hence the previous setup delegate has been removed as an argument to this method
- HttpExceptionDescriptorResponseHandlerExtensions class in the Cuemon.Extensions.AspNetCore.Diagnostics namespace in the method AddYamlResponseHandler; from Action{ExceptionDescriptorOptions} setup --> IOptions{ExceptionDescriptorOptions} setup
- ServiceCollectionExtensions class in the Cuemon.Extensions.AspNetCore.Diagnostics namespace with four new extension method for the IServiceCollection interface: AddFaultDescriptorOptions, AddServerTimingOptions, AddExceptionDescriptorOptions and PostConfigureAllExceptionDescriptorOptions
- ServerTimingOptions class in the Cuemon.AspNetCore.Diagnostics namespace to include a new property, LogLevelSelector, with a function delegate signature that determines the LogLevel for a given ServerTimingMetric
- ServerTimingFilter class in the Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace to incorporate the new LogLevelSelector delegate and have less cluttered defaults in regards to Name and Description when used from ServerTimingAttribute
- ServerTimingAttribute class in the Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace to embrace the new capabilities provided by the ServerTimingFilter (such as DesiredLogLevel)
- HostingEnvironmentOptions class in the Cuemon.AspNetCore.Hosting namespace so that the SuppressHeaderPredicate function delegate now returns true when the environment is Production
- YamlSerializer class in the Cuemon.Runtime.Serialization namespace so it is consistent with other serializers/formatters in the Cuemon family
- YamlFormatter class in the Cuemon.Text.Yaml.Formatters so that it inherits from StreamFormatter{T} as the remainder of the formatters in the Cuemon family
- YamlFormatterOptions class in the Cuemon.Text.Yaml.Formatters so that it shares common denominators with the remainder of the formatter options in the Cuemon family
- YamlConverterExtensions class in the Cuemon.Extensions.AspNetCore.Text.Yaml.Converters so the signature of methods take an argument of YamlFormatterOptions instead of ExceptionDescriptorOptions (both implement IExceptionDescriptorOptions)
- ExceptionDescriptor was moved from Cuemon.Diagnostics namespace to Cuemon.Core assembly (retaining the namespace)
- ExceptionDescriptorAttribute was moved from Cuemon.Diagnostics namespace to Cuemon.Core assembly (retaining the namespace)
- ExceptionDescriptorOptions was moved from Cuemon.Diagnostics namespace to Cuemon.Core assembly (retaining the namespace)
- FaultSensitivityDetails was moved from Cuemon.Diagnostics namespace to Cuemon.Core assembly (retaining the namespace)
- IExceptionDescriptorOptions was moved from Cuemon.Diagnostics namespace to Cuemon.Core assembly (retaining the namespace)
- MemberEvidence was moved from Cuemon.Diagnostics namespace to Cuemon.Core assembly (retaining the namespace)
- ExceptionDescriptorConverter was moved from Cuemon.Diagnostics.Text.Yaml namespace to Cuemon.Core assembly into the Cuemon.Text.Yaml.Converters namespace
- ServiceCollectionExtensions class in the Cuemon.Extensions.AspNetCore.Diagnostics namespace to allow the AddMvcFaultDescriptorOptions method to accept an optional Action{MvcFaultDescriptorOptions} delegate, as opposed to being mandatory
- RestfulApiVersioningOptions class in the Cuemon.Extensions.Asp.Versioning namespace to include non-official MIME-types in the ValidAcceptHeaders property
- StreamFormatter{T} class in the Cuemon.Runtime.Serialization.Formatters namespace was extended to include an additional eight overloaded static members for SerializeObject and DeserializeObject (support for TOptions)
- HttpExceptionDescriptorResponseHandlerExtensions class in the Cuemon.Extensions.AspNetCore.Diagnostics marking the two methods, AddResponseHandler and AddYamlResponseHandler, obsolete (latter should use AddYamlExceptionResponseFormatter instead)
- HttpExceptionDescriptorResponseHandlerExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json marking the method, AddNewtonsoftJsonResponseHandler, obsolete (should use AddNewtonsoftJsonExceptionResponseFormatter instead)
- MvcBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace to be more lean having only two extension methods remaining; AddNewtonsoftJsonFormatters and AddNewtonsoftJsonFormattersOptions
- MvcCoreBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace to be more lean having only two extension methods remaining; AddNewtonsoftJsonFormatters and AddNewtonsoftJsonFormattersOptions
- HttpExceptionDescriptorResponseHandlerExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json marking the method, AddJsonResponseHandler, obsolete (should use AddJsonExceptionResponseFormatter instead)
- MvcBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json namespace to be more lean having only two extension methods remaining; AddJsonFormatters and AddJsonFormattersOptions
- MvcCoreBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json namespace to be more lean having only two extension methods remaining; AddJsonFormatters and AddJsonFormattersOptions
- HttpExceptionDescriptorResponseHandlerExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml marking the method, AddXmlResponseHandler, obsolete (should use AddXmlExceptionResponseFormatter instead)
- MvcBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml namespace to be more lean having only two extension methods remaining; AddXmlFormatters and AddXmlFormattersOptions
- MvcCoreBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml namespace to be more lean having only two extension methods remaining; AddXmlFormatters and AddXmlFormattersOptions
- Configurable{T} class in the Cuemon.Configuration namespace to have the constructor validated by Validator.ThrowIfInvalidOptions hereby reducing the risk of misconfigured Options
- ServiceCollectionExtensions class in the Cuemon.Extensions.AspNetCore.Http.Throttling namespace with one additional extension method for the IServiceCollection interface: AddThrottlingSentinelOptions
- MvcBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Filters namespace to allow all extension methods to have optional Action{TOptions} delegate
- HttpExceptionDescriptorResponseHandler class in the Cuemon.AspNetCore.Diagnostics namespace with a new static method; CreateDefaultFallbackHandler that provides a scaled down HttpExceptionDescriptorResponseHandler suitable as fallback handler for when content negotiation fails
- ConditionalValue class from the Cuemon.Threading namespace was moved to the Cuemon namespace
- SuccessfulValue class from the Cuemon.Threading namespace was moved to the Cuemon namespace
- UnsuccessfulValue class from the Cuemon.Threading namespace was moved to the Cuemon namespace
- ConditionalValue class in the Cuemon namespace to include a Failure property that is of type Exception
- AsyncOptions class in the Cuemon.Threading namespace to include a function delegate property, CancellationTokenProvider, that takes precedence when set, meaning that the getter of existing CancellationToken property will invoke said mentioned function delegate (edge case usage)
- ServerTimingOptions class in the Cuemon.AspNetCore.Diagnostics namespace to include a new property, UseTimeMeasureProfiler, with a boolean signature that determines if action methods in a Controller should time measuring automatically

## [8.0.1] - 2024-01-11

### Fixed

- CachingManager class in the Cuemon.Runtime.Caching namespace so that it no longer throws a MissingMethodException due to changes introduced with target-typed new from C# 9
- NewtonsoftJsonFormatterOptions class in the Cuemon.Extensions.Newtonsoft.Json.Formatters namespace to be consistent with general date time handling; applied DateFormatString = "O"

## [8.0.0] - 2023-11-14

### Added

- TFM net8.0 for all projects
- Tool for extracting NLS surrogates `tooling/gse` (Globalization Surrogates Extractor); this was done to mitigate the original design decision that was most [unfortunate](https://github.com/gimlichael/Cuemon/commit/71ff4f9ecb95897170aab1e6ba894c320ae095bd)
- Static method `CreateFlags` to the MemberReflection class in the Cuemon.Reflection namespace
- MethodSignature class in the Cuemon.Reflection namespace to represent a lightweight signature of a method when serializing and deserializing
- Formatter class in the Cuemon.Runtime.Serialization.Formatters namespace that complements serialization and deserialization of an object
- `GetAllProperties`, `GetAllFields`, `GetAllEvents`, `GetAllMethods`, `GetDerivedTypes`, `GetInheritedTypes` and `GetHierarchyTypes` extension methods on the TypeExtensions class in the Cuemon.Extensions.Reflection namespace
- ReplaceLineEndings extension method on the StringExtensions class in the Cuemon.Extensions namespace specifically for TFM netstandard2.0
- MemberArgument class in the Cuemon.Reflection namespace to represent an argument given to a member in the context of reflection
- MemberParser class in the Cuemon.Reflection namespace to provide a generic way to rehydrate serialized objects
- Docker-Ubuntu profile to `testenvironments.json` for further local testing in a Linux environment
- Static method `EncloseToExpose` together with a new property, `ArgumentName`, to the Decorator class in the Cuemon namespace that can be used to re-use non-common extension methods from native extension methods without double-validating arguments
- An overload of `CheckParameter` to the Validator class in the Cuemon namespace that satisfies validating when doing constructor nesting
- DataManagerOptions in the Cuemon.Data namespace that provides configuration options for the DataManager class
- DataStatementOptions in the Cuemon.Data namespace that provides configuration options for the DataStatement class
- ThrowIfDisposed to the Validator class in the Cuemon namespace
- `ToYaml` extension method on the ExceptionDescriptorExtensions class in the Cuemon.Extensions.Diagnostics namespace
- CorrelationToken class in the Cuemon.Messaging namespace that represents a default implementation of the ICorrelationToken interface
- RequestToken class in the Cuemon.Messaging namespace that represents a default implementation of the IRequestToken interface

### Removed

- TFM netstandard2.0 for all ASP.NET Core related projects
- Due to [Legacy serialization infrastructure APIs marked obsolete](https://github.com/dotnet/docs/issues/34893) all `SerializableAttribute` and `ISerializable` implementations was decided removed for all TFMs
- MethodDescriptor in the Cuemon.Reflection namespace was slightly refactored in regards to members exposed
- DataAdapter class from the Cuemon.Data namespace
- DataAdapterEventArgs class from the Cuemon.Data namespace
- DataAdapterException class in the Cuemon.Data namespace
- DataConnection class in the Cuemon.Data namespace
- DbColumn class in the Cuemon.Data namespace
- DbParameterEqualityComparer class in the Cuemon.Data namespace
- IDataCommand interface in the Cuemon.Data namespace
- IDataConnection interface in the Cuemon.Data namespace
- QueryInsertAction enum in the Cuemon.Data namespace
- GenericHostTestFactory had two obsolete methods removed; CreateGenericHostTest{..}
- ToInsightsString extension method on the ExceptionDescriptorExtensions class in the Cuemon.Extensions.Diagnostics namespace
- DynamicCorrelation class in the Cuemon.Messaging namespace
- DynamicRequest class in the Cuemon.Messaging namespace

### Changed

- Extended unit-test to include TFM net8.0, net7.0, net6.0 and net48 for Windows
  - Had to include Microsoft.TestPlatform.ObjectModel for xUnit when testing on legacy .NET Framework
- Extended unit-test to include TFM net8.0, net7.0 and net6.0 for Linux
- Many unit-test had to be tweaked with preprocessor directives due to the addition of TFM net48
- Validator class in the Cuemon namespace was modernized and greatly improved for both consistency and changes introduced by Microsoft for both C# 10 and recent .NET versions. All excessive fats was removed and earlier brain-farts has been eradicated
- DateParseHandling from `DateTimeOffset` to `DateTime` (as majority of Cuemon is the latter) on the JsonFormatterOptions class in the Cuemon.Extensions.Newtonsoft.Json.Formatters namespace
- ContractResolver to use custom rules as Newtonsoft relies heavily on the now deprecated ISerializable and SerializableAttribute
- ChangeType (hidden) extension method to always convert DateTime values ending with Z to an UTC DateTime kind on the ObjectDecoratorExtensions class in the Cuemon namespace
- JsonFormatterOptions class in the Cuemon.Extensions.Text.Json.Formatters namespace to, consciously, use `JavaScriptEncoder.UnsafeRelaxedJsonEscaping` as the default Encoder on the JsonSerializerOptions instance
  - Sometime you have to balance security and usability/developer experience; if you need to expose a highly secured API you can simply change this settings as part of your application startup
- XmlFormatter class in the Cuemon.Xml.Serialization.Formatters namespace now inherits from StreamFormatter{XmlFormatterOptions} and is consistent with similar classes
- Best effort to have consistency between System.Text.Json and Newtonsoft.Json serialization/deserialization
- Changed the description of the Decorator class in the Cuemon namespace to add clarity to usage
- Renamed DataCommand class in the Cuemon.Data namespace to DataStatement and increased the scope of responsibility
- SqlDataManager class in the Cuemon.Data.SqlClient namespace to be more lean and consistent with other classes and fully embracing the configurable DataManagerOptions
- DataManager class in the Cuemon.Data namespace to be more lean and consistent with other classes and fully embracing the configurable DataManagerOptions
  - Including support for Async operations
- Simplified the FormattingOptions class in the Cuemon namespace away from a somewhat confusing generic variant to a straight to the point implementation
- DataReader class in the Cuemon.Data namespace to rely only on a default constructor
- DsvDataReader class in the Cuemon.Data namespace to be more consistent with other classes fully embracing the configurable DelimitedStringOptions
- XmlDataReader class in the Cuemon.Data.Xml namespace to be more consistent with other classes fully embracing the configurable FormattingOptions
- Renamed ThrowIfObjectInDistress method on the Validator class in the Cuemon namespace to ThrowIfInvalidState
- MiddlewareTestFactory received a long overdue change of signature from Action{IApplicationBuilder} pipelineSetup, Action{IServiceCollection} serviceSetup --> Action{IServiceCollection} serviceSetup, Action{IApplicationBuilder} pipelineSetup as this is more logical, intuitive and in consistency with GenericHostTestFactory
- WebApplicationTestFactory received a long overdue change of signature from Action{IApplicationBuilder} pipelineSetup, Action{IServiceCollection} serviceSetup --> Action{IServiceCollection} serviceSetup, Action{IApplicationBuilder} pipelineSetup as this is more logical, intuitive and in consistency with GenericHostTestFactory
- JsonFormatter class in the Cuemon.Extensions.Newtonsoft.Json.Formatters namespace was renamed to NewtonsoftJsonFormatter
- JsonFormatterOptions class in the Cuemon.Extensions.Newtonsoft.Json.Formatters namespace was renamed to NewtonsoftJsonFormatterOptions
- ICorrelation interface in the Cuemon.Messaging namespace was renamed to ICorrelationToken
- IRequest interface in the Cuemon.Messaging namespace was renamed to IRequestToken

### Fixed

- National Language Support (NLS) surrogates was updated in the Cuemon.Extensions.Globalization assembly
- World class in the Cuemon.Globalization namespace so that it no longer throws an ArgumentException when adding a duplicate culture (on Linux)
- The default DateTimeConverter for serializing XML no longer converts a DateTime value to UTC
- AddNewtonsoftJsonResponseHandler extension method to properly propagate options to NewtonsoftJsonFormatter serialization method in the HttpExceptionDescriptorResponseHandlerExtensions in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace
- AddJsonResponseHandler extension method to properly propagate options to JsonFormatter serialization method in the HttpExceptionDescriptorResponseHandlerExtensions in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json namespace
- AddXmlResponseHandler extension method to properly propagate options to XmlFormatter serialization method in the HttpExceptionDescriptorResponseHandlerExtensions in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml namespace
- Header is now populated on first read in the DsvDataReader class located in the Cuemon.Data namespace
- Columns are now populated before first read in the IDataReader implementation passed to the DataTransferRowCollection class in the Cuemon.Data namespace
- Added null conditional operator to the ServiceProvider property on the HostFixture class in the Cuemon.Extensions.Xunit.Hosting namespace
- WriteObject to skip serializing the object when it is null on the YamlTextWriter class in the Cuemon.Runtime.Serialization namespace

## [7.1.0] 2022-12-11

### Added

- Alter{T}, Change{T, TResult} methods on Tweaker class in the Cuemon namespace
- As extension method on the ObjectExtensions class on the Cuemon.Extensions namespace
- As{T, TResult}, Alter{T} extension methods on the ObjectExtensions class in the Cuemon.Extensions namespace
- UseBuiltInRfc7807 property on the RestfulApiVersioningOptions class in the Cuemon.Extensions.Asp.Versioning namespace (https://github.com/dotnet/aspnet-api-versioning/releases/tag/v7.0.0)

### Removed

- Support for TFM .NET Core 3.1 (LTS) in all relevant projects
- GetValueOrDefault{TKey, TValue} overload from the DictionaryExtensions class in the Cuemon.Extensions.Collections.Generic namespace due to naming conflict introduced with .NET 7
- ProblemDetailsFactoryType and UseProblemDetailsFactory{T} from the RestfulApiVersioningOptions class in the Cuemon.Extensions.Asp.Versioning namespace when targeting .NET 7 (https://github.com/dotnet/aspnet-api-versioning/releases/tag/v7.0.0)

### Fixed

- As{T} overload on ObjectExtensions class in the Cuemon.Extensions namespace due to wrongfully throw of an ArgumentNullException when argument was null
- Adjust{T} overload on ObjectExtensions class in the Cuemon.Extensions namespace to fail fast by adding null check on argument
- ReservedKeywordException class in the Cuemon namespace so the default message is used in case of null value
- RestfulProblemDetailsFactory class in the Cuemon.Extensions.Asp.Versioning namespace due to changes for 6.3 release of Asp.Versioning (https://github.com/dotnet/aspnet-api-versioning/commit/0a999316aebc81fb1bf3842a2980901f9539978b)

### Changed

- GenericHostTestFactory class in the Cuemon.Extensions.Xunit.Hosting namespace to have non-ambiguous overloads of CreateGenericHostTest -> Create, CreateWithHostBuilderContext (old methods marked with Obsolete attribute)
- ServiceCollectionExtensions class in the Cuemon.Extensions.Asp.Versioning namespace so that AddRestfulApiVersioning now is backward compatible with recent changes mentioned here https://github.com/dotnet/aspnet-api-versioning/releases/tag/v7.0.0

## [7.0.0] 2022-11-09

### Added

- Target framework moniker (TFM) for .NET 7 in all projects
- HttpAuthenticationSchemes class in the Cuemon.Net.Http namespace that specifies options that defines constants for well-known HTTP authentication schemes
- HttpHeaderNames class in the Cuemon.Net.Http namespace that defines constants for well-known HTTP headers
- FaultDescriptorExceptionHandlerOptions class in the Cuemon.AspNetCore.Diagnostics namespace that specifies options that is related to ExceptionHandlerMiddleware operations
- HttpFaultResolver class in the Cuemon.AspNetCore.Diagnostics namespace that provides a way to evaluate an exception and provide details about it in a developer friendly way, optimized for open- and otherwise public application programming interfaces (API)
- HttpRequestEvidence class in the Cuemon.AspNetCore.Diagnostics namespace that provides detailed information about a given HttpRequest
- JsonConverterCollectionExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.Converters namespace that consist of extension methods for the JsonConverter class: AddHttpExceptionDescriptorConverter, AddStringValuesConverter
- JsonSerializationInputFormatter class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json namespace that handles deserialization of JSON to objects using JsonFormatter class
- JsonSerializationMvcOptionsSetup class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json namespace that is a ConfigureOptions{TOptions} implementation which will add the JSON serializer formatters to MvcOptions
- JsonSerializationOutputFormatter class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json namespace that handles serialization of objects to JSON using JsonFormatter class
- MvcBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json namespace that consist of extension methods for the IMvcBuilder interface: AddJsonFormatters, AddJsonFormattersOptions
- MvcCoreBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json namespace that consist of extension methods for the IMvcCoreBuilder interface: AddJsonFormatters, AddJsonFormattersOptions
- JsonConverterCollectionExtensions class in the Cuemon.Extensions.Text.Json.Converters namespace that consist of extension methods for the JsonConverter class: AddStringEnumConverter, AddStringFlagsEnumConverter, AddExceptionDescriptorConverterOf, AddTimeSpanConverter, AddExceptionConverter, AddDataPairConverter, AddDateTimeConverter
- StringEnumConverter class in the Cuemon.Extensions.Text.Json.Converters namespace that convert enums to and from strings
- StringFlagsEnumConverter class in the Cuemon.Extensions.Text.Json.Converters namespace that convert enums with FlagsAttribute to and from strings
- JsonFormatter class in the Cuemon.Extensions.Text.Json.Formatters namespace that serializes and deserializes an object, in JSON format
- JsonFormatterOptions class in the Cuemon.Extensions.Text.Json.Formatters namespace that specifies options that is related to JsonFormatter operations
- DynamicJsonConverter class in the Cuemon.Extensions.Text.Json namespace that provides a factory based way to create and wrap a JsonConverter implementation
- JsonNamingPolicyExtensions class in the Cuemon.Extensions.Text.Json namespace that consist of extension methods for the JsonNamingPolicy class: DefaultOrConvertName
- JsonReaderExtensions class in the Cuemon.Extensions.Text.Json namespace that consist of extension methods for the Utf8JsonReader struct: ToHierarchy
- JsonSerializerOptionsExtensions class in the Cuemon.Extensions.Text.Json namespace that consist of extension methods for the JsonSerializerOptions class: SetPropertyName
- Utf8JsonReaderFunc delegate in the Cuemon.Extensions.Text.Json namespace that represents the Read method of JsonConverter{T}
- Utf8JsonWriterAction delegate in the Cuemon.Extensions.Text.Json namespace that represents the Write method of JsonConverter{T}
- Utf8JsonWriterExtensions class in the Cuemon.Extensions.Text.Json namespace that consist of extension methods for the Utf8JsonWriter class: WriteObject
- DateTimeConverter class in the Cuemon.Extensions.Text.Json.Converters namespace that provides a DateTime converter that can be configured like the Newtonsoft.JSON equivalent
- BufferWriterOptions class in the Cuemon.IO namespace that specifies options that is related to the IBufferWriter interface
- RestfulApiVersioningOptions class in the Cuemon.Extensions.Asp.Versioning namespace that provides programmatic configuration for the ServiceCollectionExtensions.AddRestfulApiVersioning method
- ServiceCollectionExtensions class in the Cuemon.Extensions.Asp.Versioning namespace that consist of extension methods for the IServiceCollection interface: AddRestfulApiVersioning
- ApplicationBuilderExtensions class in the Cuemon.Extensions.Asp.Versioning namespace that consist of extension methods for the IApplicationBuilder interface: UseRestfulApiVersioning
- RestfulApiVersionReader class in the Cuemon.Extensions.Asp.Versioning namespace that represents a RESTful API version reader that reads the value from a filtered list of HTTP Accept headers in the request
- RestfulProblemDetailsFactory class in the Cuemon.Extensions.Asp.Versioning namespace that represents a RESTful implementation of the IProblemDetailsFactory which throws variants of HttpStatusCodeException that needs to be translated accordingly
- Aws4HmacAuthorizationHeader class in the Cuemon.Extensions.AspNetCore.Authentication.AwsSignature4 namespace that provides a representation of a HTTP AWS4-HMAC-SHA256 Authentication header
- Aws4HmacAuthorizationHeaderBuilder class in the Cuemon.Extensions.AspNetCore.Authentication.AwsSignature4 namespace that provides a way to fluently represent a HTTP AWS4-HMAC-SHA256 Authentication header
- Aws4HmacFields class in the Cuemon.Extensions.AspNetCore.Authentication.AwsSignature4 namespace that is a collection of constants for Aws4HmacAuthorizationHeaderBuilder and related
- DateTimeExtensions class in the Cuemon.Extensions.AspNetCore.Authentication.AwsSignature4 namespace that consist of extension methods for the DateTime struct: ToAwsDateString, ToAwsDateTimeString
- ConfigureSwaggerGenOptions class in the Cuemon.Extensions.Swashbuckle.AspNetCore namespace that represents something that configures the SwaggerGenOptions type
- ConfigureSwaggerUIOptions class in the Cuemon.Extensions.Swashbuckle.AspNetCore namespace that represents something that configures the SwaggerUIOptions type
- DocumentFilter class in the Cuemon.Extensions.Swashbuckle.AspNetCore namespace that represents the base class of an IDocumentFilter implementation
- OpenApiInfoOptions class in the Cuemon.Extensions.Swashbuckle.AspNetCore namespace that represents a proxy for configuring an Open API Info Object that provides metadata about an Open API endpoint
- OperationFilter class in the Cuemon.Extensions.Swashbuckle.AspNetCore namespace that represents the base class of an IOperationFilter implementation
- RestfulSwaggerOptions class in the Cuemon.Extensions.Swashbuckle.AspNetCore namespace that provides programmatic configuration for the ServiceCollectionExtensions.AddRestfulSwagger method
- ServiceCollectionExtensions class in the Cuemon.Extensions.Swashbuckle.AspNetCore namespace that consist of extension methods for the IServiceCollection interface: AddRestfulSwagger
- SwaggerGenOptionsExtensions class in the Cuemon.Extensions.Swashbuckle.AspNetCore namespace that consist of extension methods for the SwaggerGenOptions class: AddUserAgent, AddXApiKeySecurity, AddJwtBearerSecurity
- UserAgentDocumentFilter class in the Cuemon.Extensions.Swashbuckle.AspNetCore namespace that provides a User-Agent field to the generated OpenApiDocument
- UserAgentDocumentOptions class in the Cuemon.Extensions.Swashbuckle.AspNetCore namespace that provides programmatic configuration for the UserAgentDocumentFilter class
- XPathDocumentExtensions class in the Cuemon.Extensions.Swashbuckle.AspNetCore namespace that consist of extension methods for the XPathDocument class: AddByType, AddByAssembly, AddByFilename
- JsonFormatter class in the Cuemon.Extensions.Newtonsoft.Json.Formatters namespace was extended with two static methods; SerializeObject and DeserializeObject
- XmlFormatter class in the Cuemon.Xml.Serialization.Formatters namespace was extended with two static methods; SerializeObject and DeserializeObject
- YamlSerializer class in the Cuemon.Runtime.Serialization namespace that provides functionality to serialize objects to YAML and to deserialize YAML into objects
- YamlSerializerOptions class in the Cuemon.Runtime.Serialization namespace that specifies options related to YamlSerializer
- YamlTextReader class in the Cuemon.Runtime.Serialization namespace that represents a reader that provides fast, non-cached, forward-only access to YAML data
- YamlTextWriter class in the Cuemon.Runtime.Serialization namespace that represents a writer that provides a fast, non-cached, forward-only way to generate streams or files that contain YAML data
- ExceptionConverter class in the Cuemon.Text.Yaml.Converters namespace that converts an Exception to or from YAML
- YamlConverter class in the Cuemon.Text.Yaml.Converters namespace that converts an object to or from YAML (YAML ain't markup language)
- YamlFormatter class in the Cuemon.Text.Yaml.Formatters namespace that serializes and deserializes an object, in YAML format
- YamlFormatterOptions class in the Cuemon.Text.Yaml.Formatters namespace that specifies options related to YamlFormatter
- YamlConverterFactory class in the Cuemon.Text.Yaml namespace that provides a factory based way to create and wrap an YamlConverter implementation
- YamlNamingPolicy class in the Cuemon.Text.Yaml namespace that determines the naming policy used to convert a string-based name to another format
- ExceptionDescriptorConverter class in the Cuemon.Diagnostics.Text.Yaml namespace that converts an ExceptionDescriptor to or from YAML
- YamlConverterExtensions class in the Cuemon.Extensions.AspNetCore.Text.Yaml.Converters namespace that consist of extension methods for the YamlConverter class: AddExceptionDescriptorConverter, AddHttpExceptionDescriptorConverter
- HttpExceptionDescriptorResponseHandler class in the Cuemon.AspNetCore.Diagnostics namespace that provides a way to support content negotiation for HttpExceptionDescriptor
- HttpExceptionDescriptorResponseHandlerExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace that consist of extension methods for the HttpExceptionDescriptorResponseHandler class: AddNewtonsoftJsonResponseHandler
- HttpExceptionDescriptorResponseHandlerExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json namespace that consist of extension methods for the HttpExceptionDescriptorResponseHandler class: AddJsonResponseHandler
- HttpExceptionDescriptorResponseHandlerExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml namespace that consist of extension methods for the HttpExceptionDescriptorResponseHandler class: AddXmlResponseHandler
- IExceptionDescriptorOptions interface in the Cuemon.Diagnostics namespace that defines options that is related to ExceptionDescriptor operations
- ExceptionConverter class in the Cuemon.Extensions.Newtonsoft.Json.Converters namespace that converts an Exception to or from JSON
- ExceptionConverter class in the Cuemon.Extensions.Text.Json.Converters namespace that converts an Exception to or from JSON
- ExceptionConverter class in the Cuemon.Xml.Serialization.Converters namespace that converts an Exception to XML
- ServiceOptions class in the Cuemon.Extensions.DependencyInjection namespace that specifies options related to Microsoft Dependency Injection
- TypeForwardServiceOptions class in the Cuemon.Extensions.DependencyInjection namespace that specifies options related to Microsoft Dependency Injection that support nested type forwarding
- CultureInfoExtensions class in the Cuemon.Extensions.Globalization namespace that consist of extension methods for the CultureInfo class: MergeWithOriginalFormatting
- CacheableObjectResultOptions{T} class in the Cuemon.AspNetCore.Mvc namespace that specifies options related to the ICacheableObjectResult interface
- ContentBasedObjectResultOptions{T} class in the Cuemon.AspNetCore.Mvc namespace that specifies options related to the ICacheableObjectResult interface
- IContentBasedObjectResultOptions{T} interface in the Cuemon.AspNetCore.Mvc namespace that specifies options related to the ICacheableObjectResult interface
- ITimeBasedObjectResultOptions{T} interface in the Cuemon.AspNetCore.Mvc namespace that specifies options related to the ICacheableObjectResult interface
- TimeBasedObjectResultOptions{T} class in the Cuemon.AspNetCore.Mvc namespace that specifies options related to the ICacheableObjectResult interface
- IParameterObject interface in the Cuemon.Configuration namespace that serves as a marker interface denoting a Parameter Object
- IValidatableParameterObject interface in the Cuemon.Configuration namespace that denotes a Parameter Object where one or more conditions can be verified that they are in a valid state
- Patterns class in the Cuemon namespace was extended with two new static members: ConfigureRevertExchange{T}, CreateInstance{T}
- Validator class in the Cuemon namespace was extended with several new static members: ThrowIfInvalidConfigurator{T}, ThrowIfInvalidOptions{T}, ThrowIfObjectInDistress
- Eradicate class in the Cuemon namespace was extended with one new static member: TrailingBytes
- CallerArgumentExpressionAttribute class in the System.Runtime.CompilerServices namespace that indicates that a parameter captures the expression passed for another parameter as a string (for .NET versions <= .NET Core 3.0)

### Changed

- ThrowIfNull{T} --> ThrowIfNull method on the Validator class in the Cuemon namespace
- ThrowIfNull, ThrowIfNullOrEmpty and ThrowIfNullOrWhitespace on the Validator class in the Cuemon namespace to embrace the new CallerArgumentExpression attribute
- ThrottlingSentinelOptions class in the Cuemon.AspNetCore.Http.Throttling namespace in the context of renaming the ResponseBroker property to ResponseHandler
- HttpStatusCodeException class in the Cuemon.AspNetCore.Http namespace to include a new property where HTTP response Headers can be associated with the exception
- UserAgentSentinelMiddleware class in the Cuemon.AspNetCore.Http.Headers namespace to have a more lean and fault tolerant design
- ThrottlingSentinelMiddleware class in the Cuemon.AspNetCore.Http.Throttling namespace to have a more lean and fault tolerant design
- HttpContextItemsKeyForCapturedRequestBody string constant from FaultDescriptorFilter class in the Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace was moved to the HttpRequestEvidence class in the Cuemon.AspNetCore.Diagnostics namespace
- FaultDescriptorFilter class in the Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace to include a reference to HttpContext from the ExceptionCallback delegate
- FaultDescriptorOptions class in the Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace to be named MvcFaultDescriptorOptions and inherit from FaultDescriptorOptions class in the Cuemon.AspNetCore.Diagnostics namespace
- FaultDescriptorFilter class in the Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace to rely on MvcFaultDescriptorOptions instead of FaultDescriptorOptions
- HttpFaultResolver class from the Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace was moved to the Cuemon.AspNetCore.Diagnostics namespace
- HttpRequestEvidence class from the Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace was moved to the Cuemon.AspNetCore.Diagnostics namespace
- UserAgentSentinelFilter class in the Cuemon.AspNetCore.Mvc.Filters.Headers namespace received a more lean and fault tolerant design
- ThrottlingSentinelFilter class in the Cuemon.AspNetCore.Mvc.Filters.Throttling namespace received a more lean and fault tolerant design
- ApplicationBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Diagnostics namespace with one new extension method for the IApplicationBuilder interface: UseFaultDescriptorExceptionHandler
- MvcBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace in the context of renaming the AddJsonSerializationFormatters method to AddNewtonsoftJsonFormatters
- MvcBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace in the context of renaming the AddJsonFormatterOptions method to AddNewtonsoftJsonFormattersOptions
- MvcCoreBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace in the context of renaming the AddJsonSerializationFormatters method to AddNewtonsoftJsonFormatters
- MvcCoreBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace in the context of renaming the AddJsonFormatterOptions method to AddNewtonsoftJsonFormattersOptions
- MvcBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml namespace in the context of renaming the AddXmlSerializationFormatters method to AddXmlFormatters
- MvcBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml namespace in the context of renaming the AddXmlFormatterOptions method to AddXmlFormattersOptions
- MvcCoreBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml namespace in the context of renaming the AddXmlSerializationFormatters method to AddXmlFormatters
- MvcCoreBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml namespace in the context of renaming the AddXmlFormatterOptions method to AddXmlFormattersOptions
- StreamFactory class in the Cuemon.IO namespace to include five new overloaded methods that accept an IBufferWriter{byte} as the first argument
- HttpCacheableOptions class in the Cuemon.AspNetCore.Mvc.Filters.Cacheable namespace so that the age of a dynamically applied cache header is now 5 minutes instead of 24 hours
- MiddlewareTestFactory class in the Cuemon.Extensions.Xunit.Hosting.AspNetCore namespace to have non-ambiguous overloads of CreateMiddlewareTest -> Create, CreateWithHostBuilderContext and RunMiddlewareTest -> Run, RunWithHostBuilderContext
- WebApplicationTestFactory class in the Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc namespace to have non-ambiguous overloads of CreateWebApplicationTest -> Create, CreateWithHostBuilderContext and RunWebApplicationTest -> Run, RunWithHostBuilderContext
- AddTimeSpanConverter extension method on XmlConverterDecoratorExtensions class in the Cuemon.Xml.Serialization.Converters namespace to be aligned with the JSON equivalents
- ActivatorOptions class in the Cuemon.Reflection namespace to match Activator BindingFlags defaults from Microsoft
- TypeNameOptions class in the Cuemon.Reflection namespace that could throw a NullReferenceException when FullName is null; fallback to Name
- JsonFormatterOptions class in the Cuemon.Extensions.Newtonsoft.Json.Formatters namespace to use DateTimeZoneHandling.RoundtripKind instead of DateTimeZoneHandling.Utc when dealing with DateTimeZoneHandling
- FaultDescriptorExceptionHandlerOptions class in the Cuemon.AspNetCore.Diagnostics namespace was renamed to FaultDescriptorOptions
- ExceptionDescriptorOptions class in the Cuemon.Diagnostics namespace to exclude all sensitive details of a failure while refactoring all bool properties into one SensitivityDetails property that uses the FaultSensitivityDetails enum (with FlagsAttribute)
- HttpStatusCodeException class in the Cuemon.AspNetCore.Http namespace was extended with one new overloaded static member: TryParse
- ServiceCollectionExtensions class in the Cuemon.Extensions.DependencyInjection namespace was extended with twelve new overloaded extension methods for the IServiceCollection interface: Add, TryAdd
- CacheableObjectResultExtensions in the Cuemon.Extensions.AspNetCore.Mvc namespace to exclude non-generic MakeCacheable methods
- MakeCacheable{T} --> WithLastModifiedHeader{T}, WithEntityTagHeader{T} and WithCacheableHeaders{T} on the CacheableObjectResultExtensions class in the Cuemon.Extensions.AspNetCore.Mvc namespace
- Configure{T}, ConfigureExchange{T} and ConfigureRevertExchange{T} on the Patterns class in the Cuemon namespace to have type conditions include IParameterObject
- FaultDescriptorOptions class in the Cuemon.AspNetCore.Diagnostics namespace so that it matches the recent changes to IExceptionDescriptorOptions from the Cuemon.Diagnostics namespace
- FaultDescriptorOptions class in the Cuemon.AspNetCore.Diagnostics namespace so that function delegate RequestBodyParser was replaced by RequestEvidenceProvider to accommodate the changes introduced by IExceptionDescriptorOptions refactoring

### Removed

- Support for TFM .NET 5 (STS) in all relevant projects
- CdnTagHelper class from the Cuemon.AspNetCore.Razor.TagHelpers namespace
- CdnUriScheme class from the Cuemon.AspNetCore.Razor.TagHelpers namespace
- ImageCdnTagHelper class from the Cuemon.AspNetCore.Razor.TagHelpers namespace
- LinkCdnTagHelper class from the Cuemon.AspNetCore.Razor.TagHelpers namespace
- ScriptCdnTagHelper class from the Cuemon.AspNetCore.Razor.TagHelpers namespace
- IMvcFilterTest interface from the Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc
- MvcFilterTestFactory class from the Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc
- AddTimeSpanConverter extension method from JsonConverterCollectionExtensions class in the Cuemon.Extensions.Newtonsoft.Json.Converters namespace
- CheckParameter overloads from the Validator class in the Cuemon namespace
- ExceptionDescriptorExtensions class from the Cuemon.Extensions.Newtonsoft.Json.Diagnostics namespace
- ExceptionDescriptorExtensions class from the Cuemon.Extensions.Xml.Serialization.Diagnostics namespace

### Fixed

- HttpCacheableFilter class in the Cuemon.AspNetCore.Mvc.Filters.Cacheable namespace so that logic is only applied if qualified and that response has not started
- FakeHttpResponseFeature class in the Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http.Features namespace so that the OnStarting method ensures that callback delegate is only run once per response
- ServerTimingFilter class in the Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace that was triggered when parsing runtime parameters for time measuring and parameters exceeded what was part of route
- StringFlagsEnumConverter class in the Cuemon.Extensions.Newtonsoft.Json.Converters namespace so that it includes check on FlagsAttribute definition in inherited CanConvert method
- StreamOutputFormatter class in the Cuemon.AspNetCore.Mvc.Formatters namespace so that only non-nullable objects are being serialized
- World class in the Cuemon.Globalization namespace to exclude CultureInfo with LCID value of 127 (triggered exception on Alpine OS)
- HttpEntityTagHeaderFilter class in the Cuemon.AspNetCore.Mvc.Filters.Cacheable namespace that was triggered when UseEntityTagResponseParser was set to true and no cacheable object was returned

## [6.4.1] - 2022-05-08

### Changed

- InMemoryTestStore class in the Cuemon.Extensions.Xunit namespace to be more flexible and allow instantiation

## [6.4.0] - 2022-01-24

### Added

- ConditionalValue class in the Cuemon.Threading namespace that represents the base class to determine whether an async operation was a success or not
- ConditionalValue{TResult} class in the Cuemon.Threading namespace that represents the base class to support the Try-Parse pattern of an async operation
- SuccessfulValue class in the Cuemon.Threading namespace that provides a way to indicate a successful async operation
- SuccessfulValue{TResult} class in the Cuemon.Threading namespace that provides a way to indicate a successful async operation
- UnsuccessfulValue class in the Cuemon.Threading namespace that provides a way to indicate a faulted async operation
- UnsuccessfulValue{TResult} class in the Cuemon.Threading namespace that provides a way to indicate a faulted async operation
- IDependencyInjectionMarker interface in the Cuemon.Extensions.DependencyInjection namespace that defines a generic way to support multiple implementations of a given service for Microsoft Dependency Injection
- TypeExtensions class in the Cuemon.Extensions.DependencyInjection namespace that consist of extension methods for the Type class: TryGetDependencyInjectionMarker

## [6.3.0] - 2021-11-28

### Added

- Target framework moniker (TFM) for .NET 6 in all projects
- ReservedKeywordException class in the Cuemon namespace that provides the exception that is thrown when the value of an argument is a reserved keyword
- InMemoryTestStore class in the Cuemon.Extensions.Xunit namespace that provides a default base implementation of the ITestStore{T} interface
- ITestStore{T} interface in the Cuemon.Extensions.Xunit namespace that represents the members needed for adding and querying a store tailored for unit testing
- GenericHostTestFactory class in the Cuemon.Extensions.Xunit.Hosting namespace that provides a set of static methods for IHost unit testing
- IGenericHostTest interface in the Cuemon.Extensions.Xunit.Hosting namespace that represents the members needed for bare-bone DI testing with support for IHost

### Changed

- Replaced target framework moniker (TFM) for .NET Core 3.0 with .NET Core 3.1 (LTS) in all relevant projects
- IMiddlewareTest class in the Cuemon.Extensions.Xunit.Hosting.AspNetCore namespace to inherit from IGenericHostTest

## [6.2.0] - 2021-05-30

### Added

- CacheableMiddleware class in the Cuemon.AspNetCore.Http.Headers namespace that provides a Cache-Control middleware implementation for ASP.NET Core
- CacheableOptions class in the Cuemon.AspNetCore.Http.Headers namespace that specifies options related to CacheableMiddleware
- ExpiresHeaderValue class in the Cuemon.AspNetCore.Http.Headers namespace that represents a HTTP Expires header that contains the date/time after which the response is considered stale
- ICacheableValidator interface in the Cuemon.AspNetCore.Http.Headers namespace that represents an HTTP validator tailored for cacheable flows, that asynchronously surrounds execution of the intercepted response body
- EntityTagCacheableValidator class in the Cuemon.Extensions.AspNetCore.Http.Headers namespace that provides an HTTP validator that conforms to the ETag response header

### Changed

- Condition class in the Cuemon namespace with one new static member: FlipFlopAsync
- ApplicationBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Http.Headers namespace with one new extension method for the IApplicationBuilder interface: UseCacheControl

## [6.1.0] - 2021-05-16

### Added

- AppImageTagHelper class in the Cuemon.AspNetCore.Razor.TagHelpers namespace that provides an implementation targeting `<img>` elements that supports ICacheBusting versioning of a static image placed on a location outside (but tied to) your application
- AppLinkTagHelper class in the Cuemon.AspNetCore.Razor.TagHelpers namespace that provides an implementation targeting `<link>` elements that supports ICacheBusting versioning of a static resource placed on a location outside (but tied to) your application
- AppScriptTagHelper class in the Cuemon.AspNetCore.Razor.TagHelpers namespace that provides an implementation targeting `<script>` elements that supports ICacheBusting versioning of a static script placed on a location outside (but tied to) your application
- AppTagHelperOptions class in the Cuemon.AspNetCore.Razor.TagHelpers namespace that specifies options related to AppImageTagHelper, AppLinkTagHelper and AppScriptTagHelper
- CacheBustingTagHelper class in the Cuemon.AspNetCore.Razor.TagHelpers namespace that provides a base-class for static content related TagHelper implementation in Razor for ASP.NET Core
- ImageTagHelper class in the Cuemon.AspNetCore.Razor.TagHelpers namespace that provides a base-class for targeting `<img>` elements that supports ICacheBusting versioning
- LinkTagHelper class in the Cuemon.AspNetCore.Razor.TagHelpers namespace that provides a base-class for targeting `<link>` elements that supports ICacheBusting versioning
- ProtocolUriScheme class in the Cuemon.AspNetCore.Razor.TagHelpers namespace that defines protocol URI schemes for static resource related operations
- ScriptTagHelper class in the Cuemon.AspNetCore.Razor.TagHelpers namespace that provides a base-class for targeting `<script>` elements that supports ICacheBusting versioning
- TagHelperOptions class in the Cuemon.AspNetCore.Razor.TagHelpers namespace that specifies options related to CacheBustingTagHelper{TOptions}
- CdnImageTagHelper class in the Cuemon.AspNetCore.Razor.TagHelpers namespace that provides an implementation targeting `<img>` elements that supports ICacheBusting versioning of a static image placed on a location with a CDN role
- CdnLinkTagHelper class in the Cuemon.AspNetCore.Razor.TagHelpers namespace that provides an implementation targeting `<link>` elements that supports ICacheBusting versioning of a static resource placed on a location with a CDN role
- CdnScriptTagHelper class in the Cuemon.AspNetCore.Razor.TagHelpers namespace that provides an implementation targeting `<script>` elements that supports ICacheBusting versioning of a static script placed on a location with a CDN role
- PageBaseExtensions class in the Cuemon.AspNetCore.Razor.TagHelpers namespace that consist of extension methods for the PageBase class: GetAppUrl, GetCdnUrl
- IWebApplicationTest interface in the Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc namespace that the members needed for ASP.NET Core MVC, Razor and related testing
- WebApplicationTestFactory class in the Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc namespace that provides a set of static methods for ASP.NET Core MVC, Razor and related unit testing

### Fixed

- Output of type-attribute to skip HTML encoding on the LinkTagHelper class (earlier LinkCdnTagHelper class) located in the Cuemon.AspNetCore.Razor.TagHelpers namespace
- TotalYears calculation on the DateSpan struct located in the Cuemon namespace

## [6.0.1] - 2021-05-03

### Added

- ServerTimingMiddleware class in the Cuemon.AspNetCore.Diagnostics namespace that provides a Server-Timing middleware implementation for ASP.NET Core
- ApplicationBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Diagnostics namespace that consist of extension methods for the IApplicationBuilder interface: UseServerTiming

### Fixed

- A misguided dependency to TimeMeasureProfiler on the ServerTimingFilter class located in the Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace
- Critical bug in the DateSpan struct located in the Cuemon namespace

## [6.0.0] - 2021-04-18

### Added

- IServerTiming interface in the Cuemon.AspNetCore.Diagnostics namespace that represents the Server Timing as per W3C Working Draft 28 July 2020 (https://www.w3.org/TR/2020/WD-server-timing-20200728/)
- ServerTiming class in the Cuemon.AspNetCore.Diagnostics namespace that provides a default implementation of the IServerTiming interface
- ServerTimingMetric class in the Cuemon.AspNetCore.Diagnostics namespace that represents a HTTP Server-Timing header field entry to communicate one metric and description for the given request-response cycle
- RetryConditionScope enum in the Cuemon.AspNetCore.Http.Headers namespace that specifies a set of values defining what value to use with a given HTTP header in regards to a retry condition
- CacheBusting class in the Cuemon.AspNetCore.Configuration namespace that represents a way to provide cache-busting capabilities
- CacheBustingOptions class in the Cuemon.AspNetCore.Configuration namespace that specifies options related to CacheBusting
- DynamicCacheBusting class in the Cuemon.AspNetCore.Configuration namespace that provides cache-busting capabilities on a duration based interval
- DynamicCacheBustingOptions class in the Cuemon.AspNetCore.Configuration namespace that specifies options related to DynamicCacheBusting
- ICacheBusting interface in the Cuemon.AspNetCore.Configuration namespace that is an interface to provide cache-busting capabilities
- BadRequestException class in the Cuemon.AspNetCore.Http namespace that is the exception that is thrown when the server could not understand the request due to invalid syntax
- ConflictException class in the Cuemon.AspNetCore.Http namespace that is the exception that is thrown when a request conflicts with the current state of the server
- ForbiddenException class in the Cuemon.AspNetCore.Http namespace that is the exception that is thrown when the client does not have access rights to the content; that is, it is unauthorized, so the server is refusing to give the requested resource. Unlike 401, the client's identity is known to the server
- GoneException class in the Cuemon.AspNetCore.Http namespace that is the exception that is thrown when the requested content has been permanently deleted from server, with no forwarding address
- NotFoundException class in the Cuemon.AspNetCore.Http namespace that is the exception that is thrown when the server can not find the requested resource
- PayloadTooLargeException class in the Cuemon.AspNetCore.Http namespace that is the exception that is thrown when the request entity is larger than limits defined by server
- PreconditionFailedException class in the Cuemon.AspNetCore.Http namespace that is the exception that is thrown when the client has indicated preconditions in its headers which the server does not meet
- PreconditionRequiredException class in the Cuemon.AspNetCore.Http namespace that is the exception that is thrown when the origin server requires the request to be conditional
- TooManyRequestsException class in the Cuemon.AspNetCore.Http namespace that is the exception that is thrown when the user has sent too many requests in a given amount of time ("rate limiting")
- UnauthorizedException class in the Cuemon.AspNetCore.Http namespace that is the exception that is thrown when the requirements of an HTTP WWW-Authenticate header is not meet
- MethodNotAllowedException class in the Cuemon.AspNetCore.Http namespace that is the exception that is thrown when the request method is known by the server but has been disabled and cannot be used
- NotAcceptableException class in the Cuemon.AspNetCore.Http namespace that is the exception that is thrown when the web server, after performing server-driven content negotiation, does not find any content that conforms to the criteria given by the user agent
- BasicAuthorizationHeader class in the Cuemon.AspNetCore.Authentication.Basic namespace that provides a representation of a HTTP Basic Authentication header
- BasicAuthorizationHeaderBuilder class in the Cuemon.AspNetCore.Authentication.Basic namespace that provides a way to fluently represent a HTTP Basic Authentication header
- BasicFields class in the Cuemon.AspNetCore.Authentication.Basic namespace that holds a collection of constants for BasicAuthorizationHeaderBuilder
- DigestAuthorizationHeader class in the Cuemon.AspNetCore.Authentication.Basic namespace that provides a representation of a HTTP Digest Access Authentication header
- DigestAuthorizationHeaderBuilder class in the Cuemon.AspNetCore.Authentication.Basic namespace that provides a way to fluently represent a HTTP Digest Access Authentication header
- DigestFields class in the Cuemon.AspNetCore.Authentication.Basic namespace that holds a collection of constants for DigestAuthorizationHeaderBuilder
- AuthorizationHeader class in the Cuemon.AspNetCore.Authentication namespace that represents the base class from which all implementations of authorization header should derive
- AuthorizationHeaderBuilder class in the Cuemon.AspNetCore.Authentication namespace that represents the base class from which all implementations of authorization header builders should derive
- AuthorizationHeaderOptions class in the Cuemon.AspNetCore.Authentication namespace that specifies options related to AuthorizationHeader
- INonceTracker interface in the Cuemon.AspNetCore.Authentication namespace that represents tracking of server-generated nonce values
- MemoryNonceTracker class in the Cuemon.AspNetCore.Authentication namespace that provides a default in-memory implementation of the INonceTracker interface
- TooManyRequestsObjectResult class in the Cuemon.AspNetCore.Mvc namespace that is an ObjectResult that when executed will produce a Too Many Requests (429) response
- TooManyRequestsResult class in the Cuemon.AspNetCore.Mvc namespace that is an ActionResult that returns a TooManyRequests (429) response
- CacheableObjectFactory class in the Cuemon.AspNetCore.Mvc namespace that provides access to factory methods for creating and configuring objects implementing the ICacheableObjectResult interface
- ConfigurableInputFormatter class in the Cuemon.AspNetCore.Mvc.Formatters namespace that provides an alternate way to read an object from a request body with a text format
- StreamInputFormatter class in the Cuemon.AspNetCore.Mvc.Formatters namespace that provides a way to read an object from a request body with a text format with the constraint that TFormatter must be assignable from Formatter{Stream}
- ConfigurableOutputFormatter class in the Cuemon.AspNetCore.Mvc.Formatters namespace that provides an alternate way to write an object in a given text format to the output stream
- StreamOutputFormatter class in the Cuemon.AspNetCore.Mvc.Formatters namespace that provides a way to write an object in a given text format to the output stream with the constraint that TFormatter must be assignable from Formatter{Stream}
- ResourceAttribute class in the Cuemon.Globalization namespace that provides a generic way to support localization on attribute decorated methods
- Arguments class in the Cuemon.Collections.Generic namespace that provides a set of static methods for both typing (no conversion) and converting a variable number of arguments into its equivalent T:object[], IEnumerable{T} and T:T[]
- EnumReadOnlyDictionary class in the Cuemon.Collections.Generic namespace that represents a read-only collection of key/value pairs that provides information about the specified TEnum
- PartitionerCollection class in the Cuemon.Collections.Generic namespace that represents a generic and read-only collection that is iterated in partitions
- PartitionerEnumerable class in the Cuemon.Collections.Generic namespace that exposes the enumerator, which supports iteration in partitions over a collection of a specified type
- HierarchySerializer class in the Cuemon.Runtime.Serialization namespace that provides a way to serialize objects to nodes of IHierarchy{T}
- Formatter class in the Cuemon.Runtime.Serialization.Formatters namespace that is an abstract class that supports serialization and deserialization of an object, in a given format
- CyclicRedundancyCheck64 class in the Cuemon.Security namespace that provides a CRC-64 implementation of the CRC (Cyclic Redundancy Check) checksum algorithm for 64-bit hash values
- CyclicRedundancyCheckAlgorithm enum in the Cuemon.Security namespace that provides different models of the CRC algorithm family
- CyclicRedundancyCheckOptions class in the Cuemon.Security namespace that specifies options related to CyclicRedundancyCheck
- FowlerNollVo1024 class in the Cuemon.Security namespace that provides an implementation of the FVN (Fowler–Noll–Vo) non-cryptographic hashing algorithm for 1024-bit hash values
- FowlerNollVo128 class in the Cuemon.Security namespace that provides an implementation of the FVN (Fowler–Noll–Vo) non-cryptographic hashing algorithm for 128-bit hash values
- FowlerNollVo256 class in the Cuemon.Security namespace that provides an implementation of the FVN (Fowler–Noll–Vo) non-cryptographic hashing algorithm for 256-bit hash values
- FowlerNollVo32 class in the Cuemon.Security namespace that provides an implementation of the FVN (Fowler–Noll–Vo) non-cryptographic hashing algorithm for 32-bit hash values
- FowlerNollVo512 class in the Cuemon.Security namespace that provides an implementation of the FVN (Fowler–Noll–Vo) non-cryptographic hashing algorithm for 512-bit hash values
- FowlerNollVo64 class in the Cuemon.Security namespace that provides an implementation of the FVN (Fowler–Noll–Vo) non-cryptographic hashing algorithm for 64-bit hash values
- FowlerNollVoAlgorithm enum in the Cuemon.Security namespace that defines the algorithms of the Fowler-Noll-Vo hash function
- FowlerNollVoHash class in the Cuemon.Security namespace that represents the base class from which all implementations of the Fowler–Noll–Vo non-cryptographic hashing algorithm must derive
- FowlerNollVoOptions class in the Cuemon.Security namespace that specifies options related to FowlerNollVoHash
- Hash class in the Cuemon.Security namespace that represents the base class from which all implementations of hash algorithms and checksums should derive
- HashFactory class in the Cuemon.Security namespace that provides access to factory methods for creating and configuring Hash instances
- IHash interface in the Cuemon.Security namespace that defines the bare minimum of both non-cryptographic and cryptographic transformations
- NonCryptoAlgorithm enum in the Cuemon.Security namespace that specifies the different implementations of a non-cryptographic hashing algorithm
- ByteOrderMark class in the Cuemon.Text namespace that provides a set of static methods for Unicode related operations
- EnumStringOptions class in the Cuemon.Text namespace that specifies options related to ParserFactory.FromEnum
- GuidStringOptions class in the Cuemon.Text namespace that specifies options related to ParserFactory.FromGuid
- IConfigurableParser interface in the Cuemon.Text namespace that defines methods that converts a string to an object of a particular type having a way to configure the input
- IEncodingOptions interface in the Cuemon.Text namespace that defines configuration options for Encoding
- IParser interface in the Cuemon.Text namespace that defines methods that converts a string to an object of a particular type
- ParserFactory class in the Cuemon.Text namespace that provides access to factory methods that are tailored for parsing operations adhering IParser and IConfigurableParser{TOptions}
- ProtocolRelativeUriStringOptions class in the Cuemon.Text namespace that specifies options related to ParserFactory.FromProtocolRelativeUri
- Stem class in the Cuemon.Text namespace that provides a way to support assigning a stem to a value
- UriStringOptions class in the Cuemon.Text namespace that specifies options related to ParserFactory.FromUri
- Alphanumeric class in the Cuemon namespace that provides a set of alphanumeric constant and static fields that consists of both letters, numbers and other symbols (such as punctuation marks and mathematical symbols)
- BinaryPrefix class in the Cuemon namespace that defines a binary unit prefix for multiples of measurement for data that refers strictly to powers of 2
- BitStorageCapacity class in the Cuemon namespace that represent a table of both binary and metric prefixes for a BitUnit
- BitUnit class in the Cuemon namespace that represents a unit of measurement for bits and is used with measurement of data
- ByteStorageCapacity class in the Cuemon namespace that represent a table of both binary and metric prefixes for a ByteUnit
- Convertible class in the Cuemon namespace that provides a set of static methods, suitable for verifying integrity of data, that convert IConvertible implementations to and from a sequence of bytes
- ConvertibleConverterDictionary class in the Cuemon namespace that represents a collection of function delegates that converts an IConvertible implementation to its T:byte[] equivalent
- ConvertibleOptions class in the Cuemon namespace that specifies options related to Convertible
- DecimalPrefix class in the Cuemon namespace that defines a decimal (metric) unit prefix for multiples and submultiples of measurement that refers strictly to powers of 10
- Decorator class in the Cuemon namespace that provides a way to dynamically enclose/wrap an object to support the decorator pattern
- DelimitedString class in the Cuemon namespace that provides a set of static methods to convert a sequence into a delimited string and break a delimited string into substrings
- DelimitedStringOptions class in the Cuemon namespace that specifies options related to DelimitedString.Split
- Disposable class in the Cuemon namespace that provides a mechanism for releasing both managed and unmanaged resources with focus on the former
- DisposableOptions class in the Cuemon namespace that specifies options related to Disposable
- EndianOptions class in the Cuemon namespace that specifies options related to BitConverter
- Endianness class in the Cuemon namespace that defines the order in which a sequence of bytes are represented
- Eradicate class in the Cuemon namespace that provides a set of static methods for eradicating different types of values or sequences of values
- ExceptionCondition class in the Cuemon namespace that provides a fluent and generic way to setup a condition for raising an Exception
- ExceptionInsights class in the Cuemon namespace that provides a set of static methods for embedding environment specific insights to an exception
- FinalizeDisposable class in the Cuemon namespace that provides a mechanism for releasing both managed and unmanaged resources with focus on the latter
- FormattingOptions class in the Cuemon namespace that specifies options related to IFormatProvider
- Generate class in the Cuemon namespace that provides a set of static methods for generating different types of values or sequences of values
- GuidFormats enum in the Cuemon namespace that specifies allowed GUID formats in parsing related methods
- IDecorator interface in the Cuemon namespace that defines a decorator that exposes the inner decorated type
- IPrefixMultiple interface in the Cuemon namespace that defines a unit prefix that can can be expressed as a either a multiple or a submultiple of the unit of measurement
- IUnit interface in the Cuemon namespace that defines a unit of measurement that is used as a standard for measurement of the same kind of quantity
- MultipleTable class in the Cuemon namespace that defines a unit of measurement that provides a way to represent a table of both binary and metric prefixes that precedes a unit of measure to indicate a multiple of the unit
- NamingStyle enum in the Cuemon namespace that specifies ways that a string must be represented in terms of naming style
- ObjectFormattingOptions class in the Cuemon namespace that specifies options related to ParserFactory.FromObject
- ObjectPortrayalOptions class in the Cuemon namespace that specifies options related to Generate.ObjectPortrayal
- Patterns class in the Cuemon namespace that provides a generic way to support different types of design patterns and practices with small utility methods
- StringFactory class in the Cuemon namespace that provides access to factory methods for creating and configuring encoded string instances
- SystemSnapshot enum in the Cuemon namespace that specifies the system states to capture runtime
- UnitFormatOptions class in the Cuemon namespace that specifies options related to BitUnit and ByteUnit
- UnitPrefix class in the Cuemon namespace that specifies the two standards for binary multiples and decimal multiples
- UnitPrefixFormatter class in the Cuemon namespace that defines the string formatting of objects having an implementation of either IPrefixUnit or IUnit
- PrefixMultiple class in the Cuemon namespace that represents the base class from which all implementations of unit prefix that can can be expressed as a either a multiple or a submultiple of the unit of measurement should derive
- StorageCapacity class in the Cuemon namespace that provides a way to represent a table of both binary and metric prefixes that precedes a unit of measure optimized for storage capacity measurement standards
- StorageCapacityOptions class in the Cuemon namespace that specifies options related to StorageCapacity
- PrefixUnit class in the Cuemon namespace that represents the base class from which all implementations of a unit of measurement should derive
- AsyncOptions class in the Cuemon.Threading namespace that specifies options that is related to asynchronous operations
- IAsyncOptions interface in the Cuemon.Threading namespace that defines options that is related to asynchronous operations
- AsyncEncodingOptions interface in the Cuemon.Text namespace that defines options that is related to Encoding class
- PaginationEnumerable class in the Cuemon.Collections.Generic namespace that represents a generic and read-only pagination sequence
- PaginationList class in the Cuemon.Collections.Generic namespace that represents an eagerly materialized generic and read-only pagination list
- PaginationOptions class in the Cuemon.Collections.Generic namespace that specifies options related to PaginationEnumerable and PaginationList
- VersionResult class in the Cuemon.Reflection namespace that represents different representations of a version scheme in a consistent way
- TimeRange class in the Cuemon namespace that represents a period of time between two TimeSpan values
- Range{T} class in the Cuemon namespace that represents a period of time between two types
- TimerFactory class in the Cuemon.Threading namespace that provides access to factory methods for creating and configuring Timer instances
- IWatcher interface in the Cuemon.Runtime namespace which specifies that an supports a way to monitor a resource
- WatcherOptions class in the Cuemon.Runtime namespace that specifies options that is related to IWatcher
- FileDependency class in the Cuemon.Runtime namespace that provides a way to monitor any changes occurred to one or more files while notifying subscribing objects
- FileWatcher class in the Cuemon.Runtime namespace that provides a watcher implementation designed to monitor and signal changes applied to a file by raising the Changed event
- DsvDataReader class in the Cuemon.Data namespace that provides a way of reading a forward-only stream of rows from a DSV (Delimiter Separated Values) based data source
- InOperatorResult class in the Cuemon.Data namespace that provides the result of an InOperator{T} operation
- TokenBuilder class in the Cuemon.Data namespace that represents a mutable string of characters optimized for tokens
- DatabaseDependency class in the Cuemon.Data namespace that provides a way to monitor any changes occurred to one or more relational data sources while notifying subscribing objects
- DatabaseWatcher class in the Cuemon.Data namespace that provides a watcher implementation designed to monitor and signal changes applied to a relational database by raising the Changed event
- CacheValidatorFactory class in the Cuemon.Data.Integrity namespace that provides access to factory methods for creating and configuring CacheValidator instances
- DataIntegrityFactory class in the Cuemon.Data.Integrity namespace that provides access to factory methods for creating and configuring implementations of the IDataIntegrity interface
- FileChecksumOptions class in the Cuemon.Data.Integrity namespace that specifies configuration options for FileInfo
- FileIntegrityOptions class in the Cuemon.Data.Integrity namespace that specifies configuration options for FileInfo
- IDataIntegrity interface in the Cuemon.Data.Integrity namespace determines the integrity of data
- ExceptionDescriptorOptions class into Cuemon.Diagnostics namespace that specifies configuration options for serializer implementations
- AsyncTimeMeasureOptions class into Cuemon.Diagnostics namespace that specifies configuration options for TimeMeasure class
- FaultHandler class into Cuemon.Diagnostics namespace that provides a generic way to implement a fault resolver that evaluate an exception and provide details about it in a developer friendly way
- FaultResolver class into Cuemon.Diagnostics namespace that a way to evaluate an exception and provide details about it in a developer friendly way
- Int32Extensions class in the Cuemon.Extensions.AspNetCore.Http namespace that consist of extension methods for the Int32 struct: IsInformationStatusCode, IsSuccessStatusCode, IsRedirectionStatusCode, IsNotModifiedStatusCode, IsClientErrorStatusCode, IsServerErrorStatusCode
- ApplicationBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Authentication namespace that consist of extension methods for the IApplicationBuilder interface: UseBasicAuthentication, UseDigestAccessAuthentication, UseHmacAuthentication 
- ServiceCollectionExtensions class in the Cuemon.Extensions.AspNetCore.Authentication namespace that consist of extension methods for the IServiceCollection interface: AddInMemoryDigestAuthenticationNonceTracker
- CollectionExtensions class in the Cuemon.Extensions.Collections.Generic namespace that consist of extension methods for the ICollection{T} interface: ToPartitioner{T}, AddRange{T}
- DictionaryExtensions class in the Cuemon.Extensions.Collections.Generic namespace that consist of extension methods for the IDictionary{TKey, TValue} interface: GetValueOrDefault{TKey, TValue}, TryGetValueOrFallback{TKey, TValue}, ToEnumerable{TKey, TValue}, TryAdd{TKey, TValue}, TryAddOrUpdate{TKey, TValue}
- EnumerableExtensions class in the Cuemon.Extensions.Collections.Generic namespace that consist of extension methods for the IEnumerable{T} interface: Chunk{T}, Shuffle{T}, OrderAscending{T}, OrderDescending{T}, RandomOrDefault{T}, Yield{T}, ToDictionary{TKey, TValue}, ToPartitioner{T}, ToPagination{T}, ToPaginationList{T}
- ListExtensions class in the Cuemon.Extensions.Collections.Generic namespace that consist of extension methods for the IList{T} interface: Remove{T}, HasIndex{T}, Next{T}, Previous{T}
- DictionaryExtensions class in the Cuemon.Extensions.Collections.Specialized namespace that consist of extension methods for the IDictionary{string, string[]} interface: ToNameValueCollection
- NameValueCollectionExtensions class in the Cuemon.Extensions.Collections.Specialized namespace that consist of extension methods for the NameValueCollection class: ContainsKey, ToDictionary
- ActionExtensions class in the Cuemon.Extensions namespace that consist of extension methods for the Action delegate: Configure{TOptions}, CreateInstance{T}
- ByteExtensions class in the Cuemon.Extensions namespace that consist of extension methods for the Byte struct: ToEncodedString, ToHexadecimalString, ToBinaryString, ToUrlEncodedBase64String, ToBase64String, TryDetectUnicodeEncoding
- CharExtensions class in the Cuemon.Extensions namespace that consist of extension methods for the Char struct: ToEnumerable, FromChars
- ConditionExtensions class in the Cuemon.Extensions namespace that consist of extension methods for the Condition class: HasDifference
- DateTimeExtensions class in the Cuemon.Extensions namespace that consist of extension methods for the DateTime struct: ToUnixEpochTime, ToUtcKind, ToLocalKind, ToDefaultKind, IsWithinRange, IsTimeOfDayNight, IsTimeOfDayMorning, IsTimeOfDayForenoon, IsTimeOfDayAfternoon, IsTimeOfDayEvening, Floor, Ceiling, Round
- DoubleExtensions class in the Cuemon.Extensions namespace that consist of extension methods for the Double struct: FromUnixEpochTime, ToTimeSpan, Factorial, RoundOff
- ExceptionExtensions class in the Cuemon.Extensions namespace that consist of extension methods for the Exception class: Flatten
- IntegerExtensions class in the Cuemon.Extensions namespace that consist of extension methods for signed integers: Min, Max, IsPrime, IsCountableSequence, IsEven, IsOdd
- MappingExtensions class in the Cuemon.Extensions namespace that consist of extension methods for the Mapping class: Add
- ObjectExtensions class in the Cuemon.Extensions namespace that consist of extension methods for the Object class: UseWrapper{T}, As{T}, GetHashCode32{T}, GetHashCode64{T}, ToDelimitedString{T}, Adjust{T}, IsNullable{T}
- StringExtensions class in the Cuemon.Extensions namespace that consist of extension methods for the String class: Difference, ToByteArray, FromUrlEncodedBase64, ToGuid, FromBinaryDigits, FromBase64, ToCasing, ToUri, IsNullOrEmpty, IsNullOrWhiteSpace, IsEmailAddress, IsGuid, IsHex, IsNumeric, IsBase64, IsCountableSequence, SplitDelimited, Count, RemoveAll, ReplaceAll, JsEscape, JsUnescape, ContainsAny, ContainsAll, EqualsAny, StartsWith, TrimAll, IsSequenceOf{T}, FromHexadecimal, ToHexadecimal, ToEnum{TEnum}, ToTimeSpan, SubstringBefore, Chunk, SuffixWith, SuffixWithForwardingSlash, PrefixWith
- TimeSpanExtensions class in the Cuemon.Extensions namespace that consist of extension methods for the TimeSpan struct: GetTotalNanoseconds, GetTotalMicroseconds, Floor, Ceiling, Round
- TypeExtensions class in the Cuemon.Extensions namespace that consist of extension methods for the Type class: ToFriendlyName, ToTypeCode, HasEqualityComparerImplementation, HasComparableImplementation, HasComparerImplementation, HasEnumerableImplementation, HasDictionaryImplementation, HasKeyValuePairImplementation, IsNullable, HasAnonymousCharacteristics, IsComplex, IsSimple, GetDefaultValue, HasTypes, HasInterfaces, HasAttributes
- ValidatorExtensions class in the Cuemon.Extensions namespace that consist of extension methods for the Validator class: HasDifference, NoDifference
- DataTransferExtensions class in the Cuemon.Extensions.Data namespace that consist of extension methods for the IDataReader interface: ToColumns, ToRows
- QueryFormatExtensions class in the Cuemon.Extensions.Data namespace that consist of extension methods for the QueryFormat enum: Embed
- AssemblyExtensions class in the Cuemon.Extensions.Data.Integrity namespace that consist of extension methods for the Assembly class: GetCacheValidator
- ChecksumBuilderExtensions class in the Cuemon.Extensions.Data.Integrity namespace that consist of extension methods for the ChecksumBuilder class: CombineWith{T}
- DateTimeExtensions class in the Cuemon.Extensions.Data.Integrity namespace that consist of extension methods for the DateTime struct: GetCacheValidator
- FileInfoExtensions class in the Cuemon.Extensions.Data.Integrity namespace that consist of extension methods for the FileInfo class: GetCacheValidator
- ServiceCollectionExtensions class in the Cuemon.Extensions.DependencyInjection namespace that consist of extension methods for the IServiceCollection interface: Add, Add{TOptions}, Add{TService, TImplementation}, Add{TService, TImplementation, TOptions}, TryAdd, TryAdd{TOptions}, TryAdd{TService, TImplementation}, TryAdd{TService, TImplementation, TOptions}
- ExceptionDescriptorExtensions class in the Cuemon.Extensions.Diagnostics namespace that consist of extension methods for the ExceptionDescriptor class: ToInsightsString
- FileVersionInfoExtensions class in the Cuemon.Extensions.Diagnostics namespace that consist of extension methods for the FileVersionInfo class: ToProductVersion, ToFileVersion
- HostEnvironmentExtensions class in the Cuemon.Extensions.Hosting namespace that consist of extension methods for the IHostEnvironment interface: IsLocalDevelopment and IsNonProduction
- HostingEnvironmentExtensions class in the Cuemon.Extensions.Hosting namespace that consist of extension methods for the IHostingEnvironment interface: IsLocalDevelopment and IsNonProduction
- Environments class in the Cuemon.Extensions.Hosting namespace that provides a set of constants for commonly used environment names
- ByteArrayExtensions class in the Cuemon.Extensions.IO namespace that consist of extension methods for the byte[] struct: ToStream, ToStreamAsync
- StreamExtensions class in the Cuemon.Extensions.IO namespace that consist of extension methods for the Stream class: Concat, ToCharArray, ToByteArray, ToByteArrayAsync, WriteAsync, TryDetectUnicodeEncoding, ToEncodedString, ToEncodedStringAsync, CompressBrotli, CompressBrotliAsync, CompressDeflate, CompressDeflateAsync, CompressGZip, CompressGZipAsync, DecompressBrotli, DecompressBrotliAsync, DecompressDeflate, DecompressDeflateAsync, DecompressGZip, DecompressGZipAsync
- StringExtensions class in the Cuemon.Extensions.IO namespace that consist of extension methods for the String class: ToStream, ToStreamAsync, ToTextReader
- TextReaderExtensions class in the Cuemon.Extensions.IO namespace that consist of extension methods for the TextReader class: CopyToAsync, ReadAllLines, ReadAllLinesAsync
- HttpManagerFactory class in the Cuemon.Extensions.Net.Http namespace that provides access to factory methods for creating and configuring HttpManager instances
- HttpMethodExtensions class in the Cuemon.Extensions.Net.Http namespace that consist of extension methods for the HttpMethod class: ToHttpMethod
- SlimHttpClientFactory class in the Cuemon.Extensions.Net.Http namespace that provides a simple and lightweight implementation of the IHttpClientFactory interface
- SlimHttpClientFactoryOptions class in the Cuemon.Extensions.Net.Http namespace that specifies options related to SlimHttpClientFactory
- UriExtensions class in the Cuemon.Extensions.Net.Http namespace that consist of extension methods for the Uri class: HttpDeleteAsync, HttpGetAsync, HttpHeadAsync, HttpOptionsAsync, HttpPostAsync, HttpPutAsync, HttpPatchAsync, HttpTraceAsync, HttpAsync
- SignedUriOptions class in the Cuemon.Extensions.Net.Security namespace that specifies options related to ToSignedUri extensions
- StringExtensions class in the Cuemon.Extensions.Net.Security namespace that consist of extension methods for the String class: ToSignedUri, ValidateSignedUri
- UriExtensions class in the Cuemon.Extensions.Net.Security namespace that consist of extension methods for the Uri class: ToSignedUri, ValidateSignedUri
- ByteArrayExtensions class in the Cuemon.Extensions.Net namespace that consist of extension methods for the byte[] struct: UrlEncode
- DictionaryExtensions class in the Cuemon.Extensions.Net namespace that consist of extension methods for the IDictionary{string, string[]} interface: ToQueryString
- NameValueCollectionExtensions class in the Cuemon.Extensions.Net namespace that consist of extension methods for the NameValueCollection class: ToQueryString
- StringExtensions class in the Cuemon.Extensions.Net namespace that consist of extension methods for the String class: UrlEncode, UrlDecode
- ExceptionDescriptorExtensions class in the Cuemon.Extensions.Newtonsoft.Json.Diagnostics namespace that consist of extension methods for the ExceptionDescriptor class: ToInsightsJsonString
- JData class in the Cuemon.Extensions.Newtonsoft.Json namespace that provides a factory based way to parse and extract values from various sources of JSON data. Compliant with RFC 7159 as it uses JsonTextReader behind the scene
- JDataResultExtensions class in the Cuemon.Extensions.Newtonsoft.Json namespace that consist of extension methods for the JDataResult class: ExtractArrayValues, ExtractObjectValues
- ValidatorExtensions class in the Cuemon.Extensions.Newtonsoft.Json namespace that consist of extension methods for the Validator class: InvalidJsonDocument
- ContractResolverExtensions class in the Cuemon.Extensions.Newtonsoft.Json.Serialization namespace that consist of extension methods for the IContractResolver interface: ResolveNamingStrategyOrDefault
- AssemblyExtensions class in the Cuemon.Extensions.Reflection namespace that consist of extension methods for the Assembly class: GetAssemblyVersion, GetFileVersion, GetProductVersion, IsDebugBuild
- PropertyInfoExtensions class in the Cuemon.Extensions.Reflection namespace that consist of extension methods for the PropertyInfo class: IsAutoProperty
- TypeExtensions class in the Cuemon.Extensions.Reflection namespace that consist of extension methods for the Type class: GetEmbeddedResources, GetRuntimePropertiesExceptOf{T}, ToFullNameIncludingAssemblyName
- CacheEnumerableExtensions class in the Cuemon.Extensions.Runtime.Caching namespace that consist of extension methods for the ICacheEnumerable{TKey} interface: GetOrAdd, Memoize
- EncodingOptionsExtensions class in the Cuemon.Extensions.Text namespace that consist of extension methods for the EncodingOptionsExtensions class: DetectUnicodeEncoding
- StringExtensions class in the Cuemon.Extensions.Text namespace that consist of extension methods for the string class: ToEncodedString, ToAsciiEncodedString
- TaskExtensions class in the Cuemon.Extensions.Threading.Tasks namespace that consist of extension methods for the Task class: ContinueWithCapturedContext, ContinueWithCapturedContext{TResult}, ContinueWithSuppressedContext, ContinueWithSuppressedContext{TResult}
- XmlConverterExtensions class in the Cuemon.Extensions.Xml.Serialization.Converters namespace that consist of extension methods for the IList{XmlConverter} interface: FirstOrDefaultReaderConverter, FirstOrDefaultWriterConverter, AddXmlConverter, InsertXmlConverter, AddEnumerableConverter, AddExceptionDescriptorConverter, AddUriConverter, AddDateTimeConverter, AddTimeSpanConverter, AddStringConverter, AddExceptionConverter
- ExceptionDescriptorExtensions class in the Cuemon.Extensions.Xml.Serialization.Diagnostics namespace that consist of extension methods for the ExceptionDescriptor class: ToInsightsXmlString
- XmlSerializerOptionsExtensions class in the Cuemon.Extensions.Xml.Serialization namespace that consist of extension methods for the XmlSerializerOptions class: ApplyToDefaultSettings
- ByteArrayExtensions class in the Cuemon.Extensions.Xml namespace that consist of extension methods for the byte[] struct: ToXmlReader
- HierarchyExtensions class in the Cuemon.Extensions.Xml namespace that consist of extension methods for the IHierarchy{T} interface: IsNodeEnumerable, GetXmlRootOrElement, OrderByXmlAttributes
- StreamExtensions class in the Cuemon.Extensions.Xml namespace that consist of extension methods for the Stream class: ToXmlReader, CopyXmlStream, TryDetectXmlEncoding
- UriExtensions class in the Cuemon.Extensions.Xml namespace that consist of extension methods for the Uri class: ToXmlReader
- XmlReaderExtensions class in the Cuemon.Extensions.Xml namespace that consist of extension methods for the XmlReader class: ToHierarchy, MoveToFirstElement
- XmlWriterExtensions class in the Cuemon.Extensions.Xml namespace that consist of extension methods for the XmlWriter class: WriteObject, WriteObject{T}, WriteStartElement, WriteEncapsulatingElementWhenNotNull{T}, WriteXmlRootElement{T}
- Test class in the Cuemon.Extensions.Xunit namespace that represents the base class from which all implementations of unit testing should derive
- ITest interface in the Cuemon.Extensions.Xunit namespace that represents the members needed for vanilla testing
- TestOutputHelperExtensions class in the Cuemon.Extensions.Xunit namespace that consist of extension methods for the ITestOutputHelper interface: WriteLines
- HostTest class in the Cuemon.Extensions.Xunit.Hosting namespace that represents a base class from which all implementations of unit testing, that uses Microsoft Dependency Injection, should derive
- IHostFixture interface in the Cuemon.Extensions.Xunit.Hosting namespace that provides a way to use Microsoft Dependency Injection in unit tests
- HostFixture class in the Cuemon.Extensions.Xunit.Hosting namespace that provides a default implementation of the IHostFixture interface
- FakeHttpResponseFeature class in the Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http.Features namespace that represents a way to trigger IHttpResponseFeature.OnStarting
- FakeHttpContextAccessor class in the Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http namespace that provides a unit test implementation of IHttpContextAccessor
- AspNetCoreHostFixture class in the Cuemon.Extensions.Xunit.Hosting.AspNetCore namespace that provides a default implementation of the IAspNetCoreHostFixture interface
- AspNetCoreHostTest{T} class in the Cuemon.Extensions.Xunit.Hosting.AspNetCore namespace that represents a base class from which all implementations of unit testing, that uses Microsoft Dependency Injection and depends on ASP.NET Core, should derive
- IAspNetCoreHostFixture interface in the Cuemon.Extensions.Xunit.Hosting.AspNetCore namespace that provides a way to use Microsoft Dependency Injection in unit tests tailored for ASP.NET Core
- IMiddlewareTest interface in the Cuemon.Extensions.Xunit.Hosting.AspNetCore namespace that represents the members needed for ASP.NET Core middleware testing
- IPipelineTest interface in the Cuemon.Extensions.Xunit.Hosting.AspNetCore namespace that represents the members needed for ASP.NET Core pipeline testing
- MiddlewareTestFactory class in the Cuemon.Extensions.Xunit.Hosting.AspNetCore namespace that provides a set of static methods for ASP.NET Core middleware unit testing
- ServiceCollectionExtensions class in the Cuemon.Extensions.Xunit.Hosting.AspNetCore namespace that consist of extension methods for the IServiceCollection interface: AddFakeHttpContextAccessor
- IMvcFilterTest interface in the Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc namespace that represents the members needed for ASP.NET Core MVC filter testing
- MvcFilterTestFactory class in the Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc namespace that provides a set of static methods for ASP.NET Core MVC filter unit testing
- AsyncDisposableOptions class in the Cuemon.IO namespace that specifies options related to a cancelable IDisposable implementation
- AsyncStreamCompressionOptions class in the Cuemon.IO namespace that specifies options related to a cancelable Stream compression
- AsyncStreamCopyOptions class in the Cuemon.IO namespace that specifies options related to a cancelable Stream copy operation
- AsyncStreamEncodingOptions class in the Cuemon.IO namespace that specifies options related to a cancelable Stream encoding
- AsyncStreamReaderOptions class in the Cuemon.IO namespace that specifies options related to a cancelable StreamReader operation
- FileInfoOptions class in the Cuemon.IO namespace that specifies options related to FileInfo
- StreamCompressionOptions class in the Cuemon.IO namespace that specifies options related to a Stream compression
- StreamCopyOptions class in the Cuemon.IO namespace that specifies options related to a Stream copy operation
- StreamEncodingOptions class in the Cuemon.IO namespace that specifies options related to a Stream encoding
- StreamReaderOptions class in the Cuemon.IO namespace that specifies options related to a StreamReader operation
- StreamWriterOptions class in the Cuemon.IO namespace that specifies options related to a StreamWriter operation
- MailDistributor class in the Cuemon.Net.Mail namespace that provides a way for applications to distribute one or more e-mails in batches by using the Simple Mail Transfer Protocol (SMTP)
- FieldValueSeparator enum in the Cuemon.Net namespace that specifies a range of key-value separators
- QueryStringCollection class in the Cuemon.Net namespace that provides a collection of string values that is equivalent to a query string of an Uri
- HttpDependency class in the Cuemon.Net.Http namespace that provides a way to monitor any changes occurred to one or more URI resources while notifying subscribing objects
- HttpWatcher class in the Cuemon.Net.Http namespace that provides a watcher implementation designed to monitor and signal changes applied to an URI resource by raising the Changed event
- HttpWatcherOptions class in the Cuemon.Net.Http namespace that that specifies options related to HttpWatcher
- LatencyException class in the Cuemon.Resilience namespace that represents the exception that is thrown when a latency related operation was taking to long to complete
- TransientFaultEvidence class in the Cuemon.Resilience namespace that provides evidence about a faulted TransientOperation
- TransientFaultException class in the Cuemon.Resilience namespace that represents the exception that is thrown when a transient fault handling was unsuccessful
- TransientOperation class in the Cuemon.Resilience namespace that provides a set of static methods that enable developers to make their applications more resilient by adding robust transient fault handling logic ideal for temporary condition such as network connectivity issues or service unavailability
- TransientOperationOptions class in the Cuemon.Resilience namespace that specifies options related to TransientOperation class
- AsyncTransientOperationOptions class in the Cuemon.Resilience namespace that specifies options related to TransientOperation class
- CacheEntry class in the Cuemon.Runtime.Caching namespace that represents an individual cache entry in the cache
- CacheInvalidation class in the Cuemon.Runtime.Caching namespace that represents a set of eviction and expiration details for a specific cache entry
- ICacheEnumerable{TKey} interface in the Cuemon.Runtime.Caching namespace that is used to provide cache implementations for an application
- SlimMemoryCache class in the Cuemon.Runtime.Caching namespace that represents the type that implements an in-memory cache for an application
- SlimMemoryCacheOptions class in the Cuemon.Runtime.Caching namespace that specifies options related to SlimMemoryCache
- AesCryptor class in the Cuemon.Security.Cryptography namespace that provides an implementation of the Advanced Encryption Standard (AES) symmetric algorithm
- AesCryptorOptions class in the Cuemon.Security.Cryptography namespace that specifies options related to AesCryptor
- AesKeyOptions class in the Cuemon.Security.Cryptography namespace that specifies options related to AesCryptor.GenerateKey
- HmacMessageDigest5 class in the Cuemon.Security.Cryptography namespace that provides a Hash-based Message Authentication Code (HMAC) using the MD5 hash function
- HmacSecureHashAlgorithm1 class in the Cuemon.Security.Cryptography namespace that provides a Hash-based Message Authentication Code (HMAC) using the SHA1 hash function
- HmacSecureHashAlgorithm256 class in the Cuemon.Security.Cryptography namespace that provides a Hash-based Message Authentication Code (HMAC) using the SHA256 hash function
- HmacSecureHashAlgorithm384 class in the Cuemon.Security.Cryptography namespace that provides a Hash-based Message Authentication Code (HMAC) using the SHA384 hash function
- HmacSecureHashAlgorithm512 class in the Cuemon.Security.Cryptography namespace that provides a Hash-based Message Authentication Code (HMAC) using the SHA512 hash function
- KeyedCryptoHash class in the Cuemon.Security.Cryptography namespace that represents the base class from which all implementations of Hash-based Message Authentication Code (HMAC) should derive
- MessageDigest5 class in the Cuemon.Security.Cryptography namespace that provides a MD5 implementation of the MD (Message Digest) cryptographic hashing algorithm for 128-bit hash values
- SecureHashAlgorithm1 class in the Cuemon.Security.Cryptography namespace that provides a SHA-1 implementation of the SHA (Secure Hash Algorithm) cryptographic hashing algorithm for 160-bit hash values
- SecureHashAlgorithm256 class in the Cuemon.Security.Cryptography namespace that provides a SHA-256 implementation of the SHA (Secure Hash Algorithm) cryptographic hashing algorithm for 256-bit hash values
- SecureHashAlgorithm384 class in the Cuemon.Security.Cryptography namespace that provides a SHA-384 implementation of the SHA (Secure Hash Algorithm) cryptographic hashing algorithm for 384-bit hash values
- SecureHashAlgorithm512 class in the Cuemon.Security.Cryptography namespace that provides a SHA-512 implementation of the SHA (Secure Hash Algorithm) cryptographic hashing algorithm for 512-bit hash values
- UnkeyedCryptoHash class in the Cuemon.Security.Cryptography namespace that represents the base class from which all implementations of cryptographic hashing algorithm should derive
- AsyncTaskFactoryOptions class in the Cuemon.Threading namespace that specifies options that is related to both ParallelFactory and AdvancedParallelFactory
- AsyncWorkloadOptions class in the Cuemon.Threading namespace that specifies options that is related to both ParallelFactory and AdvancedParallelFactory
- AdvancedParallelFactory static class in the Cuemon.Threading namespace that provides a factory based way to work with advanced scenarios that encapsulate and re-use existing code while adding support for typically long-running parallel loops and regions
- ForLoopRuleset class in the Cuemon.Threading namespace that specifies the rules of a for-loop control flow statement
- DefaultXmlConverter class into Cuemon.Xml.Serialization.Converters namespace that provides a default way to convert objects to and from XML
- XmlConverter class into Cuemon.Xml.Serialization.Converters namespace that converts an object to and from XML
- XmlFormatter class into Cuemon.Xml.Serialization.Formatters namespace that serializes and deserializes an object in XML format
- XmlFormatterOptions class into Cuemon.Xml.Serialization.Formatters namespace that specifies configuration options for XmlFormatter
- DynamicXmlConverter class into Cuemon.Xml.Serialization namespace that provides a factory based way to create and wrap an XmlConverter implementation
- DynamicXmlSerializable class into Cuemon.Xml.Serialization namespace that provides a factory based way to create and wrap an IXmlSerializable implementation
- XmlConvert class into Cuemon.Xml.Serialization namespace that provides methods for converting between .NET types and XML types
- XmlSerializer class into Cuemon.Xml.Serialization namespace that serializes and deserializes objects into and from the XML format
- XmlSerializerSettings class into Cuemon.Xml.Serialization namespace that specifies configuration options for XmlSerializer

### Changed

- HttpExceptionDescriptor class from the Cuemon.AspNetCore.Http namespace was moved to the Cuemon.AspNetCore.Diagnostics namespace
- ThrottlingSentinelOptions class in the Cuemon.AspNetCore.Http.Throttling namespace to be more compliant with industry standards
- ApplicationBuilderFactory class in the Cuemon.AspNetCore.Builder namespace was renamed to MiddlewareBuilderFactory
- HttpStatusCodeException class in the Cuemon.AspNetCore.Http namespace in the context of:
  - Adding a new ReasonPhrase property
  - An overloaded constructor with default values
  - A better representation of the object by overloading the ToString method
- AuthenticationUtility class in the Cuemon.AspNetCore.Authentication namespace was renamed to Authenticator and all constants was removed
- BasicAuthenticationMiddleware class from the Cuemon.AspNetCore.Authentication namespace was moved to the Cuemon.AspNetCore.Authentication.Basic namespace (including refactoring)
- BasicAuthenticationOptions class from the Cuemon.AspNetCore.Authentication namespace was moved to the Cuemon.AspNetCore.Authentication.Basic namespace
- BasicAuthenticator delegate from the Cuemon.AspNetCore.Authentication namespace was moved to the Cuemon.AspNetCore.Authentication.Basic namespace
- DigestAccessAuthenticationMiddleware class from the Cuemon.AspNetCore.Authentication namespace was moved to the Cuemon.AspNetCore.Authentication.Digest namespace (including refactoring)
- DigestAccessAuthenticationOptions class from the Cuemon.AspNetCore.Authentication namespace was moved to the Cuemon.AspNetCore.Authentication.Digest namespace (including refactoring)
- DigestAccessAuthenticator delegate from the Cuemon.AspNetCore.Authentication namespace was moved to the Cuemon.AspNetCore.Authentication.Digest namespace
- DigestAccessAuthenticationMiddleware class in the Cuemon.AspNetCore.Authentication.Digest namespace was renamed to DigestAuthenticationMiddleware
- DigestAccessAuthenticationOptions class in the Cuemon.AspNetCore.Authentication.Digest namespace was renamed to DigestAuthenticationOptions
- DigestAccessAuthenticator class in the Cuemon.AspNetCore.Authentication.Digest namespace was renamed to DigestAuthenticator
- HmacAuthenticationMiddleware class from the Cuemon.AspNetCore.Authentication namespace was moved to the Cuemon.AspNetCore.Authentication.Hmac namespace (including refactoring)
- HmacAuthenticationOptions class from the Cuemon.AspNetCore.Authentication namespace was moved to the Cuemon.AspNetCore.Authentication.Hmac namespace (including refactoring)
- HmacAuthenticator delegate from the Cuemon.AspNetCore.Authentication namespace was moved to the Cuemon.AspNetCore.Authentication.Hmac namespace
- UnauthorizedException class from the Cuemon.AspNetCore.Authentication namespace was moved to the Cuemon.AspNetCore assembly in the Cuemon.AspNetCore.Http namespace
- Any former extension methods of the Cuemon.AspNetCore.Mvc namespace was merged either into the Cuemon.Extensions.AspNetCore.Mvc namespace or Cuemon.Extensions.AspNetCore namespace
- ICacheableObjectResult interface (and related) from the Cuemon.AspNetCore.Mvc.Filters.Cacheable namespace was moved to the Cuemon.AspNetCore.Mvc namespace
- HttpEntityTagHeader class in the Cuemon.AspNetCore.Mvc.Filters.Cacheable namespace was renamed to HttpEntityTagHeaderFilter
- HttpLastModifiedHeader class in the Cuemon.AspNetCore.Mvc.Filters.Cacheable namespace was renamed to HttpLastModifiedHeaderFilter
- TimeMeasureAttribute class in the Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace was renamed to ServerTimingAttribute (including refactoring)
- TimeMeasuringFilter class in the Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace was renamed to ServerTimingFilter (including refactoring)
- TimeMeasuringOptions class in the Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace was renamed to ServerTimingOptions (including refactoring)
- FaultResolverDecoratorExtensions class in the Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace in the context of renaming the Add{T} method to AddHttpFaultResolver{T}
- FaultResolver class in the Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace was renamed to HttpFaultResolver
- PreambleSequence enum from the Cuemon namespace was moved to the Cuemon.Text namespace
- DayParts class from the Cuemon namespace was merged, renamed and changed to DayPart struct
- TimeRange struct in the Cuemon namespace was merged, renamed and changed DateTimeRange class
- HashResult class in the Cuemon.Security namespace to implement IEquatable{HashResult} as well as other improvements
- Condition class in the Cuemon namespace with several new static members: Query, IsEnum, IsProtocolRelativeUrl, IsUri, HasConsecutiveCharacters, IsBase64, IsBinaryDigits, IsPrime, IsEven, IsOdd, IsCountableSequence, IsTrue, IsFalse (Query makes this class extensible - enabling you to write your own conditions)
- TaskActionFactory class in the Cuemon namespace to have CancellationToken support
- TaskFuncFactory class in the Cuemon namespace to have CancellationToken support
- Validator class in the Cuemon namespace with several new static members: ThrowIf, ThrowWhenCondition, ThrowIfNumber, ThrowIfNotNumber, ThrowIfNull, ThrowIfSequenceEmpty, ThrowIfEmpty, ThrowIfContainsInterface, ThrowIfNotContainsInterface, ThrowIfNotContainsType, ThrowIfEnumType, ThrowIfNotBinaryDigits, ThrowIfNotBase64String, ThrowIfTrue, ThrowIfFalse, ThrowIfSequenceNullOrEmpty, CheckParameter{T}, CheckParameter{T,TValue}
- BinaryPrefix in the Cuemon namespace from struct to sealed class
- DecimalPrefix in the Cuemon namespace from struct to sealed class
- MultipleTable in the Cuemon namespace to be more generic and moved non-generic functionality to the new StorageCapacity class
- BitUnit in the Cuemon namespace from struct to sealed class
- ByteUnit in the Cuemon namespace from struct to sealed class
- XmlDataReader class in the Cuemon.Data.Xml namespace to match and inherit from DataReader
- DataManager class in the Cuemon.Data namespace to have a higher cohesion and lower coupling
- StringDataReader class in the Cuemon.Data namespace to DataReader{T} (including major refactoring of the underlying code)
- DataParameterEqualityComparer class in the Cuemon.Data namespace to DbParameterEqualityComparer
- InOperator class in the Cuemon.Data namespace to be less complex and make usage of InOperatorResult
- DataTransferRowCollection in the Cuemon.Data namespace in the context of removing a legacy-leftover property: IsReadOnly
- ChecksumMethod enum in the Cuemon.Data.Integrity namespace was renamed to EntityDataIntegrityMethod (including rename of Default --> Unaltered)
- ChecksumStrength enum in the Cuemon.Data.Integrity namespace was renamed to EntityDataIntegrityValidation (including rename of None --> Unspecified)
- ICacheableIntegrity interface in the Cuemon.Data.Integrity namespace was renamed to IEntityDataIntegrity
- ICacheableTimestamp interface in the Cuemon.Data.Integrity namespace was renamed to IEntityDataTimestamp
- ICacheableEntity interface in the Cuemon.Data.Integrity namespace was renamed to IEntityInfo
- CacheValidatorFactory class in the Cuemon.Data.Integrity namespace to use same Hash factory provider
- SqlDataManager class in the Cuemon.Data.SqlClient namespace to be less dependant on base class and applied quality gate actions
- SqlInOperator class in the Cuemon.Data.SqlClient namespace to adapt the changes made to the base class
- ExceptionDescriptorAttribute class in the Cuemon.Diagnostics namespace to have a more simple and streamlined design
- TimeMeasure class in the Cuemon.Diagnostics namespace to fully support the Task-based Asynchronous Pattern (TAP) with cancellation
- ExceptionDescriptor class in the Cuemon.Diagnostics namespace with a new static method: Extract
- ApplicationBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Builder namespace in the context of renaming the UseHostingEnvironmentHeader method to UseHostingEnvironment
- ApplicationBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Builder namespace in the context of renaming the UseCorrelationIdentifierHeader method to UseCorrelationIdentifier
- ApplicationBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Builder namespace in the context of renaming the UseRequestIdentifierHeader method to UseRequestIdentifier
- ApplicationBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Builder namespace in the context of renaming the UseCustomThrottlingSentinel method to UseThrottlingSentinel
- ChecksumBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Data.Integrity namespace in the context of renaming the ToEntityTag method to ToEntityTagHeaderValue
- ChecksumBuilderExtensions class in the Cuemon.Extensions.AspNetCore.Http.Throttling namespace in the context of renaming the AddMemoryThrottling method to AddMemoryThrottlingCache
- HttpResponseExtensions class in the Cuemon.Extensions.AspNetCore.Http namespace in the context of renaming the SetEntityTagHeaderInformation method to AddOrUpdateEntityTagHeader
- HttpResponseExtensions class in the Cuemon.Extensions.AspNetCore.Http namespace in the context of renaming the SetLastModifiedHeaderInformation method to AddOrUpdateLastModifiedHeader
- HttpResponseExtensions class in the Cuemon.Extensions.AspNetCore.Http namespace in the context of:
  - Removing the method IsSuccessStatusCode
  - Removing the method IsNotModifiedStatusCode
- ServiceCollectionExtensions class in the Cuemon.Extensions.AspNetCore.Http.Throttling namespace with one new extension method for the IServiceCollection interface: AddThrottlingCache{T}
- HttpResponseExtensions class in the Cuemon.Extensions.AspNetCore.Http namespace with one new extension method for the HttpResponse class: OnStartingInvokeTransformer
- CacheableObjectResultExtensions class in the Cuemon.Extensions.AspNetCore.Mvc namespace in the context of:
  - Renaming the ToCacheableObjectResult{T} method to MakeCacheable{T}
  - Adding a non-generic variant: MakeCacheable
- HttpFaultResolverExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Filters.Diagnostics namespace in the context of renaming the Add{T} method to AddHttpFaultResolver{T}
- HtmlHelperExtensions class in the Cuemon.Extensions.AspNetCore.Mvc namespace in the context of renaming the UseWhen{T} method to UseWhenView{T}
- HtmlHelperExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Rendering namespace with one new extension method for the IHtmlHelper interface: UseWhenPage{T}
- CacheValidatorExtensions class in the Cuemon.Extensions.AspNetCore.Data.Integrity namespace in the context of renaming the ToEntityTag method to ToEntityTagHeaderValue
- JsonReaderResult class in the Cuemon.Extensions.Newtonsoft.Json namespace was renamed to JDataResult (including some refactoring)
- StringFlagsEnumConverter class in the Cuemon.Extensions.Newtonsoft.Json.Converters namespace to comply with Newtonsoft.Json.Serialization.NamingStrategy implementations
- JsonFormatterOptions class in the Cuemon.Extensions.Newtonsoft.Json namespace with several new options and a uniform way of adding default converters
- JsonConverterCollectionExtensions class in the Cuemon.Extensions.Newtonsoft.Json.Converters namespace to fully support whatever desired naming strategy wanted while simplifying the code greatly
- StringFlagsEnumConverter class in the Cuemon.Extensions.Newtonsoft.Json.Converters namespace to fully support whatever desired naming strategy wanted while simplifying the code greatly
- DynamicJsonConverter class in the Cuemon.Extensions.Newtonsoft.Json namespace to fully support whatever desired naming strategy wanted while being significantly more versatile in usage
- JsonWriterExtensions class in the Cuemon.Extensions.Newtonsoft.Json namespace to fully support whatever desired naming strategy wanted while simplifying the code greatly
- XmlReaderExtensions class in the Cuemon.Xml namespace in the context of renaming the Copy method to ToStream
- StreamWriterUtility class in the Cuemon.IO namespace was renamed to StreamFactory (and reduced overloads to max. 5 generic parameters)
- HttpManagerOptions class in the Cuemon.Net.Http namespace with a new method: SetHandlerFactory{T} (opt-in to allow set directly on HandlerFactory property)
- HttpManager class in the Cuemon.Net.Http namespace with a new constructor overload that takes a client factory delegate which creates and configures an HttpClient instance
- HttpManagerOptions class in the Cuemon.Net.Http namespace in the context of changing the default value for the DisposeHandler property from true to false
  - This is due to the way Microsoft has designed the HttpClient with an implementation of IDisposable that could result in SocketException errors if not instantiated once and re-used throughout the life of an application
  - This setting reduces the risk of SocketException errors on existing code
- AdvancedEncryptionStandardKeySize enum in the Cuemon.Security.Cryptography namespace was renamed to AesSize
- HashAlgorithmType enum in the Cuemon.Security.Cryptography namespace was renamed to UnkeyedCryptoAlgorithm
- HashUtility class in the Cuemon.Security.Cryptography namespace was renamed to UnkeyedHashFactory
- HmacAlgorithmType enum in the Cuemon.Security.Cryptography namespace was renamed to KeyedCryptoAlgorithm
- HmacUtility class in the Cuemon.Security.Cryptography namespace was renamed to KeyedHashFactory
- AdvancedParallelFactory class in the Cuemon.Security.Cryptography namespace in the context of:
  - Supports true async functionality: ForAsync, ForResultAsync, WhileAsync and WhileResultAsync
  - Advanced members was moved to here conform to Framework Design Guidelines
- ParallelFactory class in the Cuemon.Security.Cryptography namespace now supports true async functionality: ForAsync, ForResultAsync, ForEachAsync and ForEachResultAsync
- XPathNavigableConverter class in the Cuemon.Xml.XPath namespace was renamed to XPathDocumentFactory (including rename of remaining members)
- XmlDocumentConverter class in the Cuemon.Xml namespace was renamed to XmlDocumentFactory (including rename of remaining members)
- XmlStreamConverter class in the Cuemon.Xml namespace was renamed to XmlStreamFactory (including rename of remaining members)
- XmlConverterDecoratorExtensions class in the Cuemon.Xml.Serialization.Converters namespace to always express a DateTime value as an ISO8601 string with roundtrip to UTC
- XmlSerializerSettings class in the Cuemon.Xml.Serialization namespace was renamed to XmlSerializerOptions
- XmlSerializer class in the Cuemon.Xml.Serialization namespace with a new overloaded method: Serialize

### Fixed

- HostingEnvironmentMiddleware class in the Cuemon.AspNetCore.Hosting namespace to be compliant with https://rules.sonarsource.com/csharp/RSPEC-927
- DigestAuthorizationHeaderBuilder class in the Cuemon.AspNetCore.Authentication.Digest namespace to be compliant with https://rules.sonarsource.com/csharp/RSPEC-3358
- DigestAuthenticationMiddleware class in the Cuemon.AspNetCore.Authentication.Digest namespace to be compliant with https://rules.sonarsource.com/csharp/RSPEC-927
- DigestAuthorizationHeader class in the Cuemon.AspNetCore.Authentication.Digest namespace to be compliant with https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1822
- HmacAuthorizationHeaderBuilder class in the Cuemon.AspNetCore.Authentication.Hmac namespace to be compliant with https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1834
- ServerTimingFilter class in the Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace to be compliant with https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1825
- All relevant classes in the Cuemon.AspNetCore.Mvc namespace to be compliant with https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30#allowsynchronousio-disabled
- Minor bug in the AssemblyDecoratorExtensions class located in the Cuemon.Reflection namespace
- Minor bug in the PropertyInfoDecoratorExtensions class located in the Cuemon.Reflection namespace
- SystemSnapshot enum in the Cuemon namespace to SystemSnapshots to be compliant with https://rules.sonarsource.com/csharp/RSPEC-2342
- Validator class in the Cuemon namespace to be compliant with https://rules.sonarsource.com/csharp/RSPEC-4136
- Hierarchy class in the Cuemon namespace to be compliant with https://rules.sonarsource.com/csharp/RSPEC-1066
- Hierarchy class in the Cuemon namespace to be compliant with https://rules.sonarsource.com/csharp/RSPEC-4456
- UnitPrefixFormatter class in the Cuemon namespace to be compliant with https://rules.sonarsource.com/csharp/RSPEC-927
- Justified https://rules.sonarsource.com/csharp/RSPEC-3925 on DataPairDictionary class in the Cuemon.Collections namespace
- CyclicRedundancyCheck class in the Cuemon.Security namespace to be compliant with https://rules.sonarsource.com/csharp/RSPEC-1264
- Arguments class in the Cuemon.Collections.Generic namespace to be compliant with https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1825
- ProcessInfo class in the Cuemon.Diagnostics namespace to be compliant with https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1834
- BinaryPrefix class in the Cuemon namespace to have 0 duplicated blocks of lines of code
- DecimalPrefix class in the Cuemon namespace to have 0 duplicated blocks of lines of code
- BitUnit class in the Cuemon namespace to have 0 duplicated blocks of lines of code
- ByteUnit class in the Cuemon namespace to have 0 duplicated blocks of lines of code
- ByteArrayDecoratorExtensions class in the Cuemon namespace to be compliant with https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1835
- StringDecoratorExtensions class in the Cuemon namespace to be compliant with https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1835
- HashResult class in the Cuemon.Security namespace to be compliant with https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1825
- Template class in the Cuemon namespace to be compliant with https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1825
- Justified https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1068 on StringDecoratorExtensions class in the Cuemon namespace
- MethodDescriptor class in the Cuemon.Reflection namespace to be compliant with https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1834
- UnitPrefixFormatter class in the Cuemon namespace to be compliant with https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1822
- Justified https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1822 on Validator class in the Cuemon namespace
- ByteOrderMark class in the Cuemon.Text namespace to be compliant with https://rules.sonarsource.com/csharp/RSPEC-3776
- AssemblyDecoratorExtensions class in the Cuemon.Reflection namespace to be compliant with https://rules.sonarsource.com/csharp/RSPEC-3776
- ParserFactory class in the Cuemon.Text namespace to be compliant with https://rules.sonarsource.com/csharp/RSPEC-3776
- DataTransferRowCollection class in the Cuemon.Data namespace to be compliant with https://rules.sonarsource.com/csharp/RSPEC-3358
- DsvDataReader class in the Cuemon.Data namespace to have 0 duplicated blocks of lines of code by removing ConcurrentDsvDataReader class and merge ReadAsync to DsvDataReader
- XmlDataReader class in the Cuemon.Data.Xml namespace to be compliant with https://rules.sonarsource.com/csharp/RSPEC-3776
- DataManager class in the Cuemon.Data namespace to be compliant with https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1822
- DataIntegrityFactory class in the Cuemon.Data.Integrity namespace to be compliant with https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1825
- TimeMeasure class in the Cuemon.Diagnostics namespace to be compliant with https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1068
- HttpResponseExtensions class in the Cuemon.Extensions.AspNetCore.Http namespace to be compliant with https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1835
- HtmlHelperExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Rendering namespace to have 0 duplicated blocks of lines of code
- All relevant classes in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace to be compliant with https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30#allowsynchronousio-disabled
- JsonSerializationInputFormatter class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace to have 0 duplicated blocks of lines of code
- JsonSerializationOutputFormatter class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace to have 0 duplicated blocks of lines of code
- JsonConverterCollectionExtensions class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Converters namespace to have 0 duplicated blocks of lines of code
- All relevant classes in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml namespace to be compliant with https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30#allowsynchronousio-disabled
- XmlSerializationInputFormatter class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml namespace to have 0 duplicated blocks of lines of code
- XmlSerializationOutputFormatter class in the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml namespace to have 0 duplicated blocks of lines of code
- A bug that would disallow use of extension method Chunk on EnumerableExtensions class in the Cuemon.Extensions.Collections.Generic namespace
- Justified https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca2200 on ValidatorExtensions class in the Cuemon.Extensions namespace
- Justified https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca2249 on StringExtensions class in the Cuemon.Extensions namespace
- ServiceCollectionExtensions class in the Cuemon.Extensions.DependencyInjection namespace to be compliant with https://rules.sonarsource.com/csharp/RSPEC-4136
- StringExtensions class in the Cuemon.Extensions.IO namespace to be compliant with https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1068
- Justified https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca2200 on ValidatorExtensions class in the Cuemon.Extensions.Newtonsoft.Json namespace
- JsonReaderExtensions class in the Cuemon.Extensions.Newtonsoft.Json namespace to have 0 duplicated blocks of lines of code
- JsonConverterCollectionExtensions class in the Cuemon.Extensions.Newtonsoft.Json.Converters namespace to have 0 duplicated blocks of lines of code
- MvcFilterAspNetCoreHostTest class in the Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc namespace to have 0 duplicated blocks of lines of code
- Justified https://rules.sonarsource.com/csharp/RSPEC-2436 on StreamFactory class in the Cuemon.IO namespace
- StreamDecoratorExtensions class in the Cuemon.IO namespace to be compliant with https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1068
- StringDecoratorExtensions class in the Cuemon.Net namespace to be compliant with https://rules.sonarsource.com/csharp/RSPEC-3358
- ByteArrayDecoratorExtensions class in the Cuemon.Net namespace to be compliant with https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1825
- Justified https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca2249 on StringDecoratorExtensions class in the Cuemon.Net namespace
- TransientOperation class in the Cuemon.Resilience namespace to be compliant with https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1068
- Applied ConfigureAwait(false) to all async methods
- A bug that would lead to endless loop if workload was 1 (PartitionSize)
- A bug that in some cases could throw a SerializationException when serializable type consisted of only a default contructor (Cuemon.Xml.Serialization.Converters.DefaultXmlConverter.ParseReadXmlDefault)
- A bug that would trigger a redundant entry of InnerException on AggregateException when serializing exceptions (Cuemon.Xml.Serialization.Converters.XmlConverterDecoratorExtensions.WriteInnerExceptions)
- A bug that could trigger a NullReferenceException when serializing namespace information on an Exception class (Cuemon.Xml.Serialization.Converters.XmlConverterDecoratorExtensions.WriteException)
- Assignment of default converters with multicast delegate (Cuemon.Xml.Serialization.Formatters.XmlFormatterOptions.ctor)
- DefaultXmlConverter class in the Cuemon.Xml.Serialization.Converters namespace to be compliant with https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1822
- XmlReaderDecoratorExtensions class in the Cuemon.Xml namespace to have 0 duplicated blocks of lines of code

### Removed

- Any former extension methods of the Cuemon.AspNetCore namespace was merged into the Cuemon.Extensions.AspNetCore namespace (except for the HttpResponseMessageExtensions class)
- ThrottlingRetryAfterHeader enum from the Cuemon.AspNetCore.Http.Throttling namespace
- Any former extension methods of the Cuemon.AspNetCore.Authentication namespace was merged into the Cuemon.Extensions.AspNetCore.Authentication namespace
- DigestAccessAuthenticationParameters class from the Cuemon.AspNetCore.Authentication namespace
- DigestAuthenticationUtility class from the Cuemon.AspNetCore.Authentication namespace
- HmacAuthenticationParameters class from the Cuemon.AspNetCore.Authentication namespace
- ICacheBusting interface (and related) from the Cuemon.AspNetCore.Mvc.Configuration namespace
- AssemblyCacheBusting class (and related) from the Cuemon.AspNetCore.Mvc.Configuration namespace
- Any former extension methods of the Cuemon.AspNetCore.Razor.TagHelpers namespace was merged into the Cuemon.Extensions.Core namespace
- StringFormatter class from the Cuemon namespace
- StandardizedDateTimeFormatPattern enum from the Cuemon namespace
- ComparisonUtility class from the Cuemon.Collections.Generic namespace
- DictionaryConverter class from the Cuemon.Collections.Generic namespace
- DictionaryUtility class from the Cuemon.Collections.Generic namespace
- EnumerableConverter class from the Cuemon.Collections.Generic namespace
- EnumerableUtility class from the Cuemon.Collections.Generic namespace
- ISortableTable interface from the Cuemon.Collections.Generic namespace
- ListUtility class in from Cuemon.Collections.Generic namespace
- PartitionCollection class from the Cuemon.Collections.Generic namespace
- MethodBaseConverter class from the Cuemon.Reflection namespace
- ReflectionUtility class from the Cuemon.Reflection namespace
- JsonConverter class from the Cuemon.Runtime.Serialization namespace
- JsonInstance class from the Cuemon.Runtime.Serialization namespace
- JsonInstanceCollection class from the Cuemon.Runtime.Serialization namespace
- JsonTextWriter class from the Cuemon.Runtime.Serialization namespace
- JsonWriter class from the Cuemon.Runtime.Serialization namespace
- EncodingConverter class from the Cuemon.Text namespace
- EncodingUtility class from the Cuemon.Text namespace
- JsonWebToken class from the Cuemon.Security.Web namespace
- JsonWebTokenHashAlgorithm class from the Cuemon.Security.Web namespace
- JsonWebTokenHashAlgorithmConverter class from the Cuemon.Security.Web namespace
- JsonWebTokenHeader class from the Cuemon.Security.Web namespace
- JsonWebTokenPayload class from the Cuemon.Security.Web namespace
- Obfuscator class from the Cuemon.Security namespace
- ObfuscatorMapping class from the Cuemon.Security namespace
- SecurityToken class from the Cuemon.Security namespace
- SecurityTokenSettings class from the Cuemon.Security namespace (replaced with SignedUriOptions in the Cuemon.Extensions.Net.Security namespace)
- SecurityUtility class from the Cuemon.Security namespace
- ArgumentEmptyException class from the Cuemon namespace
- ByteConverter class from the Cuemon namespace
- ByteUtility class from the Cuemon namespace
- CharConverter class from the Cuemon namespace
- ConditionBuilder class from the Cuemon namespace
- Converter class from the Cuemon namespace
- ConvertibleConverter class from the Cuemon namespace
- DateTimeConverter class from the Cuemon namespace
- DelegateUtility class from the Cuemon namespace
- DoubleConverter class from the Cuemon namespace
- EnumUtility class from the Cuemon namespace
- EventUtility class from the Cuemon namespace
- ExceptionUtility class from the Cuemon namespace
- GuidConverter class from the Cuemon namespace
- GuidUtility class from the Cuemon namespace
- HierarchySerializer class from the Cuemon namespace
- HierarchyUtility class from the Cuemon namespace
- LoopUtility class from the Cuemon namespace
- MethodWrappedException class from the Cuemon namespace
- NumberUtility class from the Cuemon namespace
- ObjectConverter class from the Cuemon namespace
- RandomSeverity enum from the Cuemon namespace
- StringConverter class from the Cuemon namespace
- StringFormatter class from the Cuemon namespace
- StringUtility class from the Cuemon namespace
- StructUtility class from the Cuemon namespace
- TesterDoer class from the Cuemon namespace
- TesterFuncUtility class from the Cuemon namespace
- TimeSpanConverter class from the Cuemon namespace
- TupleUtility class from the Cuemon namespace
- TypeCodeConverter class from the Cuemon namespace
- TypeUtility class from the Cuemon namespace
- UriConverter class from the Cuemon namespace
- UriUtility class from the Cuemon namespace
- VersionUtility class from the Cuemon namespace
- IMessageLocalizer interface from the Cuemon.Globalization namespace
- PagedCollection class from the Cuemon.Collections.Generic namespace
- PagedSettings class from the Cuemon.Collections.Generic namespace
- LatencyException class from the Cuemon namespace
- TransientOperation class from the Cuemon namespace
- TransientFaultEvidence class from the Cuemon namespace
- TransientFaultException class from the Cuemon namespace
- TransientOperation class from the Cuemon namespace
- TransientOperationOptions class from the Cuemon namespace
- Any former extension methods of the Cuemon namespace (and related) was either removed completely or merged into their respective Cuemon.Extensions.* namespace equivalent
- The Cuemon.Data.XmlClient assembly and namespace was removed with this version
- Any former extension methods of the Cuemon.Data namespace was merged into the Cuemon.Extensions.Data namespace
- DataTransferSorter class from the Cuemon.Data namespace
- CsvDataReader class from the Cuemon.Data.CsvClient namespace (including the namespace)
- Cuemon.Data.SqlClient namespace and merged it to its own assembly
- QueryUtility class from the Cuemon.Data namespace
- GetPagedRows from the DataTransfer class in the Cuemon.Data namespace
- DataWatcher class from the Cuemon.Data namespace
- DataDependency class from the Cuemon.Data namespace
- The Cuemon.Integrity namespace was removed with this version
- Any former extension methods of the Cuemon.Integrity namespace was merged into the Cuemon.Extensions.Data.Integrity namespace
- EventLogEntryType enum from the Cuemon.Diagnostics namespace as it is now (finally) part of .NET Platform Extensions and .NET Core
- ApplicationBuilderExtensions class from the Cuemon.Extensions.AspNetCore.Mvc namespace
- The Cuemon.AspNetCore.Mvc.Formatters.Json namespace was removed with this version
- Any types found in the Cuemon.AspNetCore.Mvc.Formatters.Json namespace was merged into the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace
- DefaultJsonSerializerSettings class from the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace
- The Cuemon.AspNetCore.Mvc.Formatters.Xml namespace was removed with this version
- Any types found in the Cuemon.AspNetCore.Mvc.Formatters.Xml namespace was merged into the Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml namespace
- The Cuemon.Serialization.Json namespace was removed with this version
- Any types found in the Cuemon.Serialization.Json namespace was merged into the Cuemon.Extensions.Newtonsoft.Json namespace
- JsonReaderResultExtensions class from the Cuemon.Extensions.Newtonsoft.Json namespace
- JsonReaderParser class from the Cuemon.Extensions.Newtonsoft.Json namespace
- The Cuemon.IO.Compression namespace was removed with this version
- Any former extension methods of the Cuemon.IO namespace was merged into the Cuemon.Extensions.IO namespace
- CompressionType enum from the Cuemon.IO.Compression namespace
- CompressionUtility class from the Cuemon.IO.Compression namespace
- CompressionUtilityExtensions class from the Cuemon.IO.Compression namespace
- FileInfoConverter class from the Cuemon.IO namespace
- StreamConverter class from the Cuemon.IO namespace
- StreamConverterExtensions class from the Cuemon.IO namespace
- TextReaderConverter class from Cuemon.IO namespace
- TextReaderConverterExtensions class from the Cuemon.IO namespace
- The Cuemon.Net.Mail assembly was removed with this version
- Any former extension methods of the Cuemon.Net namespace was merged into the Cuemon.Extensions.Net namespace
- NetDependency class from the Cuemon.Net namespace
- NetWatcher class from the Cuemon.Net namespace
- The Cuemon.Security assembly was removed with this version
- Any former extension methods of the Cuemon.Security namespace was removed completely due to the new intuitive static factory classes (HashFactory, KeyedHashFactory and UnkeyedHashFactory)
- AdvancedEncryptionStandardUtility class from the Cuemon.Security.Cryptography namespace
- HashOptions class from the Cuemon.Security.Cryptography namespace
- HashUtilityExtensions class from the Cuemon.Security.Cryptography namespace
- KeyedHashOptions class from the Cuemon.Security.Cryptography namespace
- PolynomialRepresentation enum from the Cuemon.Security.Cryptography namespace
- StreamHashOptions class from the Cuemon.Security.Cryptography namespace
- StreamKeyedHashOptions class from the Cuemon.Security.Cryptography namespace
- StringHashOptions class from the Cuemon.Security.Cryptography namespace
- StringKeyedHashOptions class from the Cuemon.Security.Cryptography namespace
- StrongNumberUtility class from the Cuemon.Security.Cryptography namespace
- CyclicRedundancyCheck class from the Cuemon.Security.Cryptography namespace
- CyclicRedundancyCheck32 class from the Cuemon.Security.Cryptography namespace
- HashResult class from the Cuemon.Security.Cryptography namespace
- ThreadPoolUtility class from the Cuemon.Threading namespace
- The Cuemon.Serialization.Xml assembly and namespace was removed with this version
- Any former extension methods of the Cuemon.Xml namespace was merged into the Cuemon.Extensions.Xml namespace
- XElementExtensions class from the Cuemon.Xml.Serialization.Linq namespace
- SerializableOrder class from the Cuemon.Xml.Serialization namespace
- XmlJsonInstance class from the Cuemon.Xml.Serialization namespace
- JsonStreamConverter class from the Cuemon.Xml namespace
- SecureXmlObfuscator class from the Cuemon.Xml namespace
- XmlConvertExtensions class from the Cuemon.Xml namespace
- XmlCopyOptions class from the Cuemon.Xml namespace
- XmlEncodingUtility class from the Cuemon.Xml namespace
- XmlObfuscator class from the Cuemon.Xml namespace
- XmlReaderConverter class from the Cuemon.Xml namespace
- XmlReaderUtility class from the Cuemon.Xml namespace
- XmlReaderUtilityExtensions class from the Cuemon.Xml namespace
- XmlUtility class from the Cuemon.Xml namespace
- XmlUtilityExtensions class from the Cuemon.Xml namespace
- XmlWriterUtility class from Cuemon.Xml namespace
- XmlWriterUtilityExtensions class from the Cuemon.Xml namespace

[10.5.3]: https://github.com/codebeltnet/cuemon/compare/v10.5.2...v10.5.3
[10.5.2]: https://github.com/codebeltnet/cuemon/compare/v10.5.1...v10.5.2
[10.5.1]: https://github.com/codebeltnet/cuemon/compare/v10.5.0...v10.5.1
[10.5.0]: https://github.com/codebeltnet/cuemon/compare/v10.4.0...v10.5.0
[10.4.0]: https://github.com/codebeltnet/cuemon/compare/v10.3.0...v10.4.0
[10.3.0]: https://github.com/codebeltnet/cuemon/compare/v10.2.1...v10.3.0
[10.2.1]: https://github.com/codebeltnet/cuemon/compare/v10.2.0...v10.2.1
[10.2.0]: https://github.com/codebeltnet/cuemon/compare/v10.1.2...v10.2.0
[10.1.2]: https://github.com/codebeltnet/cuemon/compare/v10.1.1...v10.1.2
[10.1.1]: https://github.com/codebeltnet/cuemon/compare/v10.1.0...v10.1.1
[10.1.0]: https://github.com/codebeltnet/cuemon/compare/v10.0.0...v10.1.0
[10.0.0]: https://github.com/codebeltnet/cuemon/releases/tag/v10.0.0
