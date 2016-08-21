using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InvertedTomato.IO;

namespace InvertedTomato.Common.Tests.IO {
    [TestClass]
    public class BitsTests {
        [TestMethod]
        public void ParseString_00000000() {
            var result = Bits.ParseString("00000000");
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(byte.MinValue, result[0]);
        }
        [TestMethod]
        public void ParseString_________() {
            var result = Bits.ParseString("________");
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(byte.MinValue, result[0]);
        }
        [TestMethod]
        public void ParseString_11111111() {
            var result = Bits.ParseString("11111111");
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(byte.MaxValue, result[0]);
        }
        [TestMethod]
        public void ParseString_11111111_00000000() {
            var result = Bits.ParseString("11111111 00000000");
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(byte.MaxValue, result[0]);
            Assert.AreEqual(byte.MinValue, result[1]);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseString_Bad1() {
            Bits.ParseString("0");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseString_Bad2() {
            Bits.ParseString("00000000a");
        }

        [TestMethod]
        public void ToString_Array_0() {
            Assert.AreEqual("00000000", Bits.ToString(new byte[] { 0 }));
        }
        [TestMethod]
        public void ToString_Array_1() {
            Assert.AreEqual("00000001", Bits.ToString(new byte[] { 1 }));
        }
        [TestMethod]
        public void ToString_Array_255() {
            Assert.AreEqual("11111111", Bits.ToString(new byte[] { 255 }));
        }
        [TestMethod]
        public void ToString_Array_0_255() {
            Assert.AreEqual("00000000 11111111", Bits.ToString(new byte[] { 0, 255 }));
        }
        [TestMethod]
        public void ToString_Byte_0() {
            Assert.AreEqual("00000000", Bits.ToString((byte)0));
        }
        [TestMethod]
        public void ToString_Byte_1() {
            Assert.AreEqual("00000001", Bits.ToString((byte)1));
        }
        [TestMethod]
        public void ToString_Byte_255() {
            Assert.AreEqual("11111111", Bits.ToString((byte)255));
        }
        [TestMethod]
        public void ToString_ULong_1() {
            Assert.AreEqual("00000001 00000000 00000000 00000000 00000000 00000000 00000000 00000000", Bits.ToString((ulong)1));
        }
        [TestMethod]
        public void ToString_ULong_Max() {
            Assert.AreEqual("11111111 11111111 11111111 11111111 11111111 11111111 11111111 11111111", Bits.ToString(ulong.MaxValue));
        }

        [TestMethod]
        public void CountUsed_0() {
            Assert.AreEqual(1, Bits.CountUsed(0));
        }
        [TestMethod]
        public void CountUsed_1() {
            Assert.AreEqual(1, Bits.CountUsed(1));
        }
        [TestMethod]
        public void CountUsed_3() {
            Assert.AreEqual(2, Bits.CountUsed(3));
        }
    }
}
