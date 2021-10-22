namespace QR
{
    class Program
    {
        static void Main(string[] args)
        {
            Decoder decoder = new Decoder(0);
            decoder.StartCapture();
        }
    }
}
