using System.Collections.Generic;

namespace SimpleNetworking
{
    public interface IPacketHandlerBuilder
    {
        void AddHandlerMap(Dictionary<int, PacketHandlerMethod> handlerMap);
        PacketHandler Build();
        void RegisterHandlerMethod(int packetId, PacketHandlerMethod handlerMethod);
        void ResetBuilder();
    }
}