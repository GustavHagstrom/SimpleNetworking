using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace SimpleNetworking
{
    public class ServerClient : ClientBase
    {
        private ServerClientTcpHandler tcp;
        
        public ServerClientTcpHandler Tcp
        {
            get => tcp;
            private set
            {
                if (tcp != null)
                {
                    tcp.PacketReceived -= OnPacketReceived;
                    tcp.Connected -= OnTcpConnected;
                    tcp.Disconnected -= OnTcpDisconnected;
                }
                tcp = value;
                tcp.PacketReceived += OnPacketReceived;
                tcp.Connected += OnTcpConnected;
                tcp.Disconnected += OnTcpDisconnected;
            }
        }

        public override bool IsConnected => Tcp.IsConnected;

        internal event PacketReceivedEventHandler PacketReceived;
        internal event DisconnectedEventHandler Disconnected;
        internal event ConnectedEventHandler Connected;

        public ServerClient() : base()
        {
            Tcp = new ServerClientTcpHandler();
        }
        protected override void OnPacketReceived(IPacket packet, ProtocolType type)
        {
            packet.ClientId = Id;
            PacketReceived?.Invoke(packet, type);
        }
        private void OnTcpDisconnected(Exception e, ProtocolType type, int clientId)
        {
            Disconnected?.Invoke(e, type, this.Id);
        }
        protected override void OnTcpConnected(IPAddress remoteAddress, ProtocolType type, int clientId)
        {
            RemoteIP = ((IPEndPoint)Tcp.socket.Client.RemoteEndPoint).Address;
            //Udp.Connect(RemoteEndPoint.Address, RemoteEndPoint.Port);
            Debugger.Log(1, null, $"{nameof(ServerClient)}: Connection to server {remoteAddress.ToString()} established with ProtocolType {type.ToString()}\n");
            Udp.Connect(RemoteIP);
            Connected?.Invoke(remoteAddress, type, this.Id);
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
