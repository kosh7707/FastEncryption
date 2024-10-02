using Google.Protobuf;
using Google.Protobuf.Security;
using NetworkCore.Encryption.BlockCipher.Algorithm;
using NetworkCore.Encryption.BlockCipher.OperationMode;
using NetworkCore.Encryption.PublicKey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NetworkCore.Packet.Security
{
    public class SessionKeyExchangeHandler
    { 
        public static void C_HelloHandler(PacketSession session, IMessage packet)
        {
            C_Hello helloPacket = packet as C_Hello;

            bool isSuccess = true;

            // Cipher Suite Validation Check
            isSuccess &= IsValidCipherSuite(helloPacket.CipherSuite);

            // Packet Parsing
            CipherSuite cipherSuite = helloPacket.CipherSuite;
            byte[] clientPubKeyXArray = helloPacket.PubKeyX.ToByteArray();
            byte[] clientPubKeyYArray = helloPacket.PubKeyY.ToByteArray();
            BigInteger clientPubKeyX = new BigInteger(clientPubKeyXArray, isUnsigned: true, isBigEndian: true);
            BigInteger clientPubKeyY = new BigInteger(clientPubKeyYArray, isUnsigned: true, isBigEndian: true);
            ECPoint clientPubKey = new ECPoint(clientPubKeyX, clientPubKeyY);

            // Client Public Key Validation Check
            isSuccess &= (session.ECDSA.IsValidPoint(clientPubKey) == 0);

            // Create PrivKey
            BigInteger privKey = session.ECDH.GeneratePrivKey();

            // Calculate PubKey
            ECPoint pubKey = session.ECDH.GetPublicKey(privKey);
            byte[] pubKeyX = pubKey.X.ToByteArray(isUnsigned: true, isBigEndian: true);
            byte[] pubKeyY = pubKey.Y.ToByteArray(isUnsigned: true, isBigEndian: true);

            // Calculate SharedSecret
            BigInteger sharedSecret = session.ECDH.ComputeSharedSecret(privKey, clientPubKey);
            byte[] sharedSecretArray = sharedSecret.ToByteArray(isUnsigned: true, isBigEndian: true);

            // Signature
            byte[] message = ServerSignature.Instance.Message;
            byte[] signature = ServerSignature.Instance.Signature;

            // Set CipherSuite, PrivKey, PubKey, SharedSecret
            session.CipherSuite = cipherSuite;
            session.PrivKey = privKey;
            session.PubKey = pubKey;
            Array.Copy(sharedSecretArray, session.SharedSecret, sharedSecretArray.Length);

            // Set salt (HKDF)
            byte[] salt = new byte[16];
            using (System.Security.Cryptography.RandomNumberGenerator rng = System.Security.Cryptography.RandomNumberGenerator.Create())
                rng.GetBytes(salt);
            Array.Copy(salt, session.Salt, salt.Length);

            // Send Packet
            S_Hello resHello = new S_Hello();
            resHello.Success = isSuccess;
            resHello.PubKeyX = ByteString.CopyFrom(pubKeyX);
            resHello.PubKeyY = ByteString.CopyFrom(pubKeyY);
            resHello.Message = ByteString.CopyFrom(message);
            resHello.Signature = ByteString.CopyFrom(signature);
            resHello.Salt = ByteString.CopyFrom(salt);
            session.Send(resHello);
        }

        public static void S_HelloHandler(PacketSession session, IMessage packet)
        {
            S_Hello helloPacket = packet as S_Hello;

            bool isSuccess = true;

            // Validation Check
            if (!helloPacket.Success) 
                throw new Exception("S_HelloHandler SessionKeyExchangeError");

            // Packet Parsing
            byte[] serverPubKeyXArray = helloPacket.PubKeyX.ToByteArray();
            byte[] serverPubKeyYArray = helloPacket.PubKeyY.ToByteArray();
            byte[] serverMessage = helloPacket.Message.ToByteArray();
            byte[] serverSignature = helloPacket.Signature.ToByteArray();
            byte[] salt = helloPacket.Salt.ToByteArray();   
            BigInteger serverPubKeyX = new BigInteger(serverPubKeyXArray, isUnsigned: true, isBigEndian: true);
            BigInteger serverPubKeyY = new BigInteger(serverPubKeyYArray, isUnsigned: true, isBigEndian: true);
            ECPoint serverPubKey = new ECPoint(serverPubKeyX, serverPubKeyY);

            // Sigature Verification
            ECPoint serverSignaturePubKey = CertificateAuthority.Instance.ServerSignaturePubKey;
            isSuccess &= session.ECDSA.Verify(serverMessage, serverSignature, serverSignaturePubKey);

            // Calculate SharedSecret
            BigInteger sharedSecret = session.ECDH.ComputeSharedSecret(session.PrivKey, serverPubKey);
            byte[] sharedSecretArray = sharedSecret.ToByteArray(isUnsigned: true, isBigEndian: true);

            // Set SharedSecret, Salt
            Array.Copy(sharedSecretArray, session.SharedSecret, sharedSecretArray.Length);
            Array.Copy(salt, session.Salt, salt.Length);

            // Validation Check
            isSuccess &= true;

            // Send Packet
            C_Hello_Done resHelloDone = new C_Hello_Done();
            resHelloDone.Success = isSuccess;
            session.Send(resHelloDone);
        }

        public static void C_HelloDoneHandler(PacketSession session, IMessage packet)
        {
            C_Hello_Done helloDonePacket = packet as C_Hello_Done;

            // Validation Check
            if (!helloDonePacket.Success)
                throw new Exception("SessionKeyExchangeError");

            // Set BlockCipher
            session.OperationMode = GetOperationModeFromCipherSuite(session.CipherSuite, session.SessionKey);

            // Send Packet
            S_Hello_Done resHelloDone = new S_Hello_Done();
            resHelloDone.Success = true;
            session.Send(resHelloDone);

            // SessionKey Derivation and Set
            byte[] sessionKey = HKDF.KeyDerivation(session.SharedSecret, session.Salt, null, 16);
            Array.Copy(sessionKey, session.SessionKey, sessionKey.Length);

            // Set Secure Communication Flag
            session.IsSecure = true;

            // Debug
            Logger.DebugLog($"C_HelloDone, SessionKey: {BitConverter.ToString(session.SessionKey).Replace("-", " ")}");
        }

        public static void S_HelloDoneHandler(PacketSession session, IMessage packet)
        {
            S_Hello_Done helloDonePacket = packet as S_Hello_Done;

            // Validation Check
            if (!helloDonePacket.Success)
                throw new Exception("SessionKeyExchangeError");

            // Set BlockCipher
            session.OperationMode = GetOperationModeFromCipherSuite(session.CipherSuite, session.SessionKey);

            // SessionKey Derivation and Set
            byte[] sessionKey = HKDF.KeyDerivation(session.SharedSecret, session.Salt, null, 16);
            Array.Copy(sessionKey, session.SessionKey, sessionKey.Length);

            // Set Secure Communication Flag
            session.IsSecure = true;

            // Debug
            Logger.DebugLog($"S_HelloDone, SessionKey: {BitConverter.ToString(session.SessionKey).Replace("-", " ")}");
        }

        static IOperationMode GetOperationModeFromCipherSuite(CipherSuite cipherSuite, byte[] sessionKey)
        {
            string cipherSuiteString = cipherSuite.ToString();

            var parts = System.Text.RegularExpressions.Regex.Split(cipherSuiteString, @"(?<!^)(?=[A-Z])");

            if (parts.Length != 2)
                throw new ArgumentException("Invalid CipherSuite format");

            string encryptionAlgorithmName = parts[0].ToUpper();
            string operationModeName = parts[1].ToUpper();

            IEncryptionAlgorithm encryptionAlgorithm = CreateInstance<IEncryptionAlgorithm>(encryptionAlgorithmName, sessionKey);

            IOperationMode operationMode = CreateInstance<IOperationMode>(operationModeName, encryptionAlgorithm);

            return operationMode;
        }

        static T CreateInstance<T>(string className, params object[] args)
        {
            var type = Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name.Equals(className, StringComparison.OrdinalIgnoreCase) && typeof(T).IsAssignableFrom(t));

            if (type == null)
            {
                throw new ArgumentException($"Type not found: {className}");
            }

            return (T)Activator.CreateInstance(type, args);
        }

        static bool IsValidCipherSuite(CipherSuite cipherSuite)
        {
            return Enum.IsDefined(typeof(CipherSuite), cipherSuite);
        }
    }
}
