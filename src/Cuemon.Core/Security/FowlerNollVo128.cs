using System;
using System.Globalization;
using System.Numerics;

namespace Cuemon.Security;
/// <summary>
/// Provides an implementation of the FVN (Fowler–Noll–Vo) non-cryptographic hashing algorithm for 128-bit hash values. This class cannot be inherited.
/// Implements the <see cref="FowlerNollVoHash" />
/// </summary>
/// <seealso cref="FowlerNollVoHash" />
public sealed class FowlerNollVo128 : FowlerNollVoHash
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FowlerNollVo128"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="FowlerNollVoOptions" /> which may be configured.</param>
    public FowlerNollVo128(Action<FowlerNollVoOptions> setup = null) : base(128,
        BigInteger.Parse("309485009821345068724781371", CultureInfo.InvariantCulture),
        BigInteger.Parse("144066263297769815596495629667062367629", CultureInfo.InvariantCulture),
        setup)
    {
    }
}
