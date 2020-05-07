using System.Net.Sockets;

namespace SimpleNetworking
{
    public interface IServerClientTcpHandler : ITcpHandlerBase
    {
        void SetConnectedTcpClient(TcpClient client);
    }
}