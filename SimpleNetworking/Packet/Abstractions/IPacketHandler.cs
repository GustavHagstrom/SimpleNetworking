namespace SimpleNetworking
{
    public interface IPacketHandler
    {
        void HandlePacket(IPacket packet, int clientId);
    }
}