using System.Net.Http.Headers;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Mvc.Assets;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Mvc.Filters;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml;
using Cuemon.Extensions.DependencyInjection;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
public class FaultDescriptorFilterTest : Test
{
    public FaultDescriptorFilterTest(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(FaultSensitivityDetails.All)]
    [InlineData(FaultSensitivityDetails.Evidence)]
    [InlineData(FaultSensitivityDetails.FailureWithStackTraceAndData)]
    [InlineData(FaultSensitivityDetails.FailureWithData)]
    [InlineData(FaultSensitivityDetails.FailureWithStackTrace)]
    [InlineData(FaultSensitivityDetails.Failure)]
    [InlineData(FaultSensitivityDetails.None)]
    public async Task OnException_ShouldCaptureException_RenderAsProblemDetails_UsingJson(FaultSensitivityDetails sensitivity)
    {
        using var response = await WebHostTestFactory.RunAsync(
            services =>
            {
                services
                    .AddControllers(o => o.Filters.AddFaultDescriptor())
                    .AddApplicationPart(typeof(StatusCodesController).Assembly)
                    .AddJsonFormatters()
                    .AddFaultDescriptorOptions(o => o.FaultDescriptor = PreferredFaultDescriptor.ProblemDetails);
                services.PostConfigureAllOf<IExceptionDescriptorOptions>(o => o.SensitivityDetails = sensitivity);
            },
            app =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            },
            responseFactory: client =>
            {
                client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                return client.GetAsync("/statuscodes/XXX/serverError");
            });

        var body = await response.Content.ReadAsStringAsync();
        TestOutput.WriteLine(body);

        switch (sensitivity)
        {
            case FaultSensitivityDetails.All:
                Assert.True(Match("""
                                      {
                                        "type": "about:blank",
                                        "title": "InternalServerError",
                                        "status": 500,
                                        "detail": "An unhandled exception was raised by *",
                                        "instance": "http://localhost/statuscodes/XXX/serverError",
                                        "traceId": "*",
                                        "failure": {
                                          "type": "System.NotSupportedException",
                                          "source": "Cuemon.AspNetCore.Mvc.FunctionalTests",
                                          "message": "Main exception - look out for inner!",
                                          "stack": [
                                            "at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app) *",
                                            "at *",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute*",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()",
                                            "--- End of stack trace from previous location ---",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()",
                                            "--- End of stack trace from previous location ---",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextExceptionFilterAsync>*"
                                          ],
                                          "inner": {
                                            "type": "System.ArgumentException",
                                            "source": "Cuemon.AspNetCore.Mvc.FunctionalTests",
                                            "message": "This is an inner exception message ... (Parameter 'app')",
                                            "stack": [
                                              "at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app)*"
                                            ],
                                            "data": {
                                              "app": "serverError"
                                            },
                                            "paramName": "app"
                                          }
                                        },
                                        "request": {
                                          "location": "http://localhost/statuscodes/XXX/serverError",
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
                                        "title": "InternalServerError",
                                        "status": 500,
                                        "detail": "An unhandled exception was raised by *",
                                        "instance": "http://localhost/statuscodes/XXX/serverError",
                                        "traceId": "*",
                                        "request": {
                                          "location": "http://localhost/statuscodes/XXX/serverError",
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
                                        "title": "InternalServerError",
                                        "status": 500,
                                        "detail": "An unhandled exception was raised by *",
                                        "instance": "http://localhost/statuscodes/XXX/serverError",
                                        "traceId": "*",
                                        "failure": {
                                          "type": "System.NotSupportedException",
                                          "source": "Cuemon.AspNetCore.Mvc.FunctionalTests",
                                          "message": "Main exception - look out for inner!",
                                          "stack": [
                                            "at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app) *",
                                            "at *",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute*",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()",
                                            "--- End of stack trace from previous location ---",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()",
                                            "--- End of stack trace from previous location ---",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextExceptionFilterAsync>*"
                                          ],
                                          "inner": {
                                            "type": "System.ArgumentException",
                                            "source": "Cuemon.AspNetCore.Mvc.FunctionalTests",
                                            "message": "This is an inner exception message ... (Parameter 'app')",
                                            "stack": [
                                              "at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app)*"
                                            ],
                                            "data": {
                                              "app": "serverError"
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
                                        "title": "InternalServerError",
                                        "status": 500,
                                        "detail": "An unhandled exception was raised by *",
                                        "instance": "http://localhost/statuscodes/XXX/serverError",
                                        "traceId": "*",
                                        "failure": {
                                          "type": "System.NotSupportedException",
                                          "source": "Cuemon.AspNetCore.Mvc.FunctionalTests",
                                          "message": "Main exception - look out for inner!",
                                          "inner": {
                                            "type": "System.ArgumentException",
                                            "source": "Cuemon.AspNetCore.Mvc.FunctionalTests",
                                            "message": "This is an inner exception message ... (Parameter 'app')",
                                            "data": {
                                              "app": "serverError"
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
                                        "title": "InternalServerError",
                                        "status": 500,
                                        "detail": "An unhandled exception was raised by *",
                                        "instance": "http://localhost/statuscodes/XXX/serverError",
                                        "traceId": "*",
                                        "failure": {
                                          "type": "System.NotSupportedException",
                                          "source": "Cuemon.AspNetCore.Mvc.FunctionalTests",
                                          "message": "Main exception - look out for inner!",
                                          "stack": [
                                            "at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app) *",
                                            "at *",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute*",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()",
                                            "--- End of stack trace from previous location ---",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()",
                                            "--- End of stack trace from previous location ---",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextExceptionFilterAsync>*"
                                          ],
                                          "inner": {
                                            "type": "System.ArgumentException",
                                            "source": "Cuemon.AspNetCore.Mvc.FunctionalTests",
                                            "message": "This is an inner exception message ... (Parameter 'app')",
                                            "stack": [
                                              "at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app)*"
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
                                        "title": "InternalServerError",
                                        "status": 500,
                                        "detail": "An unhandled exception was raised by *",
                                        "instance": "http://localhost/statuscodes/XXX/serverError",
                                        "traceId": "*",
                                        "failure": {
                                          "type": "System.NotSupportedException",
                                          "source": "Cuemon.AspNetCore.Mvc.FunctionalTests",
                                          "message": "Main exception - look out for inner!",
                                          "inner": {
                                            "type": "System.ArgumentException",
                                            "source": "Cuemon.AspNetCore.Mvc.FunctionalTests",
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
                                        "title": "InternalServerError",
                                        "status": 500,
                                        "detail": "An unhandled exception was raised by *",
                                        "instance": "http://localhost/statuscodes/XXX/serverError",
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
    public async Task OnException_ShouldCaptureException_RenderAsDefault_UsingJson(FaultSensitivityDetails sensitivity)
    {
        using var response = await WebHostTestFactory.RunAsync(
            services =>
            {
                services
                    .AddControllers(o => o.Filters.AddFaultDescriptor())
                    .AddApplicationPart(typeof(StatusCodesController).Assembly)
                    .AddJsonFormatters()
                    .AddFaultDescriptorOptions(o => o.FaultDescriptor = PreferredFaultDescriptor.FaultDetails);
                services.PostConfigureAllOf<IExceptionDescriptorOptions>(o => o.SensitivityDetails = sensitivity);
            },
            app =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            },
            responseFactory: client =>
            {
                client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                return client.GetAsync("/statuscodes/XXX/serverError");
            });

        var body = await response.Content.ReadAsStringAsync();
        TestOutput.WriteLine(body);

        switch (sensitivity)
        {
            case FaultSensitivityDetails.All:
                Assert.True(Match("""
                                      {
                                        "error": {
                                          "instance": "http://localhost/statuscodes/XXX/serverError",
                                          "status": 500,
                                          "code": "InternalServerError",
                                          "message": "An unhandled exception was raised by *.",
                                          "failure": {
                                            "type": "System.NotSupportedException",
                                            "source": "Cuemon.AspNetCore.Mvc.FunctionalTests",
                                            "message": "Main exception - look out for inner!",
                                            "stack": [
                                              "at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app) *",
                                              "at *",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute*",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()",
                                              "--- End of stack trace from previous location ---",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()",
                                              "--- End of stack trace from previous location ---",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextExceptionFilterAsync>*"
                                            ],
                                            "inner": {
                                              "type": "System.ArgumentException",
                                              "source": "Cuemon.AspNetCore.Mvc.FunctionalTests",
                                              "message": "This is an inner exception message ... (Parameter 'app')",
                                              "stack": [
                                                "at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app)*"
                                              ],
                                              "data": {
                                                "app": "serverError"
                                              },
                                              "paramName": "app"
                                            }
                                          }
                                        },
                                        "evidence": {
                                          "request": {
                                            "location": "http://localhost/statuscodes/XXX/serverError",
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
                                          "instance": "http://localhost/statuscodes/XXX/serverError",
                                          "status": 500,
                                          "code": "InternalServerError",
                                          "message": "An unhandled exception was raised by *."
                                        },
                                        "evidence": {
                                          "request": {
                                            "location": "http://localhost/statuscodes/XXX/serverError",
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
                                          "instance": "http://localhost/statuscodes/XXX/serverError",
                                          "status": 500,
                                          "code": "InternalServerError",
                                          "message": "An unhandled exception was raised by *.",
                                          "failure": {
                                            "type": "System.NotSupportedException",
                                            "source": "Cuemon.AspNetCore.Mvc.FunctionalTests",
                                            "message": "Main exception - look out for inner!",
                                            "stack": [
                                              "at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app) *",
                                              "at *",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute*",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()",
                                              "--- End of stack trace from previous location ---",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()",
                                              "--- End of stack trace from previous location ---",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextExceptionFilterAsync>*"
                                            ],
                                            "inner": {
                                              "type": "System.ArgumentException",
                                              "source": "Cuemon.AspNetCore.Mvc.FunctionalTests",
                                              "message": "This is an inner exception message ... (Parameter 'app')",
                                              "stack": [
                                                "at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app)*"
                                              ],
                                              "data": {
                                                "app": "serverError"
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
                                          "instance": "http://localhost/statuscodes/XXX/serverError",
                                          "status": 500,
                                          "code": "InternalServerError",
                                          "message": "An unhandled exception was raised by *.",
                                          "failure": {
                                            "type": "System.NotSupportedException",
                                            "source": "Cuemon.AspNetCore.Mvc.FunctionalTests",
                                            "message": "Main exception - look out for inner!",
                                            "inner": {
                                              "type": "System.ArgumentException",
                                              "source": "Cuemon.AspNetCore.Mvc.FunctionalTests",
                                              "message": "This is an inner exception message ... (Parameter 'app')",
                                              "data": {
                                                "app": "serverError"
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
                                          "instance": "http://localhost/statuscodes/XXX/serverError",
                                          "status": 500,
                                          "code": "InternalServerError",
                                          "message": "An unhandled exception was raised by *.",
                                          "failure": {
                                            "type": "System.NotSupportedException",
                                            "source": "Cuemon.AspNetCore.Mvc.FunctionalTests",
                                            "message": "Main exception - look out for inner!",
                                            "stack": [
                                              "at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app) *",
                                              "at *",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute*",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()",
                                              "--- End of stack trace from previous location ---",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()",
                                              "--- End of stack trace from previous location ---",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextExceptionFilterAsync>*"
                                            ],
                                            "inner": {
                                              "type": "System.ArgumentException",
                                              "source": "Cuemon.AspNetCore.Mvc.FunctionalTests",
                                              "message": "This is an inner exception message ... (Parameter 'app')",
                                              "stack": [
                                                "at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app)*"
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
                                          "instance": "http://localhost/statuscodes/XXX/serverError",
                                          "status": 500,
                                          "code": "InternalServerError",
                                          "message": "An unhandled exception was raised by *.",
                                          "failure": {
                                            "type": "System.NotSupportedException",
                                            "source": "Cuemon.AspNetCore.Mvc.FunctionalTests",
                                            "message": "Main exception - look out for inner!",
                                            "inner": {
                                              "type": "System.ArgumentException",
                                              "source": "Cuemon.AspNetCore.Mvc.FunctionalTests",
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
                                          "instance": "http://localhost/statuscodes/XXX/serverError",
                                          "status": 500,
                                          "code": "InternalServerError",
                                          "message": "An unhandled exception was raised by *."
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
    public async Task OnException_ShouldCaptureException_RenderAsProblemDetails_UsingXml(FaultSensitivityDetails sensitivity)
    {
        using var response = await WebHostTestFactory.RunAsync(
            services =>
            {
                services
                    .AddControllers(o => o.Filters.AddFaultDescriptor())
                    .AddApplicationPart(typeof(StatusCodesController).Assembly)
                    .AddXmlFormatters(o => o.Settings.Writer.Indent = true)
                    .AddFaultDescriptorOptions(o => o.FaultDescriptor = PreferredFaultDescriptor.ProblemDetails);
                services.PostConfigureAllOf<IExceptionDescriptorOptions>(o => o.SensitivityDetails = sensitivity);
            },
            app =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            },
            responseFactory: client =>
            {
                client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/xml"));
                return client.GetAsync("/statuscodes/XXX/serverError");
            });

        var body = await response.Content.ReadAsStringAsync();
        TestOutput.WriteLine(body);

        switch (sensitivity)
        {
            case FaultSensitivityDetails.All:
                Assert.True(Match("""
                                      <?xml version="1.0" encoding="utf-8"?>
                                      <ProblemDetails>
                                      	<Type>about:blank</Type>
                                      	<Title>InternalServerError</Title>
                                      	<Status>500</Status>
                                      	<Detail>An unhandled exception was raised by *</Detail>
                                      	<Instance>http://localhost/statuscodes/XXX/serverError</Instance>
                                      	<TraceId>*</TraceId>
                                      	<System.NotSupportedException namespace="System">
                                      		<Source>Cuemon.AspNetCore.Mvc.FunctionalTests</Source>
                                      		<Message>Main exception - look out for inner!</Message>
                                      		<Stack>
                                      			<Frame>at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app)*</Frame>
                                      			<Frame>at *</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute*</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State&amp; next, Scope&amp; scope, Object&amp; state, Boolean&amp; isCompleted)</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()</Frame>
                                      			<Frame>--- End of stack trace from previous location ---</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State&amp; next, Scope&amp; scope, Object&amp; state, Boolean&amp; isCompleted)</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()</Frame>
                                      			<Frame>--- End of stack trace from previous location ---</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.&lt;InvokeNextExceptionFilterAsync*</Frame>
                                      		</Stack>
                                      		<System.ArgumentException namespace="System">
                                      			<Source>Cuemon.AspNetCore.Mvc.FunctionalTests</Source>
                                      			<Message>This is an inner exception message ... (Parameter 'app')</Message>
                                      			<Stack>
                                      				<Frame>at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app)*</Frame>
                                      			</Stack>
                                      			<Data>
                                      				<app>serverError</app>
                                      			</Data>
                                      			<ParamName>app</ParamName>
                                      		</System.ArgumentException>
                                      	</System.NotSupportedException>
                                      	<Request>
                                      		<Location>http://localhost/statuscodes/XXX/serverError</Location>
                                      		<Method>GET</Method>
                                      		<Header name="Accept">application/xml</Header>
                                      		<Header name="Host">localhost</Header>
                                      	</Request>
                                      </ProblemDetails>
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.Evidence:
                Assert.True(Match("""
                                      <?xml version="1.0" encoding="utf-8"?>
                                      <ProblemDetails>
                                      	<Type>about:blank</Type>
                                      	<Title>InternalServerError</Title>
                                      	<Status>500</Status>
                                      	<Detail>An unhandled exception was raised by *</Detail>
                                      	<Instance>http://localhost/statuscodes/XXX/serverError</Instance>
                                      	<TraceId>*</TraceId>
                                      	<Request>
                                      		<Location>http://localhost/statuscodes/XXX/serverError</Location>
                                      		<Method>GET</Method>
                                      		<Header name="Accept">application/xml</Header>
                                      		<Header name="Host">localhost</Header>
                                      	</Request>
                                      </ProblemDetails>
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.FailureWithStackTraceAndData:
                Assert.True(Match("""
                                      <?xml version="1.0" encoding="utf-8"?>
                                      <ProblemDetails>
                                      	<Type>about:blank</Type>
                                      	<Title>InternalServerError</Title>
                                      	<Status>500</Status>
                                      	<Detail>An unhandled exception was raised by *</Detail>
                                      	<Instance>http://localhost/statuscodes/XXX/serverError</Instance>
                                      	<TraceId>*</TraceId>
                                      	<System.NotSupportedException namespace="System">
                                      		<Source>Cuemon.AspNetCore.Mvc.FunctionalTests</Source>
                                      		<Message>Main exception - look out for inner!</Message>
                                      		<Stack>
                                      			<Frame>at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app)*</Frame>
                                      			<Frame>at *</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute*</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State&amp; next, Scope&amp; scope, Object&amp; state, Boolean&amp; isCompleted)</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()</Frame>
                                      			<Frame>--- End of stack trace from previous location ---</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State&amp; next, Scope&amp; scope, Object&amp; state, Boolean&amp; isCompleted)</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()</Frame>
                                      			<Frame>--- End of stack trace from previous location ---</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.&lt;InvokeNextExceptionFilterAsync*</Frame>
                                      		</Stack>
                                      		<System.ArgumentException namespace="System">
                                      			<Source>Cuemon.AspNetCore.Mvc.FunctionalTests</Source>
                                      			<Message>This is an inner exception message ... (Parameter 'app')</Message>
                                      			<Stack>
                                      				<Frame>at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app)*</Frame>
                                      			</Stack>
                                      			<Data>
                                      				<app>serverError</app>
                                      			</Data>
                                      			<ParamName>app</ParamName>
                                      		</System.ArgumentException>
                                      	</System.NotSupportedException>
                                      </ProblemDetails>
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.FailureWithData:
                Assert.True(Match("""
                                      <?xml version="1.0" encoding="utf-8"?>
                                      <ProblemDetails>
                                      	<Type>about:blank</Type>
                                      	<Title>InternalServerError</Title>
                                      	<Status>500</Status>
                                      	<Detail>An unhandled exception was raised by *</Detail>
                                      	<Instance>http://localhost/statuscodes/XXX/serverError</Instance>
                                      	<TraceId>*</TraceId>
                                      	<System.NotSupportedException namespace="System">
                                      		<Source>Cuemon.AspNetCore.Mvc.FunctionalTests</Source>
                                      		<Message>Main exception - look out for inner!</Message>
                                      		<System.ArgumentException namespace="System">
                                      			<Source>Cuemon.AspNetCore.Mvc.FunctionalTests</Source>
                                      			<Message>This is an inner exception message ... (Parameter 'app')</Message>
                                      			<Data>
                                      				<app>serverError</app>
                                      			</Data>
                                      			<ParamName>app</ParamName>
                                      		</System.ArgumentException>
                                      	</System.NotSupportedException>
                                      </ProblemDetails>
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.FailureWithStackTrace:
                Assert.True(Match("""
                                      <?xml version="1.0" encoding="utf-8"?>
                                      <ProblemDetails>
                                      	<Type>about:blank</Type>
                                      	<Title>InternalServerError</Title>
                                      	<Status>500</Status>
                                      	<Detail>An unhandled exception was raised by *</Detail>
                                      	<Instance>http://localhost/statuscodes/XXX/serverError</Instance>
                                      	<TraceId>*</TraceId>
                                      	<System.NotSupportedException namespace="System">
                                      		<Source>Cuemon.AspNetCore.Mvc.FunctionalTests</Source>
                                      		<Message>Main exception - look out for inner!</Message>
                                      		<Stack>
                                      			<Frame>at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app)*</Frame>
                                      			<Frame>at *</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute*</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State&amp; next, Scope&amp; scope, Object&amp; state, Boolean&amp; isCompleted)</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()</Frame>
                                      			<Frame>--- End of stack trace from previous location ---</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State&amp; next, Scope&amp; scope, Object&amp; state, Boolean&amp; isCompleted)</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()</Frame>
                                      			<Frame>--- End of stack trace from previous location ---</Frame>
                                      			<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.&lt;InvokeNextExceptionFilterAsync*</Frame>
                                      		</Stack>
                                      		<System.ArgumentException namespace="System">
                                      			<Source>Cuemon.AspNetCore.Mvc.FunctionalTests</Source>
                                      			<Message>This is an inner exception message ... (Parameter 'app')</Message>
                                      			<Stack>
                                      				<Frame>at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app)*</Frame>
                                      			</Stack>
                                      			<ParamName>app</ParamName>
                                      		</System.ArgumentException>
                                      	</System.NotSupportedException>
                                      </ProblemDetails>
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.Failure:
                Assert.True(Match("""
                                      <?xml version="1.0" encoding="utf-8"?>
                                      <ProblemDetails>
                                      	<Type>about:blank</Type>
                                      	<Title>InternalServerError</Title>
                                      	<Status>500</Status>
                                      	<Detail>An unhandled exception was raised by *</Detail>
                                      	<Instance>http://localhost/statuscodes/XXX/serverError</Instance>
                                      	<TraceId>*</TraceId>
                                      	<System.NotSupportedException namespace="System">
                                      		<Source>Cuemon.AspNetCore.Mvc.FunctionalTests</Source>
                                      		<Message>Main exception - look out for inner!</Message>
                                      		<System.ArgumentException namespace="System">
                                      			<Source>Cuemon.AspNetCore.Mvc.FunctionalTests</Source>
                                      			<Message>This is an inner exception message ... (Parameter 'app')</Message>
                                      			<ParamName>app</ParamName>
                                      		</System.ArgumentException>
                                      	</System.NotSupportedException>
                                      </ProblemDetails>
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.None:
                Assert.True(Match("""
                                      <?xml version="1.0" encoding="utf-8"?>
                                      <ProblemDetails>
                                      	<Type>about:blank</Type>
                                      	<Title>InternalServerError</Title>
                                      	<Status>500</Status>
                                      	<Detail>An unhandled exception was raised by *</Detail>
                                      	<Instance>http://localhost/statuscodes/XXX/serverError</Instance>
                                      	<TraceId>*</TraceId>
                                      </ProblemDetails>
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
    public async Task OnException_ShouldCaptureException_RenderAsDefault_UsingXml(FaultSensitivityDetails sensitivity)
    {
        using var response = await WebHostTestFactory.RunAsync(
            services =>
            {
                services
                    .AddControllers(o => o.Filters.AddFaultDescriptor())
                    .AddApplicationPart(typeof(StatusCodesController).Assembly)
                    .AddXmlFormatters(o => o.Settings.Writer.Indent = true)
                    .AddFaultDescriptorOptions(o => o.FaultDescriptor = PreferredFaultDescriptor.FaultDetails);
                services.PostConfigureAllOf<IExceptionDescriptorOptions>(o => o.SensitivityDetails = sensitivity);
            },
            app =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            },
            responseFactory: client =>
            {
                client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/xml"));
                return client.GetAsync("/statuscodes/XXX/serverError");
            });

        var body = await response.Content.ReadAsStringAsync();
        TestOutput.WriteLine(body);

        switch (sensitivity)
        {
            case FaultSensitivityDetails.All:
                Assert.True(Match("""
                                      <?xml version="1.0" encoding="utf-8"?>
                                      <HttpExceptionDescriptor>
                                      	<Error>
                                      		<Instance>http://localhost/statuscodes/XXX/serverError</Instance>
                                      		<Status>500</Status>
                                      		<Code>InternalServerError</Code>
                                      		<Message>An unhandled exception was raised by *.</Message>
                                      		<Failure>
                                      			<NotSupportedException namespace="System">
                                      				<Source>Cuemon.AspNetCore.Mvc.FunctionalTests</Source>
                                      				<Message>Main exception - look out for inner!</Message>
                                      				<Stack>
                                      					<Frame>at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app)*</Frame>
                                      					<Frame>at *</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute*</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State&amp; next, Scope&amp; scope, Object&amp; state, Boolean&amp; isCompleted)</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()</Frame>
                                      					<Frame>--- End of stack trace from previous location ---</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State&amp; next, Scope&amp; scope, Object&amp; state, Boolean&amp; isCompleted)</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()</Frame>
                                      					<Frame>--- End of stack trace from previous location ---</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.&lt;InvokeNextExceptionFilterAsync*</Frame>
                                      				</Stack>
                                      				<ArgumentException namespace="System">
                                      					<Source>Cuemon.AspNetCore.Mvc.FunctionalTests</Source>
                                      					<Message>This is an inner exception message ... (Parameter 'app')</Message>
                                      					<Stack>
                                      						<Frame>at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app)*</Frame>
                                      					</Stack>
                                      					<Data>
                                      						<app>serverError</app>
                                      					</Data>
                                      					<ParamName>app</ParamName>
                                      				</ArgumentException>
                                      			</NotSupportedException>
                                      		</Failure>
                                      	</Error>
                                      	<Evidence>
                                      		<Request>
                                      			<Location>http://localhost/statuscodes/XXX/serverError</Location>
                                      			<Method>GET</Method>
                                      			<Header name="Accept">application/xml</Header>
                                      			<Header name="Host">localhost</Header>
                                      		</Request>
                                      	</Evidence>
                                      	<TraceId>*</TraceId>
                                      </HttpExceptionDescriptor>
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.Evidence:
                Assert.True(Match("""
                                      <?xml version="1.0" encoding="utf-8"?>
                                      <HttpExceptionDescriptor>
                                      	<Error>
                                      		<Instance>http://localhost/statuscodes/XXX/serverError</Instance>
                                      		<Status>500</Status>
                                      		<Code>InternalServerError</Code>
                                      		<Message>An unhandled exception was raised by *.</Message>
                                      	</Error>
                                      	<Evidence>
                                      		<Request>
                                      			<Location>http://localhost/statuscodes/XXX/serverError</Location>
                                      			<Method>GET</Method>
                                      			<Header name="Accept">application/xml</Header>
                                      			<Header name="Host">localhost</Header>
                                      		</Request>
                                      	</Evidence>
                                      	<TraceId>*</TraceId>
                                      </HttpExceptionDescriptor>
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.FailureWithStackTraceAndData:
                Assert.True(Match("""
                                      <?xml version="1.0" encoding="utf-8"?>
                                      <HttpExceptionDescriptor>
                                      	<Error>
                                      		<Instance>http://localhost/statuscodes/XXX/serverError</Instance>
                                      		<Status>500</Status>
                                      		<Code>InternalServerError</Code>
                                      		<Message>An unhandled exception was raised by *.</Message>
                                      		<Failure>
                                      			<NotSupportedException namespace="System">
                                      				<Source>Cuemon.AspNetCore.Mvc.FunctionalTests</Source>
                                      				<Message>Main exception - look out for inner!</Message>
                                      				<Stack>
                                      					<Frame>at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app)*</Frame>
                                      					<Frame>at *</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute*</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State&amp; next, Scope&amp; scope, Object&amp; state, Boolean&amp; isCompleted)</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()</Frame>
                                      					<Frame>--- End of stack trace from previous location ---</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State&amp; next, Scope&amp; scope, Object&amp; state, Boolean&amp; isCompleted)</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()</Frame>
                                      					<Frame>--- End of stack trace from previous location ---</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.&lt;InvokeNextExceptionFilterAsync*</Frame>
                                      				</Stack>
                                      				<ArgumentException namespace="System">
                                      					<Source>Cuemon.AspNetCore.Mvc.FunctionalTests</Source>
                                      					<Message>This is an inner exception message ... (Parameter 'app')</Message>
                                      					<Stack>
                                      						<Frame>at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app)*</Frame>
                                      					</Stack>
                                      					<Data>
                                      						<app>serverError</app>
                                      					</Data>
                                      					<ParamName>app</ParamName>
                                      				</ArgumentException>
                                      			</NotSupportedException>
                                      		</Failure>
                                      	</Error>
                                      	<TraceId>*</TraceId>
                                      </HttpExceptionDescriptor>
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.FailureWithData:
                Assert.True(Match("""
                                      <?xml version="1.0" encoding="utf-8"?>
                                      <HttpExceptionDescriptor>
                                      	<Error>
                                      		<Instance>http://localhost/statuscodes/XXX/serverError</Instance>
                                      		<Status>500</Status>
                                      		<Code>InternalServerError</Code>
                                      		<Message>An unhandled exception was raised by *.</Message>
                                      		<Failure>
                                      			<NotSupportedException namespace="System">
                                      				<Source>Cuemon.AspNetCore.Mvc.FunctionalTests</Source>
                                      				<Message>Main exception - look out for inner!</Message>
                                      				<ArgumentException namespace="System">
                                      					<Source>Cuemon.AspNetCore.Mvc.FunctionalTests</Source>
                                      					<Message>This is an inner exception message ... (Parameter 'app')</Message>
                                      					<Data>
                                      						<app>serverError</app>
                                      					</Data>
                                      					<ParamName>app</ParamName>
                                      				</ArgumentException>
                                      			</NotSupportedException>
                                      		</Failure>
                                      	</Error>
                                      	<TraceId>*</TraceId>
                                      </HttpExceptionDescriptor>
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.FailureWithStackTrace:
                Assert.True(Match("""
                                      <?xml version="1.0" encoding="utf-8"?>
                                      <HttpExceptionDescriptor>
                                      	<Error>
                                      		<Instance>http://localhost/statuscodes/XXX/serverError</Instance>
                                      		<Status>500</Status>
                                      		<Code>InternalServerError</Code>
                                      		<Message>An unhandled exception was raised by *.</Message>
                                      		<Failure>
                                      			<NotSupportedException namespace="System">
                                      				<Source>Cuemon.AspNetCore.Mvc.FunctionalTests</Source>
                                      				<Message>Main exception - look out for inner!</Message>
                                      				<Stack>
                                      					<Frame>at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app)*</Frame>
                                      					<Frame>at *</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute*</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State&amp; next, Scope&amp; scope, Object&amp; state, Boolean&amp; isCompleted)</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()</Frame>
                                      					<Frame>--- End of stack trace from previous location ---</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State&amp; next, Scope&amp; scope, Object&amp; state, Boolean&amp; isCompleted)</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()</Frame>
                                      					<Frame>--- End of stack trace from previous location ---</Frame>
                                      					<Frame>at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.&lt;InvokeNextExceptionFilterAsync*</Frame>
                                      				</Stack>
                                      				<ArgumentException namespace="System">
                                      					<Source>Cuemon.AspNetCore.Mvc.FunctionalTests</Source>
                                      					<Message>This is an inner exception message ... (Parameter 'app')</Message>
                                      					<Stack>
                                      						<Frame>at Cuemon.AspNetCore.Mvc.Assets.StatusCodesController.Get_XXX(String app)*</Frame>
                                      					</Stack>
                                      					<ParamName>app</ParamName>
                                      				</ArgumentException>
                                      			</NotSupportedException>
                                      		</Failure>
                                      	</Error>
                                      	<TraceId>*</TraceId>
                                      </HttpExceptionDescriptor>
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.Failure:
                Assert.True(Match("""
                                      <?xml version="1.0" encoding="utf-8"?>
                                      <HttpExceptionDescriptor>
                                      	<Error>
                                      		<Instance>http://localhost/statuscodes/XXX/serverError</Instance>
                                      		<Status>500</Status>
                                      		<Code>InternalServerError</Code>
                                      		<Message>An unhandled exception was raised by *.</Message>
                                      		<Failure>
                                      			<NotSupportedException namespace="System">
                                      				<Source>Cuemon.AspNetCore.Mvc.FunctionalTests</Source>
                                      				<Message>Main exception - look out for inner!</Message>
                                      				<ArgumentException namespace="System">
                                      					<Source>Cuemon.AspNetCore.Mvc.FunctionalTests</Source>
                                      					<Message>This is an inner exception message ... (Parameter 'app')</Message>
                                      					<ParamName>app</ParamName>
                                      				</ArgumentException>
                                      			</NotSupportedException>
                                      		</Failure>
                                      	</Error>
                                      	<TraceId>*</TraceId>
                                      </HttpExceptionDescriptor>
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
            case FaultSensitivityDetails.None:
                Assert.True(Match("""
                                      <?xml version="1.0" encoding="utf-8"?>
                                      <HttpExceptionDescriptor>
                                      	<Error>
                                      		<Instance>http://localhost/statuscodes/XXX/serverError</Instance>
                                      		<Status>500</Status>
                                      		<Code>InternalServerError</Code>
                                      		<Message>An unhandled exception was raised by *.</Message>
                                      	</Error>
                                      	<TraceId>*</TraceId>
                                      </HttpExceptionDescriptor>
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                break;
        }
    }
}
