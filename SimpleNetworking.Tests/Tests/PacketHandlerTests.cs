using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SimpleNetworking.Tests
{
    public class PacketHandlerTests
    {
        [Theory]
        [ClassData(typeof(PacketSample))]
        public void PacketHandlerTest(Packet packet)
        {
            //Arrange
            PacketHandlerSample sampleHandler = new PacketHandlerSample();
            SerializeableClass sampleClass = new SerializeableClass();
            string actualText = sampleClass.someText;
            int actualInt = sampleClass.value1;


            //Act
            sampleHandler.DataReceived(packet.Binarys);


            //Assert
            if(packet.PacketTypeId == (int)PacketId.Value)
            {
                Assert.Equal(actualInt, sampleHandler.value);
            }
            else if(packet.PacketTypeId == (int)PacketId.Text)
            {
                Assert.Equal(actualText, sampleHandler.text);
            }
        }
    }
}
