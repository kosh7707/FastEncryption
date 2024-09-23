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
    public class LoginHandler
    {
        public static void C_LoginHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_Login loginPkt = packet as C_Login;


        }
    }
}
