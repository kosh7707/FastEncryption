using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using Session;

namespace Network.Packet.Handler
{
    public class RegisterHandler
    {
        public static void S_RegisterHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_Register registerPkt = packet as S_Register; 
        }
    }
}