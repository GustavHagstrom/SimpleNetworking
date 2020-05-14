using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SimpleNetworking.User
{
    public class UserClientUdpHandler
    {
        private UdpClient client = new UdpClient();
        private IPEndPoint endPoint;

        public event EventHandler<Packet> PacketReceived;
        public void Connect(IPAddress address, int port = 0)
        {
            endPoint = new IPEndPoint(address, port);
            client.Connect(endPoint);
        }
        public void Send(Packet packet)
        {
            client.Send(packet.AllBytes, packet.PacketLength);
            Thread.Sleep(1);
        }
        private void Receive()
        {
            client.BeginReceive((ar) =>
            {
                byte[] data = client.EndReceive(ar, ref endPoint);
                Receive();

                PacketReceived?.Invoke(this, new Packet { AllBytes = data });
            }, null);
        }
        
    }
    
}
