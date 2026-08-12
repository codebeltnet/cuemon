using System.Collections.Generic;
using System.IO;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Cuemon.Security.Cryptography;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Configuration;
public class AssemblyCacheBustingTest : Test
{
    public AssemblyCacheBustingTest(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [MemberData(nameof(GetAlgorithmOptions))]
    public void Ctor_ShouldUseDifferentAlgorithms(AssemblyCacheBustingOptions options)
    {
        var sut1 = new AssemblyCacheBusting(new OptionsWrapper<AssemblyCacheBustingOptions>(options));
        var sut3 = Generate.HashCode64(typeof(AssemblyCacheBustingTest).Assembly.Location);
        var sut4 = Generate.HashCode64(typeof(AssemblyCacheBustingTest).Assembly.FullName);
        var sut5 = UnkeyedHashFactory.CreateCrypto(options.Algorithm).ComputeHash(Convertible.GetBytes(sut3).Concat(Convertible.GetBytes(sut4)).ToArray()).ToHexadecimalString();

        TestOutput.WriteLine(sut1.Version);

        Assert.Equal(sut5, sut1.Version);
    }

    [Theory]
    [MemberData(nameof(GetAlgorithmOptionsWithStrongIntegrityEnabled))]
    public void Ctor_ShouldUseDifferentAlgorithmsWithStrongIntegrity(AssemblyCacheBustingOptions options)
    {
        var sut1 = new AssemblyCacheBusting(new OptionsWrapper<AssemblyCacheBustingOptions>(options));
        var sut2 = File.ReadAllBytes(typeof(AssemblyCacheBustingTest).Assembly.Location).Concat(Convertible.GetBytes(Generate.HashCode64(typeof(AssemblyCacheBustingTest).Assembly.FullName))).ToArray();
        var sut3 = UnkeyedHashFactory.CreateCrypto(options.Algorithm).ComputeHash(sut2).ToHexadecimalString();

        TestOutput.WriteLine(sut1.Version);

        Assert.Equal(sut3, sut1.Version);
    }

    public static IEnumerable<object[]> GetAlgorithmOptions()
    {
        var parameters = new List<object[]>()
        {
            new object[]
            {
                new AssemblyCacheBustingOptions()
                {
                    Assembly = typeof(AssemblyCacheBustingTest).Assembly
                }
            },
            new object[]
            {
                new AssemblyCacheBustingOptions()
                {
                    Assembly = typeof(AssemblyCacheBustingTest).Assembly,
                    Algorithm = UnkeyedCryptoAlgorithm.Sha1
                }
            },
            new object[]
            {
                new AssemblyCacheBustingOptions()
                {
                    Assembly = typeof(AssemblyCacheBustingTest).Assembly,
                    Algorithm = UnkeyedCryptoAlgorithm.Sha256
                }
            },
            new object[]
            {
                new AssemblyCacheBustingOptions()
                {
                    Assembly = typeof(AssemblyCacheBustingTest).Assembly,
                    Algorithm = UnkeyedCryptoAlgorithm.Sha384
                }
            },
            new object[]
            {
                new AssemblyCacheBustingOptions()
                {
                    Assembly = typeof(AssemblyCacheBustingTest).Assembly,
                    Algorithm = UnkeyedCryptoAlgorithm.Sha512
                }
            }
        };

        return parameters;
    }

    public static IEnumerable<object[]> GetAlgorithmOptionsWithStrongIntegrityEnabled()
    {
        var parameters = new List<object[]>()
        {
            new object[]
            {
                new AssemblyCacheBustingOptions()
                {
                    Assembly = typeof(AssemblyCacheBustingTest).Assembly,
                    ReadByteForByteChecksum = true
                }
            },
            new object[]
            {
                new AssemblyCacheBustingOptions()
                {
                    Assembly = typeof(AssemblyCacheBustingTest).Assembly,
                    Algorithm = UnkeyedCryptoAlgorithm.Sha1,
                    ReadByteForByteChecksum = true
                }
            },
            new object[]
            {
                new AssemblyCacheBustingOptions()
                {
                    Assembly = typeof(AssemblyCacheBustingTest).Assembly,
                    Algorithm = UnkeyedCryptoAlgorithm.Sha256,
                    ReadByteForByteChecksum = true
                }
            },
            new object[]
            {
                new AssemblyCacheBustingOptions()
                {
                    Assembly = typeof(AssemblyCacheBustingTest).Assembly,
                    Algorithm = UnkeyedCryptoAlgorithm.Sha384,
                    ReadByteForByteChecksum = true
                }
            },
            new object[]
            {
                new AssemblyCacheBustingOptions()
                {
                    Assembly = typeof(AssemblyCacheBustingTest).Assembly,
                    Algorithm = UnkeyedCryptoAlgorithm.Sha512,
                    ReadByteForByteChecksum = true
                }
            }
        };

        return parameters;
    }
}
