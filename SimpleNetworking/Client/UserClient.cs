using System;
using System.Collections.Generic;
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

        public UserClient() : base()
        {
            Tcp = new UserClientTcpHandler();
        }
        
        private void OnPacketReceived(IPacket packet)
        {
            packet.ClientId = Id;
            ReceivedPackets.Enqueue(packet);
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
