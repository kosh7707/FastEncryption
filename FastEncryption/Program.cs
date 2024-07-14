using FastEncryption.EncryptionAlgorithm;
using FastEncryption.Test;
using System;

namespace FastEncryption
{
    class Program
    {
        static void Main()
        {
            AESTest.Run();
            ARIATest.Run();
            HIGHTTest.Run();
            SPECKTest.Run();
            TWINETest.Run();
        }
    }
}


