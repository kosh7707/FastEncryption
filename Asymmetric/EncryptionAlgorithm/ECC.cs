using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Asymmetric.EncryptionAlgorithm
{
    internal struct ECPoint(BigInteger x, BigInteger y)
    {
        public BigInteger X { get; } = x;
        public BigInteger Y { get; } = y;

        public readonly bool IsInfinity => X == 0 && Y == 0;

        public static readonly ECPoint Infinity = new(0, 0);
    }

    internal class ECC(BigInteger a, BigInteger b, BigInteger p, BigInteger n, ECPoint G)
    {
        private readonly BigInteger a = a, b = b, p = p, n = n;
        private readonly ECPoint G = G;

        public byte[] Sign(byte[] message, BigInteger privKey)
        {
            byte[] hash = SHA256.HashData(message);
            BigInteger z = new(hash, isUnsigned: true, isBigEndian: true);

            if (z >= n)
            {
                z >>= (z.GetByteCount() * 8 - n.GetByteCount() * 8);
            }

            BigInteger r = 0, s = 0;

            while (s == 0)
            {
                BigInteger k = GenerateRandomBigInteger(n);
                ECPoint R = MultiplyPoint(G, k);

                r = R.X % n;

                if (r == 0) continue;

                s = (ModInverse(k, n) * (z + r * privKey)) % n;

                if (s == 0) continue;
            }

            byte[] rBytes = r.ToByteArray(isUnsigned: true, isBigEndian: true);
            byte[] sBytes = s.ToByteArray(isUnsigned: true, isBigEndian: true);

            byte[] rPadded = new byte[32], sPadded = new byte[32];
            Array.Copy(rBytes, 0, rPadded, 32 - rBytes.Length, rBytes.Length);
            Array.Copy(sBytes, 0, sPadded, 32 - sBytes.Length, sBytes.Length);

            byte[] signature = new byte[64];
            Array.Copy(rPadded, 0, signature, 0, 32);
            Array.Copy(sPadded, 0, signature, 32, 32);

            return signature;
        }

        public bool Verify(byte[] message, byte[] signature, ECPoint pubKey)
        {
            byte[] hash = SHA256.HashData(message);
            BigInteger z = new(hash, isUnsigned: true, isBigEndian: true);

            if (z >= n)
            {
                z >>= (z.GetByteCount() * 8 - n.GetByteCount() * 8);
            }

            int halfLength = signature.Length / 2;
            byte[] rBytes = new byte[halfLength];
            byte[] sBytes = new byte[halfLength];
            Array.Copy(signature, 0, rBytes, 0, halfLength);
            Array.Copy(signature, halfLength, sBytes, 0, halfLength);

            BigInteger r = new(rBytes, isUnsigned: true, isBigEndian: true);
            BigInteger s = new(sBytes, isUnsigned: true, isBigEndian: true);

            if (r <= 0 || r >= n || s <= 0 || s >= n)
                return false;

            BigInteger w = ModInverse(s, n);
            BigInteger u1 = (z * w) % n;
            BigInteger u2 = (r * w) % n;

            ECPoint point1 = MultiplyPoint(G, u1);
            ECPoint point2 = MultiplyPoint(pubKey, u2);
            ECPoint R = AddPoints(point1, point2);

            return (!R.IsInfinity) && (R.X % n == r);
        }

        public ECPoint GetPublicKey(BigInteger privKey)
        {
            if (privKey <= 0 || privKey >= n)
                throw new ArgumentException("Private key must be in the range [1, n-1]");

            return MultiplyPoint(G, privKey);
        }

        private static BigInteger ModInverse(BigInteger a, BigInteger m)
        {
            if (a == 0) throw new DivideByZeroException();

            a %= m;
            if (a < 0) a += m;

            BigInteger m0 = m, t, q;
            BigInteger x0 = 0, x1 = 1;

            while (a > 1)
            {
                q = a / m;
                t = m;
                m = a % m;
                a = t;
                t = x0;
                x0 = x1 - q * x0;
                x1 = t;
            }

            if (x1 < 0) x1 += m0;

            return x1;
        }

        private ECPoint AddPoints(ECPoint p1, ECPoint p2)
        {
            if (p1.IsInfinity) return p2;
            if (p2.IsInfinity) return p1;

            // CheckPoint(p1);
            // CheckPoint(p2);

            BigInteger m;
            if (p1.X == p2.X)
            {
                if (p1.Y != p2.Y)
                    return ECPoint.Infinity;

                // m = (3 * x1^2 + a) / (2 * y1) mod p
                m = (3 * BigInteger.ModPow(p1.X, 2, p) + a) * ModInverse(2 * p1.Y, p);
            }
            else
            {
                // m = (y2 - y1) / (x2 - x1) mod p
                m = (p2.Y - p1.Y) * ModInverse(p2.X - p1.X, p);
            }
                
            m = m % p;
            if (m < 0) m += p;

            // x3 = m^2 - x1 - x2 mod p
            BigInteger x3 = (BigInteger.ModPow(m, 2, p) - p1.X - p2.X) % p;
            if (x3 < 0) x3 += p;

            // y3 = m * (x1 - x3) - y1 mod p
            BigInteger y3 = (m * (p1.X - x3) - p1.Y) % p;
            if (y3 < 0) y3 += p;

            return new ECPoint(x3, y3);
        }

        private ECPoint MultiplyPoint(ECPoint point, BigInteger k)
        {
            if (point.IsInfinity || k == 0)
                return ECPoint.Infinity;

            // CheckPoint(point);

            ECPoint result = ECPoint.Infinity;
            ECPoint addend = point;

            while (k != 0)
            {
                if ((k & 1) == 1)
                    result = AddPoints(result, addend);

                addend = AddPoints(addend, addend);

                k >>= 1;
            }

            return result;
        }

        private static BigInteger GenerateRandomBigInteger(BigInteger max)
        {
            BigInteger result;
            byte[] bytes = new byte[max.ToByteArray().Length];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                do
                {
                    rng.GetBytes(bytes);
                    result = new BigInteger(bytes, isUnsigned: true, isBigEndian: true);
                } while (result >= max || result == 0);
            }

            return result;
        }

        private void CheckPoint(ECPoint point)
        {
            if (!IsOnCurve(point)) 
                throw new ArgumentException("Point is not on curve.");
        }

        private bool IsOnCurve(ECPoint point)
        {
            if (point.IsInfinity) return true;

            BigInteger lhs = BigInteger.ModPow(point.Y, 2, p);
            BigInteger rhs = (BigInteger.ModPow(point.X, 3, p) + a * point.X + b) % p;

            return lhs == rhs;
        }

        public static string AlgorithmName => "ECC";
    }
}
