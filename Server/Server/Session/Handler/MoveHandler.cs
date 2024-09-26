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
    public class MoveHandler
    {
        public static void C_MoveStartHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_MoveStart moveStartPkt = packet as C_MoveStart;


        }
        public static void C_MovingHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_Moving movingPkt = packet as C_Moving;


        }

        public static void C_MoveEndHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_MoveEnd moveEndPkt = packet as C_MoveEnd;


        }
    }
}
