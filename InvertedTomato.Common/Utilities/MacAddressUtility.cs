using System;
using System.Linq;

namespace InvertedTomato {
    [Obsolete("This is flawed, to be removed in a future release.")]
    public static class MacAddressUtility {
        /// <summary>
        /// Converts the mac address string to a byte array.
        /// </summary>
        /// <param name="macAddress">The mac address.</param>
        /// <param name="fromBase">The from base of the input string.</param>
        /// <returns></returns>
        [Obsolete("This is flawed, to be removed in a future release.")]
        public static byte[] ToMacAddress(this string macAddress, int fromBase) {
            var macAddressArray = macAddress.Split(':').Select(b => Convert.ToByte(b, fromBase)).ToArray();
            return macAddressArray;
        }
    }
}
