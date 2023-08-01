using System;
using System.Numerics;


namespace BRIDGES.Arithmetic.Polynomials.Multivariate
{
    /// <summary>
    /// Class defining a multivariate monomial.
    /// </summary>
    public class Monomial :
        IMultiplyOperators<Monomial, Monomial, Monomial>
    {
        #region Fields

        /// <summary>
        /// Exponent for each variables of this <see cref="Monomial"/>.
        /// </summary>
        private readonly int[] _exponents;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the exponent of the variable at a given index.
        /// </summary>
        /// <param name="index"> Index of the variable whose exponent to get. </param>
        /// <returns> The exponent of the variable at the given index. </returns>
        public int this[int index] => _exponents[index];

        /// <summary>
        /// Gets the total degree of this <see cref="Monomial"/>.
        /// </summary>
        public int TotalDegree
        {
            get
            {
                int totaldegree = 0;
                for (int i = 0; i < _exponents.Length; i++)
                {
                    totaldegree += _exponents[i];
                }
                return totaldegree;
            }
        }

        /// <summary>
        /// Gets the number of variable of this <see cref="Monomial"/>.
        /// </summary>
        public int VariableCount => _exponents.Length;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="Monomial"/> class by defining each variable's exponent .
        /// </summary>
        /// <param name="exponents"> Variable's exponents. </param>
        public Monomial(params int[] exponents)
        {
            _exponents = (int[])exponents.Clone();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Monomial"/> class from another <see cref="Monomial"/>.
        /// </summary>
        /// <param name="other"> <see cref="Monomial"/> to deep copy. </param>
        public Monomial(Monomial other)
        {
            _exponents = (int[])other._exponents.Clone();
        }


        /// <summary>
        /// Initialises a new instance of <see cref="Monomial"/> class by defining each variable's exponent .
        /// </summary>
        /// <param name="exponents"> Variable's exponents. </param>
        internal Monomial(ref int[] exponents)
        {
            _exponents = exponents;
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets a new <see cref="Monomial"/>, constant equal to one.
        /// </summary>
        public static Monomial One => new Monomial(0);

        #endregion

        #region Static Methods

        /******************** Algebraic Multiplicative Monoid ********************/

        /// <inheritdoc cref="operator *(Monomial, Monomial)"/>
        public static Monomial Multiply(Monomial left, Monomial right) => left * right;

        #endregion

        #region Operators

        /******************** Algebraic Multiplicative Monoid ********************/

        /// <summary>
        /// Computes the multiplication of two <see cref="Monomial"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Monomial"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Monomial"/> for the multiplication. </param>
        /// <returns> The new <see cref="Monomial"/> resulting from the multiplication. </returns>
        public static Monomial operator *(Monomial left, Monomial right)
        {
            Monomial less, more;
            if (left.VariableCount < right.VariableCount) { less = left; more = right; }
            else { less = right; more = left; }

            int lessCount = less.VariableCount;
            int moreCount = more.VariableCount;

            int[] exponents = new int[moreCount];

            int i_E = 0;
            for (; i_E < lessCount; i_E++) { exponents[i_E] = less._exponents[i_E] + more._exponents[i_E]; }
            for (; i_E < moreCount; i_E++) { exponents[i_E] = more._exponents[i_E]; }

            return new Monomial(exponents);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Evaluates this <see cref="Monomial"/> at a given point.
        /// </summary>
        /// <param name="point"> Point to evaluate at. </param>
        /// <returns> The value of this <see cref="Monomial"/> at the given point. </returns>
        public virtual double EvaluateAt(double[] point)
        {
            if (point is null) { throw new ArgumentNullException(nameof(point)); }

            double result = 1d;
            for (int i = 0; i < VariableCount; i++)
            {
                result *= Math.Pow(point[i], _exponents[i]);
            }

            return result;
        }

        #endregion
    }
}



