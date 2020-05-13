using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace SimpleNetworking
{
    

    public class Server 
    {
        private TcpListener tcpListener;
        //private UdpClient udpListener;

        public event DisconnectedEventHandler ClientDisconnected;
        public event ConnectedEventHandler ClientConnected;


        public int MaxConnections { get; private set; }
        public int Port { get; private set; }
        public Dictionary<int, ServerClient> Clients { get; private set; } = new Dictionary<int, ServerClient>();
        //public List<ServerClient> Clients { get; private set; } = new List<ServerClient>();
        public Queue<IPacket> ReceivedPackets { get; private set; } = new Queue<IPacket>();


        public Server()
        {

        }
       

        public void StartListening(int maxConnections, int port)
        {
            MaxConnections = maxConnections;
            Port = port;
            IPEndPoint listeningPoint = new IPEndPoint(IPAddress.Any, Port);

            tcpListener = new TcpListener(listeningPoint);
            tcpListener.Start();

            Debugger.Log(1, null, $"{nameof(Server)}: Listening for connections on {IPAddress.Parse(((IPEndPoint)tcpListener.LocalEndpoint).Address.ToString())} on port {((IPEndPoint)tcpListener.LocalEndpoint).Port}\n");

            tcpListener.BeginAcceptTcpClient(TcpConnectionCallback, null);


            //udpListener = new UdpClient(ListeningPoint);
        }
        public void StopListening()
        {
            tcpListener.Stop();
        }
        private void TcpConnectionCallback(IAsyncResult result)
        {
            TcpClient tcpClient = tcpListener.EndAcceptTcpClient(result);
            tcpListener.BeginAcceptTcpClient(TcpConnectionCallback, null);

            Debugger.Log(1, null, $"{nameof(Server)}: Client {((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString()} trying to connect to the server from port {((IPEndPoint)tcpClient.Client.RemoteEndPoint).Port}\n");

            if (Clients.Count >= MaxConnections)
            {
                return;
            }
            AddClient(tcpClient);
        }
        private void AddClient(TcpClient tcpClient)
        {
            
            ServerClient client;
            IPEndPoint endPoint = (IPEndPoint)tcpClient.Client.RemoteEndPoint;

            Debugger.Log(1, null, $"{nameof(Server)}: Client {((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString()} accepted from the server. Id will be {endPoint.GetHashCode()}\n");
            if (!Clients.ContainsKey(endPoint.GetHashCode()))
            {

                client = CreateInitializedClient(endPoint);
                Clients.Add(client.Id, client);
                client.Tcp.SetConnectedTcpClient(tcpClient);
            }
            else
            {
                client = Clients[endPoint.GetHashCode()];
                client.Tcp.SetConnectedTcpClient(tcpClient);
            }

            //client.Udp.BeginReceiving();
        }
        //private void AddClient(UdpClient udpClient)
        //{
        //    IPEndPoint endPoint = (IPEndPoint)udpClient.Client.RemoteEndPoint;

        //    //ServerClient client = Clients.Find(c => c.Id == endPoint.GetHashCode());
        //    ServerClient client = Clients[endPoint.GetHashCode()];
        //    if (client == null)
        //    {
        //        client = CreateInitializedClient(endPoint);
        //        Clients.Add(client.Id, client);
        //    }
        //}
        private ServerClient CreateInitializedClient(IPEndPoint endPoint)
        {
            ServerClient client = new ServerClient
            {
                Id = endPoint.GetHashCode(),
                RemoteIP = ((IPEndPoint)endPoint).Address,
            };
            client.PacketReceived += OnClientReceivedPacket;
            client.Connected += OnClientConnected;
            client.Disconnected += OnClientDisconnected;
            
            return client;
        }
        private void RemoveClient(int clientId)
        {
            //ServerClient client = Clients.Find(c => c.Id == clientId);
            ServerClient client = Clients[clientId];
            client.Dispose();
            Clients.Remove(clientId);
        }        
        private void OnClientReceivedPacket(IPacket packet, ProtocolType type)
        {
            Debugger.Log(1, null, $"{nameof(Server)}: Received packet from {packet.ClientId} with ProtocolType {type.ToString()}\n");
            ReceivedPackets.Enqueue(packet);
        }

        private void OnClientDisconnected(Exception e, ProtocolType type, int clientId)
        {
            Debugger.Log(1, null, $"{nameof(Server)}: Client with Id {clientId} has disconnected. ProtocolType {type.ToString()}. Exception: {e.Message}\n");
            ClientDisconnected?.Invoke(e, type, clientId);
        }
        private void OnClientConnected(IPAddress address, ProtocolType type, int clientId)
        {
            Debugger.Log(1, null, $"{nameof(Server)}: Client with Id {clientId} has connected with ProtocolType {type.ToString()}\n");
            ClientConnected?.Invoke(address, type, clientId);
        }

    }
}
