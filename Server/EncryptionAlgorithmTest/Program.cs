using EncryptionAlgorithmTest.SymmetricTest;
using EncryptionAlgorithmTest.AsymmetricTest;

namespace EncryptionAlgorithmTest {
    internal class Program
    {
        static void Test1()
        {
            AESTest.Run();
            ARIATest.Run();
            HIGHTTest.Run();
            SPECKTest.Run();
            TWINETest.Run();
        }

        static void Test2()
        {
            ECCTest.Run();
        }

        public static void Main(string[] args)
        {
            // Test1();
            Test2();
        }

    }
}