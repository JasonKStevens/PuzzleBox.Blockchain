using PuzzleBox.Blockchain.Abstraction.Providers;
using System;
using System.Security.Cryptography;
using System.Text;

namespace PuzzleBox.Blockchain.Providers
{
    public class SHA256CryptoProvider : ICryptoProvider
    {
        private readonly Func<HashAlgorithm> _hashAlgorithmFactory;

        public SHA256CryptoProvider()
        {
            _hashAlgorithmFactory = SHA256.Create;
        }

        public string GetHash(string input)
        {
            using (var hashAlgorithm = _hashAlgorithmFactory())
            {
                return GetHash(hashAlgorithm, input);
            }
        }

        public bool VerifyHash(string input, string hash)
        {
            using (var hashAlgorithm = _hashAlgorithmFactory())
            {
                var hashOfInput = GetHash(hashAlgorithm, input);
                var isVerified = StringComparer.OrdinalIgnoreCase.Compare(hashOfInput, hash) == 0;
                return isVerified;
            }
        }

        private string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            var hash = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            var hashString = ByteArrayToString(hash);
            return hashString;
        }

        private static string ByteArrayToString(byte[] data)
        {
            var builder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
