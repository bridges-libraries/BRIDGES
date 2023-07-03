using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.Arithmetic.Numbers;

using Alg_Sets = BRIDGES.Algebra.Sets;


namespace BRIDGES.Tests.Arithmetic.Numbers
{
    /// <summary>
    /// Class testing the members of the <see cref="Real"/> structure.
    /// </summary>
    [TestClass]
    public class RealTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="Real"/> is not reference type.
        /// </summary>
        [TestMethod("Behavior IsNotReference")]
        public void IsNotReference()
        {
            // Arrange
            Real realA = new Real(1d);
            Real realB = new Real(3d);
            //Act
            realA = realB;
            // Assert
            Assert.IsTrue(realB.Equals(realA));
            Assert.AreNotSame(realA, realB);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the initialisation of the <see cref="Real"/> from its value and the <see cref="Real.Value"/> property.
        /// </summary>
        [TestMethod("Property Value")]
        public void Value()
        {
            // Assign
            Real actual = new Real(2.5);
            double expected = 2.5;
            // Act

            // Assert
            Assert.AreEqual(expected, actual.Value, Settings.AbsolutePrecision);
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Tests the static property <see cref="Real.Zero"/>.
        /// </summary>
        [TestMethod("Static Zero")]
        public void Static_Zero()
        {
            // Arrange
            Real expected = new Real(0d);
            // Act
            Real actual = Real.Zero;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static property <see cref="Real.One"/>.
        /// </summary>
        [TestMethod("Static One")]
        public void Static_One()
        {
            // Arrange
            Real expected = new Real(1d);
            // Act
            Real actual = Real.One;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        #endregion

        #region Static Methods

        /******************** Algebraic Field ********************/

        /// <summary>
        /// Tests the static method <see cref="Real.Add(Real, Real)"/>.
        /// </summary>
        [TestMethod("Static Add(Real,Real)")]
        [DataRow(1.5, 5.5, 7d, DisplayName = "Add(Positive,Positive)")]
        [DataRow(-2.3, 7.5, 5.2, DisplayName = "Add(Negative,Positive)")]
        [DataRow(5.2, -9.1, -3.9, DisplayName = "Add(Positive,Negative)")]
        [DataRow(-2.5, -1.3, -3.8, DisplayName = "Add(Negative,Negative)")]
        public void Static_Add_Real_Real(double left, double right, double sum)
        {
            // Arrange
            Real realA = new Real(left);
            Real realB = new Real(right);
            Real expected = new Real(sum);
            //Act
            Real actual = Real.Add(realA, realB);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Real.Subtract(Real, Real)"/>.
        /// </summary>
        [TestMethod("Static Subtract(Real,Real)")]
        public void Static_Substract_Real_Real()
        {
            // Arrange
            Real realA = new Real(1.5);
            Real realB = new Real(5.5);
            Real expected = new Real(-4.0);
            //Act
            Real actual = Real.Subtract(realA, realB);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static method <see cref="Real.Opposite(Real)"/>.
        /// </summary>
        [TestMethod("Static Opposite(Real)")]
        public void Static_Opposite_Real()
        {
            // Arrange
            Real real = new Real(5.0);
            Real expected = new Real(-5.0);
            // Act
            Real actual = Real.Opposite(real);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static method <see cref="Real.Multiply(Real, Real)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Real,Real)")]
        public void Static_Multiply_Real_Real()
        {
            // Arrange
            Real realA = new Real(2.4);
            Real realB = new Real(2.0);
            Real expected = new Real(4.8);
            //Act
            Real actual = Real.Multiply(realA, realB);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Real.Divide(Real, Real)"/>.
        /// </summary>
        [TestMethod("Static Divide(Real,Real)")]
        public void Static_Divide_Real_Real()
        {
            // Arrange
            Real realA = new Real(4.9);
            Real realB = new Real(2.0);
            Real expected = new Real(2.45);
            // Act
            Real actual = Real.Divide(realA, realB);
            // Assert 
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static method <see cref="Real.Inverse(Real)"/>.
        /// </summary>
        [TestMethod("Static Inverse(Real)")]
        public void Static_Inverse_Real()
        {
            // Arrange
            Real real = new Real(5.0);
            Real expected = new Real(0.2);
            // Act
            Real actual = Real.Inverse(real);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static method <see cref="Real.Multiply(Real, double)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Real, double)")]
        public void Static_Multiply_Real_double()
        {
            // Arrange
            Real real = new Real(5.0);
            double factor = 4.5;
            Real expected = new Real(22.5);
            // Act
            Real actual = Real.Multiply(real, factor);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Real.Multiply(double, Real)"/>.
        /// </summary>
        [TestMethod("Static Multiply(double, Real)")]
        public void Static_Multiply_double_Real()
        {
            // Arrange
            double factor = 4.5;
            Real real = new Real(5.0);
            Real expected = new Real(22.5);
            // Act
            Real actual = Real.Multiply(factor, real);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static method <see cref="Real.Divide(Real, double)"/>.
        /// </summary>
        [TestMethod("Static Divide(Real, double)")]
        public void Static_Divide_Real_double()
        {
            // Arrange
            Real real = new Real(4.5);
            double divisor = 5d;
            Real expected = new Real(0.9);
            // Act
            Real actual = Real.Divide(real, divisor);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        #endregion

        #region Operators

        /******************** Algebraic Field ********************/

        /// <summary>
        /// Tests the static operator <see cref="Real.operator +(Real,Real)"/>.
        /// </summary>
        [TestMethod("Operator Add(Real,Real)")]
        public void Operator_Add_Real_Real()
        {
            // Arrange
            Real realA = new Real(1.5);
            Real realB = new Real(5.5);
            Real expected = new Real(7.0);
            //Act
            Real actual = realA + realB;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Real.operator -(Real,Real)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Real,Real)")]
        public void Operator_Substract_Real_Real()
        {
            // Arrange
            Real realA = new Real(1.5);
            Real realB = new Real(5.5);
            Real expected = new Real(-4.0);
            //Act
            Real actual = realA - realB;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Real.operator -(Real)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Real)")]
        public void Operator_Substract_Real()
        {
            // Arrange
            Real real = new Real(5.0);
            Real expected = new Real(-5.0);
            // Act
            Real actual = -real;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Real.operator *(Real,Real)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Real,Real)")]
        public void Operator_Multiply_Real_Real()
        {
            // Arrange
            Real realA = new Real(1.2);
            Real realB = new Real(-2.5);
            Real expected = new Real(-3.0);
            //Act
            Real actual = realA * realB;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Real.operator /(Real,Real)"/>.
        /// </summary>
        [TestMethod("Operator Divide(Real,Real)")]
        public void Operator_Divide_Real_Real()
        {
            // Arrange
            Real realA = new Real(5.0);
            Real realB = new Real(2.0);
            Real expected = new Real(2.5);
            // Act
            Real actual = realA / realB;
            // Assert 
            Assert.IsTrue(expected.Equals(actual));
        }


        /******************** Embedding : double ********************/

        /// <summary>
        /// Tests the static operator <see cref="Real.operator +(Real,double)"/>.
        /// </summary>
        [TestMethod("Operator Add(Real,double)")]
        public void Operator_Add_Real_double()
        {
            // Arrange
            Real real = new Real(1.5);
            double number = 10.0;
            Real expected = new Real(11.5);
            //Act
            Real actual = real + number;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Real.operator +(double,Real)"/>.
        /// </summary>
        [TestMethod("Operator Add(double,Real)")]
        public void Operator_Add_double_Real()
        {
            // Arrange
            double number = 10.0;
            Real real = new Real(1.5);
            Real expected = new Real(11.5);
            //Act
            Real actual = number + real;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static operator <see cref="Real.operator -(Real,double)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Real,double)")]
        public void Operator_Subtract_Real_double()
        {
            // Arrange
            Real real = new Real(11.5);
            double number = 10.0;
            Real expected = new Real(1.5);
            //Act
            Real actual = real - number;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Real.operator -(double,Real)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(double,Real)")]
        public void Operator_Subtract_double_Real()
        {
            // Arrange
            double number = 10.0;
            Real real = new Real(1.5);
            Real expected = new Real(8.5);
            //Act
            Real actual = number - real;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static operator <see cref="Real.operator *(Real,double)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Real,double)")]
        public void Operator_Multiply_Real_double()
        {
            // Arrange
            Real real = new Real(2.0);
            double number = 4.0;
            Real expected = new Real(8.0);
            //Act
            Real actual = real * number;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Real.operator *(double,Real)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(double,Real)")]
        public void Operator_Multiply_double_Real()
        {
            // Arrange
            double number = 4.0;
            Real real = new Real(0.75);
            Real expected = new Real(3.0);
            //Act
            Real actual = number * real;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }


        /// <summary>
        /// Tests the static operator <see cref="Real.operator /(Real,double)"/>.
        /// </summary>
        [TestMethod("Operator Divide(Real,double)")]
        public void Operator_Divide_Real_double()
        {
            // Arrange
            Real real = new Real(2.5);
            double number = -5.0;
            Real expected = new Real(-0.5);
            //Act
            Real actual = real / number;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the static operator <see cref="Real.operator /(double,Real)"/>.
        /// </summary>
        [TestMethod("Operator Divide(double,Real)")]
        public void Operator_Divide_double_Real()
        {
            // Arrange
            double number = 5.0;
            Real real = new Real(2.0);
            Real expected = new Real(2.5);
            // Act
            Real actual = number / real;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Tests the explicit conversion of a <see cref="Real"/> into a <see cref="double"/>.
        /// </summary>
        [TestMethod("Convert Todouble")]
        public void Convert_Todouble()
        {
            // Arrange
            Real real = new Real(5.0);
            double expected = 5.0;
            // Act
            double actual = (double)real;
            // Assert
            Assert.AreEqual(expected, actual, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the explicit conversion of a <see cref="double"/> into a <see cref="Real"/>.
        /// </summary>
        [TestMethod("Convert Fromdouble")]
        public void Convert_Fromdouble()
        {
            // Arrange
            double number = 20.0;
            Real expected = new Real(20.0);
            // Act
            Real actual = (Real)number;
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tests the method <see cref="Real.DotProduct(Real)"/>.
        /// </summary>
        [TestMethod("Method DotProduct(Real)")]
        public void DotProduct_Real()
        {
            // Arrange
            Real realA = new Real(9.5);
            Real realB = new Real(4.3);
            double expected = 40.85;
            // Act 
            double actual = realA.DotProduct(realB);
            // Assert
            Assert.AreEqual(expected, actual, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the method <see cref="Real.Norm()"/>.
        /// </summary>
        [TestMethod("Method Norm()")]
        public void Norm()
        {
            // Arrange
            Real real = new Real(-9.25);
            double expected = 9.25;
            // Act 
            double actual = real.Norm();
            // Assert
            Assert.AreEqual(expected, actual, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the method <see cref="Real.DistanceTo(Real)"/>.
        /// </summary>
        [TestMethod("Method DistanceTo(Real)")]
        public void DistanceTo_Real()
        {
            // Arrange
            Real realA = new Real(-9.25);
            Real realB = new Real(2.5);
            double expected = 11.75;
            // Act 
            double actual = realA.DistanceTo(realB);
            // Assert
            Assert.AreEqual(expected, actual, Settings.AbsolutePrecision);
        }


        /// <summary>
        /// Tests the method <see cref="Real.Equals(Real)"/>.
        /// </summary>
        [TestMethod("Method Equals(Real)")]
        public void Equals_Real()
        {
            // Arrange
            Real realA = new Real(9.85);
            Real realB = new Real(9.85);
            // Assert
            Assert.IsTrue(realA.Equals(realB));
        }

        #endregion


        #region Explicit Implementations

        /******************** IGroupAction<Real, double> ********************/

        /// <summary>
        /// Tests the <see cref="Alg_Sets.IGroupAction{TSelf, TValue}.Multiply(TValue)"/> method of <see cref="Real"/>.
        /// </summary>
        [TestMethod("AsIGroupAction<Real,double> Multiply(double)")]
        public void AsIGroupAction_Multiply_double()
        {
            // Arrange
            Real real = new Real(2.5);
            double factor = 4.0;
            Real expected = new Real(10.0);
            //Act
            Alg_Sets.IGroupAction<Real, double> representation = (Alg_Sets.IGroupAction<Real, double>)real;
            Real actual = representation.Multiply(factor);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests the <see cref="Alg_Sets.IGroupAction{TSelf, TValue}.Divide(TValue)"/> method of <see cref="Real"/>.
        /// </summary>
        [TestMethod("AsIGroupAction<Real,double> Divide(double)")]
        public void AsIGroupAction_Divide_double()
        {
            // Arrange
            Real real = new Real(2.5);
            double factor = -4.0;
            Real expected = new Real(-0.625);
            //Act
            Alg_Sets.IGroupAction<Real, double> representation = (Alg_Sets.IGroupAction<Real, double>)real;
            Real actual = representation.Divide(factor);
            // Assert
            Assert.IsTrue(expected.Equals(actual));
        }

        #endregion
    }
}
