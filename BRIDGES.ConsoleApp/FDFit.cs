using BRIDGES.Solvers.GuidedProjection;
using BRIDGES.Geometry.Euclidean3D;
using System;
using System.Collections.Generic;

using BRIDGES.Solvers.GuidedProjection.Interfaces;
using Euc3D = BRIDGES.Geometry.Euclidean3D;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.Solvers.GuidedProjection.QuadraticConstraintTypes;

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
            new Point(11.298875,16.220161,0),
new Point(20.576656,13.530453,0),
new Point(31.913134,11.695838,0),
new Point(10.026275,23.832838,0),
new Point(21.745256,22.255203,5.221012),
new Point(37.241184,20.677567,4.381117),
new Point(13.042107,30.658022,0),
new Point(24.186035,30.979952,0),
new Point(37.559697,29.659296,0),

        };

            List<Point> initialPoints = new List<Point>();
            foreach (Point p in points) initialPoints.Add(new Point(p.X, p.Y, p.Z));

            Dictionary<Point, bool> IsSupport = new Dictionary<Point, bool>();
            foreach (Point point in points)
            {
                if (point.Z == 0.0) IsSupport.Add(point, true);
                else IsSupport.Add(point, false);
            }

            //The force F applied to all free nodes
            List<Euc3D.Vector> F = new List<Euc3D.Vector>
            {
new Euc3D.Vector(0,0,-30.657775),
new Euc3D.Vector(0,0,-51.107507),
new Euc3D.Vector(0,0,-44.023815),
new Euc3D.Vector(0,0,-35.561527),
new Euc3D.Vector(0,0,-128.576385),
new Euc3D.Vector(0,0,-67.812302),
new Euc3D.Vector(0,0,-33.074499),
new Euc3D.Vector(0,0,-63.981667),
new Euc3D.Vector(0,0,-33.537442),
            };


            Dictionary<int, int> ConnectivityMatrix = new Dictionary<int, int>
            {
              {1,1},
{-1,4},
{2,3},
{-2,4},
{3,4},
{-3,5},
{4,4},
{-4,7},
            };




            int Nfd = ConnectivityMatrix.Count / 2;
            double fdInit = -1;

            double[] qList = new double[Nfd];
            for (int i = 0; i < Nfd; i++) { qList[i] = fdInit; }

            #endregion

            #region Declaration of the Solver

            double tolerance = 0.00000001;
            int maxIter = 100;  // Useless as long as GPA is neing debuged
            GuidedProjectionAlgorithm gpa = new GuidedProjectionAlgorithm(tolerance, maxIter);

            #endregion

            #region Set Variables
            int fdCount = Nfd;

            /********** Force Densities **********/
            int vertexDimension = 1;
            VariableSet forceDensities = gpa.AddVariableSet(vertexDimension, fdCount);
            for (int i = 0; i < fdCount; i++) { forceDensities.AddVariable(qList[i]); }


            /********** Dummy variable (for UpperBound Constraint) **********/
            VariableSet dummy = gpa.AddVariableSet(1, fdCount);
            for (int i = 0; i < fdCount; i++) { dummy.AddVariable(1.0); }

            #endregion

            #region Set Energy

            UpperFDBound upperBound = new UpperFDBound(-8);
            for (int j = 0; j < fdCount; j++)
            {
                List<(VariableSet, int)> variables = new List<(VariableSet, int)>{
                                (forceDensities, j), (dummy, j)
                            };

                gpa.AddConstraint(upperBound, variables, 1000.0);
            }

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

                        NodeEquilibriumFD energyType = new NodeEquilibriumFD(point.GetCoordinates()[i], adjNodesCoord, F[segmentCounter].GetCoordinates()[i], adjNodesIndex.Count);

                        List<(VariableSet, int)> variables = new List<(VariableSet, int)> { };
                        foreach (int m in adjFDIndex) { variables.Add((forceDensities, m)); }

                        gpa.AddEnergy(energyType, variables,0.01);


                    }
                }
                segmentCounter++;
            }

            #endregion

            #region Launch

            gpa.InitialiseX();

            for (int j = 0; j < 2*Nfd; j++)
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

            for (int j = 0; j < 2*Nfd; j++)
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
internal class NodeEquilibriumFD : IEnergyType
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
    public NodeEquilibriumFD(double node0, double[] adjNodes, double extForce, int count)
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

