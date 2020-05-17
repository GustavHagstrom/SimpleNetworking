using SimpleNetworking.Common;
using SimpleNetworking.Packets;
using SimpleNetworking.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SimpleNetworking.User
{
    public class UserClient : IDisposable
    {
        private UserClientTcpHandler tcp = new UserClientTcpHandler();
        private UdpListener udpListener = new UdpListener();
        private UdpSender udpSender = new UdpSender();
        private PacketHandler reserverPacketsHandler;

        public UserClient()
        {
            udpListener.PacketReceived += OnPacketReceived;

            tcp.PacketReceived += OnPacketReceived;
            tcp.ConnectionSucceded += OnConnectionSucceded;
            tcp.Disconnected += OnDisconnected;
        }
        private void InitPacketHandler()
        {
            PacketHandlerBuilder builder = new PacketHandlerBuilder();
            builder.RegisterHandlerMethod((int)ReserverdPacketIds.ClientIdSetter, (packet) =>
            {
                ConnectionHandshakePacket handshakePacket = new ConnectionHandshakePacket { Bytes = packet.Bytes };
                this.Id = handshakePacket.ServerAssignedId;
                ConnectionSucceded?.Invoke(this, true);
            });
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
        public async Task ConnectAsync(IPAddress address, int port)
        {
            ConnectedIPAddress = address;
            RemotePort = port;
            await tcp.ConnectAsync(address, RemotePort);
        }
        public void Disconnect()
        {
            if(tcp != null)
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
            if(succeded == false)
            {
                ConnectionSucceded?.Invoke(this, succeded);
            }
            //Waiting for Handshake to get ClientId ConnectionSecceded handled in HandshakeReceived
        }
        private void OnPacketReceived(object sender, Packet packet)
        {
            IEnumerable<ReserverdPacketIds> reservedIds = Enum.GetValues(typeof(ReserverdPacketIds)).Cast<ReserverdPacketIds>();
            if (reservedIds.Contains((ReserverdPacketIds)packet.PacketTypeId)) // == (int)ReserverdPacketIds.ClientIdSetter)
            {
                reserverPacketsHandler.HandlePacket(packet);
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
