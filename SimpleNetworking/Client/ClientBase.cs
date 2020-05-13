using System;
using System.Net;
using System.Net.Sockets;

namespace SimpleNetworking
{
    public abstract class ClientBase : IDisposable
    {
        private ServerUdpHandler udp;

        public int Id { get; set; }
        public abstract bool IsConnected { get; }
        public IPAddress RemoteIP { get; set; }


        public ServerUdpHandler Udp
        {
            get => udp;
            set
            {
                if (udp != null)
                {
                    udp.PacketReceived -= OnPacketReceived;
                }
                udp = value;
                udp.PacketReceived += OnPacketReceived;
            }
        }
        public ClientBase()
        {
            Udp = new ServerUdpHandler();
        }
        protected abstract void OnTcpConnected(IPAddress address, ProtocolType type, int clientId);
        protected abstract void OnPacketReceived(IPacket packet, ProtocolType type);
        public void Disconnect()
        {
            Dispose();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected abstract void Dispose(bool disposing);
    }
}
