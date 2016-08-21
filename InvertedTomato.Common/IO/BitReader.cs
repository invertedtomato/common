using System;
using System.IO;

namespace InvertedTomato.IO {
    /// <summary>
    /// Reader for bit streams
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

        public bool Peak1() {
            // If needed, load byte
            ReadBufferIfNeeded();

            // Return bit
            return (BufferValue & (1 << 7 - BufferPosition)) > 0;
        }

        public bool Read1() {
            var value = Peak1();

            Increment(1);

            return value;
        }
        public byte Read8(byte count) { // TODO: These reads aren't real different.
            if (count > 8) {
                throw new ArgumentOutOfRangeException("Count must be between 0 and 8, not " + count + ".", "count");
            }
            byte result = 0;

            while (count > 0) {
                // If needed, load byte
                ReadBufferIfNeeded();

                // Calculate chunk size
                var chunk = CalculateChunk(count);

                // Create mask
                var bufferMask = byte.MaxValue;
                bufferMask <<= 8 - chunk;
                bufferMask >>= BufferPosition;
                
                // Make room in output
                result <<= chunk;

                // Add bits
                result |= (byte)((BufferValue & bufferMask) >> 8 - chunk - BufferPosition);

                // Reduce count by number of retrieved bits
                count -= chunk;

                // Increment offset
                Increment(chunk);
            }

            return result;
        }
        public ulong Read64(byte count) {
            if (count > 64) {
                throw new ArgumentOutOfRangeException("Count must be between 0 and 64, not " + count + ".", "count");
            }
            ulong result = 0;

            while (count > 0) {
                // If needed, load byte
                ReadBufferIfNeeded();

                // Calculate chunk size
                var chunk = CalculateChunk(count);

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

                // Increment offset
                Increment(chunk);
            }

            return result;
        }





        private void Increment(byte offset) {
            // Increment position
            BufferPosition += offset;
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

        private byte CalculateChunk(byte max) {
            return (byte)Math.Min(max, 8 - BufferPosition);
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
