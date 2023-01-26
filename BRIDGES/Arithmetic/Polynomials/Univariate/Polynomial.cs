using System;


using Alg_Sets = BRIDGES.Algebra.Sets;


namespace BRIDGES.Arithmetic.Polynomials.Univariate
{
    /// <summary>
    /// Class defining a univarite polynomial.
    /// </summary>
    /// <remarks> For a multivariate polynomial, refer to <see cref="Multivariate.Polynomial"/>. </remarks>
    public class Polynomial :
        Alg_Sets.IGroupAction<Polynomial, double>
    {
        #region Fields

        /// <summary>
        /// Coefficients of this <see cref="Polynomial"/>, starting from the constant value.
        /// </summary>
        internal double[] _coefficients;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the degree of this <see cref="Polynomial"/>.
        /// </summary>
        public int Degree => _coefficients.Length - 1;

        /// <summary>
        /// Gets the value of the coefficient at a given index.
        /// </summary>
        /// <param name="index"> Index of the coefficient to get. </param>
        /// <returns> The value of the coefficient at the given index. </returns>
        public double this[int index] =>_coefficients[index];

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="Polynomial"/> class by defining its coefficients.
        /// </summary>
        /// <param name="coefficients"> Coefficients of the polynomial, starting from the constant value. </param>
        public Polynomial(params double[] coefficients)
        {
            _coefficients = (double[])coefficients.Clone();

            if (coefficients[coefficients.Length - 1] == 0d) { Clean(); }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Polynomial"/> class from another <see cref="Polynomial"/>.
        /// </summary>
        /// <param name="other"> <see cref="Polynomial"/> to deep copy. </param>
        public Polynomial(Polynomial other)
        {
            _coefficients = (double[])other._coefficients.Clone();
        }


        /// <summary>
        /// Initialises a new instance of <see cref="Polynomial"/> class by defining its coefficients.
        /// </summary>
        /// <param name="coefficients"> Coefficients of the polynomial, starting from the constant value. </param>
        internal Polynomial(ref double[] coefficients)
        {
            _coefficients = coefficients;
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets a new <see cref="Polynomial"/>, constant equal to zero.
        /// </summary>
        public static Polynomial Zero =>new Polynomial(0d);

        /// <summary>
        /// Gets a new <see cref="Polynomial"/>, constant equal to one.
        /// </summary>
        public static Polynomial One => new Polynomial(1d);

        #endregion

        #region Static Methods

        /// <summary>
        /// Computes the <see cref="Polynomial"/> which is the derivative of the given <see cref="Polynomial"/> at the given order.
        /// </summary>
        /// <param name="polynomial"> <see cref="Polynomial"/> to derive. </param>
        /// <param name="order"> Order of the derivative to compute. </param>
        /// <returns> The new <see cref="Polynomial"/> resulting from the derivation. </returns>
        public static Polynomial Derive(Polynomial polynomial, int order = 1)
        {
            if (order > polynomial.Degree) { return new Polynomial(0d); }

            int newDegree = polynomial.Degree - order;
            double[] coefficients = new double[newDegree + 1];

            for (int i_C = 0; i_C < newDegree + 1; i_C++)
            {
                coefficients[i_C] = polynomial[i_C + order];
                for (int i = 0; i < order; i++)
                {
                    coefficients[i_C] *= i_C + 1 + order;
                }
            }

            return new Polynomial(coefficients);
        }

        /******************** Algebraic Near Ring ********************/

        /// <inheritdoc cref="operator +(Polynomial, Polynomial)"/>
        public static Polynomial Add(Polynomial left, Polynomial right) => left + right;

        /// <inheritdoc cref="operator -(Polynomial, Polynomial)"/>
        public static Polynomial Subtract(Polynomial left, Polynomial right) => left - right;


        /// <inheritdoc cref="operator -(Polynomial)"/>
        public static Polynomial Opposite(Polynomial operand) => -operand;


        /// <inheritdoc cref="operator *(Polynomial, Polynomial)"/>
        public static Polynomial Multiply(Polynomial left, Polynomial right) => left * right;


        /******************** Group Action ********************/

        /// <inheritdoc cref="operator *(Polynomial, double)"/>
        public static Polynomial Multiply(Polynomial polynomial, double factor) => polynomial * factor;

        /// <inheritdoc cref="operator *(double, Polynomial)"/>
        public static Polynomial Multiply(double factor, Polynomial polynomial) => factor * polynomial;

        /// <inheritdoc cref="operator /(Polynomial, double)"/>
        public static Polynomial Divide(Polynomial polynomial, double divisor) => polynomial / divisor;


        #endregion

        #region Operators

        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Computes the addition of two <see cref="Polynomial"/>.
        /// </summary>
        /// <param name="left"> <see cref="Polynomial"/> for the addition. </param>
        /// <param name="right"> <see cref="Polynomial"/> for the addition. </param>
        /// <returns> The new <see cref="Polynomial"/> resulting from the addition. </returns>
        public static Polynomial operator +(Polynomial left, Polynomial right)
        {
            Polynomial less, more;
            if (left.Degree < right.Degree) { less = left; more = right; }
            else { less = right; more = left; }

            int lessCoefCount = less._coefficients.Length;
            int moreCoefCount = more._coefficients.Length;

            double[] coefficients = new double[moreCoefCount];

            for (int i_C = 0; i_C < lessCoefCount; i_C++)
            {
                coefficients[i_C] = less[i_C] + more[i_C];
            }
            for (int i_C = lessCoefCount; i_C < moreCoefCount; i_C++)
            {
                coefficients[i_C] = more[i_C];
            }

            if (left.Degree == right.Degree && left[left.Degree] == -right[right.Degree])
            {
                return new Polynomial(coefficients);
            }
            else { return new Polynomial(ref coefficients); }
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="Polynomial"/>.
        /// </summary>
        /// <param name="left"> <see cref="Polynomial"/> to subtract. </param>
        /// <param name="right"> <see cref="Polynomial"/> to subtract with. </param>
        /// <returns> The new <see cref="Polynomial"/> resulting from the subtraction. </returns>
        public static Polynomial operator -(Polynomial left, Polynomial right)
        {
            double[] coefficients;

            if (left.Degree < right.Degree)
            {
                int lessCoefCount = left._coefficients.Length;
                int moreCoefCount = right._coefficients.Length;

                coefficients = new double[moreCoefCount];

                for (int i_C = 0; i_C < lessCoefCount; i_C++)
                {
                    coefficients[i_C] = left[i_C] - right[i_C];
                }

                for (int i_C = lessCoefCount; i_C < moreCoefCount; i_C++)
                {
                    coefficients[i_C] = -right[i_C];
                }
            }
            else
            {
                int lessCoefCount = right._coefficients.Length;
                int moreCoefCount = left._coefficients.Length;

                coefficients = new double[moreCoefCount];

                for (int i_C = 0; i_C < lessCoefCount; i_C++)
                {
                    coefficients[i_C] = left[i_C] - right[i_C];
                }

                for (int i_C = lessCoefCount; i_C < moreCoefCount; i_C++)
                {
                    coefficients[i_C] = left[i_C];
                }
            }

            if (left.Degree == right.Degree && left[left.Degree] == right[right.Degree])
            {
                return new Polynomial(coefficients);
            }
            else { return new Polynomial(ref coefficients); }
        }


        /// <summary>
        /// Computes the opposite of the given <see cref="Polynomial"/>.
        /// </summary>
        /// <param name="polynomial"> <see cref="Polynomial"/> to be opposed. </param>
        /// <returns> The new <see cref="Polynomial"/>, opposite of the initial one. </returns>
        public static Polynomial operator -(Polynomial polynomial)
        {
            int coefCount = polynomial.Degree + 1;

            double[] coefficients = new double[coefCount];
            for (int i_C = 0; i_C < coefCount; i_C++)
            {
                coefficients[i_C] = -polynomial[i_C];
            }

            return new Polynomial(ref coefficients);
        }


        /******************** Algebraic Multiplicative Monoid ********************/

        /// <summary>
        /// Computes the multiplication of two <see cref="Polynomial"/>.
        /// </summary>
        /// <param name="left"> <see cref="Polynomial"/> for the multiplication. </param>
        /// <param name="right"> <see cref="Polynomial"/> for the multiplication. </param>
        /// <returns> The new <see cref="Polynomial"/> resulting from the multiplication. </returns>
        public static Polynomial operator *(Polynomial left, Polynomial right)
        {
            int leftDegree = left.Degree;
            int rightDegree = right.Degree;

            double[] coefficients = new double[leftDegree + rightDegree + 1];
            for (int i = 0; i < leftDegree + 1; i++)
            {
                for (int j = 0; j < rightDegree + 1; j++)
                {
                    coefficients[i + j] += left[i] * right[j];
                }
            }

            return new Polynomial(ref coefficients);
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the right scalar multiplication of a <see cref="Polynomial"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/> number. </param>
        /// <param name="polynomial"> <see cref="Polynomial"/> to multiply. </param>
        /// <returns> The new <see cref="Polynomial"/> resulting from the scalar multiplication. </returns>
        public static Polynomial operator *(Polynomial polynomial, double factor)
        {
            // Special case
            if (factor == 0d) { return new Polynomial(0d); }


            int coefCount = polynomial.Degree + 1;

            double[] coefficients = new double[coefCount];
            for (int i_C = 0; i_C < coefCount; i_C++)
            {
                coefficients[i_C] = polynomial[i_C] * factor;
            }

            return new Polynomial(ref coefficients);
        }

        /// <summary>
        /// Computes the left scalar multiplication of a <see cref="Polynomial"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/> number. </param>
        /// <param name="polynomial"> <see cref="Polynomial"/> to multiply. </param>
        /// <returns> The new <see cref="Polynomial"/> resulting from the scalar multiplication. </returns>
        public static Polynomial operator *(double factor, Polynomial polynomial)
        {
            // Special case
            if (factor == 0d) { return new Polynomial(0d); }


            int coefCount = polynomial.Degree + 1;

            double[] coefficients = new double[coefCount];
            for (int i_C = 0; i_C < coefCount; i_C++)
            {
                coefficients[i_C] = factor * polynomial[i_C];
            }

            return new Polynomial(ref coefficients);
        }


        /// <summary>
        /// Computes the scalar division of a <see cref="Polynomial"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="polynomial"> <see cref="Polynomial"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/> number to divide with. </param>
        /// <returns> The new <see cref="Polynomial"/> resulting from the scalar division. </returns>
        public static Polynomial operator /(Polynomial polynomial, double divisor)
        {
            int coefCount = polynomial.Degree + 1;

            double[] coefficients = new double[coefCount];
            for (int i_C = 0; i_C < coefCount; i_C++)
            {
                coefficients[i_C] = polynomial[i_C] / divisor;
            }

            return new Polynomial(ref coefficients);
        }


        #endregion

        #region Public Methods 

        /// <summary>
        /// Returns the constant coefficient of the term at the given index.
        /// </summary>
        /// <param name="index"> Index of the term whose coefficient to return. </param>
        /// <returns> The coefficient of the term at the given index.  </returns>
        public double Coefficient(int index) => this[index];


        /// <summary>
        /// Computes the current <see cref="Polynomial"/> at a given value using Horner's method.
        /// </summary>
        /// <param name="val"> Value to evaluate at. </param>
        /// <returns> The computed value of the current <see cref="Polynomial"/>. </returns>
        public virtual double EvaluateAt(double val)
        {
            /* Horners method could be used */

            double result = _coefficients[Degree];
            for (int i = Degree - 1; i > -1; i--)
            {
                result = result * val + _coefficients[i];
            }

            return result;
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Cleans the leading zero coefficients.
        /// </summary>
        private void Clean()
        {
            int actualDegree = 0;
            for (int i_D = _coefficients.Length - 1; i_D > -1; i_D--)
            {
                if (_coefficients[i_D] != 0.0) { actualDegree = i_D; break; }
            }

            int coefCount = actualDegree + 1;

            double[] coefficients = new double[coefCount];
            for (int i_C = 0; i_C < coefCount; i_C++)
            {
                coefficients[i_C] = _coefficients[i_C];
            }

            _coefficients = coefficients;

        }

        #endregion


        #region Explicit Implementations

        /******************** IGroupAction<DenseMatrix, double> ********************/

        /// <inheritdoc/>
        Polynomial Alg_Sets.IGroupAction<Polynomial, double>.Multiply(double factor) => this * factor;

        /// <inheritdoc/>
        Polynomial Alg_Sets.IGroupAction<Polynomial, double>.Divide(double divisor) => this / divisor;

        #endregion
    }
}
