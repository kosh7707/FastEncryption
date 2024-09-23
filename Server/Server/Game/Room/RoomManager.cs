using Google.Protobuf;
using NetworkCore.Log;
using Server.Session;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Room
{
    public class RoomManager
    {
        object _lock = new object();    

        // <SessionId, ClientSession>
        Dictionary<int, ClientSession> _sessions = new();

        // <RoomId, Room>
        Dictionary<int, Room> _rooms = new();

        static RoomManager _instance = new RoomManager();
        public static RoomManager Instance
        {
            get => _instance;
        }

        RoomManager()
        {
            Init();
        }

        void Init()
        {
            _rooms.Add(1, Room.LoadRoom(1));
            _rooms.Add(2, Room.LoadRoom(2));
            _rooms.Add(3, Room.LoadRoom(3));
        }

        public void Update()
        {
            foreach (Room room in _rooms.Values)
                room.Update();
        }

        bool EnterRoom(int roomId, int mapId, ClientSession session)
        {
            bool ret = true;

            if (_rooms.TryGetValue(roomId, out Room room))
            {
                if (!_sessions.ContainsKey(session.SessionId))
                {
                    _sessions.Add(session.SessionId, session);
                    ret &= _rooms[roomId].EnterMap(mapId, session);
                }
                else
                {
                    ret = false;
                    Logger.ErrorLog($"Can't EnterRoom SessionId: {session.SessionId}");
                }
            }
            else
            {
                ret = false;
                Logger.ErrorLog($"Can't EnterRoom SessionId: {session.SessionId}");
            }

            return ret;
        }

        public bool LeaveRoom(int roomId, int mapId, ClientSession session)
        {
            bool ret = true;

            if (_rooms.TryGetValue(roomId, out Room room))
            {
                if (!_sessions.ContainsKey(session.SessionId))
                {
                    _sessions.Remove(session.SessionId);
                    ret &= _rooms[roomId].LeaveMap(mapId, session);
                }
                else
                {
                    ret = false;
                    Logger.ErrorLog($"Can't EnterRoom SessionId: {session.SessionId}");
                }
            }
            else
            {
                ret = false;
                Logger.ErrorLog($"Can't EnterRoom SessionId: {session.SessionId}");
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
