using System.Configuration;
using System.Collections.Specialized;

namespace QR
{
    class Program
    {
        static void Main(string[] args)
        {
            Decoder decoder = new Decoder();
            decoder.InitializeCamera(0);
            decoder.InitializeReader(ZXing.BarcodeFormat.QR_CODE);
            decoder.StartCapture();
        }
    }
}
