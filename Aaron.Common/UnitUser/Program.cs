﻿using Aaron.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitUser
{
    class Program
    {

        static void Main(string[] args)
        {

            string path = @"D:\认知方法论-音频\bf0414.mp3";
            TagLib.File f = TagLib.File.Create(path);
            f.Tag.Album = "认知方法论";

            f.Save();
            Console.ReadLine();



        }
    }
}
