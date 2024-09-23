using Google.Protobuf;
using NetworkCore.Job;
using NetworkCore.Log;
using Server.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Room
{
    public class Map : JobSerializer
    {
        object _lock = new object();

        public int MapId { get; set; }

        // <SessionId, ClientSession>
        Dictionary<int, ClientSession> _sessions = new();

        public static Map LoadMap(int mapId)
        {
            Map map = new();
            map.MapId = mapId; 
            return map;
        }

        public Map()
        {
            Init();
        }

        void Init()
        {

        }

        public void Update()
        {
            Flush();
        }
        
        public bool Enter(ClientSession session)
        {
            bool ret = true;

            lock (_lock)
            {
                if (!_sessions.ContainsKey(session.SessionId))
                {
                    ret &= true;
                    _sessions.Add(session.SessionId, session);
                }
                else
                {
                    ret = false;
                    Logger.ErrorLog($"Can't Enter SessionId: {session.SessionId}");
                }
            }
            
            return ret;
        }

        public bool Leave(ClientSession session)
        {
            bool ret = true;

            lock (_lock)
            {
                if (_sessions.ContainsKey(session.SessionId))
                {
                    ret &= true;
                    _sessions.Remove(session.SessionId);
                }
                else
                {
                    ret = false;
                    Logger.ErrorLog($"Can't Leave SessionId: {session.SessionId}");
                }
            }

            return ret;
        }

        public void Broadcast(IMessage packet)
        {
            foreach (ClientSession session in _sessions.Values)
                session.Send(packet);
        }
    }
}
