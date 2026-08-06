using Cuemon.Assets;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon;
public class TweakerTest : Test
{
    public TweakerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Adjust_ShouldChangeValueUsingFunctionDelegate()
    {
        var sut1 = new ClampOptions();
        var sut2 = Tweaker.Adjust(sut1, co => new ClampOptions()
        {
            MaxConcurrentJobs = co.MaxConcurrentJobs
        });

        Assert.NotSame(sut1, sut2);
        Assert.Equal(10, sut1.MaxConcurrentJobs);
        Assert.Equal(10, sut2.MaxConcurrentJobs);
    }

    [Fact]
    public void Alter_ShouldChangeValueUsingDelegate()
    {
        var sut1 = new ClampOptions();
        var sut2 = Tweaker.Alter(sut1, co => co.MaxConcurrentJobs = 128);

        Assert.Same(sut1, sut2);
        Assert.Equal(128, sut1.MaxConcurrentJobs);
        Assert.Equal(128, sut2.MaxConcurrentJobs);
    }
}
