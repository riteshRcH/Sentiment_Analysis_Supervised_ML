﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string s in "brave".Split(' '))
                Console.Write(s);
            Console.Read();
        }
    }
}
