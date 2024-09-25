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

            S_EnterRoom resPkt = new S_EnterRoom();

            // RoomId 체크
            Room? room = RoomManager.Instance.GetRoom(roomId);
            if (room == null)
            {
                // Invalid RoomId
                resPkt.Success = false;
                clientSession.Send(resPkt);
                return;
            }

            // Room 입장 시도
            if (!room.Enter(clientSession))
            {
                // Room 입장 실패
                resPkt.Success = false;
                clientSession.Send(resPkt);
                return;
            }

            // Room 입장 성공
            clientSession.RoomId = roomId;

            resPkt.Success = true;
            clientSession.Send(resPkt);
        }

        public static void C_LeaveRoomHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_LeaveRoom leaveRoomPkt = packet as C_LeaveRoom;

            int roomId = clientSession.RoomId;

            S_LeaveRoom resPkt = new S_LeaveRoom();

            // RoomId 체크
            Room? room = RoomManager.Instance.GetRoom(roomId);
            if (room == null)
            {
                // Invalid RoomId
                resPkt.Success = false;
                clientSession.Send(resPkt);
                return;
            }

            // Room 퇴장 시도
            if (!room.Leave(clientSession))
            {
                // Room 퇴장 실패
                resPkt.Success = false;
                clientSession.Send(resPkt);
                return;
            }

            // Room 퇴장 성공
            clientSession.RoomId = -1;

            resPkt.Success = true;
            clientSession.Send(resPkt);
        }
    }
}
