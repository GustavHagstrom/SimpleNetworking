using System.Collections.Generic;

namespace SimpleNetworking
{
    public delegate void PacketHandlerMethod(Packet packet, int clientId);
    public class PacketHandler
    {
        private Dictionary<int, PacketHandlerMethod> handlerMap;
        internal PacketHandler(Dictionary<int, PacketHandlerMethod> handlerMap)
        {
            this.handlerMap = handlerMap;
        }
        public void HandlePacket(Packet packet, int clientId)
        {
            handlerMap[packet.PacketTypeId](packet, clientId);
        }
    }
}
