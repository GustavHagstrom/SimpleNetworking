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
            DateTime expectedSentTime = new DateTime(2020, 01, 01, 0, 0, 0);
            DateTime expectedReceivedTime = new DateTime(2020, 01, 01, 0, 0, 2);
            int expectedClientId = 11;
            byte[] expectedData = BitConverter.GetBytes(255);

            Packet firstPacket = new Packet
            {
                ClientId = expectedClientId,
                Sent = expectedSentTime,
                Received = expectedReceivedTime,
                PacketTypeId = expectedPacketId,
                Data = expectedData

            };
            Packet secondPacket = new Packet();
            secondPacket.AllBytes = firstPacket.AllBytes;
            int expectedPacketLength = firstPacket.AllBytes.Length;

            Assert.Equal(expectedData, firstPacket.Data);
            Assert.Equal(expectedPacketId, firstPacket.PacketTypeId);
            Assert.Equal(expectedReceivedTime, firstPacket.Received);
            Assert.Equal(expectedSentTime, firstPacket.Sent);
            Assert.Equal(expectedClientId, firstPacket.ClientId);
            Assert.Equal(expectedPacketLength, firstPacket.AllBytes.Length);

            Assert.Equal(expectedData, secondPacket.Data);
            Assert.Equal(expectedPacketId, secondPacket.PacketTypeId);
            Assert.Equal(expectedReceivedTime, secondPacket.Received);
            Assert.Equal(expectedSentTime, secondPacket.Sent);
            Assert.Equal(expectedClientId, secondPacket.ClientId);
            Assert.Equal(expectedPacketLength, secondPacket.AllBytes.Length);
        }

    }
}
