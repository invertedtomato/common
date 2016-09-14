using InvertedTomato.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvertedTomato.Common.LoadTest {
    class Program {
        static void Main(string[] args) {
            var stopWatch = Stopwatch.StartNew();

            for (ulong i = 0; i < 10000000; i++) {
                using (var stream = new MemoryStream()) {
                    using (var writer = new BitWriter(stream)) {
                        writer.Write(i, 10);
                    }
                }
            }

            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds + "ms");
            Console.ReadKey(true);
            //2400ms
        }
    }
}
