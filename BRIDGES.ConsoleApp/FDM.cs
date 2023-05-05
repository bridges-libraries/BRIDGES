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
    internal class FDM
    {
        internal static void FDMWE()
        {
            #region Geometry
            //The nodes of the model
            List<Point> points = new List<Point>
            {
                new Point(11.298875,16.220161,0),
                new Point(13.229426,15.152515,0),
new Point(15.88022,14.384027,0),
new Point(18.963161,13.795034,0),
new Point(22.196153,13.268365,0),
new Point(25.435147,12.744189,0),
new Point(28.67414,12.220014,0),
new Point(31.913134,11.695838,0),
new Point(10.215031,18.096054,0),
new Point(13.035337,17.394798,0.251408),
new Point(16.091651,16.802566,0.641675),
new Point(19.314277,16.275746,1.326402),
new Point(22.635681,15.77164,2.434212),
new Point(26.038114,15.268442,3.473239),
new Point(29.553608,14.765244,3.331132),
new Point(33.216358,14.262046,0.86856),
new Point(9.851431,20.271104,0),
new Point(13.077257,19.746104,0.744983),
new Point(16.278808,19.244653,1.626679),
new Point(19.541366,18.75733,2.761079),
new Point(22.950616,18.274914,4.236511),
new Point(26.6015,17.792694,5.505027),
new Point(30.598216,17.310475,5.382408),
new Point(35.045364,16.828255,2.656771),
new Point(9.919978,22.62565,0),
new Point(13.285488,22.162823,1.221131),
new Point(16.526971,21.700869,2.596008),
new Point(19.811757,21.239438,4.034462),
new Point(23.307082,20.778189,5.433253),
new Point(27.17808,20.316947,6.388588),
new Point(31.587777,19.855705,6.196119),
new Point(36.69911,19.394463,4.138431),
new Point(10.145076,25.037731,0),
new Point(13.593419,24.60051,1.426791),
new Point(16.917911,24.161615,3.193179),
new Point(20.285809,23.721714,4.870789),
new Point(23.864283,23.281463,6.037114),
new Point(27.818431,22.841199,6.404594),
new Point(32.311279,22.400935,5.820615),
new Point(37.505762,21.960671,4.138431),
new Point(10.538693,27.332583,0),
new Point(14.005412,26.995475,1.259132),
new Point(17.452567,26.613132,3.119605),
new Point(20.963556,26.203647,4.851898),
new Point(24.622218,25.784738,5.746165),
new Point(28.522553,25.365452,5.545042),
new Point(32.76872,24.946166,4.443647),
new Point(37.46532,24.52688,2.656771),
new Point(11.400359,29.282635,0),
new Point(14.596801,29.264785,0.765558),
new Point(18.051058,29.037508,2.134601),
new Point(21.68471,28.684575,3.417221),
new Point(25.421685,28.288013,3.943866),
new Point(29.23987,27.889704,3.513254),
new Point(33.171115,27.491396,2.392371),
new Point(37.249617,27.093088,0.86856),
new Point(13.042107,30.658022,0),
new Point(15.446004,31.324668,0),
new Point(18.629984,31.416648,0),
new Point(22.282012,31.163828,0),
new Point(26.096558,30.791287,0),
new Point(29.917604,30.413957,0),
new Point(33.738651,30.036627,0),
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
            new Euc3D.Vector(0,0,-1.78211),
new Euc3D.Vector(0,0,-2.763638),
new Euc3D.Vector(0,0,-3.656222),
new Euc3D.Vector(0,0,-4.452334),
new Euc3D.Vector(0,0,-5.820403),
new Euc3D.Vector(0,0,-7.035519),
new Euc3D.Vector(0,0,-7.909713),
new Euc3D.Vector(0,0,-2.358927),
new Euc3D.Vector(0,0,-2.822265),
new Euc3D.Vector(0,0,-6.255854),
new Euc3D.Vector(0,0,-8.176486),
new Euc3D.Vector(0,0,-9.96396),
new Euc3D.Vector(0,0,-11.794759),
new Euc3D.Vector(0,0,-13.107364),
new Euc3D.Vector(0,0,-15.018878),
new Euc3D.Vector(0,0,-7.968031),
new Euc3D.Vector(0,0,-3.551158),
new Euc3D.Vector(0,0,-7.96877),
new Euc3D.Vector(0,0,-9.105397),
new Euc3D.Vector(0,0,-10.184813),
new Euc3D.Vector(0,0,-11.022971),
new Euc3D.Vector(0,0,-11.489578),
new Euc3D.Vector(0,0,-13.705151),
new Euc3D.Vector(0,0,-9.349876),
new Euc3D.Vector(0,0,-4.203621),
new Euc3D.Vector(0,0,-8.854421),
new Euc3D.Vector(0,0,-9.332141),
new Euc3D.Vector(0,0,-9.996707),
new Euc3D.Vector(0,0,-10.384483),
new Euc3D.Vector(0,0,-10.888321),
new Euc3D.Vector(0,0,-13.145114),
new Euc3D.Vector(0,0,-8.027227),
new Euc3D.Vector(0,0,-4.290276),
new Euc3D.Vector(0,0,-9.30637),
new Euc3D.Vector(0,0,-9.530949),
new Euc3D.Vector(0,0,-9.728945),
new Euc3D.Vector(0,0,-10.003518),
new Euc3D.Vector(0,0,-11.104506),
new Euc3D.Vector(0,0,-13.576689),
new Euc3D.Vector(0,0,-7.015336),
new Euc3D.Vector(0,0,-3.681349),
new Euc3D.Vector(0,0,-9.095161),
new Euc3D.Vector(0,0,-10.205495),
new Euc3D.Vector(0,0,-10.33569),
new Euc3D.Vector(0,0,-10.822257),
new Euc3D.Vector(0,0,-12.273119),
new Euc3D.Vector(0,0,-14.358741),
new Euc3D.Vector(0,0,-7.424637),
new Euc3D.Vector(0,0,-3.041408),
new Euc3D.Vector(0,0,-7.36745),
new Euc3D.Vector(0,0,-11.550326),
new Euc3D.Vector(0,0,-13.405027),
new Euc3D.Vector(0,0,-14.617281),
new Euc3D.Vector(0,0,-14.9758),
new Euc3D.Vector(0,0,-14.503523),
new Euc3D.Vector(0,0,-6.713575),
new Euc3D.Vector(0,0,-1.968402),
new Euc3D.Vector(0,0,-3.57443),
new Euc3D.Vector(0,0,-5.062283),
new Euc3D.Vector(0,0,-8.2916),
new Euc3D.Vector(0,0,-8.934017),
new Euc3D.Vector(0,0,-8.224503),
new Euc3D.Vector(0,0,-6.863518),
new Euc3D.Vector(0,0,-2.560788),





            };

            Dictionary<int, int> ConnectivityMatrix = new Dictionary<int, int>
            {
                {1,1},
{-1,9},
{2,2},
{-2,10},
{3,3},
{-3,11},
{4,4},
{-4,12},
{5,5},
{-5,13},
{6,6},
{-6,14},
{7,8},
{-7,9},
{8,9},
{-8,10},
{9,9},
{-9,17},
{10,10},
{-10,11},
{11,10},
{-11,18},
{12,11},
{-12,12},
{13,11},
{-13,19},
{14,12},
{-14,13},
{15,12},
{-15,20},
{16,13},
{-16,14},
{17,13},
{-17,21},
{18,14},
{-18,15},
{19,14},
{-19,22},
{20,16},
{-20,17},
{21,17},
{-21,18},
{22,17},
{-22,25},
{23,18},
{-23,19},
{24,18},
{-24,26},
{25,19},
{-25,20},
{26,19},
{-26,27},
{27,20},
{-27,21},
{28,20},
{-28,28},
{29,21},
{-29,22},
{30,21},
{-30,29},
{31,22},
{-31,23},
{32,22},
{-32,30},
{33,24},
{-33,25},
{34,25},
{-34,26},
{35,25},
{-35,33},
{36,26},
{-36,27},
{37,26},
{-37,34},
{38,27},
{-38,28},
{39,27},
{-39,35},
{40,28},
{-40,29},
{41,28},
{-41,36},
{42,29},
{-42,30},
{43,29},
{-43,37},
{44,30},
{-44,31},
{45,30},
{-45,38},
{46,32},
{-46,33},
{47,33},
{-47,34},
{48,33},
{-48,41},
{49,34},
{-49,35},
{50,34},
{-50,42},
{51,35},
{-51,36},
{52,35},
{-52,43},
{53,36},
{-53,37},
{54,36},
{-54,44},
{55,37},
{-55,38},
{56,37},
{-56,45},
{57,38},
{-57,39},
{58,38},
{-58,46},
{59,40},
{-59,41},
{60,41},
{-60,42},
{61,41},
{-61,49},
{62,42},
{-62,43},
{63,42},
{-63,50},
{64,43},
{-64,44},
{65,43},
{-65,51},
{66,44},
{-66,45},
{67,44},
{-67,52},
{68,45},
{-68,46},
{69,45},
{-69,53},
{70,46},
{-70,47},
{71,46},
{-71,54},
{72,48},
{-72,49},
{73,49},
{-73,50},
{74,49},
{-74,57},
{75,50},
{-75,51},
{76,50},
{-76,58},
{77,51},
{-77,52},
{78,51},
{-78,59},
{79,52},
{-79,53},
{80,52},
{-80,60},
{81,53},
{-81,54},
{82,53},
{-82,61},
{83,54},
{-83,55},
{84,54},
{-84,62},


            };

            //Definie the force densities of each elements

            int Nfd = ConnectivityMatrix.Count / 2;
            double fdInit = -5;

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
            /*      VariableSet forceDensities = gpa.AddVariableSet(1, fdCount);
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
            /*     VariableSet dummy = gpa.AddVariableSet(1, fdCount);
                 for (int i = 0; i < fdCount; i++) { dummy.AddVariable(d[i]); }
            */



            #endregion

            #region Set Constraints

            /********** Target FD Upper Bound *******/
            /* double upperFDbound = -10;

             for (int q = 0; q < fdCount; q++)
             {
                 UpperBound constraintTypeC = new UpperBound(upperFDbound);
                 List<(VariableSet, int)> variablesC = new List<(VariableSet, int)> { };

                 variablesC.Add((forceDensities, q));
                 variablesC.Add((dummy, q));


                 gpa.AddConstraint(constraintTypeC, variablesC, 1);
             }

             /********** Target Distance *************/
            /*    int segmentCounterA = 0;
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

                    double[] adjFD = new double[adjNodesIndex.Count];
                    for (int n = 0; n < adjNodesIndex.Count; n++) adjFD[n] = qList[adjFDIndex[n]];

                    for (int i = 0; i < 3; i++)
                    {
                        FDMsolve constraintTypeB = new FDMsolve(adjFD, F[segmentCounterB].GetCoordinates()[i]);

                        List<(VariableSet, int)> variablesB = new List<(VariableSet, int)> { };
                        
                        variablesB.Add((nodes, segmentCounterB * 3 + i));

                        foreach (int n in adjNodesIndex)
                        {
                            variablesB.Add((nodes, 3 * n + i));
                        }

                        gpa.AddEnergy(constraintTypeB, variablesB, 1);

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

           
            for (int k = 0; k < points.Count; k++)
            {
                System.Console.Write(String.Format("{0:0.00000} ", gpa.X[ 3 * k]));
                System.Console.Write(String.Format("{0:0.00000} ", gpa.X[3 * k + 1]));
                System.Console.Write(String.Format("{0:0.00000} ", gpa.X[ 3 * k + 2]));

                System.Console.WriteLine();
            }

          

            #endregion



        }
    }
}


/// <summary>
/// Energy enforcing the sum of scalar variables to equal a given value.
/// </summary>
internal class FDMsolve : IEnergyType
{
    #region Properties

    /// <inheritdoc cref="IEnergyType.LocalKi"/>
    public SparseVector LocalKi { get; }

    /// <inheritdoc cref="IEnergyType.Si"/>
    public double Si { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initialises a new instance of the <see cref="SummedValue"/> class.
    /// </summary>
    /// <param name="targetValue"> Target value for the sum. </param>
    /// <param name="count"> Number of scalar value to sum.. </param>
    public FDMsolve(double[] FDs, double extForce)
    {
        /******************** Define LocalKi ********************/

        LocalKi = new SparseVector(FDs.Count() + 1);

        LocalKi[0] = FDs.Sum();
        for (int i = 0; i < FDs.Count(); i++)
        {
            LocalKi[i + 1] = FDs[i];
        }


        /******************** Define Si ********************/

        Si = -extForce;
    }

    #endregion
}
  

