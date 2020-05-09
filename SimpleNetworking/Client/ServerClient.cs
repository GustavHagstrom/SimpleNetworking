using System;
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
        public string Ip { get; set; } = string.Empty;

        internal Server server;
        internal event PacketReceivedEventHandler PacketReceived;
        internal event DisconnectedEventHandler Disconnected;
        internal event ConnectedEventHandler Connected;

        public ServerClient(Server server) : base()
        {
            Tcp = new ServerClientTcpHandler();
            this.server = server;
        }
        private void OnPacketReceived(IPacket packet)
        {
            packet.ClientId = Id;
            PacketReceived?.Invoke(packet);
        }
        private void OnTcpDisconnected(Exception e, ProtocolType type, int clientId)
        {
            Disconnected?.Invoke(e, type, this.Id);
        }
        private void OnTcpConnected(ProtocolType type, int clientId)
        {
            Connected?.Invoke(type, this.Id);
        }

    }
}
