using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleNetworking
{
    public delegate void ClientConnectionChangedEventHandler(object source, ClientConnectionEventArgs args);
    
    public class ClientConnectionEventArgs : EventArgs
    {
        public enum ConnectionStatus
        {
            Connected,
            Disconnected
        }
        public IClient Client { get; }
        public ConnectionStatus Status { get; }
        public ClientConnectionEventArgs(IClient client, ConnectionStatus status)
        {
            Client = client;
            Status = status;


        }
    }

    public class Server : IServer
    {
        private TcpListener tcpListener;
        private int idCounter = 1;
        private IPacketHandler packetHandler;

        public int MaxConnections { get; private set; }
        public int Port { get; private set; }
        public Dictionary<int, IClient> Clients { get; private set; }
        public event ClientConnectionChangedEventHandler ClientConnectionChanged;


        public Server(IPacketHandler packetHandler, int maxConnections, int port)
        {
            this.packetHandler = packetHandler;
            MaxConnections = maxConnections;
            Port = port;
            Clients = new Dictionary<int, IClient>();
            tcpListener = new TcpListener(IPAddress.Any, Port);
        }

        public void Start()
        {
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(TcpConnectionCallback, null);
            //tcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectionCallback), null);
        }
        public void Stop()
        {
            tcpListener.Stop();
        }
        private void TcpConnectionCallback(IAsyncResult result)
        {
            TcpClient tcpClient = tcpListener.EndAcceptTcpClient(result);
            tcpListener.BeginAcceptTcpClient(TcpConnectionCallback, null);
            //tcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectionCallback), null);

            if (Clients.Count >= MaxConnections)
            {
                return;
            }
            AddClient(tcpClient);
            

        }
        private void AddClient(TcpClient tcpClient)
        {
            Client client = new Client(new TcpHandler(tcpClient));
            client.Id = idCounter;
            Clients.Add(idCounter, client);
            idCounter += 1;
            ClientConnectionChanged?.Invoke(this, new ClientConnectionEventArgs(client, ClientConnectionEventArgs.ConnectionStatus.Connected));
        }
        private void RemoveClient(int id)
        {
            var client = Clients[id];
            Clients.Remove(id);
            ClientConnectionChanged?.Invoke(this, new ClientConnectionEventArgs(client, ClientConnectionEventArgs.ConnectionStatus.Connected));
        }
    }
}
