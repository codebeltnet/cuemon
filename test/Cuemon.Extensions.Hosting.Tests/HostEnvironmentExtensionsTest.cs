using Codebelt.Extensions.Xunit.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Cuemon.Extensions.Hosting
{
    public class HostEnvironmentExtensionsTest : HostTest<ManagedHostFixture>
    {
        public HostEnvironmentExtensionsTest(ManagedHostFixture hostFixture, ITestOutputHelper output = null) : base(hostFixture, output)
        {
        }

        protected override void ConfigureHost(IHostBuilder hb)
        {
            hb.UseEnvironment(Environments.LocalDevelopment);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
        }

        [Fact]
        public void IsLocalDevelopment_VerifyEnvironmentEqualsLocalDevelopment()
        {
#if NET10_0_OR_GREATER
            Assert.True(Environment.IsLocalDevelopment());
            Assert.False(Environment.IsProduction());
            Assert.False(Environment.IsStaging());
            Assert.False(Environment.IsDevelopment());
#else
            Assert.True(Environment.IsLocalDevelopment());
            Assert.False(Environment.IsProduction());
            Assert.False(Environment.IsStaging());
            Assert.False(Environment.IsDevelopment());
#endif
            TestOutput.WriteLine(Environment.EnvironmentName);
        }

        [Fact]
        public void IsLocalDevelopment_VerifyEnvironmentIsNonProduction()
        {
#if NET10_0_OR_GREATER
            Assert.True(Environment.IsNonProduction());
            Assert.False(Environment.IsProduction());
#else
            Assert.True(Environment.IsNonProduction());
            Assert.False(Environment.IsProduction());
#endif
            TestOutput.WriteLine(Environment.EnvironmentName);
        }
    }
}