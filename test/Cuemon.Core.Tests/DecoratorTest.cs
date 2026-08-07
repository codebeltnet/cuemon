using System;
using Cuemon.Assets;
using Xunit;

namespace Cuemon;
public class DecoratorTest
{
    [Fact]
    public void Enclose_ShouldIncludeNotifierAndFacebookAndTwitterAndSlack_WhenAllTrue()
    {
        var notifier = new Notifier();
        var decorator = Decorator.Enclose(notifier).Send(true, true, true);
        Assert.Contains("Facebook", decorator);
        Assert.Contains("Twitter", decorator);
        Assert.Contains("Slack", decorator);
        Assert.Contains("Notifier", decorator);
    }

    [Fact]
    public void Enclose_ShouldIncludeOnlyNotifier_WhenDefaultIsUsed()
    {
        var notifier = new Notifier();
        var decorator = Decorator.Enclose(notifier).Send();
        Assert.DoesNotContain("Facebook", decorator);
        Assert.DoesNotContain("Twitter", decorator);
        Assert.DoesNotContain("Slack", decorator);
        Assert.Contains("Notifier", decorator);
    }

    [Fact]
    public void Enclose_ShouldIncludeNotifierAndTwitter_WhenOptionalTwitterIsTrue()
    {
        var notifier = new Notifier();
        var decorator = Decorator.Enclose(notifier).Send(twitter: true);
        Assert.DoesNotContain("Facebook", decorator);
        Assert.Contains("Twitter", decorator);
        Assert.DoesNotContain("Slack", decorator);
        Assert.Contains("Notifier", decorator);
    }

    [Fact]
    public void Enclose_ShouldIncludeNotifierAndFacebook_WhenOptionalFacebookIsTrue()
    {
        var notifier = new Notifier();
        var decorator = Decorator.Enclose(notifier).Send(true);
        Assert.Contains("Facebook", decorator);
        Assert.DoesNotContain("Twitter", decorator);
        Assert.DoesNotContain("Slack", decorator);
        Assert.Contains("Notifier", decorator);
    }

    [Fact]
    public void Enclose_ShouldIncludeNotifierAndFacebook_WhenOptionalSlackIsTrue()
    {
        var notifier = new Notifier();
        var decorator = Decorator.Enclose(notifier).Send(slack: true);
        Assert.DoesNotContain("Facebook", decorator);
        Assert.DoesNotContain("Twitter", decorator);
        Assert.Contains("Slack", decorator);
        Assert.Contains("Notifier", decorator);
    }

    [Fact]
    public void Enclose_ShouldHaveReferenceToNotifier()
    {
        var notifier = new Notifier();
        var decorator = Decorator.Enclose(notifier);
        Assert.Same(notifier, decorator.Inner);
    }

    [Fact]
    public void Enclose_ShouldThrowArgumentNullException_WhenSourceIsNull()
    {
        Notifier notifier = null;
        var ex = Assert.Throws<ArgumentNullException>(() =>
        {
            Decorator.Enclose(notifier).Send();
        });
        Assert.Contains("Value cannot be null.", ex.Message);
        Assert.Equal("inner", ex.ParamName);
    }

    [Fact]
    public void EncloseToExpose_ShouldThrowArgumentNullException_WhenSourceIsNull()
    {
        Notifier notifier = null;
        var ex = Assert.Throws<ArgumentNullException>(() =>
        {
            Decorator.EncloseToExpose(notifier).Send();
        });
        Assert.Contains("Value cannot be null.", ex.Message);
        Assert.Equal(nameof(notifier), ex.ParamName);
    }
}
