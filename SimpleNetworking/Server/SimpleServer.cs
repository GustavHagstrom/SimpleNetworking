﻿using SimpleNetworking.Common;
using SimpleNetworking.Packets;
using SimpleNetworking.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace SimpleNetworking.Server
{


    public class SimpleServer
    {
        private ServerConnectionListener connectionListener;
        private UdpListener udpListener;

        public event EventHandler<DisconnectedEventArgs> ClientDisconnected;
        public event EventHandler<ServerClient> ClientAccepted;
        public event EventHandler ServerCapacityReached;

        public int MaxConnections { get; private set; }
        public int Port { get; private set; }
        public Dictionary<int, ServerClient> Clients { get; private set; } = new Dictionary<int, ServerClient>();
        public Queue<Packet> ReceivedPackets { get; private set; } = new Queue<Packet>();


        public SimpleServer()
        {

        }
       

        public void Start(int maxConnections, int port)
        {
            MaxConnections = maxConnections;
            Port = port;

            udpListener = new UdpListener();
            connectionListener = new ServerConnectionListener();

            udpListener.PacketReceived += OnPacketReceived;
            connectionListener.ClientConnecting += OnClientConnecting;

            connectionListener.Start(Port);
            udpListener.Start(IPAddress.Any, Port);
        }
        public void Stop()
        {
            connectionListener.Stop();
            udpListener.Stop();
        }
        private void AddClient(TcpClient tcpClient)
        {
            int calculatedId = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).GetHashCode();

            Debugger.Log(1, null, $"{nameof(SimpleServer)}: Client {((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString()} accepted from the server. Id will be {calculatedId}\n");

            if (!Clients.ContainsKey(calculatedId))
            {
                Clients.Add(calculatedId, CreateInitializedClient(calculatedId));
            }
            Clients[calculatedId].SetConnection(tcpClient);
            SendServerAssignedId(calculatedId);
            ClientAccepted?.Invoke(this, Clients[calculatedId]);
        }
        private void SendServerAssignedId(int clientId)
        {
            Clients[clientId].Send(new ConnectionHandshakePacket
            {
                ClientId = clientId,
                PacketTypeId = (int)ReserverdPacketIds.ClientIdSetter,
                ServerAssignedId = clientId,
            }, ProtocolType.Tcp);
        }
        private ServerClient CreateInitializedClient(int id)
        {
            ServerClient client = new ServerClient
            {
                Id = id,
                //Port = this.Port,
            };
            client.PacketReceived += OnPacketReceived;
            client.Disconnected += OnClientDisconnected;
            
            return client;
        }
        public void RemoveClient(int clientId)
        {
            ServerClient client = Clients[clientId];
            client.Dispose();
            Clients.Remove(clientId);
        }
        private void OnClientConnecting(object sender, TcpClient tcpClient)
        {
            if (Clients.Count >= MaxConnections)
            {
                ServerCapacityReached?.Invoke(this, EventArgs.Empty);
                return;
            }
            AddClient(tcpClient);
        }
        private void OnPacketReceived(object sender, Packet packet)
        {
            if (Clients.ContainsKey(packet.ClientId))
            {
                ReceivedPackets.Enqueue(packet);
            }
        }
        private void OnClientDisconnected(object sender, DisconnectedEventArgs args)
        {
            Debugger.Log(1, null, $"{nameof(SimpleServer)}: Client with Id {args.ClientId} has disconnected. Exception: {args.Error.Message}\n");
            ClientDisconnected?.Invoke(this, args);
        }      
    }
}
