using FastEncryption.EncryptionAlgorithm;
using FastEncryption.Test;
using System;

namespace FastEncryption
{
    class Program
    {
        static void Main()
        {
            TestSingleBlock.Run();
            TestECB.Run();
            TestCBC.Run();
            TestCFB.Run();
            TestOFB.Run();
            TestCTR.Run();

            Console.WriteLine("press any key to continue...");
            Console.ReadKey();
        }
    }
}


