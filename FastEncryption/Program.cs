using FastEncryption.EncryptionAlgorithm;
using FastEncryption.OperationMode;
using FastEncryption.Test;
using System;
using System.Numerics;

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


