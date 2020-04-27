using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleNetworking.Tests.Helpers
{
    public class ClientSample
    {

        private IClient client;
        private IPacketHandler packetHandler;
        public string WelcomeString { get; private set; } = string.Empty;

        public ClientSample()
        {
            packetHandler = CreatePacketHandler();
        }
        public void Connect(string host, int port)
        {
            client = new Client(new TcpHandler());
            client.TcpPacketReceived += OnTcpPacketReceived;
            client.TcpConnect(host, port);
        }
        private IPacketHandler CreatePacketHandler()
        {
            IPacketHandlerBuilder builder = new PacketHandlerBuilder();
            builder.RegisterHandlerMethod((int)PacketIdTypes.Welcome, Welcome);
            return builder.Build();
        }
        private void OnTcpPacketReceived(object source, PacketReceivedEventArgs args)
        {

        }
        private void Welcome(Packet packet)
        {
            WelcomeString = Encoding.ASCII.GetString(packet.Data);
            Packet welcomeReceivedpacket = Packet.NewPacket((int)PacketIdTypes.WelcomeReceived, Encoding.ASCII.GetBytes("Client has recieved a WelcomePacket"));
            client.TcpSend(welcomeReceivedpacket);
        }
    }
}
