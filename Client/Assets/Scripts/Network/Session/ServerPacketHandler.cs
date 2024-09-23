using Google.Protobuf;
using NetworkCore.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class ServerPacketHandler : PacketHandler
    {
        public ServerPacketHandler()
        {
            AddHandler();
            AddOnRecv();
        }

        protected override void AddHandler()
        {
        }

        protected override void AddOnRecv()
        {
        }

        public Action<PacketSession, IMessage> GetPacketHandler(ushort id)
        {
            Action<PacketSession, IMessage> action = null;
            if (_handler.TryGetValue(id, out action))
                return action;
            return null;
        }
    }
}
