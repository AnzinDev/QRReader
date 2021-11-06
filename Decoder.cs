﻿using System;
using System.Collections.Generic;
using System.Drawing;
using AForge.Video.DirectShow;
using AForge.Video;
using ZXing;

namespace QR
{
    class Decoder
    {
        VideoCaptureDevice VideoCaptureDevice { get; set; }
        FilterInfoCollection FilterInfoCollection { get; set; }
        BarcodeReader BarcodeReader { get; set; }
        string text { get; set; }

        public void InitializeReader(BarcodeFormat format)
        {
            BarcodeReader = new BarcodeReader();
            BarcodeReader.Options.PossibleFormats = new List<BarcodeFormat>();
            BarcodeReader.Options.PossibleFormats.Add(format);
        }

        public string DecodeFromImage(Bitmap bitmap)
        {
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
            text = DecodeFromImage(frame);
            GetResult();
        }

        public void GetResult()
        {
            Console.WriteLine(text);
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
