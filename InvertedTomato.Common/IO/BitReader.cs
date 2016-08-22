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
        
        public bool TryRead(out ulong result, byte count) {
            if (count > 64) {
                throw new ArgumentOutOfRangeException("Count must be between 0 and 64, not " + count + ".", "count");
            }
            if (IsDisposed) {
                throw new ObjectDisposedException("this");
            }

            result = 0;

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
                result <<= chunk;

                // Add bits
                result |= (ulong)(BufferValue & bufferMask) >> 8 - chunk - BufferPosition;

                // Reduce count by number of retrieved bits
                count -= chunk;

                // Increment position
                BufferPosition += chunk;
            }

            return true;
        }

        public ulong Read(byte count) {
            ulong value;
            if (!TryRead(out value, count)) {
                throw new EndOfStreamException();
            }

            return value;
        }

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
