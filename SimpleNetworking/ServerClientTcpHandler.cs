using Serilog;
using SimpleNetworking.DI;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace SimpleNetworking
{

    public class ServerClientTcpHandler : IServerClientTcpHandler
    {
        private NetworkStream stream;
        private byte[] receiveBuffer;
        private TcpClient socket = new TcpClient();

        public int DataBufferSize { get; set; } = 4096; // Bad if user changes this mid connection
        public bool Connected => socket.Connected;
        public ReceivedPacketHandler HandleReceivedPacket { get; set; }

        public ServerClientTcpHandler()
        {

        }
        public void Disconnect()
        {
            socket.Close();
        }
        public void Send(IPacket packet)
        {
            packet.Sent = DateTime.Now;
            stream.Write(packet.Bytes, 0, packet.Bytes.Length);
        }
        public void SetConnectedTcpClient(TcpClient client)
        {
            socket = client;
            SetupConnectedTcpClient(client);
        }
        private void ConnectCallback(IAsyncResult result)
        {
            socket.EndConnect(result);

            if (!socket.Connected)
            {
                return;
            }
            SetupConnectedTcpClient(socket);
        }
        private void ReceiveCallback(IAsyncResult result)
        {
            int byteLength = stream.EndRead(result);
            if (byteLength <= 0)
            {
                return;
            }
            byte[] data = new byte[byteLength];
            Array.Copy(receiveBuffer, data, byteLength);
            stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);

            IPacket packet = ServiceLocator.Instance.Get<IPacket>();
            packet.SetContentFromReceivedBytes(data);
            packet.Received = DateTime.Now;
            HandleReceivedPacket?.Invoke(packet);
        }
        private void SetupConnectedTcpClient(TcpClient client)
        {
            receiveBuffer = new byte[DataBufferSize];
            stream = socket.GetStream();
            stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
        }
        

    }
}
