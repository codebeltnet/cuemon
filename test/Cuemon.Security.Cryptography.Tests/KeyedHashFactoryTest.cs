using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security.Cryptography;
public class KeyedHashFactoryTest : Test
{
    public KeyedHashFactoryTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CreateHmacCryptoMd5_ShouldBeValidHashResult()
    {
        var h = KeyedHashFactory.CreateHmacCryptoMd5(Decorator.Enclose("unittest").ToByteArray());
        Assert.Equal("d0ee1decd115feac4608976f28e19e10", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
        Assert.Equal("34bdd10e5c71eb6860294d19f2db8233", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
        Assert.Equal("70adffcc026ac757db66de1bbb005e04", h.ComputeHash("what-a-feeling-#-¤-%-!-cover-from-dj-bobo-128379539285784289529893278278173981247983251311").ToHexadecimalString());
    }

    [Fact]
    public void CreateHmacCryptoSha1_ShouldBeValidHashResult()
    {
        var h = KeyedHashFactory.CreateHmacCryptoSha1(Decorator.Enclose("unittest").ToByteArray());
        Assert.Equal("0c7d9d4461c66d2d6eafef7211689abd20b28b39", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
        Assert.Equal("62ea258e7e4d0522fe5e2b06e1ab600051c542c4", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
        Assert.Equal("bae9d2e09c9b5f65f9a7a3c5a55748b180ee65c1", h.ComputeHash("what-a-feeling-#-¤-%-!-cover-from-dj-bobo-128379539285784289529893278278173981247983251311").ToHexadecimalString());
    }

    [Fact]
    public void CreateHmacCryptoSha256_ShouldBeValidHashResult()
    {
        var h = KeyedHashFactory.CreateHmacCryptoSha256(Decorator.Enclose("unittest").ToByteArray());
        Assert.Equal("c06272f221bcbfc6783c059e85d7ecdb412e80f2c2b704c27885d64d984d95e3", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
        Assert.Equal("1fd4ff25f71fa49435bc6996bbbaeeb9abe2f47ed3256b8a9f44b8eea49a23b5", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
        Assert.Equal("38367544d02183414967e4669cd298ca6d644db6c6bfa903afae497c6ab300fa", h.ComputeHash("what-a-feeling-#-¤-%-!-cover-from-dj-bobo-128379539285784289529893278278173981247983251311").ToHexadecimalString());
    }

    [Fact]
    public void CreateHmacCryptoSha384_ShouldBeValidHashResult()
    {
        var h = KeyedHashFactory.CreateHmacCryptoSha384(Decorator.Enclose("unittest").ToByteArray());
        Assert.Equal("87d90ce62441fd39f81d327fb4d3a9902d80a0d8312a961fdfc9b1bcf756afd77f0f0596d30a868a3cda1c49abf1a3ee", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
        Assert.Equal("7a90151a628de76431d11cf01200a7213f0dc491f7677aafbcd232e512521ca1ac3771d2b6204fa1ab6b54cf085ba2d3", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
        Assert.Equal("3b00f1f207819422761a22b918c69a3e16ae0d5268c01ad331311184b6a809b927b324f75d8cb8f1cee0f849382558bd", h.ComputeHash("what-a-feeling-#-¤-%-!-cover-from-dj-bobo-128379539285784289529893278278173981247983251311").ToHexadecimalString());
    }

    [Fact]
    public void CreateHmacCryptoSha512_ShouldBeValidHashResult()
    {
        var h = KeyedHashFactory.CreateHmacCryptoSha512(Decorator.Enclose("unittest").ToByteArray());
        Assert.Equal("db0087dde1ad907c11037243bf700a9f08430899edfc19fe1fadb7d4e6846182fa0100762cf42c64828f1bfe41493275b98f5c0c1a80300656e0d97f9f1d892e", h.ComputeHash(Alphanumeric.LettersAndNumbers).ToHexadecimalString());
        Assert.Equal("7b674e1138c2ee91828fcf5318d356e72a9c9e4e533234181640b0d1728f305db656bb7e57c95e101c0efbed9290454ef0c37f514c8dfb85f003c483bf0f236a", h.ComputeHash(Alphanumeric.Numbers).ToHexadecimalString());
        Assert.Equal("33520bc89652184cf5a17600a78a513db44a0b7eb1fb001525ab0e77c94afcb45b7878b259cd37896ddbbc5dff76d2cbb8f57eaf44c4f4aac77695fe8fe28992", h.ComputeHash("what-a-feeling-#-¤-%-!-cover-from-dj-bobo-128379539285784289529893278278173981247983251311").ToHexadecimalString());
    }
}
