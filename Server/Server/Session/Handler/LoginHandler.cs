using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using Server.Game.Account;
using Server.Game.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Session.Handler
{
    public class LoginHandler
    {
        public static void C_LoginHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_Login loginPkt = packet as C_Login;

            /*
            message C_Login
            {
	            string id = 1;
	            string pw = 2;
            }

            message S_Login
            {
	            bool success = 1;
	            RoomInfo roomInfo = 2;
	            int32 errorCode = 3;
            }
             */

            string id = loginPkt.Id;
            string pw = loginPkt.Pw;

            Account account = AccountDB.Instance.GetAccount(id);
            S_Login resPkt = new S_Login();
            if (account == null)
            {
                // 1: 없는 아이디
                resPkt.Success = false;
                resPkt.ErrorCode = 1;
                clientSession.Send(resPkt);
                return;
            }

            if (!account.PasswordVerify(pw))
            {
                // 2: 잘못된 비밀번호
                resPkt.Success = false;
                resPkt.ErrorCode = 2;
                clientSession.Send(resPkt);
                return; 
            }

            resPkt.Success = true;
            resPkt.Player = account.Player;
            foreach (RoomInfo roomInfo in RoomManager.Instance.GetRoomInfo())
                resPkt.RoomInfo.Add(roomInfo);
            clientSession.Send(resPkt);

            clientSession.Account = account;
            clientSession.Player = account.Player;
        }
    }
}
