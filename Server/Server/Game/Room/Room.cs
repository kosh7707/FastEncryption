using Google.Protobuf;
using NetworkCore.Job;
using Server.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore;

namespace Server.Game.Room
{
    public class Room : JobSerializer
    {
        object _lock = new object();

        public int RoomId { get; set; }
        public string Name { get; set; }

        // <SessionId, ClientSession>
        Dictionary<int, ClientSession> _sessions = new();

        // <MapId, Map>
        Dictionary<int, Map> _maps = new();
        
        public static Room LoadRoom(int roomId)
        {
            Room room = new Room();
            room.RoomId = roomId;
            room.Name = $"Room {roomId}";
            return room;
        }

        public Room()
        {
            Init();
        }

        void Init()
        {
            _maps.Add(1, Map.LoadMap(1));
            _maps.Add(2, Map.LoadMap(2));
            _maps.Add(3, Map.LoadMap(3));
            _maps.Add(4, Map.LoadMap(4));
            _maps.Add(5, Map.LoadMap(5));
        }

        public void Update()
        {
            foreach (Map map in _maps.Values)
                map.Update();

            Flush();
        }

        public bool EnterMap(int mapId, ClientSession session)
        {
            bool ret = true;

            lock (_lock)
            {
                if (_maps.TryGetValue(mapId, out Map map))
                {
                    if (!_sessions.ContainsKey(session.SessionId))
                    {
                        _sessions.Add(session.SessionId, session);
                        ret &= _maps[mapId].Enter(session);
                    }
                    else
                    {
                        ret = false;
                        Logger.ErrorLog($"Can't EnterMap SessionId: {session.SessionId}");
                    }
                }
                else
                {
                    ret = false;
                    Logger.ErrorLog($"Can't EnterMap SessionId: {session.SessionId}");
                }
            }

            return ret;
        }

        public bool LeaveMap(int mapId, ClientSession session)
        {
            bool ret = true;

            lock ( _lock)
            {
                if (_maps.TryGetValue(mapId, out Map map))
                {
                    if (!_sessions.ContainsKey(session.SessionId))
                    {
                        _sessions.Remove(session.SessionId);
                        ret &= _maps[mapId].Leave(session);
                    }
                    else
                    {
                        ret = false;
                        Logger.ErrorLog($"Can't EnterMap SessionId: {session.SessionId}");
                    }
                }
                else
                {
                    ret = false;
                    Logger.ErrorLog($"Can't EnterMap SessionId: {session.SessionId}");
                }
            }

            return ret;
        }

        public void Broadcast(IMessage packet)
        {
            foreach (ClientSession session in _sessions.Values)
                session.Send(packet);
        }

        public Map GetMap(int mapId)
        {
            _maps.TryGetValue(mapId, out Map map);
            return map;
        }
    }
}
