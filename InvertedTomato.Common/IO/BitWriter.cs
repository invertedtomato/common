using System;
using System.IO;

namespace InvertedTomato.IO {
    public class BitWriter : IDisposable {

        public bool IsDisposed { get; private set; }
        private readonly Stream Output;

        public BitWriter(Stream output) {
            if (null == output) {
                throw new ArgumentNullException("input");
            }

            Output = output;
        }

        public void Write1(bool value) { throw new NotImplementedException(); }
        public void Write8(byte buffer, int length) { throw new NotImplementedException(); }
        public void Write64(ulong buffer, int length) { throw new NotImplementedException(); }

        
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
