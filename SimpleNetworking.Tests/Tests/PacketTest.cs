using SimpleNetworking.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SimpleNetworking.Tests.Tests
{
    public class PacketTest
    {
        [Fact]
        public void PropertiesTest()
        {
            int expectedPacketId = 1;
            int expectedClientId = 11;
            string expectedDataString = "Hello!";

            ExamplePacket firstPacket = new ExamplePacket
            {
                ClientId = expectedClientId,
                PacketTypeId = expectedPacketId,
                DataString = expectedDataString

            };
            ExamplePacket secondPacket = new ExamplePacket();
            secondPacket.Bytes = firstPacket.Bytes;
            int expectedPacketLength = firstPacket.Bytes.Length;

            Assert.Equal(expectedDataString, firstPacket.DataString);
            Assert.Equal(expectedPacketId, firstPacket.PacketTypeId);
            Assert.Equal(expectedClientId, firstPacket.ClientId);
            Assert.Equal(expectedPacketLength, firstPacket.Bytes.Length);

            Assert.Equal(expectedDataString, secondPacket.DataString);
            Assert.Equal(expectedPacketId, secondPacket.PacketTypeId);
            Assert.Equal(expectedClientId, secondPacket.ClientId);
            Assert.Equal(expectedPacketLength, secondPacket.Bytes.Length);

            Assert.Equal(firstPacket.PacketLength, firstPacket.Bytes.Length);
            Assert.Equal(secondPacket.PacketLength, secondPacket.Bytes.Length);
        }

    }
}
