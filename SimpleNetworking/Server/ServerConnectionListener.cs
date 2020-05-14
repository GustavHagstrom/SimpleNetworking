using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace SimpleNetworking.Server
{
    public class ServerConnectionListener
    {
        private TcpListener tcpListener;

        public event EventHandler<TcpClient> ClientConnecting;
        public ServerConnectionListener()
        {

        }
        public void Start(int port)
        {
            
            IPEndPoint listeningPoint = new IPEndPoint(IPAddress.Any, port);

            tcpListener = new TcpListener(listeningPoint);
            tcpListener.Start();

            Debugger.Log(1, null, $"{nameof(ServerConnectionListener)}: Listening for connections on {IPAddress.Parse(((IPEndPoint)tcpListener.LocalEndpoint).Address.ToString())} on port {((IPEndPoint)tcpListener.LocalEndpoint).Port}\n");

            tcpListener.BeginAcceptTcpClient(new AsyncCallback(ConnectionCallback), null);
        }
        public void Stop()
        {
            tcpListener.Stop();
        }
        private void ConnectionCallback(IAsyncResult result)
        {
            TcpClient tcpClient = tcpListener.EndAcceptTcpClient(result);
            tcpListener.BeginAcceptTcpClient(ConnectionCallback, null);

            Debugger.Log(1, null, $"{nameof(SimpleServer)}: Client from {((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString()} asks to connect to the server from port {((IPEndPoint)tcpClient.Client.RemoteEndPoint).Port}\n");
            ClientConnecting?.Invoke(this, tcpClient);
        }
    }
}
