using NUnit.Framework;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QR.Test
{
    [TestFixture]
    class TestClass
    {
        [Test]
        public void Test1()
        {
            Decoder decoder = new Decoder();

            var images = UploadImages(@"D:\VS Solutions\QR\bin\Debug\fortest");

            string[] expected = new string[images.Length];
            string[] actual = new string[images.Length];

            for (int i = 0; i < expected.Length; i++)
            {
                expected[i] = "test";
            }

            for (int i = 0; i < images.Length; i++)
            {
                actual[i] = decoder.DecodeImages(images[i])[0];
                Console.WriteLine(actual[i]);
            }

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        public Bitmap[] UploadImages(string absoluteFolderPath)
        {
            List<Bitmap> images = new List<Bitmap>();
            try
            {
                DirectoryInfo directory = new DirectoryInfo(absoluteFolderPath);
                if (directory.Exists)
                {
                    var files = directory.GetFiles();
                    for (int i = 0; i < files.Length; i++)
                    {
                        images.Add(new Bitmap(files[i].FullName));
                    }
                }
                else
                {
                    throw new Exception("No such folder");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return images.ToArray();
        }
    }
}
