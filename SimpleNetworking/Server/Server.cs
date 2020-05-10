using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace SimpleNetworking
{
    

    public class Server : IServer
    {
        private TcpListener tcpListener;

        public event DisconnectedEventHandler ClientDisconnected;
        public event ConnectedEventHandler ClientConnected;


        public int MaxConnections { get; private set; }
        public int Port { get; private set; }
        //public Dictionary<int, IServerClient> Clients { get; private set; } = new Dictionary<int, IServerClient>();
        public List<ServerClient> Clients { get; private set; } = new List<ServerClient>();
        public Queue<IPacket> ReceivedPackets { get; private set; } = new Queue<IPacket>();


        public Server()
        {

        }
       

        public void StartListening(int maxConnections, int port)
        {
            MaxConnections = maxConnections;
            Port = port;
            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(TcpConnectionCallback, null);
        }
        public void StopListening()
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
            ServerClient client = Clients.Find(c => c.Id == id);
            if (client == null)
            {
                client = new ServerClient(this);
                client.Id = id;
                client.Ip = ip;
                client.PacketReceived += OnClientReceivedPacket;
                client.Connected += OnClientConnected;
                client.Disconnected += OnClientDisconnected;
                client.Tcp.SetConnectedTcpClient(tcpClient);
                Clients.Add(client);
            }
            else
            {
                client.Tcp.SetConnectedTcpClient(tcpClient);
            }
        }
        private void AddClient(UdpClient udpClient)
        {
            throw new NotImplementedException();
        }
        private void RemoveClient(int clientId)
        {
            ServerClient client = Clients.Find(c => c.Id == clientId);
            client.Dispose();
            Clients.Remove(Clients.Find(c => c.Id == clientId));
        }        
        private void OnClientReceivedPacket(IPacket packet)
        {
            ReceivedPackets.Enqueue(packet);
        }

        private void OnClientDisconnected(Exception e, ProtocolType type, int clientId)
        {
            ClientDisconnected?.Invoke(e, type, clientId);
        }
        private void OnClientConnected(ProtocolType type, int clientId)
        {
            ClientConnected?.Invoke(type, clientId);
        }

    }
}
