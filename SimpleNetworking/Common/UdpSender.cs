using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SimpleNetworking.Common
{
    public class UdpSender
    {
        private UdpClient client = new UdpClient();

        public UdpSender()
        {

        }

        public void Send(Packet packet, IPAddress iPAddress, int port)
        {
            client.Send(packet.Bytes, packet.PacketLength, new IPEndPoint(iPAddress, port));
            Thread.Sleep(1);
        }
    }
}
