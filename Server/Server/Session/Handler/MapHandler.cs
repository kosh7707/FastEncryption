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
    public class MapHandler
    {
        public static void C_EnterMapHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_EnterMap enterMapPkt = packet as C_EnterMap;


        }

        public static void C_LeaveMapHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_LeaveMap leaveMapPkt = packet as C_LeaveMap;


        }
    }
}
