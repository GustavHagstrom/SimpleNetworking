using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using SimpleNetworking;
using Xunit;

namespace SimpleNetworking.Tests
{
    public class PacketTests
    {
        [Theory]
        [InlineData(1, new byte[] { 1, 1, 1 })]
        [InlineData(2, new byte[] { 1, 1, 1 })]
        public void PacketIdTest(int expectedId, byte[] expectedData)
        {
            //Arrange
            Packet packet = Packet.NewPacket(expectedId, expectedData);

            //Assert
            Assert.Equal(expectedId, packet.PacketTypeId);
            Assert.Equal(expectedData, packet.Data);

        }

        [Theory]
        [ClassData(typeof(SerializeableClass))]
        public void PacketDataTest(SerializeableClass expectedData)
        {
            //Arrange
            int packetId = 1;
            byte[] data = ConvertToBinaryData(expectedData);
            
            IFormatter formatter = new BinaryFormatter();


            //Act
            Packet packet = Packet.NewPacket(packetId, data);
            MemoryStream stream = new MemoryStream(packet.Data);
            SerializeableClass actualData = (SerializeableClass)formatter.Deserialize(stream);


            //Assert
            Assert.Equal(expectedData.someText, actualData.someText);
            Assert.Equal(expectedData.value1, actualData.value1);

        }
        private byte[] ConvertToBinaryData(object data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, data);
            stream.Close();
            return stream.ToArray();
        }
    }
}
