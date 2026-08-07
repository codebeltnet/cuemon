using System;
using System.Globalization;
using System.Numerics;

namespace Cuemon.Security;
/// <summary>
/// Provides an implementation of the FVN (Fowler–Noll–Vo) non-cryptographic hashing algorithm for 512-bit hash values. This class cannot be inherited.
/// Implements the <see cref="FowlerNollVoHash" />
/// </summary>
/// <seealso cref="FowlerNollVoHash" />
public sealed class FowlerNollVo512 : FowlerNollVoHash
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FowlerNollVo512"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="FowlerNollVoOptions" /> which may be configured.</param>
    public FowlerNollVo512(Action<FowlerNollVoOptions> setup = null) : base(512,
        BigInteger.Parse("35835915874844867368919076489095108449946327955754392558399825615420669938882575126094039892345713852759", CultureInfo.InvariantCulture),
        BigInteger.Parse("9659303129496669498009435400716310466090418745672637896108374329434462657994582932197716438449813051892206539805784495328239340083876191928701583869517785", CultureInfo.InvariantCulture),
        setup)
    {
    }
}
