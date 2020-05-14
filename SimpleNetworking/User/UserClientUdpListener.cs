using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleNetworking.User
{
    public class UserClientUdpListener
    {
        private UdpClient udpListener;
        private IPEndPoint endPoint;

        public event EventHandler<Packet> PacketReceived;

        public void Start(IPAddress address, int port)
        {
            endPoint = new IPEndPoint(address, port);
            udpListener = new UdpClient(endPoint);
            Receive();
        }
        public void Stop()
        {
            udpListener.Dispose();
        }
        private void Receive()
        {
            Debugger.Log(1, null, $"{nameof(UserClientUdpListener)}: Receiving udp packets from IP address: {endPoint.Address}, on port: {endPoint.Port}.\n");
            udpListener.BeginReceive((ar) =>
            {
                byte[] data = udpListener.EndReceive(ar, ref endPoint);

                Receive();

                PacketReceived(this, new Packet { AllBytes = data });
            }, null);
        }

    }
}
