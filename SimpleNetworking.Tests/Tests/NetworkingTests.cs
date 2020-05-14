using SimpleNetworking.Server;
using SimpleNetworking.Tests.Helpers;
using System;
using System.Collections.Generic;
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
            server.Clients[userClient.Id].Send(sendPacket, ProtocolType.Udp);


            Thread.Sleep(10);

            Assert.True(server.ReceivedPackets.Count == 4);
            Assert.True(userClient.ReceivedPackets.Count == 4);
            Assert.True(userClient.Id != 0);
        }
        
    }
}
