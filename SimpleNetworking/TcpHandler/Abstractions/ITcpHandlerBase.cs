namespace SimpleNetworking
{
    public interface ITcpHandlerBase
    {
        bool IsConnected { get; }
        int DataBufferSize { get; set; }
        //event PacketReceivedEventHandler PacketReceived;
        //event DisconnectedEventHandler Disconnected;
        //event ConnectedEventHandler Connected;

        void Disconnect();
        void Send(IPacket packet);
    }
}