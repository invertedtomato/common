using System;
using System.IO;

namespace InvertedTomato.IO {
    /// <summary>
    /// Write bits or groups of bits to a stream.
    /// </summary>
    public class BitWriter : IDisposable {
        // LEAST significant BIT is on the RIGHT of the byte
        // LEAST significant BYTE is the FIRST in the stream
        // MOST significant BIT is on the LEFT of the byte
        // MOST significant BYTE is the LAST in the stream

        /// <summary>
        /// If disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Underlying stream to output to.
        /// </summary>
        private readonly Stream Output;

        /// <summary>
        /// The current byte being worked on.
        /// </summary>
        private byte BufferValue;

        /// <summary>
        /// The current bit within the current byte being worked on next.
        /// </summary>
        private byte BufferPosition;

        /// <summary>
        /// Standard instantiation.
        /// </summary>
        /// <param name="output"></param>
        public BitWriter(Stream output) {
            if (null == output) {
                throw new ArgumentNullException("input");
            }

            Output = output;
        }
        
        /// <summary>
        /// Write bits from 64-bit buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="count">Number of bits to take,</param>
        public void Write(ulong buffer, byte count) {
            if (count > 64) {
                throw new ArgumentOutOfRangeException("Count must be between 0 and 64, not " + count + ".", "count");
            }

            // Remove unwanted bits
            buffer <<= 64 - count;
            buffer >>= 64 - count;
            
            // While there are bits remaining
            while (count > 0) {
                // Calculate chunk size we're writing next - the number of bits remaining to be written, or the amount of space left in the buffer - whichever is least
                var chunk = (byte)Math.Min(count, 8 - BufferPosition);

                // Add to byte
                if (BufferPosition + count > 8) {
                    BufferValue |= (byte)(buffer >> count - chunk);
                } else {
                    BufferValue |= (byte)(buffer << 8 - BufferPosition - chunk);
                }

                // Update length available
                count -= chunk;

                // Move position
                BufferPosition += chunk;

                // If buffer is full...
                if (BufferPosition == 8) {
                    // Write buffer
                    Flush();
                }

#if DEBUG
                // Catch insane situation
                if (BufferPosition > 8) {
                    throw new Exception("Invalid position " + BufferPosition + ". Position has been offset by an incorrect value.");
                }
#endif
            }
        }

        /// <summary>
        /// Flush current byte from buffer
        /// </summary>
        private void Flush() {
            // Abort flush if there's nothing to flush
            if (BufferPosition == 0) {
                return;
            }

            // Flush buffer
            Output.WriteByte(BufferValue);

            // Clear buffer
            BufferValue = 0;

            // Reset position
            BufferPosition = 0;
        }

        protected virtual void Dispose(bool disposing) {
            if (IsDisposed) {
                return;
            }
            IsDisposed = true;

            if (disposing) {
                // Flush buffer
                Flush();

                // Dispose managed state (managed objects).
            }
        }

        public void Dispose() {
            Dispose(true);
        }
    }



}
