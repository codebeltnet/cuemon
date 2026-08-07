using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Cuemon.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.AspNetCore;
public class MiddlewareTest : Test
{
    public MiddlewareTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task Middleware_ShouldInvokeNextDelegate_ForAllSupportedArityVariants()
    {
        var calls = new List<string>();
        RequestDelegate next = _ =>
        {
            calls.Add("next");
            return Task.CompletedTask;
        };
        var context = new DefaultHttpContext();

        await new FakeMiddleware(next, calls).InvokeAsync(context);
        await new FakeMiddleware<string>(next, calls).InvokeAsync(context, "one");
        await new FakeMiddleware<string, int>(next, calls).InvokeAsync(context, "two", 2);
        await new FakeMiddleware<string, int, bool>(next, calls).InvokeAsync(context, "three", 3, true);
        await new FakeMiddleware<string, int, bool, decimal>(next, calls).InvokeAsync(context, "four", 4, false, 4.5m);
        await new FakeMiddleware<string, int, bool, decimal, Guid>(next, calls).InvokeAsync(context, "five", 5, true, 5.5m, Guid.Empty);

        Assert.Equal(new[]
        {
            "0", "next",
            "1:one", "next",
            "2:two:2", "next",
            "3:three:3:True", "next",
            $"4:four:4:False:{4.5m}", "next",
            $"5:five:5:True:{5.5m}:{Guid.Empty}", "next"
        }, calls);
    }

    [Fact]
    public async Task ConfigurableMiddleware_ShouldExposeConfiguredOptions_ForAllSupportedArityVariants()
    {
        var calls = new List<string>();
        RequestDelegate next = _ =>
        {
            calls.Add("next");
            return Task.CompletedTask;
        };
        var context = new DefaultHttpContext();

        var zeroFromOptions = new FakeConfigurableMiddleware(next, Options.Create(new FakeOptions { Message = "o0" }), calls);
        var zeroFromAction = new FakeConfigurableMiddleware(next, o => o.Message = "a0", calls);
        var oneFromOptions = new FakeConfigurableMiddleware<string>(next, Options.Create(new FakeOptions { Message = "o1" }), calls);
        var oneFromAction = new FakeConfigurableMiddleware<string>(next, o => o.Message = "a1", calls);
        var twoFromOptions = new FakeConfigurableMiddleware<string, int>(next, Options.Create(new FakeOptions { Message = "o2" }), calls);
        var twoFromAction = new FakeConfigurableMiddleware<string, int>(next, o => o.Message = "a2", calls);
        var threeFromOptions = new FakeConfigurableMiddleware<string, int, bool>(next, Options.Create(new FakeOptions { Message = "o3" }), calls);
        var threeFromAction = new FakeConfigurableMiddleware<string, int, bool>(next, o => o.Message = "a3", calls);
        var fourFromOptions = new FakeConfigurableMiddleware<string, int, bool, decimal>(next, Options.Create(new FakeOptions { Message = "o4" }), calls);
        var fourFromAction = new FakeConfigurableMiddleware<string, int, bool, decimal>(next, o => o.Message = "a4", calls);
        var fiveFromOptions = new FakeConfigurableMiddleware<string, int, bool, decimal, Guid>(next, Options.Create(new FakeOptions { Message = "o5" }), calls);
        var fiveFromAction = new FakeConfigurableMiddleware<string, int, bool, decimal, Guid>(next, o => o.Message = "a5", calls);

        await zeroFromOptions.InvokeAsync(context);
        await zeroFromAction.InvokeAsync(context);
        await oneFromOptions.InvokeAsync(context, "one");
        await oneFromAction.InvokeAsync(context, "one");
        await twoFromOptions.InvokeAsync(context, "two", 2);
        await twoFromAction.InvokeAsync(context, "two", 2);
        await threeFromOptions.InvokeAsync(context, "three", 3, true);
        await threeFromAction.InvokeAsync(context, "three", 3, true);
        await fourFromOptions.InvokeAsync(context, "four", 4, false, 4.5m);
        await fourFromAction.InvokeAsync(context, "four", 4, false, 4.5m);
        await fiveFromOptions.InvokeAsync(context, "five", 5, true, 5.5m, Guid.Empty);
        await fiveFromAction.InvokeAsync(context, "five", 5, true, 5.5m, Guid.Empty);

        Assert.Equal("o0", zeroFromOptions.Options.Message);
        Assert.Equal("a0", zeroFromAction.Options.Message);
        Assert.Equal("o1", oneFromOptions.Options.Message);
        Assert.Equal("a1", oneFromAction.Options.Message);
        Assert.Equal("o2", twoFromOptions.Options.Message);
        Assert.Equal("a2", twoFromAction.Options.Message);
        Assert.Equal("o3", threeFromOptions.Options.Message);
        Assert.Equal("a3", threeFromAction.Options.Message);
        Assert.Equal("o4", fourFromOptions.Options.Message);
        Assert.Equal("a4", fourFromAction.Options.Message);
        Assert.Equal("o5", fiveFromOptions.Options.Message);
        Assert.Equal("a5", fiveFromAction.Options.Message);
        Assert.Equal(24, calls.Count);
    }

    [Fact]
    public void MiddlewareCtor_ShouldThrow_WhenNextIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new FakeMiddleware(null, new List<string>()));
        Assert.Throws<ArgumentNullException>(() => new FakeConfigurableMiddleware(null, o => o.Message = "nope", new List<string>()));
    }

    private sealed class FakeOptions : IParameterObject
    {
        public string Message { get; set; }
    }

    private sealed class FakeMiddleware : Middleware
    {
        private readonly IList<string> _calls;

        public FakeMiddleware(RequestDelegate next, IList<string> calls) : base(next)
        {
            _calls = calls;
        }

        public override async Task InvokeAsync(HttpContext context)
        {
            _calls.Add("0");
            await Next(context);
        }
    }

    private sealed class FakeMiddleware<T> : Middleware<T>
    {
        private readonly IList<string> _calls;

        public FakeMiddleware(RequestDelegate next, IList<string> calls) : base(next)
        {
            _calls = calls;
        }

        public override async Task InvokeAsync(HttpContext context, T di)
        {
            _calls.Add($"1:{di}");
            await Next(context);
        }
    }

    private sealed class FakeMiddleware<T1, T2> : Middleware<T1, T2>
    {
        private readonly IList<string> _calls;

        public FakeMiddleware(RequestDelegate next, IList<string> calls) : base(next)
        {
            _calls = calls;
        }

        public override async Task InvokeAsync(HttpContext context, T1 di1, T2 di2)
        {
            _calls.Add($"2:{di1}:{di2}");
            await Next(context);
        }
    }

    private sealed class FakeMiddleware<T1, T2, T3> : Middleware<T1, T2, T3>
    {
        private readonly IList<string> _calls;

        public FakeMiddleware(RequestDelegate next, IList<string> calls) : base(next)
        {
            _calls = calls;
        }

        public override async Task InvokeAsync(HttpContext context, T1 di1, T2 di2, T3 di3)
        {
            _calls.Add($"3:{di1}:{di2}:{di3}");
            await Next(context);
        }
    }

    private sealed class FakeMiddleware<T1, T2, T3, T4> : Middleware<T1, T2, T3, T4>
    {
        private readonly IList<string> _calls;

        public FakeMiddleware(RequestDelegate next, IList<string> calls) : base(next)
        {
            _calls = calls;
        }

        public override async Task InvokeAsync(HttpContext context, T1 di1, T2 di2, T3 di3, T4 di4)
        {
            _calls.Add($"4:{di1}:{di2}:{di3}:{di4}");
            await Next(context);
        }
    }

    private sealed class FakeMiddleware<T1, T2, T3, T4, T5> : Middleware<T1, T2, T3, T4, T5>
    {
        private readonly IList<string> _calls;

        public FakeMiddleware(RequestDelegate next, IList<string> calls) : base(next)
        {
            _calls = calls;
        }

        public override async Task InvokeAsync(HttpContext context, T1 di1, T2 di2, T3 di3, T4 di4, T5 di5)
        {
            _calls.Add($"5:{di1}:{di2}:{di3}:{di4}:{di5}");
            await Next(context);
        }
    }

    private sealed class FakeConfigurableMiddleware : ConfigurableMiddleware<FakeOptions>
    {
        private readonly IList<string> _calls;

        public FakeConfigurableMiddleware(RequestDelegate next, IOptions<FakeOptions> setup, IList<string> calls) : base(next, setup)
        {
            _calls = calls;
        }

        public FakeConfigurableMiddleware(RequestDelegate next, Action<FakeOptions> setup, IList<string> calls) : base(next, setup)
        {
            _calls = calls;
        }

        public override async Task InvokeAsync(HttpContext context)
        {
            _calls.Add(Options.Message);
            await Next(context);
        }
    }

    private sealed class FakeConfigurableMiddleware<T> : ConfigurableMiddleware<T, FakeOptions>
    {
        private readonly IList<string> _calls;

        public FakeConfigurableMiddleware(RequestDelegate next, IOptions<FakeOptions> setup, IList<string> calls) : base(next, setup)
        {
            _calls = calls;
        }

        public FakeConfigurableMiddleware(RequestDelegate next, Action<FakeOptions> setup, IList<string> calls) : base(next, setup)
        {
            _calls = calls;
        }

        public override async Task InvokeAsync(HttpContext context, T di)
        {
            _calls.Add(Options.Message);
            await Next(context);
        }
    }

    private sealed class FakeConfigurableMiddleware<T1, T2> : ConfigurableMiddleware<T1, T2, FakeOptions>
    {
        private readonly IList<string> _calls;

        public FakeConfigurableMiddleware(RequestDelegate next, IOptions<FakeOptions> setup, IList<string> calls) : base(next, setup)
        {
            _calls = calls;
        }

        public FakeConfigurableMiddleware(RequestDelegate next, Action<FakeOptions> setup, IList<string> calls) : base(next, setup)
        {
            _calls = calls;
        }

        public override async Task InvokeAsync(HttpContext context, T1 di1, T2 di2)
        {
            _calls.Add(Options.Message);
            await Next(context);
        }
    }

    private sealed class FakeConfigurableMiddleware<T1, T2, T3> : ConfigurableMiddleware<T1, T2, T3, FakeOptions>
    {
        private readonly IList<string> _calls;

        public FakeConfigurableMiddleware(RequestDelegate next, IOptions<FakeOptions> setup, IList<string> calls) : base(next, setup)
        {
            _calls = calls;
        }

        public FakeConfigurableMiddleware(RequestDelegate next, Action<FakeOptions> setup, IList<string> calls) : base(next, setup)
        {
            _calls = calls;
        }

        public override async Task InvokeAsync(HttpContext context, T1 di1, T2 di2, T3 di3)
        {
            _calls.Add(Options.Message);
            await Next(context);
        }
    }

    private sealed class FakeConfigurableMiddleware<T1, T2, T3, T4> : ConfigurableMiddleware<T1, T2, T3, T4, FakeOptions>
    {
        private readonly IList<string> _calls;

        public FakeConfigurableMiddleware(RequestDelegate next, IOptions<FakeOptions> setup, IList<string> calls) : base(next, setup)
        {
            _calls = calls;
        }

        public FakeConfigurableMiddleware(RequestDelegate next, Action<FakeOptions> setup, IList<string> calls) : base(next, setup)
        {
            _calls = calls;
        }

        public override async Task InvokeAsync(HttpContext context, T1 di1, T2 di2, T3 di3, T4 di4)
        {
            _calls.Add(Options.Message);
            await Next(context);
        }
    }

    private sealed class FakeConfigurableMiddleware<T1, T2, T3, T4, T5> : ConfigurableMiddleware<T1, T2, T3, T4, T5, FakeOptions>
    {
        private readonly IList<string> _calls;

        public FakeConfigurableMiddleware(RequestDelegate next, IOptions<FakeOptions> setup, IList<string> calls) : base(next, setup)
        {
            _calls = calls;
        }

        public FakeConfigurableMiddleware(RequestDelegate next, Action<FakeOptions> setup, IList<string> calls) : base(next, setup)
        {
            _calls = calls;
        }

        public override async Task InvokeAsync(HttpContext context, T1 di1, T2 di2, T3 di3, T4 di4, T5 di5)
        {
            _calls.Add(Options.Message);
            await Next(context);
        }
    }
}
