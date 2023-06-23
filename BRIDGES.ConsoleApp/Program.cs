
using BRIDGES.ConsoleApp;
using System;
using System.Collections.Generic;
using BRIDGES.ConsoleApp.Examples.GuidedProjection;

namespace BRIDGES.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();

           // FDFit.FDFitWE();
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
