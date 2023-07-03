using System;

using Alg_Sets = BRIDGES.Algebra.Sets;
using Alg_Meas = BRIDGES.Algebra.Measure;


namespace BRIDGES.Arithmetic.Numbers
{
    /// <summary>
    /// Structure defining quaternion number.
    /// </summary>
    public struct Quaternion :
        IEquatable<Quaternion>,
        Alg_Meas.IDotProduct<Quaternion, double>,
        Alg_Sets.IGroupAction<Quaternion, double>, Alg_Sets.IGroupAction<Quaternion, Real>, Alg_Sets.IGroupAction<Quaternion, Complex>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the scalar part of this <see cref="Quaternion"/> number.
        /// </summary>
        public double ScalarPart { get; set; }

        /// <summary>
        /// Gets or sets the first component "i" of the vector part of this <see cref="Quaternion"/> number.
        /// </summary>
        public double I { get; set; }

        /// <summary>
        /// Gets or sets the second component "j" of the vector part of this <see cref="Quaternion"/> number.
        /// </summary>
        public double J { get; set; }

        /// <summary>
        /// Gets or sets the third component "k" of the vector part of this <see cref="Quaternion"/> number.
        /// </summary>
        public double K { get; set; }

        /// <summary>
        /// Gets the vector part of this <see cref="Quaternion"/> number.
        /// </summary>
        public double[] VectorPart
        {
            get { return new double[3] { I, J, K }; }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="Quaternion"/> structure by defining its real and imaginary components.
        /// </summary>
        /// <param name="r"> Value of the first component. </param>
        /// <param name="i"> Value of the second component. </param>
        /// <param name="j"> Value of the third component. </param>
        /// <param name="k"> Value of the fourth component. </param>
        public Quaternion(double r, double i, double j, double k)
        {
            ScalarPart = r; I = i; J = j; K = k;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Quaternion"/> structure by defining its components.
        /// </summary>
        /// <param name="components"> Value of the components. </param>
        public Quaternion(double[] components)
        {
            if(components.Length != 4) { throw new ArgumentException("The length of the components array is different from four, the dimension of quaternions."); }
            
            ScalarPart = components[0]; 
            I = components[1]; 
            J = components[2]; 
            K = components[3];
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Quaternion"/> structure from another <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="quaternion"> <see cref="Quaternion"/> number to deep copy. </param>
        public Quaternion(Quaternion quaternion)
        {
            ScalarPart = quaternion.ScalarPart;
            I = quaternion.I;
            J = quaternion.J;
            K = quaternion.K;
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets a new instance of the <see cref="Quaternion"/> structure equal to the additive neutral element : <c>(0.0, 0.0, 0.0, 0.0)</c>.
        /// </summary>
        /// <returns> The new <see cref="Quaternion"/> number equal to zero. </returns>
        public static Quaternion Zero => new Quaternion(0d, 0d, 0d, 0d);

        /// <summary>
        /// Gets a new instance of the <see cref="Quaternion"/> structure equal to the multiplicative neutral element : <c>(1.0, 0.0, 0.0, 0.0)</c>.
        /// </summary>
        /// <returns> The new <see cref="Quaternion"/> number equal to one. </returns>
        public static Quaternion One => new Quaternion(1d, 0d, 0d, 0d);

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets the conjugate value of a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="quaternion"> <see cref="Quaternion"/> number from which the conjugate is computed. </param>
        /// <returns> The new <see cref="Quaternion"/> number, conjugate of the initial one. </returns>
        public static Quaternion Conjugate(Quaternion quaternion) => new Quaternion(quaternion.ScalarPart, -quaternion.I, -quaternion.J, -quaternion.K); 


        /******************** Algebraic Field ********************/

        /// <inheritdoc cref="operator +(Quaternion, Quaternion)"/>
        public static Quaternion Add(Quaternion left, Quaternion right) => left + right;

        /// <inheritdoc cref="operator -(Quaternion, Quaternion)"/>
        public static Quaternion Subtract(Quaternion left, Quaternion right) => left - right;


        /// <inheritdoc cref="operator -(Quaternion)"/>
        public static Quaternion Opposite(Quaternion operand) => - operand;


        /// <inheritdoc cref="operator *(Quaternion, Quaternion)"/>
        public static Quaternion Multiply(Quaternion left, Quaternion right) => left * right;

        /// <inheritdoc cref="operator /(Quaternion, Quaternion)"/>
        public static Quaternion Divide(Quaternion left, Quaternion right) => left / right;


        /// <summary>
        /// Computes the inverse of this <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Quaternion"/> from which the inverse is computed. </param>
        /// <returns> The new <see cref="Quaternion"/> number, inverse of the initial one. </returns>
        public static Quaternion Inverse(Quaternion operand)
        {
            double norm = (operand.ScalarPart * operand.ScalarPart) + (operand.I * operand.I) + (operand.J * operand.J) + (operand.K * operand.K);

            return new Quaternion(operand.ScalarPart / norm, -operand.I / norm, -operand.J / norm, -operand.K / norm);
        }


        /******************** Embedding : Complex ********************/

        /// <inheritdoc cref="operator +(Quaternion, Complex)"/>
        public static Quaternion Add(Quaternion left, Complex right) => left + right;

        /// <inheritdoc cref="operator +(Complex, Quaternion)"/>
        public static Quaternion Add(Complex left, Quaternion right) => left + right;


        /// <inheritdoc cref="operator -(Quaternion, Complex)"/>
        public static Quaternion Subtract(Quaternion left, Complex right) => left - right;

        /// <inheritdoc cref="operator -(Complex, Quaternion)"/>
        public static Quaternion Subtract(Complex left, Quaternion right) => left - right;


        /// <inheritdoc cref="operator *(Quaternion, Complex)"/>
        public static Quaternion Multiply(Quaternion left, Complex right) => left * right;

        /// <inheritdoc cref="operator *(Complex, Quaternion)"/>
        public static Quaternion Multiply(Complex left, Quaternion right) => left * right;


        /// <inheritdoc cref="operator /(Quaternion, Complex)"/>
        public static Quaternion Divide(Quaternion left, Complex right) => left / right;

        /// <inheritdoc cref="operator /(Complex, Quaternion)"/>
        public static Quaternion Divide(Complex left, Quaternion right) => left / right;


        /******************** Embedding : Real ********************/

        /// <inheritdoc cref="operator +(Quaternion, Real)"/>
        public static Quaternion Add(Quaternion left, Real right) => left + right;

        /// <inheritdoc cref="operator +(Real, Quaternion)"/>
        public static Quaternion Add(Real left, Quaternion right) => left + right;


        /// <inheritdoc cref="operator -(Quaternion, Real)"/>
        public static Quaternion Subtract(Quaternion left, Real right) => left - right;

        /// <inheritdoc cref="operator -(Real, Quaternion)"/>
        public static Quaternion Subtract(Real left, Quaternion right) => left - right;


        /// <inheritdoc cref="operator *(Quaternion, Real)"/>
        public static Quaternion Multiply(Quaternion left, Real right) => left * right;

        /// <inheritdoc cref="operator *(Real, Quaternion)"/>
        public static Quaternion Multiply(Real left, Quaternion right) => left * right;


        /// <inheritdoc cref="operator /(Quaternion, Real)"/>
        public static Quaternion Divide(Quaternion left, Real right) => left / right;

        /// <inheritdoc cref="operator /(Real, Quaternion)"/>
        public static Quaternion Divide(Real left, Quaternion right) => left / right;


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the left scalar multiplication of a <see cref="Quaternion"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <param name="operand"> <see cref="Quaternion"/> number to multiply. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the scalar multiplication. </returns>
        public static Quaternion Multiply(Quaternion operand, double factor) => operand * factor;

        /// <summary>
        /// Computes the right scalar multiplication of a <see cref="Quaternion"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <param name="operand"> <see cref="Quaternion"/> number to multiply. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the scalar multiplication. </returns>
        public static Quaternion Multiply(double factor, Quaternion operand) => factor * operand;


        /// <summary>
        /// Computes the scalar division of a <see cref="Quaternion"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="operand"> <see cref="Quaternion"/> number to divide. </param>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the scalar division. </returns>
        public static Quaternion Divide(Quaternion operand, double divisor) => operand / divisor;

        #endregion

        #region Operators

        /******************** Algebraic Field ********************/

        /// <summary>
        /// Computes the addition of two <see cref="Quaternion"/> numbers.
        /// </summary>
        /// <param name="left"> <see cref="Quaternion"/> number for the addition. </param>
        /// <param name="right"> <see cref="Quaternion"/> number for the addition. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the addition. </returns>
        public static Quaternion operator +(Quaternion left, Quaternion right)
        {
            return new Quaternion(left.ScalarPart + right.ScalarPart, left.I + right.I, left.J + right.J, left.K + right.K);
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="Quaternion"/> numbers.
        /// </summary>
        /// <param name="left"> <see cref="Quaternion"/> number to subtract. </param>
        /// <param name="right"> <see cref="Quaternion"/> number to subtract with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the subtraction. </returns>
        public static Quaternion operator -(Quaternion left, Quaternion right)
        {
            return new Quaternion(left.ScalarPart - right.ScalarPart, left.I - right.I, left.J - right.J, left.K - right.K);
        }


        /// <summary>
        /// Computes the opposite of the given <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Quaternion"/> number from which the opposite is computed. </param>
        /// <returns> The new <see cref="Quaternion"/> number, opposite of the initial one. </returns>
        public static Quaternion operator -(Quaternion operand)
        {
            return new Quaternion(-operand.ScalarPart, -operand.I, -operand.J, -operand.K);
        }


        /// <summary>
        /// Computes the multiplication of two <see cref="Quaternion"/> numbers.
        /// </summary>
        /// <param name="left"> <see cref="Quaternion"/> number for the multiplication. </param>
        /// <param name="right"> <see cref="Quaternion"/> number for the multiplication. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the multiplication. </returns>
        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            return new Quaternion((left.ScalarPart * right.ScalarPart) - (left.I * right.I) - (left.J * right.J) - (left.K * right.K),
                (left.ScalarPart * right.I) + (left.I * right.ScalarPart) + (left.J * right.K) - (left.K * right.J),
                (left.ScalarPart * right.J) - (left.I * right.K) + (left.J * right.ScalarPart) + (left.K * right.I),
                (left.ScalarPart * right.K) + (left.I * right.J) - (left.J * right.I) + (left.K * right.ScalarPart));
        }

        /// <summary>
        /// Computes the division of two <see cref="Quaternion"/> numbers.
        /// </summary>
        /// <param name="left"> <see cref="Quaternion"/> number to divide. </param>
        /// <param name="right"> <see cref="Quaternion"/> number to divide with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the division. </returns>
        public static Quaternion operator /(Quaternion left, Quaternion right)
        {
            double norm = (right.ScalarPart * right.ScalarPart) + (right.I * right.I) + (right.J * right.J) + (right.K * right.K);

            return new Quaternion(((left.ScalarPart * right.ScalarPart) + (left.I * right.I) + (left.J * right.J) + (left.K * right.K)) / norm,
                (-(left.ScalarPart * right.I) + (left.I * right.ScalarPart) - (left.J * right.K) + (left.K * right.J)) / norm,
                (-(left.ScalarPart * right.J) + (left.I * right.K) + (left.J * right.ScalarPart) - (left.K * right.I)) / norm,
                (-(left.ScalarPart * right.K) - (left.I * right.J) + (left.J * right.I) + (left.K * right.ScalarPart)) / norm);
        }


        /******************** Embedding : Complex ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="Quaternion"/> number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Quaternion"/> number for the addition. </param>
        /// <param name="right"> <see cref="Complex"/> number for the addition. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the addition. </returns>
        public static Quaternion operator +(Quaternion left, Complex right)
        {
            return new Quaternion(left.ScalarPart + right.RealPart, left.I + right.ImaginaryPart, left.J, left.K);
        }

        /// <summary>
        /// Computes the left addition of a <see cref="Complex"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Complex"/> number for the addition. </param>
        /// <param name="right"> <see cref="Quaternion"/> number for the addition. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the addition. </returns>
        public static Quaternion operator +(Complex left, Quaternion right)
        {
            return new Quaternion(left.RealPart + right.ScalarPart, left.ImaginaryPart + right.I, right.J, right.K);
        }


        /// <summary>
        /// Computes the subtraction of a <see cref="Quaternion"/> number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Quaternion"/> number to subtract. </param>
        /// <param name="right"> <see cref="Complex"/> number to subtract with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the subtraction. </returns>
        public static Quaternion operator -(Quaternion left, Complex right)
        {
            return new Quaternion(left.ScalarPart - right.RealPart, left.I - right.ImaginaryPart, left.J, left.K);
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="Complex"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Complex"/> number to subtract. </param>
        /// <param name="right"> <see cref="Quaternion"/> number to subtract with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the subtraction. </returns>
        public static Quaternion operator -(Complex left, Quaternion right)
        {
            return new Quaternion(left.RealPart - right.ScalarPart, left.ImaginaryPart - right.I, -right.J, -right.K);
        }


        /// <summary>
        /// Computes the right multiplication of a <see cref="Quaternion"/> number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Quaternion"/> number for the multiplicaion. </param>
        /// <param name="right"> <see cref="Complex"/> number for the multiplicaion. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the multiplication. </returns>
        public static Quaternion operator *(Quaternion left, Complex right)
        {
            return new Quaternion((left.ScalarPart * right.RealPart) - (left.I * right.ImaginaryPart),
                (left.ScalarPart * right.ImaginaryPart) + (left.I * right.RealPart),
                (left.J * right.RealPart) + (left.K * right.ImaginaryPart),
                -(left.J * right.ImaginaryPart) + (left.K * right.RealPart));
        }

        /// <summary>
        /// Computes the left multiplication of a <see cref="Complex"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Complex"/> number for the multiplicaion. </param>
        /// <param name="right"> <see cref="Quaternion"/> number for the multiplicaion. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the multiplication. </returns>
        public static Quaternion operator *(Complex left, Quaternion right)
        {
            return new Quaternion((left.RealPart * right.ScalarPart) - (left.ImaginaryPart * right.I),
                (left.ImaginaryPart * right.ScalarPart) + (left.RealPart * right.I),
                (left.RealPart * right.J) + (left.ImaginaryPart * right.K),
                -(left.ImaginaryPart * right.J) + (left.RealPart * right.K));
        }


        /// <summary>
        /// Computes the division of a <see cref="Quaternion"/> number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Quaternion"/> number to divide. </param>
        /// <param name="right"> <see cref="Complex"/> number to divide with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the division. </returns>
        public static Quaternion operator /(Quaternion left, Complex right)
        {
            double norm = (right.RealPart * right.RealPart) + (right.ImaginaryPart * right.ImaginaryPart);

            return new Quaternion(((left.ScalarPart * right.RealPart) + (left.I * right.ImaginaryPart)) / norm,
                (-(left.ScalarPart * right.ImaginaryPart) + (left.I * right.RealPart)) / norm,
                ((left.J * right.RealPart) - (left.K * right.ImaginaryPart)) / norm,
                ((left.J * right.ImaginaryPart) + (left.K * right.RealPart)) / norm);
        }

        /// <summary>
        /// Computes the division of a <see cref="Complex"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Complex"/> number to divide. </param>
        /// <param name="right"> <see cref="Quaternion"/> number to divide with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the division. </returns>
        public static Quaternion operator /(Complex left, Quaternion right)
        {
            double norm = (right.ScalarPart * right.ScalarPart) + (right.I * right.I) + (right.J * right.J) + (right.K * right.K);

            return new Quaternion(
                ((left.RealPart * right.ScalarPart) + (left.ImaginaryPart * right.I)) / norm,
                (-(left.RealPart * right.I) + (left.ImaginaryPart * right.ScalarPart)) / norm,
                (-(left.RealPart * right.J) + (left.ImaginaryPart * right.K)) / norm,
                (-(left.RealPart * right.K) - (left.ImaginaryPart * right.J)) / norm);
        }


        /******************** Embedding : Real ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="Quaternion"/> number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Quaternion"/> number for the addition. </param>
        /// <param name="right"> <see cref="Real"/> number for the addition. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the addition. </returns>
        public static Quaternion operator +(Quaternion left, Real right)
        {
            return new Quaternion(left.ScalarPart + right.Value, left.I, left.J, left.K);
        }

        /// <summary>
        /// Computes the left addition of a <see cref="Real"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Real"/> number for the addition. </param>
        /// <param name="right"> <see cref="Quaternion"/> number for the addition. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the addition. </returns>
        public static Quaternion operator +(Real left, Quaternion right)
        {
            return new Quaternion(left.Value + right.ScalarPart, right.I, right.J, right.K);
        }


        /// <summary>
        /// Computes the subtraction of a <see cref="Quaternion"/> number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Quaternion"/> number to subtract. </param>
        /// <param name="right"> <see cref="Real"/> number to subtract with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the subtraction. </returns>
        public static Quaternion operator -(Quaternion left, Real right)
        {
            return new Quaternion(left.ScalarPart - right.Value, left.I, left.J, left.K);
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="Real"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Real"/> number to subtract. </param>
        /// <param name="right"> <see cref="Quaternion"/> number to subtract with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the subtraction. </returns>
        public static Quaternion operator -(Real left, Quaternion right)
        {
            return new Quaternion(left.Value - right.ScalarPart, -right.I, -right.J, -right.K);
        }


        /// <summary>
        /// Computes the right multiplication of a <see cref="Quaternion"/> number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Quaternion"/> number for the multiplicaion. </param>
        /// <param name="right"> <see cref="Real"/> number for the multiplicaion. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the multiplication. </returns>
        public static Quaternion operator *(Quaternion left, Real right)
        {
            return new Quaternion(left.ScalarPart * right.Value, left.I * right.Value, left.J * right.Value, left.K * right.Value);
        }

        /// <summary>
        /// Computes the left multiplication of a <see cref="Real"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Real"/> number for the multiplicaion. </param>
        /// <param name="right"> <see cref="Quaternion"/> number for the multiplicaion. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the multiplication. </returns>
        public static Quaternion operator *(Real left, Quaternion right)
        {
            return new Quaternion(left.Value * right.ScalarPart, left.Value * right.I, left.Value * right.J, left.Value * right.K);
        }


        /// <summary>
        /// Computes the division of a <see cref="Quaternion"/> number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Quaternion"/> number to divide. </param>
        /// <param name="right"> <see cref="Real"/> number to divide with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the division. </returns>
        public static Quaternion operator /(Quaternion left, Real right)
        {
            return new Quaternion(left.ScalarPart / right.Value, left.I / right.Value, left.J / right.Value, left.K / right.Value);
        }

        /// <summary>
        /// Computes the division of a <see cref="Real"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="left"> <see cref="Real"/> number to divide. </param>
        /// <param name="right"> <see cref="Quaternion"/> number to divide with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the division. </returns>
        public static Quaternion operator /(Real left, Quaternion right)
        {
            double norm = (right.ScalarPart * right.ScalarPart) + (right.I * right.I) + (right.J * right.J) + (right.K * right.K);

            return new Quaternion(left.Value * right.ScalarPart / norm, left.Value * (-right.I) / norm, left.Value * (-right.J) / norm, left.Value * (-right.K) / norm);
        }


        /******************** Embedding : Real ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="Quaternion"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="left"> <see cref="Quaternion"/> number for the addition. </param>
        /// <param name="right"> <see cref="double"/>-precision real number for the addition. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the addition. </returns>
        public static Quaternion operator +(Quaternion left, double right)
        {
            return new Quaternion(left.ScalarPart + right, left.I, left.J, left.K);
        }

        /// <summary>
        /// Computes the left addition of a <see cref="double"/>-precision real number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="left"> <see cref="double"/>-precision real number for the addition. </param>
        /// <param name="right"> <see cref="Quaternion"/> number for the addition. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the addition. </returns>
        public static Quaternion operator +(double left, Quaternion right)
        {
            return new Quaternion(left + right.ScalarPart, right.I, right.J, right.K);
        }
        

        /// <summary>
        /// Computes the subtraction of a <see cref="Quaternion"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="left"> <see cref="Quaternion"/> number to subtract. </param>
        /// <param name="right"> <see cref="double"/>-precision real number to subtract with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the subtraction. </returns>
        public static Quaternion operator -(Quaternion left, double right)
        {
            return new Quaternion(left.ScalarPart - right, left.I, left.J, left.K);
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="Real"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="left"> <see cref="double"/>-precision real number to subtract. </param>
        /// <param name="right"> <see cref="Quaternion"/> number to subtract with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the subtraction. </returns>
        public static Quaternion operator -(double left, Quaternion right)
        {
            return new Quaternion(left - right.ScalarPart, -right.I, -right.J, -right.K);
        }


        /// <summary>
        /// Computes the right multiplication of a <see cref="Quaternion"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="left"> <see cref="Quaternion"/> number for the multiplicaion. </param>
        /// <param name="right"> <see cref="double"/>-precision real number for the multiplicaion. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the multiplication. </returns>
        public static Quaternion operator *(Quaternion left, double right)
        {
            return new Quaternion(left.ScalarPart * right, left.I * right, left.J * right, left.K * right);
        }

        /// <summary>
        /// Computes the left multiplication of a <see cref="double"/>-precision real number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="left"> <see cref="double"/>-precision real number for the multiplicaion. </param>
        /// <param name="right"> <see cref="Quaternion"/> number for the multiplicaion. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the multiplication. </returns>
        public static Quaternion operator *(double left, Quaternion right)
        {
            return new Quaternion(left * right.ScalarPart, left * right.I, left * right.J, left * right.K);
        }


        /// <summary>
        /// Computes the division of a <see cref="Quaternion"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="left"> <see cref="Quaternion"/> number to divide. </param>
        /// <param name="right"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the division. </returns>
        public static Quaternion operator /(Quaternion left, double right)
        {
            return new Quaternion(left.ScalarPart / right, left.I / right, left.J / right, left.K / right);
        }

        /// <summary>
        /// Computes the division of a <see cref="double"/>-precision real number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="left"> <see cref="double"/>-precision real number to divide. </param>
        /// <param name="right"> <see cref="Quaternion"/> number to divide with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the division. </returns>
        public static Quaternion operator /(double left, Quaternion right)
        {
            double norm = (right.ScalarPart * right.ScalarPart) + (right.I * right.I) + (right.J * right.J) + (right.K * right.K);

            return new Quaternion(left * right.ScalarPart / norm, left * (-right.I) / norm, left * (-right.J) / norm, left * (-right.K) / norm);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts a <see cref="Complex"/> number into a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="complex"> <see cref="Complex"/> number to convert. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the conversion. </returns>
        public static explicit operator Quaternion(Complex complex) => new Quaternion(complex.RealPart, complex.ImaginaryPart, 0d, 0d);

        /// <summary>
        /// Converts a <see cref="Real"/> number into a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="real"> <see cref="Real"/> number to convert. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the conversion. </returns>
        public static explicit operator Quaternion(Real real) => new Quaternion(real.Value, 0d, 0d, 0d);

        /// <summary>
        /// Converts a <see cref="double"/>-precision real number into a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="number"> <see cref="double"/>-precision real number to convert. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the conversion. </returns>
        public static explicit operator Quaternion(double number) => new Quaternion(number, 0d, 0d, 0d);

        /// <summary>
        /// Converts a <see cref="ValueTuple{T1, T2, T3, T4}"/> into a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="quadruple"> <see cref="ValueTuple{T1, T2, T3, T4}"/> to convert. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the conversion. </returns>
        public static explicit operator Quaternion(ValueTuple<double, double, double, double> quadruple) => new Quaternion(quadruple.Item1, quadruple.Item2, quadruple.Item3, quadruple.Item4);

        #endregion

        #region Public Methods

        /// <summary>
        /// Computes the euclidean dot product of this <see cref="Quaternion"/> number with another <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="other"> Right <see cref="Quaternion"/> number for the dot product. </param>
        /// <returns> The value of the dot product of the two elements. </returns>
        public double DotProduct(Quaternion other)
        {
            return (ScalarPart * other.ScalarPart) + (I * other.I) + (J * other.J) + (K * other.K);
        }

        /// <summary>
        /// Computes the euclidean norm of this <see cref="Quaternion"/> number.
        /// </summary>
        /// <returns> The value of the norm. </returns>
        public double Norm() 
        { 
            return Math.Sqrt((ScalarPart * ScalarPart) + (I * I) + (J * J) + (K * K)); 
        }

        /// <inheritdoc cref="Alg_Meas.INorm{TSelf}.Norm()"/>
        public double DistanceTo(Quaternion other) => (this - other).Norm();


        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(Quaternion other)
        {
            return (Math.Abs(ScalarPart - other.ScalarPart) < Settings.AbsolutePrecision 
                && Math.Abs(I - other.I) < Settings.AbsolutePrecision
                && Math.Abs(J - other.J) < Settings.AbsolutePrecision 
                && Math.Abs(K - other.K) < Settings.AbsolutePrecision);
        }

        #endregion


        #region Overrides

        /******************** object ********************/

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Quaternion quaternion && Equals(quaternion);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"({ScalarPart}, {I}, {J}, {K})";
        }

        #endregion

        #region Explicit Implementations

        /******************** IGroupAction<Quaternion, Complex> ********************/

        /// <inheritdoc/>
        Quaternion Alg_Sets.IGroupAction<Quaternion, Complex>.Multiply(Complex factor) => this * factor;

        /// <inheritdoc/>
        Quaternion Alg_Sets.IGroupAction<Quaternion, Complex>.Divide(Complex divisor) => this / divisor;


        /******************** IGroupAction<Quaternion, Real> ********************/

        /// <inheritdoc/>
        Quaternion Alg_Sets.IGroupAction<Quaternion, Real>.Multiply(Real factor) => this * factor;

        /// <inheritdoc/>
        Quaternion Alg_Sets.IGroupAction<Quaternion, Real>.Divide(Real divisor) => this / divisor;


        /******************** IGroupAction<Quaternion, double> ********************/

        /// <inheritdoc/>
        Quaternion Alg_Sets.IGroupAction<Quaternion, double>.Multiply(double factor) => this * factor;

        /// <inheritdoc/>
        Quaternion Alg_Sets.IGroupAction<Quaternion, double>.Divide(double divisor) => this / divisor;

        #endregion
    }
}
