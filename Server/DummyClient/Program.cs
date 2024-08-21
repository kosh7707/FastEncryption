using ServerCore;
using System.Net;

namespace DummyClient
{
    class Program
    {
        static void Main(string[] args) 
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new(ipAddr, 7777);

            Connector connector = new();

            connector.Connect(endPoint, () => { return SessionManager.Instance.Generate(); }, 10);

            while (true)
            {
                try
                {
                    SessionManager.Instance.SendForEach();
                    Thread.Sleep(250);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }
}
