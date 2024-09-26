using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Session.Handler
{
    public class JumpHandler
    {
        public static void C_JumpHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_Jump jumpPkt = packet as C_Jump;


        }

        public static void C_FallHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_Fall fallPkt = packet as C_Fall;


        }

        public static void C_LandHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_Land landPkt = packet as C_Land;


        }
    }
}
