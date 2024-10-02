using Google.Protobuf;
using Google.Protobuf.Protocol;
using Network.Packet.Handler;
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

        protected override void AddOnRecv()
        {
            // Register
            _onRecv.Add((ushort)MsgId.SRegister, MakePacket<S_Register>);

            // Login
            _onRecv.Add((ushort)MsgId.SLogin, MakePacket<S_Login>);

            // Room
            _onRecv.Add((ushort)MsgId.SEnterroom, MakePacket<S_EnterRoom>);
            _onRecv.Add((ushort)MsgId.SLeaveroom, MakePacket<S_LeaveRoom>);

            // Map
            _onRecv.Add((ushort)MsgId.SEntermap, MakePacket<S_EnterMap>);
            _onRecv.Add((ushort)MsgId.SLeavemap, MakePacket<S_LeaveMap>);
            _onRecv.Add((ushort)MsgId.SEntermapbroadcast, MakePacket<S_EnterMapBroadcast>);

            // Chat
            _onRecv.Add((ushort)MsgId.SChat, MakePacket<S_Chat>);

            // Move
            _onRecv.Add((ushort)MsgId.SMovestart, MakePacket<S_MoveStart>);
            _onRecv.Add((ushort)MsgId.SMoving, MakePacket<S_Moving>);
            _onRecv.Add((ushort)MsgId.SMoveend, MakePacket<S_MoveEnd>);

            // Jump
            _onRecv.Add((ushort)MsgId.SJump, MakePacket<S_Jump>);
            _onRecv.Add((ushort)MsgId.SFall, MakePacket<S_Fall>);
            _onRecv.Add((ushort)MsgId.SLand, MakePacket<S_Land>);

            // Skill
            _onRecv.Add((ushort)MsgId.SSkill, MakePacket<S_Skill>);

            // Hit
            _onRecv.Add((ushort)MsgId.SHit, MakePacket<S_Hit>);

            // Test
            _onRecv.Add((ushort)MsgId.STest, MakePacket<S_Test>);
        }

        protected override void AddHandler()
        {
            // Register
            _handler.Add((ushort)MsgId.SRegister, RegisterHandler.S_RegisterHandler);
            
            // Login
            _handler.Add((ushort)MsgId.SLogin, LoginHandler.S_LoginHandler);

            // Room
            _handler.Add((ushort)MsgId.SEnterroom, RoomHandler.S_EnterRoomHandler);
            _handler.Add((ushort)MsgId.SLeaveroom, RoomHandler.S_LeaveRoomHandler);

            // Map
            _handler.Add((ushort)MsgId.SEntermap, MapHandler.S_EnterMapHandler);
            _handler.Add((ushort)MsgId.SLeavemap, MapHandler.S_LeaveMapHandler);
            _handler.Add((ushort)MsgId.SEntermapbroadcast, MapHandler.S_EnterMapBroadcastHandler);

            // Chat
            _handler.Add((ushort)MsgId.SChat, ChatHandler.S_ChatHandler);

            // Move
            _handler.Add((ushort)MsgId.SMovestart, MoveHandler.S_MoveStartHandler);
            _handler.Add((ushort)MsgId.SMoving, MoveHandler.S_MovingHandler);
            _handler.Add((ushort)MsgId.SMoveend, MoveHandler.S_MoveEndHandler);

            // Jump
            _handler.Add((ushort)MsgId.SJump, JumpHandler.S_JumpHandler);
            _handler.Add((ushort)MsgId.SFall, JumpHandler.S_FallHandler);
            _handler.Add((ushort)MsgId.SLand, JumpHandler.S_LandHandler);

            // Skill
            _handler.Add((ushort)MsgId.SSkill, SkillHandler.S_SkillHandler);

            // Hit
            _handler.Add((ushort)MsgId.SHit, HitHandler.S_HitHandler);

            // Test
            _handler.Add((ushort)MsgId.STest, TestHandler.S_TestHandler);
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
