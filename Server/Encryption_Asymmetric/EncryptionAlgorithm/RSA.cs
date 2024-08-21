using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Encryption_Asymmetric.EncryptionAlgorithm
{
    public class RSA(BigInteger n)
    {
        private readonly BigInteger n = n;
        public byte[] Sign(byte[] message, BigInteger privKey)
        {
            byte[] hash = SHA256.HashData(message);

            BigInteger hash_int  = new(hash, isUnsigned: true, isBigEndian: true);
            BigInteger signature_int = BigInteger.ModPow(hash_int, privKey, n);

            return signature_int.ToByteArray(isUnsigned: true, isBigEndian: true);
        }

        public bool Verify(byte[] message, byte[] signature, BigInteger pubKey)
        {
            byte[] hash = SHA256.HashData(message);

            BigInteger signature_int = new(signature, isUnsigned: true, isBigEndian: true);
            BigInteger decryptedHash_int = BigInteger.ModPow(signature_int, pubKey, n);

            byte[] decryptedHash = decryptedHash_int.ToByteArray(isUnsigned: true, isBigEndian: true);

            return hash.SequenceEqual(decryptedHash);
        }

        public static string AlgorithmName => "RSA";
    }
}
