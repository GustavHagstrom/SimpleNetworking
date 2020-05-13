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
            
            int port = 8888;
            int maxCnn = 10;
            //string host = "127.0.0.1";
            IPAddress hostAddress = IPAddress.Parse("127.0.0.1");
            byte[] expectedData = BitConverter.GetBytes(1000);
            Packet expectedPacket = new Packet
            {
                ClientId = 1,
                PacketTypeId = 1,
                Data = expectedData,
            };

            Server server = new Server();
            UserClient userClient = new UserClient();

            server.StartListening(maxCnn, port);
            userClient.Connect(hostAddress, port);
            Thread.Sleep(4 * 1000);

            userClient.Tcp.Send(expectedPacket);

            userClient.Udp.Send(expectedPacket);
            //userClient.Udp.Send(expectedPacket);
            //userClient.Udp.Send(expectedPacket);

            server.Clients.First().Value.Tcp.Send(expectedPacket);
            //server.Clients.First().Value.Tcp.Send(expectedPacket);
            //server.Clients.First().Value.Tcp.Send(expectedPacket);
            //server.Clients.First().Value.Tcp.Send(expectedPacket);

            //server.Clients.First().Value.Udp.Send(expectedPacket);
            //server.Clients.First().Value.Udp.Send(expectedPacket);

            Thread.Sleep(20);

            Assert.True(userClient.Tcp.IsConnected);
            Assert.True(server.Clients.First().Value.Tcp.IsConnected);

            Assert.Equal(expectedPacket.Data, server.ReceivedPackets.Peek().Data);
            //Assert.Equal(expectedPacket.Data, userClient.ReceivedPackets.Peek().Data);

            //Assert.Equal(expectedPacket.PacketTypeId, userClient.ReceivedPackets.Peek().PacketTypeId);
            Assert.Equal(expectedPacket.PacketTypeId, server.ReceivedPackets.Peek().PacketTypeId);


            Assert.Equal(1, server.ReceivedPackets.Count);
            Assert.Equal(1, userClient.ReceivedPackets.Count);
            //Assert.Equal(1, server.ReceivedPackets.Count);


        }
        [Fact]
        public void UdpTest()
        {
            //IPHostEntry entry = Dns.GetHostEntry("localhost");
            //IPAddress address = entry.AddressList.Last();
            string ip = "127.0.0.1";
            int serverPort = 8888;
            int clientPort = 8810;
            string expectedString = "somestring";
            byte[] stringBytes = Encoding.UTF8.GetBytes(expectedString);

            UdpListener listener = new UdpListener();
            UdpListener sender = new UdpListener();

            listener.Server(ip, serverPort);
            sender.Client(ip, clientPort);

            sender.Send(stringBytes);
            //listener.ServerSend(stringBytes, ip, port);
            Thread.Sleep(10);

            Assert.NotEmpty(listener.ReceivedData);
            //Assert.Equal(stringBytes, sender.ReceivedData.First().ToArray());
            //IPEndPoint EndPoint = new IPEndPoint(address, port);
            //UdpClient udpReceiver = new UdpClient();
            //UdpClient udpSender = new UdpClient();

            //udpReceiver.Connect(EndPoint);

            //string expectedString = "somestring";
            //byte[] stringBytes = Encoding.UTF8.GetBytes(expectedString);

            //byte[] receivedBytes = new byte[1];
            //Task.Run(() => receivedBytes = udpReceiver.Receive(ref EndPoint));
            //udpSender.Send(stringBytes, stringBytes.Length, EndPoint);
            //Thread.Sleep(100);

            //Assert.Equal(expectedString, Encoding.UTF8.GetString(receivedBytes));

        }
        
    }
}
