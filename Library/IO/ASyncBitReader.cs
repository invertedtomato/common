using System;

namespace InvertedTomato.IO {
    public class ASyncBitReader {
        /// <summary>
        /// The method to callback when we have reached the desired number of bits. Also returns number of bits to fetch next.
        /// </summary>
        private readonly Func<ulong, int> Output;

        /// <summary>
        /// Buffer of current bits.
        /// </summary>
        private ulong Buffer = 0;

        /// <summary>
        /// Number of bits currently in the buffer.
        /// </summary>
        private int Level = 0;

        /// <summary>
        /// Number of bits wanted by the receiver.
        /// </summary>
        private int BitsWanted;

        public ASyncBitReader(Func<ulong, int> output) {
            if (null == output) {
                throw new ArgumentNullException("callback");
            }

            // Store
            Output = output;

            // Seed callback
            CallbackWrap(0);
        }

        /// <summary>
        /// Inject a number of bytes.
        /// </summary>
        /// <param name="buffer"></param>
        public void Insert(byte[] buffer) {
            if (null == buffer) {
                throw new ArgumentNullException("buffer");
            }

            // For each byte
            foreach (var b in buffer) {
                // Check for buffer overflow
                if (Level + 8 > 64) { // TODO: Is this an issue?
                    throw new OverflowException("Max of 64 bits can fit in buffer. Attempted to exceed by " + (64 - Level + 8) + " bits.");
                }

                // Load byte onto buffer
                Buffer = (Buffer << 8) | b;
                Level += 8;

                while (Level >= BitsWanted) {
                    // Right align value
                    var value = Buffer >> Level - BitsWanted;

                    // Remove unwanted prefix bits
                    value &= ulong.MaxValue >> 64 - BitsWanted;

                    // Reduce buffer usage counter
                    Level -= BitsWanted;

                    // Callback value
                    CallbackWrap(value);
                }
            }
        }

        /// <summary>
        /// Flush remainder of current byte.
        /// </summary>
        public void FlushByte() {
            Level -= Level % 8;
        }

        private void CallbackWrap(ulong value) {
            // Return value
            BitsWanted = Output(value);

            // Check sane number of bits requested next
            if (BitsWanted < 0 || BitsWanted > 64) {
                throw new ArgumentOutOfRangeException("BitsWanted must be between 1 and 64, not " + BitsWanted + ".");
            }
        }
    }
    /*
    public class ASyncFibonacciUnsignedReader {
        private readonly Action<ulong> Output;
        private ulong Buffer;
        private int FibPosition;
        private bool LastBit;

        public ASyncFibonacciUnsignedReader(Action<ulong> output) {
            if (null == output) {
                throw new ArgumentNullException("callback");
            }

            // Store
            Output = output;
        }

        public int Input(ulong buffer) {
            if (buffer > 0) {
                // If double 1 bits
                if (LastBit) {
                    // Output value
                    Output(Buffer);

                    // Reset for next value
                    FibPosition = 0;
                    LastBit = false;
                } else {
                    // Add value to buffer
                    Buffer += Fibonacci.Value[FibPosition];

                    // Note bit for next cycle
                    LastBit = true;
                }
            } else {
                LastBit = false;
            }

            FibPosition++;

            return 1;
        }
    }*/
}
