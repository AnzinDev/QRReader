using System.Drawing;
using System.Drawing.Imaging;
using Emgu.CV.Structure;
using Emgu.CV;
using System;
using System.IO;
using System.ComponentModel;

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

        public static unsafe byte[] BitmapToOtsuBinarizedBytes(this Bitmap bmp)
        {
            const int depth = 256;
            int height = bmp.Height,
                width = bmp.Width;
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int size = height * width;
            byte[] grayImage = new byte[size];
            byte[] res = new byte[size * 3];
            try
            {
                //covert from full-color byte depth RGB image to byte depth GRAY image
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        var curr = ((byte*)bd.Scan0) + j * bd.Stride;
                        grayImage[height * j + i] = (byte)((*(curr++) + *(curr++) + *(curr++)) / 3);
                    }
                }

                //creating an image histogram and sum image intensity
                int* hist = stackalloc int[depth];
                int fullIntensity = 0;
                for (int i = 0; i < size; i++)
                {
                    ++hist[grayImage[i]];
                    fullIntensity += grayImage[i];
                }

                //Otsu method
                int threshold = 0;
                float maxSigma = 0;
                int fcpc = 0;
                int fcis = 0;

                for (int t = 0; t < depth - 1; ++t)
                {
                    fcpc += hist[t];
                    fcis += t * hist[t];

                    float fcp = fcpc / (float)size;
                    float scp = 1.0f - fcp;

                    float fcm = fcis / (float)fcpc;
                    float scm = (fullIntensity - fcis) / (float)(size - fcpc);
                    float delta = fcm - scm;

                    float sigma = fcp * scp * delta * delta;

                    if (sigma > maxSigma)
                    {
                        maxSigma = sigma;
                        threshold = t;
                    }
                }

                //binarization
                
                for (int i = 0; i < size; i++)
                {
                    byte pixel = grayImage[i] >= (byte)threshold ? (byte)255 : (byte)0;
                    res[i] = res[i + 1] = res[i + 2] = pixel;
                }
            }
            finally
            {
                bmp.UnlockBits(bd);
            }
            
            return res;
        }
    }
}
