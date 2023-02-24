using BRIDGES.DiscreteElementModelling.Model;
using BRIDGES.Solvers.GuidedProjection;
using BRIDGES.Solvers.GuidedProjection.EnergyTypes;
using BRIDGES.Solvers.GuidedProjection.QuadraticConstraintTypes;
using BRIDGES.Geometry.Euclidean3D;
using MathNet.Numerics.LinearAlgebra.Solvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BRIDGES.Solvers.GuidedProjection;
using BRIDGES.Solvers.GuidedProjection.Interfaces;
using BRIDGES.Solvers.GuidedProjection.EnergyTypes;
using BRIDGES.Solvers.GuidedProjection.QuadraticConstraintTypes;
using Euc3D = BRIDGES.Geometry.Euclidean3D;
using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.ConsoleApp.Examples.GuidedProjection;

namespace BRIDGES.ConsoleApp
{
    internal class FDFit
    {
        internal static void FDFitWE()
        {
            #region Geometry
            //The nodes of the model
            List<Point> points = new List<Point>();
            points.Add(new Point(0.0, 0.0, 0.0));
            points.Add(new Point(2.0, 0.0, 0.0));
            points.Add(new Point(2.0, 2.0, 0.0));
            points.Add(new Point(0.0, 2.0, 0.0));

            Point pointO = new Point(1.0, 1.5, 5);

            //The force F applied to the free node O
            Euc3D.Vector F = new Euc3D.Vector(0.0, 0.0, -70.0);


            //List<Particle> particles = new List<Particle>
            //{
            //    new Particle(pointA, Euc3D.Vector.Zero, true, 0),
            //    new Particle(pointB, Euc3D.Vector.Zero, true, 1),
            //    new Particle(pointC, Euc3D.Vector.Zero, true, 2),
            //    new Particle(pointD, Euc3D.Vector.Zero, true, 3),
            //    new Particle(pointO, F, false, 4)
            //};

            //Definie the force densities of each elements
            double[] qList = new double[4];
            qList[0] = -4;
            qList[1] = -1;
            qList[2] = -2;
            qList[3] = -4.68;

            //Create the elements
            List<ForceDensity> elements = new List<ForceDensity>
            {
                new ForceDensity(0,4,qList[0],1),
                new ForceDensity(1,4,qList[1],1),
                new ForceDensity(2,4,qList[2],1),
                new ForceDensity(3,4,qList[3],1)
            };

            // Model myModel = new Model(particles, elements);
            #endregion

            #region Declaration of the Solver

            double tolerance = 0.00001;
            int maxIter = 1000;  // Useless as long as GPA is neing debuged
            GuidedProjectionAlgorithm gpa = new GuidedProjectionAlgorithm(tolerance, maxIter);

            #endregion

            #region Set Variables
            int fdCount = 4;
            /********** Force Densities **********/

            int vertexDimension = 1;
            VariableSet forceDensities = gpa.AddVariableSet(vertexDimension, fdCount);
            for (int i = 0; i < fdCount; i++) { forceDensities.AddVariable(qList[i]); }


            /********** Dummy variable (for LowerBound Constraint) **********/

            //VariableSet dummy = gpa.AddVariableSet(1, fdCount);
            //for (int i = 0; i < fdCount; i++) { dummy.AddVariable(1.0); }

            #endregion

            #region Set Energy

            /********** Target Equilibrium **********/

            int segmentCounter = 0;
            for (int i = 0; i < 3; i++)
            {
                double[] adjNodesCoord = new double[4];
                for (int j = 0; j < 4; j++)
                {
                    adjNodesCoord[j] = points[j].GetCoordinates()[i];
                }

                Equilibrium energyType = new Equilibrium(pointO.GetCoordinates()[i], adjNodesCoord, F.GetCoordinates()[i], 4);

                List<(VariableSet, int)> variables = new List<(VariableSet, int)> {
                        (forceDensities, segmentCounter), (forceDensities, segmentCounter + 1), (forceDensities, segmentCounter+2), (forceDensities, segmentCounter+3)};

                gpa.AddEnergy(energyType, variables, 100.0);
            
            }




            #endregion

            #region Launch

            gpa.InitialiseX();

            for (int j = 0; j <  4; j++)
            {
                System.Console.Write(String.Format("{0:0.00000} ", gpa.X[j]));
            }

            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();

            for (int i = 0; i < gpa.MaxIteration; i++)
            {
                gpa.RunIteration(false);
            }

            for (int j = 0; j < 4; j++)
            {
                System.Console.Write(String.Format("{0:0.00000} ", gpa.X[j]));
            }

            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();

            for (int j = 0; j < 4; j++)
            {
                System.Console.Write(String.Format("{0:0.00000} ", gpa.X[j]));
            }

            System.Console.WriteLine();
            System.Console.WriteLine("-----------FDFit---------");
            System.Console.WriteLine("Initial particle position:");
            System.Console.WriteLine("X =" + pointO.X.ToString());
            System.Console.WriteLine("Y =" + pointO.Y.ToString());
            System.Console.WriteLine("Z =" + pointO.Z.ToString());
            System.Console.WriteLine();

            #endregion



        }
    }
}
/// <summary>
/// Energy enforcing the equilibrium of node O.
/// </summary>
internal class Equilibrium : IEnergyType
{
    #region Properties

    /// <inheritdoc cref="IEnergyType.LocalKi"/>
    public SparseVector LocalKi { get; }

    /// <inheritdoc cref="IEnergyType.Si"/>
    public double Si { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initialises a new instance of the <see cref="Equilibrium"/> class.
    /// </summary>
    /// <param name="extForce"> External force applied to node O. </param>
    /// <param name="count"> Number of scalar value to sum. </param>
    public Equilibrium(double node0, double[] adjNodes, double extForce, int count)
    {
        /******************** Define LocalKi ********************/

        LocalKi = new SparseVector(count);

        for (int i = 0; i < count; i++)
        { LocalKi[i] = node0 - adjNodes[i]; }

        /******************** Define Si ********************/

        Si = extForce;
    }

    #endregion
}

