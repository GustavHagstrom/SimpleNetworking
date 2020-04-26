namespace SimpleNetworking
{
    public interface ITcpHandler
    {
        int DataBufferSize { get; set; }

        event TcpConnectionEstablishedEventHandler TcpConnectionEstablished;
        event TcpPacketReceivedEventHandler TcpPacketReceived;

        void Connect(string host, int port);
        void Disconnect();
        void Send(Packet packet);
    }
}