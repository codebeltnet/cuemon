namespace Cuemon.Security;
/// <summary>
/// Different models of the CRC algorithm family.
/// </summary>
public enum CyclicRedundancyCheckAlgorithm
{
    /// <summary>
    /// CRC-32; also known as CRC-32/ISO-HDLC, CRC-32/ADCCP, CRC-32/V-42, CRC-32/XZ, PKZIP.
    /// </summary>
    Crc32 = 0,
    /// <summary>
    /// CRC-32/AUTOSAR.
    /// </summary>
    Crc32Autosar = 1,
    /// <summary>
    /// CRC-32/BZIP2; also known as CRC-32/AAL5, CRC-32/DECT-B, B-CRC-32.
    /// </summary>
    Crc32Bzip2 = 2,
    /// <summary>
    /// CRC32-C; also known as CRC-32/ISCSI, CRC-32/BASE91-C, CRC-32/CASTAGNOLI, CRC-32/INTERLAKEN.
    /// </summary>
    Crc32C = 3,
    /// <summary>
    /// CRC-32/CD-ROM-EDC.
    /// </summary>
    Crc32CdRomEdc = 4,
    /// <summary>
    /// CRC-32D; also known as BASE91-D.
    /// </summary>
    Crc32D = 5,
    /// <summary>
    /// CRC-32/MPEG-2.
    /// </summary>
    Crc32Mpeg2 = 6,
    /// <summary>
    /// CRC-32/POSIX; also known as CRC-32/CKSUM, CKSUM.
    /// </summary>
    Crc32Posix = 7,
    /// <summary>
    /// CRC-32Q; also known as CRC-32/AIXM.
    /// </summary>
    Crc32Q = 8,
    /// <summary>
    /// CRC-32/JAMCRC.
    /// </summary>
    Crc32Jamcrc = 9,
    /// <summary>
    /// CRC-32/XFER.
    /// </summary>
    Crc32Xfer = 10,
    /// <summary>
    /// CRC-64; also known as CRC-64/ECMA-182.
    /// </summary>
    Crc64 = 20,
    /// <summary>
    /// CRC-64/GO-ISO.
    /// </summary>
    Crc64GoIso = 21,
    /// <summary>
    /// CRC-64/WE.
    /// </summary>
    Crc64We = 22,
    /// <summary>
    /// CRC-64/XZ; also, mistakenly, known as: CRC-64/GO-ECMA.
    /// </summary>
    Crc64Xz = 23
}
