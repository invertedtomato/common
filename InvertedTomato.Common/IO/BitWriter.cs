using System;
using System.IO;

namespace InvertedTomato.IO {
    /// <summary>
    /// Writer for bit streams.
    /// </summary>
    public class BitWriter : IDisposable {
        // Most significant BIT is on left of byte
        // Most significant BYTE is last

        public bool IsDisposed { get; private set; }
        private readonly Stream Output;

        private byte BufferValue;
        private byte BufferPosition;

        public BitWriter(Stream output) {
            if (null == output) {
                throw new ArgumentNullException("input");
            }

            Output = output;
        }

        /// <summary>
        /// Write a single bit.
        /// </summary>
        /// <param name="value"></param>
        public void Write1(bool value) {
            if (value) {
                BufferValue |= (byte)(1 << 7 - BufferPosition);
            }

            Increment(1);
        }

        /// <summary>
        /// Write bits from 8-bit buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="count">Number of bits to take,</param>
        public void Write8(byte buffer, byte count) {
            if (count > 8) {
                throw new ArgumentOutOfRangeException("Count must be between 0 and 8, not " + count + ".", "count");
            }

            // Remove unwanted bits and align on right
            buffer <<= 8 - count;
            buffer >>= 8 - count;

            while (count > 0) {
                // Calculate size of chunk
                var chunk = CalculateChunk(count);

                // Add to byte
                if (BufferPosition + count > 8) {
                    BufferValue |= (byte)(buffer >> count - chunk);
                } else {
                    BufferValue |= (byte)(buffer << 8 - BufferPosition - chunk);
                }

                // Update length available
                count -= chunk;

                // Move position
                Increment(chunk);
            }
        }

        /// <summary>
        /// Write bits from 64-bit buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="count">Number of bits to take,</param>
        public void Write64(ulong buffer, byte count) {
            if (count > 64) {
                throw new ArgumentOutOfRangeException("Count must be between 0 and 64, not " + count + ".", "count");
            }

            // Remove unwanted bits and align on right
            buffer <<= 64 - count;
            buffer >>= 64 - count;

            while (count > 0) {
                // Calculate size of chunk
                var chunk = CalculateChunk(count);

                // Add to byte
                if (BufferPosition + count > 8) {
                    BufferValue |= (byte)(buffer >> count - chunk);
                } else {
                    BufferValue |= (byte)(buffer << 8 - BufferPosition - chunk);
                }

                // Update length available
                count -= chunk;

                // Move position
                Increment(chunk);
            }
        }
        
        private void Increment(byte offset) {
            // Increment position
            BufferPosition += offset;

            // If buffer is full...
            if (BufferPosition == 8) {
                WriteBuffer();
            } else if (BufferPosition > 8) {
                throw new Exception("Invalid position " + BufferPosition + ". Position has been offset by an incorrect value.");
            }
        }

        /// <summary>
        /// Flush current byte from buffer
        /// </summary>
        private void WriteBuffer() {
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

        private byte CalculateChunk(byte max) {
            return (byte)Math.Min(max, 8 - BufferPosition);
        }

        protected virtual void Dispose(bool disposing) {
            if (IsDisposed) {
                return;
            }
            IsDisposed = true;

            if (disposing) {
                // Flush buffer
                WriteBuffer();

                // Dispose managed state (managed objects).
            }
        }

        public void Dispose() {
            Dispose(true);
        }
    }



}
