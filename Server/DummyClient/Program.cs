using DummyClient.Session;
using NetworkCore;
using System.Net;

namespace DummyClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string ipString = "127.0.0.1";
            int port = 7777;

            IPAddress ipAddr = IPAddress.Parse(ipString);
            IPEndPoint endPoint = new IPEndPoint(ipAddr, port);

            Connector connector = new Connector();

            connector.Connect(endPoint, () => { return new ServerSession(); }, 100);

            while (true)
            {
                Thread.Sleep(100);
            }
        }
    }
}
