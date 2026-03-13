using System;

namespace Cuemon
{
    /// <summary>
    /// Specifies allowed GUID formats in parsing related methods.
    /// </summary>
    [Flags]
    public enum GuidFormats
    {
        /// <summary>
        /// Specified the number format (N) which consist of 32 digits, eg. 12345678123412341234123456789abc.
        /// </summary>
        N = 1,
        /// <summary>
        /// Specified the digit format (D) which consist of 32 digits separated by hyphens, eg. 12345678-1234-1234-1234-123456789abc.
        /// </summary>
        D = 2,
        /// <summary>
        /// Specified the brace format (B) which consist of 32 digits separated by hyphens, enclosed in brackets, eg. {12345678-1234-1234-1234-123456789abc}.
        /// </summary>
        B = 4,
        /// <summary>
        /// Specified the parenthesis format (P) which consist of 32 digits separated by hyphens, enclosed in parentheses, eg. (12345678-1234-1234-1234-123456789abc).
        /// </summary>
        P = 8,
        /// <summary>
        /// Specified four hexadecimal values enclosed in braces (X) where the fourth value is a subset of eight hexadecimal values that is also enclosed in braces.
        /// </summary>
        X = 16,
        /// <summary>
        /// Specified any of the supported GUID formats (N,D,B,P,X).
        /// </summary>
        Any = N | D | B | P | X
    }
}