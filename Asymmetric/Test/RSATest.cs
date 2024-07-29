using Asymmetric.EncryptionAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Asymmetric.Test
{
    internal class RSATest
    {
        static void Run()
        {
            Console.WriteLine("\n******* RSA TEST START *******");

            // KeyGen(키 생성)
            // SigGen(서명 생성)
            // SigVer(서명 검증)

            /*
            RSA rsa = new(n);
            byte[] message = Encoding.UTF8.GetBytes("Hello, I am Kosh.");

            // 서명 생성
            byte[] signature = rsa.Sign(message, d);
            Console.WriteLine($"Signature:\n{BitConverter.ToString(signature).Replace("-", " ")}");

            // 서명 검증
            bool isValid = rsa.Verify(message, signature, e);
            Console.WriteLine($"Signature is valid: {isValid}");
            */

            Console.WriteLine("\n******* RSA TEST END *******");
        }
    }
}
