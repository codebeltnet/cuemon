using System;
using System.Globalization;
using System.Numerics;

namespace Cuemon.Security;
/// <summary>
/// Provides an implementation of the FVN (Fowler–Noll–Vo) non-cryptographic hashing algorithm for 32-bit hash values. This class cannot be inherited.
/// Implements the <see cref="FowlerNollVoHash" />
/// </summary>
/// <seealso cref="FowlerNollVoHash" />
public sealed class FowlerNollVo32 : FowlerNollVoHash
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FowlerNollVo32"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="FowlerNollVoOptions" /> which may be configured.</param>
    public FowlerNollVo32(Action<FowlerNollVoOptions> setup = null) : base(32,
        BigInteger.Parse("16777619", CultureInfo.InvariantCulture),
        BigInteger.Parse("2166136261", CultureInfo.InvariantCulture),
        setup)
    {
    }
}
