using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InvertedTomato.Utilities;

namespace InvertedTomato.Common.Tests.Utilities {
    [TestClass]
    public class ByteArrayUtilityTests {
        [TestMethod]
        public void ParseBinaryString_00000000() {
            var result = ByteArrayUtility.ParseBinaryString("00000000");
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(byte.MinValue, result[0]);
        }
        [TestMethod]
        public void ParseBinaryString_________() {
            var result = ByteArrayUtility.ParseBinaryString("________");
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(byte.MinValue, result[0]);
        }
        [TestMethod]
        public void ParseBinaryString_11111111() {
            var result = ByteArrayUtility.ParseBinaryString("11111111");
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(byte.MaxValue, result[0]);
        }
        [TestMethod]
        public void ParseBinaryString_11111111_00000000() {
            var result = ByteArrayUtility.ParseBinaryString("11111111 00000000");
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(byte.MaxValue, result[0]);
            Assert.AreEqual(byte.MinValue, result[1]);
        }
    }
}
