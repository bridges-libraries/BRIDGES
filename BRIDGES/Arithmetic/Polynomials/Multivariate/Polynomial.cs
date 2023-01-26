using System;

using Alg_Sets = BRIDGES.Algebra.Sets;


namespace BRIDGES.Arithmetic.Polynomials.Multivariate
{
    /// <summary>
    /// Class defining a multivariate polynomial.
    /// </summary>
    /// <remarks> For a univarite polynomial, refer to <see cref="Polynomial"/>. </remarks>
    public class Polynomial 
        // Alg_Sets.IGroupAction<Polynomial, double>
    {
        #region Fields

        /// <summary>
        /// Constant coefficient for each term of this <see cref="Polynomial"/>.
        /// </summary>
        private readonly double[] _coefficients;

        /// <summary>
        /// <see cref="Monomial"/> for each term of this <see cref="Polynomial"/>.
        /// </summary>
        /// <remarks> By default, the monomials are not ordered in a specific way. </remarks>
        private readonly Monomial[] _monomials;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of terms in this <see cref="Polynomial"/>.
        /// </summary>
        public int TermCount => _coefficients.Length;


        /// <summary>
        /// Gets the maximum <see cref="Monomial"/> total degree.
        /// </summary>
        public int TotalDegree
        {
            get
            {
                int result = 0;
                for (int i = 0; i < _monomials.Length; i++)
                {
                    if (result < _monomials[i].TotalDegree) { result = _monomials[i].TotalDegree; }
                }
                return result;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="Polynomial"/> class by defining its coefficients and monomials.
        /// </summary>
        /// <param name="coefficients"> Coefficients of the multivariate polynomial associated with each monomials. </param>
        /// <param name="monomials"> Monomials of the multivariate polynomial. </param>
        /// <exception cref="RankException"> The same number of coefficients and monomials must be provided. </exception>
        public Polynomial(double[] coefficients, Monomial[] monomials)
        {
            if (coefficients.Length != monomials.Length) { throw new RankException("The same number of coefficients and monomials must be provided."); }

            int length = monomials.Length;
            for (int i = 1; i < length; i++)
            {
                if (monomials[i].VariableCount != monomials[i - 1].VariableCount)
                {
                    throw new ArgumentException("The monomials must have the same number of variables.", nameof(monomials));
                }
            }

            _coefficients = new double[length];
            _monomials = new Monomial[length];
            for (int i = 0; i < length; i++)
            {
                _coefficients[i] = coefficients[i];
                _monomials[i] = new Monomial(monomials[i]);
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Polynomial"/> class from another <see cref="Polynomial"/>.
        /// </summary>
        /// <param name="other"> <see cref="Polynomial"/> to deep copy. </param>
        public Polynomial(Polynomial other)
        {
            _coefficients = (double[])other._coefficients.Clone();

            _monomials = (Monomial[])other._monomials.Clone();
        }


        /// <summary>
        /// Initialises a new instance of <see cref="Polynomial"/> class by defining its coefficients and monomials.
        /// </summary>
        /// <param name="coefficients"> Coefficients of the multivariate polynomial associated with each monomials. </param>
        /// <param name="monomials"> Monomials of the multivariate polynomial. </param>
        /// <exception cref="RankException"> The same number of coefficients and monomials must be provided. </exception>
        internal Polynomial(ref double[] coefficients, ref Monomial[] monomials)
        {
            _coefficients = coefficients;
            _monomials = monomials;
        }


        #endregion

        #region Static Properties

        /// <summary>
        /// Gets a new <see cref="Polynomial"/>, constant equal to one.
        /// </summary>
        public static Polynomial One => new Polynomial(new double[] { 1d }, new Monomial[] { Multivariate.Monomial.One });

        #endregion

        #region Static Methods

        /******************** Algebraic Near Ring ********************/
/*
        /// <inheritdoc cref="operator +(Polynomial, Polynomial)"/>
        public static Polynomial Add(Polynomial left, Polynomial right) => left + right;

        /// <inheritdoc cref="operator -(Polynomial, Polynomial)"/>
        public static Polynomial Subtract(Polynomial left, Polynomial right) => left - right;


        /// <inheritdoc cref="operator -(Polynomial)"/>
        public static Polynomial Opposite(Polynomial operand) => -operand;


        /// <inheritdoc cref="operator *(Polynomial, Polynomial)"/>
        public static Polynomial Multiply(Polynomial left, Polynomial right) => left * right;
*/
        #endregion

        #region Operators

        /******************** Algebraic Near Ring ********************/
/*
        /// <summary>
        /// Computes the addition of two <see cref="Polynomial"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Polynomial"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Polynomial"/> for the addition. </param>
        /// <returns> The <see cref="Polynomial"/> resulting from the addition. </returns>
        public static Polynomial operator +(Polynomial left, Polynomial right)
        {

        }

        /// <summary>
        /// Computes the subtraction of two <see cref="Polynomial"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Polynomial"/> to subtract. </param>
        /// <param name="right"> Right <see cref="Polynomial"/> to subtract with. </param>
        /// <returns> The <see cref="Polynomial"/> resulting from the subtraction. </returns>
        public static Polynomial operator -(Polynomial left, Polynomial right)
        {
            
        }


        /// <summary>
        /// Computes the opposite of the <see cref="Polynomial"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Polynomial"/> whose opposite is to be computed. </param>
        /// <returns> The <see cref="Polynomial"/>, opposite of the initial one. </returns>
        /// <exception cref="NotImplementedException"> The unary negation of the operand as a <see cref="Polynomial"/> is not implemented. </exception>
        public static Polynomial operator -(Polynomial operand)
        {
            int count = operand.TermCount;

            double[] coefficients = new double[count];
            Monomial[] monomials = new Monomial[count];
            for (int i = 0; i < count; i++)
            {
                coefficients[i] = -operand.Coefficient(i);
                monomials[i] = new Monomial(operand.Monomial(i));
            }

            return new Polynomial(ref coefficients, ref monomials);
        }


        /// <summary>
        /// Computes the multiplication of two <see cref="Polynomial"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Polynomial"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Polynomial"/> for the multiplication. </param>
        /// <returns> The <see cref="Polynomial"/> resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> The number of columns of left and the number of rows of right must be equal. </exception>
        public static Polynomial operator *(Polynomial left, Polynomial right)
        {

        }
*/

        #endregion

        #region Conversions

        /// <summary>
        /// Converts a <see cref="Monomial"/> into a <see cref="Polynomial"/>.
        /// </summary>
        /// <param name="monomial"> <see cref="Monomial"/> to convert. </param>
        /// <returns> The new <see cref="Polynomial"/> resulting from the conversion. </returns>
        public static implicit operator Polynomial(Monomial monomial)
        {
            return new Polynomial(new double[1] { 1.0 }, new Monomial[1] { monomial });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the constant coefficient of the term at the given index.
        /// </summary>
        /// <param name="index"> Index of the term whose coefficient to return. </param>
        /// <returns> The coefficient of the term at the given index.  </returns>
        public double Coefficient(int index) => _coefficients[index];

        /// <summary>
        /// Returns the monomial of the term at the given index.
        /// </summary>
        /// <param name="index"> Index of the term whose monomial to return. </param>
        /// <returns> The <see cref="Multivariate.Monomial"/> of the term at the given index. </returns>
        public Monomial Monomial(int index) => _monomials[index];


        /// <summary>
        /// Evaluates this <see cref="Polynomial"/> at a given point.
        /// </summary>
        /// <param name="point"> Point to evaluate at. </param>
        /// <returns> The value of this <see cref="Polynomial"/> at the given point. </returns>
        public double EvaluateAt(double[] point)
        {
            double result = 0d;
            for (int i = 0; i < _coefficients.Length; i++)
            {
                result += _coefficients[i] * _monomials[i].EvaluateAt(point);
            }

            return result;
        }

        #endregion


        #region Explicit Implementations

        /******************** IGroupAction<DenseMatrix, double> ********************/
/*
        /// <inheritdoc/>
        Polynomial Alg_Sets.IGroupAction<Polynomial, double>.Multiply(double factor) => this * factor;

        /// <inheritdoc/>
        Polynomial Alg_Sets.IGroupAction<Polynomial, double>.Divide(double divisor) => this / divisor;
*/
        #endregion
    }
}
