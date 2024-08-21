﻿using System.Net;
using ServerCore;

namespace Server
{
    class Program
    {
        static Listener _listener = new();
        public static GameRoom Room = new();
        
        static void FlushRoom()
        {
            Room.Push(() => Room.Flush());
            JobTimer.Instance.Push(FlushRoom, 250);
        }

        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new(ipAddr, 7777);

            _listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
            Console.WriteLine("Listening...");

            JobTimer.Instance.Push(FlushRoom);
            while (true)
            {
                JobTimer.Instance.Flush();
            }
        }
    }
}