
using System.Net;

namespace SimpleNetworking
{
    public interface IServerClient
    {
        int Id { get; set; }
        IServerClientTcpHandler Tcp { get; }
        string Ip { get; set; }
        //ReceivedPacketHandler HandleReceivedPacket { get; set; }

    }
}