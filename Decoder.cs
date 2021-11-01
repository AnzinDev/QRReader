using System;
using System.Collections.Generic;
using System.Drawing;
using AForge.Video.DirectShow;
using AForge.Video;
using ZXing;
using System.Drawing.Imaging;

namespace QR
{
    class Decoder
    {
        VideoCaptureDevice VideoCaptureDevice { get; set; }
        FilterInfoCollection FilterInfoCollection { get; set; }
        BarcodeReader BarcodeReader { get; set; }

        public void InitializeReader(BarcodeFormat format)
        {
            BarcodeReader = new BarcodeReader();
            BarcodeReader.Options.PossibleFormats = new List<BarcodeFormat>();
            BarcodeReader.Options.PossibleFormats.Add(format);
        }

        public string[] DecodeImages(Bitmap bitmap)
        {
            var results = BarcodeReader.DecodeMultiple(bitmap);
            if (results != null)
            {
                var length = results.Length;
                string[] resultText = new string[length];
                for (int i = 0; i < length; i++)
                {
                    resultText[i] = results[i].Text;
                }

                return resultText;
            }
            else
            {
                return default;
            }
        }

        public void InitializeCamera(int cameraNum)
        {
            //initialize camera capture
            FilterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            VideoCaptureDevice = new VideoCaptureDevice();
            VideoCaptureDevice.Source = FilterInfoCollection[cameraNum].MonikerString;
            VideoCaptureDevice.NewFrame += NewFrame;
        }

        private void NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            var frame = eventArgs.Frame.Clone() as Bitmap;
            
            var results = DecodeImages(frame);
            foreach (var item in results)
            {
                Console.WriteLine(item);
            }
        }

        private unsafe byte[] BitmapToRawBytes(Bitmap bmp)
        {
            int height = bmp.Height, width = bmp.Width;
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            byte[] gray = new byte[height * width];
            try
            {
                
            }
            finally
            {
                    
            }
        }

        public void StartCapture()
        {
            try
            {
                VideoCaptureDevice.Start();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
    }
}
