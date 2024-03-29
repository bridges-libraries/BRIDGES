﻿using System;
using System.Collections.Generic;
using BRIDGES.Arithmetic.Polynomials.Univariate;
using BRIDGES.Geometry.Euclidean3D;

namespace BRIDGES.Arithmetic.Polynomials.Univariate.Specials
{
    /// <summary>
    /// Class defining a B-Spline polynomial.
    /// </summary>
    public class BSpline : Polynomial
    {
        #region Fields

        /// <summary>
        /// Knot vector associated with the current <see cref="BSpline"/>.
        /// </summary>
        protected List<double> _knotVector;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the index of the current <see cref="BSpline"/>.
        /// </summary>
        public int Index { get; }


        /// <summary>
        /// Gets the knot vector associated with the current <see cref="BSpline"/>.
        /// </summary>
        public double[] KnotVector
        {
            get { return _knotVector.ToArray(); }
        }

        /// <summary>
        /// Gets the index of knot span on which the current <see cref="BSpline"/> is defined.
        /// </summary>
        public int SpanIndex { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="BSpline"/> class by defining its index, degree and knot vector.
        /// </summary>
        /// <param name="spanIndex"> Index of knot span on which the current <see cref="BSpline"/> is defined. </param>
        /// <param name="index"> Index of the B-Spline polynomial. </param>
        /// <param name="degree"> Degree of the B-Spline polynomial. </param>
        /// <param name="knotVector"> Knot vector of the B-Spline polynomial. </param>
        public BSpline(int spanIndex, int index, int degree, IList<double> knotVector) 
            : base(ComputeCoefficients(spanIndex, index, degree, knotVector))
        {
            // Initialise properties
            Index = index;
            SpanIndex = spanIndex;

            // Initialise fields
            SetKnotVector(knotVector, degree, out _knotVector);

        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Identifies the index of the knot span containing a given value, using a binary search.
        /// </summary>
        /// <remarks> 
        /// The code is adapted from algorithm 2.1 described in <see href="https://doi.org/10.1007/978-3-642-59223-2">the NURBS Book</see>, by L. Piegl and  W. Tiller.
        /// </remarks>
        /// <param name="val"> Value to locate in the knot vector. </param>
        /// <param name="degree"> Degree of the <see cref="BSpline"/> basis. </param>
        /// <param name="knotVector"> Knot vector of the <see cref="BSpline"/> basis. </param>
        /// <returns> The (zero-based) index of the knot span containing the value. </returns>
        public static int FindKnotSpanIndex(double val, int degree, IList<double> knotVector)
        {
            // Number of segments in the control polygon.
            int n = knotVector.Count - degree - 1;

            // Special case : The value equals the last possible knot.
            if (val == knotVector[n + 1]) { return n; }

            int i_StartKnot = degree;
            int i_EndKnot = n + 1;

            int i_MidKnot = (int)Math.Truncate((i_StartKnot + i_EndKnot) / 2.0);
            while (val < knotVector[i_MidKnot] || val >= knotVector[i_MidKnot + 1])
            {
                if (val < knotVector[i_MidKnot]) { i_EndKnot = i_MidKnot; }
                else { i_StartKnot = i_MidKnot; }

                i_MidKnot = (int)Math.Truncate((i_StartKnot + i_EndKnot) / 2.0);
            }

            return i_MidKnot;
        }

        /******************** On B-Spline Polynomial Basis ********************/

        /// <summary>
        /// Evaluates a <see cref="BSpline"/> basis at a given value. 
        /// </summary>
        /// <remarks> 
        /// The code is adapted from algorithm 2.2 described in <see href="https://doi.org/10.1007/978-3-642-59223-2">the NURBS Book</see>, by L. Piegl and  W. Tiller.
        /// </remarks>
        /// <param name="val"> Value to evaluate at. </param>
        /// <param name="knotSpanIndex"> Index of knot span containing the value. </param>
        /// <param name="degree"> Degree of the <see cref="BSpline"/> basis. </param>
        /// <param name="knotVector"> Knot vector of the <see cref="BSpline"/> basis. </param>
        /// <returns> 
        /// The values of the non-zero <see cref="BSpline"/> polynomials of the basis, i.e. those ranging from 
        /// N_{<paramref name="knotSpanIndex"/> - <paramref name="degree"/>, <paramref name="degree"/>} to N_{<paramref name="knotSpanIndex"/>, <paramref name="degree"/>}.
        /// </returns>
        public static double[] EvaluateBasisAt(double val, int knotSpanIndex, int degree, IList<double> knotVector)
        {
            // Value of the polynomials
            double[] N = new double[degree + 1];

            double[] left = new double[degree + 1];
            double[] right = new double[degree + 1];

            /********** Initialise the zeroth-degree B-Spline polynomials **********/

            N[0] = 1.0;


            /********** Compute triangular table **********/

            for (int j = 1; j < degree + 1; j++)
            {
                left[j] = val - knotVector[knotSpanIndex + 1 - j];
                right[j] = knotVector[knotSpanIndex + j] - val;

                double saved = 0.0;

                for (int r = 0; r < j; r++)
                {
                    double temp = N[r] / (right[r + 1] + left[j - r]);
                    N[r] = saved + right[r + 1] * temp;
                    saved = left[j - r] * temp;
                }

                N[j] = saved;
            }

            return N;
        }

        /// <summary>
        /// Evaluates the <see cref="BSpline"/> basis' derivatives of a given order, at a given value. 
        /// </summary>
        /// <remarks> 
        /// The code is adapted from algorithm 2.3 described in <see href="https://doi.org/10.1007/978-3-642-59223-2">the NURBS Book</see>, by L. Piegl and  W. Tiller.
        /// </remarks>
        /// <param name="val"> Value to evaluate at. </param>
        /// <param name="knotSpanIndex"> Index of knot span containing the value. </param>
        /// <param name="degree"> Initial degree of the <see cref="BSpline"/> basis. </param>
        /// <param name="knotVector"> Initial knot vector of the <see cref="BSpline"/> basis. </param>
        /// <param name="order"> Order of the derivatives. </param>
        /// <returns> 
        /// The values of successive derivatives of the non-zero <see cref="BSpline"/> polynomial of the basis, i.e. those ranging from 
        /// N_{<paramref name="knotSpanIndex"/> - <paramref name="degree"/>, <paramref name="degree"/>} to N_{<paramref name="knotSpanIndex"/>, <paramref name="degree"/>}. <br/>
        /// The first index corresponds to the order of derivation, the second to the index of the <see cref="BSpline"/> polynomial shifted to start from zero.
        /// </returns>
        public static double[][] EvaluateBasisDerivativesAt(double val, int knotSpanIndex, int degree, IList<double> knotVector, int order)
        {
            int n = order < degree ? order : degree;

            /********** Array storing the B-SPline polynomials and knot differences **********/

            double[][] ndu = new double[degree + 1][];
            for (int j = 0; j < degree + 1; j++) { ndu[j] = new double[degree + 1]; }

            double[] left = new double[degree + 1];
            double[] right = new double[degree + 1];

            double saved, temp;


            /********** Initialise the zeroth-degree B-Spline polynomials **********/

            ndu[0][0] = 1.0;

            for (int j = 1; j < degree + 1; j++)
            {
                left[j] = val - knotVector[knotSpanIndex + 1 - j];
                right[j] = knotVector[knotSpanIndex + j] - val;

                saved = 0.0;

                for (int r = 0; r < j; r++) /* Lower triangle */
                {
                    // Lower triangle (Knot difference)
                    ndu[j][r] = right[r + 1] + left[j - r];
                    temp = ndu[r][j - 1] / ndu[j][r];

                    // Upper triangle (B-Spline Polynomial N_{i + r,j})
                    ndu[r][j] = saved + right[r + 1] * temp;
                    saved = left[j - r] * temp;
                }

                ndu[j][j] = saved;
            }


            /********** Compute the derivatives **********/

            // Initialise result[k][j] = N^(k)_{knotSpanIndex - degree + j,degree}.
            double[][] result = new double[n + 1][];
            for (int k = 0; k < n + 1; k++)
            {
                result[k] = new double[degree + 1];
            }
            for (int j = 0; j < degree + 1; j++) // Load the zeroth derivatives
            {
                result[0][j] = ndu[j][degree];
            }

            // Array storing, in alternate fashion, the most recent rows of "a" (See equation 2.10)
            double[][] a = new double[2][];
            for (int j = 0; j < degree + 1; j++) { a[j] = new double[degree + 1]; }

            // Loop over the index of the B-Spline
            for (int r = 0; r < degree + 1; r++)
            {
                // Index of the last (s1) and penultimate (s2) row in "a".
                int s1 = 0, s2 = 1;

                a[0][0] = 1.0;

                // Loop over the order of the derivative
                for (int k = 1; k < n + 1; k++)
                {
                    double d = 0.0;
                    int rk = r - k;
                    int pk = degree - k;

                    if (r >= k)
                    {
                        a[s2][0] = a[s1][0] / ndu[pk + 1][rk];
                        d = a[s2][0] * ndu[rk][pk];
                    }

                    int j1 = rk >= -1 ? 1 : -rk;
                    int j2 = r - 1 <= pk ? k - 1 : degree - r;

                    for (int j = j1; j < j2 + 1; j++)
                    {
                        a[s2][j] = (a[s1][j] - a[s1][j - 1]) / ndu[pk + 1][rk + j];
                        d += a[s2][j] * ndu[rk + j][pk];
                    }

                    if (r <= pk)
                    {
                        a[s2][k] = -a[s1][k - 1] / ndu[pk + 1][r];
                        d += a[s2][k] * ndu[r][pk];
                    }

                    result[k][r] = d;

                    (s2, s1) = (s1, s2); // Switch rows
                }
            }

            /********** Multiply through by the correct factors (See equation 2.9) **********/

            int f = degree;
            for (int k = 1; k < n + 1; k++)
            {
                for (int j = 0; j < degree + 1; j++)
                {
                    result[k][j] *= f;
                }

                f *= degree - k;
            }

            return result;
        }


        /******************** On B-Spline Polynomial ********************/

        /// <summary>
        /// Evaluates the <see cref="BSpline"/> at a given value. 
        /// </summary>
        /// <remarks>
        /// The code is adapted from algorithm 2.4 described in <see href="https://doi.org/10.1007/978-3-642-59223-2">the NURBS Book</see>, by L. Piegl and  W. Tiller.
        /// </remarks>
        /// <param name="val"> Value to evaluate at. </param>
        /// <param name="index"> Index of the <see cref="BSpline"/> to evaluate. </param>
        /// <param name="degree"> Degree of the <see cref="BSpline"/> to evaluate. </param>
        /// <param name="knotVector"> Knot Vector associated with the <see cref="BSpline"/> to evaluate. </param>
        /// <returns> The value of the <see cref="BSpline"/> the given value. </returns>
        public static double EvaluateAt(double val, int index, int degree, IList<double> knotVector)
        {
            // Number of knot span
            int m = knotVector.Count - 1;

            // If the first B-spline polynomial is evaluated at the first knot,
            // or if the last B-spline polynomial is evaluated at the last knot.
            if (index == 0 && val == knotVector[0] || index == m - degree - 1 && val == knotVector[m]) { return 1d; }

            // If value is out of the non-zero domain of the B-Spline polynomial.
            if (val < knotVector[index] || knotVector[index + degree + 1] <= val) { return 0d; }


            /********** Initialise the zeroth-degree B-Spline polynomials **********/

            double[] N = new double[degree + 1];
            for (int j = 0; j < degree + 1; j++)
            {
                if (knotVector[index + j] <= val && val < knotVector[index + j + 1]) { N[j] = 1d; }
                else { N[j] = 0d; }
            }


            /********** Compute the triangular table **********/

            for (int k = 1; k < degree + 1; k++)
            {
                double saved;
                if (N[0] == 0d) { saved = 0d; }
                else { saved = (val - knotVector[index]) * N[0] / (knotVector[index + k] - knotVector[index]); }

                for (int j = 0; j < degree - k + 1; j++)
                {
                    double knotLeft = knotVector[index + j + 1];
                    double knotRight = knotVector[index + j + k + 1];

                    if (N[j + 1] == 0d)
                    {
                        N[j] = saved;
                        saved = 0d;
                    }
                    else
                    {
                        double temp = N[j + 1] / (knotRight - knotLeft);
                        N[j] = saved + (knotRight - val) * temp;
                        saved = (val - knotLeft) * temp;
                    }
                }
            }

            return N[0];
        }

        /// <summary>
        /// Evaluates the <see cref="BSpline"/> derivative of a given order; at a given value. 
        /// </summary>
        /// <remarks>
        /// The code is adapted from algorithm 2.5 described in <see href="https://doi.org/10.1007/978-3-642-59223-2">the NURBS Book</see>, by L. Piegl and  W. Tiller.
        /// </remarks>
        /// <param name="val"> Value to evaluate at. </param>
        /// <param name="index"> Index of the <see cref="BSpline"/> to evaluate. </param>
        /// <param name="degree"> Initial degree of the <see cref="BSpline"/> to evaluate. </param>
        /// <param name="knotVector"> Initial knot Vector associated with the <see cref="BSpline"/> to evaluate. </param>
        /// <param name="order"> Order of the derivative. </param>
        /// <returns> The value of the successive <see cref="BSpline"/> derivatives at the given value. The index corresponds to the order of derivation. </returns>
        public static double[] EvaluateDerivativesAt(double val, int index, int degree, IList<double> knotVector, int order)
        {
            int n = order < degree ? order : degree;

            double[] ders = new double[n + 1];

            // If value is out of the non-zero domain of the B-Spline polynomial.
            if (val < knotVector[index] || val >= knotVector[index + degree + 1])
            {
                for (int k = 0; k < n + 1; k++) { ders[k] = 0.0; }
                return ders;
            }


            /********** Initialise the zeroth-degree B-Spline polynomials **********/

            double[][] N = new double[degree + 1][];
            for (int j = 0; j < degree + 1; j++) { N[j] = new double[degree + 1]; }


            for (int j = 0; j < degree + 1; j++)
            {
                if (val >= knotVector[index + j] && val < knotVector[index + j + 1]) { N[j][0] = 1.0; }
                else { N[j][0] = 0.0; }
            }


            /********** Compute the triangular table **********/

            double saved, temp;
            double Uleft, Uright;

            for (int d = 1; d < degree + 1; d++)
            {
                if (N[0][d - 1] == 0.0) { saved = 0.0; }
                else { saved = (val - knotVector[index]) * N[0][d - 1] / (knotVector[index + d] - knotVector[index]); }

                for (int j = 0; j < degree + 1 - d; j++)
                {
                    Uleft = knotVector[index + j + 1];
                    Uright = knotVector[index + j + d + 1];

                    if (N[j + 1][d - 1] == 0.0) { N[j][d] = saved; saved = 0.0; }
                    else
                    {
                        temp = N[j + 1][d - 1] / (Uright - Uleft);
                        N[j][d] = saved + (Uright - val) * temp;
                        saved = (val - Uleft) * temp;
                    }
                }
            }

            /********** Compute the derivatives **********/

            ders[0] = N[0][degree];

            for (int k = 1; k < n + 1; k++)
            {
                double[] ND = new double[k + 1];

                // Load appropriate column 
                for (int j = 0; j < k + 1; j++) { ND[j] = N[j][degree - k]; }

                // Compute table of width k
                for (int jj = 1; jj < k + 1; jj++)
                {
                    if (ND[0] == 0.0) { saved = 0.0; }
                    else { saved = ND[0] / (knotVector[index + degree - k + jj] - knotVector[index]); }

                    for (int j = 0; j < k - jj + 1; j++)
                    {
                        Uleft = knotVector[index + j + 1];
                        Uright = knotVector[index + j + degree + jj + 1];

                        if (ND[j + 1] == 0.0) { ND[j] = (degree - k + jj) * saved; saved = 0.0; }
                        else
                        {
                            temp = ND[j + 1] / (Uright - Uleft);
                            ND[j] = (degree - k + jj) * (saved - temp);
                            saved = temp;
                        }
                    }
                }

                ders[k] = ND[0]; /* kth derivative */
            }

            return ders;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Computes the current <see cref="BSpline"/> at a given value.
        /// </summary>
        /// <param name="val"> Value to evaluate at. </param>
        /// <returns> The computed value of the current <see cref="BSpline"/>. </returns>
        public override double EvaluateAt(double val)
        {
            // Number of knot span
            int m = _knotVector.Count - 1;

            // Get the index of the knot span containing the value.
            int i_KnotSpan = FindKnotSpanIndex(val, Degree, _knotVector);

            // If the first B-spline polynomial is evaluated at the first knot,
            // or if the last B-spline polynomial is evaluated at the last knot.
            if (Index == 0 && val == _knotVector[0] || Index == m - Degree - 1 && val == _knotVector[m]) { return 1d; }

            // If value is out of the non-zero domain of the current B-Spline polynomial.
            if (val < _knotVector[Index] || val <= _knotVector[Index + Degree + 1]) { return 0d; }

            // The value is within the non-zero domain of the current B-Spline polynomial.
            return base.EvaluateAt(val);
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Sets the knot vector of the current <see cref="BSpline"/> polynomial while ensuring its validity.
        /// </summary>
        /// <param name="knotVector"> Knot vector to evaluate. </param>
        /// <param name="degree"> Degree of the B-Spline polynomial. </param>
        /// <param name="knotList"> Knot vector to set.</param>
        /// <exception cref="ArgumentException"> The number of knots provided is not valid. </exception>
        /// <exception cref="ArgumentException"> The knots should be provided in ascending order. </exception>
        protected static bool SetKnotVector(IList<double> knotVector, int degree, out List<double> knotList)
        {
            IEnumerator<double> knotEnumerator = knotVector.GetEnumerator();

            knotList = new List<double>();
            try
            {

                double previousKnot;
                if (knotEnumerator.MoveNext())
                {
                    previousKnot = knotEnumerator.Current;
                    knotList.Add(previousKnot);
                }
                else { throw new ArgumentException($"The number of knots provided is not valid.", nameof(knotVector)); }

                while (knotEnumerator.MoveNext())
                {
                    double knot = knotEnumerator.Current;

                    if (knot < previousKnot) { throw new ArgumentException("The knots should be provided in ascending order.", nameof(knotVector)); }

                    knotList.Add(knot);

                    previousKnot = knot;
                }

                if (knotList.Count < degree + 2)
                {
                    throw new ArgumentException($"The number of knots provided is not valid.", nameof(knotVector));
                }
            }
            finally { knotEnumerator.Dispose(); }

            return true;
        }

        /// <summary>
        /// Sets the coefficients of the current <see cref="BSpline"/> polynomial.
        /// </summary>
        /// <param name="spanIndex"> Index of knot span on which the current <see cref="BSpline"/> is defined.</param>
        /// <param name="index"> Index of the <see cref="BSpline"/> polynomial. </param>
        /// <param name="degree"> Degree of the <see cref="BSpline"/> polynomial. </param>
        /// <param name="knotVector"> Knot vector of the <see cref="BSpline"/> polynomial. </param>
        /// <returns> The coefficients of the <see cref="BSpline"/> polynomial. </returns>
        protected static double[] ComputeCoefficients(int spanIndex, int index, int degree, IList<double> knotVector)
        {
            // Number of knot span
            int m = knotVector.Count - 1;

            // Initialise the zeroth-degree B-Spline polynomials from N_{index,0} to N_{index+degree,0}
            Polynomial[][] N = new Polynomial[degree + 1][]; // Beware the indices are "inverted" in N.
            N[0] = new Polynomial[degree + 1];
            for (int j = 0; j < degree + 1; j++)
            {
                if (index + j == spanIndex) { N[0][j] = new Polynomial(1.0); }
                else { N[0][j] = new Polynomial(0.0); }
            }

            // Compute the triangular table.
            for (int d = 1; d < degree + 1; d++)
            {
                N[d] = new Polynomial[degree + 1 - d];

                for (int j = 0; j < degree + 1 - d; j++)
                {
                    // Compute N_{index + j, d}
                    Polynomial first;
                    double firstDenominator = knotVector[index + j + d] - knotVector[index + j];
                    if (firstDenominator == 0.0) { first = new Polynomial(0.0); }
                    else { first = new Polynomial(-knotVector[index + j], 1.0) / firstDenominator; }


                    Polynomial second;
                    double secondDenominator = knotVector[index + j + d + 1] - knotVector[index + j + 1];
                    if (secondDenominator == 0.0) { second = new Polynomial(0.0); }
                    else { second = new Polynomial(knotVector[index + j + d + 1], -1.0) / secondDenominator; }

                    N[d][j] = first * N[d - 1][j] + second * N[d - 1][j + 1];
                }
            }

            return N[degree][0]._coefficients;
        }

        #endregion
    }
}
