using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using InvertedTomato.IO;

namespace InvertedTomato.Common.Tests.IO {
    [TestClass]
    public class BitWriterTests {
        private string WriteBits(params bool[] values) {
            using (var stream = new MemoryStream()) {
                using (var writer = new BitWriter(stream)) {
                    foreach (var value in values) {
                        writer.Write((ulong)(value ? 1 : 0), 1);
                    }
                }
                return stream.ToArray().ToBinaryString();
            }
        }


        private string Write(ulong value1, byte length1, ulong value2 = 0, byte length2 = 0) {
            using (var stream = new MemoryStream()) {
                using (var writer = new BitWriter(stream)) {

                    writer.Write(value1, length1);
                    if (length2 > 0) {
                        writer.Write(value2, length2);
                    }

                }
                return stream.ToArray().ToBinaryString();
            }
        }

        [TestMethod]
        public void Write_1() {
            Assert.AreEqual("10000000", WriteBits(true));
        }
        [TestMethod]
        public void Write_0() {
            Assert.AreEqual("00000000", WriteBits(false));
        }
        [TestMethod]
        public void Write_111111111() {
            Assert.AreEqual("11111111 10000000", WriteBits(true, true, true, true, true, true, true, true, true));
        }
        [TestMethod]
        public void Write_000000000() {
            Assert.AreEqual("00000000 00000000", WriteBits(false, false, false, false, false, false, false, false, false));
        }
        [TestMethod]
        public void Write_1L1() {
            Assert.AreEqual("10000000", Write(1, 1));
        }
        [TestMethod]
        public void Write_1L8() {
            Assert.AreEqual("00000001", Write(1, 8));
        }
        [TestMethod]
        public void Write_1L8_1L4() {
            Assert.AreEqual("00000001 00010000", Write(1, 8, 1, 4));
        }
        [TestMethod]
        public void Write_255L2_1L8() {
            Assert.AreEqual("11000000 01000000", Write(255, 2, 1, 8));
        }
        [TestMethod]
        public void Write_1L0() {
            Assert.AreEqual("", Write(1, 0));
        }
        [TestMethod]
        public void Write_1L9() {
            Assert.AreEqual("00000000 10000000", Write((ulong)1, 9));
        }
        [TestMethod]
        public void Write_255L9() {
            Assert.AreEqual("01111111 10000000", Write((ulong)255, 9));
        }
    }
}