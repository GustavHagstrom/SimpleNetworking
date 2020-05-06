﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace SimpleNetworking.Tests.Tests
{
    public class NetworkingTests
    {
        [Fact]
        public void Sending_Receiving_Packets()
        {
            Server server = new Server();
            UserClient userClient = new UserClient();
            int port = 8888;
            int maxCnn = 10;
            string host = "localhost";
            byte[] expectedData = BitConverter.GetBytes(1000);
            Packet expectedPacket = new Packet
            {
                ClientId = 1,
                PacketTypeId = 1,
                Data = expectedData,
            };


            server.Start(maxCnn, port);
            userClient.Tcp.Connect(host, port);
            Thread.Sleep(5 * 1000);
            userClient.Tcp.Send(expectedPacket);
            server.Clients.First().Tcp.Send(expectedPacket);
            Thread.Sleep(5 * 1000);

            Assert.True(userClient.Tcp.Connected);
            Assert.True(server.Clients.First().Tcp.Connected);
            Assert.Equal(server.ReceivedPackets.Peek().Data, userClient.ReceivedPackets.Peek().Data);
            Assert.Equal(server.ReceivedPackets.Peek().PacketTypeId, userClient.ReceivedPackets.Peek().PacketTypeId);

        }
    }
}
