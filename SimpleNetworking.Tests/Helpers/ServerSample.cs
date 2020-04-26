using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SimpleNetworking.Tests.Helpers
{
    enum PacketIdTypes
    {
        Welcome = 1,
        WelcomeReceived = 2,
    }
    
    public class ServerSample : IEnumerable<object[]>
    {
        

        private IServer server;

        //private int port = 8888;
        public int MaxConnections { get; set; } = 10;
        public bool IsWelcomeReceived { get; private set; } = false;
        public string WelcomeReceivedMessage { get; private set; } = string.Empty;
        public ServerSample()
        {
            
            
        }
        public void Start(int port)
        {
            server = new Server(CreatePacketHandler(), MaxConnections, port);
            server.ClientConnectedToServer += OnClientConnectedToServer;
            server.Start();
        }

        private IPacketHandler CreatePacketHandler()
        {
            IPacketHandlerBuilder builder = new PacketHandlerBuilder();
            builder.RegisterHandlerMethod((int)PacketIdTypes.WelcomeReceived, WelcomeReceived);
            return builder.Build();
        }
        private void WelcomeReceived(Packet packet)
        {
            IsWelcomeReceived = true;
            WelcomeReceivedMessage = Encoding.ASCII.GetString(packet.Data);
        }
        private void OnClientConnectedToServer(object source, ClientConnectedToServerEventArgs args)
        {
            args.Client.TcpSend(Packet.NewPacket((int)PacketIdTypes.Welcome, Encoding.ASCII.GetBytes("Welcome to the server")));
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new ServerSample(),
                new ClientSample(),
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
