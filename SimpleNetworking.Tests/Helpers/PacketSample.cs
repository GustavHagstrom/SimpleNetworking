using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SimpleNetworking.Tests
{
    public class PacketSample : IEnumerable<object[]>
    {
        private byte[] ConvertToBinaryData(object data)
        {
            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, data);
                stream.Close();
                return stream.ToArray();
            }

        }
        public IEnumerator<object[]> GetEnumerator()
        {
            Packet packet1 = Packet.NewPacket((int)PacketId.Value, ConvertToBinaryData(new SerializeableClass()));
            Packet packet2 = Packet.NewPacket((int)PacketId.Text, ConvertToBinaryData(new SerializeableClass()));


            yield return new object[] { packet1 };
            yield return new object[] { packet2 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
