using FastEncryption.EncryptionAlgorithm;
using System;

namespace FastEncryption
{
    class Program
    {
        static void printHex(byte[] data)
        {
            Console.Write(BitConverter.ToString(data).Replace("-", " "));
        }

        static void TestAES()
        {
            Console.WriteLine("\n\n--- TestAES ---");
            byte[] key          = { 0x2b, 0x7e, 0x15, 0x16, 0x28, 0xae, 0xd2, 0xa6, 0xab, 0xf7, 0x15, 0x88, 0x09, 0xcf, 0x4f, 0x3c };
            byte[] plainText    = { 0x65, 0x66, 0x67, 0x68, 0x69, 0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x80 };

            AES aes = new(key);

            Console.Write("key:         ");
            printHex(key);
            Console.Write("\n\n");

            Console.Write("plainText:   ");
            printHex(plainText);
            Console.Write("\n");

            Console.Write("cipherText:  ");
            byte[] encryptText = aes.Encrypt(plainText);
            printHex(encryptText);
            Console.Write("\n");

            Console.Write("decryptText: ");
            byte[] decryptText = aes.Decrypt(encryptText);
            printHex(decryptText);
            Console.Write("\n");

            if (Enumerable.SequenceEqual(plainText, decryptText))
            {
                Console.WriteLine("Test Success");
            }
        }

        static void TestARIA()
        {
            Console.WriteLine("\n\n--- TestARIA ---");
            byte[] key          = { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff };
            byte[] plainText    = { 0x11, 0x11, 0x11, 0x11, 0xaa, 0xaa, 0xaa, 0xaa, 0x11, 0x11, 0x11, 0x11, 0xbb, 0xbb, 0xbb, 0xbb };

            ARIA aria = new(key);

            Console.Write("key:         "); 
            printHex(key); 
            Console.Write("\n\n");

            Console.Write("plainText:   "); 
            printHex(plainText); 
            Console.Write("\n");

            Console.Write("cipherText:  ");
            byte[] encryptText = aria.Encrypt(plainText);
            printHex(encryptText);
            Console.Write("\n");

            Console.Write("decryptText: ");
            byte[] decryptText = aria.Decrypt(encryptText);
            printHex(decryptText);
            Console.Write("\n");

            if (Enumerable.SequenceEqual(plainText, decryptText))
            {
                Console.WriteLine("Test Success");
            }
        }

        static void TestHIGHT()
        {
            Console.WriteLine("\n\n--- TestHIGHT ---");
            byte[] key          = { 0x88, 0xE3, 0x4F, 0x8F, 0x08, 0x17, 0x79, 0xF1, 0xE9, 0xF3, 0x94, 0x37, 0x0A, 0xD4, 0x05, 0x89 };
            byte[] plainText    = { 0xD7, 0x6D, 0x0D, 0x18, 0x32, 0x7E, 0xC5, 0x62 };

            HIGHT hight = new(key);

            Console.Write("key:         ");
            printHex(key);
            Console.Write("\n\n");

            Console.Write("plainText:   ");
            printHex(plainText);
            Console.Write("\n");

            Console.Write("cipherText:  ");
            byte[] encryptText = hight.Encrypt(plainText);
            printHex(encryptText);
            Console.Write("\n");

            Console.Write("decryptText: ");
            byte[] decryptText = hight.Decrypt(encryptText);
            printHex(decryptText);
            Console.Write("\n");

            if (Enumerable.SequenceEqual(plainText, decryptText))
            {
                Console.WriteLine("Test Success");
            }
        }

        static void TestTWINE()
        {
            Console.WriteLine("\n\n--- TestTWINE ---");
            byte[] key = { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
            byte[] plainText = { 0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef };

            TWINE twine = new(key);

            Console.Write("key:         ");
            printHex(key);
            Console.Write("\n\n");

            Console.Write("plainText:   ");
            printHex(plainText);
            Console.Write("\n");

            Console.Write("cipherText:  ");
            byte[] encryptText = twine.Encrypt(plainText);
            printHex(encryptText);
            Console.Write("\n");

            Console.Write("decryptText: ");
            byte[] decryptText = twine.Decrypt(encryptText);
            printHex(decryptText);
            Console.Write("\n");

            if (Enumerable.SequenceEqual(plainText, decryptText))
            {
                Console.WriteLine("Test Success");
            }
        }

        static void TestSPECK()
        {
            Console.WriteLine("\n\n--- TestSPECK ---");
            byte[] key          = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
            byte[] plainText    = { 0x20, 0x6D, 0x61, 0x64, 0x65, 0x20, 0x69, 0x74, 0x20, 0x65, 0x71, 0x75, 0x69, 0x76, 0x61, 0x6C };

            SPECK speck = new(key);

            Console.Write("key:         ");
            printHex(key);
            Console.Write("\n\n");

            Console.Write("plainText:   ");
            printHex(plainText);
            Console.Write("\n");

            Console.Write("cipherText:  ");
            byte[] encryptText = speck.Encrypt(plainText);
            printHex(encryptText);
            Console.Write("\n");

            Console.Write("decryptText: ");
            byte[] decryptText = speck.Decrypt(encryptText);
            printHex(decryptText);
            Console.Write("\n");

            if (Enumerable.SequenceEqual(plainText, decryptText))
            {
                Console.WriteLine("Test Success");
            }
        }

        static void Main()
        {
            TestAES();
            TestARIA();
            TestHIGHT();
            TestTWINE();
            TestSPECK();
            Console.WriteLine("press any key to continue...");
            Console.ReadKey();
        }
    }
}


