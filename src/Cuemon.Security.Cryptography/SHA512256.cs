// -----------------------------------------------------------------------------
//  SHA512/256 Implementation for .NET - Cuemon.Security.Cryptography
// -----------------------------------------------------------------------------
//  This implementation provides a pure .NET version of SHA-512/256
//  (as defined in NIST FIPS PUB 180-4, Section 5.3.6.2) with no external
//  dependencies, suitable for use via the abstract HashAlgorithm base class.
//
//  References:
//  - NIST FIPS PUB 180-4: https://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.180-4.pdf
//  - .NET Runtime (SHA512Managed): https://github.com/dotnet/runtime
//  - Conceptual structure inspired by BouncyCastle and OpenSSL (no code copied)
//
//  Author: ChatGPT (OpenAI) — Composed from scratch using official specifications
//  Library Integration: Cuemon.Security.Cryptography (v9.0.5+)
//  URL: https://www.nuget.org/packages/Cuemon.Security.Cryptography
//
//  Special thanks to:
//  - The .NET cryptography reference source for guidance on HashAlgorithm integration
//  - BouncyCastle and OpenSSL for helping shape best practices in crypto implementations
//  - The Free and Open Source Software (FOSS) community for collaboration and inspiration
// -----------------------------------------------------------------------------

using System;
using System.Security.Cryptography;

namespace Cuemon.Security.Cryptography;
/// <summary>
/// Represents the SHA-512/256 cryptographic hash algorithm, which produces a 256-bit hash value using the SHA-512 algorithm as its base.
/// </summary>
/// <remarks>Full disclosure; this class was created in collaboration with OpenAI ChatGPT. Have a look at the prompt here: https://chatgpt.com/share/67fbd1fe-17d8-8010-8144-e94251c2e79d</remarks>
public sealed class SHA512256 : HashAlgorithm
{
    private const int BlockSize = 128; // 1024 bits
    private const int DigestLength = 32; // 256 bits

    private static readonly ulong[] K = new ulong[]
    {
            // Constants used in the SHA-512 algorithm
            0x428a2f98d728ae22, 0x7137449123ef65cd, 0xb5c0fbcfec4d3b2f, 0xe9b5dba58189dbbc,
            0x3956c25bf348b538, 0x59f111f1b605d019, 0x923f82a4af194f9b, 0xab1c5ed5da6d8118,
            0xd807aa98a3030242, 0x12835b0145706fbe, 0x243185be4ee4b28c, 0x550c7dc3d5ffb4e2,
            0x72be5d74f27b896f, 0x80deb1fe3b1696b1, 0x9bdc06a725c71235, 0xc19bf174cf692694,
            0xe49b69c19ef14ad2, 0xefbe4786384f25e3, 0x0fc19dc68b8cd5b5, 0x240ca1cc77ac9c65,
            0x2de92c6f592b0275, 0x4a7484aa6ea6e483, 0x5cb0a9dcbd41fbd4, 0x76f988da831153b5,
            0x983e5152ee66dfab, 0xa831c66d2db43210, 0xb00327c898fb213f, 0xbf597fc7beef0ee4,
            0xc6e00bf33da88fc2, 0xd5a79147930aa725, 0x06ca6351e003826f, 0x142929670a0e6e70,
            0x27b70a8546d22ffc, 0x2e1b21385c26c926, 0x4d2c6dfc5ac42aed, 0x53380d139d95b3df,
            0x650a73548baf63de, 0x766a0abb3c77b2a8, 0x81c2c92e47edaee6, 0x92722c851482353b,
            0xa2bfe8a14cf10364, 0xa81a664bbc423001, 0xc24b8b70d0f89791, 0xc76c51a30654be30,
            0xd192e819d6ef5218, 0xd69906245565a910, 0xf40e35855771202a, 0x106aa07032bbd1b8,
            0x19a4c116b8d2d0c8, 0x1e376c085141ab53, 0x2748774cdf8eeb99, 0x34b0bcb5e19b48a8,
            0x391c0cb3c5c95a63, 0x4ed8aa4ae3418acb, 0x5b9cca4f7763e373, 0x682e6ff3d6b2b8a3,
            0x748f82ee5defb2fc, 0x78a5636f43172f60, 0x84c87814a1f0ab72, 0x8cc702081a6439ec,
            0x90befffa23631e28, 0xa4506cebde82bde9, 0xbef9a3f7b2c67915, 0xc67178f2e372532b,
            0xca273eceea26619c, 0xd186b8c721c0c207, 0xeada7dd6cde0eb1e, 0xf57d4f7fee6ed178,
            0x06f067aa72176fba, 0x0a637dc5a2c898a6, 0x113f9804bef90dae, 0x1b710b35131c471b,
            0x28db77f523047d84, 0x32caab7b40c72493, 0x3c9ebe0a15c9bebc, 0x431d67c49c100d4c,
            0x4cc5d4becb3e42b6, 0x597f299cfc657e2a, 0x5fcb6fab3ad6faec, 0x6c44198c4a475817
    };

    private static readonly ulong[] IV512_256 = new ulong[]
    {
            // Initial hash values for SHA-512/256
            0x22312194FC2BF72C, 0x9F555FA3C84C64C2,
            0x2393B86B6F53B151, 0x963877195940EABD,
            0x96283EE2A88EFFE3, 0xBE5E1E2553863992,
            0x2B0199FC2C85B8AA, 0x0EB72DDC81C52CA2
    };

    private ulong[] _H = new ulong[8];
    private ulong[] _W = new ulong[80];
    private byte[] _buffer = new byte[BlockSize];
    private int _bufferPos;
    private ulong _bitCountHigh;
    private ulong _bitCountLow;

    /// <summary>
    /// Initializes a new instance of the <see cref="SHA512256"/> class.
    /// </summary>
    public SHA512256()
    {
        Initialize();
        HashSizeValue = 256;
    }

    /// <summary>
    /// Initializes the hash algorithm, resetting its state.
    /// </summary>
    public override void Initialize()
    {
        Array.Copy(IV512_256, _H, 8);
        Array.Clear(_buffer, 0, _buffer.Length);
        _bufferPos = 0;
        _bitCountHigh = 0;
        _bitCountLow = 0;
    }

    /// <summary>
    /// Routes data written to the object into the hash algorithm for computing the hash.
    /// </summary>
    /// <param name="array">The input data.</param>
    /// <param name="ibStart">The offset into the byte array from which to begin using data.</param>
    /// <param name="cbSize">The number of bytes in the array to use as data.</param>
    protected override void HashCore(byte[] array, int ibStart, int cbSize)
    {
        while (cbSize > 0)
        {
            int toCopy = Math.Min(BlockSize - _bufferPos, cbSize);
            Array.Copy(array, ibStart, _buffer, _bufferPos, toCopy);
            _bufferPos += toCopy;
            ibStart += toCopy;
            cbSize -= toCopy;

            AddLength((ulong)(toCopy * 8)); // track total bit count

            if (_bufferPos == BlockSize)
            {
                ProcessBlock(_buffer, 0);
                _bufferPos = 0;
            }
        }
    }

    /// <summary>
    /// Finalizes the hash computation after the last data is processed.
    /// </summary>
    /// <returns>The computed hash code.</returns>
    protected override byte[] HashFinal()
    {
        // Padding
        _buffer[_bufferPos++] = 0x80;
        if (_bufferPos > BlockSize - 16)
        {
            Array.Clear(_buffer, _bufferPos, BlockSize - _bufferPos);
            ProcessBlock(_buffer, 0);
            _bufferPos = 0;
        }

        Array.Clear(_buffer, _bufferPos, BlockSize - _bufferPos);

        // Append total bit count (128-bit big-endian)
        WriteULongBE(_bitCountHigh, _buffer, BlockSize - 16);
        WriteULongBE(_bitCountLow, _buffer, BlockSize - 8);
        ProcessBlock(_buffer, 0);

        // Produce digest (first 256 bits = first 4 H words)
        byte[] output = new byte[DigestLength];
        for (int i = 0; i < 4; i++)
        {
            WriteULongBE(_H[i], output, i * 8);
        }

        return output;
    }

    /// <summary>
    /// Adds the specified number of bits to the total bit count.
    /// </summary>
    /// <param name="bits">The number of bits to add.</param>
    private void AddLength(ulong bits)
    {
        _bitCountLow += bits;
        if (_bitCountLow < bits)
            _bitCountHigh++;
    }

    /// <summary>
    /// Writes a 64-bit unsigned integer to a byte array in big-endian format.
    /// </summary>
    /// <param name="value">The value to write.</param>
    /// <param name="buffer">The buffer to write to.</param>
    /// <param name="offset">The offset in the buffer to start writing at.</param>
    private static void WriteULongBE(ulong value, byte[] buffer, int offset)
    {
        for (int i = 7; i >= 0; i--)
            buffer[offset + 7 - i] = (byte)(value >> (i * 8));
    }

    /// <summary>
    /// Processes a single 1024-bit block of data.
    /// </summary>
    /// <param name="block">The block of data to process.</param>
    /// <param name="offset">The offset in the block to start processing at.</param>
    private void ProcessBlock(byte[] block, int offset)
    {
        for (int i = 0; i < 16; i++)
        {
            _W[i] =
                ((ulong)block[offset + i * 8 + 0] << 56) |
                ((ulong)block[offset + i * 8 + 1] << 48) |
                ((ulong)block[offset + i * 8 + 2] << 40) |
                ((ulong)block[offset + i * 8 + 3] << 32) |
                ((ulong)block[offset + i * 8 + 4] << 24) |
                ((ulong)block[offset + i * 8 + 5] << 16) |
                ((ulong)block[offset + i * 8 + 6] << 8) |
                ((ulong)block[offset + i * 8 + 7]);
        }

        for (int i = 16; i < 80; i++)
        {
            ulong s0 = RotateRight(_W[i - 15], 1) ^ RotateRight(_W[i - 15], 8) ^ (_W[i - 15] >> 7);
            ulong s1 = RotateRight(_W[i - 2], 19) ^ RotateRight(_W[i - 2], 61) ^ (_W[i - 2] >> 6);
            _W[i] = _W[i - 16] + s0 + _W[i - 7] + s1;
        }

        ulong a = _H[0], b = _H[1], c = _H[2], d = _H[3];
        ulong e = _H[4], f = _H[5], g = _H[6], h = _H[7];

        for (int i = 0; i < 80; i++)
        {
            ulong S1 = RotateRight(e, 14) ^ RotateRight(e, 18) ^ RotateRight(e, 41);
            ulong ch = (e & f) ^ (~e & g);
            ulong temp1 = h + S1 + ch + K[i] + _W[i];
            ulong S0 = RotateRight(a, 28) ^ RotateRight(a, 34) ^ RotateRight(a, 39);
            ulong maj = (a & b) ^ (a & c) ^ (b & c);
            ulong temp2 = S0 + maj;

            h = g;
            g = f;
            f = e;
            e = d + temp1;
            d = c;
            c = b;
            b = a;
            a = temp1 + temp2;
        }

        _H[0] += a; _H[1] += b; _H[2] += c; _H[3] += d;
        _H[4] += e; _H[5] += f; _H[6] += g; _H[7] += h;
    }

    /// <summary>
    /// Rotates the bits of a 64-bit unsigned integer to the right.
    /// </summary>
    /// <param name="x">The value to rotate.</param>
    /// <param name="n">The number of bits to rotate.</param>
    /// <returns>The rotated value.</returns>
    private static ulong RotateRight(ulong x, int n) => (x >> n) | (x << (64 - n));
}
