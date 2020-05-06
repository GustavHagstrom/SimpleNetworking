using SimpleNetworking.DI;
using System;
using System.Net.Sockets;

namespace SimpleNetworking
{

    public class UserClientTcpHandler : IUserClientTcpHandler
    {
        private NetworkStream stream;
        private byte[] receiveBuffer;
        private TcpClient socket = new TcpClient();

        public int DataBufferSize { get; set; } = 4096; // Bad if user changes this mid connection
        public bool Connected => socket.Connected;
        public ReceivedPacketHandler HandleReceivedPacket { get; set; }

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
        public void Disconnect()
        {
            socket.Close();
        }
        public void Send(IPacket packet)
        {
            packet.Sent = DateTime.Now;
            stream.Write(packet.Bytes, 0, packet.Bytes.Length);
        }
        private void ConnectCallback(IAsyncResult result)
        {
            socket.EndConnect(result);

            if (!socket.Connected)
            {
                return;
            }
            stream = socket.GetStream();

            stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
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
    }
}
