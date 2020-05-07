using System.Net.Sockets;

namespace SimpleNetworking
{

    public class ServerClientTcpHandler : TcpHandlerBase, IServerClientTcpHandler
    {
        public ServerClientTcpHandler()
        {

        }
        public void SetConnectedTcpClient(TcpClient client)
        {
            socket = client;
            SetupConnectedTcpClient(client);
        }
        private void SetupConnectedTcpClient(TcpClient client)
        {
            receiveBuffer = new byte[DataBufferSize];
            stream = socket.GetStream();
            this.StartReadingNetworkStream();
        }
        

    }
}
