using System;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;

namespace Cuemon.Security.Cryptography
{
    [MemoryDiagnoser]
    public class AesCryptorBenchmark
    {
        [Params(128, 1024, 65536)]
        public int Size { get; set; }

        private AesCryptor _cryptor;
        private byte[] _plaintext;
        private byte[] _ciphertext;

        [GlobalSetup]
        public void GlobalSetup()
        {
            using var aes = Aes.Create();
            var key = aes.Key;
            var iv = aes.IV;
            _cryptor = new AesCryptor(key, iv);

            _plaintext = new byte[Size];
            var rnd = new Random(42);
            rnd.NextBytes(_plaintext);

            // Precompute ciphertext so Decrypt benchmark measures only decryption.
            _ciphertext = _cryptor.Encrypt(_plaintext);
        }

        [Benchmark(Description = "AesCryptor.Encrypt")]
        public byte[] Encrypt() => _cryptor.Encrypt(_plaintext);

        [Benchmark(Description = "AesCryptor.Decrypt")]
        public byte[] Decrypt() => _cryptor.Decrypt(_ciphertext);
    }
}
