using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV;
using System;
using System.IO;

namespace QR
{
    public static class BitmapExtension
    {
        public static Bitmap OtsuThreshold(this Bitmap bitmap)
        {
            Image<Bgr, byte> input = bitmap.ToImage<Bgr, byte>();
            var grayImage = input.Convert<Gray, byte>();
            CvInvoke.Threshold(grayImage, grayImage, 0, 255, Emgu.CV.CvEnum.ThresholdType.Otsu);
            if (Properties.Settings.Default.enableOtsuProcessedImagesSaving)
            {
                string programFolder = "QR Reader";
                string fullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), programFolder);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                grayImage.Save(fullPath + $"\\{DateTime.Now.ToString("dd-MM-yyyy_HH.mm.ss")}.jpg");
            }
            return grayImage.ToBitmap();
        }
    }
}
