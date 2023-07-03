using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.Arithmetic.Numbers;

using Alg_Sets = BRIDGES.Algebra.Sets;


namespace BRIDGES.Tests.Arithmetic.Numbers
{
    /// <summary>
    /// Class testing the members of the <see cref="Quaternion"/> structure.
    /// </summary>
    [TestClass]
    public class QuaternionTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="Quaternion"/> is not reference type.
        /// </summary>
        [TestMethod("Behavior IsNotReference")]
        public void IsNotReference()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion quaternionB = new Quaternion(6.0, 7.0, 8.0, 9.0);
            //Act
            quaternionA = quaternionB;
            // Assert
            Assert.IsTrue(quaternionA.Equals(quaternionB));
            Assert.AreNotSame(quaternionA, quaternionB);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the initialisation of the <see cref="Quaternion"/> from its real and imaginary components,
        /// and the <see cref="Quaternion.ScalarPart"/>, <see cref="Quaternion.I"/>, <see cref="Quaternion.J"/>, <see cref="Quaternion.K"/> properties.
        /// </summary>
        [TestMethod("Property ScalarPart, I, J & K")]
        public void RealPartAndImaginaryPart()
        {
            // Assign
            Quaternion quaternion = new Quaternion(2.0, 4.0, 6.0, 8.0);
            double expectedScalar = 2d, expectedI = 4d, expectedJ = 6d, expectedK = 8d;
            // Act

            // Assert
            Assert.AreEqual(expectedScalar, quaternion.ScalarPart, Settings.AbsolutePrecision);
            Assert.AreEqual(expectedI, quaternion.I, Settings.AbsolutePrecision);
            Assert.AreEqual(expectedJ, quaternion.J, Settings.AbsolutePrecision);
            Assert.AreEqual(expectedK, quaternion.K, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the property <see cref="Quaternion.VectorPart"/>.
        /// </summary>
        [TestMethod("Property VectorPart")]
        public void VectorPart()
        {
            // Assign
            Quaternion quaternion = new Quaternion(2.0, 4.0, 6.0, 8.0);
            double[] expectedVectorPart = new double[] { 4d, 6d, 8d };
            // Act
            double[] actual = quaternion.VectorPart;
            // Assert
            Assert.IsTrue(actual.Length == 3);
            Assert.AreEqual(expectedVectorPart[0], actual[0], Settings.AbsolutePrecision);
            Assert.AreEqual(expectedVectorPart[1], actual[1], Settings.AbsolutePrecision);
            Assert.AreEqual(expectedVectorPart[2], actual[2], Settings.AbsolutePrecision);
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Tests the static property <see cref="Quaternion.Zero"/>.
        /// </summary>
        [TestMethod("Static Zero")]
        public void Static_Zero()
        {
            // Arrange
            Quaternion expected = new Quaternion(0.0, 0.0, 0.0, 0.0);
            // Act
            Quaternion actual = Quaternion.Zero;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static property <see cref="Quaternion.One"/>.
        /// </summary>
        [TestMethod("Static One")]
        public void Static_One()
        {
            // Arrange
            Quaternion expected = new Quaternion(1.0, 0.0, 0.0, 0.0);
            // Act
            Quaternion actual = Quaternion.One;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Tests the static method <see cref="Quaternion.Conjugate(Quaternion)"/>.
        /// </summary>
        [TestMethod("Static Conjugate(Quaternion)")]
        public void Static_Conjugate_Quaternion()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(1.0, -2.0, -3.0, -4.0);
            // Act
            Quaternion actual = Quaternion.Conjugate(quaternion);
            // Assert
            Assert.IsTrue(actual.Equals(expected));
        }


        /******************** Algebraic Field ********************/

        /// <summary>
        /// Tests the static method <see cref="Quaternion.Add(Quaternion, Quaternion)"/>.
        /// </summary>
        [TestMethod("Static Add(Quaternion,Quaternion)")]
        public void Static_Add_Quaternion_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion quaternionB = new Quaternion(6.0, 7.0, 8.0, 9.0);
            Quaternion expected = new Quaternion(7.0, 9.0, 11.0, 13.0);
            //Act
            Quaternion actual = Quaternion.Add(quaternionA, quaternionB);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Quaternion.Subtract(Quaternion, Quaternion)"/>.
        /// </summary>
        [TestMethod("Static Subtract(Quaternion,Quaternion)")]
        public void Static_Substract_Quaternion_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion quaternionB = new Quaternion(9.0, -8.0, 7.0, 6.0);
            Quaternion expected = new Quaternion(-8.0, 10.0, -4.0, -2.0);
            //Act
            Quaternion actual = Quaternion.Subtract(quaternionA, quaternionB);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Quaternion.Opposite(Quaternion)"/>.
        /// </summary>
        [TestMethod("Static Opposite(Quaternion)")]
        public void Static_Opposite_Quaternion()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(-1.0, -2.0, -3.0, -4.0);
            // Act
            Quaternion actual = Quaternion.Opposite(quaternion);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static method <see cref="Quaternion.Multiply(Quaternion, Quaternion)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Quaternion,Quaternion)")]
        public void Static_Multiply_Quaternion_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion quaternionB = new Quaternion(6.0, 7.0, 8.0, 9.0);
            Quaternion expected = new Quaternion(
                1.0 * 6.0 - 2.0 * 7.0 - 3.0 * 8.0 - 4.0 * 9.0, 
                1.0 * 7.0 + 2.0 * 6.0 + 3.0 * 9.0 - 4.0 * 8.0, 
                1.0 * 8.0 - 2.0 * 9.0 + 3.0 * 6.0 + 4.0 * 7.0, 
                1.0 * 9.0 + 2.0 * 8.0 - 3.0 * 7.0 + 4.0 * 6.0);
            //Act
            Quaternion actual = Quaternion.Multiply(quaternionA, quaternionB);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Quaternion.Divide(Quaternion, Quaternion)"/>.
        /// </summary>
        [TestMethod("Static Divide(Quaternion,Quaternion)")]
        public void Static_Divide_Quaternion_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(6.0, 7.0, 8.0, 9.0);
            Quaternion quaternionB = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(
                 (6.0 * 1.0 + 7.0 * 2.0 + 8.0 * 3.0 + 9.0 * 4.0) / 30.0,
                (-6.0 * 2.0 + 7.0 * 1.0 - 8.0 * 4.0 + 9.0 * 3.0) / 30.0,
                (-6.0 * 3.0 + 7.0 * 4.0 + 8.0 * 1.0 - 9.0 * 2.0) / 30.0,
                (-6.0 * 4.0 - 7.0 * 3.0 + 8.0 * 2.0 + 9.0 * 1.0) / 30.0);
            // Act
            Quaternion actual = Quaternion.Divide(quaternionA, quaternionB);
            // Assert 
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Quaternion.Inverse(Quaternion)"/>.
        /// </summary>
        [TestMethod("Static Inverse(Quaternion)")]
        public void Static_Inverse_Quaternion()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(1.0 / 30.0, -2.0 / 30.0, -3.0 / 30.0, -4.0 / 30.0);
            // Act
            Quaternion actual = Quaternion.Inverse(quaternion);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Embedding : Complex ********************/

        /// <summary>
        /// Tests the static method <see cref="Quaternion.Add(Quaternion, Complex)"/>.
        /// </summary>
        [TestMethod("Static Add(Quaternion,Complex)")]
        public void Static_Add_Quaternion_Complex()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Complex complex = new Complex(8.0, 9.0);
            Quaternion expected = new Quaternion(9.0, 11.0, 3.0, 4.0);
            //Act
            Quaternion actual = Quaternion.Add(quaternion, complex);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Quaternion.Add(Complex, Quaternion)"/>.
        /// </summary>
        [TestMethod("Static Add(Complex,Quaternion)")]
        public void Static_Add_Complex_Quaternion()
        {
            // Arrange
            Complex complex = new Complex(8.0, 9.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(9.0, 11.0, 3.0, 4.0);
            //Act
            Quaternion actual = Quaternion.Add(complex, quaternion);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static method <see cref="Quaternion.Subtract(Quaternion, Complex)"/>.
        /// </summary>
        [TestMethod("Static Subtract(Quaternion,Complex)")]
        public void Static_Subtract_Quaternion_Complex()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Complex complex = new Complex(9.0, 8.0);
            Quaternion expected = new Quaternion(-8.0, -6.0, 3.0, 4.0);
            //Act
            Quaternion actual = Quaternion.Subtract(quaternion, complex);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Quaternion.Subtract(Complex, Quaternion)"/>.
        /// </summary>
        [TestMethod("Static Subtract(Complex,Quaternion)")]
        public void Static_Subtract_Complex_Quaternion()
        {
            // Arrange
            Complex complex = new Complex(9.0, 8.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(8.0, 6.0, -3.0, -4.0);
            //Act
            Quaternion actual = Quaternion.Subtract(complex, quaternion);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static method <see cref="Quaternion.Multiply(Quaternion, Complex)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Quaternion,Complex)")]
        public void Static_Multiply_Quaternion_Complex()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Complex complex = new Complex(9.0, 8.0);
            Quaternion expected = new Quaternion( 1.0 * 9.0 - 2.0 * 8.0, 1.0 * 8.0 + 2.0 * 9.0,
                  3.0 * 9.0 + 4.0 * 8.0, - 3.0 * 8.0 + 4.0 * 9.0);
            //Act
            Quaternion actual = Quaternion.Multiply(quaternion, complex);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Quaternion.Multiply(Complex, Quaternion)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Complex,Quaternion)")]
        public void Static_Multiply_Complex_Quaternion()
        {
            // Arrange
            Complex complex = new Complex(9.0, 8.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(1.0 * 9.0 - 2.0 * 8.0, 1.0 * 8.0 + 2.0 * 9.0,
                  3.0 * 9.0 + 4.0 * 8.0, -3.0 * 8.0 + 4.0 * 9.0);
            //Act
            Quaternion actual = Quaternion.Multiply(complex, quaternion);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static method <see cref="Quaternion.Divide(Quaternion, Complex)"/>.
        /// </summary>
        [TestMethod("Static Divide(Quaternion,Complex)")]
        public void Static_Divide_Quaternion_Complex()
        {
            // Arrange
            Quaternion quaternion= new Quaternion(6.0, 7.0, 8.0, 9.0);
            Complex complex = new Complex(1.0, 2.0);
            Quaternion expected = new Quaternion(
                (6.0 * 1.0 + 7.0 * 2.0) / 5.0,
                (-6.0 * 2.0 + 7.0 * 1.0) / 5.0,
                (8.0 * 1.0 - 9.0 * 2.0) / 5.0,
                (8.0 * 2.0 + 9.0 * 1.0) / 5.0);
            // Act
            Quaternion actual = Quaternion.Divide(quaternion, complex);
            // Assert 
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Quaternion.Divide(Complex, Quaternion)"/>.
        /// </summary>
        [TestMethod("Static Divide(Complex,Quaternion)")]
        public void Static_Divide_Complex_Quaternion()
        {
            // Arrange
            Complex complex = new Complex(9.0, 8.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(
                 (9.0 * 1.0 + 8.0 * 2.0) / 30.0,
                (-9.0 * 2.0 + 8.0 * 1.0) / 30.0,
                (-9.0 * 3.0 + 8.0 * 4.0) / 30.0,
                (-9.0 * 4.0 - 8.0 * 3.0) / 30.0);
            // Act
            Quaternion actual = Quaternion.Divide(complex, quaternion);
            // Assert 
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Embedding : Real ********************/

        /// <summary>
        /// Tests the static method <see cref="Quaternion.Add(Quaternion, Real)"/>.
        /// </summary>
        [TestMethod("Static Add(Quaternion,Real)")]
        public void Static_Add_Quaternion_Real()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0 ,4.0);
            Real real = new Real(10.0);
            Quaternion expected = new Quaternion(11.0, 2.0, 3.0, 4.0);
            //Act
            Quaternion actual = Quaternion.Add(quaternion, real);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Quaternion.Add(Real, Quaternion)"/>.
        /// </summary>
        [TestMethod("Static Add(Real,Quaternion)")]
        public void Static_Add_Real_Quaternion()
        {
            // Arrange
            Real real = new Real(10.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(11.0, 2.0, 3.0, 4.0);
            //Act
            Quaternion actual = Quaternion.Add(real, quaternion);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static method <see cref="Quaternion.Subtract(Quaternion, Real)"/>.
        /// </summary>
        [TestMethod("Static Subtract(Quaternion,Real)")]
        public void Static_Subtract_Quaternion_Real()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Real real = new Real(10.0);
            Quaternion expected = new Quaternion(-9.0, 2.0, 3.0, 4.0);
            //Act
            Quaternion actual = Quaternion.Subtract(quaternion, real);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Quaternion.Subtract(Real, Quaternion)"/>.
        /// </summary>
        [TestMethod("Static Subtract(Real,Quaternion)")]
        public void Static_Subtract_Real_Quaternion()
        {
            // Arrange
            Real real = new Real(10.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(9.0, -2.0, -3.0, -4.0);
            //Act
            Quaternion actual = Quaternion.Subtract(real, quaternion);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static method <see cref="Quaternion.Multiply(Quaternion, Real)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Quaternion,Real)")]
        public void Static_Multiply_Quaternion_Real()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Real real = new Real(4.0);
            Quaternion expected = new Quaternion(4.0, 8.0, 12.0, 16.0);
            //Act
            Quaternion actual = Quaternion.Multiply(quaternion, real);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Quaternion.Multiply(Real, Quaternion)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Real,Quaternion)")]
        public void Static_Multiply_Real_Quaternion()
        {
            // Arrange
            Real real = new Real(5.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(5.0, 10.0, 15.0, 20.0);
            //Act
            Quaternion actual = Quaternion.Multiply(real, quaternion);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static method <see cref="Quaternion.Divide(Quaternion, Real)"/>.
        /// </summary>
        [TestMethod("Static Divide(Quaternion,Real)")]
        public void Static_Divide_Quaternion_Real()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Real real = new Real(4.0);
            Quaternion expected = new Quaternion(0.25, 0.5, 0.75, 1.0);
            //Act
            Quaternion actual = Quaternion.Divide(quaternion, real);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Quaternion.Divide(Real, Quaternion)"/>.
        /// </summary>
        [TestMethod("Static Divide(Real,Quaternion)")]
        public void Static_Divide_Real_Quaternion()
        {
            // Arrange
            Real real = new Real(45.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(1.5, -3.0, -4.5, -6.0);
            // Act
            Quaternion actual = Quaternion.Divide(real, quaternion);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static method <see cref="Quaternion.Multiply(Quaternion, double)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Quaternion,double)")]
        public void Static_Multiply_Quaternion_double()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.25, 2.5, 3.75, -5.0);
            Real factor = new Real(4.0);
            Quaternion expected = new Quaternion(5.0, 10.0, 15.0, -20.0);
            //Act
            Quaternion actual = Quaternion.Multiply(quaternion, factor);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Quaternion.Multiply(double, Quaternion)"/>.
        /// </summary>
        [TestMethod("Static Multiply(double,Quaternion)")]
        public void Static_Multiply_double_Quaternion()
        {
            // Arrange
            Real factor = new Real(4.0);
            Quaternion quaternion = new Quaternion(1.25, 2.5, 3.75, -5.0);
            Quaternion expected = new Quaternion(5.0, 10.0, 15.0, -20.0);
            //Act
            Quaternion actual = Quaternion.Multiply(factor, quaternion);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Quaternion.Divide(Quaternion, double)"/>.
        /// </summary>
        [TestMethod("Static Divide(Quaternion,double)")]
        public void Static_Divide_Quaternion_double()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 4.0, 16.0, 64.0);
            double divisor = 4.0;
            Quaternion expected = new Quaternion(0.25, 1.0, 4.0, 16.0);
            //Act
            Quaternion actual = Quaternion.Divide(quaternion, divisor);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        #endregion

        #region Operators

        /******************** Algebraic Field ********************/

        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator +(Quaternion, Quaternion)"/>.
        /// </summary>
        [TestMethod("Operator Add(Quaternion,Quaternion)")]
        public void Operator_Add_Quaternion_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion quaternionB = new Quaternion(6.0, 7.0, 8.0, 9.0);
            Quaternion expected = new Quaternion(7.0, 9.0, 11.0, 13.0);
            //Act
            Quaternion actual = Quaternion.Add(quaternionA, quaternionB);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator -(Quaternion, Quaternion)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Quaternion,Quaternion)")]
        public void Operator_Substract_Quaternion_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion quaternionB = new Quaternion(9.0, -8.0, 7.0, 6.0);
            Quaternion expected = new Quaternion(-8.0, 10.0, -4.0, -2.0);
            //Act
            Quaternion actual = Quaternion.Subtract(quaternionA, quaternionB);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator -(Quaternion)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Quaternion)")]
        public void Operator_Substract_Quaternion()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(5.0, -15.0, -7.5, 2.5);
            Quaternion expected = new Quaternion(-5.0, 15.0, 7.5, -2.5);
            // Act
            Quaternion actual = -quaternion;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator *(Quaternion, Quaternion)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Quaternion,Quaternion)")]
        public void Operator_Multiply_Quaternion_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion quaternionB = new Quaternion(6.0, 7.0, 8.0, 9.0);
            Quaternion expected = new Quaternion(
                1.0 * 6.0 - 2.0 * 7.0 - 3.0 * 8.0 - 4.0 * 9.0,
                1.0 * 7.0 + 2.0 * 6.0 + 3.0 * 9.0 - 4.0 * 8.0,
                1.0 * 8.0 - 2.0 * 9.0 + 3.0 * 6.0 + 4.0 * 7.0,
                1.0 * 9.0 + 2.0 * 8.0 - 3.0 * 7.0 + 4.0 * 6.0);
            //Act
            Quaternion actual = Quaternion.Multiply(quaternionA, quaternionB);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator /(Quaternion, Quaternion)"/>.
        /// </summary>
        [TestMethod("Operator Divide(Quaternion,Quaternion)")]
        public void Operator_Divide_Quaternion_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(6.0, 7.0, 8.0, 9.0);
            Quaternion quaternionB = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(
                 (6.0 * 1.0 + 7.0 * 2.0 + 8.0 * 3.0 + 9.0 * 4.0) / 30.0,
                (-6.0 * 2.0 + 7.0 * 1.0 - 8.0 * 4.0 + 9.0 * 3.0) / 30.0,
                (-6.0 * 3.0 + 7.0 * 4.0 + 8.0 * 1.0 - 9.0 * 2.0) / 30.0,
                (-6.0 * 4.0 - 7.0 * 3.0 + 8.0 * 2.0 + 9.0 * 1.0) / 30.0);
            // Act
            Quaternion actual = Quaternion.Divide(quaternionA, quaternionB);
            // Assert 
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Embedding : Complex ********************/

        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator +(Quaternion, Complex)"/>.
        /// </summary>
        [TestMethod("Operator Add(Quaternion,Complex)")]
        public void Operator_Add_Quaternion_Complex()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Complex complex = new Complex(8.0, 9.0);
            Quaternion expected = new Quaternion(9.0, 11.0, 3.0, 4.0);
            //Act
            Quaternion actual = quaternion + complex;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator +(Complex, Quaternion)"/>.
        /// </summary>
        [TestMethod("Operator Add(Complex,Quaternion)")]
        public void Operator_Add_Complex_Quaternion()
        {
            // Arrange
            Complex complex = new Complex(8.0, 9.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(9.0, 11.0, 3.0, 4.0);
            //Act
            Quaternion actual = complex + quaternion;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator -(Quaternion, Complex)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Quaternion,Complex)")]
        public void Operator_Subtract_Quaternion_Complex()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Complex complex = new Complex(9.0, 8.0);
            Quaternion expected = new Quaternion(-8.0, -6.0, 3.0, 4.0);
            //Act
            Quaternion actual = quaternion - complex;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator -(Complex, Quaternion)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Complex,Quaternion)")]
        public void Operator_Subtract_Complex_Quaternion()
        {
            // Arrange
            Complex complex = new Complex(9.0, 8.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(8.0, 6.0, -3.0, -4.0);
            //Act
            Quaternion actual = complex - quaternion;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator *(Quaternion, Complex)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Quaternion,Complex)")]
        public void Operator_Multiply_Quaternion_Complex()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Complex complex = new Complex(9.0, 8.0);
            Quaternion expected = new Quaternion(1.0 * 9.0 - 2.0 * 8.0, 1.0 * 8.0 + 2.0 * 9.0,
                  3.0 * 9.0 + 4.0 * 8.0, -3.0 * 8.0 + 4.0 * 9.0);
            //Act
            Quaternion actual = quaternion * complex;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator *(Complex, Quaternion)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Complex,Quaternion)")]
        public void Operator_Multiply_Complex_Quaternion()
        {
            // Arrange
            Complex complex = new Complex(9.0, 8.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(1.0 * 9.0 - 2.0 * 8.0, 1.0 * 8.0 + 2.0 * 9.0,
                  3.0 * 9.0 + 4.0 * 8.0, -3.0 * 8.0 + 4.0 * 9.0);
            //Act
            Quaternion actual = complex * quaternion;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator /(Quaternion, Complex)"/>.
        /// </summary>
        [TestMethod("Operator Divide(Quaternion,Complex)")]
        public void Operator_Divide_Quaternion_Complex()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(6.0, 7.0, 8.0, 9.0);
            Complex complex = new Complex(1.0, 2.0);
            Quaternion expected = new Quaternion(
                (6.0 * 1.0 + 7.0 * 2.0) / 5.0,
                (-6.0 * 2.0 + 7.0 * 1.0) / 5.0,
                (8.0 * 1.0 - 9.0 * 2.0) / 5.0,
                (8.0 * 2.0 + 9.0 * 1.0) / 5.0);
            // Act
            Quaternion actual = quaternion / complex;
            // Assert 
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator /(Complex, Quaternion)"/>.
        /// </summary>
        [TestMethod("Operator Divide(Complex,Quaternion)")]
        public void Operator_Divide_Complex_Quaternion()
        {
            // Arrange
            Complex complex = new Complex(9.0, 8.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(
                 (9.0 * 1.0 + 8.0 * 2.0) / 30.0,
                (-9.0 * 2.0 + 8.0 * 1.0) / 30.0,
                (-9.0 * 3.0 + 8.0 * 4.0) / 30.0,
                (-9.0 * 4.0 - 8.0 * 3.0) / 30.0);
            // Act
            Quaternion actual = complex / quaternion;
            // Assert 
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Embedding : Real ********************/

        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator +(Quaternion, Real)"/>.
        /// </summary>
        [TestMethod("Operator Add(Quaternion,Real)")]
        public void Operator_Add_Quaternion_Real()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Real real = new Real(10.0);
            Quaternion expected = new Quaternion(11.0, 2.0, 3.0, 4.0);
            //Act
            Quaternion actual = quaternion + real;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator +(Real, Quaternion)"/>.
        /// </summary>
        [TestMethod("Operator Add(Real,Quaternion)")]
        public void Operator_Add_Real_Quaternion()
        {
            // Arrange
            Real real = new Real(10.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(11.0, 2.0, 3.0, 4.0);
            //Act
            Quaternion actual = real + quaternion;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator -(Quaternion, Real)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Quaternion,Real)")]
        public void Operator_Subtract_Quaternion_Real()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Real real = new Real(10.0);
            Quaternion expected = new Quaternion(-9.0, 2.0, 3.0, 4.0);
            //Act
            Quaternion actual = quaternion - real;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator -(Real, Quaternion)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Real,Quaternion)")]
        public void Operator_Subtract_Real_Quaternion()
        {
            // Arrange
            Real real = new Real(10.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(9.0, -2.0, -3.0, -4.0);
            //Act
            Quaternion actual = real - quaternion;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator *(Quaternion, Real)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Quaternion,Real)")]
        public void Operator_Multiply_Quaternion_Real()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Real real = new Real(4.0);
            Quaternion expected = new Quaternion(4.0, 8.0, 12.0, 16.0);
            //Act
            Quaternion actual = quaternion * real;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator *(Real, Quaternion)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Real,Quaternion)")]
        public void Operator_Multiply_Real_Quaternion()
        {
            // Arrange
            Real real = new Real(5.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(5.0, 10.0, 15.0, 20.0);
            //Act
            Quaternion actual = real * quaternion;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator /(Quaternion, Real)"/>.
        /// </summary>
        [TestMethod("Operator Divide(Quaternion,Real)")]
        public void Operator_Divide_Quaternion_Real()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Real real = new Real(4.0);
            Quaternion expected = new Quaternion(0.25, 0.5, 0.75, 1.0);
            //Act
            Quaternion actual = quaternion / real;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator /(Real, Quaternion)"/>.
        /// </summary>
        [TestMethod("Operator Divide(Real,Quaternion)")]
        public void Operator_Divide_Real_Quaternion()
        {
            // Arrange
            Real real = new Real(45.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(1.5, -3.0, -4.5, -6.0);
            // Act
            Quaternion actual = real / quaternion;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Embedding : double ********************/

        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator +(Quaternion, double)"/>.
        /// </summary>
        [TestMethod("Operator Add(Quaternion,double)")]
        public void Operator_Add_Quaternion_double()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            double number = 10.0;
            Quaternion expected = new Quaternion(11.0, 2.0, 3.0, 4.0);
            //Act
            Quaternion actual = quaternion + number;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator +(double, Quaternion)"/>.
        /// </summary>
        [TestMethod("Operator Add(double,Quaternion)")]
        public void Operator_Add_double_Quaternion()
        {
            // Arrange
            double number = 10.0;
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(11.0, 2.0, 3.0, 4.0);
            //Act
            Quaternion actual = number + quaternion;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator -(Quaternion, double)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Quaternion,double)")]
        public void Operator_Subtract_Quaternion_double()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            double number = 10.0;
            Quaternion expected = new Quaternion(-9.0, 2.0, 3.0, 4.0);
            //Act
            Quaternion actual = quaternion - number;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator -(double, Quaternion)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(double,Quaternion)")]
        public void Operator_Subtract_double_Quaternion()
        {
            // Arrange
            double number = 10.0;
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(9.0, -2.0, -3.0, -4.0);
            //Act
            Quaternion actual = number - quaternion;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator *(Quaternion, double)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Quaternion,double)")]
        public void Operator_Multiply_Quaternion_double()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            double number = 4.0;
            Quaternion expected = new Quaternion(4.0, 8.0, 12.0, 16.0);
            //Act
            Quaternion actual = quaternion * number;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator *(double, Quaternion)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(double,Quaternion)")]
        public void Operator_Multiply_double_Quaternion()
        {
            // Arrange
            double number = 5.0;
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(5.0, 10.0, 15.0, 20.0);
            //Act
            Quaternion actual = number * quaternion;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator /(Quaternion, double)"/>.
        /// </summary>
        [TestMethod("Operator Divide(Quaternion,double)")]
        public void Operator_Divide_Quaternion_double()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            double number = 4.0;
            Quaternion expected = new Quaternion(0.25, 0.5, 0.75, 1.0);
            //Act
            Quaternion actual = quaternion / number;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Quaternion.operator /(double, Quaternion)"/>.
        /// </summary>
        [TestMethod("Operator Divide(double,Quaternion)")]
        public void Operator_Divide_double_Quaternion()
        {
            // Arrange
            double number = 45.0;
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion expected = new Quaternion(1.5, -3.0, -4.5, -6.0);
            // Act
            Quaternion actual = number / quaternion;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Tests the explicit conversion of a <see cref="Complex"/> into a <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Convert FromComplex")]
        public void Convert_FromComplex()
        {
            // Arrange
            Complex complex = new Complex(1.0, 2.0);
            Quaternion expected = new Quaternion(1.0, 2.0, 0.0, 0.0);
            // Act
            Quaternion quaternion = (Quaternion)complex;
            // Assert
            Assert.IsTrue(quaternion.Equals(expected));
        }

        /// <summary>
        /// Tests the explicit conversion of a <see cref="Real"/> into a <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Convert FromReal")]
        public void Convert_FromReal()
        {
            // Arrange
            Real real = new Real(10.0);
            Quaternion expected = new Quaternion(10.0, 0.0, 0.0, 0.0);
            // Act
            Quaternion quaternion = (Quaternion)real;
            // Assert
            Assert.IsTrue(quaternion.Equals(expected));
        }

        /// <summary>
        /// Tests the explicit conversion of a <see cref="double"/> into a <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Convert Fromdouble")]
        public void Convert_Fromdouble()
        {
            // Arrange
            double number = 20.0;
            Quaternion expected = new Quaternion(20.0, 0.0, 0.0, 0.0);
            // Act
            Quaternion quaternion = (Quaternion)number;
            // Assert
            Assert.IsTrue(quaternion.Equals(expected));
        }

        /// <summary>
        /// Tests the explicit conversion of a <see cref="ValueTuple{T1,T2,T3,T4}"/> into a <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Convert FromValueTuple")]
        public void Convert_FromValueTuple()
        {
            // Arrange
            ValueTuple<double, double, double, double> quadruple = new ValueTuple<double, double, double, double>(2.0, 4.0, 6.0, 8.0);
            Quaternion expected = new Quaternion(2.0, 4.0, 6.0, 8.0);
            // Act
            Quaternion quaternion = (Quaternion)quadruple;
            // Assert
            Assert.IsTrue(quaternion.Equals(expected));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tests the method <see cref="Quaternion.DotProduct(Quaternion)"/>.
        /// </summary>
        [TestMethod("Method DotProduct(Quaternion)")]
        public void DotProduct_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion quaternionB = new Quaternion(0.5, 1.5, 2.5, 3.5);
            double expected = 25.0;
            // Act
            double actual = quaternionA.DotProduct(quaternionB);
            // Assert
            Assert.AreEqual(expected, actual , Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the method <see cref="Quaternion.Norm()"/>.
        /// </summary>
        [TestMethod("Method Norm()")]
        public void Norm()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1d, 2d, 3d, 4d);
            // Act
            double norm = quaternion.Norm();
            // Assert
            Assert.AreEqual(Math.Sqrt(30d), norm, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the method <see cref="Quaternion.DistanceTo(Quaternion)"/>.
        /// </summary>
        [TestMethod("Method DistanceTo(Quaternion)")]
        public void DistanceTo_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion quaternionB = new Quaternion(0.5, 1.5, 2.5, 3.5);
            double expected = 1d;
            // Act
            double actual = quaternionA.DistanceTo(quaternionB);
            // Assert
            Assert.AreEqual(expected, actual, Settings.AbsolutePrecision);
        }


        /// <summary>
        /// Tests the method <see cref="Quaternion.Equals(Quaternion)"/>.
        /// </summary>
        [TestMethod("Method Equals(Quaternion)")]
        public void Equals_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion quaternionB = new Quaternion(1.0, 2.0, 3.0, 4.0);
            // Assert
            Assert.IsTrue(quaternionA.Equals(quaternionB));
        }

        #endregion


        #region Explicit Implementations

        /******************** IGroupAction<Quaternion, Complex> ********************/

        /// <summary>
        /// Tests the <see cref="Alg_Sets.IGroupAction{TSelf, TValue}.Multiply(TValue)"/> method of <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("AsIGroupAction<Quaternion,Complex> Multiply(Complex)")]
        public void AsIGroupAction_Multiply_Complex()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Complex complex = new Complex(9.0, 8.0);
            Quaternion expected = new Quaternion(1.0 * 9.0 - 2.0 * 8.0, 1.0 * 8.0 + 2.0 * 9.0,
                  3.0 * 9.0 + 4.0 * 8.0, -3.0 * 8.0 + 4.0 * 9.0);
            //Act
            Alg_Sets.IGroupAction<Quaternion, Complex> representation = (Alg_Sets.IGroupAction<Quaternion, Complex>)quaternion;
            Quaternion actual = representation.Multiply(complex);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the <see cref="Alg_Sets.IGroupAction{TSelf, TValue}.Divide(TValue)"/> method of <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("AsIGroupAction<Quaternion,Complex> Divide(Complex)")]
        public void AsIGroupAction_Divide_Complex()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(6.0, 7.0, 8.0, 9.0);
            Complex complex = new Complex(1.0, 2.0);
            Quaternion expected = new Quaternion(
                (6.0 * 1.0 + 7.0 * 2.0) / 5.0,
                (-6.0 * 2.0 + 7.0 * 1.0) / 5.0,
                (8.0 * 1.0 - 9.0 * 2.0) / 5.0,
                (8.0 * 2.0 + 9.0 * 1.0) / 5.0);
            // Act
            Alg_Sets.IGroupAction<Quaternion, Complex> representation = (Alg_Sets.IGroupAction<Quaternion, Complex>)quaternion;
            Quaternion actual = representation.Divide(complex); ;
            // Assert 
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** IGroupAction<Quaternion, Real> ********************/


        /// <summary>
        /// Tests the <see cref="Alg_Sets.IGroupAction{TSelf, TValue}.Multiply(TValue)"/> method of <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("AsIGroupAction<Quaternion,Real> Multiply(Real)")]
        public void AsIGroupAction_Multiply_Real()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.5, -5.0, 10.0);
            Real factor = new Real(4.0);
            Quaternion expected = new Quaternion(4.0, 10.0, -20.0, 40.0);
            //Act
            Alg_Sets.IGroupAction<Quaternion, Real> representation = (Alg_Sets.IGroupAction<Quaternion, Real>)quaternion;
            Quaternion actual = representation.Multiply(factor);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the <see cref="Alg_Sets.IGroupAction{TSelf, TValue}.Divide(TValue)"/> method of <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("AsIGroupAction<Quaternion,Real> Divide(Real)")]
        public void AsIGroupAction_Divide_Real()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.5, 5.0, -10.0);
            Real divisor = new Real(4.0);
            Quaternion expected = new Quaternion(0.25, 0.625, 1.25, -2.5);
            //Act
            Alg_Sets.IGroupAction<Quaternion, Real> representation = (Alg_Sets.IGroupAction<Quaternion, Real>)quaternion;
            Quaternion actual = representation.Divide(divisor);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** IGroupAction<Quaternion, double> ********************/

        /// <summary>
        /// Tests the <see cref="Alg_Sets.IGroupAction{TSelf, TValue}.Multiply(TValue)"/> method of <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("AsIGroupAction<Quaternion,double> Multiply(double)")]
        public void AsIGroupAction_Multiply_double()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.5, -5.0, 10.0);
            double factor = 4.0;
            Quaternion expected = new Quaternion(4.0, 10.0, -20.0, 40.0);
            //Act
            Alg_Sets.IGroupAction<Quaternion, double> representation = (Alg_Sets.IGroupAction<Quaternion, double>)quaternion;
            Quaternion actual = representation.Multiply(factor);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the <see cref="Alg_Sets.IGroupAction{TSelf, TValue}.Divide(TValue)"/> method of <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("AsIGroupAction<Quaternion,double> Divide(double)")]
        public void AsIGroupAction_Divide_double()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.5, 5.0, -10.0);
            double divisor = 4.0;
            Quaternion expected = new Quaternion(0.25, 0.625, 1.25, -2.5);
            //Act
            Alg_Sets.IGroupAction<Quaternion, double> representation = (Alg_Sets.IGroupAction<Quaternion, double>)quaternion;
            Quaternion actual = representation.Divide(divisor);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        #endregion
    }
}
