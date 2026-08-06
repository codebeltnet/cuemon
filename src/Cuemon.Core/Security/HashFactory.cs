using System;

namespace Cuemon.Security;
/// <summary>
/// Provides access to factory methods for creating and configuring <see cref="Hash"/> instances.
/// </summary>
public static class HashFactory
{
    /// <summary>
    /// Creates an instance of a non-cryptographic implementation that derives from <see cref="FowlerNollVoHash"/> with the specified <paramref name="algorithm"/>. Default is <see cref="FowlerNollVo32"/> using <see cref="FowlerNollVoAlgorithm.Fnv1a"/>.
    /// </summary>
    /// <param name="algorithm">The <see cref="NonCryptoAlgorithm"/> that defines the non-cryptographic implementation. Default is <see cref="NonCryptoAlgorithm.Fnv32"/>.</param>
    /// <param name="setup">The <see cref="FowlerNollVoOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of the by parameter specified <paramref name="algorithm"/>.</returns>
    public static Hash CreateFnv(NonCryptoAlgorithm algorithm = default, Action<FowlerNollVoOptions> setup = null)
    {
        switch (algorithm)
        {
            case NonCryptoAlgorithm.Fnv64:
                return CreateFnv64(setup);
            case NonCryptoAlgorithm.Fnv128:
                return CreateFnv128(setup);
            case NonCryptoAlgorithm.Fnv256:
                return CreateFnv256(setup);
            case NonCryptoAlgorithm.Fnv512:
                return CreateFnv512(setup);
            case NonCryptoAlgorithm.Fnv1024:
                return CreateFnv1024(setup);
            default:
                return CreateFnv32(setup);
        }
    }

    /// <summary>
    /// Creates an instance of <see cref="FowlerNollVo32"/>.
    /// </summary>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="FowlerNollVo32"/>.</returns>
    public static Hash CreateFnv32(Action<FowlerNollVoOptions> setup = null)
    {
        return new FowlerNollVo32(setup);
    }

    /// <summary>
    /// Creates an instance of <see cref="FowlerNollVo64"/>.
    /// </summary>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="FowlerNollVo64"/>.</returns>
    public static Hash CreateFnv64(Action<FowlerNollVoOptions> setup = null)
    {
        return new FowlerNollVo64(setup);
    }

    /// <summary>
    /// Creates an instance of <see cref="FowlerNollVo128"/>.
    /// </summary>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="FowlerNollVo128"/>.</returns>
    public static Hash CreateFnv128(Action<FowlerNollVoOptions> setup = null)
    {
        return new FowlerNollVo128(setup);
    }

    /// <summary>
    /// Creates an instance of <see cref="FowlerNollVo256"/>.
    /// </summary>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="FowlerNollVo256"/>.</returns>
    public static Hash CreateFnv256(Action<FowlerNollVoOptions> setup = null)
    {
        return new FowlerNollVo256(setup);
    }

    /// <summary>
    /// Creates an instance of <see cref="FowlerNollVo512"/>.
    /// </summary>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="FowlerNollVo512"/>.</returns>
    public static Hash CreateFnv512(Action<FowlerNollVoOptions> setup = null)
    {
        return new FowlerNollVo512(setup);
    }

    /// <summary>
    /// Creates an instance of <see cref="FowlerNollVo1024"/>.
    /// </summary>
    /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="FowlerNollVo1024"/>.</returns>
    public static Hash CreateFnv1024(Action<FowlerNollVoOptions> setup = null)
    {
        return new FowlerNollVo1024(setup);
    }

    /// <summary>
    /// Creates an instance of a cyclic redundancy check implementation that derives from <see cref="CyclicRedundancyCheck"/> with the specified <paramref name="algorithm"/>. Default is <see cref="CyclicRedundancyCheck32"/> using <see cref="CyclicRedundancyCheckAlgorithm.Crc32"/>.
    /// </summary>
    /// <param name="algorithm">The <see cref="CyclicRedundancyCheckAlgorithm"/> that defines the cyclic redundancy check implementation. Default is <see cref="CyclicRedundancyCheckAlgorithm.Crc32"/>.</param>
    /// <returns>A <see cref="Hash"/> implementation of the by parameter specified <paramref name="algorithm"/>.</returns>
    public static Hash CreateCrc(CyclicRedundancyCheckAlgorithm algorithm = default)
    {
        switch (algorithm)
        {
            case CyclicRedundancyCheckAlgorithm.Crc32Autosar:
                return CreateCrc32Autosar();
            case CyclicRedundancyCheckAlgorithm.Crc32Bzip2:
                return CreateCrc32Bzip2();
            case CyclicRedundancyCheckAlgorithm.Crc32C:
                return CreateCrc32C();
            case CyclicRedundancyCheckAlgorithm.Crc32CdRomEdc:
                return CreateCrc32CdRomEdc();
            case CyclicRedundancyCheckAlgorithm.Crc32D:
                return CreateCrc32D();
            case CyclicRedundancyCheckAlgorithm.Crc32Jamcrc:
                return CreateCrc32Jamcrc();
            case CyclicRedundancyCheckAlgorithm.Crc32Mpeg2:
                return CreateCrc32Mpeg2();
            case CyclicRedundancyCheckAlgorithm.Crc32Posix:
                return CreateCrc32Posix();
            case CyclicRedundancyCheckAlgorithm.Crc32Q:
                return CreateCrc32Q();
            case CyclicRedundancyCheckAlgorithm.Crc32Xfer:
                return CreateCrc32Xfer();
            case CyclicRedundancyCheckAlgorithm.Crc64:
                return CreateCrc64();
            case CyclicRedundancyCheckAlgorithm.Crc64GoIso:
                return CreateCrc64GoIso();
            case CyclicRedundancyCheckAlgorithm.Crc64We:
                return CreateCrc64We();
            case CyclicRedundancyCheckAlgorithm.Crc64Xz:
                return CreateCrc64Xz();
            default:
                return CreateCrc32();
        }
    }

    /// <summary>
    /// Creates an instance of <see cref="CyclicRedundancyCheck32"/> from the specified arguments.
    /// </summary>
    /// <param name="polynomial">This is a binary value that should be specified as a hexadecimal number.</param>
    /// <param name="initialValue">This parameter specifies the initial value of the register when the algorithm starts.</param>
    /// <param name="finalXor">This is an W-bit value that should be specified as a hexadecimal number.</param>
    /// <param name="setup">The <see cref="CyclicRedundancyCheckOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="CyclicRedundancyCheck32"/>.</returns>
    public static Hash CreateCrc32(uint polynomial, uint initialValue, uint finalXor, Action<CyclicRedundancyCheckOptions> setup = null)
    {
        return new CyclicRedundancyCheck32(polynomial, initialValue, finalXor, setup);
    }

    /// <summary>
    /// Creates an instance of <see cref="CyclicRedundancyCheck32"/> using <see cref="CyclicRedundancyCheckAlgorithm.Crc32"/>.
    /// </summary>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="CyclicRedundancyCheck32"/>.</returns>
    public static Hash CreateCrc32()
    {
        return new CyclicRedundancyCheck32(setup: o =>
        {
            o.ReflectInput = true;
            o.ReflectOutput = true;
        });
    }

    /// <summary>
    /// Creates an instance of <see cref="CyclicRedundancyCheck32"/> using <see cref="CyclicRedundancyCheckAlgorithm.Crc32Autosar"/>.
    /// </summary>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="CyclicRedundancyCheck32"/>.</returns>
    public static Hash CreateCrc32Autosar()
    {
        return new CyclicRedundancyCheck32(0xF4ACFB13, setup: o =>
        {
            o.ReflectInput = true;
            o.ReflectOutput = true;
        });
    }

    /// <summary>
    /// Creates an instance of <see cref="CyclicRedundancyCheck32"/> using <see cref="CyclicRedundancyCheckAlgorithm.Crc32Bzip2"/>.
    /// </summary>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="CyclicRedundancyCheck32"/>.</returns>
    public static Hash CreateCrc32Bzip2()
    {
        return new CyclicRedundancyCheck32();
    }

    /// <summary>
    /// Creates an instance of <see cref="CyclicRedundancyCheck32"/> using <see cref="CyclicRedundancyCheckAlgorithm.Crc32C"/>.
    /// </summary>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="CyclicRedundancyCheck32"/>.</returns>
    public static Hash CreateCrc32C()
    {
        return new CyclicRedundancyCheck32(0x1EDC6F41, setup: o =>
        {
            o.ReflectInput = true;
            o.ReflectOutput = true;
        });
    }

    /// <summary>
    /// Creates an instance of <see cref="CyclicRedundancyCheck32"/> using <see cref="CyclicRedundancyCheckAlgorithm.Crc32CdRomEdc"/>.
    /// </summary>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="CyclicRedundancyCheck32"/>.</returns>
    public static Hash CreateCrc32CdRomEdc()
    {
        return new CyclicRedundancyCheck32(0x8001801B, 0x0, 0x0, o =>
        {
            o.ReflectInput = true;
            o.ReflectOutput = true;
        });
    }

    /// <summary>
    /// Creates an instance of <see cref="CyclicRedundancyCheck32"/> using <see cref="CyclicRedundancyCheckAlgorithm.Crc32D"/>.
    /// </summary>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="CyclicRedundancyCheck32"/>.</returns>
    public static Hash CreateCrc32D()
    {
        return new CyclicRedundancyCheck32(0xA833982B, setup: o =>
        {
            o.ReflectInput = true;
            o.ReflectOutput = true;
        });
    }

    /// <summary>
    /// Creates an instance of <see cref="CyclicRedundancyCheck32"/> using <see cref="CyclicRedundancyCheckAlgorithm.Crc32Jamcrc"/>.
    /// </summary>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="CyclicRedundancyCheck32"/>.</returns>
    public static Hash CreateCrc32Jamcrc()
    {
        return new CyclicRedundancyCheck32(finalXor: 0x0, setup: o =>
        {
            o.ReflectInput = true;
            o.ReflectOutput = true;
        });
    }

    /// <summary>
    /// Creates an instance of <see cref="CyclicRedundancyCheck32"/> using <see cref="CyclicRedundancyCheckAlgorithm.Crc32Mpeg2"/>.
    /// </summary>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="CyclicRedundancyCheck32"/>.</returns>
    public static Hash CreateCrc32Mpeg2()
    {
        return new CyclicRedundancyCheck32(finalXor: 0x0);
    }

    /// <summary>
    /// Creates an instance of <see cref="CyclicRedundancyCheck32"/> using <see cref="CyclicRedundancyCheckAlgorithm.Crc32Posix"/>.
    /// </summary>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="CyclicRedundancyCheck32"/>.</returns>
    public static Hash CreateCrc32Posix()
    {
        return new CyclicRedundancyCheck32(initialValue: 0x0);
    }

    /// <summary>
    /// Creates an instance of <see cref="CyclicRedundancyCheck32"/> using <see cref="CyclicRedundancyCheckAlgorithm.Crc32Q"/>.
    /// </summary>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="CyclicRedundancyCheck32"/>.</returns>
    public static Hash CreateCrc32Q()
    {
        return new CyclicRedundancyCheck32(0x814141AB, 0x0, 0x0);
    }

    /// <summary>
    /// Creates an instance of <see cref="CyclicRedundancyCheck32"/> using <see cref="CyclicRedundancyCheckAlgorithm.Crc32Xfer"/>.
    /// </summary>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="CyclicRedundancyCheck32"/>.</returns>
    public static Hash CreateCrc32Xfer()
    {
        return new CyclicRedundancyCheck32(0xAF, 0x0, 0x0);
    }

    /// <summary>
    /// Creates an instance of <see cref="CyclicRedundancyCheck64"/> from the specified arguments.
    /// </summary>
    /// <param name="polynomial">This is a binary value that should be specified as a hexadecimal number.</param>
    /// <param name="initialValue">This parameter specifies the initial value of the register when the algorithm starts.</param>
    /// <param name="finalXor">This is an W-bit value that should be specified as a hexadecimal number.</param>
    /// <param name="setup">The <see cref="CyclicRedundancyCheckOptions" /> which may be configured.</param>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="CyclicRedundancyCheck64"/>.</returns>
    public static Hash CreateCrc64(ulong polynomial, ulong initialValue, ulong finalXor, Action<CyclicRedundancyCheckOptions> setup = null)
    {
        return new CyclicRedundancyCheck64(polynomial, initialValue, finalXor, setup);
    }

    /// <summary>
    /// Creates an instance of <see cref="CyclicRedundancyCheck64"/> using <see cref="CyclicRedundancyCheckAlgorithm.Crc64"/>.
    /// </summary>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="CyclicRedundancyCheck64"/>.</returns>
    public static Hash CreateCrc64()
    {
        return new CyclicRedundancyCheck64();
    }

    /// <summary>
    /// Creates an instance of <see cref="CyclicRedundancyCheck64"/> using <see cref="CyclicRedundancyCheckAlgorithm.Crc64GoIso"/>.
    /// </summary>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="CyclicRedundancyCheck64"/>.</returns>
    public static Hash CreateCrc64GoIso()
    {
        return new CyclicRedundancyCheck64(0x000000000000001B, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, o =>
        {
            o.ReflectInput = true;
            o.ReflectOutput = true;
        });
    }

    /// <summary>
    /// Creates an instance of <see cref="CyclicRedundancyCheck64"/> using <see cref="CyclicRedundancyCheckAlgorithm.Crc64We"/>.
    /// </summary>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="CyclicRedundancyCheck64"/>.</returns>
    public static Hash CreateCrc64We()
    {
        return new CyclicRedundancyCheck64(initialValue: 0xFFFFFFFFFFFFFFFF, finalXor: 0xFFFFFFFFFFFFFFFF);
    }

    /// <summary>
    /// Creates an instance of <see cref="CyclicRedundancyCheck64"/> using <see cref="CyclicRedundancyCheckAlgorithm.Crc64Xz"/>.
    /// </summary>
    /// <returns>A <see cref="Hash"/> implementation of <see cref="CyclicRedundancyCheck64"/>.</returns>
    public static Hash CreateCrc64Xz()
    {
        return new CyclicRedundancyCheck64(initialValue: 0xFFFFFFFFFFFFFFFF, finalXor: 0xFFFFFFFFFFFFFFFF, setup: o =>
        {
            o.ReflectInput = true;
            o.ReflectOutput = true;
        });
    }
}
