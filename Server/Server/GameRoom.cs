using ServerCore;
using System.Security.Cryptography.X509Certificates;

namespace Server
{
    internal class GameRoom : IJobQueue
    {
        List<ClientSession> _sessions = new();
        JobQueue _jobQueue = new();
        List<ArraySegment<byte>> _pendingList = new();

        public void Push(Action job)
        {
            _jobQueue.Push(job);
        }

        public void Broadcast(ArraySegment<byte> segment)
        {
            _pendingList.Add(segment);
        }

        public void Flush()
        {
            foreach (ClientSession s in _sessions)
                s.Send(_pendingList);

            _pendingList.Clear();
        }

        public void Enter(ClientSession session)
        {
            _sessions.Add(session);
            session.Room = this;

            // 현재 GameRoom에 있는 player list를 세션에 전달
            S_PlayerList players = new();
            foreach (ClientSession s in _sessions)
            {
                players.players.Add(new S_PlayerList.Player()
                {
                    isSelf = (s == session), 
                    playerId = s.SessionId, 
                    posX = s.PosX,
                    posY = s.PosY,  
                    posZ = s.PosZ,
                });
            }
            session.Send(players.Write());

            // 입장 이벤트 브로드캐스트
            S_BroadcastEnterGame enter = new();
            enter.playerId = session.SessionId;
            enter.posX = 0;
            enter.posY = 0;
            enter.posZ = 0;
            Broadcast(enter.Write());
        }

        public void Leave(ClientSession session)
        {
            _sessions.Remove(session);

            S_BroadcastLeaveGame leave = new();
            leave.playerId = session.SessionId;
            Broadcast(leave.Write());
        }

        public void Move(ClientSession session, C_Move packet)
        {
            // 좌표 얻기
            session.PosX = packet.posX;
            session.PosY = packet.posY;
            session.PosZ = packet.posZ;

            // 브로드캐스트
            S_BroadcastMove move = new();
            move.playerId = session.SessionId;
            move.posX = packet.posX;
            move.posY = packet.posY;
            move.posZ = packet.posZ;
            Broadcast(move.Write());
        }
    }
}
