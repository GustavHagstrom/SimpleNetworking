using System;

namespace SimpleNetworking
{

    public class Client : IClient
    {
        private ITcpHandler tcpHandler;
        //private IPacketHandler packetHandler;

        public int Id { get; set; } = 0;
        public event TcpPacketReceivedEventHandler TcpPacketReceived;
        public event TcpConnectionEstablishedEventHandler TcpConnectionEstablished;

        public Client(ITcpHandler tcpHandler)//, IPacketHandler packetHandler)
        {
            this.tcpHandler = tcpHandler;
            //this.packetHandler = packetHandler;
            tcpHandler.TcpConnectionEstablished += OnTcpConnectionEstablished;
            tcpHandler.TcpPacketReceived += OnTcpPacketReceived;
        }

        public void TcpConnect(string host, int port)
        {
            tcpHandler.Connect(host, port);
        }
        public void TcpSend(Packet packet)
        {
            tcpHandler.Send(packet);
        }
        private void OnTcpConnectionEstablished(object source, EventArgs args)
        {
            TcpConnectionEstablished?.Invoke(source, args);
        }
        private void OnTcpPacketReceived(object source, PacketReceivedEventArgs args)
        {
            //packetHandler.HandlePacket(args.Packet);
            TcpPacketReceived?.Invoke(this, args);
        }

    }
}
