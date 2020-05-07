using System;
using System.Net.Sockets;

namespace SimpleNetworking
{

 
    public class TcpHandlerBase : ITcpHandlerBase
    {
        protected NetworkStream stream;
        protected byte[] receiveBuffer;
        protected TcpClient socket = new TcpClient();

        
        public int DataBufferSize { get; set; } = 4096; // Bad if user changes this mid connection
        public bool IsConnected => socket.Connected;

        internal event PacketReceivedEventHandler PacketReceived; //ok
        internal event DisconnectedEventHandler Disconnected; //ok
        internal event ConnectedEventHandler Connected; //ok

        public TcpHandlerBase()
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
        protected void ConnectCallback(IAsyncResult result)
        {
            socket.EndConnect(result);

            if (!socket.Connected)
            {
                return;
            }
            stream = socket.GetStream();
            Connected?.Invoke(ConnectionProtocolType.Tcp, 0);
            StartReadingNetworkStream();            
        }
        protected void ReceiveCallback(IAsyncResult result)
        {
            int byteLength = stream.EndRead(result);
            if (byteLength <= 0)
            {
                return;
            }
            byte[] data = new byte[byteLength];
            Array.Copy(receiveBuffer, data, byteLength);
            StartReadingNetworkStream();

            Packet packet = new Packet();
            packet.SetContentFromReceivedBytes(data);
            packet.Received = DateTime.Now;
            PacketReceived?.Invoke(packet);
        }
        protected void StartReadingNetworkStream()
        {
            try
            {
                stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
            }
            catch (Exception e)
            {
                Disconnected?.Invoke(e, ConnectionProtocolType.Tcp, 0);
            }
        }
    }
}
