using System.Text;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security
{
    public class HashFactoryTest : Test
    {
        public HashFactoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CreateFnv64_Fnv1_ShouldHaveSizeOf64Bits()
        {
            var s1 = "957-KEY";
            var s2 = "958-KEY";
            var hf = HashFactory.CreateFnv64(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1a);

            TestOutput.WriteLine(hf.ComputeHash(s1).ToHexadecimalString());
            TestOutput.WriteLine(hf.ComputeHash(s2).ToHexadecimalString());
        }

        [Fact]
        public void CreateCrc_Crc64_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc64);
            Assert.Equal("5CA18585B92C58B9", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc64GoIso_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc64GoIso);
            Assert.Equal("8400E49282258B59", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc64We_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc64We);
            Assert.Equal("A36DA8F71E78B6FB", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc64Xz_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc64Xz);
            Assert.Equal("0305BFE116B75626", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc32_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc();
            Assert.Equal("1fc2e6d2", h.ComputeHash(Alphanumeric.LettersAndNumbers, o => o.Encoding = Encoding.ASCII).ToHexadecimalString());
            Assert.Equal("a684c7c6", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("aa11d1c3", h.ComputeHash("what-a-feeling-#-¤-%-!-cover-from-dj-bobo-128379539285784289529893278278173981247983251311").ToHexadecimalString());

            h = HashFactory.CreateCrc32(0x4C11DB7, 0xFFFFFFFF, 0xFFFFFFFF, o =>
            {
                o.ReflectInput = false;
                o.ReflectOutput = true;
            });
            Assert.Equal("11842e05", h.ComputeHash(Alphanumeric.LettersAndNumbers, o => o.Encoding = Encoding.ASCII).ToHexadecimalString());
            Assert.Equal("07270d69", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("4475e1b8", h.ComputeHash("what-a-feeling-#-¤-%-!-cover-from-dj-bobo-128379539285784289529893278278173981247983251311").ToHexadecimalString());

            h = HashFactory.CreateCrc32(0x4C11DB7, 0xFFFFFFFF, 0xFFFFFFFF, o =>
            {
                o.ReflectInput = true;
                o.ReflectOutput = false;
            });
            Assert.Equal("4b6743f8", h.ComputeHash(Alphanumeric.LettersAndNumbers, o => o.Encoding = Encoding.ASCII).ToHexadecimalString());
            Assert.Equal("63e32165", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("c38b8855", h.ComputeHash("what-a-feeling-#-¤-%-!-cover-from-dj-bobo-128379539285784289529893278278173981247983251311").ToHexadecimalString());
        }

        [Fact]
        public void CreateCrc_Crc32Bzip2_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc32Bzip2);
            Assert.Equal("a0742188", h.ComputeHash(Alphanumeric.LettersAndNumbers, o => o.Encoding = Encoding.ASCII).ToHexadecimalString());
            Assert.Equal("96b0e4e0", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("1d87ae22", h.ComputeHash("what-a-feeling-#-¤-%-!-cover-from-dj-bobo-128379539285784289529893278278173981247983251311").ToHexadecimalString());
        }

        [Fact]
        public void CreateCrc_Crc32C_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc32C);
            Assert.Equal("A245D57D", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc32D_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc32D);
            Assert.Equal("17578C36", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc32Mpeg2_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc32Mpeg2);
            Assert.Equal("5F8BDE77", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc32Posix_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc32Posix);
            Assert.Equal("D96EF8CF", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc32Q_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc32Q);
            Assert.Equal("071381D3", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc32Jamcrc_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc32Jamcrc);
            Assert.Equal("E03D192D", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc32Xfer_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc32Xfer);
            Assert.Equal("E0334EE2", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc32Autosar_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc32Autosar);
            Assert.Equal("ADD7278E", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateCrc_Crc32CdRomEdc_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateCrc(CyclicRedundancyCheckAlgorithm.Crc32CdRomEdc);
            Assert.Equal("D9B8B5E9", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString().ToUpper());
        }

        [Fact]
        public void CreateFnv32_Fnv1_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv32(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1);
            Assert.Equal("6792412c", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("050c5d1f", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv64_Fnv1_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv64(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1);
            Assert.Equal("93466e18b44cc858", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("c3f080735df30b0c", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("af63bd4c8601b7df", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv128_Fnv1_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv128(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1);
            Assert.Equal("a9c6ce956469c801ebd62b0bbf9d6bf0", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("eea7dd269d8d5af5f47ac00d5eb7f714", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("d228cb69101a8caf78912b704e4a147f", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv256_Fnv1_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv256(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1);
            Assert.Equal("3ea64656b211bff487a9b62f18ba64392c7068ddf241baf2c41a47514a0ffc48", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("063cfa6dd23b329ac23cc7d1120d59e6a397a0c32d3ba388af79d1d72628225c", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("63323fb0f35303ec28dc561d0a33bdfa4de6a99b7266494f6183b2716811387f", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv512_Fnv1_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv512(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1);
            Assert.Equal("593826b252ba64905c4832380622bd2c4e177cf0fd7cb9cd72f2da0638ec7fc82beaa91153430a76042dffa611435e70bd754d44aa59e2c93ccb0b926efc7464", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("fa7eb91efb6464118a735adc66e062a777fa3da6e3d7d3e73728da570c1fafc3d06e4dd9534a9fd4a52c438bd21169834ae60d20834d4f408192203a729b3ef0", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("e43a992dc8fc5ad7de493e3d696d6f85d64326ec28000000000000000011986f90c2532caf5be7d88291baa894a395225328b196bd6a8a643fe12cd87b282bbf", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv1024_Fnv1_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv1024(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1);
            Assert.Equal("70e427242b62d481df8f97b5a7c389f5f6df3457fda072841eb0ac24759648a39784a0ab922c4730b68efa7d0980de290e79de582d88c97e17c953592b9b70ce6dca5ccd19cd93182254abfe9ed6face84979b6793e44e46621ad88c76744b1296ed3934a03e443ce593f1d3dd137dcba2ac2c5edb2cc9c7353111c2327224ca", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("c801f8e08ae91b180b98dd7d9f65ceb687ca86358c6905f60a7d1014c182b04f441590d012afb5871d0f57000000000000000000000000000000000000000000000000000000000000000000000000000000018045149ade1c79abe3b709a406f7d9205169bec59b126140bcb96f9d5d3e2ea91e0b2b52fa8d2d0d70ecdaeab2", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("000000000000000098d7c19fbce653df221b9f717d3490ff95ca87fdaef30d1b823372f85b24a372f50e380000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000007685cd81a491dbccc21ad06648d09a5c8cf5a78482054e91470b33dde77252caef66597", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv32_Fnv1a_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv32(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1a);
            Assert.Equal("9b2bce4e", h.ComputeHash(Decorator.Enclose(Alphanumeric.LettersAndNumbers).ToStream()).ToHexadecimalString());
            Assert.Equal("f9808ff2", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("050c5d1f", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv64_Fnv1a_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv64(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1a);
            Assert.Equal("c35365f271d8c80e", h.ComputeHash(Decorator.Enclose(Alphanumeric.LettersAndNumbers).ToStream()).ToHexadecimalString());
            Assert.Equal("50c0aafd8b4330b2", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("af63bd4c8601b7df", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv128_Fnv1a_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv128(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1a);
            Assert.Equal("849d079e7ac91227b14ecbd5246bb93e", h.ComputeHash(Decorator.Enclose(Alphanumeric.LettersAndNumbers).ToStream()).ToHexadecimalString());
            Assert.Equal("0b7df68bb60da90266201c9330963d52", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("d228cb69101a8caf78912b704e4a147f", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv256_Fnv1a_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv256(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1a);
            Assert.Equal("16ce73ab46b29992f1d12aff42c3f70fb4d178dfc96513ef1ea856751d21d40e", h.ComputeHash(Decorator.Enclose(Alphanumeric.LettersAndNumbers).ToStream()).ToHexadecimalString());
            Assert.Equal("e2bdd7f76e05160bc86d1ed1120d59e6a397a0c3284fbcafaaa8566d5f79ebd2", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("63323fb0f35303ec28dc561d0a33bdfa4de6a99b7266494f6183b2716811387f", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv512_Fnv1a_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv512(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1a);
            Assert.Equal("d67158511cb86ff6a655a5cc30b6957faa2d4a113d88d7d4782641fbb3f1b034fa859ea95e767c2b93479b4e38977c409dab6cce192179923d0d3ffc10a7a076", h.ComputeHash(Decorator.Enclose(Alphanumeric.LettersAndNumbers).ToStream()).ToHexadecimalString());
            Assert.Equal("fa7eb91efb6464118a73639a14a22d3284c5433232d7d3e73728da570c1fafc3d06e4dd9534a9fd4a52c438bd21169834ae60d2084794a3e4e815976ca1e01f2", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("e43a992dc8fc5ad7de493e3d696d6f85d64326ec28000000000000000011986f90c2532caf5be7d88291baa894a395225328b196bd6a8a643fe12cd87b282bbf", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv1024_Fnv1a_ShouldBeValidHashResult()
        {
            var h = HashFactory.CreateFnv1024(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1a);
            Assert.Equal("199c0ace56c5c33d8bce6f7cf4bc4b555e0fc3ae8d37c4b7384678a34d96ae8192825ae6bcda63dbb9e3417d0980de290e79de582d88c97e17c9535950c35f4f16d311bc66d1ac2892d59f7b0697257eba9fc1e3accbc85729218306b34996eedf99292c814e8a75f41ddc5a5b5177b6e60c0211ad8d8f78395c7c2d2c483e7e", h.ComputeHash(Decorator.Enclose(Alphanumeric.LettersAndNumbers).ToStream()).ToHexadecimalString());
            Assert.Equal("c801f8e08ae91b180b98dd7d9f65ceb687ca86358c6905f60a7d1014c182b04ee2ab1bd0066e9857a7f7de000000000000000000000000000000000000000000000000000000000000000000000000000000018045149ade1c79abe3b709a406f7d9205169bec59b126140bcb96f9d5d3e2ea91dfc0f40af8e7e3f25d14c3186", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("000000000000000098d7c19fbce653df221b9f717d3490ff95ca87fdaef30d1b823372f85b24a372f50e380000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000007685cd81a491dbccc21ad06648d09a5c8cf5a78482054e91470b33dde77252caef66597", h.ComputeHash(byte.MinValue).ToHexadecimalString());
        }

        // New tests to cover remaining factory overloads and switches
        [Fact]
        public void CreateFnv_Default_ShouldBeSameAsCreateFnv32()
        {
            var input = Alphanumeric.LettersAndNumbers;
            var a = HashFactory.CreateFnv();
            var b = HashFactory.CreateFnv32();
            Assert.Equal(a.ComputeHash(input).ToHexadecimalString(), b.ComputeHash(input).ToHexadecimalString());
        }

        [Fact]
        public void CreateFnv_Switch_ShouldReturnSpecificFactoryResults()
        {
            var input = Alphanumeric.Numbers;

            var h32 = HashFactory.CreateFnv(NonCryptoAlgorithm.Fnv32);
            Assert.Equal(HashFactory.CreateFnv32().ComputeHash(input).ToHexadecimalString(), h32.ComputeHash(input).ToHexadecimalString());

            var h64 = HashFactory.CreateFnv(NonCryptoAlgorithm.Fnv64);
            Assert.Equal(HashFactory.CreateFnv64().ComputeHash(input).ToHexadecimalString(), h64.ComputeHash(input).ToHexadecimalString());

            var h128 = HashFactory.CreateFnv(NonCryptoAlgorithm.Fnv128);
            Assert.Equal(HashFactory.CreateFnv128().ComputeHash(input).ToHexadecimalString(), h128.ComputeHash(input).ToHexadecimalString());

            var h256 = HashFactory.CreateFnv(NonCryptoAlgorithm.Fnv256);
            Assert.Equal(HashFactory.CreateFnv256().ComputeHash(input).ToHexadecimalString(), h256.ComputeHash(input).ToHexadecimalString());

            var h512 = HashFactory.CreateFnv(NonCryptoAlgorithm.Fnv512);
            Assert.Equal(HashFactory.CreateFnv512().ComputeHash(input).ToHexadecimalString(), h512.ComputeHash(input).ToHexadecimalString());

            var h1024 = HashFactory.CreateFnv(NonCryptoAlgorithm.Fnv1024);
            Assert.Equal(HashFactory.CreateFnv1024().ComputeHash(input).ToHexadecimalString(), h1024.ComputeHash(input).ToHexadecimalString());
        }

        [Fact]
        public void CreateCrc64_WithParameters_ShouldMatchCreateCrc64GoIso()
        {
            var input = Alphanumeric.LettersAndNumbers;
            var hParam = HashFactory.CreateCrc64(0x000000000000001B, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, o =>
            {
                o.ReflectInput = true;
                o.ReflectOutput = true;
            });
            var hPreset = HashFactory.CreateCrc64GoIso();
            Assert.Equal(hPreset.ComputeHash(input).ToHexadecimalString().ToUpper(), hParam.ComputeHash(input).ToHexadecimalString().ToUpper());
        }
    }
}