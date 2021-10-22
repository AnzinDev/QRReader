using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using AForge.Video.DirectShow;
using AForge.Video;
using ZXing;
using ZXing.Common;

namespace QR
{
    class Decoder
    {
        VideoCaptureDevice VideoCaptureDevice { get; set; }
        FilterInfoCollection FilterInfoCollection { get; set; }
        BarcodeReader BarcodeReader { get; set; }

        public Decoder(int cameraNum)
        {
            try
            {
                //initialize and setup QR-code reader
                BarcodeReader = new BarcodeReader();
                BarcodeReader.Options.PossibleFormats = new List<BarcodeFormat>();
                BarcodeReader.Options.PossibleFormats.Add(BarcodeFormat.QR_CODE);
                //initialize camera capture
                FilterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                VideoCaptureDevice = new VideoCaptureDevice();
                VideoCaptureDevice.Source = FilterInfoCollection[cameraNum].MonikerString;
                VideoCaptureDevice.NewFrame += NewFrame;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            var frame = eventArgs.Frame.Clone() as Bitmap;

            var result = BarcodeReader.Decode(frame);

            if (result != null)
            {
                Console.WriteLine(result.Text);
            }
        }

        public void StartCapture()
        {
            try
            {
                Console.WriteLine("Press Space to start");
                Console.Read();
                VideoCaptureDevice.Start();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
    }
}
