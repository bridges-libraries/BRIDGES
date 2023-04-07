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
            List<Point> points = new List<Point>
            {
                //points.Add(new Point(0.0, 0.0, 0.0));
                //points.Add(new Point(3.0, 0.0, 0.0));
                //points.Add(new Point(2.0, 7.0, 0.0));
                //points.Add(new Point(0.0, 1.0, 0.0));

                //Point pointO = new Point(1.0, 1.5, 6);

               new Point(40.18845,37.668301,0),
new Point(21.769007,34.606365,0),
new Point(29.82111,57.56171,0),
new Point(25.025793,45.277184,11.96329),
new Point(4.629511,52.466801,0),



        };


            Dictionary<Point, bool> IsSupport = new Dictionary<Point, bool>();
            foreach (Point point in points)
            {
                if (point.Z == 0.0) IsSupport.Add(point, true);
                else IsSupport.Add(point, false);
            }

            //The force F applied to all free nodes
            Euc3D.Vector F = new Euc3D.Vector(0.0, 0.0, -1.0);


            Dictionary<int, int> ConnectivityMatrix = new Dictionary<int, int>
            {
                { 1, 0 },
                { -1, 3 },
                { 2, 1 },
                { -2, 3 },
                { 3, 2 },
                { -3, 3 },
                { 4, 3 },
                { -4, 4 }
            };




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
            double fdInit = -1;

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

            double tolerance = 0.0000000001;
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

                        NodeEquilibrium energyType = new NodeEquilibrium(point.GetCoordinates()[i], adjNodesCoord, F.GetCoordinates()[i], adjNodesIndex.Count);

                        List<(VariableSet, int)> variables = new List<(VariableSet, int)> { };
                        foreach (int m in adjFDIndex) { variables.Add((forceDensities, m)); }

                        gpa.AddEnergy(energyType, variables);

                        
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
            #endregion



        }
    }
}
/// <summary>
/// Energy enforcing the equilibrium of node O.
/// </summary>
internal class NodeEquilibrium : IEnergyType
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
    public NodeEquilibrium(double node0, double[] adjNodes, double extForce, int count)
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

