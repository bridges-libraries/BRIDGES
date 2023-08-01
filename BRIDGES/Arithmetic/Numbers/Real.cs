using System;
using System.Numerics;

using Alg_Meas = BRIDGES.Algebra.Measure;


namespace BRIDGES.Arithmetic.Numbers
{
    /// <summary>
    /// Structure defining a real number.
    /// </summary>
    public struct Real : 
        IEquatable<Real>,
        IAdditionOperators<Real, Real, Real>, ISubtractionOperators<Real, Real, Real>,
        IMultiplyOperators<Real, double, Real>, IDivisionOperators<Real, double, Real>,
        Alg_Meas.IDotProduct<Real,double>
    {
        #region Properties

        /// <summary>
        /// Value of the real numbers.
        /// </summary>
        public double Value { get; set; }

        #endregion

        #region Constructors
        
        /// <summary>
        /// Initialises a new instance of the <see cref="Real"/> structure by defining its value.
        /// </summary>
        /// <param name="number"> Value of the real number. </param>
        public Real(double number)
        {
            Value = number;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Real"/> structure from another <see cref="Real"/> number.
        /// </summary>
        /// <param name="real"> <see cref="Real"/> number to deep copy. </param>
        public Real(Real real)
        {
            Value = real.Value;
        }

        #endregion
        
        #region Static Properties

        /// <summary>
        /// Gets a new instance of the <see cref="Real"/> structure equal to zero : <c>0.0</c>.
        /// </summary>
        public static Real Zero => new Real(0d);

        /// <summary>
        /// Gets a new instance of the <see cref="Real"/> structure equal to one  : <c>1.0</c>..
        /// </summary>
        /// <returns> The new <see cref="Real"/> number equal to one. </returns>
        public static Real One => new Real(1d);

        #endregion

        #region Static Methods

        /******************** Algebraic Field ********************/

        /// <inheritdoc cref="operator +(Real, Real)"/>
        public static Real Add(Real left, Real right) => left + right;

        /// <inheritdoc cref="operator -(Real, Real)"/>
        public static Real Subtract(Real left, Real right) => left - right;


        /// <inheritdoc cref="operator -(Real)"/>
        public static Real Opposite(Real operand) => -operand;


        /// <inheritdoc cref="operator *(Real, Real)"/>
        public static Real Multiply(Real left, Real right) => left * right;

        /// <inheritdoc cref="operator /(Real, Real)"/>
        public static Real Divide(Real left, Real right) => left / right;


        /// <summary>
        /// Computes the inverse of the <see cref="Real"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Real"/> number from which the inverse is computed. </param>
        /// <returns> The new <see cref="Real"/> number, inverse of the initial one. </returns>
        public static Real Inverse(Real operand) => 1d / operand;


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the left scalar multiplication of a <see cref="Real"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <param name="operand"> <see cref="Real"/> number to multiply. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the scalar multiplication. </returns>
        public static Real Multiply(Real operand, double factor) => operand * factor;

        /// <summary>
        /// Computes the right scalar multiplication of a <see cref="Real"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <param name="operand"> <see cref="Real"/> number to multiply. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the scalar multiplication. </returns>
        public static Real Multiply(double factor, Real operand) => factor * operand;


        /// <summary>
        /// Computes the scalar division of a <see cref="Real"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="operand"> <see cref="Real"/> number to divide. </param>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the scalar division. </returns>
        public static Real Divide(Real operand, double divisor) => operand / divisor;

        #endregion

        #region Operators

        /******************** Algebraic Field ********************/

        /// <summary>
        /// Computes the addition of two <see cref="Real"/> numbers.
        /// </summary>
        /// <param name="left"> Left <see cref="Real"/> number for the addition. </param>
        /// <param name="right"> Right <see cref="Real"/> number for the addition. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the addition. </returns>
        public static Real operator +(Real left, Real right) => new Real(left.Value + right.Value);

        /// <summary>
        /// Computes the subtraction of two <see cref="Real"/> numbers.
        /// </summary>
        /// <param name="left"> Left <see cref="Real"/> number to subtract. </param>
        /// <param name="right"> Right <see cref="Real"/> number to subtract with. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the subtraction. </returns>
        public static Real operator -(Real left, Real right) => new Real(left.Value - right.Value);


        /// <summary>
        /// Computes the opposite of the <see cref="Real"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Real"/> number from which the opposite is computed. </param>
        /// <returns> The new <see cref="Real"/> number, opposite of the initial one. </returns>
        public static Real operator -(Real operand) => new Real(-operand.Value);


        /// <summary>
        /// Computes the multiplication of two <see cref="Real"/> numbers.
        /// </summary>
        /// <param name="left"> Left <see cref="Real"/> number for the multiplication. </param>
        /// <param name="right"> Right <see cref="Real"/> number for the multiplication. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the multiplication. </returns>
        public static Real operator *(Real left, Real right) => new Real(left.Value * right.Value);

        /// <summary>
        /// Computes the division of two <see cref="Real"/> numbers.
        /// </summary>
        /// <param name="left"> <see cref="Real"/> number to divide. </param>
        /// <param name="right"> <see cref="Real"/> number to divide with. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the division. </returns>
        public static Real operator /(Real left, Real right) => new Real(left.Value / right.Value);


        /******************** Embedding : double ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="Real"/> number with a <see cref="double"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Real"/> number for the addition. </param>
        /// <param name="right"> <see cref="double"/> number for the addition. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the addition. </returns>
        public static Real operator +(Real left, double right) => new Real(left.Value + right);

        /// <summary>
        /// Computes the left addition of a <see cref="Real"/> number with a <see cref="double"/> number.
        /// </summary>
        /// <param name="left"> <see cref="double"/> number for the addition. </param>
        /// <param name="right"> <see cref="Real"/> number for the addition. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the addition. </returns>
        public static Real operator +(double left, Real right) => new Real(left + right.Value);


        /// <summary>
        /// Computes the subtraction of a <see cref="Real"/> number with a <see cref="double"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Real"/> number to subtract. </param>
        /// <param name="right"> <see cref="double"/> number to subtract with. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the subtraction. </returns>
        public static Real operator -(Real left, double right) => new Real(left.Value - right);

        /// <summary>
        /// Computes the subtraction of a <see cref="double"/> number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="left"> <see cref="double"/> number to subtract. </param>
        /// <param name="right"> <see cref="Real"/> number to subtract with. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the subtraction. </returns>
        public static Real operator -(double left, Real right) => new Real(left - right.Value);


        /// <summary>
        /// Computes the right multiplication of a <see cref="Real"/> number with a <see cref="double"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Real"/> number for the multiplicaion. </param>
        /// <param name="right"> <see cref="double"/> number for the multiplicaion. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the multiplication. </returns>
        public static Real operator *(Real left, double right) => new Real(left.Value * right);

        /// <summary>
        /// Computes the left multiplication of a <see cref="Real"/> number with a <see cref="double"/> number.
        /// </summary>
        /// <param name="left"> <see cref="double"/> number for the multiplicaion. </param>
        /// <param name="right"> <see cref="Real"/> number for the multiplicaion. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the multiplication. </returns>
        public static Real operator *(double left, Real right) => new Real(left * right.Value);


        /// <summary>
        /// Computes the division of a <see cref="Real"/> number with a <see cref="double"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Real"/> number to divide. </param>
        /// <param name="right"> <see cref="double"/> number to divide with. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the division. </returns>
        public static Real operator /(Real left, double right) => new Real(left.Value / right);

        /// <summary>
        /// Computes the division of a <see cref="double"/> number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="left"> <see cref="double"/> number to divide. </param>
        /// <param name="right"> <see cref="Real"/> number to divide with. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the division. </returns>
        public static Real operator /(double left, Real right) => new Real(left / right.Value);

        #endregion

        #region Conversions

        /// <summary>
        /// Converts a <see cref="Real"/> number into a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="real"> <see cref="Real"/> number to convert. </param>
        /// <returns> The new <see cref="double"/>-precision real number resulting from the conversion. </returns>
        public static explicit operator double(Real real) => real.Value;

        /// <summary>
        /// Converts a <see cref="double"/>-precision real number into a <see cref="Real"/> number.
        /// </summary>
        /// <param name="number"> <see cref="double"/>-precision real number to convert. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the conversion. </returns>
        public static explicit operator Real(double number) => new Real(number);

        #endregion

        #region Public Methods

        /// <inheritdoc cref="Alg_Meas.IDotProduct{TSelf, TValue}.DotProduct(TSelf)"/>
        public double DotProduct(Real other) => Value * other.Value;

        /// <inheritdoc cref="Alg_Meas.INorm{TSelf}.Norm()"/>
        public double Norm() => Math.Abs(Value);

        /// <inheritdoc cref="Alg_Meas.IMetric{TSelf}.DistanceTo(TSelf)"/>
        public double DistanceTo(Real other) => Math.Abs(Value - other.Value);


        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(Real other) => Math.Abs(Value - other.Value) < Settings.AbsolutePrecision;

        #endregion


        #region Override : Object

        /******************** object ********************/

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Real real && Equals(real);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{Value}";
        }

        #endregion
    }
}
