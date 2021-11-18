namespace QR
{
    class Program
    {
        static void Main()
        {
            Decoder decoder = new Decoder();
            decoder.InitializeCamera(0);
            decoder.InitializeReader(ZXing.BarcodeFormat.QR_CODE);
            decoder.StartCapture();
        }
    }
}
