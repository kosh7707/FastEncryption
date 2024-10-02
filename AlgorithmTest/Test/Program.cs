using System.Data.SqlTypes;
using System.Numerics;
using System.Security.Cryptography;
using Test.BlockCipherTest;
using Test.Encryption.BlockCipher.Algorithm;
using Test.Encryption.BlockCipher.OperationMode;
using Test.PublicKeyTest;

namespace Test
{
    public class Program
    {
        static void BlockCipherTest()
        {
            AESTest.Run();
            ARIATest.Run();
            HIGHTTest.Run();
            SPECKTest.Run();
            TWINETest.Run();
        }

        static void PublicKeyTest()
        {
            ECDSATest.Run();
            ECDHTest.Run();
        }

        public static void Main(string[] args)
        {
            BlockCipherTest();
            PublicKeyTest();
        }
        
    }
}