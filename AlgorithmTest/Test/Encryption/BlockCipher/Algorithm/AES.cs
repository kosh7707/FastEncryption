﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Encryption.BlockCipher.Algorithm
{
    public class AES : EncryptionAlgorithm
    {
        public AES(byte[] key) : base(key)
        {
            state = new byte[16];
            roundKey = new byte[176];
            KeyExpansion();
        }

        public override byte[] Encrypt(byte[] plainText)
        {
            if (plainText == null || plainText.Length != 16)
                throw new ArgumentException("Input must be a 16-byte array.");

            Array.Copy(plainText, state, 16);

            Encrypt();

            var ret = new byte[16];
            Array.Copy(state, ret, 16);
            return ret;
        }

        public override byte[] Decrypt(byte[] cipherText)
        {
            if (cipherText == null || cipherText.Length != 16)
                throw new ArgumentException("Input must be a 16-byte array.");

            Array.Copy(cipherText, state, 16);

            Decrypt();

            var ret = new byte[16];
            Array.Copy(state, ret, 16);
            return ret;
        }

        public override string AlgorithmName => "AES";

        public override int GetBlockSize()
        {
            return 16;
        }

        private void KeyExpansion()
        {
            Array.Copy(key, roundKey, 16);

            byte[] temp = new byte[4];
            byte k;
            for (int i = 4; i < 44; i++)
            {
                temp[0] = roundKey[(i - 1) * 4 + 0];
                temp[1] = roundKey[(i - 1) * 4 + 1];
                temp[2] = roundKey[(i - 1) * 4 + 2];
                temp[3] = roundKey[(i - 1) * 4 + 3];

                if (i % 4 == 0)
                {
                    // RotWord
                    k = temp[0];
                    temp[0] = temp[1];
                    temp[1] = temp[2];
                    temp[2] = temp[3];
                    temp[3] = k;
                    // SubWord
                    temp[0] = sbox[temp[0]];
                    temp[1] = sbox[temp[1]];
                    temp[2] = sbox[temp[2]];
                    temp[3] = sbox[temp[3]];
                    // rcon xor
                    temp[0] = (byte)(temp[0] ^ rcon[i / 4 - 1]);
                }

                roundKey[i * 4 + 0] = (byte)(roundKey[(i - 4) * 4 + 0] ^ temp[0]);
                roundKey[i * 4 + 1] = (byte)(roundKey[(i - 4) * 4 + 1] ^ temp[1]);
                roundKey[i * 4 + 2] = (byte)(roundKey[(i - 4) * 4 + 2] ^ temp[2]);
                roundKey[i * 4 + 3] = (byte)(roundKey[(i - 4) * 4 + 3] ^ temp[3]);
            }
        }

        private void AddRoundKey(byte round)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    state[i * 4 + j] ^= roundKey[round * 16 + i * 4 + j];
                }
            }
        }

        private byte xtime(byte x)
        {
            return (byte)((x << 1) ^ (((x >> 7) & 1) * 0x1b) & 0xFF);
        }

        private void SubBytes()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    state[i * 4 + j] = sbox[state[i * 4 + j]];
                }
            }
        }

        private void ShiftRows()
        {
            byte temp;

            temp = state[1];
            state[1] = state[5];
            state[5] = state[9];
            state[9] = state[13];
            state[13] = temp;

            temp = state[2];
            state[2] = state[10];
            state[10] = temp;

            temp = state[6];
            state[6] = state[14];
            state[14] = temp;

            temp = state[15];
            state[15] = state[11];
            state[11] = state[7];
            state[7] = state[3];
            state[3] = temp;
        }

        private void MixColumns()
        {
            byte Tmp, Tm, t;
            for (int i = 0; i < 4; i++)
            {
                t = state[i * 4 + 0];
                Tmp = (byte)(state[i * 4 + 0] ^ state[i * 4 + 1] ^ state[i * 4 + 2] ^ state[i * 4 + 3]);
                Tm = (byte)(state[i * 4 + 0] ^ state[i * 4 + 1]); Tm = xtime(Tm); state[i * 4] ^= (byte)(Tm ^ Tmp);
                Tm = (byte)(state[i * 4 + 1] ^ state[i * 4 + 2]); Tm = xtime(Tm); state[i * 4 + 1] ^= (byte)(Tm ^ Tmp);
                Tm = (byte)(state[i * 4 + 2] ^ state[i * 4 + 3]); Tm = xtime(Tm); state[i * 4 + 2] ^= (byte)(Tm ^ Tmp);
                Tm = (byte)(state[i * 4 + 3] ^ t); Tm = xtime(Tm); state[i * 4 + 3] ^= (byte)(Tm ^ Tmp);
            }
        }

        private void Encrypt()
        {
            AddRoundKey(0);

            for (byte round = 1; round < 10; round++)
            {
                SubBytes();
                ShiftRows();
                MixColumns();
                AddRoundKey(round);
            }

            SubBytes();
            ShiftRows();
            AddRoundKey(10);
        }

        private byte multiply(byte x, byte y)
        {
            return (byte)(((y & 1) * x) ^
                    ((y >> 1 & 1) * xtime(x)) ^
                    ((y >> 2 & 1) * xtime(xtime(x))) ^
                    ((y >> 3 & 1) * xtime(xtime(xtime(x)))) ^
                    ((y >> 4 & 1) * xtime(xtime(xtime(xtime(x))))));
        }

        private void InvSubBytes()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    state[i * 4 + j] = rsbox[state[i * 4 + j]];
                }
            }
        }

        private void InvShiftRows()
        {
            byte temp;

            temp = state[13];
            state[13] = state[9];
            state[9] = state[5];
            state[5] = state[1];
            state[1] = temp;

            temp = state[2];
            state[2] = state[10];
            state[10] = temp;
            temp = state[6];
            state[6] = state[14];
            state[14] = temp;

            temp = state[3];
            state[3] = state[7];
            state[7] = state[11];
            state[11] = state[15];
            state[15] = temp;
        }

        private void InvMixColumns()
        {
            byte t1, t2, t3, t4;
            for (int i = 0; i < 4; i++)
            {
                t1 = state[i * 4 + 0]; t2 = state[i * 4 + 1];
                t3 = state[i * 4 + 2]; t4 = state[i * 4 + 3];

                state[i * 4 + 0] = (byte)(multiply(t1, 0x0e) ^ multiply(t2, 0x0b) ^ multiply(t3, 0x0d) ^ multiply(t4, 0x09));
                state[i * 4 + 1] = (byte)(multiply(t1, 0x09) ^ multiply(t2, 0x0e) ^ multiply(t3, 0x0b) ^ multiply(t4, 0x0d));
                state[i * 4 + 2] = (byte)(multiply(t1, 0x0d) ^ multiply(t2, 0x09) ^ multiply(t3, 0x0e) ^ multiply(t4, 0x0b));
                state[i * 4 + 3] = (byte)(multiply(t1, 0x0b) ^ multiply(t2, 0x0d) ^ multiply(t3, 0x09) ^ multiply(t4, 0x0e));
            }
        }

        private void Decrypt()
        {
            AddRoundKey(10);

            for (byte round = 9; round > 0; round--)
            {
                InvShiftRows();
                InvSubBytes();
                AddRoundKey(round);
                InvMixColumns();
            }

            InvShiftRows();
            InvSubBytes();
            AddRoundKey(0);
        }

        private readonly byte[] roundKey;
        private readonly byte[] state;

        private static readonly byte[] sbox = new byte[256]
        {
            0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76,
            0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0,
            0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15,
            0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75,
            0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84,
            0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf,
            0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8,
            0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2,
            0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73,
            0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb,
            0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79,
            0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08,
            0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a,
            0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e,
            0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf,
            0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16
        };
        private static readonly byte[] rsbox = new byte[256]
        {
            0x52, 0x09, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3, 0x9e, 0x81, 0xf3, 0xd7, 0xfb,
            0x7c, 0xe3, 0x39, 0x82, 0x9b, 0x2f, 0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb,
            0x54, 0x7b, 0x94, 0x32, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 0x0b, 0x42, 0xfa, 0xc3, 0x4e,
            0x08, 0x2e, 0xa1, 0x66, 0x28, 0xd9, 0x24, 0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25,
            0x72, 0xf8, 0xf6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d, 0x65, 0xb6, 0x92,
            0x6c, 0x70, 0x48, 0x50, 0xfd, 0xed, 0xb9, 0xda, 0x5e, 0x15, 0x46, 0x57, 0xa7, 0x8d, 0x9d, 0x84,
            0x90, 0xd8, 0xab, 0x00, 0x8c, 0xbc, 0xd3, 0x0a, 0xf7, 0xe4, 0x58, 0x05, 0xb8, 0xb3, 0x45, 0x06,
            0xd0, 0x2c, 0x1e, 0x8f, 0xca, 0x3f, 0x0f, 0x02, 0xc1, 0xaf, 0xbd, 0x03, 0x01, 0x13, 0x8a, 0x6b,
            0x3a, 0x91, 0x11, 0x41, 0x4f, 0x67, 0xdc, 0xea, 0x97, 0xf2, 0xcf, 0xce, 0xf0, 0xb4, 0xe6, 0x73,
            0x96, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37, 0xe8, 0x1c, 0x75, 0xdf, 0x6e,
            0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29, 0xc5, 0x89, 0x6f, 0xb7, 0x62, 0x0e, 0xaa, 0x18, 0xbe, 0x1b,
            0xfc, 0x56, 0x3e, 0x4b, 0xc6, 0xd2, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe, 0x78, 0xcd, 0x5a, 0xf4,
            0x1f, 0xdd, 0xa8, 0x33, 0x88, 0x07, 0xc7, 0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f,
            0x60, 0x51, 0x7f, 0xa9, 0x19, 0xb5, 0x4a, 0x0d, 0x2d, 0xe5, 0x7a, 0x9f, 0x93, 0xc9, 0x9c, 0xef,
            0xa0, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0, 0xc8, 0xeb, 0xbb, 0x3c, 0x83, 0x53, 0x99, 0x61,
            0x17, 0x2b, 0x04, 0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0c, 0x7d
        };
        private static readonly byte[] rcon = new byte[10]
        {
            0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36
        };
    }
}
