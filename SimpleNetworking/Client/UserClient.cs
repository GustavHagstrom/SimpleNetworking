using System;
using System.Collections.Generic;

namespace SimpleNetworking
{
    public class ClientDisconnectedEventArgs : EventArgs
    {
        public Exception Error { get; private set; }
        public int ClientId { get; private set; }
        public ClientDisconnectedEventArgs(Exception error, int clientId)
        {
            Error = error;
            ClientId = clientId;
        }
    }

    public class UserClient : IUserClient
    {
        private UserClientTcpHandler tcp;

        public int Id { get; set; } = 0;
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
                }
                tcp = value;
                tcp.PacketReceived += OnPacketReceived;
                tcp.Connected += OnTcpConnected;
                tcp.Disconnected += OnTcpDisconnected;
            }
        }
        public Queue<IPacket> ReceivedPackets { get; private set; } = new Queue<IPacket>();

        public event DisconnectedEventHandler Disconnected;
        public event ConnectedEventHandler Connected;

        public UserClient()
        {
            Tcp = new UserClientTcpHandler();
        }
        
        private void OnPacketReceived(IPacket packet)
        {
            packet.ClientId = Id;
            ReceivedPackets.Enqueue(packet);
        }
        private void OnTcpDisconnected(Exception e, ConnectionProtocolType type, int clientId)
        {
            Disconnected?.Invoke(e, type, this.Id);
        }
        private void OnTcpConnected(ConnectionProtocolType type, int clientId)
        {
            Connected?.Invoke(type, this.Id);
        }

    }
}
