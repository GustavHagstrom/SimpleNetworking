namespace SimpleNetworking
{
    public interface IUserClientTcpHandler
    {
        int DataBufferSize { get; set; }
        bool Connected { get; }
        ReceivedPacketHandler HandleReceivedPacket { get; set; }
        void Disconnect();
        void Send(IPacket packet);
        void Connect(string host, int port);
    }
}