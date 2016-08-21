using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using InvertedTomato.IO;
using System.Collections.Generic;
using System.Linq;

namespace InvertedTomato.Common.Tests.IO {
    [TestClass]
    public class BitReaderTests {
        private IEnumerable<bool> Read1(string value) {
            using (var stream = new MemoryStream(Bits.ParseString(value))) {
                using (var reader = new BitReader(stream)) {
                    for (var i = 0; i < value.Replace(" ", "").Length; i++) {
                        yield return reader.Read1();
                    }
                }
            }
        }
        private IEnumerable<string> Read8(string value, byte length) {
            using (var stream = new MemoryStream(Bits.ParseString(value))) {
                using (var reader = new BitReader(stream)) {
                    for (var i = 0; i < value.Replace(" ", "").Length / length; i++) {
                        yield return Bits.ToString(reader.Read8(length));
                    }
                }
            }
        }
        private IEnumerable<string> Read64(string value, byte length) {
            using (var stream = new MemoryStream(Bits.ParseString(value))) {
                using (var reader = new BitReader(stream)) {
                    for (var i = 0; i < value.Replace(" ", "").Length / length; i++) {
                        yield return Bits.ToString(reader.Read64(length));
                    }
                }
            }
        }

        private bool Peak1(string value) {
            using (var stream = new MemoryStream(Bits.ParseString(value))) {
                using (var reader = new BitReader(stream)) {
                    return reader.Peak1();
                }
            }
        }

        [TestMethod]
        public void Peak1_0() {
            Assert.AreEqual(false, Peak1("00000000"));
        }
        [TestMethod]
        public void Peak1_1() {
            Assert.AreEqual(true, Peak1("10000000"));
        }

        [TestMethod]
        public void Read1_0() {
            var result = Read1("01000000 10000000");
            Assert.AreEqual(false, result.ElementAt(0));
            Assert.AreEqual(true, result.ElementAt(1));
            Assert.AreEqual(false, result.ElementAt(2));
            Assert.AreEqual(false, result.ElementAt(3));
            Assert.AreEqual(false, result.ElementAt(4));
            Assert.AreEqual(false, result.ElementAt(5));
            Assert.AreEqual(false, result.ElementAt(6));
            Assert.AreEqual(false, result.ElementAt(7));

            Assert.AreEqual(true, result.ElementAt(8));
            Assert.AreEqual(false, result.ElementAt(9));
            Assert.AreEqual(false, result.ElementAt(10));
            Assert.AreEqual(false, result.ElementAt(11));
            Assert.AreEqual(false, result.ElementAt(12));
            Assert.AreEqual(false, result.ElementAt(13));
            Assert.AreEqual(false, result.ElementAt(14));
            Assert.AreEqual(false, result.ElementAt(15));
        }

        [TestMethod]
        public void Read8_1() {
            var result = Read8("01000000 10000000", 1);
            Assert.AreEqual("00000000", result.ElementAt(0));
            Assert.AreEqual("00000001", result.ElementAt(1));
            Assert.AreEqual("00000000", result.ElementAt(2));
            Assert.AreEqual("00000000", result.ElementAt(3));
            Assert.AreEqual("00000000", result.ElementAt(4));
            Assert.AreEqual("00000000", result.ElementAt(5));
            Assert.AreEqual("00000000", result.ElementAt(6));
            Assert.AreEqual("00000000", result.ElementAt(7));

            Assert.AreEqual("00000001", result.ElementAt(8));
            Assert.AreEqual("00000000", result.ElementAt(9));
            Assert.AreEqual("00000000", result.ElementAt(10));
            Assert.AreEqual("00000000", result.ElementAt(11));
            Assert.AreEqual("00000000", result.ElementAt(12));
            Assert.AreEqual("00000000", result.ElementAt(13));
            Assert.AreEqual("00000000", result.ElementAt(14));
            Assert.AreEqual("00000000", result.ElementAt(15));
        }

        [TestMethod]
        public void Read8_5() {
            var result = Read8("01000000 10000000 00000000", 5);
            Assert.AreEqual("00001000", result.ElementAt(0));
            Assert.AreEqual("00000010", result.ElementAt(1));
            Assert.AreEqual("00000000", result.ElementAt(3));
        }

        [TestMethod]
        public void Read64_1() {
            var result = Read64("01000000 10000000", 1);
            Assert.AreEqual("00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000", result.ElementAt(0));
            Assert.AreEqual("00000001 00000000 00000000 00000000 00000000 00000000 00000000 00000000", result.ElementAt(1));
            Assert.AreEqual("00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000", result.ElementAt(2));
            Assert.AreEqual("00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000", result.ElementAt(3));
            Assert.AreEqual("00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000", result.ElementAt(4));
            Assert.AreEqual("00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000", result.ElementAt(5));
            Assert.AreEqual("00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000", result.ElementAt(6));
            Assert.AreEqual("00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000", result.ElementAt(7));

            Assert.AreEqual("00000001 00000000 00000000 00000000 00000000 00000000 00000000 00000000", result.ElementAt(8));
            Assert.AreEqual("00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000", result.ElementAt(9));
            Assert.AreEqual("00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000", result.ElementAt(10));
            Assert.AreEqual("00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000", result.ElementAt(11));
            Assert.AreEqual("00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000", result.ElementAt(12));
            Assert.AreEqual("00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000", result.ElementAt(13));
            Assert.AreEqual("00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000", result.ElementAt(14));
            Assert.AreEqual("00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000", result.ElementAt(15));
        }

        [TestMethod]
        public void Read64_12() {
            var result = Read64("01000000 10000000 00000000", 12);
            Assert.AreEqual("00001000 00000100 00000000 00000000 00000000 00000000 00000000 00000000", result.ElementAt(0));
            Assert.AreEqual("00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000", result.ElementAt(1));
        }

        [TestMethod]
        public void ReadWrite() {
            using(var stream = new MemoryStream()) {
                using(var writer = new BitWriter(stream)) {
                    for (ulong i = 1; i < ulong.MaxValue/2; i *= 2) {
                        writer.Write64(i,Bits.CountUsed(i));
                    }
                }

                stream.Position = 0;

                using (var reader = new BitReader(stream)) {
                    for (ulong i = 1; i < ulong.MaxValue / 2; i *= 2) {
                        Assert.AreEqual(i, reader.Read64(Bits.CountUsed(i)));
                    }
                }
            }
        }
    }
}
