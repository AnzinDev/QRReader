using NUnit.Framework;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace QR.Test
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void Test1()
        {
            Decoder decoder = new Decoder();

            var images = UploadImages("fortest");

            string[] expected = new string[images.Length];
            string[] actual = new string[images.Length];

            for (int i = 0; i < expected.Length; i++)
            {
                expected[i] = "test";
            }

            for (int i = 0; i < images.Length; i++)
            {
                actual[i] = decoder.DecodeFromImage(images[i]);
                Console.WriteLine(actual[i]);
            }

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        public Bitmap[] UploadImages(string folderPath)
        {
            List<Bitmap> images = new List<Bitmap>();
            try
            {
                DirectoryInfo directory = new DirectoryInfo(Path.Combine(Assembly.GetExecutingAssembly().Location, folderPath));
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
