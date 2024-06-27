using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastEncryption.EncryptionAlgorithm
{
    internal interface IEncryptionAlgorithm
    {
        byte[] Encrypt(byte[] plainText);
        byte[] Decrypt(byte[] cipherText);
    }
}
