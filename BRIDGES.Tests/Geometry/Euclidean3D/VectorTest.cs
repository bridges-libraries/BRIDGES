using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.Geometry.Euclidean3D;

using Alg_Meas = BRIDGES.Algebra.Measure;


namespace BRIDGES.Tests.Geometry.Euclidean3D
{
    /// <summary>
    /// Class testing the members of the <see cref="Vector"/> structure.
    /// </summary>
    [TestClass]
    public class VectorTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="Vector"/> is not reference type.
        /// </summary>
        [TestMethod("Behavior IsNotReference")]
        public void IsNotReference()
        {
            // Arrange
            Vector vectorA = new Vector(1.0, 2.0, 3.0);
            Vector vectorB = new Vector(4.0, 5.0, 6.0);
            //Act
            vectorA = vectorB;
            // Assert
            Assert.IsTrue(vectorA.Equals(vectorB));
            Assert.AreNotSame(vectorA, vectorB);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the <see cref="Vector.Dimension"/> property.
        /// </summary>
        [TestMethod("Property Dimension")]
        public void Dimension()
        {
            // Arrange
            Vector vector = new Vector(1.5, 1.7, 2.1);
            //Act
            int dimension = vector.Dimension;
            // Assert
            Assert.AreEqual(3, dimension);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Vector"/> from its three coordinates,
        /// and the <see cref="Vector.X"/>, <see cref="Vector.Y"/>, <see cref="Vector.Z"/> properties.
        /// </summary>
        [TestMethod("Property X & Y & Z")]
        public void XAndYAndZ()
        {
            // Arrange
            Vector vector = new Vector(1.5, 1.7, 2.1);
            double[] expected = new double[] { 1.5, 1.7, 2.1 };
            //Act
            double x = vector.X;
            double y = vector.Y;
            double z = vector.Z;
            // Assert
            Assert.AreEqual(expected[0], x, Settings.AbsolutePrecision);
            Assert.AreEqual(expected[1], y, Settings.AbsolutePrecision);
            Assert.AreEqual(expected[2], z, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the <see cref="Vector"/> indexer property.
        /// </summary>
        [TestMethod("Property this[int]")]
        public void Indexer()
        {
            // Arrange
            Vector vector = new Vector(1.5, 1.7, 2.1);
            double[] expected = new double[] { 1.5, 1.7, 2.1 };
            //Act
            double x = vector[0];
            double y = vector[1];
            double z = vector[2];
            // Assert
            Assert.AreEqual(expected[0], x, Settings.AbsolutePrecision);
            Assert.AreEqual(expected[1], y, Settings.AbsolutePrecision);
            Assert.AreEqual(expected[2], z, Settings.AbsolutePrecision);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Tests the initialisation of the <see cref="Vector"/> from its cooridnates.
        /// </summary>
        [TestMethod("Constructor(double[])")]
        public void Constructor_doubleArray()
        {
            // Arrange
            bool throwsException = false;
            Vector expected = new Vector(2.0, 5.0, 8.0);
            // Act
            Vector actual = new Vector(new double[3] { 2.0, 5.0, 8.0 });
            try { Vector otherVector = new Vector(new double[] { 2.0, 5.0, 8.0, 11.0 }); }
            catch (ArgumentOutOfRangeException) { throwsException = true; }
            // Assert
            Assert.IsTrue(expected.Equals(actual));
            Assert.IsTrue(throwsException);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Vector"/> from another <see cref="Vector"/>.
        /// </summary>
        [TestMethod("Constructor(Vector)")]
        public void Constructor_Vector()
        {
            // Arrange
            Vector expected = new Vector(2.0, 5.0, 8.0);
            // Act
            Vector actual = new Vector(expected);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Vector"/> from a start and end <see cref="Point"/>.
        /// </summary>
        [TestMethod("Constructor(Point,Point)")]
        public void Constructor_Point_Point()
        {
            // Arrange
            Point start = new Point(2.0, 4.0, 6.0);
            Point end = new Point(4.0, 9.0, 14.0);
            Vector expected = new Vector(2.0, 5.0, 8.0);
            // Act
            Vector vector = new Vector(start, end);
            // Assert
            Assert.IsTrue(vector.Equals(expected));
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Tests the static property <see cref="Vector.Zero"/>.
        /// </summary>
        [TestMethod("Static Zero")]
        public void Static_Zero()
        {
            // Arrange
            Vector expected = new Vector(0.0, 0.0, 0.0);
            // Act
            Vector actual = Vector.Zero;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static property <see cref="Vector.WorldX"/>.
        /// </summary>
        [TestMethod("Static WorldX")]
        public void Static_WorldX()
        {
            // Arrange
            Vector expected = new Vector(1.0, 0.0, 0.0);
            // Act
            Vector actual = Vector.WorldX;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static property <see cref="Vector.WorldY"/>.
        /// </summary>
        [TestMethod("Static WorldY")]
        public void Static_WorldY()
        {
            // Arrange
            Vector expected = new Vector(0.0, 1.0, 0.0);
            // Act
            Vector actual = Vector.WorldY;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static property <see cref="Vector.WorldZ"/>.
        /// </summary>
        [TestMethod("Static WorldZ")]
        public void Static_WorldZ()
        {
            // Arrange
            Vector expected = new Vector(0.0, 0.0, 1.0);
            // Act
            Vector vector = Vector.WorldZ;
            // Assert
            Assert.IsTrue(expected.Equals(vector));
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Tests the static method <see cref="Vector.CrossProduct(Vector, Vector)"/>.
        /// </summary>
        [TestMethod("Static CrossProduct(Vector,Vector)")]
        public void Static_CrossProduct_Vector_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.0, 2.0, 3.0);
            Vector vectorB = new Vector(1.5, 3.5, 5.0);
            Vector expected = new Vector(-0.5, -0.5, 0.5);
            //Act
            Vector actual = Vector.CrossProduct(vectorA, vectorB);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static method <see cref="Vector.AreParallel(Vector, Vector)"/>.
        /// </summary>
        [TestMethod("Static AreParallel(Vector, Vector)")]
        [DataRow(new double[] { 1.5, 3.5, 5.0 }, false, DisplayName = "False")]
        [DataRow(new double[] { 1.5, 3.0, 4.5 }, true, DisplayName = "True")]
        public void Static_AreParallel_Vector_Vector(double[] coordinates, bool expected)
        {
            // Arrange
            Vector vectorA = new Vector(1.0, 2.0, 3.0);
            Vector vectorB = new Vector(coordinates);
            //Act
            bool actual = Vector.AreParallel(vectorA, vectorB);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the static method <see cref="Vector.AreOrthogonal(Vector, Vector)"/>.
        /// </summary>
        [TestMethod("Static AreOrthogonal(Vector, Vector)")]
        [DataRow(new double[] { 1.5, 3.5, 5.0 }, false, DisplayName = "False")]
        [DataRow(new double[] { 4.0, -2.0, 0.0 }, true, DisplayName = "True")]
        public void Static_AreOrthogonal_Vector_Vector(double[] coordinates, bool expected)
        {
            // Arrange
            Vector vectorA = new Vector(1.0, 2.0, 3.0);
            Vector vectorB = new Vector(coordinates);
            //Act
            bool actual = Vector.AreOrthogonal(vectorA, vectorB);
            // Assert
            Assert.AreEqual(expected, actual);
        }


        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Tests the static method <see cref="Vector.Add(Vector, Vector)"/>.
        /// </summary>
        [TestMethod("Static Add(Vector,Vector)")]
        public void Static_Add_Vector_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.5, 2.5, 3.5);
            Vector vectorB = new Vector(1.2, 2.4, 3.6);
            Vector expected = new Vector(2.7, 4.9, 7.1);
            //Act
            Vector actual = Vector.Add(vectorA, vectorB);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Vector.Subtract(Vector, Vector)"/>.
        /// </summary>
        [TestMethod("Static Subtract(Vector,Vector)")]
        public void Static_Substract_Vector_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.5, 2.5, 3.5);
            Vector vectorB = new Vector(1.2, 2.4, 3.6);
            Vector expected = new Vector(0.3, 0.1, -0.1);
            //Act
            Vector actual = Vector.Subtract(vectorA, vectorB);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Embedding : Point ********************/

        /// <summary>
        /// Tests the static method <see cref="Vector.Add(Vector, Point)"/>.
        /// </summary>
        [TestMethod("Static Add(Vector,Point)")]
        public void Static_Add_Vector_Point()
        {
            // Arrange
            Vector vector = new Vector(1.5, 2.5, 3.5);
            Point point = new Vector(1.2, 2.4, 3.6);
            Vector expected = new Vector(2.7, 4.9, 7.1);
            //Act
            Vector actual = Vector.Add(vector, point);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Vector.Add(Point, Vector)"/>.
        /// </summary>
        [TestMethod("Static Add(Point,Vector)")]
        public void Static_Add_Point_Vector()
        {
            // Arrange
            Point point = new Point(1.5, 2.5, 3.5);
            Vector vector = new Vector(1.2, 2.4, 3.6);
            Vector expected = new Vector(2.7, 4.9, 7.1);
            //Act
            Vector actual = Vector.Add(point, vector);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static method <see cref="Vector.Subtract(Vector, Point)"/>.
        /// </summary>
        [TestMethod("Static Subtract(Vector,Point)")]
        public void Static_Substract_Vector_Point()
        {
            // Arrange
            Vector vector = new Vector(1.5, 2.5, 3.5);
            Point point = new Point(1.2, 2.4, 3.6);
            Vector expected = new Vector(0.3, 0.1, -0.1);
            //Act
            Vector actual = Vector.Subtract(vector, point);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Vector.Subtract(Point, Vector)"/>.
        /// </summary>
        [TestMethod("Static Subtract(Point,Vector)")]
        public void Static_Substract_Point_Vector()
        {
            // Arrange
            Point point = new Point(1.5, 2.5, 3.5);
            Vector vector = new Vector(1.2, 2.4, 3.6);
            Vector expected = new Vector(0.3, 0.1, -0.1);
            //Act
            Vector actual = Vector.Subtract(point, vector);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static method <see cref="Vector.Multiply(Vector, double)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Vector, double)")]
        public void Static_Multiply_Vector_double()
        {
            // Arrange
            Vector vector = new Vector(2.2, -4.6, 9.2);
            double factor = 1.5;
            Vector expected = new Vector(3.3, -6.9, 13.8);
            //Act
            Vector actual = Vector.Multiply(vector, factor);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Vector.Multiply(double, Vector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(double,Vector)")]
        public void Static_Multiply_double_Vector()
        {
            // Arrange
            double factor = 1.5;
            Vector vector = new Vector(2.2, -4.6, 9.2);
            Vector expected = new Vector(3.3, -6.9, 13.8);
            //Act
            Vector actual = Vector.Multiply(factor, vector);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static method <see cref="Vector.Divide(Vector, double)"/>.
        /// </summary>
        [TestMethod("Static Divide(Vector,double)")]
        public void Static_Divide_Vector_double()
        {
            // Arrange
            Vector vector = new Vector(2.4, -4.6, 9.2);
            double divisor = 2.0;
            Vector expected = new Vector(1.2, -2.3, 4.6);
            //Act
            Vector actual = Vector.Divide(vector, divisor);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Hilbert Space ********************/

        /// <summary>
        /// Tests the static method <see cref="Vector.DotProduct(Vector, Vector)"/>.
        /// </summary>
        [TestMethod("Static DotProduct(Vector,Vector)")]
        public void Static_DotProduct_Vector_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(2.2, -4.6, 9.2);
            Vector vectorB = new Vector(1.4, -2.6, -5.2);
            double expected = -32.8;
            //Act
            double actual = Vector.DotProduct(vectorA, vectorB);
            // Assert
            Assert.AreEqual(expected, actual, Settings.AbsolutePrecision);
        }


        /// <summary>
        /// Tests the static method <see cref="Vector.AngleBetween(Vector, Vector)"/>.
        /// </summary>
        [TestMethod("Static AngleBetween(Vector,Vector)")]
        public void Static_AngleBetween_Vector_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.0, 0.0, 0.0);
            Vector vectorB = new Vector(1.0, 1.0, 0.0);
            double expected = Math.PI / 4d;
            //Act
            double actual = Vector.AngleBetween(vectorA, vectorB);
            // Assert
            Assert.AreEqual(expected, actual, Settings.AngularPrecision);
        }


        #endregion

        #region Static Operator

        /******************** Algebraic Field ********************/

        /// <summary>
        /// Tests the static operator <see cref="Vector.operator +(Vector, Vector)"/>.
        /// </summary>
        [TestMethod("Operator Add(Vector,Vector)")]
        public void Operator_Add_Vector_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.5, 2.5, 3.5);
            Vector vectorB = new Vector(1.2, 2.4, 3.6);
            Vector expected = new Vector(2.7, 4.9, 7.1);
            //Act
            Vector actual = vectorA + vectorB;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Vector.operator -(Vector, Vector)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Vector,Vector)")]
        public void Operator_Subtract_Vector_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.5, 2.5, 3.5);
            Vector vectorB = new Vector(1.2, 2.4, 3.6);
            Vector expected = new Vector(0.3, 0.1, -0.1);
            //Act
            Vector actual = vectorA - vectorB;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Vector.operator -(Vector)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Vector)")]
        public void Operator_Subtract_Vector()
        {
            // Arrange
            Vector vector = new Vector(1.0, -2.0, 3.0);
            Vector expected = new Vector(-1.0, 2.0, -3.0);
            //Act
            Vector actual = -vector;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static operator <see cref="Vector.operator *(Vector, double)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(double,Vector)")]
        public void Operator_Multiply_Vector_double()
        {
            // Arrange
            Vector vector = new Vector(2.2, -4.6, 9.2);
            double factor = 1.5;
            Vector expected = new Vector(3.3, -6.9, 13.8);
            //Act
            Vector actual = vector * factor;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Vector.operator *(double, Vector)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(double,Vector)")]
        public void Operator_Multiply_double_Vector()
        {
            // Arrange
            double factor = 1.5;
            Vector vector = new Vector(2.2, -4.6, 9.2);
            Vector expected = new Vector(3.3, -6.9, 13.8);
            //Act
            Vector actual = factor * vector;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static operator <see cref="Vector.operator /(Vector, double)"/>.
        /// </summary>
        [TestMethod("Operator Divide(Vector,double)")]
        public void Operator_Divide_Vector_double()
        {
            // Arrange
            Vector vector = new Vector(2.4, -4.6, 9.2);
            double divisor = 2.0;
            Vector expected = new Vector(1.2, -2.3, 4.6);
            //Act
            Vector actual = vector / divisor;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Hilbert Space ********************/

        /// <summary>
        /// Tests the static operator <see cref="Vector.operator *(Vector, Vector)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Vector,Vector)")]
        public void Operator_Multiply_Vector_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(2.2, -4.6, 9.2);
            Vector vectorB = new Vector(1.4, -2.6, -5.2);
            double expected = -32.8;
            //Act
            double actual = vectorA * vectorB;
            // Assert
            Assert.AreEqual(expected, actual, Settings.AbsolutePrecision);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Tests the implicit conversion of a <see cref="Point"/> into a <see cref="Vector"/>.
        /// </summary>
        [TestMethod("Convert FromPoint")]
        public void Convert_FromPoint()
        {
            // Arrange
            Point point = new Point(1.0, 2.0, -3.0);
            Vector expected = new Vector(1.0, 2.0, -3.0);
            // Act
            Vector vector = point;
            // Assert
            Assert.IsTrue(vector.Equals(expected));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tests the method <see cref="Vector.GetCoordinates"/>.
        /// </summary>
        [TestMethod("Method GetCoordinates()")]
        public void GetCoordinates()
        {
            // Arrange
            Vector vector = new Vector(1.0, 3.0, 5.0);
            double[] expected = new double[3] { 1.0, 3.0, 5.0 };
            // Act
            double[] actual = vector.GetCoordinates();
            //Assert
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(expected[i], actual[i], Settings.AbsolutePrecision);
            }
        }


        /// <summary>
        /// Tests the method <see cref="Vector.Length"/>.
        /// </summary>
        [TestMethod("Method Length()")]
        public void Length()
        {
            // Arrange
            Vector vector = new Vector(1.0, -3.0, 5.0);
            double expected = Math.Sqrt(35.0);
            // Act
            double actual = vector.Length();
            //Assert
            Assert.AreEqual(expected, actual, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the method <see cref="Vector.SquaredLength"/>.
        /// </summary>
        [TestMethod("Method SquaredLength()")]
        public void SquaredLength()
        {
            // Arrange
            Vector vector = new Vector(1.0, 3.0, 5.0);
            double expected = 35.0;
            // Act
            double actual = vector.SquaredLength();
            //Assert
            Assert.AreEqual(expected, actual, Settings.AbsolutePrecision);
        }


        /// <summary>
        /// Tests the method <see cref="Vector.Unitise"/>.
        /// </summary>
        [TestMethod("Method Unitise()")]
        public void Unitise()
        {
            // Arrange
            Vector vector = new Vector(1.0, -5.0, 2.5);
            // Act
            vector.Unitise();
            double actual = vector.Length();
            // Assert
            Assert.AreEqual(1d, actual, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the method <see cref="Vector.IsUnit"/>.
        /// </summary>
        [TestMethod(displayName: "Method IsUnit()")]
        [DataRow(new double[] { 1.0, -5.0, 2.5 }, false, DisplayName = "False")]
        [DataRow(new double[] { 3.0 / 5.0, 4.0 / 5.0, 0.0 }, true, DisplayName = "True")]
        public void IsUnit(double[] coordinates, bool expected)
        {
            // Arrange
            Vector vector = new Vector(coordinates);
            // Act
            bool actual  = vector.IsUnit();
            // Assert
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        /// Tests the method <see cref="Vector.AngleWith(Vector)"/>.
        /// </summary>
        [TestMethod("Method AngleWith(Vector)")]
        public void AngleWith_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.5, 3.0, 7.5);
            Vector vectorB = new Vector(4.0, 5.0, 6.0);
            double expected = Math.Acos(66 / (Math.Sqrt(67.5) * Math.Sqrt(77)));
            //Act
            double actual = vectorA.AngleWith(vectorB);
            // Assert
            Assert.AreEqual(expected, actual, Settings.AngularPrecision);
        }


        /// <summary>
        /// Tests the method <see cref="Vector.Equals(Vector)"/>.
        /// </summary>
        [TestMethod("Method Equals(Vector)")]
        public void Equals_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.0, 3.0, 5.0);
            Vector vectorB = new Vector(1.0, 3.0, 5.0);
            // Act
            bool areEqual = vectorA.Equals(vectorB);
            // Assert
            Assert.IsTrue(areEqual);
        }

        #endregion


        #region Explicit Implementations

        /******************** IDotProduct<Point, double> ********************/

        /// <summary>
        /// Tests the <see cref="Alg_Meas.IMetric{T}.DistanceTo(T)"/> method
        /// computing the distance between the current <see cref="Vector"/> with another <see cref="Vector"/>.
        /// </summary>
        [TestMethod("AsIMetric<Vector> Distance(Vector)")]
        public void AsIMetric_Distance_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.0, 3.0, 5.0);
            Vector vectorB = new Vector(2.0, 2.0, 2.0);
            double expected = Math.Sqrt(11.0);
            // Act
            Alg_Meas.IMetric<Vector> representation = (Alg_Meas.IMetric<Vector>)vectorA;
            double actual = representation.DistanceTo(vectorB);
            //Assert
            Assert.AreEqual(expected, actual, Settings.AbsolutePrecision);
        }


        /// <summary>
        /// Tests the <see cref="Alg_Meas.INorm{T}.Norm"/> method computing the norm of the current <see cref="Vector"/>.
        /// </summary>
        [TestMethod("AsINorm<Vector> Norm()")]
        public void Norm()
        {
            // Arrange
            Vector vector = new Vector(1.0, 3.0, 5.0);
            double expected = Math.Sqrt(35.0);
            // Act
            Alg_Meas.INorm<Vector> representation = (Alg_Meas.INorm<Vector>)vector;
            double actual = representation.Norm();
            //Assert
            Assert.AreEqual(expected, actual, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the <see cref="Alg_Meas.IDotProduct{TSelf, TValue}.DotProduct(TSelf)"/> method 
        /// computing the dot product of the current <see cref="Vector"/> with another <see cref="Vector"/>.
        /// </summary>
        [TestMethod("AsIDotProduct<Vector,double> DotProduct(Vector)")]
        public void AsIDotProduct_DotProduct_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(2.2, -4.6, 9.2);
            Vector vectorB = new Vector(1.4, -2.6, -5.2);
            double expected = -32.8;
            //Act
            Alg_Meas.IDotProduct<Vector, double> dotProductable = (Alg_Meas.IDotProduct<Vector, double>)vectorA;
            double actual = dotProductable.DotProduct(vectorB);
            // Assert
            Assert.AreEqual(expected, actual, Settings.AbsolutePrecision);
        }

        #endregion
    }
}
