---
uid: Cuemon.Extensions.IO
summary: *content
---
Convert strings and byte arrays to streams, compress and decompress using Brotli, Deflate, or GZip, detect unicode encoding, and read text content asynchronously. Use this namespace when you need comprehensive I/O operations like stream compression, encoding detection, or string-to-stream conversion. Start with `ToStream` on `String` or `byte[]` for stream conversion, or `CompressGZip` on `Stream` for compression.

[!INCLUDE [availability-all](../../includes/availability-all.md)]

Complements: [Cuemon.IO namespace](https://docs.cuemon.net/api/dotnet/Cuemon.IO.html) 🔗

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|byte[]|⬇️|`ToStream`, `ToStreamAsync`|
|Stream|⬇️|`Concat`, `ToCharArray`, `ToByteArray`, `ToByteArrayAsync`, `WriteAllAsync`, `TryDetectUnicodeEncoding`, `ToEncodedString`, `ToEncodedStringAsync`, `CompressBrotli`, `CompressBrotliAsync`, `CompressDeflate`, `CompressDeflateAsync`, `CompressGZip`, `CompressGZipAsync`, `DecompressBrotli`, `DecompressBrotliAsync`, `DecompressDeflate`, `DecompressDeflateAsync`, `DecompressGZip`, `DecompressGZipAsync`|
|String|⬇️|`ToStream`, `ToStreamAsync`, `ToTextReader`|
|TextReader|⬇️|`CopyToAsync`, `ReadAllLines`, `ReadAllLinesAsync`|
