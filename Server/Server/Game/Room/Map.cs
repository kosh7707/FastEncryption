using Google.Protobuf;
using NetworkCore.Job;
using Server.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore;
using Google.Protobuf.Protocol;

namespace Server.Game.Room
{
    public class Map : JobSerializer
    {
        object _lock = new object();

        public int MapId { get; set; }

        // <SessionId, ClientSession>
        Dictionary<int, ClientSession> _sessions = new();
        Dictionary<int, Player> _players = new();

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
                ret &= _sessions.TryAdd(session.SessionId, session);
                ret &= _players.TryAdd(session.Player.PlayerId, session.Player);
            }
            
            return ret;
        }

        public bool Leave(ClientSession session)
        {
            bool ret = true;

            lock (_lock)
            {
                ret &= _sessions.Remove(session.SessionId);
                ret &= _players.Remove(session.Player.PlayerId);
            }

            return ret;
        }

        public void Broadcast(IMessage packet)
        {
            foreach (ClientSession session in _sessions.Values)
                session.Send(packet);
        }

        public List<Player> GetPlayers()
        {
            List<Player> players = new List<Player>(_players.Values);
            return players;
        }
    }
}
