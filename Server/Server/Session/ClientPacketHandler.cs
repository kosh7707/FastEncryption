using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using Server.Session.Handler;
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
            // Register
            _onRecv.Add((ushort)MsgId.CRegister, MakePacket<C_Register>);

            // Login
            _onRecv.Add((ushort)MsgId.CLogin, MakePacket<C_Login>);

            // Room
            _onRecv.Add((ushort)MsgId.CEnterroom, MakePacket<C_EnterRoom>);
            _onRecv.Add((ushort)MsgId.CLeaveroom, MakePacket<C_LeaveRoom>);
            
            // Map
            _onRecv.Add((ushort)MsgId.CEntermap, MakePacket<C_EnterMap>);
            _onRecv.Add((ushort)MsgId.CLeavemap, MakePacket<C_LeaveMap>);

            // Chat
            _onRecv.Add((ushort)MsgId.CChat, MakePacket<C_Chat>);

            // Move
            _onRecv.Add((ushort)MsgId.CMovestart, MakePacket<C_MoveStart>);
            _onRecv.Add((ushort)MsgId.CMoving, MakePacket<C_Moving>);
            _onRecv.Add((ushort)MsgId.CMoveend, MakePacket<C_MoveEnd>);

            // Jump
            _onRecv.Add((ushort)MsgId.CJump, MakePacket<C_Jump>);
            _onRecv.Add((ushort)MsgId.CFall, MakePacket<C_Fall>);
            _onRecv.Add((ushort)MsgId.CLand, MakePacket<C_Land>);

            // Skill
            _onRecv.Add((ushort)MsgId.CSkill, MakePacket<C_Skill>);

            // Test
            _onRecv.Add((ushort)MsgId.CTest, MakePacket<C_Test>);
        }

        protected override void AddOnRecv()
        {
            // Register
            _handler.Add((ushort)MsgId.CRegister, RegisterHandler.C_RegisterHandler);

            // Login
            _handler.Add((ushort)MsgId.CLogin, LoginHandler.C_LoginHandler);

            // Room
            _handler.Add((ushort)MsgId.CEnterroom, RoomHandler.C_EnterRoomHandler);
            _handler.Add((ushort)MsgId.CLeaveroom, RoomHandler.C_LeaveRoomHandler);
            
            // Map
            _handler.Add((ushort)MsgId.CEntermap, MapHandler.C_EnterMapHandler);
            _handler.Add((ushort)MsgId.CLeavemap, MapHandler.C_LeaveMapHandler);

            // Chat
            _handler.Add((ushort)MsgId.CChat, ChatHandler.C_ChatHandler);

            // Move
            _handler.Add((ushort)MsgId.CMovestart, MoveHandler.C_MoveStartHandler);
            _handler.Add((ushort)MsgId.CMoving, MoveHandler.C_MovingHandler);
            _handler.Add((ushort)MsgId.CMoveend, MoveHandler.C_MoveEndHandler);

            // Jump
            _handler.Add((ushort)MsgId.CJump, JumpHandler.C_JumpHandler);
            _handler.Add((ushort)MsgId.CFall, JumpHandler.C_FallHandler);
            _handler.Add((ushort)MsgId.CLand, JumpHandler.C_LandHandler);

            // Skill
            _handler.Add((ushort)MsgId.CSkill, SkillHandler.C_SkillHandler);

            // Test
            _handler.Add((ushort)MsgId.CTest, TestHandler.C_TestHandler);
        }
    }
}

