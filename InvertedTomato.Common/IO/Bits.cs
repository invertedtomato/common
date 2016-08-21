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

        public static ulong Push(ulong host, ulong add, byte count) {
            if (count > 64) {
                throw new ArgumentOutOfRangeException("Must be between 0 and 64, not " + count + ".", "count");
            }

            // Remove unwanted bits
            add <<= 64 - count;
            add >>= 64 - count;

            // Make room on host
            host <<= count;

            // Add the bits
            host |= add;

            return host;
        }

        public static ulong Pop(ulong host, byte count) {
            if (count > 64) {
                throw new ArgumentOutOfRangeException("Must be between 0 and 64, not " + count + ".", "count");
            }

            // Create mask
            var mask = ulong.MaxValue;
            mask >>= 64 - count;

            // Extract
            var remove = host & mask;

            // Remove from host
            host >>= count;

            return remove;
        }

        /// <summary>
        /// Convert a ulong to a binary string. No byte reordering - the MSB is always on the left, LSB is always on the right. A space between bytes. No padding.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(ulong value) {
            return ToString(value, 1);
        }

        /// <summary>
        /// Convert a ulong to a binary string. No byte reordering - the MSB is always on the left, LSB is always on the right. A space between bytes. Padding only if required to meet minBits.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minBits"></param>
        /// <returns></returns>
        public static string ToString(ulong value, byte minBits) {
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
