using System;

using Xunit;

using Euc3D = BRIDGES.Geometry.Euclidean3D;


namespace BRIDGES.Tests.Geometry.Euclidean3D
{

    /// <summary>
    /// Tests the members of the <see cref=Euc3D.Point"/> class.
    /// </summary>
    public class Point
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="Euc3D.Point"/> is not reference type.
        /// </summary>
        [Fact(DisplayName = "Is Not Reference")]
        public void IsNotReference()
        {
            // Arrange
            Euc3D.Point pointA = new Euc3D.Point(1.0, 2.0, 3.0);
            Euc3D.Point pointB = new Euc3D.Point(4.0, 5.0, 6.0);

            //Act
            pointA = pointB;

            // Assert
            Assert.Equal(pointA, pointB);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the initialisation of the <see cref="Euc3D.Point"/> from its three cooridnates,
        /// and the <see cref="Euc3D.Point.X"/>, <see cref="Euc3D.Point.Y"/>, <see cref="Euc3D.Point.Z"/> properties.
        /// </summary>
        [Fact(DisplayName = "Prop. X & Y & Z")]
        public void Property__XAndYAndZ()
        {
            // Arrange
            Euc3D.Point point = new Euc3D.Point(1.5, 1.7, 2.1);
            double[] expected = new double[] { 1.5, 1.7, 2.1 };
            //Act
            double x = point.X;
            double y = point.Y;
            double z = point.Z;
            // Assert
            Assert.Equal(expected[0], x, Settings.AbsolutePrecision);
            Assert.Equal(expected[1], y, Settings.AbsolutePrecision);
            Assert.Equal(expected[2], z, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the <see cref="Euc3D.Point"/> indexer property.
        /// </summary>
        [Fact(DisplayName = "Prop. this[int]")]
        public void Property__Indexer()
        {
            // Arrange
            Euc3D.Point point = new Euc3D.Point(1.5, 1.7, 2.1);
            double[] expected = new double[] { 1.5, 1.7, 2.1 };
            //Act
            double x = point[0];
            double y = point[1];
            double z = point[2];
            // Assert
            Assert.Equal(expected[0], x, Settings.AbsolutePrecision);
            Assert.Equal(expected[1], y, Settings.AbsolutePrecision);
            Assert.Equal(expected[2], z, Settings.AbsolutePrecision);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Tests the initialisation of the <see cref="Euc3D.Point"/> from its X, Y and Z coordinates.
        /// </summary>
        [Fact(DisplayName = "Constructor(X,Y,Z)")]
        public void Constructor__Double_Double_Double()
        {
            // Arrange
            Euc3D.Point expected = new Euc3D.Point(new double[3] { 2.0, 5.0, 8.0 });

            // Act
            Euc3D.Point actual = new Euc3D.Point(2.0, 5.0, 8.0);

            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Euc3D.Point"/> from its coordinates.
        /// </summary>
        [Fact(DisplayName = "Constructor(double[])")]
        public void Constructor__ArrayOfDouble()
        {
            // Arrange
            Euc3D.Point expected = new Euc3D.Point(2.0, 5.0, 8.0);
            bool throwsException = false;

            // Act
            Euc3D.Point actual = new Euc3D.Point(new double[3] { 2.0, 5.0, 8.0 });

            try { Euc3D.Point otherPoint = new Euc3D.Point(new double[] { 2.0, 5.0, 8.0, 11.0 }); } 
            catch (ArgumentOutOfRangeException) { throwsException = true; }

            // Assert
            Assert.Equal(expected, actual);
            Assert.True(throwsException);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Euc3D.Point"/> from another <see cref="Euc3D.Point"/>.
        /// </summary>
        [Fact(DisplayName = "Constructor(Point)")]
        public void Constructor__Point()
        {
            // Arrange
            Euc3D.Point expected = new Euc3D.Point(2.0, 5.0, 8.0);

            // Act
            Euc3D.Point actual = new Euc3D.Point(expected);

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Tests the static property <see cref="Euc3D.Point.Zero"/>.
        /// </summary>
        [Fact(DisplayName = "Static Zero")]
        public void Static__Zero()
        {
            // Arrange
            Euc3D.Point expected = new Euc3D.Point(0.0, 0.0, 0.0);
            // Act
            Euc3D.Point actual = Euc3D.Point.Zero;
            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Public Static Methods

        //     -----     Internal Binary Operation on Points : Additive     -----     //

        /// <summary>
        /// Tests the static method <see cref="Euc3D.Point.Add(Euc3D.Point, Euc3D.Point)"/>.
        /// </summary>
        [Fact(DisplayName = "Static Add(Point,Point)")]
        public void Static__Add__Point_Point()
        {
            // Arrange
            Euc3D.Point pointA = new Euc3D.Point(1.5, 2.5, 3.5);
            Euc3D.Point pointB = new Euc3D.Point(1.2, 2.4, 3.6);

            Euc3D.Point expected = new Euc3D.Point(2.7, 4.9, 7.1);
            
            //Act
            Euc3D.Point actual = Euc3D.Point.Add(pointA, pointB);
            
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the static method <see cref="Euc3D.Point.Subtract(Euc3D.Point, Euc3D.Point)"/>.
        /// </summary>
        [Fact(DisplayName = "Static Subtract(Point,Point)")]
        public void Static__Substract__Point_Point()
        {
            // Arrange
            Euc3D.Point pointA = new Euc3D.Point(1.5, 2.5, 3.5);
            Euc3D.Point pointB = new Euc3D.Point(1.2, 2.4, 3.6);

            Euc3D.Point expected = new Euc3D.Point(0.3, 0.1, -0.1);

            //Act
            Euc3D.Point actual = Euc3D.Point.Subtract(pointA, pointB);
            
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


        //     -----     Group Action of Scalars on Points : Multiplicative     -----     //

        /// <summary>
        /// Tests the static method <see cref="Euc3D.Point.Multiply(Euc3D.Point, double)"/>
        /// </summary>
        [Fact(DisplayName = "Static Multiply(Point,double)")]
        public void Static__Multiply__Point_double()
        {
            // Arrange
            Euc3D.Point point = new Euc3D.Point(2.2, -4.6, 9.2);
            double factor = 1.5;
            Euc3D.Point expected = new Euc3D.Point(3.3, -6.9, 13.8);
            
            //Act
            Euc3D.Point actual = Euc3D.Point.Multiply(point, factor);
            
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the static method <see cref="Euc3D.Point.Multiply(double, Euc3D.Point)"/>
        /// </summary>
        [Fact(DisplayName = "Static Multiply(double,Point)")]
        public void Static__Multiply__double_Point()
        {
            // Arrange
            double factor = 1.5;
            Euc3D.Point point = new Euc3D.Point(2.2, -4.6, 9.2);
            Euc3D.Point expected = new Euc3D.Point(3.3, -6.9, 13.8);
            //Act
            Euc3D.Point actual = Euc3D.Point.Multiply(factor, point);
            // Assert
            Assert.Equal(expected, actual);
        }


        /// <summary>
        /// Tests the static method <see cref="Euc3D.Point.Divide(Euc3D.Point, double)"/>
        /// </summary>
        [Fact(DisplayName = "Static Divide(Point,double)")]
        public void Static__Divide__Point_double()
        {
            // Arrange
            Euc3D.Point point = new Euc3D.Point(2.4, -4.6, 9.2);
            double divisor = 2.0;
            Euc3D.Point expected = new Euc3D.Point(1.2, -2.3, 4.6);
            //Act
            Euc3D.Point actual = Euc3D.Point.Divide(point, divisor);
            // Assert
            Assert.Equal(expected, actual);
        }


        //     -----     Group Action of Vectors on Points : Additive     -----     //

        /// <summary>
        /// Tests the static method <see cref="Euc3D.Point.Add(Euc3D.Point, Euc3D.Vector)"/>
        /// </summary>
        [Fact(DisplayName = "Static Add(Point,Vector)")]
        public void Static__Add__Point_Vector()
        {
            // Arrange
            Euc3D.Point point = new Euc3D.Point(2.2, 4.6, 9.2);
            Euc3D.Vector vector = new Euc3D.Vector(3.3, 4.4, 5.5);

            Euc3D.Point expected = new Euc3D.Point(5.5, 9.0, 14.7);

            //Act
            Euc3D.Point actual = Euc3D.Point.Add(point, vector);
            
            // Assert
            Assert.Equal(expected, actual);
        }


        /// <summary>
        /// Tests the static method <see cref="Euc3D.Point.Subtract(Euc3D.Point, Euc3D.Vector)"/>
        /// </summary>
        [Fact(DisplayName = "Static Subtract(Point,Vector)")]
        public void Static__Subtract__Point_Vector()
        {
            // Arrange
            Euc3D.Point point = new Euc3D.Point(2.2, 4.6, 9.2);
            Euc3D.Vector vector = new Euc3D.Vector(3.3, -4.4, 5.5);

            Euc3D.Point expected = new Euc3D.Point(-1.1, 9.0, 3.7);

            //Act
            Euc3D.Point actual = Euc3D.Point.Subtract(point, vector);
            
            // Assert
            Assert.Equal(expected, actual);
        }


        //     -----     Scalar Product     -----     //

        /// <summary>
        /// Tests the static method <see cref="Euc3D.Point.DotProduct(Euc3D.Point, Euc3D.Point)"/>.
        /// </summary>
        [Fact(DisplayName = "Static DotProduct(Point,Point)")]
        public void Static__DotProduct__Point_Point()
        {
            // Arrange
            Euc3D.Point pointA = new Euc3D.Point(2.2, -4.6, 9.2);
            Euc3D.Point pointB= new Euc3D.Point(1.4, -2.6, -5.2);

            double expected = -32.8; 
            
            //Act
            double actual = Euc3D.Point.DotProduct(pointA, pointB);
            
            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Public Static Operator

        //     -----     Internal Binary Operation of Points : Additive     -----     //

        /// <summary>
        /// Tests the static operator <see cref="Euc3D.Point.operator +(Euc3D.Point, Euc3D.Point)"/>.
        /// </summary>
        [Fact(DisplayName = "Op. + (Point,Point)")]
        public void Operator__Add__Point_Point()
        {
            // Arrange
            Euc3D.Point pointA = new Euc3D.Point(1.5, 2.5, 3.5);
            Euc3D.Point pointB = new Euc3D.Point(1.2, 2.4, 3.6);

            Euc3D.Point expected = new Euc3D.Point(2.7, 4.9, 7.1);
            
            //Act
            Euc3D.Point actual = pointA + pointB;
            
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the static operator <see cref="Euc3D.Point.operator -(Euc3D.Point, Euc3D.Point)"/>.
        /// </summary>
        [Fact(DisplayName = "Op. - (Point,Point)")]
        public void Operator__Subtract__Point_Point()
        {
            // Arrange
            Euc3D.Point pointA = new Euc3D.Point(1.5, 2.5, 3.5);
            Euc3D.Point pointB = new Euc3D.Point(1.2, 2.4, 3.6);

            Euc3D.Point expected = new Euc3D.Point(0.3, 0.1, -0.1);
            //Act
            Euc3D.Point actual = pointA - pointB;
            // Assert
            Assert.Equal(expected, actual);
        }


        /// <summary>
        /// Tests the static operator <see cref="Euc3D.Point.operator -(Euc3D.Point)"/>.
        /// </summary>
        [Fact(DisplayName = "Op. - (Point)")]
        public void Operator__UnaryNegation__Point()
        {
            // Arrange
            Euc3D.Point point = new Euc3D.Point(1.0, -2.0, 3.0);

            Euc3D.Point expected = new Euc3D.Point(-1.0, 2.0, -3.0);
            
            //Act
            Euc3D.Point actual = - point;
            
            // Assert
            Assert.Equal(expected, actual);
        }


        //     -----     Group Action of Scalars on Points : Multiplicative     -----     //

        /// <summary>
        /// Tests the static operator <see cref="Euc3D.Point.operator *(double, Euc3D.Point)"/>.
        /// </summary>
        [Fact(DisplayName = "Op. * (double,Point)")]
        public void Operator__Multiply__double_Point()
        {
            // Arrange
            double number = 1.5;
            Euc3D.Point point = new Euc3D.Point(2.2, -4.6, 9.2);

            Euc3D.Point expected = new Euc3D.Point(3.3, -6.9, 13.8);
            
            //Act
            Euc3D.Point actual = number * point;
            
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the static operator <see cref="Euc3D.Point.operator *(Euc3D.Point, double)"/>.
        /// </summary>
        [Fact(DisplayName = "Op. * (Point,double)")]
        public void Operator__Multiply__Point_double()
        {
            // Arrange
            Euc3D.Point point = new Euc3D.Point(2.2, -4.6, 9.2);
            double number = 1.5;

            Euc3D.Point expected = new Euc3D.Point(3.3, -6.9, 13.8);
            
            //Act
            Euc3D.Point actual = point * number;
            
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the static operator <see cref="Euc3D.Point.operator /(Euc3D.Point, double)"/>.
        /// </summary>
        [Fact(DisplayName ="Op. / (Point,double)")]
        public void Operator__Divide__Point_double()
        {
            // Arrange
            Euc3D.Point point = new Euc3D.Point(2.4, -4.6, 9.2);
            double number = 2.0;

            Euc3D.Point expected = new Euc3D.Point(1.2, -2.3, 4.6);
            
            //Act
            Euc3D.Point actual = point / number;
            
            // Assert
            Assert.Equal(expected, actual);
        }


        //     -----     Group Action of Vectors on Points : Additive     -----     //

        /// <summary>
        /// Tests the static operator <see cref="Euc3D.Point.operator +(Euc3D.Point, Euc3D.Vector)"/>.
        /// </summary>
        [Fact(DisplayName = "Op. + (Point,Vector)")]
        public void Operator__Add__Point_Vector()
        {
            // Arrange
            Euc3D.Point point = new Euc3D.Point(1.5, 2.5, 3.5);
            Euc3D.Vector vector = new Euc3D.Vector(1.2, 2.4, 3.6);

            Euc3D.Point expected = new Euc3D.Point(2.7, 4.9, 7.1);
            
            //Act
            Euc3D.Point actual = point + vector;
            
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the static operator <see cref="Euc3D.Point.operator -(Euc3D.Point, Euc3D.Vector)"/>.
        /// </summary>
        [Fact(DisplayName ="Op. - (Point,Vector)")]
        public void Operator__Subtract__Point_Vector()
        {
            // Arrange
            Euc3D.Point point = new Euc3D.Point(1.5, 2.5, 3.5);
            Euc3D.Vector vector = new Euc3D.Vector(1.2, 2.4, 3.6);

            Euc3D.Point expected = new Euc3D.Point(0.3, 0.1, -0.1);
            
            //Act
            Euc3D.Point actual = point - vector;
            
            // Assert
            Assert.Equal(expected, actual);
        }


        //     -----     Scalar Product     -----     //

        /// <summary>
        /// Tests the static operator <see cref="Euc3D.Point.operator *(Euc3D.Point, Euc3D.Point)"/>.
        /// </summary>
        [Fact(DisplayName = "Op. * (Point,Point)")]
        public void Operator__Multiply__Point_Point()
        {
            // Arrange
            Euc3D.Point pointA = new Euc3D.Point(2.2, -4.6, 9.2);
            Euc3D.Point pointB = new Euc3D.Point(1.4, -2.6, -5.2);

            double expected = -32.8;
            
            //Act
            double actual = pointA * pointB;
            
            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Tests the implicit conversion of a <see cref="Euc3D.Vector"/> into a <see cref="Euc3D.Point"/>.
        /// </summary>
        [Fact(DisplayName = "Convert FromVector")]
        public void Convert_FromVector()
        {
            // Arrange
            Euc3D.Vector vector = new Euc3D.Vector(1.0, 2.0, -3.0);

            Euc3D.Point expected = new Euc3D.Point(1.0, 2.0, -3.0);
            
            // Act
            Euc3D.Point actual = (Euc3D.Point)vector;
            
            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tests the method <see cref="Euc3D.Point.DistanceTo(Euc3D.Point)"/>.
        /// </summary>
        [Fact(DisplayName = "Distance(Point)")]
        public void Distance_Point()
        {
            // Arrange
            Euc3D.Point pointA = new Euc3D.Point(1.0, 3.0, 5.0);
            Euc3D.Point pointB = new Euc3D.Point(2.0, 2.0, 2.0);
            double expected = Math.Sqrt(11.0);
            
            // Act
            double actual = pointA.DistanceTo(pointB);
            
            //Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the method <see cref="Euc3D.Point.Norm"/>.
        /// </summary>
        [Fact(DisplayName = "Norm()")]
        public void Norm()
        {
            // Arrange
            Euc3D.Point point = new Euc3D.Point(1.0, 3.0, 5.0);
            double expected = Math.Sqrt(35.0);
            
            // Act
            double actual = point.Norm();
            
            //Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the method <see cref="Euc3D.Point.GetCoordinates"/>.
        /// </summary>
        [Fact(DisplayName = "GetCoordinates()")]
        public void GetCoordinates()
        {
            // Arrange
            Euc3D.Point point = new Euc3D.Point(1.0, 3.0, 5.0);
            double[] expected = new double[3] { 1.0, 3.0, 5.0 };
            
            // Act
            double[] actual = point.GetCoordinates();
            
            //Assert
            Assert.Equal(expected.Length, actual.Length);
            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(expected[i], actual[i]);
            }
        }


        /// <summary>
        /// Tests the method <see cref="Euc3D.Point.Equals(Euc3D.Point)"/>.
        /// </summary>
        [Fact(DisplayName = "Equals(Point)")]
        public void Equals_Point()
        {
            // Arrange
            Euc3D.Point pointA = new Euc3D.Point(1.0, 3.0, 5.0);
            Euc3D.Point pointB = new Euc3D.Point(1.0, 3.0, 5.0);
            
            // Act
            bool areEqual = pointA.Equals(pointB);
            
            // Assert
            Assert.True(areEqual);
        }

        #endregion
    }
}
