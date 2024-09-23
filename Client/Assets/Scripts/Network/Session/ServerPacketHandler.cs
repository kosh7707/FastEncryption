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
    }
}
