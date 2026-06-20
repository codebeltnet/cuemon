---
uid: Cuemon.Extensions.Runtime
summary: *content
---
Model runtime objects as hierarchical tree structures to inspect their relationships, paths, and structure during application execution. Use this namespace when you need to build and navigate object graphs as hierarchies. Start with `IHierarchy<T>` for defining hierarchical data or `HierarchyDecoratorExtensions` for navigating tree structures.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Cuemon.Extensions namespace](/api/extensions/dotnet/Cuemon.Extensions.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IDecorator<IHierarchy<DataPair>>|⬇️|`UseConvertibleFormatter`, `UseUriFormatter`, `UseDateTimeFormatter`, `UseGuidFormatter`, `UseStringFormatter`, `UseDecimalFormatter`, `UseCollection`, `UseDictionary`|
|IDecorator<IHierarchy<T>>|⬇️|`FindFirstInstance<T>`, `FindSingleInstance<T>`, `FindInstance<T>`, `FindFirst<T>`, `FindSingle<T>`, `Find<T>`, `Replace<T>`, `Root<T>`, `AncestorsAndSelf<T>`, `DescendantsAndSelf<T>`, `SiblingsAndSelf<T>`, `SiblingsAndSelfAt<T>`, `NodeAt<T>`, `FlattenAll<T>`|
|IDecorator<IEnumerable<IHierarchy<T>>>|⬇️|`ReplaceAll<T>`|
|IHierarchy<DataPair>|⬇️|`UseGenericConverter<T>`|
|IEnumerable<IHierarchy<DataPair>>|⬇️|`ParseCollectionItem`, `ParseDictionaryItem`|

Related: [Cuemon.Extensions.Runtime.Serialization namespace](/api/extensions/dotnet/Cuemon.Extensions.Runtime.Serialization.html) 📘
