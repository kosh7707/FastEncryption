using Google.Protobuf;
using Google.Protobuf.Security;
using NetworkCore.Encryption.PublicKey;
using NetworkCore.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using NetworkCore;

namespace DummyClient.Session
{
    public class ServerSession : PacketSession
    {
        ServerPacketHandler serverPacketHandler = new();

        public override void OnConnected(EndPoint endPoint)
        {
            Logger.InfoLog("[Server]: Successfully Connected.");

            StartSessionKeyExchange();
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Logger.InfoLog("[Server]: Successfully Disconnected.");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            serverPacketHandler.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int numOfBytes)
        {
        }
    }
}
