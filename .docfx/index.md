---
uid: concept-md
title: Concept Reference
---
# Concept Reference

**Cuemon for .NET** was born out of simple idea; follow the design guidelines in the book Framework Design Guidelines by Krzysztof Cwalina and Brad Abrams while staying true to always follow the namespace of Microsoft .NET Framework 2.0.

A lot of water has passed under the bridge since the first version of **Cuemon .NET Framework Additions**, which consisted of only four assemblies and was closed source from 2008 till 2015.

Fast forwarding till today, **Cuemon for .NET** has matured and grown to be a reference project for all coders, programmers, developers and the likes thereof; programmed with passion and love for .NET, C# and software architecture in general.

With that prelude, lets look at the concepts behind the work.

## Rules of engagement

+ Follow Framework Design Guidelines and Engineering Guidelines from Microsoft
+ Make sure to increment code coverage from unit test regularly
+ Follow recommendations from code quality analyzers; justify when a rule is either false-positive or not relevant
+ Maintain release notes and keep them as part of the code under Properties (PackageReleaseNotes.txt)
+ Adhere to SOLID principles for all new features
+ Do not let code rot; always embrace the boy scout rule
+ Make sure refactoring is done only on unit tested code (if no unit test - write unit test)
+ Extension methods in non-extension assemblies must be hidden by the IDecorator interface
+ Do not duplicate code

### Solution Structure

The solution file is placed in the root of the Git repo.

Any supporting file for the solution (such as versioning, documentation, pipeline) is allowed to be placed in the root of the Git repo.\
However, if and when supporting files start to clutter the solution, group them in context specific folders such as `/.azure`, `/.github`, `/.nuget` and the likes hereof.

Any build properties shared between projects must be placed in `Directory.Build.props`.\
Any special build handling of projects must be placed in `Directory.Build.targets`.

All projects that is candidate for a NuGet package is placed in `src`.\
All projects that is not candidate for a NuGet package is placed in `tooling`.\
All unit test projects is placed in `test` and must follow these rules (as stated by Microsoft Engineering Guidelines):

- The unit tests for the `Cuemon.Foo` assembly live in the `Cuemon.Foo.Tests` assembly
- The functional tests for the `Cuemon.Foo` assembly live in the `Cuemon.Foo.FunctionalTests` assembly
- Test class names end with `Test` and live in the **same namespace** as the class being tested, e.g., the unit tests for the `Boo` class that resides in the `Cuemon.Foo` assembly would be named `BooTest` and placed in the `Cuemon.Foo` namespace (hint: `<RootNamespace>Cuemon.Foo</RootNamespace>`) in the `Cuemon.Foo.Tests` assembly

Example of structure:

```
/src/Cuemon.Core/Security/HashFactory.cs
/src/Cuemon.Core/Cuemon.Core.csproj
/src/Cuemon.Core/Disposable.cs
/test/Cuemon.Core.Tests/Security/HashFactoryTest.cs
/test/Cuemon.Core.Tests/Cuemon.Core.Tests.csproj
/test/Cuemon.Core.Tests/DisposableTest.cs
/CHANGELOG.md
/Cuemon.sln
/Directory.Build.props
/Directory.Build.targets
/LICENSE.md
/README.md
```

The general naming pattern for projects is `Cuemon.AspNetCore.<area>.<subarea>`, `Cuemon.<area>.<subarea>`, and `Cuemon.Extensions.<area>.<subarea>`.\
`Cuemon.Extensions.<area>.<subarea>` covers extension methods and/or new features that extends upon an existing project/package, such as `Cuemon.Extensions.Net`/`Cuemon.Extensions.Xunit`.

### Principles

+ SOLID
  + **S**ingle Responsibility Principle (SRP)
  + **O**pen-Closed Principle (OCP)
  + **L**iskov Substitution Principle (LSP)
  + **I**nterface Segregation Principle (ISP)
  + **D**ependency Inversion Principle (DIP)
+ DRY
+ Consistency
+ Generality
+ Abstraction
+ Modularity
+ Separation of Concerns (SoC)
  + Same concept as two of the most important design principles of SOLID; SRP and ISP
+ Measure Twice, Cut Once

### Patterns

+ Factory Method
+ Options / Parameter Object
+ Template Method
+ Decorator
+ Builder
+ Abstract Factory

### Reusability

### Extensions

Extensions, as in [extension methods](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods), is prohibited in all projects not following the `Cuemon.Extensions.<area>.<subarea>` convention.

That said, it is allowed to add extension methods that are not pure; eg. they have to be hidden by the [IDecorator{T} interface](https://github.com/gimlichael/Cuemon/blob/development/src/Cuemon.Core/IDecorator.cs).

Reason for this rule is, that developers are divided into two groups; those who embrace extension methods - and those who does not.

So by adhering to this rule, we ensure that both groups is taken care of; the former can consciously include `Cuemon.Extensions.<area>.<subarea>` and benefit from an abundant amount of extension methods, while the former can have peace of mind that no foreign extension methods is cluttering the IntelliSense.

### Delegates

### Tools

+ [GitHub from Microsoft](https://github.com/) is used to host the source code of Cuemon for .NET
+ [Azure DevOps from Microsoft](https://azure.microsoft.com/en-us/services/devops/) is used for CI/CD integration with GitHub
+ Distributed packages are based on [NuGet](https://www.nuget.org/)
  + Preview packages was pushed to a private feed prior to latest release of .NET 8.0 - [for reference](https://nuget.codebelt.net/v3/index.json)
  + Stable, RCs and select previews are pushed to NuGet (https://api.nuget.org/v3/index.json)
+ [DocFx](https://github.com/dotnet/docfx) is used to produce documentation from source code including raw Markdown files
+ [Visual Studio 2022 from Microsoft](https://visualstudio.microsoft.com/vs/) is used as the primary tool for writing CSharp code
+ [ReSharper from JetBrains](https://www.jetbrains.com/resharper/) is an indispensable Visual Studio extension that makes development, refactoring and unit testing a bliss to work with
+ [GhostDoc Pro from SubMain Software](https://submain.com/ghostdoc/) is used to write all the source code documentation
+ [Visual Studio Code from Microsoft](https://code.visualstudio.com/) is used to anything besides writing CSharp code
+ [NuGet Package Explorer](https://github.com/NuGetPackageExplorer/NuGetPackageExplorer) is helpful to iron out kinks of NuGet packages
+ It goes without saying, that without [Microsoft .NET](https://dotnet.microsoft.com/) there would be no [Cuemon for .NET](https://github.com/gimlichael/Cuemon)

## Continuous improvement of Cuemon for .NET

The constant of the code being written for **Cuemon for .NET** has and will always be **Framework Design Guidelines: Conventions, Idioms, and Patterns for Reusable .NET Libraries**. In more recent history, [Engineering Guidelines](https://github.com/dotnet/aspnetcore/wiki/Engineering-guidelines) from Microsoft ASP.NET Core team, has also been widely adapted into this project.

Here is a non-exhausting list of literature and websites that inspires and keep this project going:

### Books

+ [Framework Design Guidelines (1st. edition)](https://www.amazon.com/Framework-Design-Guidelines-Conventions-Libraries/dp/0321246756)
+ [Framework Design Guidelines (2nd. edition)](https://www.amazon.com/Framework-Design-Guidelines-Conventions-Libraries/dp/0321545613)
+ [Framework Design Guidelines (3rd. edition)](https://www.amazon.com/Framework-Design-Guidelines-Conventions-Addison-Wesley/dp/0135896460)
+ [Clean Code: A Handbook of Agile Software Craftsmanship](https://www.amazon.com/Clean-Code-Handbook-Software-Craftsmanship/dp/0132350882)
+ [Clean Architecture: A Craftsman's Guide to Software Structure and Design](https://www.amazon.com/Clean-Architecture-Craftsmans-Software-Structure/dp/0134494164)
+ [The Clean Coder: A Code of Conduct for Professional Programmers](https://www.amazon.com/Clean-Coder-Conduct-Professional-Programmers/dp/B08X7MNTCX/)
+ [Building Maintainable Software, C# Edition: Ten Guidelines for Future-Proof Code](https://www.amazon.com/Building-Maintainable-Software-Guidelines-Future-Proof-ebook/dp/B01GSRN582)
+ [Agile Principles, Patterns, and Practices in C#](https://www.amazon.com/Agile-Principles-Patterns-Practices-C/dp/0131857258)
+ [CLR via C#](https://www.amazon.com/CLR-via-C-Developer-Reference/dp/0735627045)
+ [Microsoft .NET - Architecting Applications for the Enterprise](https://www.amazon.com/Microsoft-NET-Architecting-Applications-Enterprise/dp/0735685355)
+ [The Software Craftsman: Professionalism, Pragmatism, Pride](https://www.amazon.com/Software-Craftsman-Professionalism-Pragmatism-Robert/dp/0134052501/)
+ [The Pragmatic Programmer: 20th Anniversary Edition, 2nd Edition: Your Journey to Mastery](https://www.amazon.com/Pragmatic-Programmer-Anniversary-Journey-Mastery/dp/B0833FBNHV/)
+ [Clean Craftsmanship: Disciplines, Standards, and Ethics](https://www.amazon.com/Clean-Craftsmanship-Disciplines-Standards-Ethics/dp/013691571X/)

### Links

+ [Separation of Concerns in Software Design](https://nalexn.github.io/separation-of-concerns/)
+ [Microsoft Engineering Guidelines](https://github.com/dotnet/aspnetcore/wiki/Engineering-guidelines)
+ [Refactoring Guru](https://refactoring.guru/design-patterns)
+ [Options pattern in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options)
+ [Dependency injection in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)
+ [Clean Code in 5 minutes](https://issuu.com/softhouse/docs/cleancode_5minutes_120523)
+ [The Boy Scout Rule](https://biratkirat.medium.com/step-8-the-boy-scout-rule-robert-c-martin-uncle-bob-9ac839778385)
+ [Manifesto for Software Craftsmanship](https://manifesto.softwarecraftsmanship.org/)
