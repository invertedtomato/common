using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace InvertedTomato.IO {
    /// <summary>
    /// Tools for managing bits.
    /// </summary>
    public static class Bits {
        private static Regex Binary = new Regex("^([01]{8})+$");

        /// <summary>
        /// Parse a binary string into a byte array.
        /// </summary>
        /// <param name="input">A string of 1s and 0s in groups of 8. Can also include placeholder character '_' which is treated as a 0.</param>
        /// <returns></returns>
        public static byte[] ParseString(string input) {
            // Clean input
            input = input.Replace(" ", "") // Remove any spaces
                .Replace('_','0'); // Replace placeholder 0

            // Abort if input isn't sane
            if (!Binary.IsMatch(input)) {
                throw new ArgumentException("Not valid binary.", "input");
            }
            
            // Do the conversion
            return Enumerable
                .Range(0, input.Length / 8).Select(i => input.Substring(i * 8, 8)) // Split into 8-character chunks
                .Select(a => Convert.ToByte(a, 2)) // Convert to bytes
                .ToArray(); // Convert to array
        }

        /// <summary>
        /// Convert a byte array into a binary string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToString(byte[] input) {
            if (null == input) {
                throw new ArgumentNullException("input");
            }

            // Do the conversion
            return string.Join(" ", input.Select(a => Convert.ToString(a, 2).PadLeft(8, '0')));
        }

        /// <summary>
        /// Convert a byte into a binary string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(byte value) {
            return Convert.ToString(value, 2).PadLeft(8, '0'); // This is required, otherwise BitConverter.GetBytes will cast to ushort
        }

        /// <summary>
        /// Convert a uint to a binary string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(uint value) {
            return ToString(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Convert a ulong to a binary string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(ulong value) {
            return ToString(BitConverter.GetBytes(value));
        }

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
    }
}
