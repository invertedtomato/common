using System;
using System.IO;

namespace InvertedTomato.IO {
    public class BitReader : IDisposable {
        public bool IsDisposed { get; private set; }
        private readonly Stream Input;

        public BitReader(Stream input) {
            if (null == input) {
                throw new ArgumentNullException("input");
            }

            Input = input;
        }

        public bool Read1() { throw new NotImplementedException(); }
        public byte Read8(int length) { throw new NotImplementedException(); }
        public ulong Read64(int length) { throw new NotImplementedException(); }

        
        public bool Peak1() { throw new NotImplementedException(); }
        
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
