---
uid: Cuemon.Extensions.Net.Security
summary: *content
---
Create tamper-proof signed URIs that expire, enabling your own shared access signature (SAS) pattern without Azure dependencies. Use this namespace when you need to sign URIs with expiration for secure resource access. Start with `ToSignedUri` on a `String` or `Uri` to produce a signed URI, then call `ValidateSignedUri` to verify authenticity later.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|String|⬇️|`ToSignedUri`, `ValidateSignedUri`|
|Uri|⬇️|`ToSignedUri`, `ValidateSignedUri`|
