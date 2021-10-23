using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV;

namespace QR
{
    public static class BitmapExtension
    {
        public static Bitmap OtsuThreshold(this Bitmap bitmap)
        {
            Image<Bgr, byte> input = bitmap.ToImage<Bgr, byte>();
            var grayImage = input.Convert<Gray, byte>();
            CvInvoke.Threshold(grayImage, grayImage, 0, 255, Emgu.CV.CvEnum.ThresholdType.Otsu);
            return grayImage.ToBitmap();
        }
    }
}
