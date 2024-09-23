using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using Session;

namespace Network.Packet.Handler
{
    public class MoveHandler
    {
        public static void S_MoveStartHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_MoveStart moveStartPkt = packet as S_MoveStart;
        }

        public static void S_MovingHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_Moving movingPkt = packet as S_Moving;
        }

        public static void S_MoveEndHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_MoveEnd moveEndPkt = packet as S_MoveEnd;
        }
    }
}