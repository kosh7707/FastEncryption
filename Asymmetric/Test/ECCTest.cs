using Asymmetric.EncryptionAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Asymmetric.Test
{
    internal class ECCTest
    {
        public static void Run()
        {
            string filePath;

            Console.WriteLine("\n******* ECC(P-256,SHA-256) TEST START *******\n");

            // NIST P-256 // SHA-256
            BigInteger a = HexStringToBigInteger("ffffffff00000001000000000000000000000000fffffffffffffffffffffffc"),
                       b = HexStringToBigInteger("5ac635d8aa3a93e7b3ebbd55769886bc651d06b0cc53b0f63bce3c3e27d2604b"),
                       p = HexStringToBigInteger("ffffffff00000001000000000000000000000000ffffffffffffffffffffffff"),
                       n = HexStringToBigInteger("ffffffff00000000ffffffffffffffffbce6faada7179e84f3b9cac2fc632551"),
                       gx = HexStringToBigInteger("6b17d1f2e12c4247f8bce6e563a440f277037d812deb33a0f4a13945d898c296"),
                       gy = HexStringToBigInteger("4fe342e2fe1a7f9b8ee7eb4a7c0f9e162bce33576b315ececbb6406837bf51f5");
            ECC ecc = new(a, b, p, n, new ECPoint(gx, gy));

            // 비밀키에 대한 공개키 생성 검증
            filePath = "../../../Test/test-vector/ECC/KeyPairTest.txt";
            if (KeyPairTest(ecc, filePath))
            {    
                Console.WriteLine($"[ECC - KeyPairTest]\t[Success]\t{Path.GetFileName(filePath)}");
            }
            else
            {
                Console.WriteLine($"[ECC - KeyPairTest]\t[Fail]   \t{Path.GetFileName(filePath)}");
            }

            // 공개키 유효성 검증 
            filePath = "../../../Test/test-vector/ECC/PKVTest.txt";
            if (PKVTest(ecc, filePath))
            {
                Console.WriteLine($"[ECC - PKVTest]\t\t[Success]\t{Path.GetFileName(filePath)}");
            }
            else
            {
                Console.WriteLine($"[ECC - PKVTest]\t\t[Fail]   \t{Path.GetFileName(filePath)}");
            }

            // 서명 검증 테스트
            filePath = "../../../Test/test-vector/ECC/SigVerTest.txt";
            if (SigVerTest(ecc, filePath))
            {
                Console.WriteLine($"[ECC - SigVerTest]\t[Success]\t{Path.GetFileName(filePath)}");
            }
            else
            {
                Console.WriteLine($"[ECC - SigVerTest]\t[Fail]   \t{Path.GetFileName(filePath)}");
            }

            // 서명 생성 테스트
            filePath = "../../../Test/test-vector/ECC/SigGenTest.txt";
            if (SigGenTest(ecc, filePath))
            {
                Console.WriteLine($"[ECC - SigGenTest]\t[Success]\t{Path.GetFileName(filePath)}");
            }
            else
            {
                Console.WriteLine($"[ECC - SigGenTest]\t[Fail]   \t{Path.GetFileName(filePath)}");
            }

            Console.WriteLine("\n******* ECC(P-256,SHA-256) TEST END *******");
        }


        internal class ECCTestVector
        {
            public BigInteger Key { get; set; }
            public BigInteger Qx { get; set; }
            public BigInteger Qy { get; set; }
            public BigInteger K {  get; set; }
            public BigInteger R { get; set; }
            public BigInteger S { get; set; }
            public bool IsValid { get; set; }
            public byte[] Msg { get; set; }

        }

        static bool KeyPairTest(ECC ecc, string filePath)
        {
            Regex KeyPattern    = new Regex(@"d\s*=\s*([0-9a-fA-F]+)");
            Regex QxPattern     = new Regex(@"Qx\s*=\s*([0-9a-fA-F]+)");
            Regex QyPattern     = new Regex(@"Qy\s*=\s*([0-9a-fA-F]+)");

            List<ECCTestVector> vectors = new List<ECCTestVector>();
            bool d_flag = false, qx_flag = false, qy_flag = false;
            BigInteger d = 0, qx = 0, qy = 0;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (KeyPattern.IsMatch(line))
                {
                    d = HexStringToBigInteger(KeyPattern.Match(line).Groups[1].Value);
                    d_flag = true;
                }
                else if (QxPattern.IsMatch(line))
                {
                    qx = HexStringToBigInteger(QxPattern.Match(line).Groups[1].Value);
                    qx_flag = true;
                }
                else if (QyPattern.IsMatch(line))
                {
                    qy = HexStringToBigInteger(QyPattern.Match(line).Groups[1].Value);
                    qy_flag = true;
                }

                if (d_flag && qx_flag && qy_flag)
                {
                    vectors.Add(new ECCTestVector
                    {
                        Key = d, 
                        Qx = qx,
                        Qy = qy,
                    });
                    d_flag = qx_flag = qy_flag = false;
                }
            }

            foreach (ECCTestVector vector in vectors) 
            {
                ECPoint pubKey = ecc.GetPublicKey(vector.Key);
                if (pubKey.X != vector.Qx || pubKey.Y != vector.Qy)
                {
                    Console.Write($"Key: {vector.Key:X}\n");

                    Console.Write($"vector.Qx: {vector.Qx:X}\n");
                    Console.Write($"Qx: {pubKey.X:X}\n");

                    Console.Write($"vector.Qy: {vector.Qy:X}\n");
                    Console.Write($"Qy: {pubKey.Y:X}\n");

                    return false;
                }
            }

            return true;
        }

        static bool PKVTest(ECC ecc, string filePath)
        {
            Regex QxPattern         = new Regex(@"Qx\s*=\s*([0-9a-fA-F]+)");
            Regex QyPattern         = new Regex(@"Qy\s*=\s*([0-9a-fA-F]+)");
            Regex ResultPattern     = new Regex(@"Result\s*=\s*(P|F)\s*\((\d+).*?\)");

            List<ECCTestVector> vectors = new List<ECCTestVector>();
            bool qx_flag = false, qy_flag = false, isValid_flag = false;
            BigInteger qx = 0, qy = 0; bool isValid = false;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (QxPattern.IsMatch(line))
                {
                    qx = HexStringToBigInteger(QxPattern.Match(line).Groups[1].Value);
                    qx_flag = true;
                }
                else if (QyPattern.IsMatch(line))
                {
                    qy = HexStringToBigInteger(QyPattern.Match(line).Groups[1].Value);
                    qy_flag = true;
                }
                else if (ResultPattern.IsMatch(line))
                {
                    isValid = ResultPattern.Match(line).Groups[1].Value == "P";
                    isValid_flag = true;
                }

                if (qx_flag && qy_flag && isValid_flag)
                {
                    vectors.Add(new ECCTestVector
                    {
                        Qx = qx,
                        Qy = qy,
                        IsValid = isValid,
                    });
                    qx_flag = qy_flag = isValid_flag = false;
                }
            }

            foreach (ECCTestVector vector in vectors)
            {
                int res = ecc.isValidPoint(new ECPoint(vector.Qx, vector.Qy));

                if ( (res == 0 && !vector.IsValid) || ((res == 1 || res == 2) && vector.IsValid) )
                {
                    Console.Write($"vector.Qx: {vector.Qx:X}\n");
                    Console.Write($"vector.Qy: {vector.Qy:X}\n");

                    Console.Write($"Expected: {vector.IsValid}\n");
                    Console.Write($"Result: {res}\n");

                    return false;
                }
            }

            return true;
        }

        static bool SigVerTest(ECC ecc, string filePath)
        {
            Regex MsgPattern    = new Regex(@"Msg\s*=\s*([0-9a-fA-F]+)");
            Regex QxPattern     = new Regex(@"Qx\s*=\s*([0-9a-fA-F]+)");
            Regex QyPattern     = new Regex(@"Qy\s*=\s*([0-9a-fA-F]+)");
            Regex RPattern      = new Regex(@"R\s*=\s*([0-9a-fA-F]+)");
            Regex SPattern      = new Regex(@"S\s*=\s*([0-9a-fA-F]+)");
            Regex ResultPattern = new Regex(@"Result\s*=\s*(P|F)\s*\((\d+).*?\)");

            List<ECCTestVector> vectors = new List<ECCTestVector>();
            bool msg_flag = false, qx_flag = false, qy_flag = false, r_flag = false, s_flag = false, isValid_flag = false;
            BigInteger qx = 0, qy = 0, r = 0, s = 0; bool isValid = false; byte[] msg = null;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (MsgPattern.IsMatch(line))
                {
                    msg = HexStringToByteArray(MsgPattern.Match(line).Groups[1].Value);
                    msg_flag = true;
                }
                else if (QxPattern.IsMatch(line))
                {
                    qx = HexStringToBigInteger(QxPattern.Match(line).Groups[1].Value);
                    qx_flag = true;
                }
                else if (QyPattern.IsMatch(line))
                {
                    qy = HexStringToBigInteger(QyPattern.Match(line).Groups[1].Value);
                    qy_flag = true;
                }
                else if (RPattern.IsMatch(line))
                {
                    r = HexStringToBigInteger(RPattern.Match(line).Groups[1].Value);
                    r_flag = true;
                }
                else if (SPattern.IsMatch(line))
                {
                    s = HexStringToBigInteger(SPattern.Match(line).Groups[1].Value);
                    s_flag = true;
                }
                else if (ResultPattern.IsMatch(line))
                {
                    isValid = ResultPattern.Match(line).Groups[1].Value == "P";
                    isValid_flag = true;
                }

                if (msg_flag && qx_flag && qy_flag && r_flag && s_flag && isValid_flag)
                {
                    vectors.Add(new ECCTestVector
                    {
                        Msg = msg,
                        Qx = qx,
                        Qy = qy,
                        R = r,
                        S = s,
                        IsValid = isValid,
                    });
                    msg_flag = qx_flag = qy_flag = r_flag = s_flag = isValid_flag = false;
                }
            }

            foreach (ECCTestVector vector in vectors)
            {
                bool res = ecc.Verify(vector.Msg, vector.R, vector.S, new ECPoint(vector.Qx, vector.Qy));
                if ( (res && !vector.IsValid) || (!res && vector.IsValid) )
                {
                    Console.Write("vector.Msg: ");
                    printHex(vector.Msg);
                    Console.WriteLine();

                    Console.Write($"vector.Qx: {vector.Qx:X}\n");
                    Console.Write($"vector.Qy: {vector.Qy:X}\n");
                    Console.Write($"vector.R:  {vector.R:X}\n");
                    Console.Write($"vector.S:  {vector.S:X}\n");

                    Console.Write($"Expected: {vector.IsValid}\n");
                    Console.Write($"Result: {res}\n");

                    return false;
                }
            }

            return true;
        }

        static bool SigGenTest(ECC ecc, string filePath)
        {
            Regex MsgPattern    = new Regex(@"Msg\s*=\s*([0-9a-fA-F]+)");
            Regex KeyPattern    = new Regex(@"d\s*=\s*([0-9a-fA-F]+)");
            Regex QxPattern     = new Regex(@"Qx\s*=\s*([0-9a-fA-F]+)");
            Regex QyPattern     = new Regex(@"Qy\s*=\s*([0-9a-fA-F]+)");
            Regex KPattern      = new Regex(@"k\s*=\s*([0-9a-fA-F]+)");
            Regex RPattern      = new Regex(@"R\s*=\s*([0-9a-fA-F]+)");
            Regex SPattern      = new Regex(@"S\s*=\s*([0-9a-fA-F]+)");
            
            List<ECCTestVector> vectors = new List<ECCTestVector>();
            bool msg_flag = false, d_flag = false, qx_flag = false, qy_flag = false, k_flag = false, r_flag = false, s_flag = false;
            BigInteger d = 0, qx = 0, qy = 0, k = 0, r = 0, s = 0; byte[] msg = null;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (MsgPattern.IsMatch(line))
                {
                    msg = HexStringToByteArray(MsgPattern.Match(line).Groups[1].Value);
                    msg_flag = true;
                }
                else if (KeyPattern.IsMatch(line))
                {
                    d = HexStringToBigInteger(KeyPattern.Match(line).Groups[1].Value);
                    d_flag = true;
                }
                else if (QxPattern.IsMatch(line))
                {
                    qx = HexStringToBigInteger(QxPattern.Match(line).Groups[1].Value);
                    qx_flag = true;
                }
                else if (QyPattern.IsMatch(line))
                {
                    qy = HexStringToBigInteger(QyPattern.Match(line).Groups[1].Value);
                    qy_flag = true;
                }
                else if (KPattern.IsMatch(line))
                {
                    k = HexStringToBigInteger(KPattern.Match(line).Groups[1].Value);
                    k_flag = true;
                }
                else if (RPattern.IsMatch(line))
                {
                    r = HexStringToBigInteger(RPattern.Match(line).Groups[1].Value);
                    r_flag = true;
                }
                else if (SPattern.IsMatch(line))
                {
                    s = HexStringToBigInteger(SPattern.Match(line).Groups[1].Value);
                    s_flag = true;
                }

                if (msg_flag && d_flag && qx_flag && qy_flag && k_flag && r_flag && s_flag)
                {
                    vectors.Add(new ECCTestVector
                    {
                        Msg = msg,
                        Key = d,
                        Qx = qx,
                        Qy = qy,
                        K = k,
                        R = r,
                        S = s,
                    });
                    msg_flag = d_flag = qx_flag = qy_flag = k_flag = r_flag = s_flag = false;
                }
            }

            foreach (ECCTestVector vector in vectors)
            {
                byte[] rBytes = vector.R.ToByteArray(isUnsigned: true, isBigEndian: true);
                byte[] sBytes = vector.S.ToByteArray(isUnsigned: true, isBigEndian: true);
                byte[] TestSignature = new byte[64];
                Array.Copy(rBytes, 0, TestSignature, 0, rBytes.Length);
                Array.Copy(sBytes, 0, TestSignature, rBytes.Length, rBytes.Length);

                byte[] signature = ecc.Sign(vector.Msg, vector.Key, vector.K);
                if (!signature.SequenceEqual(TestSignature))
                {
                    Console.Write("vector.Msg: ");
                    printHex(vector.Msg);
                    Console.WriteLine();

                    Console.Write($"vector.Key: {vector.Key:X}\n");
                    Console.Write($"vector.Qx:  {vector.Qx:X}\n");
                    Console.Write($"vector.Qy:  {vector.Qy:X}\n");
                    Console.Write($"vector.K:   {vector.K:X}\n");
                    Console.Write($"vector.R:   {vector.R:X}\n");
                    Console.Write($"vector.S:   {vector.S:X}\n");

                    Console.Write("Calculated Signature: ");
                    printHex(signature);
                    Console.WriteLine();

                    return false;
                }
            }

            return true;
        }

        static BigInteger HexStringToBigInteger(string hexString)
        {
            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(hexString.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return new BigInteger(bytes, isUnsigned: true, isBigEndian: true);
        }

        static byte[] HexStringToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];

            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        static void printHex(byte[] data)
        {
            Console.WriteLine(BitConverter.ToString(data).Replace("-", " "));
        }
    }
}
