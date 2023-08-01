using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;

using Alg_Meas = BRIDGES.Algebra.Measure;


namespace BRIDGES.LinearAlgebra.Vectors
{
    /// <summary>
    /// Class defining a vector in a real vector space.
    /// </summary>
    public abstract class Vector :
        IEquatable<Vector>, IEnumerable<double>,
        IMultiplyOperators<Vector, double, Vector>, IDivisionOperators<Vector, double, Vector>,
        Alg_Meas.IDotProduct<Vector, double>
    {
        #region Properties

        /// <summary>
        /// Number of component of this vector.
        /// </summary>
        public abstract int Size { get; }

        /// <summary>
        /// Gets or sets the component at a given index.
        /// </summary>
        /// <param name="index"> Index of the component to get or set. </param>
        /// <returns> The value of the component. </returns>
        public abstract double this[int index] { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Vector"/> class.
        /// </summary>
        /// <param name="size"> Number of compoenent of the new <see cref="Vector"/>. </param>
        protected Vector(int size)
        {
            /* Do Nothing */
        }

        #endregion

        #region Static Methods

        /******************** Additive Abelian Group ********************/

        /// <inheritdoc cref="operator +(Vector, Vector)"/>
        public static Vector Add(Vector left, Vector right) => left + right;

        /// <inheritdoc cref="operator -(Vector, Vector)"/>
        public static Vector Subtract(Vector left, Vector right) => left - right;


        /// <inheritdoc cref="operator -(Vector)"/>
        public static Vector Opposite(Vector operand) => -operand;


        /******************** Group Action ********************/

        /// <inheritdoc cref="operator *(double, Vector)"/>
        public static Vector Multiply(double factor, Vector operand) => factor * operand;

        /// <inheritdoc cref="operator *(Vector, double)"/>
        public static Vector Multiply(Vector operand, double factor) => operand * factor;

        /// <inheritdoc cref="operator /(Vector, double)"/>
        public static Vector Divide(Vector operand, double divisor) => operand / divisor;


        /******************** Other Operations ********************/

        /// <summary>
        /// Computes the multiplication between the transposed left <see cref="Vector"/> and the right <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vector"/> to transpose, then multiply. </param>
        /// <param name="right"> Right <see cref="Vector"/> to multiply. </param>
        /// <returns> The scalar value resulting from the operation. </returns>
        /// <exception cref="ArgumentException"> This transpose-multiply operation of these two vector as <see cref="Vector"/> is not implemented. </exception>
        public static double TransposeMultiply(Vector left, Vector right)
        {
            if (left is DenseVector denseLeft) { return DenseVector.TransposeMultiply(denseLeft, right); }
            else if (left is SparseVector sparseLeft) { return SparseVector.TransposeMultiply(sparseLeft, right); }
            else { throw new NotImplementedException($"The multiplication between a transposed {left.GetType()} as {nameof(Vector)} and a {right.GetType()} as {nameof(Vector)} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }


        /******************** On Vector Sets ********************/

        /// <summary>
        /// Ortho-normalise the set of <see cref="Vector"/> using a Gram-Schimdt process. 
        /// </summary>
        /// <remarks> If the vectors are not linearly independent the number of vectors will change. </remarks>
        /// <param name="vectors"> Set of <see cref="Vector"/> to operate on. </param>
        /// <returns> The ortho-normal set of <see cref="Vector"/>. </returns>
        public static Vector[] GramSchmidt(IEnumerable<Vector> vectors)
        {
            if (vectors is IEnumerable<DenseVector> denseVectors) { return DenseVector.GramSchmidt(denseVectors); }
            else if (vectors is IEnumerable<SparseVector> sparseVectors) { return SparseVector.GramSchmidt(sparseVectors); }
            else { throw new NotImplementedException($"The Gram-Schmidt process on a set of {vectors.GetType()} as {nameof(Vector)} is not implemented."); }
        }

        #endregion

        #region Operators

        /******************** Additive Abelian Group ********************/

        /// <summary>
        /// Computes the addition of two <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vector"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Vector"/> for the addition. </param>
        /// <returns> The <see cref="Vector"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> The addition of left as a <see cref="Vector"/> with right as a <see cref="Vector"/> is not implemented. </exception>
        public static Vector operator +(Vector left, Vector right)
        {
            if (left is DenseVector denseLeft) { return denseLeft + right; }
            else if (left is SparseVector sparseLeft) { return sparseLeft + right; }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} as {typeof(Vector)} with a {right.GetType()} as {typeof(Vector)} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vector"/> to subtract. </param>
        /// <param name="right"> Right <see cref="Vector"/> to subtract with. </param>
        /// <returns> The <see cref="Vector"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> The subtraction of left as a <see cref="Vector"/> with right as a <see cref="Vector"/> is not implemented. </exception>
        public static Vector operator -(Vector left, Vector right)
        {
            if (left is DenseVector denseLeft) { return denseLeft - right; }
            else if (left is SparseVector sparseLeft) { return sparseLeft - right; }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} as {typeof(Vector)} with a {right.GetType()} as {typeof(Vector)} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }


        /// <summary>
        /// Computes the opposite of the <see cref="Vector"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Vector"/> whose opposite is to be computed. </param>
        /// <returns> The <see cref="Vector"/>, opposite of the initial one. </returns>
        /// <exception cref="NotImplementedException"> The unary negation of the operand as a <see cref="Vector"/> is not implemented. </exception>
        public static Vector operator -(Vector operand)
        {
            if (operand is DenseVector dense) { return -dense; }
            else if (operand is SparseVector sparse) { return -sparse; }
            else { throw new NotImplementedException($"The unary negation of a {operand.GetType()} as {typeof(Vector)} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the right scalar multiplication of a <see cref="Vector"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Vector"/> to multiply on the right. </param>
        /// <param name="factor"> <see cref="double"/> number to multiply with. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the right scalar multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right scalar multiplication of the operand as a <see cref="Vector"/> with a <see cref="double"/> is not implemented. </exception>
        public static Vector operator *(Vector operand, double factor)
        {
            if (operand is DenseVector dense) { return dense * factor; }
            else if (operand is SparseVector sparse) { return sparse * factor; }
            else { throw new NotImplementedException($"The right scalar multiplication of a {operand.GetType()} as {typeof(Vector)} with a {factor.GetType()} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }

        /// <summary>
        /// Computes the left scalar multiplication of a <see cref="Vector"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Vector"/> to multiply on the left. </param>
        /// <param name="factor"> <see cref="double"/> number to multiply with. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the left scalar multiplication. </returns>
        /// <exception cref="NotImplementedException"> The left scalar multiplication of the operand as a <see cref="Vector"/> with a <see cref="double"/> is not implemented. </exception>
        public static Vector operator *(double factor, Vector operand)
        {
            if (operand is DenseVector dense) { return factor * dense; }
            else if (operand is SparseVector sparse) { return factor * sparse; }
            else { throw new NotImplementedException($"The left scalar multiplication of a {operand.GetType()} with a {factor.GetType()} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }


        /// <summary>
        /// Computes the scalar division of a <see cref="Vector"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Vector"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/> number to divide with. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the scalar division. </returns>
        /// <exception cref="NotImplementedException"> The scalar division of the operand as a <see cref="Vector"/> with a <see cref="double"/> is not implemented. </exception>
        public static Vector operator /(Vector operand, double divisor)
        {
            if (operand is DenseVector dense) { return dense / divisor; }
            else if (operand is SparseVector sparse) { return sparse / divisor; }
            else { throw new NotImplementedException($"The scalar division of a {operand.GetType()} with a {divisor.GetType()} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }


        /******************** Equality Operators ********************/

        /// <summary>
        /// Evaluates whether two <see cref="Vector"/> are equal or not.
        /// </summary>
        /// <param name="left"> Left <see cref="Vector"/> for the comparison. </param>
        /// <param name="right"> Right <see cref="Vector"/> for the comparison. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Vector"/> are equal, <see langword="false"/> otherwise. </returns>
        /// <exception cref="NotImplementedException"> The equality comparison of left as a <see cref="Vector"/> with right as a <see cref="Vector"/> is not implemented. </exception>
        public static bool operator ==(Vector left, Vector right)
        {
            if (left is DenseVector denseLeft) { return denseLeft == right; }
            else if (left is SparseVector sparseLeft) { return sparseLeft == right; }
            else { throw new NotImplementedException($"The equality comparison of a {left?.GetType()} with a {right?.GetType()} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }

        /// <summary>
        /// Evaluates whether two <see cref="Vector"/> are different.
        /// </summary>
        /// <param name="left"> Left <see cref="Vector"/> for the comparison. </param>
        /// <param name="right"> Right <see cref="Vector"/> for the comparison. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Vector"/> are different, <see langword="false"/> otherwise. </returns>
        /// <exception cref="NotImplementedException"> The inequality comparison of left as a <see cref="Vector"/> with right as a <see cref="Vector"/> is not implemented. </exception>
        public static bool operator !=(Vector left, Vector right)
        {
            if (left is DenseVector denseLeft) { return denseLeft != right; }
            else if (left is SparseVector sparseLeft) { return sparseLeft != right; }
            else { throw new NotImplementedException($"The inequality comparison of a {left?.GetType()} with a {right?.GetType()} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Computes the length of this vector.
        /// </summary>
        /// <returns> The value of the vector length. </returns>
        public virtual double Norm()
        {
            int size = Size;
            double[] components = ToArray();

            double result = 0d;
            for (int i = 0; i < size; i++)
            {
                result += components[i] * components[i];
            }

            return Math.Sqrt(result);
        }

        /// <summary>
        /// Computes the squared norm of this vector.
        /// </summary>
        /// <returns> The value of the vector squared norm. </returns>
        public virtual double SquaredNorm()
        {
            int size = Size;
            double result = 0d;

            double[] components = ToArray();
            for (int i = 0; i < size; i++)
            {
                result += components[i] * components[i];
            }

            return result;
        }


        /// <summary>
        /// Translates this vector into its array representation.
        /// </summary>
        /// <returns> The array representing the vector. </returns>
        public abstract double[] ToArray();

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator()"/>
        public abstract IEnumerator<double> GetEnumerator();


        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public virtual bool Equals(Vector other)
        {
            if (Size != other?.Size) { return false; }

            int size = Size;
            double[] components = this.ToArray();
            double[] otherComponents = other.ToArray();
            for (int i = 0; i < size; i++)
            {
                if (Math.Abs(components[i] - otherComponents[i]) < Settings.AbsolutePrecision) { return false; }
            }

            return true;
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Vector vector && Equals(vector);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"Vector (S:{Size})";
        }

        #endregion

        #region Explicit Implementations

        /******************** IDotProduct<Vector, double>, ********************/

        /// <inheritdoc cref="Alg_Meas.IMetric{TSelf}.DistanceTo(TSelf)"/>
        double Alg_Meas.IMetric<Vector>.DistanceTo(Vector other) => (this - other).Norm();

        /// <inheritdoc cref="Alg_Meas.IDotProduct{TSelf,TValue}.DotProduct(TSelf)"/>
        double Alg_Meas.IDotProduct<Vector,double>.DotProduct(Vector other) => TransposeMultiply(this, other);

        /******************** IEnumerable ********************/

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
