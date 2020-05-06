using SimpleNetworking.DI;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace SimpleNetworking
{
    

    public class Server : IServer
    {
        private TcpListener tcpListener;
        //private int idCounter = 1;

        public int MaxConnections { get; private set; }
        public int Port { get; private set; }
        //public Dictionary<int, IServerClient> Clients { get; private set; } = new Dictionary<int, IServerClient>();
        public List<IServerClient> Clients { get; private set; } = new List<IServerClient>();
        public Queue<IPacket> ReceivedPackets { get; private set; } = new Queue<IPacket>();


        public Server()
        {

        }
       

        public void Start(int maxConnections, int port)
        {
            MaxConnections = maxConnections;
            Port = port;
            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(TcpConnectionCallback, null);
        }
        public void Stop()
        {
            tcpListener.Stop();
        }
        private void TcpConnectionCallback(IAsyncResult result)
        {
            TcpClient tcpClient = tcpListener.EndAcceptTcpClient(result);
            tcpListener.BeginAcceptTcpClient(TcpConnectionCallback, null);

            if (Clients.Count >= MaxConnections)
            {
                return;
            }
            AddClient(tcpClient);
        }
        private void AddClient(TcpClient tcpClient)
        {
            int id = tcpClient.Client.RemoteEndPoint.GetHashCode();
            string ip = tcpClient.Client.RemoteEndPoint.ToString();
            if (Clients.Find(c => c.Id == id) == null)
            {
                IServerClient client = ServiceLocator.Instance.Get<IServerClient>();
                client.Id = id;
                client.Ip = ip;
                client.Tcp.HandleReceivedPacket = OnClientReceivedPacket;
                client.Tcp.SetConnectedTcpClient(tcpClient);
                Clients.Add(client);
            }
            else
            {
                IServerClient client = Clients.Find(c => c.Id == id);
                client.Tcp.HandleReceivedPacket = OnClientReceivedPacket;
                client.Tcp.SetConnectedTcpClient(tcpClient);
            }
            
            //idCounter += 1;
        }
        private void AddClient(UdpClient udpClient)
        {
            throw new NotImplementedException();
        }
        private void RemoveClient(int clientId)
        {
            Clients.Remove(Clients.Find(c => c.Id == clientId));
            //Clients.Remove(clientId);
        }        
        private void OnClientReceivedPacket(IPacket packet)
        {
            ReceivedPackets.Enqueue(packet);
        }
        
    }
}
