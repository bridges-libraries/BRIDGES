using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.Geometry.Euclidean3D;

using Alg_Mes = BRIDGES.Algebra.Measure;


namespace BRIDGES.Tests.Geometry.Euclidean3D
{
    /// <summary>
    /// Class testing the members of the <see cref="Point"/> structure.
    /// </summary>
    [TestClass]
    public class PointTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="Point"/> is not reference type.
        /// </summary>
        [TestMethod("Behavior IsNotReference")]
        public void IsNotReference()
        {
            // Arrange
            Point pointA = new Point(1.0, 2.0, 3.0);
            Point pointB = new Point(4.0, 5.0, 6.0);

            //Act
            pointA = pointB;

            // Assert
            Assert.IsTrue(pointA.Equals(pointB));
            Assert.AreNotSame(pointA, pointB);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the <see cref="Point.Dimension"/> property.
        /// </summary>
        [TestMethod("Property Dimension")]
        public void Dimension()
        {
            // Arrange
            Point point = new Point(1.5, 1.7, 2.1);
            //Act
            int dimension = point.Dimension;
            // Assert
            Assert.AreEqual(3, dimension);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Point"/> from its three cooridnates,
        /// and the <see cref="Point.X"/>, <see cref="Point.Y"/>, <see cref="Point.Z"/> properties.
        /// </summary>
        [TestMethod("Property X & Y & Z")]
        public void XAndYAndZ()
        {
            // Arrange
            Point point = new Point(1.5, 1.7, 2.1);
            double[] expected = new double[] { 1.5, 1.7, 2.1 };
            //Act
            double x = point.X;
            double y = point.Y;
            double z = point.Z;
            // Assert
            Assert.AreEqual(expected[0], x, Settings.AbsolutePrecision);
            Assert.AreEqual(expected[1], y, Settings.AbsolutePrecision);
            Assert.AreEqual(expected[2], z, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the <see cref="Point"/> indexer property.
        /// </summary>
        [TestMethod("Property this[int]")]
        public void Indexer()
        {
            // Arrange
            Point point = new Point(1.5, 1.7, 2.1);
            double[] expected = new double[] { 1.5, 1.7, 2.1 };
            //Act
            double x = point[0];
            double y = point[1];
            double z = point[2];
            // Assert
            Assert.AreEqual(expected[0], x, Settings.AbsolutePrecision);
            Assert.AreEqual(expected[1], y, Settings.AbsolutePrecision);
            Assert.AreEqual(expected[2], z, Settings.AbsolutePrecision);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Tests the initialisation of the <see cref="Point"/> from its coordinates.
        /// </summary>
        [TestMethod("Constructor(double[])")]
        public void Constructor_doubleArray()
        {
            // Arrange
            Point expected = new Point(2.0, 5.0, 8.0);
            bool throwsException = false;

            // Act
            Point actual = new Point(new double[3] { 2.0, 5.0, 8.0 });

            try { Point otherPoint = new Point(new double[] { 2.0, 5.0, 8.0, 11.0 }); } 
            catch (ArgumentOutOfRangeException) { throwsException = true; }

            // Assert
            Assert.IsTrue(expected.Equals(actual));
            Assert.IsTrue(throwsException);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Point"/> from another <see cref="Point"/>.
        /// </summary>
        [TestMethod("Constructor(Point)")]
        public void Constructor_Point()
        {
            // Arrange
            Point expected = new Point(2.0, 5.0, 8.0);

            // Act
            Point actual = new Point(expected);

            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Tests the static property <see cref="Point.Zero"/>.
        /// </summary>
        [TestMethod("Static Zero")]
        public void Static_Zero()
        {
            // Arrange
            Point expected = new Point(0.0, 0.0, 0.0);
            // Act
            Point actual = Point.Zero;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Tests the static method <see cref="Point.CrossProduct(Point, Point)"/>.
        /// </summary>
        [TestMethod("Static CrossProduct(Point,Point)")]
        public void Static_CrossProduct_Point_Point()
        {
            // Arrange
            Point pointA = new Point(1.0, 2.0, 3.0);
            Point pointB = new Point(1.5, 3.5, 5.0);
            Point expected = new Point(-0.5, -0.5, 0.5);
            //Act
            Point actual = Point.CrossProduct(pointA, pointB);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Tests the static method <see cref="Point.Add(Point, Point)"/>.
        /// </summary>
        [TestMethod("Static Add(Point,Point)")]
        public void Static_Add_Point_Point()
        {
            // Arrange
            Point pointA = new Point(1.5, 2.5, 3.5);
            Point pointB = new Point(1.2, 2.4, 3.6);
            Point expected = new Point(2.7, 4.9, 7.1);
            //Act
            Point actual = Point.Add(pointA, pointB);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Point.Subtract(Point, Point)"/>.
        /// </summary>
        [TestMethod("Static Subtract(Point,Point)")]
        public void Static_Substract_Point_Point()
        {
            // Arrange
            Point pointA = new Point(1.5, 2.5, 3.5);
            Point pointB = new Point(1.2, 2.4, 3.6);
            Point expected = new Point(0.3, 0.1, -0.1);
            //Act
            Point actual = Point.Subtract(pointA, pointB);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Embedding : Vector ********************/

        /// <summary>
        /// Tests the static method <see cref="Point.Add(Point, Vector)"/>
        /// </summary>
        [TestMethod("Static Add(Point,Vector)")]
        public void Static_Add_Point_Vector()
        {
            // Arrange
            Point point = new Point(2.2, 4.6, 9.2);
            Vector vector = new Vector(3.3, 4.4, 5.5);
            Point expected = new Point(5.5, 9.0, 14.7);
            //Act
            Point actual = Point.Add(point, vector);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Point.Add(Vector, Point)"/>
        /// </summary>
        [TestMethod("Static Add(Vector,Point)")]
        public void Static_Add_Vector_Point()
        {
            // Arrange
            Vector vector = new Vector(2.2, 4.6, 9.2);
            Point point = new Point(3.3, 4.4, 5.5);
            Point expected = new Point(5.5, 9.0, 14.7);
            //Act
            Point actual = Point.Add(vector, point);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static method <see cref="Point.Subtract(Point, Vector)"/>
        /// </summary>
        [TestMethod("Static Subtract(Point,Vector)")]
        public void Static_Subtract_Point_Vector()
        {
            // Arrange
            Point point = new Point(2.2, 4.6, 9.2);
            Vector vector = new Vector(3.3, -4.4, 5.5);
            Point expected = new Point(-1.1, 9.0, 3.7);
            //Act
            Point actual = Point.Subtract(point, vector);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Point.Subtract(Vector, Point)"/>
        /// </summary>
        [TestMethod("Static Subtract(Vector,Point)")]
        public void Static_Subtract_Vector_Point()
        {
            // Arrange
            Vector vector = new Vector(2.2, 4.6, 9.2);
            Point point = new Point(3.3, -4.4, 5.5);
            Point expected = new Point(-1.1, 9.0, 3.7);
            //Act
            Point actual = Point.Subtract(vector, point);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static method <see cref="Point.Multiply(Point, double)"/>
        /// </summary>
        [TestMethod("Static Multiply(Point,double)")]
        public void Static_Multiply_Point_double()
        {
            // Arrange
            Point point = new Point(2.2, -4.6, 9.2);
            double factor = 1.5;
            Point expected = new Point(3.3, -6.9, 13.8);
            //Act
            Point actual = Point.Multiply(point, factor);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Point.Multiply(double, Point)"/>
        /// </summary>
        [TestMethod("Static Multiply(double,Point)")]
        public void Static_Multiply_double_Point()
        {
            // Arrange
            double factor = 1.5;
            Point point = new Point(2.2, -4.6, 9.2);
            Point expected = new Point(3.3, -6.9, 13.8);
            //Act
            Point actual = Point.Multiply(factor, point);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Point.Divide(Point, double)"/>
        /// </summary>
        [TestMethod("Static Divide(Point,double)")]
        public void Static_Divide_Point_double()
        {
            // Arrange
            Point point = new Point(2.4, -4.6, 9.2);
            double divisor = 2.0;
            Point expected = new Point(1.2, -2.3, 4.6);
            //Act
            Point actual = Point.Divide(point, divisor);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Hilbert Space ********************/

        /// <summary>
        /// Tests the static method <see cref="Point.DotProduct(Point, Point)"/>.
        /// </summary>
        [TestMethod("Static DotProduct(Point,Point)")]
        public void Static_DotProduct_Point_Point()
        {
            // Arrange
            Point pointA = new Point(2.2, -4.6, 9.2);
            Point pointB= new Point(1.4, -2.6, -5.2);
            double expected = -32.8; 
            //Act
            double actual = Point.DotProduct(pointA, pointB);
            // Assert
            Assert.AreEqual(expected, actual, Settings.AbsolutePrecision);
        }

        #endregion

        #region Static Operator

        /******************** Algebraic Field ********************/

        /// <summary>
        /// Tests the static operator <see cref="Point.operator +(Point, Point)"/>.
        /// </summary>
        [TestMethod("Operator Add(Point,Point)")]
        public void Operator_Add_Point_Point()
        {
            // Arrange
            Point pointA = new Point(1.5, 2.5, 3.5);
            Point pointB = new Point(1.2, 2.4, 3.6);
            Point expected = new Point(2.7, 4.9, 7.1);
            //Act
            Point actual = pointA + pointB;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Point.operator -(Point, Point)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Point,Point)")]
        public void Operator_Subtract_Point_Point()
        {
            // Arrange
            Point pointA = new Point(1.5, 2.5, 3.5);
            Point pointB = new Point(1.2, 2.4, 3.6);
            Point expected = new Point(0.3, 0.1, -0.1);
            //Act
            Point actual = pointA - pointB;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Point.operator -(Point)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Point)")]
        public void Operator_Subtract_Point()
        {
            // Arrange
            Point point = new Point(1.0, -2.0, 3.0);
            Point expected = new Point(-1.0, 2.0, -3.0);
            //Act
            Point actual = - point;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Vector Embedding ********************/

        /// <summary>
        /// Tests the static operator <see cref="Point.operator +(Point, Vector)"/>.
        /// </summary>
        [TestMethod("Operator Add(Point,Vector)")]
        public void Operator_Add_Point_Vector()
        {
            // Arrange
            Point point = new Point(1.5, 2.5, 3.5);
            Vector vector = new Vector(1.2, 2.4, 3.6);
            Point expected = new Point(2.7, 4.9, 7.1);
            //Act
            Point actual = point + vector;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Point.operator -(Point, Vector)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Point,Vector)")]
        public void Operator_Subtract_Point_Vector()
        {
            // Arrange
            Point point = new Point(1.5, 2.5, 3.5);
            Vector vector = new Vector(1.2, 2.4, 3.6);
            Point expected = new Point(0.3, 0.1, -0.1);
            //Act
            Point actual = point - vector;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static operator <see cref="Point.operator *(double, Point)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(double,Point)")]
        public void Operator_Multiply_double_Point()
        {
            // Arrange
            double number = 1.5;
            Point point = new Point(2.2, -4.6, 9.2);
            Point expected = new Point(3.3, -6.9, 13.8);
            //Act
            Point actual = number * point;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Point.operator *(Point, double)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Point,double)")]
        public void Operator_Multiply_Point_double()
        {
            // Arrange
            Point point = new Point(2.2, -4.6, 9.2);
            double number = 1.5;
            Point expected = new Point(3.3, -6.9, 13.8);
            //Act
            Point actual = point * number;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Point.operator /(Point, double)"/>.
        /// </summary>
        [TestMethod("Operator Divide(Point,double)")]
        public void Operator_Divide_Point_double()
        {
            // Arrange
            Point point = new Point(2.4, -4.6, 9.2);
            double number = 2.0;
            Point expected = new Point(1.2, -2.3, 4.6);
            //Act
            Point actual = point / number;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Hilbert Space ********************/

        /// <summary>
        /// Tests the static operator <see cref="Point.operator *(Point, Point)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Point,Point)")]
        public void Operator_Multiply_Point_Point()
        {
            // Arrange
            Point pointA = new Point(2.2, -4.6, 9.2);
            Point pointB = new Point(1.4, -2.6, -5.2);
            double expected = -32.8;
            //Act
            double actual = pointA * pointB;
            // Assert
            Assert.AreEqual(expected, actual, Settings.AbsolutePrecision);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Tests the implicit conversion of a <see cref="Vector"/> into a <see cref="Point"/>.
        /// </summary>
        [TestMethod("Convert FromVector")]
        public void Convert_FromVector()
        {
            // Arrange
            Vector vector = new Vector(1.0, 2.0, -3.0);
            Point expected = new Point(1.0, 2.0, -3.0);
            // Act
            Point actual = vector;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tests the method <see cref="Point.DistanceTo(Point)"/>.
        /// </summary>
        [TestMethod("Method Distance(Point)")]
        public void Distance_Point()
        {
            // Arrange
            Point pointA = new Point(1.0, 3.0, 5.0);
            Point pointB = new Point(2.0, 2.0, 2.0);
            double expected = Math.Sqrt(11.0);
            // Act
            double actual = pointA.DistanceTo(pointB);
            //Assert
            Assert.AreEqual(expected, actual, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the method <see cref="Point.Norm"/>.
        /// </summary>
        [TestMethod("Method Norm()")]
        public void Norm()
        {
            // Arrange
            Point point = new Point(1.0, 3.0, 5.0);
            double expected = Math.Sqrt(35.0);
            // Act
            double actual = point.Norm();
            //Assert
            Assert.AreEqual(expected, actual, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the method <see cref="Point.GetCoordinates"/>.
        /// </summary>
        [TestMethod("Method GetCoordinates()")]
        public void GetCoordinates()
        {
            // Arrange
            Point point = new Point(1.0, 3.0, 5.0);
            double[] expected = new double[3] { 1.0, 3.0, 5.0 };
            // Act
            double[] actual = point.GetCoordinates();
            //Assert
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(expected[i], actual[i], Settings.AbsolutePrecision);
            }
        }


        /// <summary>
        /// Tests the method <see cref="Point.Equals(Point)"/>.
        /// </summary>
        [TestMethod("Method Equals(Point)")]
        public void Equals_Point()
        {
            // Arrange
            Point pointA = new Point(1.0, 3.0, 5.0);
            Point pointB = new Point(1.0, 3.0, 5.0);
            // Act
            bool areEqual = pointA.Equals(pointB);
            // Assert
            Assert.IsTrue(areEqual);
        }

        #endregion


        #region Explicit Implementations

        /******************** IDotProduct<Point, double> ********************/

        /// <summary>
        /// Tests the <see cref="Alg_Mes.IDotProduct{TSelf, TValue}.DotProduct(TSelf)"/> method of <see cref="Point"/>.
        /// </summary>
        [TestMethod("AsIDotProduct<double,Point> DotProduct(Point)")]
        public void AsIDotProduct_DotProduct_Point()
        {
            // Arrange
            Point pointA = new Point(2.2, -4.6, 9.2);
            Point pointB = new Point(1.4, -2.6, -5.2);
            double expected = -32.8;
            //Act
            Alg_Mes.IDotProduct<Point, double> representation = (Alg_Mes.IDotProduct<Point, double>)pointA;
            double dotProduct = representation.DotProduct(pointB);
            // Assert
            Assert.AreEqual(dotProduct, expected, Settings.AbsolutePrecision);
        }

        #endregion


        /* To Do : 
         * public void Static_MobiusInversion_Point_Point_double()
         */
    }
}
