using SimpleNetworking.Server;
using SimpleNetworking.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SimpleNetworking.Tests.Tests
{
    public class NetworkingTests
    {
        [Fact]
        public void Tcp_Sending_Receiving_Packets()
        {
            SimpleServer server = new SimpleServer();
            server.Start(20, 8888);

            
            Thread.Sleep(10);

            SimpleNetworking.User.UserClient userClient = new User.UserClient();
            userClient.Connect(IPAddress.Parse("127.0.0.1"), 8888);

            Thread.Sleep(4 * 1000);

            Packet sendPacket = new ExamplePacket { DataString = "Hello?" };
            userClient.Send(sendPacket, ProtocolType.Tcp);
            userClient.Send(sendPacket, ProtocolType.Udp);
            userClient.Send(sendPacket, ProtocolType.Udp);
            userClient.Send(sendPacket, ProtocolType.Udp);

            server.Clients[userClient.Id].Send(sendPacket, ProtocolType.Tcp);
            server.Clients[userClient.Id].Send(sendPacket, ProtocolType.Udp);
            server.Clients[userClient.Id].Send(sendPacket, ProtocolType.Udp);

            Stopwatch timer = new Stopwatch();
            timer.Start();
            for (int i = 0; i < 2000; i++)
            {
                server.Clients[userClient.Id].Send(sendPacket, ProtocolType.Udp);
            }
            timer.Stop();
            timer.Reset();
            timer.Start();
            for(int i = 0; i < 2000; i++)
            {
                userClient.Send(sendPacket, ProtocolType.Tcp);
            }
            timer.Stop();
            server.Clients[userClient.Id].Disconnect();

            Thread.Sleep(10);

            Assert.Equal(2000 + 4, server.ReceivedPackets.Count);
            Assert.Equal(2000 + 3, userClient.ReceivedPackets.Count);
            Assert.True(userClient.Id != 0);
        }
        
    }
}
