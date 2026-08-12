namespace Cuemon.Extensions;
/// <summary>
/// The accuracy of a rounding for a computed number.
/// </summary>
public enum RoundOffAccuracy
{
    /// <summary>
    /// Specifies a rounding to the nearest tenth of a number.
    /// </summary>
    NearestTenth = 10,
    /// <summary>
    /// Specifies a rounding to the nearest hundredth of a number.
    /// </summary>
    NearestHundredth = NearestTenth * NearestTenth,
    /// <summary>
    /// Specifies a rounding to the nearest thousandth of a number.
    /// </summary>
    NearestThousandth = NearestTenth * NearestHundredth,
    /// <summary>
    /// Specifies a rounding to the nearest ten thousandth of a number.
    /// </summary>
    NearestTenThousandth = NearestTenth * NearestThousandth,
    /// <summary>
    /// Specifies a rounding to the nearest hundred thousandth of a number.
    /// </summary>
    NearestHundredThousandth = NearestTenth * NearestTenThousandth,
    /// <summary>
    /// Specifies a rounding to the nearest million of a number.
    /// </summary>
    NearestMillion = NearestTenth * NearestHundredThousandth
}
