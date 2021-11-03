using System;
using System.Collections.Generic;
using System.Drawing;
using AForge.Video.DirectShow;
using AForge.Video;
using ZXing;
using System.Drawing.Imaging;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Threading;

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

        public string DecodeFromImage(Bitmap bitmap)
        {
            bitmap.Save(@"D:/test/1.jpg");
            int height = bitmap.Height;
            int width = bitmap.Width;

            var result = BarcodeReader.Decode(bitmap.BitmapToOtsuBytes(), width, height, RGBLuminanceSource.BitmapFormat.Unknown);
            
            if (result != null)
            {
                return result.Text;
            }
            else
            {
                return default;
            }
        }

        public void InitializeCamera(int cameraNum)
        {
            FilterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            VideoCaptureDevice = new VideoCaptureDevice();
            VideoCaptureDevice.Source = FilterInfoCollection[cameraNum].MonikerString;
            VideoCaptureDevice.VideoResolution = VideoCaptureDevice.VideoCapabilities[5];
            VideoCaptureDevice.NewFrame += NewFrame;
        }

        private void NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap frame = eventArgs.Frame.Clone() as Bitmap;
            string result = DecodeFromImage(frame);
            Console.WriteLine(result);
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
