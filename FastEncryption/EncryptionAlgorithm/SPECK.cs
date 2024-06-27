using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastEncryption.EncryptionAlgorithm
{
    internal class SPECK : IEncryptionAlgorithm
    {
        public SPECK(byte[] key) 
        {
            if (key.Length != 16)
                throw new ArgumentException("Key must be exactly 16 bytes long.");

            this.key = new byte[16];
            Array.Copy(key, this.key, 16);


        }

        public byte[] Encrypt(byte[] plainText)
        {

        }

        public byte[] Decrypt(byte[] cipherText)
        {

        }

        private readonly byte[] key;
    }
}
