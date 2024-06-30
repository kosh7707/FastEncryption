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
            TestCTR.Run();
            TestOFB.Run();

            Console.WriteLine("press any key to continue...");
            Console.ReadKey();
        }
    }
}


