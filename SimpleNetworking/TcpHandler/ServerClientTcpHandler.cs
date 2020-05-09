using System;
using System.Linq;
using System.Net.Sockets;

namespace SimpleNetworking
{

    public sealed class ServerClientTcpHandler : TcpHandlerBase
    {
        public ServerClientTcpHandler() : base()
        {

        }

        public void SetConnectedTcpClient(TcpClient client)
        {
            socket = client;

            this.BeginReadingNetworkStream(new StateObject(DataBufferSize));
        }
    }
}
