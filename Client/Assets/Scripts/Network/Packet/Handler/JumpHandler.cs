using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using Session;

namespace Network.Packet.Handler
{
    public class JumpHandler
    {
        public static void S_JumpHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_Jump jumpPkt = packet as S_Jump;
        }

        public static void S_FallHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_Fall fallPkt = packet as S_Fall;
        }

        public static void S_LandHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_Land landPkt = packet as S_Land;
        }
    }
}
