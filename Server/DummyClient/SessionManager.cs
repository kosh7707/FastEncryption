namespace DummyClient
{
    internal class SessionManager
    {
        static SessionManager _session = new();
        public static SessionManager Instance { get { return _session; } }  

        List<ServerSession> _sessions = new List<ServerSession>();
        object _lock = new object();
        Random _rand = new();

        public ServerSession Generate()
        {
            lock (_lock)
            {
                ServerSession session = new ServerSession();
                _sessions.Add(session);
                return session;
            }
        }

        public void SendForEach()
        {
            lock (_lock)
            {
                foreach (ServerSession session in _sessions)
                {
                    C_Move movePacket = new();
                    movePacket.posX = _rand.Next(-50, 50);
                    movePacket.posY = 0;
                    movePacket.posZ = _rand.Next(-50, 50);
                    session.Send(movePacket.Write());
                }
            }
        }
    }
}
