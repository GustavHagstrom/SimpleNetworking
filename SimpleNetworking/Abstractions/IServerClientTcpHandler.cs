using System.Net.Sockets;

namespace SimpleNetworking
{
    public interface IServerClientTcpHandler 
    {
        int DataBufferSize { get; set; }
        bool Connected { get; }
        ReceivedPacketHandler HandleReceivedPacket { get; set; }
        void Disconnect();
        void Send(IPacket packet);
        void SetConnectedTcpClient(TcpClient client);
    }
}