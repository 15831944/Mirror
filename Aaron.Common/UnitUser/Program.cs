﻿using Aaron.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitUser
{
    class Program
    {

        static void Main(string[] args)
        {

            string path = @"D:\CopyGitItem";
            FileHelper.DeleteDirectory(path);


            Console.ReadLine();
        }
    }
}
