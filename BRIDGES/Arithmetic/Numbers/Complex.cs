using System;

using Alg_Sets = BRIDGES.Algebra.Sets;
using Alg_Meas = BRIDGES.Algebra.Measure;


namespace BRIDGES.Arithmetic.Numbers
{
    /// <summary>
    /// Structure defining a complex number.
    /// </summary>
    public struct Complex :
        IEquatable<Complex>,
        Alg_Meas.IDotProduct<Complex, Complex>,
        Alg_Sets.IGroupAction<Complex, double>, Alg_Sets.IGroupAction<Complex, Real>
    {
        #region Fields

        /// <summary>
        /// Real component of this <see cref="Complex"/> number.
        /// </summary>
        private double _realPart;

        /// <summary>
        /// Imaginary component of this <see cref="Complex"/> number.
        /// </summary>
        private double _imaginaryPart;


        /// <summary>
        /// Modulus of this <see cref="Complex"/> number.
        /// </summary>
        /// <remarks> The field is lazily computed. </remarks>
        private double? _modulus;

        /// <summary>
        /// Argument of this <see cref="Complex"/> number.
        /// </summary>
        /// <remarks> The field is lazily computed. </remarks>
        private double? _argument;

        #endregion

        #region Properties

        /******************** Cartesian Coordinates ********************/

        /// <summary>
        /// Gets or sets the real component of this <see cref="Complex"/> number.
        /// </summary>
        public double RealPart
        {
            get => _realPart;
            set { _realPart = value; _modulus = null; _argument = null; }
        }

        /// <summary>
        /// Gets or sets the imaginary component of this <see cref="Complex"/> number.
        /// </summary>
        public double ImaginaryPart
        {
            get => _imaginaryPart;
            set { _imaginaryPart = value; _modulus = null; _argument = null; }
        }

        /******************** Polar Coordinates ********************/

        /// <summary>
        /// Gets the modulus of this <see cref="Complex"/> number.
        /// </summary>
        public double Modulus => _modulus == null ? Math.Sqrt((_realPart * _realPart) + (_imaginaryPart * _imaginaryPart)) : _modulus.Value;
        
        /// <summary>
        /// Gets the argument of this <see cref="Complex"/> number.
        /// </summary>
        /// <exception cref="InvalidOperationException"> The complex argument is not defined. </exception>
        public double Argument
        {
            get 
            {
                if (_argument == null & Modulus != 0d)
                {
                    return _imaginaryPart < 0d ? -Math.Acos(_realPart / Modulus) : Math.Acos(_realPart / Modulus);
                }
                else { throw new InvalidOperationException("The complex argument is not defined.", new DivideByZeroException("The complex modulus is zero")); }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="Complex"/> structure by defining it real and imaginary parts.
        /// </summary>
        /// <param name="real"> Value of the real part. </param>
        /// <param name="imaginary"> Value of the imaginary part. </param>
        public Complex(double real, double imaginary)
        {
            _realPart = real;
            _imaginaryPart = imaginary;

            _modulus = null;
            _argument = null;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Complex"/> structure from another <see cref="Complex"/> numbers.
        /// </summary>
        /// <param name="complex"> <see cref="Complex"/> number to deep copy. </param>
        public Complex(Complex complex)
        {
            _realPart = complex.RealPart;
            _imaginaryPart = complex.ImaginaryPart;

            _modulus = null;
            _argument = null;
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets a new instance of the <see cref="Complex"/> structure equal to the additive neutral element : <c>(0.0, 1.0)</c>.
        /// </summary>
        /// <returns> The new <see cref="Complex"/> number equal to zero. </returns>
        public static Complex Zero => new Complex(0d, 0d);

        /// <summary>
        /// Gets a new instance of the <see cref="Complex"/> structure equal to the multiplicative neutral element : <c>(0.0, 1.0)</c>.
        /// </summary>
        /// <returns> The new <see cref="Complex"/> number equal to one. </returns>
        public static Complex One => new Complex(1d, 0d);

        /// <summary>
        /// Gets a new instance of the <see cref="Complex"/> structure equal to the unit imaginary element : <c>(0.0, 1.0)</c>.
        /// </summary>
        /// <returns> The new <see cref="Complex"/> number equal to imaginary one. </returns>
        public static Complex ImaginaryOne => new Complex(0d, 1d);

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets a new <see cref="Complex"/> number by defining its polar coordinates.
        /// </summary>
        /// <param name="modulus"> Value of the modulus. </param>
        /// <param name="argument"> Value of the argument (in radians). </param>
        /// <returns> The new <see cref="Complex"/> number. </returns>
        public static Complex FromPolarCoordinates(double modulus, double argument)
        {
            return new Complex(modulus * Math.Cos(argument), modulus * Math.Sin(argument));
        }


        /// <summary>
        /// Gets the conjugate value of a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="complex"> <see cref="Complex"/> number from which the conjugate is computed. </param>
        /// <returns> The new <see cref="Complex"/> number, conjugate of the initial one. </returns>
        public static Complex Conjugate(Complex complex) { return new Complex(complex.RealPart, -complex.ImaginaryPart); }


        /******************** Algebraic Field ********************/

        /// <inheritdoc cref="operator +(Complex, Complex)"/>
        public static Complex Add(Complex left, Complex right) => left + right;

        /// <inheritdoc cref="operator -(Complex, Complex)"/>
        public static Complex Subtract(Complex left, Complex right) => left - right;


        /// <inheritdoc cref="operator -(Complex)"/>
        public static Complex Opposite(Complex operand) => -operand;


        /// <inheritdoc cref="operator *(Complex, Complex)"/>
        public static Complex Multiply(Complex left, Complex right) => left * right;

        /// <inheritdoc cref="operator /(Complex, Complex)"/>
        public static Complex Divide(Complex left, Complex right) => left / right;


        /// <summary>
        /// Computes the inverse of the given <see cref="Complex"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Complex"/> from which the inverse is computed. </param>
        /// <returns> The new <see cref="Complex"/> number, inverse of the initial one. </returns>
        public static Complex Inverse(Complex operand)
        {
            double norm = (operand.RealPart * operand.RealPart) + (operand.ImaginaryPart * operand.ImaginaryPart);

            return new Complex(operand.RealPart / norm, -operand.ImaginaryPart / norm);
        }


        /******************** Embedding : Real ********************/

        /// <inheritdoc cref="operator +(Complex, Real)"/>
        public static Complex Add(Complex left, Real right) => left + right;

        /// <inheritdoc cref="operator +(Real, Complex)"/>
        public static Complex Add(Real left, Complex right) => left + right;


        /// <inheritdoc cref="operator -(Complex, Real)"/>
        public static Complex Subtract(Complex left, Real right) => left - right;

        /// <inheritdoc cref="operator -(Real, Complex)"/>
        public static Complex Subtract(Real left, Complex right) => left - right;


        /// <inheritdoc cref="operator *(Complex, Real)"/>
        public static Complex Multiply(Complex left, Real right) => left * right;

        /// <inheritdoc cref="operator *(Real, Complex)"/>
        public static Complex Multiply(Real left, Complex right) => left * right;


        /// <inheritdoc cref="operator /(Complex, Real)"/>
        public static Complex Divide(Complex left, Real right) => left / right;

        /// <inheritdoc cref="operator /(Real, Complex)"/>
        public static Complex Divide(Real left, Complex right) => left / right;


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the right scalar multiplication of a <see cref="Complex"/> number with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Complex"/> number to multiply. </param>
        /// <param name="factor"> <see cref="double"/> number. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the right scalar multiplication. </returns>
        public static Complex Multiply(Complex operand, double factor) => operand * factor;

        /// <summary>
        /// Computes the left scalar multiplication of a <see cref="Complex"/> number with a <see cref="double"/> number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/> number. </param>
        /// <param name="operand"> <see cref="Complex"/> number to multiply. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the left scalar multiplication. </returns>
        public static Complex Multiply(double factor, Complex operand) => factor * operand;


        /// <summary>
        /// Computes the scalar division of a <see cref="Complex"/> number with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Complex"/> number to divide. </param>
        /// <param name="divisor"> <see cref="double"/> number to divide with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the scalar division. </returns>
        public static Complex Divide(Complex operand, double divisor) => operand / divisor;

        #endregion

        #region Operators

        /******************** Algebraic Field ********************/

        /// <summary>
        /// Computes the addition of two <see cref="Complex"/> numbers.
        /// </summary>
        /// <param name="left"> <see cref="Complex"/> number for the addition. </param>
        /// <param name="right"> <see cref="Complex"/> number for the addition. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the addition. </returns>
        public static Complex operator +(Complex left, Complex right)
        {
            return new Complex(left.RealPart + right.RealPart, left.ImaginaryPart + right.ImaginaryPart);
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="Complex"/> numbers.
        /// </summary>
        /// <param name="left"> <see cref="Complex"/> number to subtract. </param>
        /// <param name="right"> <see cref="Complex"/> number to subtract with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the subtraction. </returns>
        public static Complex operator -(Complex left, Complex right)
        {
            return new Complex(left.RealPart - right.RealPart, left.ImaginaryPart - right.ImaginaryPart);
        }


        /// <summary>
        /// Computes the opposite of the given <see cref="Complex"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Complex"/> number from which the opposite is computed. </param>
        /// <returns> The new <see cref="Complex"/> number, opposite of the initial one. </returns>
        public static Complex operator -(Complex operand) => new Complex(-operand.RealPart, -operand.ImaginaryPart); 


        /// <summary>
        /// Computes the multiplication of two <see cref="Complex"/> numbers.
        /// </summary>
        /// <param name="left"> <see cref="Complex"/> number for the multiplication. </param>
        /// <param name="right"> <see cref="Complex"/> number for the multiplication. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the multiplication. </returns>
        public static Complex operator *(Complex left, Complex right)
        {
            return new Complex((left.RealPart * right.RealPart) - (left.ImaginaryPart * right.ImaginaryPart),
                (left.RealPart * right.ImaginaryPart) + (left.ImaginaryPart * right.RealPart));
        }

        /// <summary>
        /// Computes the division of two <see cref="Complex"/> numbers.
        /// </summary>
        /// <param name="left"> <see cref="Complex"/> number to divide. </param>
        /// <param name="right"> <see cref="Complex"/> number to divide with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the division. </returns>
        public static Complex operator /(Complex left, Complex right)
        {
            double normB = ((right.RealPart * right.RealPart) + (right.ImaginaryPart * right.ImaginaryPart));

            return new Complex(((left.RealPart * right.RealPart) + (left.ImaginaryPart * right.ImaginaryPart)) / normB,
                ((left.ImaginaryPart * right.RealPart) - (left.RealPart * right.ImaginaryPart)) / normB);
        }


        /******************** Embedding : Real  ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="Complex"/> number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Complex"/> number for the addition. </param>
        /// <param name="right"> <see cref="Real"/> number for the addition. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the addition. </returns>
        public static Complex operator +(Complex left, Real right) => new Complex(left.RealPart + right.Value, left.ImaginaryPart);

        /// <summary>
        /// Computes the left addition of a <see cref="Complex"/> number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Real"/> number for the addition. </param>
        /// <param name="right"> <see cref="Complex"/> number for the addition. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the addition. </returns>
        public static Complex operator +(Real left, Complex right) => new Complex(left.Value + right.RealPart, right.ImaginaryPart);


        /// <summary>
        /// Computes the subtraction of a <see cref="Complex"/> number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Complex"/> number to subtract. </param>
        /// <param name="right"> <see cref="Real"/> number to subtract with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the subtraction. </returns>
        public static Complex operator -(Complex left, Real right) => new Complex(left.RealPart - right.Value, left.ImaginaryPart);

        /// <summary>
        /// Computes the subtraction of a <see cref="Real"/> number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Real"/> number to subtract. </param>
        /// <param name="right"> <see cref="Complex"/> number to subtract with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the subtraction. </returns>
        public static Complex operator -(Real left, Complex right) => new Complex(left.Value - right.RealPart, -right.ImaginaryPart);


        /// <summary>
        /// Computes the right multiplication of a <see cref="Complex"/> number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Complex"/> number for the multiplicaion. </param>
        /// <param name="right"> <see cref="Real"/> number for the multiplicaion. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the multiplication. </returns>
        public static Complex operator *(Complex left, Real right) => new Complex(left.RealPart * right.Value, left.ImaginaryPart * right.Value);

        /// <summary>
        /// Computes the left multiplication of a <see cref="Complex"/> number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Real"/> number for the multiplicaion. </param>
        /// <param name="right"> <see cref="Complex"/> number for the multiplicaion. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the multiplication. </returns>
        public static Complex operator *(Real left, Complex right) => new Complex(left.Value * right.RealPart, left.Value * right.ImaginaryPart);


        /// <summary>
        /// Computes the division of a <see cref="Complex"/> number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Complex"/> number to divide. </param>
        /// <param name="right"> <see cref="Real"/> number to divide with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the division. </returns>
        public static Complex operator /(Complex left, Real right) => new Complex(left.RealPart / right.Value, left.ImaginaryPart / right.Value);

        /// <summary>
        /// Computes the division of a <see cref="Real"/> number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Real"/> number to divide. </param>
        /// <param name="right"> <see cref="Complex"/> number to divide with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the division. </returns>
        public static Complex operator /(Real left, Complex right)
        {
            double norm = ((right.RealPart * right.RealPart) + (right.ImaginaryPart * right.ImaginaryPart));

            return new Complex(left.Value * (right.RealPart / norm), left.Value * (-right.ImaginaryPart / norm));
        }


        /******************** Embedding : double ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="Complex"/> number with a <see cref="double"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Complex"/> number for the addition. </param>
        /// <param name="right"> <see cref="double"/> number for the addition. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the addition. </returns>
        public static Complex operator +(Complex left, double right) => new Complex(left.RealPart + right, left.ImaginaryPart);

        /// <summary>
        /// Computes the left addition of a <see cref="Complex"/> number with a <see cref="double"/> number.
        /// </summary>
        /// <param name="left"> <see cref="double"/> number for the addition. </param>
        /// <param name="right"> <see cref="Complex"/> number for the addition. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the addition. </returns>
        public static Complex operator +(double left, Complex right) => new Complex(left + right.RealPart, right.ImaginaryPart);


        /// <summary>
        /// Computes the subtraction of a <see cref="Complex"/> number with a <see cref="double"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Complex"/> number to subtract. </param>
        /// <param name="right"> <see cref="double"/> number to subtract with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the subtraction. </returns>
        public static Complex operator -(Complex left, double right) => new Complex(left.RealPart - right, left.ImaginaryPart);

        /// <summary>
        /// Computes the subtraction of a <see cref="double"/> number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> <see cref="double"/> number to subtract. </param>
        /// <param name="right"> <see cref="Complex"/> number to subtract with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the subtraction. </returns>
        public static Complex operator -(double left, Complex right) => new Complex(left- right.RealPart, -right.ImaginaryPart);


        /// <summary>
        /// Computes the right multiplication of a <see cref="Complex"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="left"> <see cref="Complex"/> number for the multiplicaion. </param>
        /// <param name="right"> <see cref="double"/> number for the multiplicaion. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the multiplication. </returns>
        public static Complex operator *(Complex left, double right) => new Complex(left.RealPart * right, left.ImaginaryPart * right);

        /// <summary>
        /// Computes the left  multiplication of a <see cref="double"/> number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> <see cref="double"/> number for the multiplicaion. </param>
        /// <param name="right"> <see cref="Complex"/> number for the multiplicaion. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the multiplication. </returns>
        public static Complex operator *(double left, Complex right) => new Complex(left * right.RealPart, left * right.ImaginaryPart);


        /// <summary>
        /// Computes the division of a <see cref="Complex"/> number with a <see cref="double"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Complex"/> number to divide. </param>
        /// <param name="right"> <see cref="double"/> number to divide with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the division. </returns>
        public static Complex operator /(Complex left, double right) => new Complex(left.RealPart / right, left.ImaginaryPart / right);

        /// <summary>
        /// Computes the division of a <see cref="double"/> number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> <see cref="double"/> number to divide. </param>
        /// <param name="right"> <see cref="Complex"/> number to divide with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the division. </returns>
        public static Complex operator /(double left, Complex right)
        {
            double norm = ((right.RealPart * right.RealPart) + (right.ImaginaryPart * right.ImaginaryPart));

            return new Complex(left * (right.RealPart / norm), left * (-right.ImaginaryPart / norm));
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts a <see cref="Real"/> number into a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="real"> <see cref="Real"/> number to convert. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the conversion. </returns>
        public static explicit operator Complex(Real real) => new Complex(real.Value, 0d);

        /// <summary>
        /// Converts a <see cref="double"/>-precision real number into a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="number"> <see cref="double"/>-precision real number to convert. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the conversion. </returns>
        public static explicit operator Complex(double number) => new Complex(number, 0d);

        /// <summary>
        /// Converts a <see cref="ValueTuple{T1, T2}"/> into a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="pair"> <see cref="ValueTuple{T1, T2}"/> to convert. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the conversion. </returns>
        public static explicit operator Complex(ValueTuple<double, double> pair) => new Complex(pair.Item1, pair.Item2);

        #endregion

        #region Public Methods

        /// <summary>
        /// Computes the hermitian dot product of this <see cref="Complex"/> number with another <see cref="Complex"/> number.
        /// </summary>
        /// <param name="other"> Right <see cref="Complex"/> for number the dot product. </param>
        /// <returns> The value of the dot product of the two elements. </returns>
        public Complex DotProduct(Complex other)
        {
            return new Complex((RealPart * other.RealPart) + (ImaginaryPart * other.ImaginaryPart),
                (ImaginaryPart * other.RealPart) - (RealPart * other.ImaginaryPart));
        }

        /// <inheritdoc cref="Alg_Meas.INorm{TSelf}.Norm()"/>
        public double Norm() => Math.Sqrt((RealPart * RealPart) + (ImaginaryPart * ImaginaryPart));
        
        /// <inheritdoc cref="Alg_Meas.IMetric{TSelf}.DistanceTo(TSelf)"/>
        public double DistanceTo(Complex other) => (this - other).Norm();


        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(Complex other) 
        { 
            return (Math.Abs(RealPart - other.RealPart) < Settings.AbsolutePrecision && Math.Abs(ImaginaryPart - other.ImaginaryPart) < Settings.AbsolutePrecision); 
        }

        #endregion


        #region Overrides

        /******************** object ********************/

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Complex complex && Equals(complex);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"({RealPart}, {ImaginaryPart})";
        }

        #endregion

        #region Explicit Implementations

        /******************** IGroupAction<Complex, Real> ********************/

        /// <inheritdoc/>
        Complex Alg_Sets.IGroupAction<Complex, Real>.Multiply(Real factor) => this * factor;

        /// <inheritdoc/>
        Complex Alg_Sets.IGroupAction<Complex, Real>.Divide(Real divisor) => this / divisor;


        /******************** IGroupAction<Complex, double> ********************/

        /// <inheritdoc/>
        Complex Alg_Sets.IGroupAction<Complex, double>.Multiply(double factor) => this * factor;

        /// <inheritdoc/>
        Complex Alg_Sets.IGroupAction<Complex, double>.Divide(double divisor) => this /divisor;

        #endregion
    }

}
