using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.Geometry.Euclidean3D;

using Geo_Ker = BRIDGES.Geometry.Kernel;


namespace BRIDGES.Tests.Geometry.Euclidean3D
{
    /// <summary>
    /// Class testing the members of the <see cref="Line"/> structure.
    /// </summary>
    [TestClass]
    public class LineTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="Line"/> is not reference type.
        /// </summary>
        [TestMethod("Behavior IsNotReference")]
        public void IsNotReference()
        {
            // Arrange
            Line lineA = new Line(new Point(1.0, 2.0 ,3.0), new Vector(1.5, 2.5, 3.5));
            Line lineB = new Line(new Point(4.0, 5.0, 6.0), new Vector(4.5, 5.5, 6.5));
            //Act
            lineA = lineB;
            // Assert
            Assert.IsTrue(lineA.Equals(lineB));
            Assert.AreNotSame(lineA, lineB);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the initialisation of the <see cref="Line"/> from its three cooridnates,
        /// and the <see cref="Line.Origin"/>, <see cref="Line.Axis"/> properties.
        /// </summary>
        [TestMethod("Property Origin & Axis")]
        public void OriginAndAxis()
        {
            // Arrange
            Line line = new Line(new Point(1.0, 2.0, 3.0), new Vector(1.5, 2.5, 3.5));
            Point expectedOrigin = new Point(1.0, 2.0, 3.0);
            Vector expectedAxis = new Vector(1.5, 2.5, 3.5);
            //Act
            Point actualOrigin = line.Origin;
            Vector actualAxis = line.Axis;
            // Assert
            Assert.IsTrue(expectedOrigin.Equals(actualOrigin));
            Assert.IsTrue(expectedAxis.Equals(actualAxis));
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Tests the initialisation of the <see cref="Line"/> from another <see cref="Line"/>.
        /// </summary>
        [TestMethod("Constructor(Line)")]
        public void Constructor_Line()
        {
            // Arrange
            Line expected = new Line(new Point(1.0, 2.0, 3.0), new Vector(1.5, 2.5, 3.5));
            // Act
            Line actual = new Line(expected);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tests the method <see cref="Line.Flip()"/>.
        /// </summary>
        [TestMethod("Method Flip()")]
        public void Flip()
        {
            // Arrange
            Line actual = new Line(new Point(1.0, 2.0, 3.0), new Vector(1.5, 2.5, 3.5));
            Line expected = new Line(new Point(1.0, 2.0, 3.0), new Vector(-1.5, -2.5, -3.5));
            // Act
            actual.Flip();
            //Assert
            Assert.IsTrue(actual.Equals(expected));
        }

        /// <summary>
        /// Tests the method <see cref="Line.PointAt(double, Geo_Ker.CurveParameterFormat)"/>.
        /// </summary>
        [TestMethod("Method PointAt(double)")]
        public void PointAt_double()
        {
            // Arrange
            Line line = new Line(new Point(1.0, 2.0, 3.0), new Vector(1.5, 2.5, 3.5));
            double paramA = 0.0;
            Point expectedA = new Point(1.0, 2.0, 3.0);
            double paramB = Math.Sqrt(20.75);
            Point expectedB = new Point(2.5, 4.5, 6.5);
            double paramC = 3.0 * Math.Sqrt(20.75);
            Point expectedC = new Point(5.5, 9.5, 13.5);
            // Act
            Point actualA = line.PointAt(paramA, Geo_Ker.CurveParameterFormat.ArcLength);
            Point actualB = line.PointAt(paramB, Geo_Ker.CurveParameterFormat.ArcLength);
            Point actualC = line.PointAt(paramC, Geo_Ker.CurveParameterFormat.ArcLength);
            //Assert
            Assert.IsTrue(expectedA.Equals(actualA));
            Assert.IsTrue(expectedB.Equals(actualB));
            Assert.IsTrue(expectedC.Equals(actualC));
        }


        /// <summary>
        /// Tests the method <see cref="Line.Equals(Line)"/>.
        /// </summary>
        [TestMethod("Method Equals(Line)")]
        public void Equals_Line()
        {
            // Arrange
            Line lineA = new Line(new Point(1.0, 2.0, 3.0), new Vector(1.5, 2.5, 3.5));
            Line lineB = new Line(new Point(1.0, 2.0, 3.0), new Vector(1.5, 2.5, 3.5));
            // Act
            bool areEqual = lineA.Equals(lineB);
            // Assert
            Assert.IsTrue(areEqual);
        }

        #endregion
    }
}
