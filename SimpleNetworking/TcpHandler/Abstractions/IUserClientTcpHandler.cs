namespace SimpleNetworking
{
    public interface IUserClientTcpHandler : ITcpHandlerBase
    {
        void Connect(string host, int port);
    }
}