using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.Arithmetic.Numbers;

using Alg_Sets = BRIDGES.Algebra.Sets;


namespace BRIDGES.Test.Arithmetic.Numbers
{
    /// <summary>
    /// Class testing the members of the <see cref="Complex"/> structure.
    /// </summary>
    [TestClass]
    public class ComplexTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="Complex"/> is not reference type.
        /// </summary>
        [TestMethod("Behavior IsNotReference")]
        public void IsNotReference()
        {
            // Arrange
            Complex complexA = new Complex(1.0, 2.0);
            Complex complexB = new Complex(3.0, 4.0);
            //Act
            complexA = complexB;
            // Assert
            Assert.IsTrue(complexB.Equals(complexA));
            Assert.AreNotSame(complexA, complexB);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the initialisation of the <see cref="Complex"/> from its real and imaginary part,
        /// and the <see cref="Complex.RealPart"/>, <see cref="Complex.ImaginaryPart"/> properties.
        /// </summary>
        [TestMethod("Property RealPart & ImaginaryPart")]
        public void RealPartAndImaginaryPart()
        {
            // Assign
            Complex complex = new Complex(2.0, 5.0);
            double expectedReal = 2d;
            double expectedImaginary = 5d;
            // Act

            // Assert
            Assert.AreEqual(expectedReal, complex.RealPart, Settings.AbsolutePrecision);
            Assert.AreEqual(expectedImaginary, complex.ImaginaryPart, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the <see cref="Complex.Modulus"/> and <see cref="Complex.Argument"/> properties.
        /// </summary>
        [TestMethod("Property Modulus & Argument")]
        public void ModulusAndArgument()
        {
            // Assign
            Complex complex = new Complex(Math.Sqrt(1.0 / 2.0), Math.Sqrt(1.0 / 2.0));
            double expectedModulus = 1d;
            double expectedArgument = Math.PI / 4d;
            // Act

            // Assert
            Assert.AreEqual(expectedModulus, complex.Modulus, Settings.AbsolutePrecision);
            Assert.AreEqual(expectedArgument, complex.Argument, Settings.AngularPrecision);
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Tests the static property <see cref="Complex.Zero"/>.
        /// </summary>
        [TestMethod("Static Zero")]
        public void Static_Zero()
        {
            // Arrange
            Complex expected = new Complex(0.0, 0.0);
            // Act
            Complex actual = Complex.Zero;
            // Assert
            Assert.IsTrue(actual.Equals(expected));
        }

        /// <summary>
        /// Tests the static property <see cref="Complex.One"/>.
        /// </summary>
        [TestMethod("Static One")]
        public void Static_One()
        {
            // Arrange
            Complex expected = new Complex(1.0, 0.0);
            // Act
            Complex actual = Complex.One;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static property <see cref="Complex.ImaginaryOne"/>.
        /// </summary>
        [TestMethod("Static ImaginaryOne")]
        public void Static_ImaginaryOne()
        {
            // Arrange
            Complex expected = new Complex(0.0, 1.0);
            // Act
            Complex actual = Complex.ImaginaryOne;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Tests the static method <see cref="Complex.FromPolarCoordinates(double, double)"/>.
        /// </summary>
        [TestMethod("Static FromPolarCoordinates(double,double)")]
        public void Static_FromPolarCoordinates()
        {
            // Arrange
            Complex complex = Complex.FromPolarCoordinates(2.0, Math.PI / 6.0);
            double expectedReal = Math.Sqrt(3.0);
            double expectedImaginary = 1.0;
            // Act

            // Assert
            Assert.AreEqual(expectedReal, complex.RealPart, Settings.AbsolutePrecision);
            Assert.AreEqual(expectedImaginary, complex.ImaginaryPart, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the static method <see cref="Complex.Conjugate(Complex)"/>.
        /// </summary>
        [TestMethod("Static Conjugate(Complex)")]
        public void Static_Conjugate_Complex()
        {
            // Arrange
            Complex complex = new Complex(3.0, 1.0);
            Complex expected = new Complex(3.0, -1.0);
            // Act
            Complex actual = Complex.Conjugate(complex);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Algebraic Field ********************/

        /// <summary>
        /// Tests the static method <see cref="Complex.Add(Complex, Complex)"/>.
        /// </summary>
        [TestMethod("Static Add(Complex,Complex)")]
        public void Static_Add_Complex_Complex()
        {
            // Arrange
            Complex complexA = new Complex(1.5, 6.0);
            Complex complexB = new Complex(5.5, 2.0);
            Complex expected = new Complex(7.0, 8.0);
            //Act
            Complex actual = Complex.Add(complexA, complexB);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Complex.Subtract(Complex, Complex)"/>.
        /// </summary>
        [TestMethod("Static Subtract(Complex,Complex)")]
        public void Static_Substract_Complex_Complex()
        {
            // Arrange
            Complex complexA = new Complex(1.5, 6.0);
            Complex complexB = new Complex(5.5, 2.0);
            Complex expected = new Complex(-4.0, 4.0);
            //Act
            Complex actual = Complex.Subtract(complexA, complexB);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Complex.Opposite(Complex)"/>.
        /// </summary>
        [TestMethod("Static Opposite(Complex)")]
        public void Static_Opposite_Complex()
        {
            // Arrange
            Complex complex = new Complex(1.0, -5.0);
            Complex expected = new Complex(-1.0, 5.0);
            // Act
             Complex actual = Complex.Opposite(complex);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static method <see cref="Complex.Multiply(Complex, Complex)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Complex,Complex)")]
        public void Static_Multiply_Complex_Complex()
        {
            // Arrange
            Complex complexA = new Complex(1.0, 2.5);
            Complex complexB = new Complex(2.0, 3.0);
            Complex expected = new Complex(-5.5, 8.0);
            //Act
            Complex actual = Complex.Multiply(complexA, complexB);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Complex.Divide(Complex, Complex)"/>.
        /// </summary>
        [TestMethod("Static Divide(Complex,Complex)")]
        public void Static_Divide_Complex_Complex()
        {
            // Arrange
            Complex complexA = new Complex(4.0, 3.0);
            Complex complexB = new Complex(2.0, 1.0);
            Complex expected = new Complex(11.0 / 5.0, 2.0 / 5.0);
            // Act
            Complex actual = Complex.Divide(complexA, complexB);
            // Assert 
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Complex.Inverse(Complex)"/>.
        /// </summary>
        [TestMethod("Static Inverse(Complex)")]
        public void Static_Inverse_Complex()
        {
            // Arrange
            Complex complex = new Complex(2.0, 3.0);
            Complex expected = new Complex(2.0 / 13.0, -3.0 / 13.0);
            // Act
            Complex actual = Complex.Inverse(complex);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Embedding : Real ********************/

        /// <summary>
        /// Tests the static method <see cref="Complex.Add(Complex, Real)"/>.
        /// </summary>
        [TestMethod("Static Add(Complex,Real)")]
        public void Static_Add_Complex_Real()
        {
            // Arrange
            Complex complex = new Complex(1.5, 6.0);
            Real real = new Real(10.0);
            Complex expected = new Complex(11.5, 6.0);
            //Act
            Complex actual = Complex.Add(complex, real);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Complex.Add(Real, Complex)"/>.
        /// </summary>
        [TestMethod("Static Add(Real,Complex)")]
        public void Static_Add_Real_Complex()
        {
            // Arrange
            Real real = new Real(10.0);
            Complex complex = new Complex(1.5, 6.0);
            Complex expected = new Complex(11.5, 6.0);
            //Act
            Complex actual = Complex.Add(real, complex);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static method <see cref="Complex.Subtract(Complex, Real)"/>.
        /// </summary>
        [TestMethod("Static Subtract(Complex,Real)")]
        public void Static_Subtract_Complex_Real()
        {
            // Arrange
            Complex complex = new Complex(11.5, 5.0);
            Real real = new Real(10.0);
            Complex expected = new Complex(1.5, 5.0);
            //Act
            Complex actual = Complex.Subtract(complex, real);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Complex.Subtract(Real, Complex)"/>.
        /// </summary>
        [TestMethod("Static Subtract(Real,Complex)")]
        public void Static_Subtract_Real_Complex()
        {
            // Arrange
            Real real = new Real(10.0);
            Complex complex = new Complex(1.5, 6.0);
            Complex expected = new Complex(8.5, -6.0);
            //Act
            Complex actual = Complex.Subtract(real, complex);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static method <see cref="Complex.Multiply(Complex, Real)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Complex,Real)")]
        public void Static_Multiply_Complex_Real()
        {
            // Arrange
            Complex complex = new Complex(1.0, 2.5);
            Real real = new Real(4.0);
            Complex expected = new Complex(4.0, 10.0);
            //Act
            Complex actual = Complex.Multiply(complex, real);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Complex.Multiply(Real, Complex)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Real,Complex)")]
        public void Static_Multiply_Real_Complex()
        {
            // Arrange
            Real real = new Real(4.0);
            Complex complex = new Complex(1.0, 2.5);
            Complex expected = new Complex(4.0, 10.0);
            //Act
            Complex actual = Complex.Multiply(real, complex);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static method <see cref="Complex.Divide(Complex, Real)"/>.
        /// </summary>
        [TestMethod("Static Divide(Complex,Real)")]
        public void Static_Divide_Complex_Real()
        {
            // Arrange
            Complex complex = new Complex(1.0, 6.0);
            Real real = new Real(4.0);
            Complex expected = new Complex(0.25, 1.5);
            //Act
            Complex actual = Complex.Divide(complex, real);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Complex.Divide(Real, Complex)"/>.
        /// </summary>
        [TestMethod("Static Divide(Real,Complex)")]
        public void Static_Divide_Real_Complex()
        {
            // Arrange
            Real real = new Real(5.0);
            Complex complex = new Complex(1.0, 3.0);
            Complex expected = new Complex(0.5, -1.5);
            // Act
            Complex actual = Complex.Divide(real, complex);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static method <see cref="Complex.Multiply(Complex, double)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Complex, double)")]
        public void Static_Multiply_Complex_double()
        {
            // Arrange
            Complex complex = new Complex(1.0, 2.5);
            double factor = 4.0;
            Complex expected = new Complex(4.0, 10.0);
            //Act
            Complex actual = Complex.Multiply(complex, factor);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Complex.Multiply(double, Complex)"/>.
        /// </summary>
        [TestMethod("Static Multiply(double,Complex)")]
        public void Static_Multiply_double_Complex()
        {
            // Arrange
            double factor = 4.0;
            Complex complex = new Complex(1.0, 2.5);
            Complex expected = new Complex(4.0, 10.0);
            //Act
            Complex actual = Complex.Multiply(factor, complex);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Complex.Divide(Complex, double)"/>.
        /// </summary>
        [TestMethod("Static Divide(Complex,double)")]
        public void Static_Divide_Complex_double()
        {
            // Arrange
            Complex complex = new Complex(1.0, 6.0);
            double divisor = 4.0;
            Complex expected = new Complex(0.25, 1.5);
            //Act
            Complex actual = Complex.Divide(complex, divisor);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        #endregion

        #region Operators

        /******************** Algebraic Field ********************/

        /// <summary>
        /// Tests the static operator <see cref="Complex.operator +(Complex,Complex)"/>.
        /// </summary>
        [TestMethod("Operator Add(Complex,Complex)")]
        public void Operator_Add_Complex_Complex()
        {
            // Arrange
            Complex complexA = new Complex(1.5, 6.0);
            Complex complexB = new Complex(5.5, 2.0);
            Complex expected = new Complex(7.0, 8.0);
            //Act
            Complex actual = complexA + complexB;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Complex.operator -(Complex,Complex)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Complex,Complex)")]
        public void Operator_Substract_Complex_Complex()
        {
            // Arrange
            Complex complexA = new Complex(1.5, 6.0);
            Complex complexB = new Complex(5.5, 2.0);
            Complex expected = new Complex(-4.0, 4.0);
            //Act
            Complex actual = complexA - complexB;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Complex.operator -(Complex)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Complex)")]
        public void Operator_Substract_Complex()
        {
            // Arrange
            Complex complex = new Complex(5.0, -15.0);
            Complex expected = new Complex(-5.0, 15.0);
            // Act
            Complex actual = -complex;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Complex.operator *(Complex,Complex)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Complex,Complex)")]
        public void Operator_Multiply_Complex_Complex()
        {
            // Arrange
            Complex complexA = new Complex(1.0, 2.5);
            Complex complexB = new Complex(2.0, 3.0);
            Complex expected = new Complex(-5.5, 8.0);
            //Act
            Complex actual = complexA * complexB;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Complex.operator /(Complex,Complex)"/>.
        /// </summary>
        [TestMethod("Operator Divide(Complex,Complex)")]
        public void Operator_Divide_Complex_Complex()
        {
            // Arrange
            Complex complexA = new Complex(4.0, 3.0);
            Complex complexB = new Complex(2.0, 1.0);
            Complex expected = new Complex(11.0 / 5.0, 2.0 / 5.0);
            // Act
            Complex actual = complexA / complexB;
            // Assert 
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Embedding : Real ********************/

        /// <summary>
        /// Tests the static operator <see cref="Complex.operator +(Complex,Real)"/>.
        /// </summary>
        [TestMethod("Operator Add(Complex,Real)")]
        public void Operator_Add_Complex_Real()
        {
            // Arrange
            Complex complex = new Complex(1.5, 6.0);
            Real real = new Real(10.0);
            Complex expected = new Complex(11.5, 6.0);
            //Act
            Complex actual = complex + real;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Complex.operator +(Real,Complex)"/>.
        /// </summary>
        [TestMethod("Operator Add(Real,Complex)")]
        public void Operator_Add_Real_Complex()
        {
            // Arrange
            Real real = new Real(10.0);
            Complex complex = new Complex(1.5, 6.0);
            Complex expected = new Complex(11.5, 6.0);
            //Act
            Complex actual = real + complex;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static operator <see cref="Complex.operator -(Complex,Real)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Complex,Real)")]
        public void Operator_Subtract_Complex_Real()
        {
            // Arrange
            Complex complex = new Complex(11.5, 5.0);
            Real real = new Real(10.0);
            Complex expected = new Complex(1.5, 5.0);
            //Act
            Complex actual = complex - real;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Complex.operator -(Real,Complex)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Real,Complex)")]
        public void Operator_Subtract_Real_Complex()
        {
            // Arrange

            Real real = new Real(10.0);
            Complex complex = new Complex(1.5, 6.0);
            Complex expected = new Complex(8.5, -6.0);
            //Act
            Complex actual = real - complex;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static operator <see cref="Complex.operator *(Complex,Real)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Complex,Real)")]
        public void Operator_Multiply_Complex_Real()
        {
            // Arrange
            Complex complex = new Complex(1.0, 2.5);
            Real real = new Real(4.0);
            Complex expected = new Complex(4.0, 10.0);
            //Act
            Complex actual = complex * real;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Complex.operator *(Real,Complex)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Real,Complex)")]
        public void Operator_Multiply_Real_Complex()
        {
            // Arrange
            Real real = new Real(4.0);
            Complex complex = new Complex(1.0, 2.5);
            Complex expected = new Complex(4.0, 10.0);
            //Act
            Complex actual = real * complex;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static operator <see cref="Complex.operator /(Complex,Real)"/>.
        /// </summary>
        [TestMethod("Operator Divide(Complex,Real)")]
        public void Operator_Divide_Complex_Real()
        {
            // Arrange
            Complex complex = new Complex(1.0, 6.0);
            Real real = new Real(4.0);
            Complex expected = new Complex(0.25, 1.5);
            //Act
            Complex actual = complex / real;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Complex.operator /(Real,Complex)"/>.
        /// </summary>
        [TestMethod("Operator Divide(Real,Complex)")]
        public void Operator_Divide_Real_Complex()
        {
            // Arrange
            Real real = new Real(5.0);
            Complex complex = new Complex(1.0, 3.0);
            Complex expected = new Complex(0.5, -1.5);
            // Act
            Complex actual = real / complex;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Embedding : double ********************/

        /// <summary>
        /// Tests the static operator <see cref="Complex.operator +(Complex,double)"/>.
        /// </summary>
        [TestMethod("Operator Add(Complex,double)")]
        public void Operator_Add_Complex_double()
        {
            // Arrange
            Complex complex = new Complex(1.5, 6.0);
            double number = 10.0;
            Complex expected = new Complex(11.5, 6.0);
            //Act
            Complex actual = complex + number;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Complex.operator +(double,Complex)"/>.
        /// </summary>
        [TestMethod("Operator Add(double,Complex)")]
        public void Operator_Add_double_Complex()
        {
            // Arrange
            double number = 10.0;
            Complex complex = new Complex(1.5, 6.0);
            Complex expected = new Complex(11.5, 6.0);
            //Act
            Complex actual = number + complex;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static operator <see cref="Complex.operator -(Complex,double)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Complex,double)")]
        public void Operator_Subtract_Complex_double()
        {
            // Arrange
            Complex complex = new Complex(11.5, 5.0);
            double number = 10.0;
            Complex expected = new Complex(1.5, 5.0);
            //Act
            Complex actual = complex - number;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Complex.operator -(double,Complex)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(double,Complex)")]
        public void Operator_Subtract_double_Complex()
        {
            // Arrange
            double number = 10.0;
            Complex complex = new Complex(1.5, 6.0);
            Complex expected = new Complex(8.5, -6.0);
            //Act
            Complex actual = number - complex;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static operator <see cref="Complex.operator *(Complex,double)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Complex,double)")]
        public void Operator_Multiply_Complex_double()
        {
            // Arrange
            Complex complex = new Complex(1.0, 2.5);
            double number = 4.0;
            Complex expected = new Complex(4.0, 10.0);
            //Act
            Complex actual = complex * number;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Complex.operator *(double,Complex)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(double,Complex)")]
        public void Operator_Multiply_double_Complex()
        {
            // Arrange
            double number = 4.0;
            Complex complex = new Complex(1.0, 2.5);
            Complex expected = new Complex(4.0, 10.0);
            //Act
            Complex actual = number * complex;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static operator <see cref="Complex.operator /(Complex,double)"/>.
        /// </summary>
        [TestMethod("Operator Divide(Complex,double)")]
        public void Operator_Divide_Complex_double()
        {
            // Arrange
            Complex complex = new Complex(1.0, 6.0);
            double number = 4.0;
            Complex expected = new Complex(0.25, 1.5);
            //Act
            Complex actual = complex / number;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Complex.operator /(double,Complex)"/>.
        /// </summary>
        [TestMethod("Operator Divide(double,Complex)")]
        public void Operator_Divide_double_Complex()
        {
            // Arrange
            double number = 5.0;
            Complex complex = new Complex(1.0, 3.0);
            Complex expected = new Complex(0.5, -1.5);
            // Act
            Complex actual = number / complex;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Tests the explicit conversion cast of a <see cref="Real"/> into a <see cref="Complex"/>.
        /// </summary>
        [TestMethod("Convert FromReal")]
        public void Convert_FromReal()
        {
            // Arrange
            Real real = new Real(10.0);
            Complex expected = new Complex(10.0, 0.0);
            // Act
            Complex actual = (Complex)real;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the explicit conversion of a <see cref="double"/> into a <see cref="Complex"/>.
        /// </summary>
        [TestMethod("Convert Fromdouble")]
        public void Convert_Fromdouble()
        {
            // Arrange
            double number = 20.0;
            Complex expected = new Complex(20.0, 0.0);
            // Act
            Complex actual = (Complex)number;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the explicit conversion of a <see cref="ValueTuple{T1,T2}"/> into a <see cref="Complex"/>.
        /// </summary>
        [TestMethod("Convert FromValueTuple")]
        public void Convert_FromValueTuple()
        {
            // Arrange
            ValueTuple<double, double> pair = new ValueTuple<double, double>(2.0, 4.0);
            Complex expected = new Complex(2.0, 4.0);
            // Act
            Complex actual = (Complex)pair;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tests the method <see cref="Complex.DotProduct(Complex)"/>.
        /// </summary>
        [TestMethod("Method DotProduct(Complex)")]
        public void DotProduct_Complex()
        {
            // Arrange
            Complex complexA = new Complex(3.0, 4.0);
            Complex complexB = new Complex(2.5, 1.5);
            Complex expected = new Complex(13.5, 5.5);
            // Act
            Complex actual = complexA.DotProduct(complexB);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the method <see cref="Complex.Norm()"/>.
        /// </summary>
        [TestMethod("Method Norm()")]
        public void Norm()
        {
            // Arrange
            Complex complex = new Complex(3.0, 4.0);
            double expected = 5d;
            // Act
            double actual = complex.Norm();
            // Assert
            Assert.AreEqual(expected, actual, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the method <see cref="Complex.DistanceTo(Complex)"/>.
        /// </summary>
        [TestMethod("Method DistanceTo(Complex)")]
        public void DistanceTo_Complex()
        {
            // Arrange
            Complex complexA = new Complex(3.0, 4.0);
            Complex complexB = new Complex(2.5, 1.5);
            double expected = Math.Sqrt(6.5);
            // Act
            double actual = complexA.DistanceTo(complexB);
            // Assert
            Assert.AreEqual(expected, actual, Settings.AbsolutePrecision);

        }


        /// <summary>
        /// Tests the method <see cref="Complex.Equals(Complex)"/>.
        /// </summary>
        [TestMethod("Method Equals(Complex)")]
        public void Equals_Complex()
        {
            // Arrange
            Complex complexA = new Complex(10.0, -4.0);
            Complex complexB = new Complex(10.0, -4.0);
            // Assert
            Assert.IsTrue(complexA.Equals(complexB));
        }

        #endregion


        #region Explicit Implementations

        /******************** IGroupAction<Complex, Real> ********************/

        /// <summary>
        /// Tests the <see cref="Alg_Sets.IGroupAction{TSelf, TValue}.Multiply(TValue)"/> method of <see cref="Complex"/>.
        /// </summary>
        [TestMethod("AsIGroupAction<Complex,Real> Multiply(Real)")]
        public void AsIGroupAction_Multiply_Real()
        {
            // Arrange
            Complex complex = new Complex(1.0, 2.5);
            Real real = new Real(4.0);
            Complex expected = new Complex(4.0, 10.0);
            //Act
            Alg_Sets.IGroupAction<Complex, Real> representation = (Alg_Sets.IGroupAction<Complex, Real>)complex;
            Complex actual = representation.Multiply(real);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the <see cref="Alg_Sets.IGroupAction{TSelf, TValue}.Divide(TValue)"/> method of <see cref="Complex"/>.
        /// </summary>
        [TestMethod("AsIGroupAction<Complex,Real> Divide(Real)")]
        public void AsIGroupAction_Divide_Real()
        {
            // Arrange
            Complex complex = new Complex(1.0, 6.0);
            Real real = new Real(4.0);
            Complex expected = new Complex(0.25, 1.5);
            //Act
            Alg_Sets.IGroupAction<Complex, Real> representation = (Alg_Sets.IGroupAction<Complex, Real>)complex;
            Complex actual = representation.Divide(real);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** IGroupAction<Complex, double> ********************/

        /// <summary>
        /// Tests the <see cref="Alg_Sets.IGroupAction{TSelf, TValue}.Multiply(TValue)"/> method of <see cref="Complex"/>.
        /// </summary>
        [TestMethod("AsIGroupAction<Complex,double> Multiply(double)")]
        public void AsIGroupAction_Multiply_double()
        {
            // Arrange
            Complex complex = new Complex(1.0, 2.5);
            double number = 4.0;
            Complex expected = new Complex(4.0, 10.0);
            //Act
            Alg_Sets.IGroupAction<Complex, double> representation = (Alg_Sets.IGroupAction<Complex, double>)complex;
            Complex actual = representation.Multiply(number);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the <see cref="Alg_Sets.IGroupAction{TSelf, TValue}.Divide(TValue)"/> method of <see cref="Complex"/>.
        /// </summary>
        [TestMethod("AsIGroupAction<Complex,double> Divide(double)")]
        public void AsIGroupAction_Divide_double()
        {
            // Arrange
            Complex complex = new Complex(1.0, 6.0);
            double number = 4.0;
            Complex expected = new Complex(0.25, 1.5);
            //Act
            Alg_Sets.IGroupAction<Complex, double> representation = (Alg_Sets.IGroupAction<Complex, double>)complex;
            Complex actual = representation.Divide(number);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        #endregion
    }
}
