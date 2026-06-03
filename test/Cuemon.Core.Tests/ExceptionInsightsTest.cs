using System;
using System.Reflection;
using Codebelt.Extensions.Xunit;
using Cuemon.Diagnostics;
using Xunit;

namespace Cuemon
{
    public class ExceptionInsightsTest : Test
    {
        public ExceptionInsightsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Embed_WithNullException_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ExceptionInsights.Embed<InvalidOperationException>(null));
        }

        [Fact]
        public void Embed_WithoutThrowerOrSnapshots_AddsFiveInsightSegments()
        {
            var sut = ExceptionInsights.Embed(new InvalidOperationException("boom"));

            Assert.True(sut.Data.Contains(ExceptionInsights.Key));
            Assert.Equal(5, ((string)sut.Data[ExceptionInsights.Key]).Split('.').Length);
        }

        [Fact]
        public void Embed_WithThreadAndEnvironmentSnapshots_ExtractsOnlyRequestedEvidence()
        {
            var sut = ExceptionInsights.Embed(new InvalidOperationException("boom"), MethodBase.GetCurrentMethod(), null, SystemSnapshots.CaptureThreadInfo | SystemSnapshots.CaptureEnvironmentInfo);
            var descriptor = ExceptionDescriptor.Extract(sut);

            Assert.Equal(3, descriptor.Evidence.Count);
            Assert.Contains("Thrower", descriptor.Evidence.Keys);
            Assert.Contains("Thread", descriptor.Evidence.Keys);
            Assert.Contains("Environment", descriptor.Evidence.Keys);
            Assert.DoesNotContain("Process", descriptor.Evidence.Keys);
        }

        [Fact]
        public void SystemSnapshots_CaptureAll_ShouldContainAllIndividualFlags()
        {
            var sut = SystemSnapshots.CaptureAll;

            Assert.Equal(SystemSnapshots.CaptureThreadInfo | SystemSnapshots.CaptureProcessInfo | SystemSnapshots.CaptureEnvironmentInfo, sut);
            Assert.True(sut.HasFlag(SystemSnapshots.CaptureThreadInfo));
            Assert.True(sut.HasFlag(SystemSnapshots.CaptureProcessInfo));
            Assert.True(sut.HasFlag(SystemSnapshots.CaptureEnvironmentInfo));
        }
    }
}
