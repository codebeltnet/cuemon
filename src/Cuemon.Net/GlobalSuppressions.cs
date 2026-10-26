// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "PlatformNotSupportedException", Scope = "type", Target = "~T:Cuemon.Net.QueryStringCollection")]
[assembly: SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "https://github.com/mono/mono/blob/da11592cbea4269971f4b1f9624769a85cc10660/mcs/class/System.Web/System.Web/HttpUtility.cs", Scope = "member", Target = "~M:Cuemon.Net.StringDecoratorExtensions.UrlDecode(Cuemon.IDecorator{System.String},System.Action{Cuemon.Text.EncodingOptions})~System.String")]
[assembly: SuppressMessage("Major Bug", "S2259:Null pointers should not be dereferenced", Justification = "False-Positive; if hasUtcLastModified is false, hasEntityTag has to be true - otherwise an exception is thrown.", Scope = "member", Target = "~M:Cuemon.Net.Http.HttpWatcher.FetchUsingHttpHeadAsync(Cuemon.Net.Http.HttpManager)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Minor Code Smell", "S3236:Caller information arguments should not be provided explicitly", Justification = "By design.", Scope = "member", Target = "~M:Cuemon.Net.Mail.MailDistributor.SendOneAsync(System.Net.Mail.MailMessage,System.Func{System.Net.Mail.MailMessage,System.Boolean})~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Minor Code Smell", "S3236:Caller information arguments should not be provided explicitly", Justification = "By design.", Scope = "member", Target = "~M:Cuemon.Net.Http.HttpManager.#ctor(System.Action{Cuemon.Net.Http.HttpManagerOptions})")]
[assembly: SuppressMessage("Style", "IDE0130:Namespace does not match folder structure", Justification = "Intentional as these embark on IDecorator.", Scope = "namespace", Target = "~N:Cuemon.Net")]
[assembly: SuppressMessage("Style", "IDE0130:Namespace does not match folder structure", Justification = "Intentional as these embark on IDecorator.", Scope = "namespace", Target = "~N:Cuemon.Net.Collections.Specialized")]
