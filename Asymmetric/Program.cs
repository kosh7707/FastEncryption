using Asymmetric.EncryptionAlgorithm;
using System.Numerics;
using System.Text;

namespace Asymmetric
{
    class Program
    {
        static void Test_RSA()
        {
            Console.WriteLine("\n\nRSA test");
            BigInteger e = HexStringToBigInteger("00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000df28ab"),
                       p = HexStringToBigInteger("e021757c777288dacfe67cb2e59dc02c70a8cebf56262336592c18dcf466e0a4ed405318ac406bd79eca29183901a557db556dd06f7c6bea175dcb8460b6b1bc05832b01eedf86463238b7cb6643deef66bc4f57bf8ff7ec7c4b8a8af14f478980aabedd42afa530ca47849f0151b7736aa4cd2ff37f322a9034de791ebe3f51"),
                       q = HexStringToBigInteger("ed1571a9e0cd4a42541284a9f98b54a6af67d399d55ef888b9fe9ef76a61e892c0bfbb87544e7b24a60535a65de422830252b45d2033819ca32b1a9c4413fa721f4a24ebb5510ddc9fd6f4c09dfc29cb9594650620ff551a62d53edc2f8ebf10beb86f483d463774e5801f3bb01c4d452acb86ecfade1c7df601cab68b065275"),
                       n = HexStringToBigInteger("cf91c0065d8e5797f0d1b1b3f4c31ac3d5e83d4be67ada7625b4b3a80fd24eda86c88b88c7fc4b2c60e311215abf8abc34e21c047035a93bbcb43387c6a44c7149c278ae27488ddc09ce56eacc7fbd437de9699d660b1dba4923ba60c9bb1fc69b7c90468ace5b7715d5d385c02e59f525ea01625c5c61760d8b23b962b7c80d14e6d58064a6064749ffdd260b0a8b0b2ffb846a6586e375a04163b61fb0af71dddf56e65478ae2101f30dc37a3f035f10ff86f53ac2e073b3a5b5f7017c6e6704b23e83c357aa171b23c49ff4d3820a03229a1962bf2e6b90acf79570f269e679292255755ad6362129870460c00799c5179e27a3b5182ee07c6ba07420e205"),
                       d = HexStringToBigInteger("1f5201b880a206cb123fb73afc2f266baac9c431afd3c584eb12abd3c6aaa106bc1eb9b034dc7b61803ca7a3a74e371f865de8af27e7d97c5287c9ed91f5c1da02ba44156aa857136685c03256fb9586567fa73a5a17c341d073aae3758fc3676f3fc87bbc2dd684915ec6c3370fa349e2b6bed9e82a8f5fb2ea3bea65a3818968081bdd80f7e046c6b5b8bdac85120d95c243725162cfc9034ae14634d14674e0c0c10f1a5e93af74152d67bf872e039fead73755c8e28f2da34f3b7eb1286deda90e09514a281cb7013a519b93e1b347728fa56543e0c3348d646e67b7f6d2481c41f6c02454cc9e6ed07b1ecf1a44857802191da376bae5027d4c3b0c6473");

            RSA rsa = new(n);
            byte[] message = Encoding.UTF8.GetBytes("Hello, I am Kosh.");

            // 서명 생성
            byte[] signature = rsa.Sign(message, d);
            Console.WriteLine($"Signature:\n{BitConverter.ToString(signature).Replace("-", " ")}");

            // 서명 검증
            bool isValid = rsa.Verify(message, signature, e);
            Console.WriteLine($"Signature is valid: {isValid}");
        }

        static void Test_ECC()
        {
            Console.WriteLine("\n\nECC test");
            BigInteger a        = HexStringToBigInteger("ffffffff00000001000000000000000000000000fffffffffffffffffffffffc"),
                       b        = HexStringToBigInteger("5ac635d8aa3a93e7b3ebbd55769886bc651d06b0cc53b0f63bce3c3e27d2604b"),
                       p        = HexStringToBigInteger("ffffffff00000001000000000000000000000000ffffffffffffffffffffffff"),
                       n        = HexStringToBigInteger("ffffffff00000000ffffffffffffffffbce6faada7179e84f3b9cac2fc632551"),
                       gx       = HexStringToBigInteger("6b17d1f2e12c4247f8bce6e563a440f277037d812deb33a0f4a13945d898c296"),
                       gy       = HexStringToBigInteger("4fe342e2fe1a7f9b8ee7eb4a7c0f9e162bce33576b315ececbb6406837bf51f5"),
                       privKey  = HexStringToBigInteger("ebb2c082fd7727890a28ac82f6bdf97bad8de9f5d7c9028692de1a255cad3e0f");

            ECC ecc = new(a, b, p, n, new ECPoint(gx, gy));
            byte[] message = Encoding.UTF8.GetBytes("Hello, I am Kosh.");

            // 공개키 생성
            privKey %= n;
            ECPoint pubKey = ecc.GetPublicKey(privKey);
            Console.WriteLine($"pubKey.X (Hex): {pubKey.X:X}");
            Console.WriteLine($"pubKey.Y (Hex): {pubKey.Y:X}");

            // 서명 생성
            byte[] signature = ecc.Sign(message, privKey);
            Console.WriteLine($"Signature:\n{BitConverter.ToString(signature).Replace("-", " ")}");

            // 서명 검증
            bool isValid = ecc.Verify(message, signature, pubKey);
            Console.WriteLine($"Signature is valid: {isValid}");
        }

        public static void Main()
        {
            Test_RSA();
            Test_ECC();
        }

        public static BigInteger HexStringToBigInteger(string hexString)
        {
            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(hexString.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return new BigInteger(bytes, isUnsigned: true, isBigEndian: true);
        }
    }
}