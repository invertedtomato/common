using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using InvertedTomato.IO;

namespace InvertedTomato.Common.Tests.IO {
    [TestClass]
    public class BitWriterTests {
        private string Write1(params bool[] values) {
            using (var stream = new MemoryStream()) {
                using (var writer = new BitWriter(stream)) {
                    foreach (var value in values) {
                        writer.Write1(value);
                    }
                }
                return Bits.ToString(stream.ToArray());
            }
        }

        private string Write8(byte value1, byte length1, byte value2 = 0, byte length2 = 0) {
            using (var stream = new MemoryStream()) {
                using (var writer = new BitWriter(stream)) {

                    writer.Write8(value1, length1, 0);
                    if (length2 > 0) {
                        writer.Write8(value2, length2, 0);
                    }

                }
                return Bits.ToString(stream.ToArray());
            }
        }

        private string Write64(ulong value1, byte length1, ulong value2 = 0, byte length2 = 0) {
            using (var stream = new MemoryStream()) {
                using (var writer = new BitWriter(stream)) {

                    writer.Write64(value1, length1, 0);
                    if (length2 > 0) {
                        writer.Write64(value2, length2, 0);
                    }

                }
                return Bits.ToString(stream.ToArray());
            }
        }

        [TestMethod]
        public void Write1_1() {
            Assert.AreEqual("10000000", Write1(true));
        }
        [TestMethod]
        public void Write1_0() {
            Assert.AreEqual("00000000", Write1(false));
        }
        [TestMethod]
        public void Write1_111111111() {
            Assert.AreEqual("11111111 10000000", Write1(true, true, true, true, true, true, true, true, true));
        }
        [TestMethod]
        public void Write1_000000000() {
            Assert.AreEqual("00000000 00000000", Write1(false, false, false, false, false, false, false, false, false));
        }

        [TestMethod]
        public void Write8_1L1() {
            Assert.AreEqual("10000000", Write8(1, 1));
        }
        [TestMethod]
        public void Write8_1L8() {
            Assert.AreEqual("00000001", Write8(1, 8));
        }
        [TestMethod]
        public void Write8_1L8_1L4() {
            Assert.AreEqual("00000001 00010000", Write8(1, 8, 1, 4));
        }
        [TestMethod]
        public void Write8_255L2_1L8() {
            Assert.AreEqual("11000000 01000000", Write8(255, 2, 1, 8));
        }
        [TestMethod]
        public void Write8_1L0() {
            Assert.AreEqual("", Write8(1, 0));
        }

        [TestMethod]
        public void Write64_1L1() {
            Assert.AreEqual("10000000", Write64(1, 1));
        }
        [TestMethod]
        public void Write64_1L8() {
            Assert.AreEqual("00000001", Write64(1, 8));
        }
        [TestMethod]
        public void Write64_1L9() {
            Assert.AreEqual("00000000 10000000", Write64(1, 9));
        }
        [TestMethod]
        public void Write64_255L9() {
            Assert.AreEqual("01111111 10000000", Write64(255, 9));
        }
        [TestMethod]
        public void Write64_1L8_1L4() {
            Assert.AreEqual("00000001 00010000", Write64(1, 8, 1, 4));
        }
        [TestMethod]
        public void Write64_255L2_1L8() {
            Assert.AreEqual("11000000 01000000", Write64(255, 2, 1, 8));
        }
        [TestMethod]
        public void Write64_1L0() {
            Assert.AreEqual("", Write64(1, 0));
        }
    }
}