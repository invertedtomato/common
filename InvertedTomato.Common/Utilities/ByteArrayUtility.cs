using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace InvertedTomato.Utilities {
    public static class ByteArrayUtility {
        private static Regex Binary = new Regex("^([01]{8})+$");

        /// <summary>
        /// Parse a binary string into a byte array.
        /// </summary>
        /// <param name="input">A string of 1s and 0s in groups of 8. Can also include placeholder character '_' which is treated as a 0.</param>
        /// <returns></returns>
        public static byte[] ParseBinaryString(string input) {
            if (null == input) {
                throw new ArgumentNullException("input");
            }

            // Clean input
            input = input.Replace(" ", "") // Remove any spaces
                .Replace('_', '0'); // Replace placeholder 0

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
    }
}
