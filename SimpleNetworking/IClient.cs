namespace SimpleNetworking
{
    public interface IClient
    {
        int Id { get; set; }

        event TcpConnectionEstablishedEventHandler TcpConnectionEstablished;

        void TcpConnect(string host, int port);
        void TcpSend(Packet packet);
    }
}