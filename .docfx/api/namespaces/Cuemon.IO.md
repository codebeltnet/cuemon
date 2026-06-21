---
uid: Cuemon.IO
summary: *content
---
Configure IO operations — compression, encoding, streaming, and buffering — through options types that extend `System.IO` infrastructure. Use this namespace when you need to configure stream compression, encoding, or buffering settings. Start with `CompressGZip` on `IDecorator<Stream>` for GZip compression, or `ToEncodedString` for encoding-aware stream output.

[!INCLUDE [availability-all](../../includes/availability-all.md)]

Complements: [System.IO namespace](https://docs.microsoft.com/en-us/dotnet/api/system.io)

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IDecorator<Stream>|⬇️|`CopyTo`, `CopyStream`, `CopyStreamAsync`, `ToByteArray`, `ToByteArrayAsync`, `ToEncodedString`, `ToEncodedStringAsync`, `InvokeToByteArray`, `WriteAllAsync`, `CompressBrotli`, `CompressBrotliAsync`, `DecompressBrotli`, `DecompressBrotliAsync`, `CompressGZip`, `CompressGZipAsync`, `DecompressGZip`, `DecompressGZipAsync`, `CompressDeflate`, `CompressDeflateAsync`, `DecompressDeflate`, `DecompressDeflateAsync`|
|IDecorator<TextReader>|⬇️|`CopyToAsync`|
