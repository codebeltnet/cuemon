using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using Cuemon.Collections.Generic;
using Cuemon.Assets;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Diagnostics;
public class ExceptionDescriptorTest : Test
{
    public ExceptionDescriptorTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Extract_VerifyThatInlineExceptionIncludesSystemSnapshot()
    {
        var ane = new ArgumentNullException("myParam", "myMessage");
        var enrichedException = ExceptionInsights.Embed(ane, MethodBase.GetCurrentMethod(), Arguments.ToArray(null, "myParam", "myMessage"), SystemSnapshots.CaptureAll);
        var ed = ExceptionDescriptor.Extract(enrichedException);

        Assert.Equal(enrichedException.ToString(), ed.ToString());
        Assert.Equal("UnhandledException", ed.Code);
        Assert.Equal("An unhandled exception occurred.", ed.Message);
        Assert.Equal(4, ed.Evidence.Count);
        var me = ed.Evidence.Single(pair => pair.Key == "Thrower").Value as MemberEvidence;
        Assert.Equal(me.MemberSignature, "Cuemon.Diagnostics.ExceptionDescriptorTest.Extract_VerifyThatInlineExceptionIncludesSystemSnapshot()");
        Assert.Equal(3, me.RuntimeParameters.Count);
        Assert.True(me.RuntimeParameters.ContainsKey("arg1"));
        Assert.True(me.RuntimeParameters.ContainsKey("arg2"));
        Assert.True(me.RuntimeParameters.ContainsKey("arg3"));
        Assert.Null(me.RuntimeParameters["arg1"]);
        Assert.Equal("myParam", me.RuntimeParameters["arg2"]);
        Assert.Equal("myMessage", me.RuntimeParameters["arg3"]);
        var ti = ed.Evidence.Single(pair => pair.Key == "Thread").Value as IDictionary<string, object>;
        Assert.NotNull(ti);
        Assert.True(ti.Count > 0);
        TestOutput.WriteLine(DelimitedString.Create(ti, o => o.Delimiter = Environment.NewLine));
        var pi = ed.Evidence.Single(pair => pair.Key == "Process").Value as IDictionary<string, object>;
        Assert.NotNull(pi);
        Assert.True(pi.Count > 0);
        TestOutput.WriteLine(DelimitedString.Create(pi, o => o.Delimiter = Environment.NewLine));
        var ei = ed.Evidence.Single(pair => pair.Key == "Environment").Value as IDictionary<string, object>;
        Assert.NotNull(ei);
        Assert.True(ei.Count > 0);
        TestOutput.WriteLine(DelimitedString.Create(ei, o => o.Delimiter = Environment.NewLine));
    }

    [Fact]
    public void ShouldCreateDefaultInstance()
    {
        var hu = new Uri("https://docs.cuemon.net/api/dotnet/Cuemon.Diagnostics.ExceptionDescriptor.html");
        var ex = new InvalidOperationException("Invalid operation test.");
        var ed = new ExceptionDescriptor(ex, "Invalid Operation Exception", "Developer did something unexpected.", hu);

        Assert.Equal(ex.ToString(), ed.ToString());
        Assert.Equal("InvalidOperationException", ed.Code);
        Assert.Equal("Developer did something unexpected.", ed.Message);
        Assert.Equal(hu, ed.HelpLink);
        Assert.Equal(0, ed.Evidence.Count);
        Assert.Equal(ex, ed.Failure);
    }

    [Fact]
    public void ShouldCreateDefaultInstanceWithPostInitializeUsingCustomImplementedExceptionDescriptorAttribute()
    {
        var sc = new SomeClass();
        var hu = new Uri("https://docs.cuemon.net/api/dotnet/Cuemon.Diagnostics.ExceptionDescriptor.html");
        var ex = Assert.Throws<ArgumentNullException>(() => sc.StringToArray(null));
        var ed = new ExceptionDescriptor(ex, "Not Null Exception", "Null is not allowed.", hu);

        Assert.StartsWith("Null is a no-go!", ex.Message);
        Assert.Contains("value", ex.Message);
        Assert.Equal(ex.ToString(), ed.ToString());
        Assert.Equal("NotNullException", ed.Code);
        Assert.Equal("Null is not allowed.", ed.Message);
        Assert.Equal(hu, ed.HelpLink);
        Assert.Equal(0, ed.Evidence.Count);
        Assert.Equal(ex, ed.Failure);

        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        ed.PostInitializeWith(sc.GetType().GetMethod("StringToArray").GetCustomAttribute<ArgumentNullExceptionDescriptorAttribute>());

        Assert.Equal("ArgumentNullException", ed.Code);
        Assert.NotEqual("The value cannot be null (none-resource).", ed.Message);
        Assert.Equal("Value cannot be null.", ed.Message);

        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("da-DK");

        ed.PostInitializeWith(sc.GetType().GetMethod("StringToArray").GetCustomAttribute<ArgumentNullExceptionDescriptorAttribute>());

        Assert.Equal("ArgumentNullException", ed.Code);
        Assert.NotEqual("Value cannot be null.", ed.Message);
        Assert.Equal("Null er ikke en gyldig værdi.", ed.Message);
    }

    [Fact]
    public void ShouldCreateDefaultInstanceWithPostInitializeUsingExceptionDescriptorAttributeWithLocalization()
    {
        var sc = new SomeClass();
        var hu = new Uri("https://docs.cuemon.net/api/dotnet/Cuemon.Diagnostics.ExceptionDescriptor.html");
        var ex = Assert.Throws<ArgumentNullException>(() => sc.Shuffle(null));
        var ed = new ExceptionDescriptor(ex, "Not Null Exception", "Null is not allowed.", hu);

        Assert.StartsWith("Null is a no-go!", ex.Message);
        Assert.Contains("value", ex.Message);
        Assert.Equal(ex.ToString(), ed.ToString());
        Assert.Equal("NotNullException", ed.Code);
        Assert.Equal("Null is not allowed.", ed.Message);
        Assert.Equal(hu, ed.HelpLink);
        Assert.Equal(0, ed.Evidence.Count);
        Assert.Equal(ex, ed.Failure);

        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        ed.PostInitializeWith(sc.GetType().GetMethod("Shuffle").GetCustomAttribute<ExceptionDescriptorAttribute>());

        Assert.Equal("ArgumentNullException", ed.Code);
        Assert.NotEqual("The value cannot be null (none-resource).", ed.Message);
        Assert.Equal("Value cannot be null.", ed.Message);

        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("da-DK");

        ed.PostInitializeWith(sc.GetType().GetMethod("Shuffle").GetCustomAttribute<ExceptionDescriptorAttribute>());

        Assert.Equal("ArgumentNullException", ed.Code);
        Assert.NotEqual("Value cannot be null.", ed.Message);
        Assert.Equal("Null er ikke en gyldig værdi.", ed.Message);
    }

    [Fact]
    public void ShouldCreateDefaultInstanceWithPostInitializeUsingExceptionDescriptorAttributeWithoutLocalization()
    {
        var sc = new SomeClass();
        var hu = new Uri("https://docs.cuemon.net/api/dotnet/Cuemon.Diagnostics.ExceptionDescriptor.html");
        var ex = Assert.Throws<ArgumentNullException>(() => sc.ShuffleNoLoc(null));
        var ed = new ExceptionDescriptor(ex, "Not Null Exception", "Null is not allowed.", hu);

        Assert.StartsWith("Null is a no-go!", ex.Message);
        Assert.Contains("value", ex.Message);
        Assert.Equal(ex.ToString(), ed.ToString());
        Assert.Equal("NotNullException", ed.Code);
        Assert.Equal("Null is not allowed.", ed.Message);
        Assert.Equal(hu, ed.HelpLink);
        Assert.Equal(0, ed.Evidence.Count);
        Assert.Equal(ex, ed.Failure);

        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        ed.PostInitializeWith(sc.GetType().GetMethod("ShuffleNoLoc").GetCustomAttribute<ExceptionDescriptorAttribute>());

        Assert.Equal("ArgumentNullException", ed.Code);
        Assert.Equal("The value cannot be null (none-resource).", ed.Message);
        Assert.NotEqual("Value cannot be null.", ed.Message);

        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("da-DK");

        ed.PostInitializeWith(sc.GetType().GetMethod("ShuffleNoLoc").GetCustomAttribute<ExceptionDescriptorAttribute>());

        Assert.Equal("ArgumentNullException", ed.Code);
        Assert.Equal("The value cannot be null (none-resource).", ed.Message);
        Assert.NotEqual("Null er ikke en gyldig værdi.", ed.Message);
    }
}
