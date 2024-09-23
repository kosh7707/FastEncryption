using NetworkCore.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Session
{
    public class ClientPacketHandler : PacketHandler
    {
        public ClientPacketHandler()
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
