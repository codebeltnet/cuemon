using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon
{
    public class DelimitedStringTest : Test
    {
        public DelimitedStringTest(ITestOutputHelper output) : base(output)
        {
        }


        [Fact]
        public void Split_ShouldSplitPreservingQualifierOfTypeDoubleQuote()
        {
            var s1 = "1999,Chevy,\"Venture \"\"Extended Edition, Very Large\"\"\",,5000.00";
            var s2 = "realm=\"unittest\", qop=\"auth, auth-int\", nonce=\"MjAyMC0xMC0yMCAyMzoxMzo0MFo6OWY1NmRjZTY0NWI3YjY5YjhlM2NlOTFhNDM2ZWI2ZGFiNDIxYzY5MjU4YzI1YTBkNDg1M2RkYTQ2NmRkOWJkNg==\"";

            var ds1 = DelimitedString.Split(s1);
            var ds2 = DelimitedString.Split(s2);

            TestOutput.WriteLine("---- ds1 -----");

            TestOutput.WriteLine(s1);

            TestOutput.WriteLine("---- ds2 -----");

            TestOutput.WriteLine(s2);

            TestOutput.WriteLine("---- foreach sc in ds1 -----");

            foreach (var sc in ds1)
            {
                TestOutput.WriteLine(sc);
            }

            TestOutput.WriteLine("---- foreach sc in ds2 -----");

            foreach (var sc in ds2)
            {
                TestOutput.WriteLine(sc);
            }

            TestOutput.WriteLine(new string('-', 5));

            var j1 = DelimitedString.Create(ds1);
            var j2 = DelimitedString.Create(ds2);

            TestOutput.WriteLine(j1);

            Assert.Equal(s1, j1);
            Assert.Equal(s2, j2);

            Assert.True(ds1.Length == 5);
            Assert.True(ds2.Length == 3);
        }
    }
}
