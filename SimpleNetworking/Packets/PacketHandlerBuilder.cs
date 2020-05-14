using System.Collections.Generic;

namespace SimpleNetworking
{

    public class PacketHandlerBuilder
    {
        private Dictionary<int, PacketHandlerMethod> handlerMap;
        public PacketHandlerBuilder()
        {
            handlerMap = new Dictionary<int, PacketHandlerMethod>();
        }
        public PacketHandler Build()
        {
            Dictionary<int, PacketHandlerMethod> map = new Dictionary<int, PacketHandlerMethod>();
            foreach(var handler in handlerMap)
            {
                map.Add(handler.Key, handler.Value);
            }
            return new PacketHandler(map);
        }
        public void RegisterHandlerMethod(int packetId, PacketHandlerMethod handlerMethod)
        {
            handlerMap.Add(packetId, handlerMethod);
        }
        public void AddHandlerMap(Dictionary<int, PacketHandlerMethod> handlerMap)
        {
            foreach (var handler in handlerMap)
            {
                this.handlerMap.Add(handler.Key, handler.Value);
            }
        }
        public void ResetBuilder()
        {
            handlerMap = new Dictionary<int, PacketHandlerMethod>();
        }
    }
}
