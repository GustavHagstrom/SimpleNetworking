using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleNetworking
{
    public class UdpState
    {
        public UdpState(IPEndPoint endPoint, UdpClient client)
        {
            EndPoint = endPoint;
            Client = client;
        }

        public IPEndPoint EndPoint { get; private set; }
        public UdpClient Client { get; private set; }
    }
}
