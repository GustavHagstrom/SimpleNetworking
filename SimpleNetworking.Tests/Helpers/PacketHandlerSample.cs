using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SimpleNetworking.Tests
{
    enum PacketId
    {
        Value = 1,
        Text = 2
    }
    public class PacketHandlerSample : IEnumerable<object[]>
    {
        private PacketHandler packetHandler;

        public int value { get; set; } = 0;
        public string text { get; set; } = "";
        public PacketHandlerSample()
        {
            InitializeClass();
        }
        public void DataReceived(byte[] data)
        {
            HandleData(data);
        }
        private void HandleData(byte[] data)
        {
            Packet packet = Packet.FromReceivedBytes(data);
            packetHandler.HandlePacket(packet);
        }
        private void InitializeClass()
        {
            PacketHandlerBuilder builder = new PacketHandlerBuilder();
            builder.RegisterHandlerMethod((int)PacketId.Value, HandleSerializeableClassValue);
            builder.RegisterHandlerMethod((int)PacketId.Text, HandleSerializeableClassText);
            this.packetHandler = builder.Build();
        }
        private void HandleSerializeableClassValue(Packet packet)
        {
            SerializeableClass myObject = (SerializeableClass)Deserialize(packet.Data);
            this.value = myObject.value1;
        }
        private void HandleSerializeableClassText(Packet packet)
        {
            SerializeableClass myObject = (SerializeableClass)Deserialize(packet.Data);
            this.text = myObject.someText;
        }
        private object Deserialize(byte[] data)
        {
            IFormatter formatter = new BinaryFormatter();
            using(MemoryStream stream = new MemoryStream(data))
            {
                return formatter.Deserialize(stream);
            }

        }

        public IEnumerator<object[]> GetEnumerator()
        {

            yield return new object[] { new PacketHandlerSample() };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
