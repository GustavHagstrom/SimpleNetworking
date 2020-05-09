using System;
using System.Net.Sockets;

namespace SimpleNetworking
{

    public class UserClientTcpHandler : TcpHandlerBase
    {
        public UserClientTcpHandler() : base()
        {
            
        }
        public void Connect(string host, int port)
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = DataBufferSize,
                SendBufferSize = DataBufferSize
            };

            socket.BeginConnect(host, port, ConnectCallback, null);
        }
        
    }
}
