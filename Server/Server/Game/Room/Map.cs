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
                if (!_sessions.ContainsKey(session.SessionId))
                {
                    ret = true;
                    _sessions.Add(session.SessionId, session);
                    _players.Add(session.Player.PlayerId, session.Player);
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
                    ret = true;
                    _sessions.Remove(session.SessionId);
                    _sessions.Remove(session.Player.PlayerId);
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

        public List<Player> GetPlayers()
        {
            List<Player> players = new List<Player>();
            foreach (Player player in _players.Values) 
                players.Add(player);
            return players;
        }
    }
}
