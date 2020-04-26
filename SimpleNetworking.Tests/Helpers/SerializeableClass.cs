using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SimpleNetworking.Tests
{
    [Serializable]
    public class SerializeableClass:IEnumerable<object[]>
    {
        public int value1 = 3;
        public string someText = "some sample text";

        public SerializeableClass()
        {

        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new SerializeableClass() };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
