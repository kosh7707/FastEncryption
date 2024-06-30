using FastEncryption.EncryptionAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastEncryption.Test
{
    internal class TestAlgorithm
    {

        public TestAlgorithm(IEncryptionAlgorithm encryptionAlgorithm, byte[] key, byte[] plainText) 
        {
            this.encryptionAlgorithm = encryptionAlgorithm;
            
            this.key = new byte[key.Length];
            Array.Copy(key, this.key, key.Length);

            this.plainText = new byte[plainText.Length];
            Array.Copy(plainText, this.plainText, plainText.Length);

        }

        public bool Run()
        {
            Console.WriteLine($"\n----- Test {encryptionAlgorithm.AlgorithmName} -----");
            
            Console.Write("key:         ");
            printHex(key);
            Console.Write("\n");

            Console.Write("plainText:   ");
            printHex(plainText);
            Console.Write("\n");

            Console.Write("cipherText:  ");
            byte[] encryptText = encryptionAlgorithm.Encrypt(plainText);
            printHex(encryptText);
            Console.Write("\n");

            Console.Write("decryptText: ");
            byte[] decryptText = encryptionAlgorithm.Decrypt(encryptText);
            printHex(decryptText);
            Console.Write("\n");

            if (Enumerable.SequenceEqual(plainText, decryptText))
            {
                Console.WriteLine("----- Test Success -----\n");
                return true;
            }
            Console.WriteLine("----- Test Failed -----\n");
            return false;
        }

        private static void printHex(byte[] data)
        {
            Console.Write(BitConverter.ToString(data).Replace("-", " "));
        }

        private readonly IEncryptionAlgorithm encryptionAlgorithm;
        private readonly byte[] key;
        private readonly byte[] plainText;
    }
}
