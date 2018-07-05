using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace InvertedTomato.Common.Tests.Extensions {
    [TestClass]
    public class StreamExtensionTests {
        [TestMethod]
        public void ReadFloat() {
            using (var stream = new MemoryStream()) {
                stream.Write(BitConverter.GetBytes((float)1.1));
                stream.Write(BitConverter.GetBytes((float)2.2));
                stream.Write(BitConverter.GetBytes((float)3.3));

                stream.Position = 0;

                Assert.AreEqual(1.1, Math.Round(stream.ReadFloat(), 1));
                Assert.AreEqual(2.2, Math.Round(stream.ReadFloat(), 1));
                Assert.AreEqual(3.3, Math.Round(stream.ReadFloat(), 1));
            }
        }

        [TestMethod]
        public void ReadDouble() {
            using (var stream = new MemoryStream()) {
                stream.Write(BitConverter.GetBytes((double)1.1));
                stream.Write(BitConverter.GetBytes((double)2.2));
                stream.Write(BitConverter.GetBytes((double)3.3));

                stream.Position = 0;

                Assert.AreEqual(1.1, stream.ReadDouble());
                Assert.AreEqual(2.2, stream.ReadDouble());
                Assert.AreEqual(3.3, stream.ReadDouble());
            }
        }

    }
}
