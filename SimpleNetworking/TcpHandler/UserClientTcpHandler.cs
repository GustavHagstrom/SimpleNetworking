using System;
using System.Net;
using System.Net.Sockets;

namespace SimpleNetworking
{

    public class UserClientTcpHandler : TcpHandlerBase
    {
        //private UserClient client;

        internal event ConnectionFailedEventHandler ConnectionFailed;

        public UserClientTcpHandler() : base()
        {
            //this.client = client;
        }
        internal void Connect(IPAddress address, int port)
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = DataBufferSize,
                SendBufferSize = DataBufferSize
            };

            try
            {
                socket.BeginConnect(address, port, ConnectCallback, null);
            }
            catch (Exception e)
            {
                ConnectionFailed?.Invoke(e, ProtocolType.Tcp);
            }


            
            
        }
        
    }
}
