using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleNetworking
{
    
    public class Packet : IPacket
    {
        private List<byte> buffer;

        public int PacketTypeId
        {
            get
            {
                return BitConverter.ToInt32(buffer.Take(4).ToArray());
            }
        }
        public byte[] Data
        {
            get
            {
                return buffer.Skip(4).ToArray();
            }
        }
        public byte[] Binarys
        {
            get
            {
                return buffer.ToArray();
            }
        }

        private Packet()
        {
            buffer = new List<byte>();
        }
        public static Packet FromReceivedBytes(byte[] data)
        {
            Packet packet = new Packet();
            packet.Write(data);
            return packet;
        }
        public static Packet NewPacket(int packetIdType, byte[] data)
        {
            Packet packet = new Packet();
            packet.Write(packetIdType);
            packet.Write(data);
            return packet;
        }
        private void Write(byte[] data)
        {
            buffer.AddRange(data);
        }
        private void Write(int id)
        {
            buffer.AddRange(BitConverter.GetBytes(id));
        }


    }
}
