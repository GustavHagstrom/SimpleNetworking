using SimpleNetworking.Common;
using SimpleNetworking.Packets;
using SimpleNetworking.Tools;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace SimpleNetworking.User
{
    public class UserClient : IDisposable
    {
        private UserClientTcpHandler tcp = new UserClientTcpHandler();
        private UdpListener udpListener = new UdpListener();
        private UdpSender udpSender = new UdpSender();

        public UserClient()
        {
            udpListener.PacketReceived += OnPacketReceived;

            tcp.PacketReceived += OnPacketReceived;
            tcp.ConnectionSucceded += OnConnectionSucceded;
            tcp.Disconnected += OnDisconnected;
        }
        public int Id { get; private set; } = 0;
        public bool IsConnected { get => tcp.client.Connected; }
        public int RemotePort { get; private set; }
        public int LocalPort { get => ((IPEndPoint)tcp.client.Client.LocalEndPoint).Port; }
        public IPAddress ConnectedIPAddress { get; private set; }
        public Queue<Packet> ReceivedPackets { get; private set; } = new Queue<Packet>();

        public event EventHandler<bool> ConnectionSucceded;
        public event EventHandler<DisconnectedEventArgs> Disconnected;

        public void Connect(IPAddress address, int port)
        {
            ConnectedIPAddress = address;
            RemotePort = port;
            tcp.Connect(address, RemotePort);
        }
        public void Disconnect()
        {
            tcp.Dispose();
        }
        public void Send(Packet packet, ProtocolType type)
        {
            packet.ClientId = this.Id;

            if ((int)ReserverdPacketIds.ClientIdSetter == packet.PacketTypeId)
            {
                throw new Exception($"{nameof(packet.PacketTypeId)} your are trying to use is a reserved id type.");
            }
            if(!IsConnected)
            {
                throw new Exception("You cannot send while not connected");
            }
            switch (type)
            {
                case ProtocolType.Tcp:
                    tcp.Send(packet);
                    break;
                case ProtocolType.Udp:
                    udpSender.Send(packet, ConnectedIPAddress, RemotePort);
                    break;
                default:
                    throw new Exception($"ProtocolType {type.ToString()} is not supported");
            }
        }
        private void OnConnectionSucceded(object sender, bool succeded)
        {
            ConnectionSucceded?.Invoke(this, succeded);
            if(succeded)
            {
                udpListener.Start(ConnectedIPAddress, LocalPort);
            }
        }
        private void OnPacketReceived(object sender, Packet packet)
        {
            if(packet.PacketTypeId == (int)ReserverdPacketIds.ClientIdSetter)
            {
                HandshakeReceived(new ConnectionHandshakePacket { Bytes = packet.Bytes });
            }
            else
            {
                ReceivedPackets.Enqueue(packet);
            }
        }
        private void OnDisconnected(object sender, DisconnectedEventArgs args)
        {
            Dispose();
            Disconnected?.Invoke(this, new DisconnectedEventArgs(args.Error, this.Id));
        }
        private void HandshakeReceived(ConnectionHandshakePacket packet)
        {
            this.Id = packet.ServerAssignedId;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                tcp.Dispose();
                udpListener.Stop();
            }

        }
    }
}
