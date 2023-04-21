
using BRIDGES.ConsoleApp;
using System;
using System.Collections.Generic;

namespace BRIDGES.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();

            CloseFit.CloseFitWE();
            //DualContour.CoreFunction();

            stopwatch.Stop();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Total time (in ms) : {stopwatch.ElapsedMilliseconds}");

          //  FDMWorkingExample.FDMMWE();

            //Console.WriteLine("Hello World!");
        }
    }
}
