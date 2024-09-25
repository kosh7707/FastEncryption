using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using Server.Game.Room;
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

            int roomId = enterRoomPkt.RoomId;

            // Room 입장 시도
            if (RoomManager.Instance.EnterRoom(roomId, clientSession))
            {
                clientSession.RoomId = roomId;

                // Room 입장 성공
                S_EnterRoom resPkt = new S_EnterRoom();
                resPkt.Success = true;
                clientSession.Send(resPkt);
            }
            else
            {
                // Room 입장 실패
                S_EnterRoom resPkt = new S_EnterRoom();
                resPkt.Success = false;
                clientSession.Send(resPkt);
            }
        }

        public static void C_LeaveRoomHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_LeaveRoom leaveRoomPkt = packet as C_LeaveRoom;

            int roomId = clientSession.RoomId;

            // Room 퇴장 시도
            if (RoomManager.Instance.LeaveRoom(roomId, clientSession))
            {
                clientSession.RoomId = -1;

                // Room 퇴장 성공
                S_LeaveRoom resPkt = new S_LeaveRoom();
                resPkt.Success = true;
                clientSession.Send(resPkt);
            }
            else
            {
                // Room 퇴장 실패
                S_LeaveRoom resPkt = new S_LeaveRoom();
                resPkt.Success = false;
                clientSession.Send(resPkt);
            }
        }
    }
}
