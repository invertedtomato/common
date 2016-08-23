using System;

namespace InvertedTomato.IO {
    /// <summary>
    /// Tools for managing bit sets.
    /// </summary>
    public static class Bits {
        /// <summary>
        /// Count the number of bits used to express number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte CountUsed(ulong value) {
            byte bits = 0;

            do {
                bits++;
                value >>= 1;
            } while (value > 0);

            return bits;
        }

        [Obsolete("Will be removed in a future release.")]
        public static ulong Push(ulong host, ulong bits, byte count) { return Push(host, bits, (int)count); }

        /// <summary>
        /// Push bits onto the least-significant side of a ulong.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="bits"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static ulong Push(ulong host, ulong bits, int count) {
            if (count > 64) {
                throw new ArgumentOutOfRangeException("Must be between 0 and 64, not " + count + ".", "count");
            }

            // Add space on host
            host <<= count;

            // Add bits
            host |= bits & (ulong.MaxValue >> 64 - count);

            return host;
        }

        [Obsolete("Will be removed in a future release.")]
        public static ulong Pop(ulong host, byte count) { return Pop(host, (int)count); }

        /// <summary>
        /// Pop bits off the least-significant side of a ulong.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static ulong Pop(ulong host, int count) {
            if (count > 64) {
                throw new ArgumentOutOfRangeException("Must be between 0 and 64, not " + count + ".", "count");
            }

            // Extract bits
            var bits = host & (ulong.MaxValue >> 64 - count);

            // Remove space from host
            host >>= count;

            return bits;
        }

        /// <summary>
        /// Convert a ulong to a binary string. No byte reordering - the MSB is always on the left, LSB is always on the right. A space between bytes. No padding.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(ulong value) {
            return ToString(value, 1);
        }

        public static string ToString(ulong value, byte minBits) { return ToString(value, (int)minBits); }

        /// <summary>
        /// Convert a ulong to a binary string. No byte reordering - the MSB is always on the left, LSB is always on the right. A space between bytes. Padding only if required to meet minBits.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minBits"></param>
        /// <returns></returns>
        public static string ToString(ulong value, int minBits) {
            var output = "";

            var pos = 0;
            do {
                output = (value % 2 == 0 ? "0" : "1") + output;
                value >>= 1;
                if (++pos % 8 == 0) {
                    output = " " + output;
                }
            } while (pos < minBits || value > 0);

            return output.Trim();
        }
    }
}
