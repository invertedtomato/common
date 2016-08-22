using System;
using System.IO;

namespace InvertedTomato.IO {
    /// <summary>
    /// Read bits or groups of bits from stream.
    /// </summary>
    public class BitReader : IDisposable {
        public bool IsDisposed { get; private set; }
        private readonly Stream Input;

        private byte BufferValue;
        private byte BufferPosition = 8;

        public BitReader(Stream input) {
            if (null == input) {
                throw new ArgumentNullException("input");
            }

            Input = input;
        }

        /// <summary>
        /// Read a set of bits. This uses ulong as a 64-bit buffer (don't think of it like an integer, think of it as a bit buffer).
        /// </summary>
        /// <param name="buffer">Buffer to retrieve bits into.</param>
        /// <param name="count">Number of bits to read, starting from the least-significant-bit (right side).</param>
        /// <returns></returns>
        public bool TryRead(out ulong buffer, byte count) {
            if (count > 64) {
                throw new ArgumentOutOfRangeException("Count must be between 0 and 64, not " + count + ".", "count");
            }
            if (IsDisposed) {
                throw new ObjectDisposedException("this");
            }

            buffer = 0;

            while (count > 0) {
                // If needed, load byte
                if (!TryReadBuffer()) {
                    return false;
                }

                // Calculate chunk size
                var chunk = (byte)Math.Min(count, 8 - BufferPosition);

                // Create mask
                var bufferMask = byte.MaxValue;
                bufferMask <<= 8 - chunk;
                bufferMask >>= BufferPosition;

                // Make room in output
                buffer <<= chunk;

                // Add bits
                buffer |= (ulong)(BufferValue & bufferMask) >> 8 - chunk - BufferPosition;

                // Reduce count by number of retrieved bits
                count -= chunk;

                // Increment position
                BufferPosition += chunk;
            }

            return true;
        }

        /// <summary>
        /// Read a set of bits. This uses ulong as a 64-bit buffer (don't think of it like an integer, think of it as a bit buffer).
        /// </summary>
        /// <param name="count">Number of bits to read, starting from the least-significant-bit (right side).</param>
        /// <returns></returns>
        public ulong Read(byte count) {
            ulong value;
            if (!TryRead(out value, count)) {
                throw new EndOfStreamException();
            }

            return value;
        }

        /// <summary>
        /// Try and view the next bit without moving the pointer.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryPeakBit(out bool value) {
            // If needed, load byte
            if (!TryReadBuffer()) {
                value = false;
                return false;
            }

            // Return bit
            value = (BufferValue & (1 << 7 - BufferPosition)) > 0;
            return true;
        }

        /// <summary>
        /// View the next bit without moving the underlying pointer.
        /// </summary>
        /// <returns></returns>
        public bool PeakBit() {
            bool value;
            if (!TryPeakBit(out value)) {
                throw new EndOfStreamException();
            }

            return value;
        }

        private bool TryReadBuffer() {
            // Only load if needed
            if (BufferPosition < 8) {
                return true;
            } else if (BufferPosition > 8) {
                throw new Exception("Invalid position " + BufferPosition + ". Position has been offset by an incorrect value.");
            }

            // Read next byte
            var buffer = Input.ReadByte();
            if (buffer < 0) {
                return false;
            }
            BufferValue = (byte)buffer;

            // Reset position
            BufferPosition = 0;

            return true;
        }


        protected virtual void Dispose(bool disposing) {
            if (IsDisposed) {
                return;
            }
            IsDisposed = true;

            if (disposing) {
                // Dispose managed state (managed objects).
            }
        }

        public void Dispose() {
            Dispose(true);
        }
    }
}
