using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace SimpleNetworking.Server
{
    public class ServerUdpListener
    {
        private UdpClient udpListener;
        private IPEndPoint endPoint;


        public event EventHandler<Packet> PacketReceived;
        public ServerUdpListener()
        {

        }
        public void Start(int port)
        {
            endPoint = new IPEndPoint(IPAddress.Any, port);
            udpListener = new UdpClient(endPoint);
            Receive();
        }
        public void Stop()
        {
            udpListener.Dispose();
        }
        private void Receive()
        {
            Debugger.Log(1, null, $"{nameof(ServerUdpListener)}: Receiving udp packets from any IP address on port: {endPoint.Port}.\n");
            udpListener.BeginReceive((ar) =>
            {
                byte[] data = udpListener.EndReceive(ar, ref endPoint);

                Receive();

                PacketReceived(this, new Packet { AllBytes = data });
            }, null);
        }
    }
}
