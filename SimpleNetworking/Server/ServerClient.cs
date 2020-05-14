using SimpleNetworking.Tools;
using SimpleNetworking.Common;
using System;
using System.Net;
using System.Net.Sockets;

namespace SimpleNetworking.Server
{
    
    public class ServerClient : IDisposable
    {
        private ServerClientTcpHandler tcp = new ServerClientTcpHandler();
        private UdpSender udpSender = new UdpSender();

        public ServerClient()
        {
            tcp.Disconnected += OnDisconnected;
            tcp.PacketReceived += OnPacketReceived;
        }

        public int Id { get; set; } = 0;
        //public int Port { get; set; }
        public bool IsConnected
        {
            get
            {
                if(tcp == null)
                {
                    return false;
                }
                return tcp.client.Connected;
            }
        }
        public IPEndPoint ConnectedRemoteEndPoint { get => ((IPEndPoint)tcp.client.Client.RemoteEndPoint); }
        

        internal event EventHandler<DisconnectedEventArgs> Disconnected;
        internal event EventHandler<Packet> PacketReceived; 


        public void SetConnection(TcpClient tcpClient)
        {
            tcp.SetConnection(tcpClient);
        }
        public void Disconnect()
        {
            Dispose();
        }
        public void Send(Packet packet, ProtocolType type)
        {
            packet.ClientId = this.Id;
            if(!IsConnected)
            {
                throw new Exception("Cannot send while not connected.");
            }
            switch (type)
            {
                case ProtocolType.Tcp:
                    tcp.Send(packet);
                    break;
                case ProtocolType.Udp:
                    udpSender.Send(packet, ConnectedRemoteEndPoint.Address, ConnectedRemoteEndPoint.Port);
                    break;
                default:
                    throw new Exception($"ProtocolType {type.ToString()} is not supported");
            }
        }
        protected virtual void OnDisconnected(object sender, DisconnectedEventArgs args)
        {
            Dispose();
            Disconnected?.Invoke(this, new DisconnectedEventArgs(args.Error, this.Id));
        }
        protected virtual void OnPacketReceived(object sender, Packet packet)
        {
            PacketReceived?.Invoke(this, packet);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                tcp.Dispose();
            }
                
        }
    }
}
