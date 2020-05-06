using System.Collections.Generic;

namespace SimpleNetworking
{
    public delegate void PacketHandlerMethod(IPacket packet, int clientId);
    public class PacketHandler : IPacketHandler
    {
        private Dictionary<int, PacketHandlerMethod> handlerMap;
        internal PacketHandler(Dictionary<int, PacketHandlerMethod> handlerMap)
        {
            this.handlerMap = handlerMap;
        }
        public void HandlePacket(IPacket packet, int clientId)
        {
            handlerMap[packet.PacketTypeId](packet, clientId);
        }
    }
}
