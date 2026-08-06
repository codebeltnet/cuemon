using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Text.Json.Formatters;
using Cuemon.Extensions.AspNetCore.Xml.Formatters;
using Cuemon.Extensions.DependencyInjection;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Xunit;

namespace Cuemon.AspNetCore.Diagnostics;
public class ApplicationBuilderExtensionsTest : Test
{
    public ApplicationBuilderExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task UseFaultDescriptorExceptionHandler_ShouldCaptureException_WhenRequestServicesIsWrappedByAspVersioningInjectApiVersion()
    {
        using var response = await WebHostTestFactory.RunAsync(
            services =>
            {
                services.AddFaultDescriptorOptions(o => o.FaultDescriptor = PreferredFaultDescriptor.ProblemDetails);
                services.AddJsonExceptionResponseFormatter();
            },
            app =>
            {
                app.UseFaultDescriptorExceptionHandler();
                app.Use((Func<Microsoft.AspNetCore.Http.HttpContext, Func<Task>, Task>)((context, _) =>
                {
                    context.RequestServices = new Asp.Versioning.Builder.EndpointBuilderFinalizer.InjectApiVersion(context.RequestServices);
                    throw new NotFoundException();
                }));
            },
            responseFactory: client =>
            {
                client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                return client.GetAsync("/");
            });

        var body = await response.Content.ReadAsStringAsync();

        TestOutput.WriteLine(body);

        Assert.Equal(404, (int)response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        Assert.EndsWith("Asp.Versioning.Builder.EndpointBuilderFinalizer+InjectApiVersion", typeof(Asp.Versioning.Builder.EndpointBuilderFinalizer.InjectApiVersion).FullName, StringComparison.Ordinal);
        Assert.Contains("\"title\": \"NotFound\"", body, StringComparison.Ordinal);
        Assert.Contains("\"status\": 404", body, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(FaultSensitivityDetails.All)]
    [InlineData(FaultSensitivityDetails.Evidence)]
    [InlineData(FaultSensitivityDetails.FailureWithStackTraceAndData)]
    [InlineData(FaultSensitivityDetails.FailureWithData)]
    [InlineData(FaultSensitivityDetails.FailureWithStackTrace)]
    [InlineData(FaultSensitivityDetails.Failure)]
    [InlineData(FaultSensitivityDetails.None)]
    public async Task UseFaultDescriptorExceptionHandler_ShouldCaptureException_RenderAsExceptionDescriptor_UsingJson(FaultSensitivityDetails sensitivity)
    {
        using var response = await WebHostTestFactory.RunAsync(
            services =>
            {
                services.AddFaultDescriptorOptions(o => o.FaultDescriptor = PreferredFaultDescriptor.FaultDetails);
                services.AddJsonExceptionResponseFormatter();
                services.PostConfigureAllOf<IExceptionDescriptorOptions>(o => o.SensitivityDetails = sensitivity);
            },
            app =>
            {
                app.UseFaultDescriptorExceptionHandler();
                app.Use(async (context, next) =>
                {
                    try
                    {
                        throw new ArgumentException("This is an inner exception message ...", nameof(app))
                        {
                            Data =
                            {
                                { "1st", "data value" }
                            },
                            HelpLink = "https://www.savvyio.net/"
                        };
                    }
                    catch (Exception e)
                    {
                        throw new NotFoundException("Main exception - look out for inner!", e);
                    }

                    await next(context);
                });
            },
            responseFactory: client =>
            {
                client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                return client.GetAsync("/");
            });

        var body = await response.Content.ReadAsStringAsync();

        TestOutput.WriteLine(body);

        switch (sensitivity)
        {
            case FaultSensitivityDetails.All:
                Assert.True(Match("""
                                      {
                                        "error": {
                                          "instance": "http://localhost/",
                                          "status": 404,
                                          "code": "NotFound",
                                          "message": "Main exception - look out for inner!",
                                          "failure": {
                                            "type": "Cuemon.AspNetCore.Http.NotFoundException",
                                            "source": "Cuemon.AspNetCore.FunctionalTests",
                                            "message": "Main exception - look out for inner!",
                                            "stack": [
                                              "at Cuemon.AspNetCore.Diagnostics.ApplicationBuilderExtensionsTest.<>c.<<UseFaultDescriptorExceptionHandler_ShouldCaptureException_RenderAsExceptionDescriptor_UsingJson*",
                                              "--- End of stack trace from previous location ---",
                                              "at Microsoft.AspNetCore.Diagnostics.ExceptionHandler*"
                                            ],
                                            "headers": {},
                                            "statusCode": 404,
                                            "reasonPhrase": "Not Found",
                                            "inner": {
                                              "type": "System.ArgumentException",
                                              "source": "Cuemon.AspNetCore.FunctionalTests",
                                              "message": "This is an inner exception message ... (Parameter 'app')",
                                              "stack": [
                                                "at Cuemon.AspNetCore.Diagnostics.ApplicationBuilderExtensionsTest.<>c.<<UseFaultDescriptorExceptionHandler_ShouldCaptureException_RenderAsExceptionDescriptor_UsingJson*"
                                              ],
                                              "data": {
                                                "1st": "data value"
                                              },
                                              "paramName": "app"
                                            }
                                          }
                                        },
                                        "evidence": {
                                          "request": {
                                            "location": "http://localhost/",
                                            "method": "GET",
                                            "headers": {
                                              "accept": "application/json",
                                              "host": "localhost"
                                            },
                                            "query": [],
                                            "cookies": [],
                                            "body": ""
                                          }
                                        },
                                        "traceId": "*"
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.Evidence:
                Assert.True(Match("""
                                      {
                                        "error": {
                                          "instance": "http://localhost/",
                                          "status": 404,
                                          "code": "NotFound",
                                          "message": "Main exception - look out for inner!"
                                        },
                                        "evidence": {
                                          "request": {
                                            "location": "http://localhost/",
                                            "method": "GET",
                                            "headers": {
                                              "accept": "application/json",
                                              "host": "localhost"
                                            },
                                            "query": [],
                                            "cookies": [],
                                            "body": ""
                                          }
                                        },
                                        "traceId": "*"
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.FailureWithStackTraceAndData:
                Assert.True(Match("""
                                      {
                                        "error": {
                                          "instance": "http://localhost/",
                                          "status": 404,
                                          "code": "NotFound",
                                          "message": "Main exception - look out for inner!",
                                          "failure": {
                                            "type": "Cuemon.AspNetCore.Http.NotFoundException",
                                            "source": "Cuemon.AspNetCore.FunctionalTests",
                                            "message": "Main exception - look out for inner!",
                                            "stack": [
                                              "at Cuemon.AspNetCore.Diagnostics.ApplicationBuilderExtensionsTest.<>c.<<UseFaultDescriptorExceptionHandler_ShouldCaptureException_RenderAsExceptionDescriptor_UsingJson*",
                                              "--- End of stack trace from previous location ---",
                                              "at Microsoft.AspNetCore.Diagnostics.ExceptionHandler*"
                                            ],
                                            "headers": {},
                                            "statusCode": 404,
                                            "reasonPhrase": "Not Found",
                                            "inner": {
                                              "type": "System.ArgumentException",
                                              "source": "Cuemon.AspNetCore.FunctionalTests",
                                              "message": "This is an inner exception message ... (Parameter 'app')",
                                              "stack": [
                                                "at Cuemon.AspNetCore.Diagnostics.ApplicationBuilderExtensionsTest.<>c.<<UseFaultDescriptorExceptionHandler_ShouldCaptureException_RenderAsExceptionDescriptor_UsingJson*"
                                              ],
                                              "data": {
                                                "1st": "data value"
                                              },
                                              "paramName": "app"
                                            }
                                          }
                                        },
                                        "traceId": "*"
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.FailureWithData:
                Assert.True(Match("""
                                      {
                                        "error": {
                                          "instance": "http://localhost/",
                                          "status": 404,
                                          "code": "NotFound",
                                          "message": "Main exception - look out for inner!",
                                          "failure": {
                                            "type": "Cuemon.AspNetCore.Http.NotFoundException",
                                            "source": "Cuemon.AspNetCore.FunctionalTests",
                                            "message": "Main exception - look out for inner!",
                                            "headers": {},
                                            "statusCode": 404,
                                            "reasonPhrase": "Not Found",
                                            "inner": {
                                              "type": "System.ArgumentException",
                                              "source": "Cuemon.AspNetCore.FunctionalTests",
                                              "message": "This is an inner exception message ... (Parameter 'app')",
                                              "data": {
                                                "1st": "data value"
                                              },
                                              "paramName": "app"
                                            }
                                          }
                                        },
                                        "traceId": "*"
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.FailureWithStackTrace:
                Assert.True(Match("""
                                      {
                                        "error": {
                                          "instance": "http://localhost/",
                                          "status": 404,
                                          "code": "NotFound",
                                          "message": "Main exception - look out for inner!",
                                          "failure": {
                                            "type": "Cuemon.AspNetCore.Http.NotFoundException",
                                            "source": "Cuemon.AspNetCore.FunctionalTests",
                                            "message": "Main exception - look out for inner!",
                                            "stack": [
                                              "at Cuemon.AspNetCore.Diagnostics.ApplicationBuilderExtensionsTest.<>c.<<UseFaultDescriptorExceptionHandler_ShouldCaptureException_RenderAsExceptionDescriptor_UsingJson*",
                                              "--- End of stack trace from previous location ---",
                                              "at Microsoft.AspNetCore.Diagnostics.ExceptionHandler*"
                                            ],
                                            "headers": {},
                                            "statusCode": 404,
                                            "reasonPhrase": "Not Found",
                                            "inner": {
                                              "type": "System.ArgumentException",
                                              "source": "Cuemon.AspNetCore.FunctionalTests",
                                              "message": "This is an inner exception message ... (Parameter 'app')",
                                              "stack": [
                                                "at Cuemon.AspNetCore.Diagnostics.ApplicationBuilderExtensionsTest.<>c.<<UseFaultDescriptorExceptionHandler_ShouldCaptureException_RenderAsExceptionDescriptor_UsingJson*"
                                              ],
                                              "paramName": "app"
                                            }
                                          }
                                        },
                                        "traceId": "*"
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.Failure:
                Assert.True(Match("""
                                      {
                                        "error": {
                                          "instance": "http://localhost/",
                                          "status": 404,
                                          "code": "NotFound",
                                          "message": "Main exception - look out for inner!",
                                          "failure": {
                                            "type": "Cuemon.AspNetCore.Http.NotFoundException",
                                            "source": "Cuemon.AspNetCore.FunctionalTests",
                                            "message": "Main exception - look out for inner!",
                                            "headers": {},
                                            "statusCode": 404,
                                            "reasonPhrase": "Not Found",
                                            "inner": {
                                              "type": "System.ArgumentException",
                                              "source": "Cuemon.AspNetCore.FunctionalTests",
                                              "message": "This is an inner exception message ... (Parameter 'app')",
                                              "paramName": "app"
                                            }
                                          }
                                        },
                                        "traceId": "*"
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.None:
                Assert.True(Match("""
                                      {
                                        "error": {
                                          "instance": "http://localhost/",
                                          "status": 404,
                                          "code": "NotFound",
                                          "message": "Main exception - look out for inner!"
                                        },
                                        "traceId": "*"
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
        }
    }

    [Theory]
    [InlineData(FaultSensitivityDetails.All)]
    [InlineData(FaultSensitivityDetails.Evidence)]
    [InlineData(FaultSensitivityDetails.FailureWithStackTraceAndData)]
    [InlineData(FaultSensitivityDetails.FailureWithData)]
    [InlineData(FaultSensitivityDetails.FailureWithStackTrace)]
    [InlineData(FaultSensitivityDetails.Failure)]
    [InlineData(FaultSensitivityDetails.None)]
    public async Task UseFaultDescriptorExceptionHandler_ShouldCaptureException_RenderAsProblemDetails_UsingJson(FaultSensitivityDetails sensitivity)
    {
        using var response = await WebHostTestFactory.RunAsync(
            services =>
            {
                services.AddFaultDescriptorOptions(o => o.FaultDescriptor = PreferredFaultDescriptor.ProblemDetails);
                services.AddJsonExceptionResponseFormatter();
                services.PostConfigureAllOf<IExceptionDescriptorOptions>(o => o.SensitivityDetails = sensitivity);
            },
            app =>
            {
                app.UseFaultDescriptorExceptionHandler();
                app.Use(async (context, next) =>
                {
                    try
                    {
                        throw new ArgumentException("This is an inner exception message ...", nameof(app))
                        {
                            Data =
                            {
                                { "1st", "data value" }
                            },
                            HelpLink = "https://www.savvyio.net/"
                        };
                    }
                    catch (Exception e)
                    {
                        throw new NotFoundException("Main exception - look out for inner!", e);
                    }

                    await next(context);
                });
            },
            responseFactory: client =>
            {
                client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                return client.GetAsync("/");
            });

        var body = await response.Content.ReadAsStringAsync();

        TestOutput.WriteLine(body);

        switch (sensitivity)
        {
            case FaultSensitivityDetails.All:
                Assert.True(Match("""
                                      {
                                        "type": "about:blank",
                                        "title": "NotFound",
                                        "status": 404,
                                        "detail": "Main exception - look out for inner!",
                                        "instance": "http://localhost/",
                                        "traceId": "*",
                                        "failure": {
                                          "type": "Cuemon.AspNetCore.Http.NotFoundException",
                                          "source": "Cuemon.AspNetCore.FunctionalTests",
                                          "message": "Main exception - look out for inner!",
                                          "stack": [
                                            "at Cuemon.AspNetCore.Diagnostics.ApplicationBuilderExtensionsTest.<>c.<<UseFaultDescriptorExceptionHandler_ShouldCaptureException_RenderAsProblemDetails_UsingJson*",
                                            "--- End of stack trace from previous location ---",
                                            "at Microsoft.AspNetCore.Diagnostics.ExceptionHandler*"
                                          ],
                                          "headers": {},
                                          "statusCode": 404,
                                          "reasonPhrase": "Not Found",
                                          "inner": {
                                            "type": "System.ArgumentException",
                                            "source": "Cuemon.AspNetCore.FunctionalTests",
                                            "message": "This is an inner exception message ... (Parameter 'app')",
                                            "stack": [
                                              "at Cuemon.AspNetCore.Diagnostics.ApplicationBuilderExtensionsTest.<>c.<<UseFaultDescriptorExceptionHandler_ShouldCaptureException_RenderAsProblemDetails_UsingJson*"
                                            ],
                                            "data": {
                                              "1st": "data value"
                                            },
                                            "paramName": "app"
                                          }
                                        },
                                        "request": {
                                          "location": "http://localhost/",
                                          "method": "GET",
                                          "headers": {
                                            "accept": "application/json",
                                            "host": "localhost"
                                          },
                                          "query": [],
                                          "cookies": [],
                                          "body": ""
                                        }
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.Evidence:
                Assert.True(Match("""
                                      {
                                        "type": "about:blank",
                                        "title": "NotFound",
                                        "status": 404,
                                        "detail": "Main exception - look out for inner!",
                                        "instance": "http://localhost/",
                                        "traceId": "*",
                                        "request": {
                                          "location": "http://localhost/",
                                          "method": "GET",
                                          "headers": {
                                            "accept": "application/json",
                                            "host": "localhost"
                                          },
                                          "query": [],
                                          "cookies": [],
                                          "body": ""
                                        }
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.FailureWithStackTraceAndData:
                Assert.True(Match("""
                                      {
                                        "type": "about:blank",
                                        "title": "NotFound",
                                        "status": 404,
                                        "detail": "Main exception - look out for inner!",
                                        "instance": "http://localhost/",
                                        "traceId": "*",
                                        "failure": {
                                          "type": "Cuemon.AspNetCore.Http.NotFoundException",
                                          "source": "Cuemon.AspNetCore.FunctionalTests",
                                          "message": "Main exception - look out for inner!",
                                          "stack": [
                                            "at Cuemon.AspNetCore.Diagnostics.ApplicationBuilderExtensionsTest.<>c.<<UseFaultDescriptorExceptionHandler_ShouldCaptureException_RenderAsProblemDetails_UsingJson*",
                                            "--- End of stack trace from previous location ---",
                                            "at Microsoft.AspNetCore.Diagnostics.ExceptionHandler*"
                                          ],
                                          "headers": {},
                                          "statusCode": 404,
                                          "reasonPhrase": "Not Found",
                                          "inner": {
                                            "type": "System.ArgumentException",
                                            "source": "Cuemon.AspNetCore.FunctionalTests",
                                            "message": "This is an inner exception message ... (Parameter 'app')",
                                            "stack": [
                                              "at Cuemon.AspNetCore.Diagnostics.ApplicationBuilderExtensionsTest.<>c.<<UseFaultDescriptorExceptionHandler_ShouldCaptureException_RenderAsProblemDetails_UsingJson*"
                                            ],
                                            "data": {
                                              "1st": "data value"
                                            },
                                            "paramName": "app"
                                          }
                                        }
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.FailureWithData:
                Assert.True(Match("""
                                      {
                                        "type": "about:blank",
                                        "title": "NotFound",
                                        "status": 404,
                                        "detail": "Main exception - look out for inner!",
                                        "instance": "http://localhost/",
                                        "traceId": "*",
                                        "failure": {
                                          "type": "Cuemon.AspNetCore.Http.NotFoundException",
                                          "source": "Cuemon.AspNetCore.FunctionalTests",
                                          "message": "Main exception - look out for inner!",
                                          "headers": {},
                                          "statusCode": 404,
                                          "reasonPhrase": "Not Found",
                                          "inner": {
                                            "type": "System.ArgumentException",
                                            "source": "Cuemon.AspNetCore.FunctionalTests",
                                            "message": "This is an inner exception message ... (Parameter 'app')",
                                            "data": {
                                              "1st": "data value"
                                            },
                                            "paramName": "app"
                                          }
                                        }
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.FailureWithStackTrace:
                Assert.True(Match("""
                                      {
                                        "type": "about:blank",
                                        "title": "NotFound",
                                        "status": 404,
                                        "detail": "Main exception - look out for inner!",
                                        "instance": "http://localhost/",
                                        "traceId": "*",
                                        "failure": {
                                          "type": "Cuemon.AspNetCore.Http.NotFoundException",
                                          "source": "Cuemon.AspNetCore.FunctionalTests",
                                          "message": "Main exception - look out for inner!",
                                          "stack": [
                                            "at Cuemon.AspNetCore.Diagnostics.ApplicationBuilderExtensionsTest.<>c.<<UseFaultDescriptorExceptionHandler_ShouldCaptureException_RenderAsProblemDetails_UsingJson*",
                                            "--- End of stack trace from previous location ---",
                                            "at Microsoft.AspNetCore.Diagnostics.ExceptionHandler*"
                                          ],
                                          "headers": {},
                                          "statusCode": 404,
                                          "reasonPhrase": "Not Found",
                                          "inner": {
                                            "type": "System.ArgumentException",
                                            "source": "Cuemon.AspNetCore.FunctionalTests",
                                            "message": "This is an inner exception message ... (Parameter 'app')",
                                            "stack": [
                                              "at Cuemon.AspNetCore.Diagnostics.ApplicationBuilderExtensionsTest.<>c.<<UseFaultDescriptorExceptionHandler_ShouldCaptureException_RenderAsProblemDetails_UsingJson*"
                                            ],
                                            "paramName": "app"
                                          }
                                        }
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.Failure:
                Assert.True(Match("""
                                      {
                                        "type": "about:blank",
                                        "title": "NotFound",
                                        "status": 404,
                                        "detail": "Main exception - look out for inner!",
                                        "instance": "http://localhost/",
                                        "traceId": "*",
                                        "failure": {
                                          "type": "Cuemon.AspNetCore.Http.NotFoundException",
                                          "source": "Cuemon.AspNetCore.FunctionalTests",
                                          "message": "Main exception - look out for inner!",
                                          "headers": {},
                                          "statusCode": 404,
                                          "reasonPhrase": "Not Found",
                                          "inner": {
                                            "type": "System.ArgumentException",
                                            "source": "Cuemon.AspNetCore.FunctionalTests",
                                            "message": "This is an inner exception message ... (Parameter 'app')",
                                            "paramName": "app"
                                          }
                                        }
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.None:
                Assert.True(Match("""
                                      {
                                        "type": "about:blank",
                                        "title": "NotFound",
                                        "status": 404,
                                        "detail": "Main exception - look out for inner!",
                                        "instance": "http://localhost/",
                                        "traceId": "*"
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
        }
    }

    [Fact]
    public async Task UseFaultDescriptorExceptionHandler_ShouldCaptureException_RenderAsExceptionDescriptor_UsingXml_WithSensitivityAll()
    {
        using var response = await WebHostTestFactory.RunAsync(
            services =>
            {
                services.AddFaultDescriptorOptions(o => o.FaultDescriptor = PreferredFaultDescriptor.FaultDetails);
                services.AddXmlExceptionResponseFormatter(o => o.Settings.Writer.Indent = true);
                services.PostConfigureAllOf<IExceptionDescriptorOptions>(o => o.SensitivityDetails = FaultSensitivityDetails.All);
            },
            app =>
            {
                app.UseFaultDescriptorExceptionHandler();
                app.Use(async (context, next) =>
                {
                    try
                    {
                        throw new ArgumentException("This is an inner exception message ...", nameof(app))
                        {
                            Data =
                            {
                                { "1st", "data value" }
                            },
                            HelpLink = "https://www.savvyio.net/"
                        };
                    }
                    catch (Exception e)
                    {
                        throw new NotFoundException("Main exception - look out for inner!", e);
                    }

                    await next(context);
                });
            },
            responseFactory: client =>
            {
                client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/xml"));
                return client.GetAsync("/");
            });

        var body = await response.Content.ReadAsStringAsync();

        TestOutput.WriteLine(body);

        Assert.True(Match("""
                              <?xml version="1.0" encoding="utf-8"?>
                              <HttpExceptionDescriptor>
                              	<Error>
                              		<Instance>http://localhost/</Instance>
                              		<Status>404</Status>
                              		<Code>NotFound</Code>
                              		<Message>Main exception - look out for inner!</Message>
                              		<Failure>
                              			<NotFoundException namespace="Cuemon.AspNetCore.Http">
                              				<Source>Cuemon.AspNetCore.FunctionalTests</Source>
                              				<Message>Main exception - look out for inner!</Message>
                              				<Stack>
                              					<Frame>at Cuemon.AspNetCore.Diagnostics.ApplicationBuilderExtensionsTest*</Frame>
                              					<Frame>--- End of stack trace from previous location ---</Frame>
                              					<Frame>at Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware*</Frame>
                              				</Stack>
                              				<StatusCode>404</StatusCode>
                              				<ReasonPhrase>Not Found</ReasonPhrase>
                              				<ArgumentException namespace="System">
                              					<Source>Cuemon.AspNetCore.FunctionalTests</Source>
                              					<Message>This is an inner exception message ... (Parameter 'app')</Message>
                              					<Stack>
                              						<Frame>at Cuemon.AspNetCore.Diagnostics.ApplicationBuilderExtensionsTest.&lt;&gt;c.&lt;&lt;UseFaultDescriptorExceptionHandler_ShouldCaptureException_RenderAsExceptionDescriptor_UsingXml_WithSensitivityAll&gt;*</Frame>
                              					</Stack>
                              					<Data>
                              						<st>data value</st>
                              					</Data>
                              					<ParamName>app</ParamName>
                              				</ArgumentException>
                              			</NotFoundException>
                              		</Failure>
                              	</Error>
                              	<Evidence>
                              		<Request>
                              			<Location>http://localhost/</Location>
                              			<Method>GET</Method>
                              			<Header name="Accept">application/xml</Header>
                              			<Header name="Host">localhost</Header>
                              		</Request>
                              	</Evidence>
                              	<TraceId>*</TraceId>
                              </HttpExceptionDescriptor>
                              """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
    }

    [Fact]
    public async Task UseFaultDescriptorExceptionHandler_ShouldCaptureException_RenderAsProblemDetails_UsingXml_WithSensitivityAll()
    {
        using var response = await WebHostTestFactory.RunAsync(
            services =>
            {
                services.AddFaultDescriptorOptions(o => o.FaultDescriptor = PreferredFaultDescriptor.ProblemDetails);
                services.AddXmlExceptionResponseFormatter(o => o.Settings.Writer.Indent = true);
                services.PostConfigureAllOf<IExceptionDescriptorOptions>(o => o.SensitivityDetails = FaultSensitivityDetails.All);
            },
            app =>
            {
                app.UseFaultDescriptorExceptionHandler();
                app.Use(async (context, next) =>
                {
                    try
                    {
                        throw new ArgumentException("This is an inner exception message ...", nameof(app))
                        {
                            Data =
                            {
                                { "1st", "data value" }
                            },
                            HelpLink = "https://www.savvyio.net/"
                        };
                    }
                    catch (Exception e)
                    {
                        throw new NotFoundException("Main exception - look out for inner!", e);
                    }

                    await next(context);
                });
            },
            responseFactory: client =>
            {
                client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/xml"));
                return client.GetAsync("/");
            });

        var body = await response.Content.ReadAsStringAsync();

        TestOutput.WriteLine(body);

        Assert.True(Match("""
                              <?xml version="1.0" encoding="utf-8"?>
                              <ProblemDetails>
                              	<Type>about:blank</Type>
                              	<Title>NotFound</Title>
                              	<Status>404</Status>
                              	<Detail>Main exception - look out for inner!</Detail>
                              	<Instance>http://localhost/</Instance>
                              	<TraceId>*</TraceId>
                              	<Cuemon.AspNetCore.Http.NotFoundException namespace="Cuemon.AspNetCore.Http">
                              		<Source>Cuemon.AspNetCore.FunctionalTests</Source>
                              		<Message>Main exception - look out for inner!</Message>
                              		<Stack>
                              			<Frame>at Cuemon.AspNetCore.Diagnostics.ApplicationBuilderExtensionsTest*</Frame>
                              			<Frame>--- End of stack trace from previous location ---</Frame>
                              			<Frame>at Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware*</Frame>
                              		</Stack>
                              		<StatusCode>404</StatusCode>
                              		<ReasonPhrase>Not Found</ReasonPhrase>
                              		<System.ArgumentException namespace="System">
                              			<Source>Cuemon.AspNetCore.FunctionalTests</Source>
                              			<Message>This is an inner exception message ... (Parameter 'app')</Message>
                              			<Stack>
                              				<Frame>at Cuemon.AspNetCore.Diagnostics.ApplicationBuilderExtensionsTest*</Frame>
                              			</Stack>
                              			<Data>
                              				<st>data value</st>
                              			</Data>
                              			<ParamName>app</ParamName>
                              		</System.ArgumentException>
                              	</Cuemon.AspNetCore.Http.NotFoundException>
                              	<Request>
                              		<Location>http://localhost/</Location>
                              		<Method>GET</Method>
                              		<Header name="Accept">application/xml</Header>
                              		<Header name="Host">localhost</Header>
                              	</Request>
                              </ProblemDetails>
                              """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
    }
}
