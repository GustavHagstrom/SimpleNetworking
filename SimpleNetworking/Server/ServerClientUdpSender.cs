using System.Net;
using System.Net.Sockets;

namespace SimpleNetworking.Server
{
    public class ServerClientUdpSender
    {
        private UdpClient client = new UdpClient();

        public ServerClientUdpSender()
        {
        }

        public void Send(Packet packet, IPAddress iPAddress, int port)
        {
            client.Send(packet.AllBytes, packet.PacketLength, new IPEndPoint(iPAddress, port));
        }
    }
}
