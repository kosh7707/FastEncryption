using FastEncryption.EncryptionAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastEncryption.OperationMode
{
    internal class CTR(IEncryptionAlgorithm encryptionAlgorithm) : OperationMode(encryptionAlgorithm)
    {
        public override byte[] Encrypt(byte[] plainText)
        {
            int blockSize = encryptionAlgorithm.GetBlockSize();
            byte[] cipherText = new byte[plainText.Length + blockSize / 2];

            byte[] nonce = new byte[blockSize / 2];
            new Random().NextBytes(nonce);
            Array.Copy(nonce, 0, cipherText, 0, blockSize / 2);

            byte[] counter = new byte[blockSize];
            Array.Copy(nonce, counter, blockSize / 2);

            for (int i = 0; i < plainText.Length; i += blockSize)
            {
                byte[] encryptedCounter = encryptionAlgorithm.Encrypt(counter);
                IncrementCounter(counter, blockSize);

                int bytesToProcess = Math.Min(blockSize, plainText.Length - i);
                Array.Copy(plainText, i, cipherText, i + blockSize / 2, bytesToProcess); 

                for (int j = 0; j < bytesToProcess; j++)
                {
                    cipherText[i + blockSize / 2 + j] ^= encryptedCounter[j];
                }
            }

            return cipherText;
        }

        public override byte[] Decrypt(byte[] cipherText)
        {
            int blockSize = encryptionAlgorithm.GetBlockSize();
            byte[] plainText = new byte[cipherText.Length - blockSize / 2];

            byte[] counter = new byte[blockSize];
            Array.Copy(cipherText, 0, counter, 0, blockSize / 2);

            for (int i = blockSize / 2; i < cipherText.Length; i += blockSize)
            {
                byte[] encryptedCounter = encryptionAlgorithm.Encrypt(counter);
                IncrementCounter(counter, blockSize);

                int bytesToProcess = Math.Min(blockSize, cipherText.Length - i);

                for (int j = 0; j < bytesToProcess; j++)
                {
                    plainText[i - blockSize / 2 + j] = (byte)(encryptedCounter[j] ^ cipherText[i + j]);    
                }
            }

            return plainText;
        }

        private void IncrementCounter(byte[] counter, int blockSize)
        {
            for (int i = blockSize - 1; i >= blockSize / 2; i--)
            {
                counter[i]++;
                if (counter[i] != 0)
                {
                    break;
                }
            }
        }

        public override string ModeName => "CTR";
        public override string AlgorithmName => encryptionAlgorithm.AlgorithmName;
    }
}
