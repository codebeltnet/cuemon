using System;
using System.Threading.Tasks;
using Cuemon.Configuration;
using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.AspNetCore.Mvc.Filters;
public class ConfigurableFilterBaseTest : Test
{
    public ConfigurableFilterBaseTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ConfigurableActionFilter_ShouldPopulateOptionsFromDelegateAndOptions()
    {
        var fromDelegate = new FakeActionFilter(o => o.Number = 42);
        var fromOptions = new FakeActionFilter(Options.Create(new FakeFilterOptions() { Number = 84 }));

        Assert.Equal(42, fromDelegate.Options.Number);
        Assert.Equal(84, fromOptions.Options.Number);
    }

    [Fact]
    public void ConfigurableAsyncActionFilter_ShouldPopulateOptionsFromDelegateAndOptions()
    {
        var fromDelegate = new FakeAsyncActionFilter(o => o.Number = 42);
        var fromOptions = new FakeAsyncActionFilter(Options.Create(new FakeFilterOptions() { Number = 84 }));

        Assert.Equal(42, fromDelegate.Options.Number);
        Assert.Equal(84, fromOptions.Options.Number);
    }

    [Fact]
    public void ConfigurableAsyncAuthorizationFilter_ShouldPopulateOptionsFromOptions()
    {
        var sut = new FakeAsyncAuthorizationFilter(Options.Create(new FakeFilterOptions() { Number = 42 }));

        Assert.Equal(42, sut.Options.Number);
    }

    [Fact]
    public void ConfigurableAsyncResultFilter_ShouldPopulateOptionsFromDelegateAndOptions()
    {
        var fromDelegate = new FakeAsyncResultFilter(o => o.Number = 42);
        var fromOptions = new FakeAsyncResultFilter(Options.Create(new FakeFilterOptions() { Number = 84 }));

        Assert.Equal(42, fromDelegate.Options.Number);
        Assert.Equal(84, fromOptions.Options.Number);
    }

    [Fact]
    public void ConfigurableFactoryFilter_ShouldPopulateOptionsAndBeNonReusableByDefault()
    {
        var fromDelegate = new FakeFactoryFilter(o => o.Number = 42);
        var fromOptions = new FakeFactoryFilter(Options.Create(new FakeFilterOptions() { Number = 84 }));

        Assert.Equal(42, fromDelegate.Options.Number);
        Assert.Equal(84, fromOptions.Options.Number);
        Assert.False(fromDelegate.IsReusable);
        Assert.Same(fromDelegate, fromDelegate.CreateInstance(null));
    }

    private sealed class FakeFilterOptions : IParameterObject
    {
        public int Number { get; set; }
    }

    private sealed class FakeActionFilter : ConfigurableActionFilter<FakeFilterOptions>
    {
        public FakeActionFilter(Action<FakeFilterOptions> setup) : base(setup)
        {
        }

        public FakeActionFilter(IOptions<FakeFilterOptions> setup) : base(setup)
        {
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }

    private sealed class FakeAsyncActionFilter : ConfigurableAsyncActionFilter<FakeFilterOptions>
    {
        public FakeAsyncActionFilter(Action<FakeFilterOptions> setup) : base(setup)
        {
        }

        public FakeAsyncActionFilter(IOptions<FakeFilterOptions> setup) : base(setup)
        {
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class FakeAsyncAuthorizationFilter : ConfigurableAsyncAuthorizationFilter<FakeFilterOptions>
    {
        public FakeAsyncAuthorizationFilter(IOptions<FakeFilterOptions> setup) : base(setup)
        {
        }

        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class FakeAsyncResultFilter : ConfigurableAsyncResultFilter<FakeFilterOptions>
    {
        public FakeAsyncResultFilter(Action<FakeFilterOptions> setup) : base(setup)
        {
        }

        public FakeAsyncResultFilter(IOptions<FakeFilterOptions> setup) : base(setup)
        {
        }

        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class FakeFactoryFilter : ConfigurableFactoryFilter<FakeFilterOptions>
    {
        public FakeFactoryFilter(Action<FakeFilterOptions> setup) : base(setup)
        {
        }

        public FakeFactoryFilter(IOptions<FakeFilterOptions> setup) : base(setup)
        {
        }

        public override IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
