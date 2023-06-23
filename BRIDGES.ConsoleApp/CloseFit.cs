﻿using BRIDGES.Solvers.GuidedProjection;
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
using System.Globalization;

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
new Point(40.18845,37.668301,0),
new Point(21.769007,34.606365,0),
new Point(29.82111,57.56171,0),
new Point(25.025793,45.277184,11.96329),
new Point(4.629511,52.466801,0),


        };

            List<Point> initialPoints = new List<Point>();
            foreach (Point p in points) initialPoints.Add(new Point(p.X, p.Y, p.Z + 1));

            Dictionary<Point, bool> IsSupport = new Dictionary<Point, bool>();
            foreach (Point point in points)
            {
                if (point.Z == 0.0) IsSupport.Add(point, true);
                else IsSupport.Add(point, false);
            }

            //The force F applied to all free nodes
            List<Euc3D.Vector> F = new List<Euc3D.Vector>
            {
new Euc3D.Vector(0,0,-95.476149),
new Euc3D.Vector(0,0,-124.128668),
new Euc3D.Vector(0,0,-128.548374),
new Euc3D.Vector(0,0,-259.01041),
new Euc3D.Vector(0,0,-113.306085),


            };

            Dictionary<int, int> ConnectivityMatrix = new Dictionary<int, int>
            {
{1,0},
{-1,3},
{2,1},
{-2,3},
{3,2},
{-3,3},
{4,3},
{-4,4}



            };

            //Definie the force densities of each elements

            int Nfd = ConnectivityMatrix.Count / 2;
            double fdInit = -100;

            double[] qList = new double[Nfd];
            for (int i = 0; i < Nfd; i++) { qList[i] = fdInit; }

            double[] d = new double[Nfd];
            for (int i = 0; i < Nfd; i++) d[i] = fdInit;

            #endregion

            #region Declaration of the Solver

            double tolerance = 0.001;
            int maxIter = 10;  // Useless as long as GPA is being debuged
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
                if (!IsSupport[p])
                {
                    nodes.AddVariable(p.X);
                    nodes.AddVariable(p.Y);
                    nodes.AddVariable(p.Z);
                }
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
                    segmentCounterA++;
                }

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
                        List<double> supports = new List<double>();
                        List<int> supportsPosition = new List<int>();
                        foreach (int p in adjNodesIndex)
                        {
                            if (IsSupport[points[p]])
                            {
                                supports.Add(points[p].GetCoordinates()[i]);
                                supportsPosition.Add(1);
                            }

                            else supportsPosition.Add(0);
                        }

                        NodeEquilibrium2 constraintTypeB = new NodeEquilibrium2(F[segmentCounterB].GetCoordinates()[i], supportsPosition, supports);

                        List<(VariableSet, int)> variablesB = new List<(VariableSet, int)> { };
                        foreach (int m in adjFDIndex) { variablesB.Add((forceDensities, m)); }

                        variablesB.Add((nodes, segmentCounterB * 3 + i));

                        foreach (int n in adjNodesIndex)
                        {
                            if (!IsSupport[points[n]]) variablesB.Add((nodes, 3 * n + i));
                        }

                        gpa.AddConstraint(constraintTypeB, variablesB, 1);

                    }
                    segmentCounterB++;
                }

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

internal class NodeEquilibrium2 : IQuadraticConstraintType
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
    public NodeEquilibrium2(double forceExt, List<int> supportsPosition, List<double> supports)
    {
        int adjNodesCount = supportsPosition.Count;
        int spaceDimension = 2 * adjNodesCount + 1;
        /******************** Define LocalHi ********************/
        DictionaryOfKeys dok_Hi = new DictionaryOfKeys(adjNodesCount * 2 - supports.Count);
        int j = 0;
        for (int i = 0; i < adjNodesCount; i++)
        {
            dok_Hi.Add(2, i, adjNodesCount);
            if (supportsPosition[i]==0) dok_Hi.Add(-2, i, adjNodesCount + 1 + j);
            j++;
        }

        LocalHi = new CompressedColumn(spaceDimension, spaceDimension, dok_Hi);


        /******************** Define LocalBi ********************/

        LocalBi = new SparseVector(adjNodesCount * 2 - supports.Count);
        int k = 0;
        for (int i = 0; i < adjNodesCount; i++)
        {
            if (supportsPosition[i] == 1)
            {
                LocalBi[i] = supports[k];
                k++;
            }
            else LocalBi[i] = 0;
        }

        for (int i = adjNodesCount; i < adjNodesCount * 2 - supports.Count; i++) LocalBi[i] = 0;


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