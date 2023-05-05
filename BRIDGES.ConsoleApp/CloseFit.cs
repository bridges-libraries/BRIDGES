using BRIDGES.Solvers.GuidedProjection;
using BRIDGES.Geometry.Euclidean3D;
using System;
using System.Collections.Generic;

using BRIDGES.Solvers.GuidedProjection.Interfaces;
using Euc3D = BRIDGES.Geometry.Euclidean3D;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.LinearAlgebra.Matrices;
using BRIDGES.LinearAlgebra.Matrices.Sparse;
using BRIDGES.LinearAlgebra.Matrices.Storage;
using System.Linq;
using BRIDGES.Solvers.GuidedProjection.QuadraticConstraintTypes;

namespace BRIDGES.ConsoleApp
{
    internal class CloseFit
    {
        internal static void CloseFitWE()
        {
            #region Geometry
            //The nodes of the model
            List<Point> points = new List<Point>
            {
               new Point(11.298875,16.220161,0),
new Point(15.165754,14.554647,0),
new Point(20.576656,13.530453,0),
new Point(26.244895,12.613146,0),
new Point(31.913134,11.695838,0),
new Point(9.890564,19.70584,0),
new Point(15.429258,18.759251,1.158091),
new Point(21.160956,17.892828,3.077762),
new Point(27.399413,17.039765,5.264799),
new Point(34.577159,16.186703,2.190558),
new Point(10.026275,23.832838,0),
new Point(15.885766,23.04402,2.557757),
new Point(21.745256,22.255203,5.221012),
new Point(28.553932,21.466385,6.518181),
new Point(37.241184,20.677567,4.381117),
new Point(10.698041,27.8614,0),
new Point(16.727324,27.316485,2.498912),
new Point(22.965645,26.617577,5.202981),
new Point(29.713399,25.893005,4.936347),
new Point(37.40044,25.168432,2.190558),
new Point(13.042107,30.658022,0),
new Point(17.77792,31.434957,0),
new Point(24.186035,30.979952,0),
new Point(30.872866,30.319624,0),
new Point(37.559697,29.659296,0)
        };

            List<Point> initialPoints = new List<Point>();
            foreach (Point p in points) initialPoints.Add(new Point(p.X, p.Y, p.Z+1));

            Dictionary<Point, bool> IsSupport = new Dictionary<Point, bool>();
            foreach (Point point in points)
            {
                if (point.Z == 0.0) IsSupport.Add(point, true);
                else IsSupport.Add(point, false);
            }

            //The force F applied to all free nodes
            List<Euc3D.Vector> F = new List<Euc3D.Vector>
            {
           new Euc3D.Vector(0,0,-6.686193),
new Euc3D.Vector(0,0,-9.252165),
new Euc3D.Vector(0,0,-15.409107),
new Euc3D.Vector(0,0,-20.005298),
new Euc3D.Vector(0,0,-12.113049),
new Euc3D.Vector(0,0,-9.772011),
new Euc3D.Vector(0,0,-25.750933),
new Euc3D.Vector(0,0,-33.042719),
new Euc3D.Vector(0,0,-38.153798),
new Euc3D.Vector(0,0,-22.336946),
new Euc3D.Vector(0,0,-12.369321),
new Euc3D.Vector(0,0,-28.803824),
new Euc3D.Vector(0,0,-30.653472),
new Euc3D.Vector(0,0,-35.469352),
new Euc3D.Vector(0,0,-20.530573),
new Euc3D.Vector(0,0,-9.84135),
new Euc3D.Vector(0,0,-31.072355),
new Euc3D.Vector(0,0,-35.560979),
new Euc3D.Vector(0,0,-41.851701),
new Euc3D.Vector(0,0,-21.03123),
new Euc3D.Vector(0,0,-8.40707),
new Euc3D.Vector(0,0,-11.950057),
new Euc3D.Vector(0,0,-23.2986),
new Euc3D.Vector(0,0,-23.04159),
new Euc3D.Vector(0,0,-8.384359),

            };

            Dictionary<int, int> ConnectivityMatrix = new Dictionary<int, int>
            {
              {1,1},
{-1,6},
{2,2},
{-2,7},
{3,3},
{-3,8},
{4,5},
{-4,6},
{5,6},
{-5,7},
{6,6},
{-6,11},
{7,7},
{-7,8},
{8,7},
{-8,12},
{9,8},
{-9,9},
{10,8},
{-10,13},
{11,10},
{-11,11},
{12,11},
{-12,12},
{13,11},
{-13,16},
{14,12},
{-14,13},
{15,12},
{-15,17},
{16,13},
{-16,14},
{17,13},
{-17,18},
{18,15},
{-18,16},
{19,16},
{-19,17},
{20,16},
{-20,21},
{21,17},
{-21,18},
{22,17},
{-22,22},
{23,18},
{-23,19},
{24,18},
{-24,23},


            };

            //Definie the force densities of each elements

            int Nfd = ConnectivityMatrix.Count / 2;
            double fdInit = -50;

            double[] qList = new double[Nfd];
            for (int i = 0; i < Nfd; i++) { qList[i] = fdInit; }

            double[] d = new double[Nfd];
            for (int i = 0; i < Nfd; i++) d[i] = fdInit;

            #endregion

            #region Declaration of the Solver

            double tolerance = 0.001;
            int maxIter = 1;  // Useless as long as GPA is being debuged
            GuidedProjectionAlgorithm gpa = new GuidedProjectionAlgorithm(tolerance, maxIter);

            #endregion

            #region Set Variables
            int fdCount = Nfd;


            /****** set force densities variables ****/
            VariableSet forceDensities = gpa.AddVariableSet(1, fdCount);
            for (int i = 0; i < fdCount; i++) { forceDensities.AddVariable(qList[i]); }


            /****** set nodes variables ******/
            int vertexDimension = 1;
            VariableSet nodes = gpa.AddVariableSet(vertexDimension, 3 * points.Count);
            foreach (Point p in points)
            {
                nodes.AddVariable(p.X);
                nodes.AddVariable(p.Y);
                nodes.AddVariable(p.Z);
            }


            /******* set dummy variables *******/
            VariableSet dummy = gpa.AddVariableSet(1, fdCount);
            for (int i = 0; i < fdCount; i++) { dummy.AddVariable(d[i]); }




            #endregion

            #region Set Constraints

            /********** Target FD Upper Bound *******/
            double upperFDbound = -5;

            for (int q = 0; q < fdCount; q++)
            {
                UpperBound constraintTypeC = new UpperBound(upperFDbound);
                List<(VariableSet, int)> variablesC = new List<(VariableSet, int)> { };

                variablesC.Add((forceDensities, q));
                variablesC.Add((dummy, q));


                gpa.AddConstraint(constraintTypeC, variablesC, 1);
            }

            /********** Target Distance *************/
                int segmentCounterA = 0;
                foreach (Point p in points)
                {
                    if (!IsSupport[p])
                    {
                        DistanceToPoint constraintTypeA = new DistanceToPoint(initialPoints[segmentCounterA]);

                        List<(VariableSet, int)> variablesA = new List<(VariableSet, int)> {
                            (nodes, segmentCounterA*3), (nodes, segmentCounterA*3 + 1), (nodes, segmentCounterA*3+2)
                        };

                        gpa.AddConstraint(constraintTypeA, variablesA, 1);
                    }
                    segmentCounterA++;
                }

            /********** Target Equilibrium **********/

            int segmentCounterB = 0;
            foreach (Point point in points)
            {
                if (!IsSupport[point])
                {
                    List<int> adjNodesIndex = new List<int>();
                    List<int> adjFDIndex = new List<int>();
                    for (int j = 1; j < Nfd + 1; j++)
                    {
                        if (ConnectivityMatrix[j] == segmentCounterB)
                        {
                            adjNodesIndex.Add(ConnectivityMatrix[-j]);
                            adjFDIndex.Add(j - 1);
                        }

                        if (ConnectivityMatrix[-j] == segmentCounterB)
                        {
                            adjNodesIndex.Add(ConnectivityMatrix[j]);
                            adjFDIndex.Add(j - 1);
                        }

                    }

                    for (int i = 0; i < 3; i++)
                    {
                        NodeEquilibrium constraintTypeB = new NodeEquilibrium(F[segmentCounterB].GetCoordinates()[i], adjNodesIndex.Count);

                        List<(VariableSet, int)> variablesB = new List<(VariableSet, int)> { };
                        foreach (int m in adjFDIndex) { variablesB.Add((forceDensities, m)); }

                        variablesB.Add((nodes, segmentCounterB * 3 + i));

                        foreach (int n in adjNodesIndex)
                        {
                            variablesB.Add((nodes, 3 * n + i));
                        }

                        gpa.AddConstraint(constraintTypeB, variablesB, 1);

                    }
                }
                segmentCounterB++;
            }
            #endregion

            #region Launch

            gpa.InitialiseX();

            //for (int j = 0; j < Nfd; j++)
            //{
            //    System.Console.Write(String.Format("{0:0.00000} ", gpa.X[j]));
            //}

            //System.Console.WriteLine();
            //System.Console.WriteLine();
            //System.Console.WriteLine();

            for (int i = 0; i < gpa.MaxIteration; i++)
            {
                gpa.RunIteration(false);
            }

            for (int j = 0; j < Nfd; j++)
            {
                System.Console.Write(String.Format("{0:0.00000} ", gpa.X[j]));
                System.Console.WriteLine();
            }
            for (int k = 0; k < points.Count; k++)
            {
                System.Console.Write(String.Format("{0:0.00000} ", gpa.X[Nfd + 3 * k]));
                System.Console.Write(String.Format("{0:0.00000} ", gpa.X[Nfd + 3 * k + 1]));
                System.Console.Write(String.Format("{0:0.00000} ", gpa.X[Nfd + 3 * k + 2]));

                System.Console.WriteLine();
            }

            for (int l = 0; l < Nfd; l++)
            {
                System.Console.Write(String.Format("{0:0.00000} ", gpa.X[l + Nfd + 3 * points.Count]));
                System.Console.WriteLine();
            }

            #endregion



        }
    }
}


/// <summary>
/// Constraint enforcing a segment defined from two point variables, <em>pi</em> and <em>pj</em>, to have a given length <em>l</em> (computed with euclidean norm).
/// </summary>
/// <remarks> The vector xReduced = [pi, pj].</remarks>
internal class NodeEquilibrium : IQuadraticConstraintType
{
    #region Properties

    /// <inheritdoc cref="IQuadraticConstraintType.LocalHi"/>
    public SparseMatrix LocalHi { get; }

    /// <inheritdoc cref="IQuadraticConstraintType.LocalBi"/>
    public SparseVector LocalBi { get; }

    /// <inheritdoc cref="IQuadraticConstraintType.Ci"/>
    public double Ci { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initialises a new instance of the <see cref="CoherentLength"/> class.
    /// </summary>
    /// <param name="targetLength"> Target length for the vector. </param>
    /// <param name="spaceDimension"> Dimension of the space containing the points. </param>
    public NodeEquilibrium(double forceExt, int adjNodesCount)
    {
        int spaceDimension = 2 * adjNodesCount + 1;
        /******************** Define LocalHi ********************/
        DictionaryOfKeys dok_Hi = new DictionaryOfKeys(adjNodesCount * 2);
        for (int i = 0; i < adjNodesCount; i++)
        {
            dok_Hi.Add(2, i, adjNodesCount);
            dok_Hi.Add(-2, i, adjNodesCount + 1 + i);
        }

        LocalHi = new CompressedColumn(spaceDimension, spaceDimension, dok_Hi);


        /******************** Define LocalBi ********************/

        LocalBi = null;


        /******************** Define Ci ********************/
        Ci = -forceExt;
    }
    #endregion
}

/// <summary>
/// Constraint enforcing a force density to remain below a given value.
/// </summary>
internal class UpperFDBound : IQuadraticConstraintType
{
    #region Properties

    /// <inheritdoc cref="IQuadraticConstraintType.LocalHi"/>
    public SparseMatrix LocalHi { get; }

    /// <inheritdoc cref="IQuadraticConstraintType.LocalBi"/>
    public SparseVector LocalBi { get; }

    /// <inheritdoc cref="IQuadraticConstraintType.Ci"/>
    public double Ci { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initialises a new instance of the <see cref="UpperFDBound"/> class.
    /// </summary>
    /// <param name="targetLength"> Target length for the vector. </param>
    /// <param name="spaceDimension"> Dimension of the space containing the points. </param>
    public UpperFDBound(double upperBound)
    {
        int spaceDimension = 2;
        /******************** Define LocalHi ********************/
        DictionaryOfKeys dok_Hi = new DictionaryOfKeys(1);
        dok_Hi.Add(-2, 1, 1);


        LocalHi = new CompressedColumn(spaceDimension, spaceDimension, dok_Hi);


        /******************** Define LocalBi ********************/

        LocalBi = new SparseVector(2);
        LocalBi[0] = -1;
        LocalBi[1] = 0;


        /******************** Define Ci ********************/
        Ci = upperBound;
    }
    #endregion
}

/// <summary>
/// Constraint enforcing a segment defined from two point variables, <em>pi</em> and <em>pj</em>, to have a given length <em>l</em> (computed with euclidean norm).
/// </summary>
/// <remarks> The vector xReduced = [pi, pj].</remarks>
internal class DistanceToPoint : IQuadraticConstraintType
{
    #region Properties

    /// <inheritdoc cref="IQuadraticConstraintType.LocalHi"/>
    public SparseMatrix LocalHi { get; }

    /// <inheritdoc cref="IQuadraticConstraintType.LocalBi"/>
    public SparseVector LocalBi { get; }

    /// <inheritdoc cref="IQuadraticConstraintType.Ci"/>
    public double Ci { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initialises a new instance of the <see cref="CoherentLength"/> class.
    /// </summary>
    /// <param name="targetLength"> Target length for the vector. </param>
    /// <param name="spaceDimension"> Dimension of the space containing the points. </param>
    public DistanceToPoint(Point initNode)
    {
        int spaceDimension = 3;
        /******************** Define LocalHi ********************/
        DictionaryOfKeys dok_Hi = new DictionaryOfKeys(spaceDimension);
        for (int i = 0; i < spaceDimension; i++)
        {
            dok_Hi.Add(2, i, i);
        }

        LocalHi = new CompressedColumn(spaceDimension, spaceDimension, dok_Hi);


        /******************** Define LocalBi ********************/
        LocalBi = new SparseVector(spaceDimension);

        LocalBi[0] = -2 * initNode.X;
        LocalBi[1] = -2 * initNode.Y;
        LocalBi[2] = -2 * initNode.Z;



        /******************** Define Ci ********************/
        Ci = initNode.X * initNode.X + initNode.Y * initNode.Y + initNode.Z * initNode.Z;
    }
    #endregion
}