using System;
using Cuemon.Reflection;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Resilience
{
    public class TransientFaultExceptionTest : Test
    {
        public TransientFaultExceptionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldAssignEvidence()
        {
            var evidence = new TransientFaultEvidence(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(3), new MethodSignature("Cuemon.Resilience.TransientFaultExceptionTest", "Ctor_ShouldAssignEvidence", null, null));
            var sut = new TransientFaultException("Failure.", evidence);

            Assert.Equal("Failure.", sut.Message);
            Assert.Same(evidence, sut.Evidence);
            Assert.Null(sut.InnerException);
        }

        [Fact]
        public void Ctor_ShouldAssignInnerExceptionAndEvidence()
        {
            var inner = new ArithmeticException();
            var evidence = new TransientFaultEvidence(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(3), new MethodSignature("Cuemon.Resilience.TransientFaultExceptionTest", "Ctor_ShouldAssignInnerExceptionAndEvidence", null, null));
            var sut = new TransientFaultException("Failure.", inner, evidence);

            Assert.Equal("Failure.", sut.Message);
            Assert.Same(inner, sut.InnerException);
            Assert.Same(evidence, sut.Evidence);
        }

        [Fact]
        public void Ctor_ShouldThrowArgumentNullExceptionWhenEvidenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new TransientFaultException("Failure.", (TransientFaultEvidence)null));
            Assert.Throws<ArgumentNullException>(() => new TransientFaultException("Failure.", new ArithmeticException(), null));
        }

        [Fact]
        public void ToString_ShouldAppendEvidence()
        {
            var evidence = new TransientFaultEvidence(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(3), new MethodSignature("Cuemon.Resilience.TransientFaultExceptionTest", "ToString_ShouldAppendEvidence", null, null));
            var sut = new TransientFaultException("Failure.", evidence);

            Assert.Equal($"{typeof(TransientFaultException).FullName}: Failure. {evidence}", sut.ToString());
        }
    }
}
