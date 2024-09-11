
using System.Runtime.InteropServices;
namespace Gerbvsharp {
    public static class App {
        public static int Main() {
            var file = Gerbv.gerb_fopen(@"E:\Test.txt");
            int readLen = 0;
            //int result = GerberFile.gerb_fgetint(file, out readLen);
            //double result = GerberFile.gerb_fgetdouble(file);
            var result = Gerbv.gerb_fgetstring(file, '2');
            Console.WriteLine("Done");
            return 0;
        }
    }
}