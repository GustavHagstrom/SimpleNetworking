namespace SimpleNetworking
{
    public interface IPacket
    {
        byte[] Data { get; }
        int PacketTypeId { get; }
    }
}