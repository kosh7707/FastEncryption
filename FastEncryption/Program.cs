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

            // testGCM();

        }
        static void printHex(byte[] data)
        {
            Console.Write(BitConverter.ToString(data).Replace("-", " "));
            Console.WriteLine("\n");
        }

        public static byte[] HexStringToByteStream(string hexString)
        {
            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(hexString.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return bytes;
        }

        static void testGCM()
        {
            byte[] key = HexStringToByteStream("a49a5e26a2f8cb63d05546c2a62f5343");
            byte[] iv = HexStringToByteStream("907763b19b9b4ab6bd4f0281");
            byte[] PlainText = HexStringToByteStream("");
            byte[] aad = HexStringToByteStream("");
            byte[] CipherText = HexStringToByteStream("");
            byte[] Tag = HexStringToByteStream("a2be08210d8c470a8df6e8fbd79ec5cf");


            AES aes = new(key);
            GCM gcm = new(aes);

            (byte[] cipherText, byte[] authTag) = gcm.Encrypt(PlainText, iv, aad);
            (bool validationCheck, byte[] plainText) = gcm.Decrypt(cipherText, iv, aad, Tag);

            Console.WriteLine("Key: ");
            printHex(key);
            Console.WriteLine("IV: ");
            printHex(iv);
            Console.WriteLine("PT: ");
            printHex(PlainText);
            Console.WriteLine("AAD: ");
            printHex(aad);
            Console.WriteLine("CT: ");
            printHex(cipherText);
            Console.WriteLine("Tag: ");
            printHex(authTag);
            Console.WriteLine(validationCheck);
        }
    }
}


