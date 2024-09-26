using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using Session;

namespace Network.Packet.Handler
{
    public class MapHandler
    {
        public static void S_EnterMapHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_EnterMap enterMapPkt = packet as S_EnterMap;
        }

        public static void S_LeaveMapHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_LeaveMap leaveMapPkt = packet as S_LeaveMap;

        }

        public static void S_EnterMapBroadcastHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_EnterMapBroadcast broadcastPkt = packet as S_EnterMapBroadcast;
        }
    }
}