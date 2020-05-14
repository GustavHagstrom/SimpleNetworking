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
            byte[] expectedData = BitConverter.GetBytes(255);

            Packet firstPacket = new Packet
            {
                ClientId = expectedClientId,
                PacketTypeId = expectedPacketId,
                Data = expectedData

            };
            Packet secondPacket = new Packet();
            secondPacket.AllBytes = firstPacket.AllBytes;
            int expectedPacketLength = firstPacket.AllBytes.Length;

            Assert.Equal(expectedData, firstPacket.Data);
            Assert.Equal(expectedPacketId, firstPacket.PacketTypeId);
            Assert.Equal(expectedClientId, firstPacket.ClientId);
            Assert.Equal(expectedPacketLength, firstPacket.AllBytes.Length);

            Assert.Equal(expectedData, secondPacket.Data);
            Assert.Equal(expectedPacketId, secondPacket.PacketTypeId);
            Assert.Equal(expectedClientId, secondPacket.ClientId);
            Assert.Equal(expectedPacketLength, secondPacket.AllBytes.Length);

            Assert.Equal(firstPacket.PacketLength, firstPacket.AllBytes.Length);
            Assert.Equal(secondPacket.PacketLength, secondPacket.AllBytes.Length);
        }

    }
}
