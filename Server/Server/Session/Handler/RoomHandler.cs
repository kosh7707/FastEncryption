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
    public class RoomHandler
    {
        public static void C_EnterRoomHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_EnterRoom enterRoomPkt = packet as C_EnterRoom;


        }

        public static void C_LeaveRoomHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_LeaveRoom leaveRoomPkt = packet as C_LeaveRoom;


        }
    }
}
