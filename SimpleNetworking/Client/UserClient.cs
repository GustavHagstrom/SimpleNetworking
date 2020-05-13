using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace SimpleNetworking
{
    public class UserClient : ClientBase
    {
        private UserClientTcpHandler tcp;

        public UserClientTcpHandler Tcp
        {
            get => tcp;
            private set
            {
                if (tcp != null)
                {
                    tcp.PacketReceived -= OnPacketReceived;
                    tcp.Connected -= OnTcpConnected;
                    tcp.Disconnected -= OnTcpDisconnected;
                    tcp.ConnectionFailed -= OnTcpConnectionFailed;
                }
                tcp = value;
                tcp.PacketReceived += OnPacketReceived;
                tcp.Connected += OnTcpConnected;
                tcp.Disconnected += OnTcpDisconnected;
                tcp.ConnectionFailed += OnTcpConnectionFailed;
            }
        }
        public Queue<IPacket> ReceivedPackets { get; private set; } = new Queue<IPacket>();

        public override bool IsConnected => Tcp.IsConnected;

        public event DisconnectedEventHandler Disconnected;
        public event ConnectedEventHandler Connected;
        public event ConnectionFailedEventHandler ConnectionFailed;

        public UserClient() : base()
        {
            Tcp = new UserClientTcpHandler();
        }
        //public UserClient(string host, int port) : this()
        //{
        //    IPHostEntry entry = Dns.GetHostEntry(host);
        //    IPAddress address = entry.AddressList.Last();
        //    this.RemoteEndPoint = new IPEndPoint(address, port);
        //}
        public void Connect(IPAddress address, int port)
        {
            RemoteIP = address;
            Tcp.Connect(address, port);
        }
        protected override void OnPacketReceived(IPacket packet, ProtocolType type)
        {
            packet.ClientId = Id;
            ReceivedPackets.Enqueue(packet);
        }
        private void OnTcpDisconnected(Exception e, ProtocolType type, int clientId)
        {
            Disconnected?.Invoke(e, type, this.Id);
        }
        protected override void OnTcpConnected(IPAddress remoteAddress, ProtocolType type, int clientId)
        {

            //RemoteIP = ((IPEndPoint)Tcp.socket.Client.RemoteEndPoint).Address;

            Debugger.Log(1, null, $"{nameof(UserClient)}: Connection to server {RemoteIP.ToString()} established with ProtocolType {type.ToString()}\n");
            //Udp.Connect(RemoteEndPoint.Address, RemoteEndPoint.Port);
            //string test = RemoteEndPoint.ToString();
            //Udp.Connect(RemoteEndPoint);
            Udp.Connect(RemoteIP, ((IPEndPoint)Tcp.socket.Client.RemoteEndPoint).Port);
            //Udp.BeginReceiving();
            Connected?.Invoke(remoteAddress, type, this.Id);
        }
        private void OnTcpConnectionFailed(Exception e, ProtocolType type)
        {
            ConnectionFailed?.Invoke(e, type);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Tcp.Dispose();
                //TODO dispose Udp
            }
        }

    }
}
