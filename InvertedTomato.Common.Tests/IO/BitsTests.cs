using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InvertedTomato.IO;
using InvertedTomato.Utilities;

namespace InvertedTomato.Common.Tests.IO {
    [TestClass]
    public class BitsTests {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseString_Bad1() {
            ByteArrayUtility.ParseBinaryString("0");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseString_Bad2() {
            ByteArrayUtility.ParseBinaryString("00000000a");
        }

        [TestMethod]
        public void ToString_0() {
            Assert.AreEqual("0", Bits.ToString(0));
        }
        [TestMethod]
        public void ToString_1() {
            Assert.AreEqual("1", Bits.ToString(1));
        }
        [TestMethod]
        public void ToString_2() {
            Assert.AreEqual("10", Bits.ToString(2));
        }
        [TestMethod]
        public void ToString_255() {
            Assert.AreEqual("11111111", Bits.ToString(255));
        }
        [TestMethod]
        public void ToString_256() {
            Assert.AreEqual("1 00000000", Bits.ToString(256));
        }
        [TestMethod]
        public void ToString_256_16() {
            Assert.AreEqual("00000001 00000000", Bits.ToString(256,16));
        }
        [TestMethod]
        public void ToString_65535() {
            Assert.AreEqual("11111111 11111111", Bits.ToString(65535));
        }
        [TestMethod]
        public void ToString_Max() {
            Assert.AreEqual("11111111 11111111 11111111 11111111 11111111 11111111 11111111 11111111", Bits.ToString(ulong.MaxValue));
        }

        [TestMethod]
        public void CountUsed_0() {
            Assert.AreEqual((ulong)1, Bits.CountUsed(0));
        }
        [TestMethod]
        public void CountUsed_1() {
            Assert.AreEqual((ulong)1, Bits.CountUsed(1));
        }
        [TestMethod]
        public void CountUsed_3() {
            Assert.AreEqual((ulong)2, Bits.CountUsed(3));
        }


        [TestMethod]
        public void Push_0_1_1() {
            Assert.AreEqual((ulong)1,Bits.Push(0, 1, 1));
        }
        [TestMethod]
        public void Push_1_1_1() {
            Assert.AreEqual((ulong)3, Bits.Push(1, 1, 1));
        }
        [TestMethod]
        public void Push_1_3_1() {
            Assert.AreEqual((ulong)3, Bits.Push(1, 3, 1));
        }
        [TestMethod]
        public void Push_1_3_2() {
            Assert.AreEqual((ulong)7, Bits.Push(1, 3, 2));
        }

        [TestMethod]
        public void Pop_0_1() {
            Assert.AreEqual((ulong)0, Bits.Pop(0, 1));
        }
        [TestMethod]
        public void Pop_3_1() {
            Assert.AreEqual((ulong)1, Bits.Pop(3, 1));
        }
    }
}
