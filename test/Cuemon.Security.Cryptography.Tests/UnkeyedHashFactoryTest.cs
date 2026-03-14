using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security.Cryptography
{
    public class UnkeyedHashFactoryTest : Test
    {
        public UnkeyedHashFactoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CreateCryptoSha512256_ShouldBeValidHashResult()
        {
            var h = UnkeyedHashFactory.CreateCryptoSha512Slash256();
            Assert.Equal("cdf1cc0effe26ecc0c13758f7b4a48e000615df241284185c39eb05d355bb9c8", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("d48b2aa4a50d1c3e324a1a762d3b2165244661ef80e004dd3669a77e02c489d8", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("e41c9660b04714cdf7249f0fd6e6c5556f54a7e04d299958b69a877e0fada2fb", h.ComputeHash(Guid.Empty.ToByteArray()).ToHexadecimalString());
            Assert.Equal("0ac561fac838104e3f2e4ad107b4bee3e938bf15f2b15f009ccccd61a913f017", h.ComputeHash("hello world").ToHexadecimalString());
        }

        [Fact]
        public void CreateCryptoSha512_ShouldBeValidHashResult()
        {
            var h = UnkeyedHashFactory.CreateCryptoSha512();
            Assert.Equal("1e07be23c26a86ea37ea810c8ec7809352515a970e9253c26f536cfc7a9996c45c8370583e0a78fa4a90041d71a4ceab7423f19c71b9d5a3e01249f0bebd5894", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("bb96c2fc40d2d54617d6f276febe571f623a8dadf0b734855299b0e107fda32cf6b69f2da32b36445d73690b93cbd0f7bfc20e0f7f28553d2a4428f23b716e90", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("0b6cbac838dfe7f47ea1bd0df00ec282fdf45510c92161072ccfb84035390c4da743d9c3b954eaa1b0f86fc9861b23cc6c8667ab232c11c686432ebb5c8c3f27", h.ComputeHash(Guid.Empty.ToByteArray()).ToHexadecimalString());
        }

        [Fact]
        public void CreateCryptoSha384_ShouldBeValidHashResult()
        {
            var h = UnkeyedHashFactory.CreateCryptoSha384();
            Assert.Equal("1761336e3f7cbfe51deb137f026f89e01a448e3b1fafa64039c1464ee8732f11a5341a6f41e0c202294736ed64db1a84", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("90ae531f24e48697904a4d0286f354c50a350ebb6c2b9efcb22f71c96ceaeffc11c6095e9ca0df0ec30bf685dcf2e5e5", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("d3b3f28933c5c91daa6a355aef5e09252e9c78baf751db717a2fde4b88e962a55e740acd869a3057b6020ad68e650a5f", h.ComputeHash(DayOfWeek.Friday).ToHexadecimalString());
        }

        [Fact]
        public void CreateCryptoSha256_ShouldBeValidHashResult()
        {
            var h = UnkeyedHashFactory.CreateCryptoSha256();
            Assert.Equal("db4bfcbd4da0cd85a60c3c37d3fbd8805c77f15fc6b1fdfe614ee0a7c8fdb4c0", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("db4bfcbd4da0cd85a60c3c37d3fbd8805c77f15fc6b1fdfe614ee0a7c8fdb4c0", h.ComputeHash(Decorator.Enclose(Alphanumeric.LettersAndNumbers).ToStream()).ToHexadecimalString());
            Assert.Equal("84d89877f0d4041efb6bf91a16f0248f2fd573e6af05c19f96bedb9f882f7882", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
#if NET10_0_OR_GREATER
            Assert.Equal("53ab3a50f51855beeae9721ab68656312c7f105b9b34bbfa97875dbfda72dbc6", h.ComputeHash(DateTime.UnixEpoch).ToHexadecimalString());
#endif
            Assert.Equal("1f1a24c833be74a0f4f99007aa70a51e2456e41f745a5628721ea2b8e1c07641", h.ComputeHash(213, "fdfsfsf", 9999).ToHexadecimalString());
        }

        [Fact]
        public void CreateCryptoSha1_ShouldBeValidHashResult()
        {
            var h = UnkeyedHashFactory.CreateCryptoSha1();
            Assert.Equal("761c457bf73b14d27e9e9265c46f4b4dda11f940", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("761c457bf73b14d27e9e9265c46f4b4dda11f940", h.ComputeHash(Decorator.Enclose(Alphanumeric.LettersAndNumbers).ToStream()).ToHexadecimalString());
            Assert.Equal("87acec17cd9dcd20a716cc2cf67417b71c8a7016", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("485bad9954c874513b8ff7b6d5ea459ac65dd075", h.ComputeHash(decimal.MinValue).ToHexadecimalString());
            Assert.Equal("ec9a4348d9ffcb19403f5b90e9eefe4cedcd6ee1", h.ComputeHash(43402934324).ToHexadecimalString());
        }

        [Fact]
        public void CreateCryptoMd5_LittleEndian_ShouldBeValidHashResult()
        {
            var h = UnkeyedHashFactory.CreateCryptoMd5(o => o.ByteOrder = Endianness.LittleEndian);
            Assert.Equal("193112cee1d1a35660f02e95a28bfea2", h.ComputeHash(32131535).ToHexadecimalString());
            Assert.Equal("d174ab98d277d9f5a5611c2c9f419d9f", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("d174ab98d277d9f5a5611c2c9f419d9f", h.ComputeHash(Decorator.Enclose(Alphanumeric.LettersAndNumbers).ToStream()).ToHexadecimalString());
            Assert.Equal("781e5e245d69b566979b86e28d23f2c7", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("612f309087565745ca61c53fcaf6fa7d", h.ComputeHash(212).ToHexadecimalString());
        }

        [Fact]
        public void CreateCryptoMd5_BigEndian_ShouldBeValidHashResult()
        {
            var h = UnkeyedHashFactory.CreateCryptoMd5(o => o.ByteOrder = Endianness.BigEndian);
            Assert.Equal("d52654efa276b2ab12ca067dccaf953f", h.ComputeHash(32131535).ToHexadecimalString());
            Assert.Equal("d174ab98d277d9f5a5611c2c9f419d9f", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
            Assert.Equal("d174ab98d277d9f5a5611c2c9f419d9f", h.ComputeHash(Decorator.Enclose(Alphanumeric.LettersAndNumbers).ToStream()).ToHexadecimalString());
            Assert.Equal("781e5e245d69b566979b86e28d23f2c7", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
            Assert.Equal("fb7b8d4f62e2be708ceca1114b439d5d", h.ComputeHash(212).ToHexadecimalString());
        }
    }
}