namespace SimpleNetworking
{
    public interface IClient
    {
        int Id { get; set; }

        event TcpPacketReceivedEventHandler TcpPacketReceived;
        event TcpConnectionEstablishedEventHandler TcpConnectionEstablished;

        void TcpConnect(string host, int port);
        void TcpSend(Packet packet);
    }
}