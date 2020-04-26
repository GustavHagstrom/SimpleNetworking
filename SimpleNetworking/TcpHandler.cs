using System;
using System.Net.Sockets;

namespace SimpleNetworking
{
    public delegate void TcpConnectionEstablishedEventHandler(object source, EventArgs args);
    public delegate void TcpPacketReceivedEventHandler(object source, PacketReceivedEventArgs args);
    public class TcpHandler : ITcpHandler
    {
        private NetworkStream stream;
        private byte[] receiveBuffer;
        private TcpClient socket;

        public int DataBufferSize { get; set; } = 4096; // Verby bad if user changes this mid connection
        public event TcpPacketReceivedEventHandler TcpPacketReceived;
        public event TcpConnectionEstablishedEventHandler TcpConnectionEstablished;

        /// <summary>
        /// Used when connection is yet to be established.
        /// </summary>
        public TcpHandler()
        {

        }
        /// <summary>
        /// Used when connection is already established
        /// </summary>
        /// <param name="socket">Connected tcpClient</param>
        public TcpHandler(TcpClient socket)
        {
            this.socket = socket;
            receiveBuffer = new byte[DataBufferSize];

            TcpConnectionEstablished?.Invoke(this, EventArgs.Empty);
            stream = socket.GetStream();

            stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
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
        public void Send(Packet packet)
        {
            stream.Write(packet.Binarys, 0, packet.Binarys.Length);
        }
        private void ConnectCallback(IAsyncResult result)
        {
            socket.EndConnect(result);

            if (!socket.Connected)
            {
                return;
            }

            TcpConnectionEstablished?.Invoke(this, EventArgs.Empty);
            stream = socket.GetStream();

            stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
        }
        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int byteLength = stream.EndRead(result);
                if (byteLength <= 0)
                {
                    return;
                }
                byte[] data = new byte[byteLength];
                Array.Copy(receiveBuffer, data, byteLength);

                TcpPacketReceived?.Invoke(this, new PacketReceivedEventArgs(Packet.FromReceivedBytes(data)));
                stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
            }
            catch (Exception e)
            {
                Disconnect();
                throw e;
            }
        }
        
    }
}
