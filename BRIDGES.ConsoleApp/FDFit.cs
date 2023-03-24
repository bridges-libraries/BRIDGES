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
            //points.Add(new Point(0.0, 0.0, 0.0));
            //points.Add(new Point(3.0, 0.0, 0.0));
            //points.Add(new Point(2.0, 7.0, 0.0));
            //points.Add(new Point(0.0, 1.0, 0.0));

            //Point pointO = new Point(1.0, 1.5, 6);

            points.Add(new Point(-15.096293, 10.52492, 5.787252));
            points.Add(new Point(-5.293628, 11.367286, 5.787252));
            points.Add(new Point(-9.46545, 2.456744, 5.787252));
            points.Add(new Point(-10.438131, 13.775888, 7.234065));
            points.Add(new Point(-17.668545, 11.72922, 0.0));
            points.Add(new Point(-14.609953, 4.865347, 7.234065));
            points.Add(new Point(-4.807287, 5.707713, 7.234065));
            points.Add(new Point(-2.964546, 12.99277, 0.0));
            points.Add(new Point(-9.22228, -0.373042, 0.0));
            points.Add(new Point(-11.410811, 25.095032, 0));
            points.Add(new Point(-9.951791, 8.116316, 7.716336));
            points.Add(new Point(-23.926279, -1.636592, 0.0));
            points.Add(new Point(5.48172, 0.890508, 0.0));


            Dictionary<Point, bool> IsSupport = new Dictionary<Point, bool>();
            foreach (Point point in points)
            {
                if (point.Z == 0.0) IsSupport.Add(point, true);
                else IsSupport.Add(point, false);
            }


            //The force F applied to all free nodes
            Euc3D.Vector F = new Euc3D.Vector(0.0, 0.0, -10);


            Dictionary<int, int> ConnectivityMatrix = new Dictionary<int, int>();
            ConnectivityMatrix.Add(1, 0);
            ConnectivityMatrix.Add(-1, 3);
            ConnectivityMatrix.Add(2, 0);
            ConnectivityMatrix.Add(-2, 4);
            ConnectivityMatrix.Add(3, 0);
            ConnectivityMatrix.Add(-3, 5);
            ConnectivityMatrix.Add(4, 1);
            ConnectivityMatrix.Add(-4, 3);
            ConnectivityMatrix.Add(5, 1);
            ConnectivityMatrix.Add(-5, 6);
            ConnectivityMatrix.Add(6, 1);
            ConnectivityMatrix.Add(-6, 7);
            ConnectivityMatrix.Add(7, 2);
            ConnectivityMatrix.Add(-7, 5);
            ConnectivityMatrix.Add(8, 2);
            ConnectivityMatrix.Add(-8, 6);
            ConnectivityMatrix.Add(9, 2);
            ConnectivityMatrix.Add(-9, 8);
            ConnectivityMatrix.Add(10, 3);
            ConnectivityMatrix.Add(-10, 9);
            ConnectivityMatrix.Add(11, 3);
            ConnectivityMatrix.Add(-11, 10);
            ConnectivityMatrix.Add(12, 5);
            ConnectivityMatrix.Add(-12, 10);
            ConnectivityMatrix.Add(13, 5);
            ConnectivityMatrix.Add(-13, 11);
            ConnectivityMatrix.Add(14, 6);
            ConnectivityMatrix.Add(-14, 10);
            ConnectivityMatrix.Add(15, 6);
            ConnectivityMatrix.Add(-15, 12);




            //List<Particle> particles = new List<Particle>
            //{
            //    new Particle(pointA, Euc3D.Vector.Zero, true, 0),
            //    new Particle(pointB, Euc3D.Vector.Zero, true, 1),
            //    new Particle(pointC, Euc3D.Vector.Zero, true, 2),
            //    new Particle(pointD, Euc3D.Vector.Zero, true, 3),
            //    new Particle(pointO, F, false, 4)
            //};

            //Definie the force densities of each elements

            int Nfd = ConnectivityMatrix.Count / 2;
            double fdInit = -10;

            double[] qList = new double[Nfd];
            for (int i = 0; i < Nfd; i++) { qList[i] = fdInit; }

            //Create the elements
            //List<ForceDensity> elements = new List<ForceDensity>
            //{
            //    new ForceDensity(0,4,qList[0],1),
            //    new ForceDensity(1,4,qList[1],1),
            //    new ForceDensity(2,4,qList[2],1),
            //    new ForceDensity(3,4,qList[3],1)
            //};

            // Model myModel = new Model(particles, elements);
            #endregion

            #region Declaration of the Solver

            double tolerance = 0.1;
            int maxIter = 1;  // Useless as long as GPA is neing debuged
            GuidedProjectionAlgorithm gpa = new GuidedProjectionAlgorithm(tolerance, maxIter);

            #endregion

            #region Set Variables
            int fdCount = Nfd;

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
            foreach (Point point in points)
            {
                if (!IsSupport[point])
                {
                    List<int> adjNodesIndex = new List<int>();
                    List<int> adjFDIndex = new List<int>();
                    for (int j = 1; j < Nfd + 1; j++)
                    {
                        if (ConnectivityMatrix[j] == segmentCounter)
                        {
                            adjNodesIndex.Add(ConnectivityMatrix[-j]);
                            adjFDIndex.Add(j - 1);
                        }

                        if (ConnectivityMatrix[-j] == segmentCounter)
                        {
                            adjNodesIndex.Add(ConnectivityMatrix[j]);
                            adjFDIndex.Add(j - 1);
                        }

                    }

                    for (int i = 0; i < 3; i++)
                    {
                        double[] adjNodesCoord = new double[adjFDIndex.Count];
                        for (int k = 0; k < adjNodesIndex.Count; k++)
                        {
                            adjNodesCoord[k] = points[adjNodesIndex[k]].GetCoordinates()[i];
                        }

                        Equilibrium energyType = new Equilibrium(point.GetCoordinates()[i], adjNodesCoord, F.GetCoordinates()[i], adjNodesIndex.Count);

                        List<(VariableSet, int)> variables = new List<(VariableSet, int)> { };
                        foreach (int m in adjFDIndex) { variables.Add((forceDensities, m)); }

                        gpa.AddEnergy(energyType, variables, 1);
                    }
                }
                segmentCounter++;
            }



            #endregion

            #region Launch

            gpa.InitialiseX();

            for (int j = 0; j < Nfd; j++)
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

            for (int j = 0; j < Nfd; j++)
            {
                System.Console.Write(String.Format("{0:0.00000} ", gpa.X[j]));
                System.Console.WriteLine();
            }

            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();


            System.Console.WriteLine();
            System.Console.WriteLine("-----------FDFit---------");
            System.Console.WriteLine("Initial particle position:");
            // System.Console.WriteLine("X =" + pointO.X.ToString());
            //System.Console.WriteLine("Y =" + pointO.Y.ToString());
            //System.Console.WriteLine("Z =" + pointO.Z.ToString());
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

