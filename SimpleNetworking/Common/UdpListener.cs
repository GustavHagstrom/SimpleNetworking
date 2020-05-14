using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace SimpleNetworking.Common
{
    public class UdpListener
    {
        private UdpClient udpListener;
        private IPEndPoint endPoint;


        public event EventHandler<Packet> PacketReceived;
        public UdpListener()
        {

        }
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
            Debugger.Log(1, null, $"{nameof(UdpListener)}: Receiving udp packets from any IP address on port: {endPoint.Port}.\n");
            udpListener.BeginReceive((ar) =>
            {
                byte[] data = udpListener.EndReceive(ar, ref endPoint);

                Receive();

                PacketReceived(this, new Packet { Bytes = data });
            }, null);
        }
    }
}
