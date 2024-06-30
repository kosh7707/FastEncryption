using FastEncryption.EncryptionAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastEncryption.OperationMode
{
    internal class OFB(IEncryptionAlgorithm encryptionAlgorithm) : OperationMode(encryptionAlgorithm)
    {
        public override byte[] Encrypt(byte[] plainText)
        {
            int blockSize = encryptionAlgorithm.GetBlockSize();
            byte[] paddedPlainText = Padding(plainText);
            byte[] cipherText = new byte[paddedPlainText.Length];

            // TODO

            return cipherText;
        }

        public override byte[] Decrypt(byte[] cipherText)
        {
            int blockSize = encryptionAlgorithm.GetBlockSize();
            byte[] paddedPlainText = new byte[cipherText.Length];

            // TODO

            return UnPadding(paddedPlainText);
        }

        public override string ModeName => "OFB";
        public override string AlgorithmName => encryptionAlgorithm.AlgorithmName;
    }
}
