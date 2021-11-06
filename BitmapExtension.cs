using System.Drawing;
using System.Drawing.Imaging;

namespace QR
{
    public static class BitmapExtension
    {
        public static unsafe byte[] BitmapToOtsuBytes(this Bitmap bmp)
        {
            const int depth = 256;
            int height = bmp.Height,
                width  = bmp.Width;
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            int size = height * width;
            byte[] grayImage = new byte[size];

            try
            {
                byte* curr;
                int stride = bd.Stride;
                //covert from full-color byte depth RGB image to byte depth GRAY image
                for (int j = 0; j < height; j++)
                {
                    curr = ((byte*)bd.Scan0) + j * stride;
                    for (int i = 0; i < width; i++)
                    {
                        grayImage[width * j + i] = (byte)((*(curr++) + *(curr++) + *(curr++)) / 3);
                    }
                }

                //creating an image histogram and sum image intensity
                int* hist = stackalloc int[depth];
                long fullIntensity = 0;

                for (int i = 0; i < size; ++i)
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
                    grayImage[i] = (grayImage[i] >= (byte)threshold) ? (byte)255 : (byte)0;
                }
            }
            finally
            {
                bmp.UnlockBits(bd);
            }
            return grayImage;
        }
    }
}
