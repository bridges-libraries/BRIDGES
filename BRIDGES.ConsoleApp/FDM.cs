using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using BRIDGES.DiscreteElementModelling.Model;
using BRIDGES.Geometry.Euclidean3D;
using ENPC.Numerics.DiscreteElementModelling.Solvers;

namespace BRIDGES.ConsoleApp
{
    public static class FDMWorkingExample
    {
        public static void FDMMWE()
        {
            //The nodes of the model
            Point pointA = new Point(0.0, 0.0, 0.0);
            Point pointB = new Point(3.0, 0.0, 0.0);
            Point pointC = new Point(2.0, 7.0, 0.0);
            Point pointD = new Point(0.0, 1.0, 0.0);

            Point pointO = new Point(1.0, 1.5, 6);

            //The force F applied to the free node O
            Vector F = new Vector(0.0, 0.0, -50.0);


            List<Particle> particles = new List<Particle>
            {
                new Particle(pointA, Vector.Zero, true, 0),
                new Particle(pointB, Vector.Zero, true, 1),
                new Particle(pointC, Vector.Zero, true, 2),
                new Particle(pointD, Vector.Zero, true, 3),
                new Particle(pointO, F, false, 4)
            };

            //Definie the force densities of each elements
            double q0 = -4.96;
            double q1 = -1.587;
            double q2 = -1.7857;
            double q3 = -0;

            //Create the elements
            List<ForceDensity> elements = new List<ForceDensity>
            {
                new ForceDensity(0,4,q0,1),
                new ForceDensity(1,4,q1,1),
                new ForceDensity(2,4,q2,1),
                new ForceDensity(3,4,q3,1)
            };

            Model myModel = new Model(particles, elements);

           // FDM fdmSolver = new FDM(particles, elements);
           //// OptimiseLBFGS fitFD = new OptimiseLBFGS(myModel);
           //fdmSolver.FDMSolve();
           // //fitFD.Optimise();
           // System.Console.WriteLine();
           // System.Console.WriteLine("Result =" + .Result.ToString());
           // System.Console.WriteLine("Exit =" + fitFD.Exit.ToString());

           // System.Console.WriteLine("Steps =" + fitFD.Steps.ToString());
           // System.Console.WriteLine("FD =");

           // foreach (double d in fitFD.Optimal_FD)
           // {
           //     System.Console.WriteLine("FD =" + d.ToString());
           // }
           // myModel.UpdateForceDensities(fitFD.Optimal_FD.ToList());

            //Initialise the solver and solve
            ForceDensityMethod fdmSolver = new ForceDensityMethod(particles, elements); //Todo: add a setting for the prefered choice of decomposition
            fdmSolver.InitialiseMatrices(myModel.Elements.Select(i => (ForceDensity)i).ToList());

            fdmSolver.FDMSolve();
            
            //FormFitFDM fit = new FormFitFDM(myModel, -4.5, 0);
            //fit.Optimise();

            System.Console.WriteLine();
            System.Console.WriteLine("-----------FDM---------");
            System.Console.WriteLine("Final particle position:");
            System.Console.WriteLine("X =" + particles.Last().Position.Current.X.ToString());
            System.Console.WriteLine("Y =" + particles.Last().Position.Current.Y.ToString());
            System.Console.WriteLine("Z =" + particles.Last().Position.Current.Z.ToString());
            System.Console.WriteLine();
          //  System.Console.WriteLine("Result =" + fit.Result.ToString());
          //  System.Console.WriteLine("Exit =" + fit.Exit.ToString());

        //    System.Console.WriteLine("Steps =" + fit.Steps.ToString());
          //  System.Console.WriteLine("FD =" + elements.Last().ForceDensityValue);

            //foreach (double d in fit.Optimal_FD)
            //{
            //    System.Console.WriteLine("FD =" + d.ToString());
            //}

        }

    }
}
