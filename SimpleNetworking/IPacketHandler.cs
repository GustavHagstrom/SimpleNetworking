namespace SimpleNetworking
{
    public interface IPacketHandler
    {
        void HandlePacket(Packet packet);
    }
}