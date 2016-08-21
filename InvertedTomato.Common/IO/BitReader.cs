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

        public ulong Read(byte count) {
            if (count > 64) {
                throw new ArgumentOutOfRangeException("Count must be between 0 and 64, not " + count + ".", "count");
            }
            if (IsDisposed) {
                throw new ObjectDisposedException("this");
            }

            ulong result = 0;

            while (count > 0) {
                // If needed, load byte
                ReadBufferIfNeeded();

                // Calculate chunk size
                var chunk = (byte)Math.Min(count, 8 - BufferPosition);

                // Create mask
                var bufferMask = byte.MaxValue;
                bufferMask <<= 8 - chunk;
                bufferMask >>= BufferPosition;

                // Make room in output
                result <<= chunk;

                // Add bits
                result |= (ulong)(BufferValue & bufferMask) >> 8 - chunk - BufferPosition;

                // Reduce count by number of retrieved bits
                count -= chunk;

                // Increment position
                BufferPosition += chunk;
            }

            return result;
        }

        public bool PeakBit() {
            // If needed, load byte
            ReadBufferIfNeeded();

            // Return bit
            return (BufferValue & (1 << 7 - BufferPosition)) > 0;
        }

        private void ReadBufferIfNeeded() {
            // Only load if needed
            if (BufferPosition < 8) {
                return;
            } else if (BufferPosition > 8) {
                throw new Exception("Invalid position " + BufferPosition + ". Position has been offset by an incorrect value.");
            }

            // Read next byte
            var buffer = Input.ReadByte();
            if (buffer < 0) {
                throw new EndOfStreamException();
            }
            BufferValue = (byte)buffer;

            // Reset position
            BufferPosition = 0;
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
