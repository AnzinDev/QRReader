﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR
{
    class Program
    {
        static void Main(string[] args)
        {
            Decoder decoder = new Decoder(0);
            decoder.StartCapture();
        }
    }
}
