using System;
using System.Globalization;
using System.Numerics;

namespace Cuemon.Security;
/// <summary>
/// Provides an implementation of the FVN (Fowler–Noll–Vo) non-cryptographic hashing algorithm for 64-bit hash values. This class cannot be inherited.
/// Implements the <see cref="FowlerNollVoHash" />
/// </summary>
/// <seealso cref="FowlerNollVoHash" />
public sealed class FowlerNollVo64 : FowlerNollVoHash
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FowlerNollVo64"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="FowlerNollVoOptions" /> which may be configured.</param>
    public FowlerNollVo64(Action<FowlerNollVoOptions> setup = null) : base(64,
        BigInteger.Parse("1099511628211", CultureInfo.InvariantCulture),
        BigInteger.Parse("14695981039346656037", CultureInfo.InvariantCulture),
        setup)
    {
    }
}
