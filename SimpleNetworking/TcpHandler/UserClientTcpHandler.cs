using System.Net.Sockets;

namespace SimpleNetworking
{

    public class UserClientTcpHandler : TcpHandlerBase, IUserClientTcpHandler
    {
        public UserClientTcpHandler()
        {
            
        }
        public void Connect(string host, int port)
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = DataBufferSize,
                SendBufferSize = DataBufferSize
            };
            receiveBuffer = new byte[DataBufferSize];

            socket.BeginConnect(host, port, ConnectCallback, null);
        }
    }
}
