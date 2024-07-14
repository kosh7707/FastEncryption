using FastEncryption.EncryptionAlgorithm;
using FastEncryption.OperationMode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FastEncryption.Test
{
    internal class AESTest
    {
        public static void Run()
        {
            Console.WriteLine("\n******* AES TEST START *******");
            ECB();
            CBC();
            CFB();
            OFB();
            Console.WriteLine("\n******* AES TEST END *********");
        }

        internal class AESTestVector
        {
            public byte[] Key { get; set; }
            public byte[] IV { get; set; }
            public byte[] PlainText { get; set; }
            public byte[] CipherText { get; set; }
        }

        static void ECB()
        {
            Console.WriteLine();
            string folderPath = "../../../Test/test-vector/AES/ECB/";
            string[] filePaths = Directory.GetFiles(folderPath, "*.rsp");

            foreach (string filePath in filePaths)
            {
                bool flag = true;
                List<AESTestVector> vectors = ReadTestVector(filePath);

                foreach (AESTestVector vector in vectors)
                {
                    AES aes = new(vector.Key);
                    ECB ecb = new(aes);
                    byte[] cipherText = ecb.Encrypt(vector.PlainText);

                    if (!SequenceEqual(cipherText, vector.CipherText, 0, 16)
                        || !SequenceEqual(ecb.Decrypt(cipherText), vector.PlainText, 0, vector.PlainText.Length))
                    {
                        Console.Write("Key: ");
                        printHex(vector.Key);
                        Console.Write("\n"); 

                        Console.Write("PlainText: ");
                        printHex(vector.PlainText);
                        Console.Write("\n");

                        Console.Write("CipherText: ");
                        printHex(cipherText);
                        Console.Write("\n");

                        Console.Write("Vector.CipherText: ");
                        printHex(vector.CipherText);
                        Console.Write("\n");

                        Console.Write("DecryptText: ");
                        printHex(ecb.Decrypt(cipherText));
                        Console.Write("\n");

                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    Console.WriteLine($"[AES - ECB] [Success]\t {Path.GetFileName(filePath)}");
                }
                else
                {
                    Console.WriteLine($"[AES - ECB] [Fail]\t {Path.GetFileName(filePath)}");
                }
            }
        }

        static void CBC()
        {
            Console.WriteLine();
            string folderPath = "../../../Test/test-vector/AES/CBC/";
            string[] filePaths = Directory.GetFiles(folderPath, "*.rsp");

            foreach (string filePath in filePaths)
            {
                bool flag = true;
                List<AESTestVector> vectors = ReadTestVectorWithIV(filePath);

                foreach (AESTestVector vector in vectors)
                {
                    AES aes = new(vector.Key);
                    CBC cbc = new(aes);
                    byte[] cipherText = cbc.Encrypt(vector.PlainText, vector.IV);

                    if (!SequenceEqual(cipherText, vector.CipherText, 16, vector.CipherText.Length + 16)
                        || !SequenceEqual(cbc.Decrypt(cipherText), vector.PlainText, 0, vector.PlainText.Length))
                    {
                        Console.Write("Key: ");
                        printHex(vector.Key);
                        Console.Write("\n");

                        Console.Write("PlainText: ");
                        printHex(vector.PlainText);
                        Console.Write("\n");

                        Console.Write("CipherText: ");
                        printHex(cipherText);
                        Console.Write("\n");

                        Console.Write("Vector.CipherText: ");
                        printHex(vector.CipherText);
                        Console.Write("\n");

                        Console.Write("DecryptText: ");
                        printHex(cbc.Decrypt(cipherText));
                        Console.Write("\n");

                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    Console.WriteLine($"[AES - CBC] [Success]\t {Path.GetFileName(filePath)}");
                }
                else
                {
                    Console.WriteLine($"[AES - CBC] [Fail]\t {Path.GetFileName(filePath)}");
                }
            }
        }

        static void CFB()
        {
            Console.WriteLine();
            string folderPath = "../../../Test/test-vector/AES/CFB/";
            string[] filePaths = Directory.GetFiles(folderPath, "*.rsp");

            foreach (string filePath in filePaths)
            {
                bool flag = true;
                List<AESTestVector> vectors = ReadTestVectorWithIV(filePath);

                foreach (AESTestVector vector in vectors)
                {
                    AES aes = new(vector.Key);
                    CFB cfb = new(aes);
                    byte[] cipherText = cfb.Encrypt(vector.PlainText, vector.IV);

                    if (!SequenceEqual(cipherText, vector.CipherText, 16, vector.CipherText.Length + 16)
                        || !SequenceEqual(cfb.Decrypt(cipherText), vector.PlainText, 0, vector.PlainText.Length))
                    {
                        Console.Write("Key: ");
                        printHex(vector.Key);
                        Console.Write("\n");

                        Console.Write("PlainText: ");
                        printHex(vector.PlainText);
                        Console.Write("\n");

                        Console.Write("CipherText: ");
                        printHex(cipherText);
                        Console.Write("\n");

                        Console.Write("Vector.CipherText: ");
                        printHex(vector.CipherText);
                        Console.Write("\n");

                        Console.Write("DecryptText: ");
                        printHex(cfb.Decrypt(cipherText));
                        Console.Write("\n");

                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    Console.WriteLine($"[AES - CFB] [Success]\t {Path.GetFileName(filePath)}");
                }
                else
                {
                    Console.WriteLine($"[AES - CFB] [Fail]\t {Path.GetFileName(filePath)}");
                }
            }
        }

        static void OFB()
        {
            Console.WriteLine();
            string folderPath = "../../../Test/test-vector/AES/OFB/";
            string[] filePaths = Directory.GetFiles(folderPath, "*.rsp");

            foreach (string filePath in filePaths)
            {
                bool flag = true;
                List<AESTestVector> vectors = ReadTestVectorWithIV(filePath);

                foreach (AESTestVector vector in vectors)
                {
                    AES aes = new(vector.Key);
                    OFB ofb = new(aes);
                    byte[] cipherText = ofb.Encrypt(vector.PlainText, vector.IV);

                    if (!SequenceEqual(cipherText, vector.CipherText, 16, vector.CipherText.Length + 16)
                        || !SequenceEqual(ofb.Decrypt(cipherText), vector.PlainText, 0, vector.PlainText.Length))
                    {
                        Console.Write("Key: ");
                        printHex(vector.Key);
                        Console.Write("\n");

                        Console.Write("PlainText: ");
                        printHex(vector.PlainText);
                        Console.Write("\n");

                        Console.Write("CipherText: ");
                        printHex(cipherText);
                        Console.Write("\n");

                        Console.Write("Vector.CipherText: ");
                        printHex(vector.CipherText);
                        Console.Write("\n");

                        Console.Write("DecryptText: ");
                        printHex(ofb.Decrypt(cipherText));
                        Console.Write("\n");

                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    Console.WriteLine($"[AES - OFB] [Success]\t {Path.GetFileName(filePath)}");
                }
                else
                {
                    Console.WriteLine($"[AES - OFB] [Fail]\t {Path.GetFileName(filePath)}");
                }
            }
        }

        static List<AESTestVector> ReadTestVector(string filePath)
        {
            Regex keyPattern = new Regex(@"KEY\s*=\s*([0-9a-fA-F]+)");
            Regex plaintextPattern = new Regex(@"PLAINTEXT\s*=\s*([0-9a-fA-F]+)");
            Regex ciphertextPattern = new Regex(@"CIPHERTEXT\s*=\s*([0-9a-fA-F]+)");

            List<AESTestVector> vectors = new List<AESTestVector>();
            byte[] key = null, plainText = null, cipherText = null; 

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (keyPattern.IsMatch(line))
                {
                    key = HexStringToByteArray(keyPattern.Match(line).Groups[1].Value);
                }
                else if (plaintextPattern.IsMatch(line))
                {
                    plainText = HexStringToByteArray(plaintextPattern.Match(line).Groups[1].Value);
                }
                else if (ciphertextPattern.IsMatch(line))
                {
                    cipherText = HexStringToByteArray(ciphertextPattern.Match(line).Groups[1].Value);
                }

                if (key != null && plainText != null && cipherText != null)
                {
                    vectors.Add(new AESTestVector
                    {
                        Key = key, PlainText = plainText, CipherText = cipherText
                    });
                    key = plainText = cipherText = null;
                }
            }

            return vectors;
        }

        static List<AESTestVector> ReadTestVectorWithIV(string filePath)
        {
            Regex keyPattern = new Regex(@"KEY\s*=\s*([0-9a-fA-F]+)");
            Regex ivPattern = new Regex(@"IV\s*=\s*([0-9a-fA-F]+)");
            Regex plaintextPattern = new Regex(@"PLAINTEXT\s*=\s*([0-9a-fA-F]+)");
            Regex ciphertextPattern = new Regex(@"CIPHERTEXT\s*=\s*([0-9a-fA-F]+)");

            List<AESTestVector> vectors = new List<AESTestVector>();
            byte[] key = null, iv = null, plainText = null, cipherText = null;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (keyPattern.IsMatch(line))
                {
                    key = HexStringToByteArray(keyPattern.Match(line).Groups[1].Value);
                }
                else if (ivPattern.IsMatch(line))
                {
                    iv = HexStringToByteArray(ivPattern.Match(line).Groups[1].Value);
                }
                else if (plaintextPattern.IsMatch(line))
                {
                    plainText = HexStringToByteArray(plaintextPattern.Match(line).Groups[1].Value);
                }
                else if (ciphertextPattern.IsMatch(line))
                {
                    cipherText = HexStringToByteArray(ciphertextPattern.Match(line).Groups[1].Value);
                }

                if (key != null && iv != null && plainText != null && cipherText != null)
                {
                    vectors.Add(new AESTestVector
                    {
                        Key = key,
                        IV = iv,
                        PlainText = plainText,
                        CipherText = cipherText
                    });
                    key = iv = plainText = cipherText = null;
                }
            }

            return vectors;
        }

        static void printHex(byte[] data)
        {
            Console.Write(BitConverter.ToString(data).Replace("-", " "));
        }

        static byte[] HexStringToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];

            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        static bool SequenceEqual(byte[] cipherText, byte[] vectorCipherText, int s, int e) 
        {
            for (int i = s; i < e; i++)
            {
                if (cipherText[i] != vectorCipherText[i - s]) 
                    return false;
            }
            return true;
        }
    }
}
