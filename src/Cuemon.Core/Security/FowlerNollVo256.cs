using System;
using System.Globalization;
using System.Numerics;

namespace Cuemon.Security;
/// <summary>
/// Provides an implementation of the FVN (Fowler–Noll–Vo) non-cryptographic hashing algorithm for 256-bit hash values. This class cannot be inherited.
/// Implements the <see cref="FowlerNollVoHash" />
/// </summary>
/// <seealso cref="FowlerNollVoHash" />
public sealed class FowlerNollVo256 : FowlerNollVoHash
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FowlerNollVo256"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="FowlerNollVoOptions" /> which may be configured.</param>
    public FowlerNollVo256(Action<FowlerNollVoOptions> setup = null) : base(256,
        BigInteger.Parse("374144419156711147060143317175368453031918731002211", CultureInfo.InvariantCulture),
        BigInteger.Parse("100029257958052580907070968620625704837092796014241193945225284501741471925557", CultureInfo.InvariantCulture),
        setup)
    {
    }
}
