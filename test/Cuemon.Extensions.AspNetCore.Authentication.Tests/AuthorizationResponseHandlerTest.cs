using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication.Basic;
using Cuemon.AspNetCore.Authentication.Digest;
using Cuemon.AspNetCore.Authentication.Hmac;
using Cuemon.AspNetCore.Http;
using Cuemon.Collections.Generic;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Text.Json.Formatters;
using Cuemon.Extensions.AspNetCore.Xml.Formatters;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Authentication;
public class AuthorizationResponseHandlerTest : Test
{
    public AuthorizationResponseHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(FaultSensitivityDetails.All)]
    [InlineData(FaultSensitivityDetails.None)]
    public async Task AuthorizationResponseHandler_BasicScheme_ShouldRenderResponseUsingDefaultPlainTextFallback_UsingAspNetBootstrapping(FaultSensitivityDetails sensitivityDetails)
    {
        using (var startup = WebHostTestFactory.Create(services =>
               {
                   services.AddAuthorizationResponseHandler();
                   services.AddAuthentication(BasicAuthorizationHeader.Scheme)
                       .AddBasic(o =>
                       {
                           o.RequireSecureConnection = false;
                           o.Authenticator = (username, password) => null;
                       });
                   services.AddAuthorization(o =>
                   {
                       o.FallbackPolicy = new AuthorizationPolicyBuilder()
                           .AddAuthenticationSchemes(BasicAuthorizationHeader.Scheme)
                           .RequireAuthenticatedUser()
                           .Build();

                   });
                   services.AddRouting();
                   services.PostConfigureAllExceptionDescriptorOptions(o => o.SensitivityDetails = sensitivityDetails);
               }, app =>
               {
                   app.UseRouting();
                   app.UseAuthentication();
                   app.UseAuthorization();
                   app.UseEndpoints(endpoints =>
                   {
                       endpoints.MapGet("/", context => context.Response.WriteAsync($"Hello {context.User.Identity!.Name}"));
                   });
               }))
        {
            var client = startup.Host.GetTestClient();
            var bb = new BasicAuthorizationHeaderBuilder()
                .AddUserName("Agent")
                .AddPassword("Test");

            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, bb.Build().ToString());
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "text/plain");

            var result = await client.GetAsync("/");
            var content = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(content);

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Basic realm=\"AuthenticationServer\"", result.Headers.WwwAuthenticate.ToString());
            if (sensitivityDetails == FaultSensitivityDetails.All)
            {
                Assert.Equal("""
                                 Cuemon.AspNetCore.Http.UnauthorizedException: The request has not been applied because it lacks valid authentication credentials for the target resource.
                                  ---> System.Security.SecurityException: Unable to authenticate Agent.
                                    --- End of inner exception stack trace ---
                                 
                                 Additional Information:
                                 	Headers: Microsoft.AspNetCore.Http.HeaderDictionary
                                 	StatusCode: 401
                                 	ReasonPhrase: Unauthorized
                                 
                                 """.ReplaceLineEndings(), content.ReplaceLineEndings());
            }
            else
            {
                Assert.Equal("The request has not been applied because it lacks valid authentication credentials for the target resource.", content);
            }
        }
    }

    [Theory]
    [InlineData(FaultSensitivityDetails.All)]
    [InlineData(FaultSensitivityDetails.None)]
    public async Task AuthorizationResponseHandler_BasicScheme_ShouldRenderResponseUsingDefaultPlainTextFallbackForAuthorization_UsingAspNetBootstrapping(FaultSensitivityDetails sensitivityDetails)
    {
        using (var startup = WebHostTestFactory.Create(services =>
               {
                   services.AddAuthorizationResponseHandler();
                   services.AddAuthentication(BasicAuthorizationHeader.Scheme)
                       .AddBasic(o =>
                       {
                           o.RequireSecureConnection = false;
                           o.Authenticator = (username, password) =>
                           {
                               if (username == "Agent" && password == "Test")
                               {
                                   return new ClaimsPrincipal(new ClaimsIdentity(Arguments.Yield(new Claim(ClaimTypes.Name, "Test Agent")), BasicAuthorizationHeader.Scheme));
                               }
                               return null;
                           };
                       });
                   services.AddAuthorization(o =>
                   {
                       o.FallbackPolicy = new AuthorizationPolicyBuilder()
                           .AddAuthenticationSchemes(BasicAuthorizationHeader.Scheme)
                           .RequireAuthenticatedUser()
                           .RequireUserName("Skywalker")
                           .Build();

                   });
                   services.AddRouting();
                   services.PostConfigureAllExceptionDescriptorOptions(o => o.SensitivityDetails = sensitivityDetails);
               }, app =>
               {
                   app.UseRouting();
                   app.UseAuthentication();
                   app.UseAuthorization();
                   app.UseEndpoints(endpoints =>
                   {
                       endpoints.MapGet("/", context => context.Response.WriteAsync($"Hello {context.User.Identity!.Name}"));
                   });
               }))
        {
            var client = startup.Host.GetTestClient();
            var bb = new BasicAuthorizationHeaderBuilder()
                .AddUserName("Agent")
                .AddPassword("Test");

            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, bb.Build().ToString());
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "text/plain");

            var result = await client.GetAsync("/");
            var content = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(content);

            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);

            if (sensitivityDetails == FaultSensitivityDetails.All)
            {
                Assert.Equal("""
                                 Cuemon.AspNetCore.Http.ForbiddenException: NameAuthorizationRequirement:Requires a user identity with Name equal to Skywalker
                                 
                                 Additional Information:
                                 	Headers: Microsoft.AspNetCore.Http.HeaderDictionary
                                 	StatusCode: 403
                                 	ReasonPhrase: Forbidden
                                 
                                 """.ReplaceLineEndings(), content.ReplaceLineEndings());
            }
            else
            {
                Assert.Equal("NameAuthorizationRequirement:Requires a user identity with Name equal to Skywalker", content);
            }
        }
    }

    [Theory]
    [InlineData(FaultSensitivityDetails.All)]
    [InlineData(FaultSensitivityDetails.None)]
    public async Task AuthorizationResponseHandler_BasicScheme_ShouldRenderResponseUsingDefaultPlainTextFallbackForAuthorization_HideReason_UsingAspNetBootstrapping(FaultSensitivityDetails sensitivityDetails)
    {
        using (var startup = WebHostTestFactory.Create(services =>
               {
                   services.AddAuthorizationResponseHandler(o => o.AuthorizationFailureHandler = failure => new NotFoundException());
                   services.AddAuthentication(BasicAuthorizationHeader.Scheme)
                       .AddBasic(o =>
                       {
                           o.RequireSecureConnection = false;
                           o.Authenticator = (username, password) =>
                           {
                               if (username == "Agent" && password == "Test")
                               {
                                   return new ClaimsPrincipal(new ClaimsIdentity(Arguments.Yield(new Claim(ClaimTypes.Name, "Test Agent")), BasicAuthorizationHeader.Scheme));
                               }
                               return null;
                           };
                       });
                   services.AddAuthorization(o =>
                   {
                       o.FallbackPolicy = new AuthorizationPolicyBuilder()
                           .AddAuthenticationSchemes(BasicAuthorizationHeader.Scheme)
                           .RequireAuthenticatedUser()
                           .RequireUserName("Skywalker")
                           .Build();

                   });
                   services.AddRouting();
                   services.PostConfigureAllExceptionDescriptorOptions(o => o.SensitivityDetails = sensitivityDetails);
               }, app =>
               {
                   app.UseRouting();
                   app.UseAuthentication();
                   app.UseAuthorization();
                   app.UseEndpoints(endpoints =>
                   {
                       endpoints.MapGet("/", context => context.Response.WriteAsync($"Hello {context.User.Identity!.Name}"));
                   });
               }))
        {
            var client = startup.Host.GetTestClient();
            var bb = new BasicAuthorizationHeaderBuilder()
                .AddUserName("Agent")
                .AddPassword("Test");

            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, bb.Build().ToString());
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "text/plain");

            var result = await client.GetAsync("/");
            var content = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(content);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);

            if (sensitivityDetails == FaultSensitivityDetails.All)
            {
                Assert.Equal("""
                                 Cuemon.AspNetCore.Http.NotFoundException: The server has not found anything matching the request URI.
                                 
                                 Additional Information:
                                 	Headers: Microsoft.AspNetCore.Http.HeaderDictionary
                                 	StatusCode: 404
                                 	ReasonPhrase: Not Found
                                 
                                 """.ReplaceLineEndings(), content.ReplaceLineEndings());
            }
            else
            {
                Assert.Equal("The server has not found anything matching the request URI.", content);
            }
        }
    }

    [Theory]
    [InlineData(FaultSensitivityDetails.All)]
    [InlineData(FaultSensitivityDetails.None)]
    public async Task AuthorizationResponseHandler_BasicScheme_ShouldRenderResponseInXml_UsingAspNetBootstrapping(FaultSensitivityDetails sensitivityDetails)
    {
        using (var startup = WebHostTestFactory.Create(services =>
               {
                   services.AddXmlExceptionResponseFormatter(o => o.Settings.Writer.Indent = true);
                   services.AddAuthorizationResponseHandler();
                   services.AddAuthentication(BasicAuthorizationHeader.Scheme)
                       .AddBasic(o =>
                       {
                           o.RequireSecureConnection = false;
                           o.Authenticator = (username, password) => null;
                       });
                   services.AddAuthorization(o =>
                   {
                       o.FallbackPolicy = new AuthorizationPolicyBuilder()
                           .AddAuthenticationSchemes(BasicAuthorizationHeader.Scheme)
                           .RequireAuthenticatedUser()
                           .Build();

                   });
                   services.AddRouting();
                   services.PostConfigureAllExceptionDescriptorOptions(o => o.SensitivityDetails = sensitivityDetails);
               }, app =>
               {
                   app.UseRouting();
                   app.UseAuthentication();
                   app.UseAuthorization();
                   app.UseEndpoints(endpoints =>
                   {
                       endpoints.MapGet("/", context => context.Response.WriteAsync($"Hello {context.User.Identity!.Name}"));
                   });
               }))
        {
            var client = startup.Host.GetTestClient();
            var bb = new BasicAuthorizationHeaderBuilder()
                .AddUserName("Agent")
                .AddPassword("Test");

            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, bb.Build().ToString());
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/xml");

            var result = await client.GetAsync("/");
            var content = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(content);

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Basic realm=\"AuthenticationServer\"", result.Headers.WwwAuthenticate.ToString());
            if (sensitivityDetails == FaultSensitivityDetails.All)
            {
                Assert.Equal("""
                                 <?xml version="1.0" encoding="utf-8"?>
                                 <HttpExceptionDescriptor>
                                 	<Error>
                                 		<Status>401</Status>
                                 		<Code>Unauthorized</Code>
                                 		<Message>The request has not been applied because it lacks valid authentication credentials for the target resource.</Message>
                                 		<Failure>
                                 			<UnauthorizedException namespace="Cuemon.AspNetCore.Http">
                                 				<Message>The request has not been applied because it lacks valid authentication credentials for the target resource.</Message>
                                 				<StatusCode>401</StatusCode>
                                 				<ReasonPhrase>Unauthorized</ReasonPhrase>
                                 				<SecurityException namespace="System.Security">
                                 					<Message>Unable to authenticate Agent.</Message>
                                 				</SecurityException>
                                 			</UnauthorizedException>
                                 		</Failure>
                                 	</Error>
                                 </HttpExceptionDescriptor>
                                 """.ReplaceLineEndings(), content.ReplaceLineEndings());
            }
            else
            {
                Assert.Equal("""
                                 <?xml version="1.0" encoding="utf-8"?>
                                 <HttpExceptionDescriptor>
                                 	<Error>
                                 		<Status>401</Status>
                                 		<Code>Unauthorized</Code>
                                 		<Message>The request has not been applied because it lacks valid authentication credentials for the target resource.</Message>
                                 	</Error>
                                 </HttpExceptionDescriptor>
                                 """.ReplaceLineEndings(), content.ReplaceLineEndings());
            }
        }
    }

    [Theory]
    [InlineData(FaultSensitivityDetails.All)]
    [InlineData(FaultSensitivityDetails.None)]
    public async Task AuthorizationResponseHandler_BasicScheme_ShouldRenderResponseInJsonNative_UsingAspNetBootstrapping(FaultSensitivityDetails sensitivityDetails)
    {
        using (var startup = WebHostTestFactory.Create(services =>
               {
                   services.AddFaultDescriptorOptions();
                   services.AddJsonExceptionResponseFormatter();
                   services.AddAuthorizationResponseHandler();
                   services.AddAuthentication(BasicAuthorizationHeader.Scheme)
                       .AddBasic(o =>
                       {
                           o.RequireSecureConnection = false;
                           o.Authenticator = (username, password) => null;
                       });
                   services.AddAuthorization(o =>
                   {
                       o.FallbackPolicy = new AuthorizationPolicyBuilder()
                           .AddAuthenticationSchemes(BasicAuthorizationHeader.Scheme)
                           .RequireAuthenticatedUser()
                           .Build();

                   });
                   services.AddRouting();
                   services.PostConfigureAllExceptionDescriptorOptions(o => o.SensitivityDetails = sensitivityDetails);
               }, app =>
               {
                   app.UseFaultDescriptorExceptionHandler();
                   app.UseRouting();
                   app.UseAuthentication();
                   app.UseAuthorization();
                   app.UseEndpoints(endpoints =>
                   {
                       endpoints.MapGet("/", context => context.Response.WriteAsync($"Hello {context.User.Identity!.Name}"));
                   });
               }))
        {
            var client = startup.Host.GetTestClient();
            var bb = new BasicAuthorizationHeaderBuilder()
                .AddUserName("Agent")
                .AddPassword("Test");

            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, bb.Build().ToString());
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");

            var result = await client.GetAsync("/");
            var content = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(content);

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Basic realm=\"AuthenticationServer\"", result.Headers.WwwAuthenticate.ToString());
            if (sensitivityDetails == FaultSensitivityDetails.All)
            {
                Assert.Equal("""
                                 {
                                   "error": {
                                     "status": 401,
                                     "code": "Unauthorized",
                                     "message": "The request has not been applied because it lacks valid authentication credentials for the target resource.",
                                     "failure": {
                                       "type": "Cuemon.AspNetCore.Http.UnauthorizedException",
                                       "message": "The request has not been applied because it lacks valid authentication credentials for the target resource.",
                                       "headers": {},
                                       "statusCode": 401,
                                       "reasonPhrase": "Unauthorized",
                                       "inner": {
                                         "type": "System.Security.SecurityException",
                                         "message": "Unable to authenticate Agent."
                                       }
                                     }
                                   }
                                 }
                                 """.ReplaceLineEndings(), content.ReplaceLineEndings());
            }
            else
            {
                Assert.Equal("""
                                 {
                                   "error": {
                                     "status": 401,
                                     "code": "Unauthorized",
                                     "message": "The request has not been applied because it lacks valid authentication credentials for the target resource."
                                   }
                                 }
                                 """.ReplaceLineEndings(), content.ReplaceLineEndings());
            }
        }
    }

    [Fact]
    public async Task AuthorizationResponseHandler_BasicScheme_ShouldAuthorizeWithTestAgent_UsingAspNetBootstrapping()
    {
        using (var startup = WebHostTestFactory.Create(services =>
               {
                   services.AddAuthentication(BasicAuthorizationHeader.Scheme)
                       .AddBasic(o =>
                       {
                           o.RequireSecureConnection = false;
                           o.Authenticator = (username, password) =>
                           {
                               if (username == "Agent" && password == "Test")
                               {
                                   return new ClaimsPrincipal(new ClaimsIdentity(Arguments.Yield(new Claim(ClaimTypes.Name, "Test Agent")), BasicAuthorizationHeader.Scheme));
                               }
                               return null;
                           };
                       });
                   services.AddAuthorization(o =>
                   {
                       o.FallbackPolicy = new AuthorizationPolicyBuilder()
                           .AddAuthenticationSchemes(BasicAuthorizationHeader.Scheme)
                           .RequireAuthenticatedUser()
                           .RequireUserName("Test Agent")
                           .Build();

                   });
                   services.AddRouting();
               }, app =>
               {
                   app.UseRouting();
                   app.UseAuthentication();
                   app.UseAuthorization();
                   app.UseEndpoints(endpoints =>
                   {
                       endpoints.MapGet("/", context => context.Response.WriteAsync($"Hello {context.User.Identity!.Name}"));
                   });
               }))
        {
            var client = startup.Host.GetTestClient();
            var bb = new BasicAuthorizationHeaderBuilder()
                .AddUserName("Agent")
                .AddPassword("Test");

            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, bb.Build().ToString());
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "text/plain");

            var result = await client.GetAsync("/");
            var content = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(content);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Hello Test Agent", content);
        }
    }

    [Fact]
    public async Task AuthorizationResponseHandler_DigestScheme_ShouldAuthorizeWithTestAgent_UsingAspNetBootstrapping()
    {
        await using var startup = WebHostTestFactory.Create(services =>
        {
            services.AddAuthentication(DigestAuthorizationHeader.Scheme)
                .AddDigestAccess(o =>
                {
                    o.RequireSecureConnection = false;
                    o.Authenticator = (string username, out string password) =>
                    {
                        if (username == "Agent")
                        {
                            password = "Test";
                            return new ClaimsPrincipal(new ClaimsIdentity(Arguments.Yield(new Claim(ClaimTypes.Name, "Test Agent")), DigestAuthorizationHeader.Scheme));
                        }
                        password = null;
                        return null;
                    };
                });
            services.AddAuthorization(o =>
            {
                o.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(DigestAuthorizationHeader.Scheme)
                    .RequireAuthenticatedUser()
                    .Build();

            });
            services.AddRouting();
        }, app =>
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context => context.Response.WriteAsync($"Hello {context.User.Identity!.Name}"));
            });
        });

        var client = startup.Host.GetTestClient();
        var options = startup.Host.Services.GetRequiredScopedService<IOptionsSnapshot<DigestAuthenticationOptions>>().Get(DigestAuthorizationHeader.Scheme);

        client.DefaultRequestHeaders.Add(HeaderNames.Accept, "text/plain");

        var result = await client.GetAsync("/");

        var db = new DigestAuthorizationHeaderBuilder(options.DigestAlgorithm)
            .AddRealm(options.Realm)
            .AddUserName("Agent")
            .AddUri("/")
            .AddNc(1)
            .AddCnonce()
            .AddQopAuthentication()
            .AddFromWwwAuthenticateHeader(result.Headers);

        var ha1 = db.ComputeHash1("Test");
        var ha2 = db.ComputeHash2("GET");

        db.ComputeResponse(ha1, ha2);
        db.AddResponse("Test", "GET");

        var token = db.Build().ToString();

        client.DefaultRequestHeaders.Add(HeaderNames.Accept, "text/plain");
        client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);

        result = await client.GetAsync("/");
        var content = await result.Content.ReadAsStringAsync();

        TestOutput.WriteLine(content);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal("Hello Test Agent", content);
    }

            [Fact]
    public async Task AuthorizationResponseHandler_DigestScheme_ShouldAuthorizeWithTestAgent_UsingMinimalStyleAspNetBootstrapping()
    {
        await using var startup = MinimalWebHostTestFactory.Create(services =>
        {
            services.AddAuthentication(DigestAuthorizationHeader.Scheme)
                .AddDigestAccess(o =>
                {
                    o.RequireSecureConnection = false;
                    o.Authenticator = (string username, out string password) =>
                    {
                        if (username == "Agent")
                        {
                            password = "Test";
                            return new ClaimsPrincipal(new ClaimsIdentity(Arguments.Yield(new Claim(ClaimTypes.Name, "Test Agent")), DigestAuthorizationHeader.Scheme));
                        }
                        password = null;
                        return null;
                    };
                    o.DigestAlgorithm = DigestCryptoAlgorithm.Sha512Slash256;
                });
            services.AddAuthorization(o =>
            {
                o.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(DigestAuthorizationHeader.Scheme)
                    .RequireAuthenticatedUser()
                    .Build();

            });
            services.AddRouting();
        }, app =>
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context => context.Response.WriteAsync($"Hello {context.User.Identity!.Name}"));
            });
        });

        var client = startup.Host.GetTestClient();
        var options = startup.Host.Services.GetRequiredScopedService<IOptionsSnapshot<DigestAuthenticationOptions>>().Get(DigestAuthorizationHeader.Scheme);

        client.DefaultRequestHeaders.Add(HeaderNames.Accept, "text/plain");

        var result = await client.GetAsync("/");

        var db = new DigestAuthorizationHeaderBuilder(options.DigestAlgorithm)
            .AddRealm(options.Realm)
            .AddUserName("Agent")
            .AddUri("/")
            .AddNc(1)
            .AddCnonce()
            .AddQopAuthentication()
            .AddFromWwwAuthenticateHeader(result.Headers);

        var ha1 = db.ComputeHash1("Test");
        var ha2 = db.ComputeHash2("GET");

        db.ComputeResponse(ha1, ha2);
        db.AddResponse("Test", "GET");

        var token = db.Build().ToString();

        client.DefaultRequestHeaders.Add(HeaderNames.Accept, "text/plain");
        client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);

        result = await client.GetAsync("/");
        var content = await result.Content.ReadAsStringAsync();

        TestOutput.WriteLine(content);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal("Hello Test Agent", content);
    }

    [Theory]
    [InlineData(FaultSensitivityDetails.All)]
    [InlineData(FaultSensitivityDetails.None)]
    public async Task AuthorizationResponseHandler_DigestScheme_ShouldRenderResponseInJsonNative_UsingAspNetBootstrapping(FaultSensitivityDetails sensitivityDetails)
    {
        await using var startup = WebHostTestFactory.Create(services =>
        {
            services.AddJsonExceptionResponseFormatter();
            services.AddAuthorizationResponseHandler();
            services.AddAuthentication(DigestAuthorizationHeader.Scheme)
                .AddDigestAccess(o =>
                {
                    o.RequireSecureConnection = false;
                    o.Authenticator = (string username, out string password) =>
                    {
                        password = null;
                        return null;
                    };
                    o.NonceGenerator = (timestamp, entityTag, privateKey) => "MjAyNC0wMi0wMyAyMTo1NjoyMVo6MDlhZTFhZDIyZGE4ZGExYTAxMmVkMzMwZWJlMzVkOTNlOGNmYTFmN2FiMzU5YzY0YTUwODFjZThkYjM1NzIwZA==";
                    o.OpaqueGenerator = () => "dd1867244f862b1f858784a9b276d609";
                    o.NonceExpiredParser = (nonce, timeToLive) => false;
                });
            services.AddAuthorization(o =>
            {
                o.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(DigestAuthorizationHeader.Scheme)
                    .RequireAuthenticatedUser()
                    .Build();

            });
            services.AddRouting();
            services.PostConfigureAllExceptionDescriptorOptions(o => o.SensitivityDetails = sensitivityDetails);
        }, app =>
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context => context.Response.WriteAsync($"Hello {context.User.Identity!.Name}"));
            });
        });

        var client = startup.Host.GetTestClient();

        client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");

        var result = await client.GetAsync("/");
        var options = startup.Host.Services.GetRequiredScopedService<IOptionsSnapshot<DigestAuthenticationOptions>>().Get(DigestAuthorizationHeader.Scheme);

        var db = new DigestAuthorizationHeaderBuilder(options.DigestAlgorithm)
            .AddRealm(options.Realm)
            .AddUserName("Agent")
            .AddUri("/")
            .AddNc(1)
            .AddCnonce("Wt8oGT4OTmExU4DVU4ibzVZsotIYpild")
            .AddQopAuthentication()
            .AddFromWwwAuthenticateHeader(result.Headers);

        var ha1 = db.ComputeHash1("Test");
        var ha2 = db.ComputeHash2("GET");

        db.ComputeResponse(ha1, ha2);
        db.AddResponse("Test", "GET");

        var token = db.Build().ToString();

        client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);

        result = await client.GetAsync("/");

        var content = await result.Content.ReadAsStringAsync();

        TestOutput.WriteLine(content);

        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        Assert.Equal("Digest realm=\"AuthenticationServer\", qop=\"auth, auth-int\", nonce=\"MjAyNC0wMi0wMyAyMTo1NjoyMVo6MDlhZTFhZDIyZGE4ZGExYTAxMmVkMzMwZWJlMzVkOTNlOGNmYTFmN2FiMzU5YzY0YTUwODFjZThkYjM1NzIwZA==\", opaque=\"dd1867244f862b1f858784a9b276d609\", stale=false, algorithm=SHA-256", result.Headers.WwwAuthenticate.ToString());
        if (sensitivityDetails == FaultSensitivityDetails.All)
        {
            Assert.Equal("""
                             {
                               "error": {
                                 "status": 401,
                                 "code": "Unauthorized",
                                 "message": "The request has not been applied because it lacks valid authentication credentials for the target resource.",
                                 "failure": {
                                   "type": "Cuemon.AspNetCore.Http.UnauthorizedException",
                                   "message": "The request has not been applied because it lacks valid authentication credentials for the target resource.",
                                   "headers": {},
                                   "statusCode": 401,
                                   "reasonPhrase": "Unauthorized",
                                   "inner": {
                                     "type": "System.Security.SecurityException",
                                     "message": "Unable to authenticate Agent."
                                   }
                                 }
                               }
                             }
                             """.ReplaceLineEndings(), content.ReplaceLineEndings());
        }
        else
        {
            Assert.Equal("""
                             {
                               "error": {
                                 "status": 401,
                                 "code": "Unauthorized",
                                 "message": "The request has not been applied because it lacks valid authentication credentials for the target resource."
                               }
                             }
                             """.ReplaceLineEndings(), content.ReplaceLineEndings());
        }
    }

    [Fact]
    public async Task AuthorizationResponseHandler_HmacScheme_ShouldAuthorizeWithTestAgent_UsingAspNetBootstrapping()
    {
        using (var startup = WebHostTestFactory.Create(services =>
               {
                   services.AddAuthentication(HmacFields.Scheme)
                       .AddHmac(o =>
                       {
                           o.RequireSecureConnection = false;
                           o.Authenticator = (string clientId, out string clientSecret) =>
                           {
                               if (clientId == "Agent")
                               {
                                   clientSecret = "Test";
                                   return new ClaimsPrincipal(new ClaimsIdentity(Arguments.Yield(new Claim(ClaimTypes.Name, "Test Agent")), HmacFields.Scheme));
                               }
                               clientSecret = null;
                               return null;
                           };
                       });
                   services.AddAuthorization(o =>
                   {
                       o.FallbackPolicy = new AuthorizationPolicyBuilder()
                           .AddAuthenticationSchemes(HmacFields.Scheme)
                           .RequireAuthenticatedUser()
                           .Build();

                   });
                   services.AddRouting();
               }, app =>
               {
                   app.UseRouting();
                   app.UseAuthentication();
                   app.UseAuthorization();
                   app.UseEndpoints(endpoints =>
                   {
                       endpoints.MapGet("/", context => context.Response.WriteAsync($"Hello {context.User.Identity!.Name}"));
                   });
               }))
        {
            var client = startup.Host.GetTestClient();

            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "text/plain");
            client.DefaultRequestHeaders.Add(HeaderNames.Date, DateTime.UtcNow.ToString("R"));
            client.DefaultRequestHeaders.Add(HeaderNames.Host, "www.cuemon.net");
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "text/plain");

            var result = await client.GetAsync("/");

            var hb = new HmacAuthorizationHeaderBuilder(HmacFields.Scheme)
                .AddFromRequest(result.RequestMessage)
                .AddClientId("Agent")
                .AddClientSecret("Test")
                .AddCredentialScope("20150830/us-east-1/iam/aws4_request");

            var token = hb.Build().ToString();

            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "text/plain");
            client.DefaultRequestHeaders.TryAddWithoutValidation(HeaderNames.Authorization, token);

            result = await client.GetAsync("/");
            var content = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(content);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Hello Test Agent", content);
        }
    }

    [Theory]
    [InlineData(FaultSensitivityDetails.All)]
    [InlineData(FaultSensitivityDetails.None)]
    public async Task AuthorizationResponseHandler_HmacScheme_ShouldRenderResponseInJsonNative_UsingAspNetBootstrapping(FaultSensitivityDetails sensitivityDetails)
    {
        using (var startup = WebHostTestFactory.Create(services =>
               {
                   services.AddJsonExceptionResponseFormatter();
                   services.AddAuthorizationResponseHandler();
                   services.AddAuthentication(HmacFields.Scheme)
                       .AddHmac(o =>
                       {
                           o.RequireSecureConnection = false;
                           o.Authenticator = (string clientId, out string clientSecret) =>
                           {
                               clientSecret = null;
                               return null;
                           };
                       });
                   services.AddAuthorization(o =>
                   {
                       o.FallbackPolicy = new AuthorizationPolicyBuilder()
                           .AddAuthenticationSchemes(HmacFields.Scheme)
                           .RequireAuthenticatedUser()
                           .Build();

                   });
                   services.AddRouting();
                   services.PostConfigureAllExceptionDescriptorOptions(o => o.SensitivityDetails = sensitivityDetails);
               }, app =>
               {
                   app.UseRouting();
                   app.UseAuthentication();
                   app.UseAuthorization();
                   app.UseEndpoints(endpoints =>
                   {
                       endpoints.MapGet("/", context => context.Response.WriteAsync($"Hello {context.User.Identity!.Name}"));
                   });
               }))
        {
            var client = startup.Host.GetTestClient();

            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            client.DefaultRequestHeaders.Add(HeaderNames.Date, DateTime.UtcNow.ToString("R"));
            client.DefaultRequestHeaders.Add(HeaderNames.Host, "www.cuemon.net");
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "text/plain");

            var result = await client.GetAsync("/");

            var hb = new HmacAuthorizationHeaderBuilder(HmacFields.Scheme)
                .AddFromRequest(result.RequestMessage)
                .AddClientId("Agent")
                .AddClientSecret("Test")
                .AddCredentialScope("20150830/us-east-1/iam/aws4_request");

            var token = hb.Build().ToString();

            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            client.DefaultRequestHeaders.TryAddWithoutValidation(HeaderNames.Authorization, token);

            result = await client.GetAsync("/");

            var content = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(content);

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("HMAC", result.Headers.WwwAuthenticate.ToString());
            if (sensitivityDetails == FaultSensitivityDetails.All)
            {
                Assert.Equal("""
                                 {
                                   "error": {
                                     "status": 401,
                                     "code": "Unauthorized",
                                     "message": "The request has not been applied because it lacks valid authentication credentials for the target resource.",
                                     "failure": {
                                       "type": "Cuemon.AspNetCore.Http.UnauthorizedException",
                                       "message": "The request has not been applied because it lacks valid authentication credentials for the target resource.",
                                       "headers": {},
                                       "statusCode": 401,
                                       "reasonPhrase": "Unauthorized",
                                       "inner": {
                                         "type": "System.Security.SecurityException",
                                         "message": "Unable to authenticate Agent."
                                       }
                                     }
                                   }
                                 }
                                 """.ReplaceLineEndings(), content.ReplaceLineEndings());
            }
            else
            {
                Assert.Equal("""
                                 {
                                   "error": {
                                     "status": 401,
                                     "code": "Unauthorized",
                                     "message": "The request has not been applied because it lacks valid authentication credentials for the target resource."
                                   }
                                 }
                                 """.ReplaceLineEndings(), content.ReplaceLineEndings());
            }
        }
    }

    [Fact]
    public async Task AuthorizationResponseHandler_BasicScheme_VerifyAsyncOptions_ShouldLogThrowTaskCanceledException_FromAuthorizationResponseHandler()
    {
        using (var startup = WebHostTestFactory.Create(services =>
               {
                   services.AddXunitTestLogging(TestOutput, LogLevel.Error);
                   services.AddAuthorizationResponseHandler(o =>
                   {
                       o.CancellationToken = new CancellationTokenSource(TimeSpan.FromMilliseconds(100)).Token;
                   });
                   services.AddAuthentication(BasicAuthorizationHeader.Scheme)
                       .AddBasic(o =>
                       {
                           o.RequireSecureConnection = false;
                           o.Authenticator = (username, password) =>
                           {
                               Thread.Sleep(25);
                               return null;
                           };
                       });
                   services.AddAuthorization(o =>
                   {
                       o.FallbackPolicy = new AuthorizationPolicyBuilder()
                           .AddAuthenticationSchemes(BasicAuthorizationHeader.Scheme)
                           .RequireAuthenticatedUser()
                           .Build();

                   });
                   services.AddRouting();
               }, app =>
               {
                   app.UseRouting();
                   app.UseAuthentication();
                   app.UseAuthorization();
                   app.UseEndpoints(endpoints =>
                   {
                       endpoints.MapGet("/", context => context.Response.WriteAsync($"Hello {context.User.Identity!.Name}"));
                   });
               }))
        {
            var client = startup.Host.GetTestClient();
            var loggerStore = startup.Host.Services.GetRequiredService<ILogger<AuthorizationResponseHandler>>().GetTestStore();

            var bb = new BasicAuthorizationHeaderBuilder()
                .AddUserName("Agent")
                .AddPassword("Test");

            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, bb.Build().ToString());
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "text/plain");

            for (var i = 0; i < 12; i++)
            {
                var result = await client.GetAsync("/");
                var content = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(content);
                TestOutput.WriteLine(i.ToString());
            }

            Assert.InRange(loggerStore.Query(entry => entry.Message.Contains("System.Threading.Tasks.TaskCanceledException: A task was canceled.")).Count(), 8, 12); // should be 10 - but high CPU can make this unstable
        }
    }

    [Fact]
    public async Task AuthorizationResponseHandler_BasicScheme_VerifyAsyncOptions_ShouldNotThrowTaskCanceledException_FromAuthorizationResponseHandler()
    {
        using (var startup = WebHostTestFactory.Create(services =>
               {
                   services.AddXunitTestLogging(TestOutput, LogLevel.Error);
                   services.AddAuthorizationResponseHandler(o =>
                   {
                       o.CancellationTokenProvider = () => new CancellationTokenSource(TimeSpan.FromMilliseconds(125)).Token;
                   });
                   services.AddAuthentication(BasicAuthorizationHeader.Scheme)
                       .AddBasic(o =>
                       {
                           o.RequireSecureConnection = false;
                           o.Authenticator = (username, password) =>
                           {
                               Thread.Sleep(25);
                               return null;
                           };
                       });
                   services.AddAuthorization(o =>
                   {
                       o.FallbackPolicy = new AuthorizationPolicyBuilder()
                           .AddAuthenticationSchemes(BasicAuthorizationHeader.Scheme)
                           .RequireAuthenticatedUser()
                           .Build();

                   });
                   services.AddRouting();
               }, app =>
               {
                   app.UseRouting();
                   app.UseAuthentication();
                   app.UseAuthorization();
                   app.UseEndpoints(endpoints =>
                   {
                       endpoints.MapGet("/", context => context.Response.WriteAsync($"Hello {context.User.Identity!.Name}"));
                   });
               }))
        {
            var client = startup.Host.GetTestClient();
            var loggerStore = startup.Host.Services.GetRequiredService<ILogger<AuthorizationResponseHandler>>().GetTestStore();

            var bb = new BasicAuthorizationHeaderBuilder()
                .AddUserName("Agent")
                .AddPassword("Test");

            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, bb.Build().ToString());
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "text/plain");

            for (var i = 0; i < 14; i++)
            {
                var result = await client.GetAsync("/");
                var content = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(content);
                TestOutput.WriteLine(i.ToString());
            }

            Assert.Equal(0, loggerStore.Query(entry => entry.Message.Contains("System.Threading.Tasks.TaskCanceledException: A task was canceled.")).Count());
        }
    }
}
