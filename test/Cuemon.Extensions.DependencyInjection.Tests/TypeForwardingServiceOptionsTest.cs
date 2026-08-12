using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.Extensions.DependencyInjection;
public class ServiceOptionsTest : Test
{
    public ServiceOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ValidateOptions_ShouldHaveDefaultValues()
    {
        var sut = new ServiceOptions();

        Assert.Equal(ServiceLifetime.Transient, sut.Lifetime);
    }
}
