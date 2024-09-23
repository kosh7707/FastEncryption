using NetworkCore.Log;
using NetworkCore.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server.Session
{
    public class ClientSession : PacketSession
    {
        public int SessionId { get; set; }

        ClientPacketHandler clientPacketHandler = new();

        public override void OnConnected(EndPoint endPoint)
        {
            Logger.InfoLog($"[Client {SessionId}]: Connected.");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Logger.InfoLog($"[Client {SessionId}]: Disconnected.");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            clientPacketHandler.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int numOfBytes)
        {

        }
    }
}
