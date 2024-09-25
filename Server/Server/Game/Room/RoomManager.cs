using Google.Protobuf;
using Server.Session;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore;
using Google.Protobuf.Protocol;

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

        public bool EnterRoom(int roomId, ClientSession session)
        {
            bool ret = true;

            if (_rooms.TryGetValue(roomId, out Room room))
            {
                if (!_sessions.ContainsKey(session.SessionId))
                {
                    ret = true;
                    _sessions.Add(session.SessionId, session);
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

        public bool LeaveRoom(int roomId, ClientSession session)
        {
            bool ret = true;

            if (_rooms.TryGetValue(roomId, out Room room))
            {
                if (!_sessions.ContainsKey(session.SessionId))
                {
                    ret = true;
                    _sessions.Remove(session.SessionId);
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

        public List<RoomInfo> GetRoomInfo()
        {
            List<RoomInfo> roomInfos = new List<RoomInfo>();
            foreach (Room room in  _rooms.Values)
            {
                RoomInfo roomInfo = new RoomInfo();
                roomInfo.RoomId = room.RoomId;
                roomInfo.RoomTitle = room.Name;
                roomInfos.Add(roomInfo);
            }
            return roomInfos;
        }

        public Room GetRoom(int roomId)
        {
            _rooms.TryGetValue(roomId, out Room room);
            return room;
        }
    }
}
