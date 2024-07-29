using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Asymmetric.Helper
{
    internal class HashDRBG
    {
        private byte[] V;
        private byte[] C;
        private int reseedCounter;
        private readonly int seedLength;
        private readonly SHA256 hash;

        public HashDRBG(byte[] seed)
        {
            hash = SHA256.Create();
            seedLength = 32; // SHA256 output size
            Initialize(seed);
        }

        private void Initialize(byte[] seed)
        {
            V = new byte[seedLength];
            C = new byte[seedLength];
            for (int i = 0; i < seedLength; i++)
            {
                V[i] = 0x01;
            }

            Update(seed);
            reseedCounter = 1;
        }

        private void Update(byte[] provided = null)
        {
            byte[] temp = new byte[seedLength * 2 + 1];
            Array.Copy(V, temp, seedLength);
            temp[seedLength] = 0x00;
            if (provided != null)
            {
                Array.Copy(provided, 0, temp, seedLength + 1, Math.Min(provided.Length, seedLength));
            }

            V = hash.ComputeHash(temp);

            temp[seedLength] = 0x01;
            if (provided != null)
            {
                Array.Copy(provided, 0, temp, seedLength + 1, Math.Min(provided.Length, seedLength));
            }

            C = hash.ComputeHash(temp);
        }

        public byte[] Generate(int numberOfBytes)
        {
            if (reseedCounter > 10000)
                throw new InvalidOperationException("Reseed required");

            byte[] output = new byte[numberOfBytes];
            int outputOffset = 0;

            while (outputOffset < numberOfBytes)
            {
                V = hash.ComputeHash(V);
                int bytesToCopy = Math.Min(V.Length, numberOfBytes - outputOffset);
                Array.Copy(V, 0, output, outputOffset, bytesToCopy);
                outputOffset += bytesToCopy;
            }

            Update();
            reseedCounter++;

            return output;
        }
    }
}
