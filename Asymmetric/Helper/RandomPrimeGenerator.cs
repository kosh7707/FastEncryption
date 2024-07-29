using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Asymmetric.Helper
{
    internal class RandomPrimeGenerator
    {
        private readonly HashDRBG drbg;

        public RandomPrimeGenerator(byte[] seed)
        {
            drbg = new HashDRBG(seed);
        }

        public BigInteger GeneratePrime(int bitLength)
        {
            while (true)
            {
                byte[] randomBytes = drbg.Generate((bitLength + 7) / 8);

                // 최상위 비트를 1로 설정하여 원하는 비트 길이 보장
                randomBytes[0] |= 0x80;
                // 최하위 비트를 1로 설정하여 홀수 보장
                randomBytes[randomBytes.Length - 1] |= 0x01;

                BigInteger candidate = new BigInteger(randomBytes);

                if (IsProbablePrime(candidate, 40))  // NIST 권장 반복 횟수
                {
                    return candidate;
                }
            }
        }

        private bool IsProbablePrime(BigInteger n, int k)
        {
            if (n == 2 || n == 3)
                return true;
            if (n < 2 || n % 2 == 0)
                return false;

            BigInteger d = n - 1;
            int s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            for (int i = 0; i < k; i++)
            {
                BigInteger a = RandomBigInteger(2, n - 2);
                BigInteger x = BigInteger.ModPow(a, d, n);
                if (x == 1 || x == n - 1)
                    continue;

                bool isProbablePrime = false;
                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, n);
                    if (x == n - 1)
                    {
                        isProbablePrime = true;
                        break;
                    }
                }

                if (!isProbablePrime)
                    return false;
            }

            return true;
        }

        private BigInteger RandomBigInteger(BigInteger min, BigInteger max)
        {
            byte[] bytes = max.ToByteArray();
            BigInteger result;
            do
            {
                bytes = drbg.Generate(bytes.Length);
                result = new BigInteger(bytes);
            } while (result < min || result > max);

            return result;
        }

        public (BigInteger p, BigInteger q) GenerateRSAPrimes(int modulusBitLength)
        {
            int primeBitLength = modulusBitLength / 2;
            BigInteger p = GeneratePrime(primeBitLength);
            BigInteger q = GeneratePrime(primeBitLength);
            return (p, q);
        }
    }
}
