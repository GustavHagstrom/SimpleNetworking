using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleNetworking.User
{
    public class UserClientUdpSender
    {
        private UdpClient client = new UdpClient();

        public UserClientUdpSender()
        {

        }

        public void Send(Packet packet, IPAddress iPAddress, int port)
        {
            client.Send(packet.AllBytes, packet.PacketLength, new IPEndPoint(iPAddress, port));
        }
    }
}
