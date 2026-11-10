using System;

using Xunit;

using Euc3D = BRIDGES.Geometry.Euclidean3D;


namespace BRIDGES.Tests.Geometry.Euclidean3D
{
    /// <summary>
    /// Tests the members of the <see cref=Euc3D.Vector"/> class.
    /// </summary>
    public class Vector
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="Euc3D.Vector"/> is not reference type.
        /// </summary>
        [Fact(DisplayName = "Is Not Reference")]
        public void IsNotReference()
        {
            // Arrange
            Euc3D.Vector vectorA = new Euc3D.Vector(1.0, 2.0, 3.0);
            Euc3D.Vector vectorB = new Euc3D.Vector(4.0, 5.0, 6.0);

            //Act
            vectorA = vectorB;

            // Assert
            Assert.Equal(vectorA, vectorB);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the initialisation of the <see cref="Euc3D.Vector"/> from its three coordinates,
        /// and the <see cref="Euc3D.Vector.X"/>, <see cref="Euc3D.Vector.Y"/>, <see cref="Euc3D.Vector.Z"/> properties.
        /// </summary>
        [Fact(DisplayName = "Prop. X & Y & Z")]
        public void Property__XAndYAndZ()
        {
            // Arrange
            Euc3D.Vector vector = new Euc3D.Vector(1.5, 1.7, 2.1);
            double[] expected = new double[] { 1.5, 1.7, 2.1 };
            //Act
            double x = vector.X;
            double y = vector.Y;
            double z = vector.Z;
            // Assert
            Assert.Equal(expected[0], x);
            Assert.Equal(expected[1], y);
            Assert.Equal(expected[2], z);
        }

        /// <summary>
        /// Tests the <see cref="Euc3D.Vector"/> indexer property.
        /// </summary>
        [Fact(DisplayName = "Property this[int]")]
        public void Property__Indexer()
        {
            // Arrange
            Euc3D.Vector vector = new Euc3D.Vector(1.5, 1.7, 2.1);
            double[] expected = new double[] { 1.5, 1.7, 2.1 };
            //Act
            double x = vector[0];
            double y = vector[1];
            double z = vector[2];
            // Assert
            Assert.Equal(expected[0], x);
            Assert.Equal(expected[1], y);
            Assert.Equal(expected[2], z);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Tests the initialisation of the <see cref="Euc3D.Vector"/> from its X, Y and Z coordinates.
        /// </summary>
        [Fact(DisplayName = "Constructor(X,Y,Z)")]
        public void Constructor__Double_Double_Double()
        {
            // Arrange
            Euc3D.Vector expected = new Euc3D.Vector(new double[3] { 2.0, 5.0, 8.0 });

            // Act
            Euc3D.Vector actual = new Euc3D.Vector(2.0, 5.0, 8.0);

            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Euc3D.Vector"/> from its cooridnates.
        /// </summary>
        [Fact(DisplayName = "Constructor(double[])")]
        public void Constructor__ArrayOfDouble()
        {
            // Arrange
            bool throwsException = false;
            
            Euc3D.Vector expected = new Euc3D.Vector(2.0, 5.0, 8.0);
            
            // Act
            Euc3D.Vector actual = new Euc3D.Vector(new double[3] { 2.0, 5.0, 8.0 });
            
            try { Euc3D.Vector otherVector = new Euc3D.Vector(new double[] { 2.0, 5.0, 8.0, 11.0 }); }
            catch (ArgumentOutOfRangeException) { throwsException = true; }
            
            // Assert
            Assert.Equal(expected, actual);
            Assert.True(throwsException);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Euc3D.Vector"/> from another <see cref="Euc3D.Vector"/>.
        /// </summary>
        [Fact(DisplayName = "Constructor(Vector)")]
        public void Constructor__Vector()
        {
            // Arrange
            Euc3D.Vector expected = new Euc3D.Vector(2.0, 5.0, 8.0);
            
            // Act
            Euc3D.Vector actual = new Euc3D.Vector(expected);
            
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Euc3D.Vector"/> from a start and end <see cref="Euc3D.Point"/>.
        /// </summary>
        [Fact(DisplayName = "Constructor(Point,Point)")]
        public void Constructor__Point_Point()
        {
            // Arrange
            Euc3D.Point start = new Euc3D.Point(2.0, 4.0, 6.0);
            Euc3D.Point end = new Euc3D.Point(4.0, 9.0, 14.0);
            
            Euc3D.Vector expected = new Euc3D.Vector(2.0, 5.0, 8.0);
            
            // Act
            Euc3D.Vector vector = new Euc3D.Vector(start, end);
            
            // Assert
            Assert.Equal(vector, expected);
        }

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Tests the static property <see cref="Euc3D.Vector.Zero"/>.
        /// </summary>
        [Fact(DisplayName = "Static Zero")]
        public void Static__Zero()
        {
            // Arrange
            Euc3D.Vector expected = new Euc3D.Vector(0.0, 0.0, 0.0);
            // Act
            Euc3D.Vector actual = Euc3D.Vector.Zero;
            // Assert
            Assert.Equal(expected, actual);
        }


        /// <summary>
        /// Tests the static property <see cref="Euc3D.Vector.WorldX"/>.
        /// </summary>
        [Fact(DisplayName = "Static WorldX")]
        public void Static__WorldX()
        {
            // Arrange
            Euc3D.Vector expected = new Euc3D.Vector(1.0, 0.0, 0.0);
            
            // Act
            Euc3D.Vector actual = Euc3D.Vector.WorldX;
            
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the static property <see cref="Euc3D.Vector.WorldY"/>.
        /// </summary>
        [Fact(DisplayName = "Static WorldY")]
        public void Static__WorldY()
        {
            // Arrange
            Euc3D.Vector expected = new Euc3D.Vector(0.0, 1.0, 0.0);
            
            // Act
            Euc3D.Vector actual = Euc3D.Vector.WorldY;
            
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the static property <see cref="Euc3D.Vector.WorldZ"/>.
        /// </summary>
        [Fact(DisplayName = "Static WorldZ")]
        public void Static__WorldZ()
        {
            // Arrange
            Euc3D.Vector expected = new Euc3D.Vector(0.0, 0.0, 1.0);
            
            // Act
            Euc3D.Vector vector = Euc3D.Vector.WorldZ;
            
            // Assert
            Assert.Equal(expected, vector);
        }

        #endregion

        #region Public Static Methods

        //     -----     Internal Binary Operation of Vectors : Additive     -----     //

        /// <summary>
        /// Tests the static method <see cref="Euc3D.Vector.Add(Euc3D.Vector, Euc3D.Vector)"/>.
        /// </summary>
        [Fact(DisplayName = "Static Add(Vector,Vector)")]
        public void Static__Add__Vector_Vector()
        {
            // Arrange
            Euc3D.Vector vectorA = new Euc3D.Vector(1.5, 2.5, 3.5);
            Euc3D.Vector vectorB = new Euc3D.Vector(1.2, 2.4, 3.6);

            Euc3D.Vector expected = new Euc3D.Vector(2.7, 4.9, 7.1);
            
            //Act
            Euc3D.Vector actual = Euc3D.Vector.Add(vectorA, vectorB);
            
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the static method <see cref="Euc3D.Vector.Subtract(Euc3D.Vector, Euc3D.Vector)"/>.
        /// </summary>
        [Fact(DisplayName = "Static Subtract(Vector,Vector)")]
        public void Static__Substract__Vector_Vector()
        {
            // Arrange
            Euc3D.Vector vectorA = new Euc3D.Vector(1.5, 2.5, 3.5);
            Euc3D.Vector vectorB = new Euc3D.Vector(1.2, 2.4, 3.6);

            Euc3D.Vector expected = new Euc3D.Vector(0.3, 0.1, -0.1);
            
            //Act
            Euc3D.Vector actual = Euc3D.Vector.Subtract(vectorA, vectorB);
            
            // Assert
            Assert.Equal(expected, actual);
        }


        /// <summary>
        /// Tests the static method <see cref="Euc3D.Point.Opposite(Euc3D.Point)"/>.
        /// </summary>
        [Fact(DisplayName = "Static Opposite(Point)")]
        public void Static__Opposite__Point()
        {
            // Arrange
            Euc3D.Point point = new Euc3D.Point(1.5, 2.5, 3.5);

            Euc3D.Point expected = new Euc3D.Point(-1.5, -2.5, -3.5);

            //Act
            Euc3D.Point actual = Euc3D.Point.Opposite(point);

            // Assert
            Assert.Equal(expected, actual);
        }


        //     -----     Group Action of Scalars on Vectors : Multiplicative     -----     //

        /// <summary>
        /// Tests the static method <see cref="Euc3D.Vector.Multiply(Euc3D.Vector, double)"/>.
        /// </summary>
        [Fact(DisplayName = "Static Multiply(Vector, Double)")]
        public void Static__Multiply__Vector_Double()
        {
            // Arrange
            Euc3D.Vector vector = new Euc3D.Vector(2.2, -4.6, 9.2);
            double factor = 1.5;

            Euc3D.Vector expected = new Euc3D.Vector(3.3, -6.9, 13.8);
            
            //Act
            Euc3D.Vector actual = Euc3D.Vector.Multiply(vector, factor);
            
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the static method <see cref="Euc3D.Vector.Multiply(double, Euc3D.Vector)"/>.
        /// </summary>
        [Fact(DisplayName = "Static Multiply(Double, Vector)")]
        public void Static__Multiply__Double_Vector()
        {
            // Arrange
            double factor = 1.5;
            Euc3D.Vector vector = new Euc3D.Vector(2.2, -4.6, 9.2);

            Euc3D.Vector expected = new Euc3D.Vector(3.3, -6.9, 13.8);
            
            //Act
            Euc3D.Vector actual = Euc3D.Vector.Multiply(factor, vector);
            
            // Assert
            Assert.Equal(expected, actual);
        }


        /// <summary>
        /// Tests the static method <see cref="Euc3D.Vector.Divide(Euc3D.Vector, double)"/>.
        /// </summary>
        [Fact(DisplayName = "Static Divide(Vector, Double)")]
        public void Static__Divide__Vector_Double()
        {
            // Arrange
            Euc3D.Vector vector = new Euc3D.Vector(2.4, -4.6, 9.2);
            double divisor = 2.0;
            
            Euc3D.Vector expected = new Euc3D.Vector(1.2, -2.3, 4.6);
            
            //Act
            Euc3D.Vector actual = Euc3D.Vector.Divide(vector, divisor);
            
            // Assert
            Assert.Equal(expected, actual);
        }


        //     -----     Scalar Product     -----     //

        /// <summary>
        /// Tests the static method <see cref="Euc3D.Vector.DotProduct(Euc3D.Vector, Euc3D.Vector)"/>.
        /// </summary>
        [Fact(DisplayName = "Static DotProduct(Vector,Vector)")]
        public void Static__DotProduct__Vector_Vector()
        {
            // Arrange
            Euc3D.Vector vectorA = new Euc3D.Vector(2.2, -4.6, 9.2);
            Euc3D.Vector vectorB = new Euc3D.Vector(1.4, -2.6, -5.2);

            double expected = -32.8;
            
            //Act
            double actual = Euc3D.Vector.DotProduct(vectorA, vectorB);
            
            // Assert
            Assert.Equal(expected, actual, Settings.AbsolutePrecision);
        }


        /// <summary>
        /// Tests the static method <see cref="Euc3D.Vector.AngleBetween(Euc3D.Vector, Euc3D.Vector)"/>.
        /// </summary>
        [Fact(DisplayName = "Static AngleBetween(Vector,Vector)")]
        public void Static__AngleBetween__Vector_Vector()
        {
            // Arrange
            Euc3D.Vector vectorA = new Euc3D.Vector(1.0, 0.0, 0.0);
            Euc3D.Vector vectorB = new Euc3D.Vector(1.0, 1.0, 0.0);

            double expected = Math.PI / 4d;
            
            //Act
            double actual = Euc3D.Vector.AngleBetween(vectorA, vectorB);
            
            // Assert
            Assert.Equal(expected, actual, Settings.AngularPrecision);
        }


        /// <summary>
        /// Tests the static method <see cref="Euc3D.Vector.AreParallel(Euc3D.Vector, Euc3D.Vector)"/>.
        /// </summary>
        [Theory(DisplayName = "Static AreParallel(Vector, Vector)")]
        [InlineData(new double[] { 1.5, 3.5, 5.0 }, false)]
        [InlineData(new double[] { 1.5, 3.0, 4.5 }, true)]
        public void Static__AreParallel__Vector_Vector(double[] coordinates, bool expected)
        {
            // Arrange
            Euc3D.Vector vectorA = new Euc3D.Vector(1.0, 2.0, 3.0);
            Euc3D.Vector vectorB = new Euc3D.Vector(coordinates);
            
            //Act
            bool actual = Euc3D.Vector.AreParallel(vectorA, vectorB);
            
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the static method <see cref="Euc3D.Vector.AreOrthogonal(Euc3D.Vector, Euc3D.Vector)"/>.
        /// </summary>
        [Theory(DisplayName = "Static AreOrthogonal(Vector, Vector)")]
        [InlineData(new double[] { 1.5, 3.5, 5.0 }, false)]
        [InlineData(new double[] { 4.0, -2.0, 0.0 }, true)]
        public void Static__AreOrthogonal__Vector_Vector(double[] coordinates, bool expected)
        {
            // Arrange
            Euc3D.Vector vectorA = new Euc3D.Vector(1.0, 2.0, 3.0);
            Euc3D.Vector vectorB = new Euc3D.Vector(coordinates);
            
            //Act
            bool actual = Euc3D.Vector.AreOrthogonal(vectorA, vectorB);
            
            // Assert
            Assert.Equal(expected, actual);
        }


        //     -----     Cross Product     -----     //

        /// <summary>
        /// Tests the static method <see cref="Euc3D.Vector.CrossProduct(Euc3D.Vector, Euc3D.Vector)"/>.
        /// </summary>
        [Fact(DisplayName = "Static CrossProduct(Vector,Vector)")]
        public void Static__CrossProduct__Vector_Vector()
        {
            // Arrange
            Euc3D.Vector vectorA = new Euc3D.Vector(1.0, 2.0, 3.0);
            Euc3D.Vector vectorB = new Euc3D.Vector(1.5, 3.5, 5.0);
            
            Euc3D.Vector expected = new Euc3D.Vector(-0.5, -0.5, 0.5);
            
            //Act
            Euc3D.Vector actual = Euc3D.Vector.CrossProduct(vectorA, vectorB);
            
            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Public Operators

        //     -----     Internal Binary Operation of Vectors : Additive     -----     //

        /// <summary>
        /// Tests the static operator <see cref="Euc3D.Vector.operator +(Euc3D.Vector, Euc3D.Vector)"/>.
        /// </summary>
        [Fact(DisplayName = "Op. + (Vector,Vector)")]
        public void Operator__Add__Vector_Vector()
        {
            // Arrange
            Euc3D.Vector vectorA = new Euc3D.Vector(1.5, 2.5, 3.5);
            Euc3D.Vector vectorB = new Euc3D.Vector(1.2, 2.4, 3.6);
            
            Euc3D.Vector expected = new Euc3D.Vector(2.7, 4.9, 7.1);
            
            //Act
            Euc3D.Vector actual = vectorA + vectorB;
            
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the static operator <see cref="Euc3D.Vector.operator -(Euc3D.Vector, Euc3D.Vector)"/>.
        /// </summary>
        [Fact(DisplayName = "Op. - (Vector,Vector)")]
        public void Operator__Subtract__Vector_Vector()
        {
            // Arrange
            Euc3D.Vector vectorA = new Euc3D.Vector(1.5, 2.5, 3.5);
            Euc3D.Vector vectorB = new Euc3D.Vector(1.2, 2.4, 3.6);
            
            Euc3D.Vector expected = new Euc3D.Vector(0.3, 0.1, -0.1);
            
            //Act
            Euc3D.Vector actual = vectorA - vectorB;
            
            // Assert
            Assert.Equal(expected, actual);
        }


        /// <summary>
        /// Tests the static operator <see cref="Euc3D.Vector.operator -(Euc3D.Vector)"/>.
        /// </summary>
        [Fact(DisplayName = "Op. - (Vector)")]
        public void Operator__Opposite__Vector()
        {
            // Arrange
            Euc3D.Vector vector = new Euc3D.Vector(1.0, -2.0, 3.0);
            
            Euc3D.Vector expected = new Euc3D.Vector(-1.0, 2.0, -3.0);
            
            //Act
            Euc3D.Vector actual = -vector;
            
            // Assert
            Assert.Equal(expected, actual);
        }


        //     -----     Group Action of Scalars on Vectors : Multiplicative     -----     //

        /// <summary>
        /// Tests the static operator <see cref="Euc3D.Vector.operator *(Euc3D.Vector, double)"/>.
        /// </summary>
        [Fact(DisplayName = "Op. * (double,Vector)")]
        public void Operator__Multiply__Vector_double()
        {
            // Arrange
            Euc3D.Vector vector = new Euc3D.Vector(2.2, -4.6, 9.2);
            double factor = 1.5;
            
            Euc3D.Vector expected = new Euc3D.Vector(3.3, -6.9, 13.8);
            
            //Act
            Euc3D.Vector actual = vector * factor;
            
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the static operator <see cref="Euc3D.Vector.operator *(double, Euc3D.Vector)"/>.
        /// </summary>
        [Fact(DisplayName = "Op. * (double,Vector)")]
        public void Operator__Multiply__double_Vector()
        {
            // Arrange
            double factor = 1.5;
            Euc3D.Vector vector = new Euc3D.Vector(2.2, -4.6, 9.2);
            
            Euc3D.Vector expected = new Euc3D.Vector(3.3, -6.9, 13.8);
            
            //Act
            Euc3D.Vector actual = factor * vector;
            
            // Assert
            Assert.Equal(expected, actual);
        }


        /// <summary>
        /// Tests the static operator <see cref="Euc3D.Vector.operator /(Euc3D.Vector, double)"/>.
        /// </summary>
        [Fact(DisplayName = "Op. / (Vector,double)")]
        public void Operator__Divide__Vector_double()
        {
            // Arrange
            Euc3D.Vector vector = new Euc3D.Vector(2.4, -4.6, 9.2);
            double divisor = 2.0;
            
            Euc3D.Vector expected = new Euc3D.Vector(1.2, -2.3, 4.6);
            
            //Act
            Euc3D.Vector actual = vector / divisor;
            
            // Assert
            Assert.Equal(expected, actual);
        }


        //     -----     Scalar Product     -----     //

        /// <summary>
        /// Tests the static operator <see cref="Euc3D.Vector.operator *(Euc3D.Vector, Euc3D.Vector)"/>.
        /// </summary>
        [Fact(DisplayName = "Op. * (Vector,Vector)")]
        public void Operator__Multiply__Vector_Vector()
        {
            // Arrange
            Euc3D.Vector vectorA = new Euc3D.Vector(2.2, -4.6, 9.2);
            Euc3D.Vector vectorB = new Euc3D.Vector(1.4, -2.6, -5.2);
            
            double expected = -32.8;
            
            //Act
            double actual = vectorA * vectorB;
            
            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Tests the implicit conversion of a <see cref="Euc3D.Point"/> into a <see cref="Euc3D.Vector"/>.
        /// </summary>
        [Fact(DisplayName = "Convert FromPoint")]
        public void Convert__FromPoint()
        {
            // Arrange
            Euc3D.Point point = new Euc3D.Point(1.0, 2.0, -3.0);
            
            Euc3D.Vector expected = new Euc3D.Vector(1.0, 2.0, -3.0);
            
            // Act
            Euc3D.Vector vector = (Euc3D.Vector)point;
            
            // Assert
            Assert.Equal(vector, expected);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tests the method <see cref="Euc3D.Vector.GetCoordinates"/>.
        /// </summary>
        [Fact(DisplayName = "GetCoordinates()")]
        public void GetCoordinates()
        {
            // Arrange
            Euc3D.Vector vector = new Euc3D.Vector(1.0, 3.0, 5.0);
            
            double[] expected = new double[3] { 1.0, 3.0, 5.0 };
            
            // Act
            double[] actual = vector.GetCoordinates();
            
            //Assert
            Assert.Equal(expected.Length, actual.Length);
            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(expected[i], actual[i]);
            }
        }


        /// <summary>
        /// Tests the method <see cref="Euc3D.Vector.Length"/>.
        /// </summary>
        [Fact(DisplayName = "Length()")]
        public void Length()
        {
            // Arrange
            Euc3D.Vector vector = new Euc3D.Vector(1.0, -3.0, 5.0);
            
            double expected = Math.Sqrt(35.0);
            
            // Act
            double actual = vector.Length();
            
            //Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the method <see cref="Euc3D.Vector.SquaredLength"/>.
        /// </summary>
        [Fact(DisplayName = "SquaredLength()")]
        public void SquaredLength()
        {
            // Arrange
            Euc3D.Vector vector = new Euc3D.Vector(1.0, 3.0, 5.0);
            
            double expected = 35.0;
            
            // Act
            double actual = vector.SquaredLength();
            
            //Assert
            Assert.Equal(expected, actual);
        }


        /// <summary>
        /// Tests the method <see cref="Euc3D.Vector.Unitise"/>.
        /// </summary>
        [Fact(DisplayName = "Unitise()")]
        public void Unitise()
        {
            // Arrange
            Euc3D.Vector vector = new Euc3D.Vector(1.0, -5.0, 2.5);
            
            // Act
            vector.Unitise();
            double actual = vector.Length();
            
            // Assert
            Assert.Equal(1d, actual);
        }

        /// <summary>
        /// Tests the method <see cref="Euc3D.Vector.IsUnit"/>.
        /// </summary>
        [Theory(DisplayName = "IsUnit()")]
        [InlineData(new double[] { 1.0, -5.0, 2.5 }, false)] // "False"
        [InlineData(new double[] { 3.0 / 5.0, 4.0 / 5.0, 0.0 }, true)] // "True"
        public void IsUnit(double[] coordinates, bool expected)
        {
            // Arrange
            Euc3D.Vector vector = new Euc3D.Vector(coordinates);
            
            // Act
            bool actual  = vector.IsUnit();
            
            // Assert
            Assert.Equal(expected, actual);
        }


        /// <summary>
        /// Tests the method <see cref="Euc3D.Vector.AngleWith(Euc3D.Vector)"/>.
        /// </summary>
        [Fact(DisplayName = "AngleWith(Vector)")]
        public void AngleWith_Vector()
        {
            // Arrange
            Euc3D.Vector vectorA = new Euc3D.Vector(1.5, 3.0, 7.5);
            Euc3D.Vector vectorB = new Euc3D.Vector(4.0, 5.0, 6.0);
            
            double expected = Math.Acos(66 / (Math.Sqrt(67.5) * Math.Sqrt(77)));
            
            //Act
            double actual = vectorA.AngleWith(vectorB);
            
            // Assert
            Assert.Equal(expected, actual, Settings.AngularPrecision);
        }


        /// <summary>
        /// Tests the method <see cref="Euc3D.Vector.Equals(Euc3D.Vector)"/>.
        /// </summary>
        [Fact(DisplayName = "Equals(Vector)")]
        public void Equals_Vector()
        {
            // Arrange
            Euc3D.Vector vectorA = new Euc3D.Vector(1.0, 3.0, 5.0);
            Euc3D.Vector vectorB = new Euc3D.Vector(1.0, 3.0, 5.0);
            
            // Act
            bool areEqual = vectorA.Equals(vectorB);
            
            // Assert
            Assert.True(areEqual);
        }

        #endregion
    }
}
