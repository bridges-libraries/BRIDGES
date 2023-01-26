using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.Geometry.Euclidean3D;

using Geo_Ker = BRIDGES.Geometry.Kernel;


namespace BRIDGES.Test.Geometry.Euclidean3D
{
    /// <summary>
    /// Class testing the members of the <see cref="NurbsCurve"/> structure.
    /// </summary>
    [TestClass]
    public class NurbsCurveTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="NurbsCurve"/> is not reference type.
        /// </summary>
        [TestMethod("Behavior IsReference")]
        public void IsReference()
        {
            // Arrange
            Point[] controlPoints = new Point[4] { new Point(0.0, 0.0, 0.0), new Point(0.0, 1.0, 0.0), new Point(1.0, 1.0, 0.0), new Point(1.0, 0.0, 0.0) };

            NurbsCurve nurbsCurveA = new NurbsCurve(2 , controlPoints);
            NurbsCurve nurbsCurveB = new NurbsCurve(3, controlPoints);

            //Act
            nurbsCurveA = nurbsCurveB;

            // Assert
            Assert.IsTrue(nurbsCurveA.Equals(nurbsCurveB));
            Assert.AreSame(nurbsCurveA, nurbsCurveB);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the <see cref="NurbsCurve.StartPoint"/> property.
        /// </summary>
        [TestMethod("Property StartPoint")]
        public void StartPoint()
        {
            // Arrange
            Point[] controlPoints = new Point[] { new Point(0.0, 0.0, 0.0), new Point(0.0, 1.0, 0.0), new Point(1.0, 1.0, 0.0), new Point(1.0, 0.0, 0.0) };
            Point start = controlPoints[0];
            // Act
            NurbsCurve quadratic = new NurbsCurve(2, controlPoints);
            NurbsCurve cubic = new NurbsCurve(3, controlPoints);
            //Assert
            Assert.IsTrue(start.Equals(quadratic.StartPoint));
            Assert.IsTrue(start.Equals(cubic.StartPoint));
        }

        /// <summary>
        /// Tests the <see cref="NurbsCurve.EndPoint"/> property.
        /// </summary>
        [TestMethod("Property EndPoint")]
        public void EndPoint()
        {
            // Arrange
            Point[] controlPoints = new Point[] { new Point(0.0, 0.0, 0.0), new Point(0.0, 1.0, 0.0), new Point(1.0, 1.0, 0.0), new Point(1.0, 0.0, 0.0) };
            Point end = controlPoints[controlPoints.Length - 1];
            // Act
            NurbsCurve quadratic = new NurbsCurve(2, controlPoints);
            NurbsCurve cubic = new NurbsCurve(3, controlPoints);
            //Assert
            Assert.IsTrue(end.Equals(quadratic.EndPoint));
            Assert.IsTrue(end.Equals(cubic.EndPoint));
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Tests the initialisation of the <see cref="NurbsCurve"/> from its degree and associated knot vector.
        /// </summary>
        [TestMethod("Constructor(int,double[])")]
        [DataRow(2, new double[7] { 0.0, 0.0, 0.0, 0.5, 1.0, 1.0, 1.0 }, DisplayName = "Quadratic Nurbs Curve")]
        [DataRow(3, new double[8] { 0.0, 0.0, 0.0, 0.0, 1.0, 1.0, 1.0, 1.0 }, DisplayName = "Cubic Nurbs Curve")]
        public void Constructor_Int_Points(int degree, double[] expectedKnotVector)
        {
            // Arrange
            Point[] controlPoints = new Point[4] { new Point(0.0, 0.0, 0.0), new Point(0.0, 1.0, 0.0), new Point(1.0, 1.0, 0.0), new Point(1.0, 0.0, 0.0) };
            // Act
            NurbsCurve nurbsCurve = new NurbsCurve(degree, controlPoints);
            // Assert
            int knotCount = expectedKnotVector.Length;
            Assert.AreEqual(knotCount, nurbsCurve.KnotCount);

            for (int i_K = 0; i_K < knotCount; i_K++)
            {
                Assert.AreEqual(expectedKnotVector[i_K], nurbsCurve.Knot(i_K), Settings.AbsolutePrecision);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tests the method <see cref="NurbsCurve.PointAt(double, Geo_Ker.CurveParameterFormat)"/>.
        /// </summary>
        [TestMethod("Method PointAt(double,CurveParameterFormat)")]
        public void PointAt_double_CurveParameterFormat()
        {
            // Arrange
            Point[] controlPoints = new Point[] { new Point(0.0, 0.0, 0.0), new Point(0.0, 1.0, 0.0), new Point(1.0, 1.0, 0.0), new Point(1.0, 0.0, 0.0) };
            
            NurbsCurve quadratic = new NurbsCurve(2, controlPoints);
            NurbsCurve cubic = new NurbsCurve(3, controlPoints);

            Point expectedA = new Point(0.18, 0.84, 0.0);   // Computed with Rhino3D
            Point expectedB = new Point(0.5, 1.0, 0.0);     // Computed with Rhino3D

            Point expectedC = new Point(0.216, 0.63, 0.0);  // Computed with Rhino3D
            Point expectedD = new Point(0.5, 0.75, 0.0);    // Computed with Rhino3D

            // Act
            Point actualA = quadratic.PointAt(0.3, Geo_Ker.CurveParameterFormat.Normalised);
            Point actualB = quadratic.PointAt(0.5, Geo_Ker.CurveParameterFormat.Normalised);

            Point actualC = cubic.PointAt(0.3, Geo_Ker.CurveParameterFormat.Normalised);
            Point actualD = cubic.PointAt(0.5, Geo_Ker.CurveParameterFormat.Normalised);

            //Assert
            Assert.IsTrue(expectedA.Equals(actualA));      
            Assert.IsTrue(expectedB.Equals(actualB));
            Assert.IsTrue(expectedC.Equals(actualC));
            Assert.IsTrue(expectedD.Equals(actualD));
        }

        /// <summary>
        /// Tests the method <see cref="Geo_Ker.BSplineCurve{TPoint}.Flip()"/>.
        /// </summary>
        [TestMethod("Method Flip()")]
        public void Flip()
        {
            // Arrange
            Point[] controlPoints = new Point[] { new Point(0.0, 0.0, 0.0), new Point(0.0, 1.0, 0.0), new Point(1.0, 1.0, 0.0), new Point(1.0, 0.0, 0.0) };

            NurbsCurve quadratic = new NurbsCurve(2, controlPoints);
            NurbsCurve cubic = new NurbsCurve(3, controlPoints);


            Point expectedA = new Point(0.5, 1.0, 0.0);     // Computed with Rhino3D
            Point expectedB = new Point(0.18, 0.84, 0.0);   // Computed with Rhino3D

            Point expectedC = new Point(0.5, 0.75, 0.0);    // Computed with Rhino3D
            Point expectedD = new Point(0.216, 0.63, 0.0);  // Computed with Rhino3D

            // Act
            quadratic.Flip();
            cubic.Flip();

            Point actualA = quadratic.PointAt(0.5, BRIDGES.Geometry.Kernel.CurveParameterFormat.Normalised);
            Point actualB = quadratic.PointAt(0.7, BRIDGES.Geometry.Kernel.CurveParameterFormat.Normalised);

            Point actualC = cubic.PointAt(0.5, BRIDGES.Geometry.Kernel.CurveParameterFormat.Normalised);
            Point actualD = cubic.PointAt(0.7, BRIDGES.Geometry.Kernel.CurveParameterFormat.Normalised);

            //Assert
            Assert.IsTrue(expectedA.Equals(actualA));
            Assert.IsTrue(expectedB.Equals(actualB));
            Assert.IsTrue(expectedC.Equals(actualC));
            Assert.IsTrue(expectedD.Equals(actualD));
        }

        #endregion
    }
}
