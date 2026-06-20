---
uid: Cuemon.AspNetCore.Authentication.Hmac
summary: *content
---
Implement [HMAC-based request signing](https://www.okta.com/identity-101/hmac/) (inspired by [AWS Signature Version 4](https://docs.aws.amazon.com/AmazonS3/latest/API/sigv4-auth-using-authorization-header.html) and its [signing process](https://docs.aws.amazon.com/general/latest/gr/sigv4_signing.html)) in your ASP.NET Core application. Use this namespace when you need to authenticate requests using a hash-based message authentication code. Start with `HmacAuthenticationHandler` for validating HMAC-signed requests in your authentication pipeline.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)