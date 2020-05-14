using SimpleNetworking.Packets;
using SimpleNetworking.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleNetworking.User
{
    public class UserClient
    {
        private UserClientTcpHandler tcp = new UserClientTcpHandler();
        private UserClientUdpListener udpListener = new UserClientUdpListener();
        private UserClientUdpSender udpSender = new UserClientUdpSender();

        public UserClient()
        {
            udpListener.PacketReceived += OnPacketReceived;

            tcp.PacketReceived += OnPacketReceived;
            tcp.ConnectionSucceded += OnConnectionSucceded;
        }
        public int Id { get; private set; } = 0;
        public bool IsConnected { get => tcp.client.Connected; }
        public int RemotePort { get; set; }
        public int LocalPort { get => ((IPEndPoint)tcp.client.Client.LocalEndPoint).Port; }
        public IPAddress ConnectedIPAddress { get; private set; }
        public Queue<Packet> ReceivedPackets { get; private set; } = new Queue<Packet>();
        public event EventHandler<bool> ConnectionSucceded;
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
                HandshakeReceived(new ConnectionHandshakePacket { AllBytes = packet.AllBytes });
            }
            else
            {
                ReceivedPackets.Enqueue(packet);
            }
        }
        private void HandshakeReceived(ConnectionHandshakePacket packet)
        {
            this.Id = packet.ServerAssignedId;
        }

    }
}
